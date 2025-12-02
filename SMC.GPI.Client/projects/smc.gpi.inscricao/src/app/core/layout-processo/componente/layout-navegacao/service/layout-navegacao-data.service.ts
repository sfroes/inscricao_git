import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class LayoutNavegacaoDataService {
  //Dados necessarios para montar os menus de navegação
  // dadosNavegacao$ = new BehaviorSubject<LayoutNavegacaoModel>(
  //   {} as LayoutNavegacaoModel,
  // );
  //loadBotaoProximo$ = new BehaviorSubject<boolean>(false);

  //botaoProximoAcionado$ = new Subject<void>();
  // desativarTodosBotoes$ = new BehaviorSubject<boolean>(false);
  //desativarBotaoProximo$ = new BehaviorSubject<boolean>(true);

  private _errosBotaoProximo = signal<string[]>([]);
  $errosBotaoProximo = this._errosBotaoProximo.asReadonly();

  setErrosBotaoProximo(data: string[]) {
    this._errosBotaoProximo.set(data);
  }
}
