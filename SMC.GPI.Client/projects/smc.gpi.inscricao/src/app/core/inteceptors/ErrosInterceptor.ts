import { DOCUMENT } from '@angular/common';
import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { catchError, EMPTY, map } from 'rxjs';
import { LayoutMenuLateralDataService } from '../layout-processo/componente/layout-menu-lateral/service/layout-menu-lateral-data.service';
import { LayoutNavegacaoService } from '../layout-processo/componente/layout-navegacao/service/layout-navegacao.service';
import { LayoutProcessoDataService } from '../layout-processo/service/layout-processo-data.service';
import { PaginaErrosDataService } from './../componentes/pagina-erros/service/pagina-erros-data.service';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const messageService = inject(MessageService);
  const document = inject(DOCUMENT);
  const layoutNavegacaoService = inject(LayoutNavegacaoService);
  const layoutMenuLateralDataService = inject(LayoutMenuLateralDataService);
  const paginaErrosDataService = inject(PaginaErrosDataService);
  const layoutProcessoDataService = inject(LayoutProcessoDataService);
  const router = inject(Router);
  const route = inject(ActivatedRoute);

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
        const isDev = origem.includes('localhost');

        if (error.url?.includes('SAS.Login4')) {
          let href = document.location.href;

          sessionStorage.setItem('urlRedirect', href);
          const baseReturnUrl = isDev
            ? `/Dev/GPI.Inscricao/Home/AngularErro`
            : `/GPI.Inscricao/Home/AngularErro`;
          document.location.href = origem + baseReturnUrl;
        }

        const errorMessages: Record<number, string> = {
          302: error.error.message || 'Redirecionamento detectado.',
        };

        if (errorMessages[error.status]) {
          errorMessage = errorMessages[error.status];
          layoutNavegacaoService.desativarTodosBotoes(true);
          if (error.status === 302 && error.error.urlRetorno) {
            let queryParams: any = {};
            const carregarRota = route.queryParams.subscribe((params) => {
              queryParams = params;
              sessionStorage.setItem('erroMessage', errorMessage);
              sessionStorage.setItem('urlRetorno', error.error.urlRetorno);
              router.navigate(['/pagina-erros'], {
                queryParams: { seqInscricao: queryParams['seqInscricao'] },
              });
            });
          }
        } else {
          errorMessage = `Erro inesperado: ${error.message}`;
        }
      }

      if (error.status !== 302) {
        messageService.add({
          severity: 'error',
          summary: 'Erro',
          detail: errorMessage,
          sticky: true,
        });
      }

      return EMPTY; // Erro tratado, não propaga.
    }),
  );
};
