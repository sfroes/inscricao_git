import { HttpClient, HttpHeaders } from '@angular/common/http';
import { effect, inject, Injectable, signal } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { map } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { LayoutProcessoDataService } from '../../../service/layout-processo-data.service';

@Injectable({
  providedIn: 'root',
})
export class LayoutMenuLateralService {
  //#region signals
  private _conteudo = signal<any>(null);
  conteudo$ = this._conteudo.asReadonly();

  private _isLoading = signal(true);
  isLoading$ = this._isLoading.asReadonly();
  //#endregion

  //#region injeção de dependencia
  sanitizer = inject(DomSanitizer);
  layoutProcessoDataService = inject(LayoutProcessoDataService);
  http = inject(HttpClient);
  //#endregion

  constructor() {
    effect(() => {
      let dados = this.layoutProcessoDataService.dadosMenuLateral$();
      const caminho = 'INS/Front/PartialMenuLateral';

      if (!dados) {
        return;
      }

      //Limpo os campos taxas pois ele não e util para o menu lateral e com isso estoura o maxlength do json
      dados = { ...this.layoutProcessoDataService.dadosMenuLateral$() };
      dados.taxas = [];

      const httpOptions = {
        headers: new HttpHeaders({
          'Content-Type': 'application/json',
        }),
        responseType: 'text' as 'json', // Define que a resposta será do tipo texto
      };

      const url = `${environment.frontUrl}${caminho}`;
      this.http
        .post(url, JSON.stringify(dados), httpOptions) // Passando o `modelo` no corpo da requisição
        .pipe(map((res) => res as string))
        .subscribe((menu) => {
          this._conteudo.set(this.sanitizer.bypassSecurityTrustHtml(menu));
          this._isLoading.set(false);
          setTimeout(() => {
            this.executeScripts();
          }, 0);
        });
    });
  }

  /**
   * Executa os scripts do menu lateral.
   * Utiliza o id do elemento 'menuLateral' para encontrar o conteúdo do menu lateral.
   * Caso o elemento seja encontrado, os scripts são executados e, em seguida, removidos do DOM.
   * @private
   */
  executeScripts() {
    const conteudoDiv = document.getElementById('menuLateral');
    if (conteudoDiv) {
      const scripts = conteudoDiv.getElementsByTagName('script');
      for (let script of Array.from(scripts)) {
        const newScript = document.createElement('script');
        newScript.textContent = script.textContent;
        document.body.appendChild(newScript);
        document.body.removeChild(newScript); // Remove o script após execução
      }
    }
  }
}
