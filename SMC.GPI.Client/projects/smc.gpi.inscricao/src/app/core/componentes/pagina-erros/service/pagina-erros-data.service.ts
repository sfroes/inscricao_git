import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class PaginaErrosDataService {
  private _mensagemErro = signal<string>('');
  $mensagemErro = this._mensagemErro.asReadonly();
  setMensagemErro(data: string) {
    this._mensagemErro.set(data);
  }

  private _urlRetorno = signal<string>('');
  $urlRetorno = this._urlRetorno.asReadonly();
  setUrlRetorno(data: string) {
    this._urlRetorno.set(data);
  }

  private _acionarBotaoSair = signal<boolean>(false);
  $acionarBotaoSair = this._acionarBotaoSair.asReadonly();
  setAcionarBotaoSair(data: boolean) {
    this._acionarBotaoSair.set(data);
  }
}
