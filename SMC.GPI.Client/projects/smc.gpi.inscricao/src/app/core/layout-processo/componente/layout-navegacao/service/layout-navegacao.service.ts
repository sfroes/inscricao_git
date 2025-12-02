import { HttpClient } from '@angular/common/http';
import { effect, inject, Injectable, signal } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from '../../../../environments/environment';
import { ToolsService } from '../../../../tools/tools.service';
import { LayoutNavegacaoModel } from '../../../model/layout-navegacao.model';
import { LayoutProcessoDataService } from '../../../service/layout-processo-data.service';
import { SelecaoOfertaDataService } from './../../../../../areas/ins/selecao-oferta/service/selecao-oferta-data.service';

@Injectable({
  providedIn: 'root',
})
export class LayoutNavegacaoService {
  //#region proriedades
  dadosModelo: LayoutNavegacaoModel = {} as LayoutNavegacaoModel;

  //#endregion

  //#region signals
  private _isLoading = signal(true);
  isLoading$ = this._isLoading.asReadonly();

  private _exibirBotaoAnterior = signal(false);
  exibirBotaoAnterior$ = this._exibirBotaoAnterior.asReadonly();

  private _loadBotaoProximo = signal(false);
  loadBotaoProximo$ = this._loadBotaoProximo.asReadonly();

  private _loadBotaoCancelar = signal(false);
  loadBotaoCancelar$ = this._loadBotaoCancelar.asReadonly();

  private _loadBotaoAnterior = signal(false);
  loadBotaoAnterior$ = this._loadBotaoAnterior.asReadonly();

  private _desabilitarBotaoAnterior = signal(false);
  desabilitarBotaoAnterior$ = this._desabilitarBotaoAnterior.asReadonly();

  private _desabilitarBotaoProximo = signal(true);
  desabilitarBotaoProximo$ = this._desabilitarBotaoProximo.asReadonly();

  private _desabilitarBotaoCancelar = signal(false);
  desabilitarBotaoCancelar$ = this._desabilitarBotaoCancelar.asReadonly();

  //#endregion

  //#region injeção de dependencia
  router = inject(Router);
  toolService = inject(ToolsService);
  http = inject(HttpClient);
  //selecaoOfertaVazioService = inject(SelecaoOfertaVazioService);
  selecaoOfertaDataService = inject(SelecaoOfertaDataService);
  layoutProcessoDataService = inject(LayoutProcessoDataService);
  //#endregion

  constructor() {
    effect(() => {
      const desativarTodosBotoes =
        this.layoutProcessoDataService.desativarTodosBotoes$();
      this.desativarTodosBotoes(desativarTodosBotoes);
    });

    effect(() => {
      const desabilitarBotaoProximo =
        this.layoutProcessoDataService.desativarBotaoProximo$();
      this._desabilitarBotaoProximo.set(desabilitarBotaoProximo);
    });

    effect(() => {
      const dadosNavegacao = this.layoutProcessoDataService.dadosNavegacao$();
      this.dadosModelo = dadosNavegacao;
      this._isLoading.set(false);
      if (
        dadosNavegacao.tokenPaginaAnteriorEncrypted != null &&
        dadosNavegacao.tokenPaginaAnteriorEncrypted != undefined &&
        dadosNavegacao.tokenPaginaAnteriorEncrypted.length > 0
      ) {
        this._exibirBotaoAnterior.set(true);
      }
    });

    effect(() => {
      const loadBotaoProximo =
        this.layoutProcessoDataService.loadBotaoProximo$();
      this._loadBotaoProximo.set(loadBotaoProximo);
    });
  }

  botaoCancelar() {
    this._loadBotaoCancelar.set(true);
    this._desabilitarBotaoAnterior.set(true);
    this._desabilitarBotaoProximo.set(true);
    //this.selecaoOfertaVazioService.desabilitarBotaoSelecionar$.next(true); // behavior
    this.selecaoOfertaDataService.setDesativarBotaoSelecionaModalHierarquia(
      true,
    );
    if (this.dadosModelo.seqInscricao === 0) {
      if (
        this.dadosModelo.uidProcesso != null &&
        this.toolService.isGuidValido(this.dadosModelo.uidProcesso)
      ) {
        const url =
          environment.base_href +
          `../Home/IndexProcesso?Guid=${this.dadosModelo.uidProcesso}`;
        window.location.href = url;
      } else {
        const url = environment.base_href + `../Home/Indexhome`;
        window.location.href = url;
      }
    } else {
      const url = environment.base_href + '../Home/Index/';
      window.location.href = url;
    }
  }

  botaoProximo(data: boolean) {
    this.layoutProcessoDataService.setBotaoProximoAcionado(data);
  }

  botaoAnterior() {
    const data = {
      token: this.dadosModelo.tokenPaginaAnteriorEncrypted,
      seqConfiguracaoEtapaPagina:
        this.dadosModelo.seqConfiguracaoEtapaPaginaAnteriorEncrypted,
      seqConfiguracaoEtapa: this.dadosModelo.seqConfiguracaoEtapaEncrypted,
      seqGrupoOferta: this.dadosModelo.seqGrupoOfertaEncrypted,
      seqInscricao: this.dadosModelo.seqInscricaoEncrypted,
      idioma: this.dadosModelo.idioma,
    };
    const url = `${environment.base_href}../INS/Inscricao/UrlPagina`;

    this.montarEnviarUrlAnterior(data, url);
  }

  desativarTodosBotoes(ind: boolean) {
    this._desabilitarBotaoAnterior.set(ind);
    this._desabilitarBotaoCancelar.set(ind);
    this._desabilitarBotaoProximo.set(ind);
  }

  private montarEnviarUrlAnterior(data: any, url: string) {
    this._loadBotaoAnterior.set(true);
    this._desabilitarBotaoCancelar.set(true);
    this._desabilitarBotaoProximo.set(true);
    //this.selecaoOfertaVazioService.desabilitarBotaoSelecionar$.next(true); // behavior
    this.selecaoOfertaDataService.setDesativarBotaoSelecionaModalHierarquia(
      true,
    );
    const form = document.createElement('form');
    form.method = 'POST';
    form.action = url;
    Object.keys(data).forEach((key) => {
      const input = document.createElement('input');
      input.type = 'hidden';
      input.name = key;
      input.value = (data as any)[key];
      form.appendChild(input);
    });

    // Submete o formulário
    document.body.appendChild(form);
    form.submit();
  }
}
