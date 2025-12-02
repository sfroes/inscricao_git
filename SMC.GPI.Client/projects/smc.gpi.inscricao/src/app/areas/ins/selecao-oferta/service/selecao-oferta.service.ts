import { BreakpointObserver } from '@angular/cdk/layout';
import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { MessageService } from 'primeng/api';
import { firstValueFrom, lastValueFrom, Subscription } from 'rxjs';
import { environment } from '../../../../core/environments/environment';
import { LayoutNavegacaoDataService } from '../../../../core/layout-processo/componente/layout-navegacao/service/layout-navegacao-data.service';
import { LayoutNavegacaoModel } from '../../../../core/layout-processo/model/layout-navegacao.model';
import { LayoutProcessoDataService } from '../../../../core/layout-processo/service/layout-processo-data.service';
import { GPILookup } from '../../../../core/models/GpiLookup';
import { ToolsService } from '../../../../core/tools/tools.service';
import { DatasourceItemModel } from '../model/datasource-item.model';
import { TipoCobranca } from '../model/enums/tipo-cobranca.enum';
import { HierarquiaModel } from '../model/herarquia.model';
import { ListaOfertaModel } from '../model/lista-oferta.model';
import {
  Ofertas,
  SelecaoOfertaModel,
  SelecaoOfertasSalvar,
  Taxa,
} from '../model/selecao-oferta.model';
import { FiltroPaginaModel } from './../../../../core/layout-processo/model/filtro-pagina.model';
import { SelecaoOfertaDataService } from './selecao-oferta-data.service';

@Injectable({
  providedIn: 'root',
})
export class SelecaoOfertaService {
  //#region injeção de dependências
  http = inject(HttpClient);
  selecaoOfertaDataService = inject(SelecaoOfertaDataService);
  route = inject(ActivatedRoute);
  layoutProcessoDataService = inject(LayoutProcessoDataService);
  sanitizer = inject(DomSanitizer);
  toolsService = inject(ToolsService);
  dadosNavegacaoService = inject(LayoutNavegacaoDataService);
  detectaDispositivo = inject(BreakpointObserver);
  layoutDataService = inject(LayoutNavegacaoDataService);
  messageService = inject(MessageService);

  //#endregion

  //#region signals


  private _abrirModalTaxas = signal<boolean>(false);
  $abrirModalTaxas = this._abrirModalTaxas.asReadonly();

  setModalTaxas(data: boolean) {
    this._abrirModalTaxas.set(data);
  }

  get abrirModalTaxas() {
    return this.$abrirModalTaxas();
  }
  set abrirModalTaxas(val: boolean) {
    this.setModalTaxas(val);
  }

  get abrirModal() {
    return this.selecaoOfertaDataService.$abrirModalSelecaoOferta();
  }
  set abrirModal(val: boolean) {
    this.selecaoOfertaDataService.setAbrirModalSelecaoOferta(val);
  }

  private _tituloModalSelecaoOferta = signal<string>('');
  $tituloModalSelecaoOferta = this._tituloModalSelecaoOferta.asReadonly();

  setTituloModalSelecaoOfertra(data: string) {
    this._tituloModalSelecaoOferta.set(data);
  }

  private _filtro = signal<FiltroPaginaModel>({} as FiltroPaginaModel);
  $filtro = this._filtro.asReadonly();
  setFiltro(data: FiltroPaginaModel) {
    this._filtro.set(data);
  }
  private _selecaoOfertaInstrucoes = signal<any>(null);
  $selecaoOfertaInstrucoes = this._selecaoOfertaInstrucoes.asReadonly();
  setSelecaoOfertaInstrucoes(data: any) {
    this._selecaoOfertaInstrucoes.set(data);
  }

  private _existeBotaoTaxas = signal<Boolean>(false);
  $existeBotaoTaxas = this._existeBotaoTaxas.asReadonly();
  setExisteBotaoTaxas(data: boolean) {
    this._existeBotaoTaxas.set(data);
  }

  private _isLoading = signal(true);
  $isLoading = this._isLoading.asReadonly();
  setIsLoading(data: boolean) {
    this._isLoading.set(data);
  }

