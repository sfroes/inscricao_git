import { DOCUMENT } from '@angular/common';
import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { MessageService } from 'primeng/api';
import { catchError, map, throwError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const messageService = inject(MessageService);
  const document = inject(DOCUMENT);

  return next(req).pipe(
    map((data) => {
      let urlRedirect = sessionStorage.getItem('urlRedirect');

      if (urlRedirect) {
        sessionStorage.removeItem('urlRedirect');
        document.location.href = urlRedirect;
      }

      return data;
    }),

    catchError((error: HttpErrorResponse) => {
      let errorMessage = 'Ocorreu um erro inesperado.';

      if (error.error instanceof ErrorEvent) {
        errorMessage = `Erro: ${error.error.message}`;
      } else {
        const origem = window.location.origin;
        const isDev = origem.includes('dev');

        if (error.url?.includes('SAS.Login4')) {
          let href = document.location.href;

          sessionStorage.setItem('urlRedirect', href);
          const token = window.location.href.split('?')[0].split('/').pop();

          const baseReturnUrl = isDev
            ? `/Dev/SAS.Login4/Usuario/Autenticar/?ReturnUrl=`
            : `/SAS.Login4/Usuario/Autenticar/?ReturnUrl=`;

          // Pegando a URL da página atual como referência para o retorno dinâmico
          //const appUrl = window.location.pathname + window.location.search;

          const appUrl = isDev
            ? '/Dev/GPI.Inscricao/Home/Angular'
            : '/GPI.Inscricao/Home/Angular';
          const wsignin1 = isDev
            ? '/Dev/SAS.Login4/?wa=wsignin1.0&wtrealm'
            : '/SAS.Login4/?wa=wsignin1.0&wtrealm';

          const returnUrl = `${wsignin1}=${encodeURIComponent(
            origem + appUrl,
          )}&wctx=rm%3d0%26id%3dpassive%26ru%3d${encodeURIComponent(
            encodeURIComponent(appUrl),
          )}&wct=${encodeURIComponent(new Date().toISOString())}&smc=79347510F9A9F0A67C659E0076C35D3FA18D104EE74204985A973F0B9D306A1C950E99DC4F7813218C84AA066A9329932832EEF604D974564619E93A7D6BD552`;

          document.location.href =
            origem + baseReturnUrl + encodeURIComponent(returnUrl);
        }

        const errorMessages: Record<number, string> = {
          302: error.error.message || 'Redirecionamento detectado.',
        };

        if (errorMessages[error.status]) {
          errorMessage = errorMessages[error.status];
          if (error.status === 302 && error.error.urlRetorno) {
            document.location.href = error.error.urlRetorno;
          }
        } else {
          errorMessage = `Erro inesperado: ${error.message}`;
        }
      }

      messageService.add({
        severity: 'error',
        summary: 'Erro',
        detail: errorMessage,
      });

      return throwError(() => new Error(errorMessage));
    }),
  );
};
