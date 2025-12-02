import { Injectable, signal } from '@angular/core';
import { LayoutNavegacaoModel } from '../model/layout-navegacao.model';

@Injectable({
  providedIn: 'root',
})
export class LayoutProcessoDataService {
  //#region signals
  private _dadosMenuLateral = signal<any>(null);
  dadosMenuLateral$ = this._dadosMenuLateral.asReadonly();
  setDadosMenuLateral(dados: any) {
    this._dadosMenuLateral.set(dados);
  }

  private _botaoProximoAcionado = signal(false);
  botaoProximoAcionado$ = this._botaoProximoAcionado.asReadonly();
  setBotaoProximoAcionado(dados: boolean) {
    this._botaoProximoAcionado.set(dados);
  }

  private _desativarTodosBotoes = signal(false);
  desativarTodosBotoes$ = this._desativarTodosBotoes.asReadonly();
  setDesativarTodosBotoes(dados: boolean) {
    this._desativarTodosBotoes.set(dados);
  }

  private _desativarBotaoProximo = signal(true);
  desativarBotaoProximo$ = this._desativarBotaoProximo.asReadonly();
  setDesativarBotaoProximo(dados: boolean) {
    this._desativarBotaoProximo.set(dados);
  }

  private _dadosNavegacao = signal<LayoutNavegacaoModel>(
    {} as LayoutNavegacaoModel,
  );
  dadosNavegacao$ = this._dadosNavegacao.asReadonly();
  setDadosNavegacao(dados: LayoutNavegacaoModel) {
    this._dadosNavegacao.set(dados);
  }

  private _loadBotaoProximo = signal(false);
  loadBotaoProximo$ = this._loadBotaoProximo.asReadonly();
  setLoadBotaoProximo(dados: boolean) {
    this._loadBotaoProximo.set(dados);
  }
  private _exibirComponenteMenuLateral = signal<boolean>(true);
  $exibirComponenteMenuLateral = this._exibirComponenteMenuLateral.asReadonly();
  setExbirComponenteMenuLateral(data: boolean) {
    this._exibirComponenteMenuLateral.set(data);
  }

  //#endregion

  constructor() {}
}