  private _titulo = signal('');
  $titulo = this._titulo.asReadonly();
  setTitulo(data: string) {
    this._titulo.set(data);
  }

  private _alertaOferta = signal('');
  $alertaOferta = this._alertaOferta.asReadonly();
  setAlertaOferta(data: string) {
    this._alertaOferta.set(data);
  }

  private _numeroMinimoGrupoNaoAtingido = signal<boolean | null>(null);

  $numeroMinimoGrupoNaoAtingido =
    this._numeroMinimoGrupoNaoAtingido.asReadonly();

  setNumeroMinimoGrupoNaoAtingido(data: boolean) {
    this._numeroMinimoGrupoNaoAtingido.set(data);
  }

  descricaoGrupoTaxasNaoAtingidos: Set<string> = new Set();

  //#endregion

  //#region propriedades
  private _numeroNoFolha = signal<number>(0);
  $numeroNoFolha = this._numeroNoFolha.asReadonly();

  //#endregion

  subs: Subscription = new Subscription();

  constructor() {}

  getBuscarSelecaoOferta(path: string, filtro: FiltroPaginaModel) {
    const parametros = new HttpParams({ fromObject: filtro as any });
    const url = `${environment.frontUrl}${path}?${parametros.toString()}`;
    return this.http.get<SelecaoOfertaModel>(url);
  }

  buscarHierarquia(path: string, filtro: FiltroPaginaModel) {
    const url = `${environment.frontUrl}${path}?seqGrupoOferta=${filtro.seqGrupoOferta}`;
    return this.http.get<HierarquiaModel[]>(url);
  }
  listarTaxasOfertaInscricaoAngular(
    path: string,
    seqOferta: number,
    seqInscricao: number,
  ) {
    const params = new HttpParams()
      .set('seqOferta', seqOferta)
      .set('seqInscricao', seqInscricao);
    const url = `${environment.frontUrl}${path}`;
    return lastValueFrom(this.http.get<Taxa[]>(url, { params }));
  }
  async buscarHierarquiaCompletaOferta(
    path: string,
    seqOferta: number,
  ): Promise<HierarquiaModel[]> {
    const url = `${environment.frontUrl}${path}?seqOferta=${seqOferta}`;
    return await firstValueFrom(this.http.get<HierarquiaModel[]>(url));
  }

  async buscarDescricaoSelecaoOfertasInscricaoSeqsOfertas(
    path: string,
    seqsOfertas: number[],
  ): Promise<DatasourceItemModel[]> {
    this.selecaoOfertaDataService.setCarregarListaOferta(true);
    const params = new HttpParams().set('seqsOfertas', seqsOfertas.join(','));
    const url = `${environment.frontUrl}${path}`;
    return await lastValueFrom(
      this.http.get<DatasourceItemModel[]>(url, { params }),
    );
  }

  async listarOfertas() {
    const listaOferta: ListaOfertaModel[] = [];
    const ofertasSelecionadas = await this.buscarDesricaoCompleta();

    let hierarquia: HierarquiaModel | null;

    for (const oferta of this.selecaoOfertaDataService.$selecaoOferta()
      .ofertas) {
      if (typeof oferta.seqOferta.seq === 'number') {
        hierarquia = {
          ...this.selecaoOfertaDataService
            .$hierarquia()
            .find((f) => f.seq === oferta.seqOferta.seq)!,
        };
      } else {
        hierarquia = {
          ...this.selecaoOfertaDataService
            .$hierarquia()
            .find((f) => f.seq === (oferta.seqOferta as any).seq)!,
        };
      }

      if (!hierarquia) {
        const arq = await this.buscarHierarquiaCompletaOferta(
          '/INS/Inscricao/BuscarHierarquiaCompletaAngular',
          oferta.seqOferta.seq!,
        );

        if (typeof oferta.seqOferta.seq === 'number') {
          hierarquia = arq.find((f) => f.seq === oferta.seqOferta.seq)!;
        } else {
          hierarquia = arq.find(
            (f) => f.seq === (oferta.seqOferta as any).seq,
          )!;
        }
      }

      let descricao = ofertasSelecionadas.find((f) => f.seq === hierarquia?.seq)
        ?.descricao as string;
      hierarquia.descricao = descricao ? descricao : hierarquia.descricao;

      let retorno: ListaOfertaModel = {
        oferta: oferta,
        hierquia: hierarquia,
      };
      listaOferta.push(retorno);
    }

    this.selecaoOfertaDataService.setListaOfertas(listaOferta);
  }

