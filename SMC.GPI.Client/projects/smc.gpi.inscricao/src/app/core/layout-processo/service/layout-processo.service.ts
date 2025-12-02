import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { map, Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class LayoutProcessoService {
  //#region propriedades
  private _descricaoProcesso = signal('');
  descricaoProcesso$ = this._descricaoProcesso.asReadonly();
  //#endregion

  //#region Injeção de dependência
  http = inject(HttpClient);
  route = inject(ActivatedRoute);
  //#endregion

  /**
   * Carrega o conteúdo do header dinamicamente.
   * Faz uma requisição GET para a URL informada, com o tipo de resposta como texto.
   * O conteúdo retornado é mapeado para uma string e entregue como observable.
   *
   * @param caminho Caminho da URL do serviço que retorna o conteúdo HTML do header
   * @returns Observable<string> Conteúdo do header como string
   */
  carregarHeader(caminho: string): Observable<string> {
    const url = environment.frontUrl + caminho;
    return this.http
      .get(url, { responseType: 'text' })
      .pipe(map((response) => response as string));
  }

  /**
   * Carrega o conteúdo do rodapé dinamicamente.
   * Faz uma requisição GET para a URL informada, com o tipo de resposta como texto.
   * O conteúdo retornado é mapeado para uma string e entregue como observable.
   *
   * @param caminho Caminho da URL do serviço que retorna o conteúdo HTML do rodapé
   * @returns Observable<string> Conteúdo do rodapé como string
   */

  /**
   * Realiza uma requisição GET para buscar a descrição do processo de inscrição.
   * Constrói a URL utilizando o caminho fornecido e o identificador de inscrição.
   *
   * @param caminho Caminho da URL do serviço que retorna a descrição do processo de inscrição
   * @param seqInscricao Identificador da inscrição a ser buscada
   * @returns Observable<any> Resposta da requisição HTTP
   */

  buscarDescricaoProcessoInscricao(seqInscricao: string) {
    const caminho = 'Ins/Front/BuscarDescricaoProcesso';
    const url = `${environment.frontUrl}${caminho}?seqInscricao=${seqInscricao}`;
    return this.http.get(url).subscribe((res) => {
      this._descricaoProcesso.set(res as string);
    });
  }

  validarUrlCss() {
    let ulrCSS = document.getElementById('cssProcesso');
    const origem = window.location.origin;
    const isDev = origem.includes('localhost');
    let queryParams: any = {};
    this.route.queryParams.subscribe((params) => {
      queryParams = params;
      if (ulrCSS?.getAttribute('href') == '@urlCSS') {
        let caminho = `/GPI.Inscricao/Inscricao/BuscarUrlCss?seqInscricao=${queryParams['seqInscricao']}`;

        if (isDev) {
          caminho = `/Dev/GPI.Inscricao/Inscricao/BuscarUrlCss?seqInscricao=${queryParams['seqInscricao']}`;
        }

        const url = origem + caminho;

        this.http.get(url).subscribe((res) => {
          const novaRef =
            origem + '/Recursos/Inscricoes/4.0/GPI.Inscricao/' + res;
          ulrCSS.setAttribute('href', novaRef);
        });
      }
    });
  }
}