  buscarSelecaoOferta() {
    this.route.queryParams.subscribe((params) => {
      const filtro = new FiltroPaginaModel();

      filtro.seqConfiguracaoEtapa = params['seqConfiguracaoEtapa'];
      filtro.idioma = params['idioma'];
      filtro.seqConfiguracaoEtapaPagina = params['seqConfiguracaoEtapaPagina'];
      filtro.seqGrupoOferta = params['seqGrupoOferta'];
      filtro.seqInscricao = params['seqInscricao'];
      this.setFiltro(filtro);

      const selecionarOferta = this.getBuscarSelecaoOferta(
        '/INS/Inscricao/SelecaoOfertaAngular',
        filtro,
      ).subscribe({
        next: (res) => {
          //validar o css caso o usuario venha pelo link somente da seleção de oferta
          //this.validarUrlCss(res);
          this.setTitulo(res.titulo);
          this.setAlertaOferta(res.alertaOferta);
          this.selecaoOfertaDataService.setSelecaoOferta(res);
          this.selecaoOfertaDataService.totalGeralInDB = res.totalGeral;
          //this.setSelecaoOferta(this.selecaoOfertaDataService.$selecaoOferta());
          this.setTituloModalSelecaoOfertra(`Selecionar ${res.labelOferta}`);
          this.montarIstrucoes();
          this.layoutProcessoDataService.setDadosMenuLateral(res);

          this.selecaoOfertaDataService.setExibirMensagemBoletoPago(
            this.selecaoOfertaDataService.$selecaoOferta().possuiBoletoPago,
          );
          //Buscar hierarquia de ofertas
          const buscarHierarquia = this.buscarHierarquia(
            '/INS/Inscricao/BuscarHierarquiaAngular',
            this.$filtro(),
          ).subscribe((arq) => {
            this.selecaoOfertaDataService.setHierarquia(arq);

            this._numeroNoFolha.set(
              this.selecaoOfertaDataService
                .$hierarquia()
                .filter((f) => f.isLeaf).length,
            );

            //Caso especifico:
            //Este cenario acontece quando a arvore for seleção multipla, possuir hierarquia e tiver somente
            //um item folha, pois quando o backend monta ele não leva em consideração o controle de vagas.
            if (
              this.$numeroNoFolha() == 1 &&
              this.selecaoOfertaDataService.$selecaoOferta().ofertas.length == 0
            ) {
              const noFolha = this.selecaoOfertaDataService
                .$hierarquia()
                .find((f) => f.isLeaf);
              const oferta = {
                seqOferta: { seq: noFolha?.seq } as GPILookup,
              } as Ofertas;
              this.selecaoOfertaDataService
                .$selecaoOferta()
                .ofertas.push(oferta);
            }
            this.selecaoOfertaDataService.setNumeroNoFolha(
              this.$numeroNoFolha(),
            );
            this.listarOfertas();
            this.validarLiberarBotaoProximo();
          });
          this.subs.add(buscarHierarquia);

          this.layoutProcessoDataService.setDadosNavegacao(
            this.dadosNavegacao(res),
          );

          if (this.selecaoOfertaDataService.$selecaoOferta().taxas.length > 0) {
            this.setExisteBotaoTaxas(true);
          }
          this.validarLiberarBotaoProximo();
          this.setIsLoading(false);
        },
      });
      this.subs.add(selecionarOferta);
    });
  }

  validarUrlCss(modelo: SelecaoOfertaModel) {
    let ulrCSS = document.getElementById('cssProcesso');
    const origem = window.location.origin;

    if (ulrCSS?.getAttribute('href') == '@urlCSS') {
      const novaRef =
        origem + '/Recursos/Inscricoes/4.0/GPI.Inscricao/' + modelo.urlCss;
      ulrCSS.setAttribute('href', novaRef);
    }
  }
  async botaoSelecionarOferta() {
    let ofertasSelecionadas =
      this.selecaoOfertaDataService.$ofertasSelecionadas();
    let ofertasExistentes =
      this.selecaoOfertaDataService.$selecaoOferta().ofertas;
    this.selecaoOfertaDataService.setDesativarBotaoSelecionaModalHierarquia(
      true,
    );

    // ultimo numero de ordenação
    let ultimaOrdenacao = 0;
    if (ofertasExistentes && ofertasExistentes.length > 0) {
      ofertasExistentes.sort((a, b) => a.numeroOpcao - b.numeroOpcao);
      ultimaOrdenacao =
        ofertasExistentes[ofertasExistentes.length - 1].numeroOpcao;
    }

    // Remove ofertas existentes que não estão nas ofertas selecionadas
    ofertasExistentes = ofertasExistentes.filter((ofertaExistente) =>
      ofertasSelecionadas.some(
        (ofertaSelecionada) =>
          ofertaSelecionada.seqOferta === ofertaExistente.seqOferta,
      ),
    );

    //remove as taxas que são por oferta
    this.selecaoOfertaDataService.$selecaoOferta().taxas =
      this.selecaoOfertaDataService.$selecaoOferta().taxas.filter((f) => {
        return this.selecaoOfertaDataService
          .$ofertasSelecionadas()
          .find((fi) => fi.seqOferta.seq == f.seqOferta);
      });

    // Ordena novamente pelo numero de opção no momento que for dar a ultima opção seja encontrada
    ofertasSelecionadas = ofertasSelecionadas.sort(
      (a, b) => a.numeroOpcao - b.numeroOpcao,
    );

    // Adicionar ofertas selecionadas que não estão nas ofertas existentes
    ofertasSelecionadas.forEach((item) => {
      if (
        !ofertasExistentes.some((oferta) => oferta.seqOferta === item.seqOferta)
      ) {
        ultimaOrdenacao += 1;
        item.numeroOpcao = ultimaOrdenacao;
        ofertasExistentes.push(item);
      }
    });

    if (ofertasSelecionadas.length === 0) {
      this.selecaoOfertaDataService.setDesativarBotaoSelecionaModalHierarquia(
        false,
      );
    }

    // Reorganizar o número de ordenação das ofertas existentes
    this.layoutProcessoDataService.setDesativarTodosBotoes(true);
    this.selecaoOfertaDataService.setAbrirModalSelecaoOferta(false);
    this.selecaoOfertaDataService.setCarregarListaOferta(true);

    const todasOfertasJaCarregadas =
      this.selecaoOfertaDataService.$selecaoOferta().ofertas;

    const idsOfertasJaCarregadas = new Set(
      todasOfertasJaCarregadas.map((oferta) => oferta.seqOferta.seq ?? 0),
    );

    const ofertasNovasParaBuscarTaxas = ofertasSelecionadas.filter(
      (ofertaSelecionada) =>
        !idsOfertasJaCarregadas.has(ofertaSelecionada.seqOferta.seq ?? 0),
    );
    const promises = ofertasExistentes.map(async (oferta, index) => {
      oferta.numeroOpcao = index + 1; // Atribui número de opção
      if (!idsOfertasJaCarregadas.has(oferta.seqOferta.seq!)) {
        const taxa = await this.listarTaxasOfertaInscricaoAngular(
          '/INS/Inscricao/ListarTaxasOfertaInscricaoAngular',
          oferta.seqOferta.seq!,
          this.selecaoOfertaDataService.$selecaoOferta().seqInscricao,
        );

        if (taxa) {
          this.selecaoOfertaDataService.updateTaxasSelecaoOferta(taxa);
        }
      }
    });

    await Promise.all(promises);
    this.selecaoOfertaDataService.setCarregarListaOferta(true);
    this.layoutProcessoDataService.setDesativarTodosBotoes(false);
    this.setExisteBotaoTaxas(
      this.selecaoOfertaDataService.$selecaoOferta().taxas.length > 0,
    );

    this.selecaoOfertaDataService.updateOfertasSelecaoOferta(ofertasExistentes);
    this.listarOfertas();
    this.validarLiberarBotaoProximo();
    this.selecaoOfertaDataService.botaoSelecionarOfertaAcionado = true;
  }

  //#region funções privadas
  private async buscarDesricaoCompleta(): Promise<DatasourceItemModel[]> {
    this.selecaoOfertaDataService.setCarregarListaOferta(true);

    const seqsOferta = this.selecaoOfertaDataService
      .$selecaoOferta()
      .ofertas.map((m) => m.seqOferta.seq!);

    const retorno =
      await this.buscarDescricaoSelecaoOfertasInscricaoSeqsOfertas(
        '/INS/Inscricao/BuscarDescricaoSelecaoOfertasInscricaoSeqsOfertas',
        seqsOferta,
      );

    this.selecaoOfertaDataService.setCarregarListaOferta(false);
    return retorno;
  }

  private montarIstrucoes() {
    // Decodifica o html das instrucoes
    const instrucao = this.selecaoOfertaDataService
      .$selecaoOferta()
      .secoes.find((f: any) => f.token == 'INSTRUCOES').texto;
    if (instrucao) {
      const instrucoesDecode = this.toolsService.decodeHtml(instrucao);

      // Cria um elemento temporário para extrair o texto
      const tempElement = document.createElement('div');
      tempElement.innerHTML = instrucoesDecode;

      // Obtém o texto e remove espaços em branco extras
      const textoConteudo =
        tempElement.textContent || tempElement.innerText || '';
      const textoLimpo = textoConteudo.trim();

      if (textoLimpo.length > 0) {
        this.setSelecaoOfertaInstrucoes(
          this.sanitizer.bypassSecurityTrustHtml(instrucoesDecode),
        );
      }
    }
  }

  private dadosNavegacao(modelo: SelecaoOfertaModel) {
    const retorno: LayoutNavegacaoModel = {
      area: 'INS',
      chaveTextoBotaoAnterior: modelo.chaveTextoBotaoAnterior,
      chaveTextoBotaoProximo: modelo.chaveTextoBotaoProximo,
      idioma: modelo.idioma,
      inscricaoIniciada: modelo.inscricaoIniciada,
      seqConfiguracaoEtapaEncrypted: modelo.seqConfiguracaoEtapaEncrypted,
      seqConfiguracaoEtapaPaginaAnteriorEncrypted:
        modelo.seqConfiguracaoEtapaPaginaAnteriorEncrypted,
      seqGrupoOfertaEncrypted: modelo.seqGrupoOfertaEncrypted,
      seqInscricao: modelo.seqInscricao,
      seqInscricaoEncrypted: modelo.seqInscricaoEncrypted,
      tokenPaginaAnteriorEncrypted: modelo.tokenPaginaAnteriorEncrypted,
      uidProcesso: modelo.uidProcesso,
    };
    return retorno;
  }
  //#endregion

  salvarSelecaoOferta(permiteAlterarBoleto: boolean = false): Promise<boolean> {
    this.layoutProcessoDataService.setLoadBotaoProximo(true);
    this.layoutProcessoDataService.setDesativarTodosBotoes(true);
    this.selecaoOfertaDataService.setDeabilitarBotaoAlterarHieraquia(true);
    this.layoutProcessoDataService.setBotaoProximoAcionado(false);
    let taxas = this.selecaoOfertaDataService
      .$selecaoOferta()
      .taxas.filter(
        (novaTaxa) =>
          !this.selecaoOfertaDataService
            .$taxasPorGrupo()
            .some((existente) => existente.seqTaxa === novaTaxa.seqTaxa),
      );

    taxas.push(...this.selecaoOfertaDataService.$taxasPorGrupo());

    let selecao: SelecaoOfertasSalvar = {
      numeroMaximoOfertaPorInscricao:
        this.selecaoOfertaDataService.$selecaoOferta()
          .numeroMaximoOfertaPorInscricao,

      exigeJustificativaOferta:
        this.selecaoOfertaDataService.$selecaoOferta().exigeJustificativaOferta,

      numeroMaximoConvocacaoPorInscricao:
        this.selecaoOfertaDataService.$selecaoOferta()
          .numeroMaximoConvocacaoPorInscricao,

      numeroOpcoesDesejadas:
        this.selecaoOfertaDataService.$selecaoOferta().numeroOpcoesDesejadas,

      ofertas: this.selecaoOfertaDataService.$selecaoOferta().ofertas,

      taxas: taxas,

      possuiBoletoPago:
        this.selecaoOfertaDataService.$selecaoOferta().possuiBoletoPago,

      cobrancaPorOferta:
        this.selecaoOfertaDataService.$selecaoOferta().cobrancaPorOferta,

      bolsaExAluno: this.selecaoOfertaDataService.$selecaoOferta().bolsaExAluno,

      seqGrupoOferta:
        this.selecaoOfertaDataService.$selecaoOferta().seqGrupoOferta,

      seqGrupoOfertaEncrypted:
        this.selecaoOfertaDataService.$selecaoOferta().seqGrupoOfertaEncrypted,

      seqInscricao: this.selecaoOfertaDataService.$selecaoOferta().seqInscricao,

      seqInscricaoEncrypted:
        this.selecaoOfertaDataService.$selecaoOferta().seqInscricaoEncrypted,

      seqProcesso: this.selecaoOfertaDataService.$selecaoOferta().seqProcesso,

      seqConfiguracaoEtapa:
        this.selecaoOfertaDataService.$selecaoOferta().seqConfiguracaoEtapa,

      idioma: this.selecaoOfertaDataService.$selecaoOferta().idioma,

      token: this.selecaoOfertaDataService.$selecaoOferta().token,

      tokenPaginaAnterior:
        this.selecaoOfertaDataService.$selecaoOferta().tokenPaginaAnterior,

      tokenPaginaAnteriorEncrypted:
        this.selecaoOfertaDataService.$selecaoOferta()
          .tokenPaginaAnteriorEncrypted,

      tokenProximaPagina:
        this.selecaoOfertaDataService.$selecaoOferta().tokenProximaPagina,

      tokenProximaPaginaEncrypted:
        this.selecaoOfertaDataService.$selecaoOferta()
          .tokenProximaPaginaEncrypted,

      tokenResource:
        this.selecaoOfertaDataService.$selecaoOferta().tokenResource,

      tokenSituacaoAtual:
        this.selecaoOfertaDataService.$selecaoOferta().tokenSituacaoAtual,

      seqConfiguracaoEtapaPagina:
        this.selecaoOfertaDataService.$selecaoOferta()
          .seqConfiguracaoEtapaPagina,
      seqConfiguracaoEtapaEncrypted:
        this.selecaoOfertaDataService.$selecaoOferta()
          .seqConfiguracaoEtapaEncrypted,
      seqConfiguracaoEtapaPaginaAnterior:
        this.selecaoOfertaDataService.$selecaoOferta()
          .seqConfiguracaoEtapaPaginaAnterior,
      seqConfiguracaoEtapaPaginaAnteriorEncrypted:
        this.selecaoOfertaDataService.$selecaoOferta()
          .seqConfiguracaoEtapaPaginaAnteriorEncrypted,
      seqConfiguracaoEtapaPaginaProxima:
        this.selecaoOfertaDataService.$selecaoOferta()
          .seqConfiguracaoEtapaPaginaProxima,
      seqConfiguracaoEtapaPaginaProximaEncrypted:
        this.selecaoOfertaDataService.$selecaoOferta()
          .seqConfiguracaoEtapaPaginaProximaEncrypted,
      permiteAlterarBoleto: permiteAlterarBoleto,
    } as SelecaoOfertasSalvar;

    const url = `${environment.frontUrl}INS/Inscricao/SalvarSelecaoOfertaAngular`;

    return firstValueFrom(this.http.post(url, selecao)).then((res: any) => {
      if (res.statusCode === 0) {
        return false;
      }

      if (res.statusCode !== 200) {
        this.messageService.add({
          severity: 'error',
          summary: 'Erro',
          detail: res.data,
          sticky: true,
        });
        this.layoutProcessoDataService.setLoadBotaoProximo(false);
        this.layoutProcessoDataService.setDesativarTodosBotoes(false);
        this.selecaoOfertaDataService.setDeabilitarBotaoAlterarHieraquia(false);
        return true;
      }

      if (res.statusCode === 200) {
        const query = new URLSearchParams(res.data).toString();
        window.location.href = `${environment.frontUrl}INS/Inscricao/UrlPagina?${query}`;
        return true;
      }
      return true;
    });
  }

  validarLiberarBotaoProximo() {
    let desativarBotaoProximo = true;
    const errors: string[] = [];
    this.layoutProcessoDataService.setDesativarBotaoProximo(false);
    //Validar se existe ofertas selecionada pois se ele não tiver nenhuma oferta selecioanda
    //irá bloquear proximo
    if (
      this.selecaoOfertaDataService.$listaOfertas().length === 0 &&
      this.selecaoOfertaDataService.$selecaoOferta().labelOferta
    ) {
      errors.push(
        `Selecione ${this.selecaoOfertaDataService.$selecaoOferta().labelOferta.toLowerCase()} para continuar.`,
      );

      this.layoutProcessoDataService.setDesativarBotaoProximo(
        desativarBotaoProximo,
      );
      this.layoutDataService.setErrosBotaoProximo(errors);
      return;
    }

    //Alguma oferta indisponivel ou impedida
    this.selecaoOfertaDataService.$listaOfertas().forEach((f) => {
      if (f.oferta.ofertaImpedida) {
        errors.push(`A opção ${f.oferta.numeroOpcao} está indisponivel.`);
      }
    });

    //Validar se alguma oferta esta sem justificativa
    if (
      this.selecaoOfertaDataService.$selecaoOferta().exigeJustificativaOferta
    ) {
      this.selecaoOfertaDataService.$listaOfertas().forEach((f) => {
        if (!f.oferta.justificativaInscricao) {
          errors.push(
            `A opção ${f.oferta.numeroOpcao} está sem justificativa.`,
          );
        }
      });
    }

    if (
      this.selecaoOfertaDataService.$selecaoOferta()
        .numeroMaximoConvocacaoPorInscricao &&
      !this.selecaoOfertaDataService.$selecaoOferta().numeroOpcoesDesejadas
    ) {
      errors.push('Selecione a quantidade de opções para ser convocado.');
    }

    if (this.$numeroMinimoGrupoNaoAtingido() === true) {
      this.descricaoGrupoTaxasNaoAtingidos.forEach((f) => {
        errors.push(`O número mínimo de ${f} não foi atingido.`);
      });
    }

    if (errors.length === 0) {
      this.layoutDataService.setErrosBotaoProximo([]);
      this.descricaoGrupoTaxasNaoAtingidos = new Set();
      desativarBotaoProximo = false;
    }

    this.layoutProcessoDataService.setDesativarBotaoProximo(
      desativarBotaoProximo,
    );
    this.layoutDataService.setErrosBotaoProximo(errors);
  }

  exibirBotaoExcluir(
    numeroNoFolha: number,
    numeroMaximoOfertaPorInscricao: number,
    ofertaImpedida: boolean,
  ) {
    let retorno = false;

    if (
      (numeroNoFolha > 1 && numeroMaximoOfertaPorInscricao != 1) ||
      ofertaImpedida
    ) {
      retorno = true;
    }

    const numeroTaxasPorOferta = this.selecaoOfertaDataService
      .$selecaoOferta()
      .taxas.filter((f) => f.tipoCobranca == TipoCobranca.porOferta).length;
    const numeroTaxasPorQuantidadeOferta = this.selecaoOfertaDataService
      .$selecaoOferta()
      .taxas.filter(
        (f) => f.tipoCobranca == TipoCobranca.porQuantidadeOfertas,
      ).length;

    if (
      this.selecaoOfertaDataService.$selecaoOferta().possuiBoletoPago &&
      (numeroTaxasPorOferta > 0 || numeroTaxasPorQuantidadeOferta > 0)
    ) {
      retorno = false;
    }

    return retorno;
  }

  exibirBotaoSelecionarHierarquia() {
    let retorno = false;

    if (this.$numeroNoFolha() > 1) {
      retorno = true;
    }
    const numeroTaxasPorOferta = this.selecaoOfertaDataService
      .$selecaoOferta()
      ?.taxas?.filter((f) => f.tipoCobranca == TipoCobranca.porOferta)?.length;

    if (
      this.selecaoOfertaDataService.$selecaoOferta().possuiBoletoPago &&
      numeroTaxasPorOferta >= 1
    ) {
      retorno = false;
    }
    return retorno;
  }
}
