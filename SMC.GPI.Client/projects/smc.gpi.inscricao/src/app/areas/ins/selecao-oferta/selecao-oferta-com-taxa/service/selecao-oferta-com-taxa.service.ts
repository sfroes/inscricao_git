import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { computed, inject, Injectable, signal, untracked } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { ToolsService } from '../../../../../core/tools/tools.service';
import { TaxasOferta } from '../../interfaces/taxas-oferta.interface';
import { TaxasSelecioandas } from '../../interfaces/taxas-selecionadas.interface';
import { TipoCobranca } from '../../model/enums/tipo-cobranca.enum';
import { GrupoTaxa } from '../../model/grupo-taxa.model';
import { ListaOfertaModel } from '../../model/lista-oferta.model';
import { SelecaoOfertaModel, Taxa } from '../../model/selecao-oferta.model';
import { GrupoTaxaPorTipoCobranca } from '../../model/taxas-selecionadas.model';
import { SelecaoOfertaDataService } from '../../service/selecao-oferta-data.service';
import { SelecaoOfertaService } from '../../service/selecao-oferta.service';
import { LayoutProcessoDataService } from './../../../../../core/layout-processo/service/layout-processo-data.service';

@Injectable({
  providedIn: 'root',
})
export class SelecaoOfertaComTaxaService {
  //#region variaveis
  selecaoOferta: SelecaoOfertaModel = {} as SelecaoOfertaModel;
  ofertaDesativada: string = 'smc-gpi-listbox-item-desativado';
  itemUnico: string = 'smc-gpi-listbox-item-unico';
  numeroNoFolha: number = 0;
  timeoutDragdrop: any;
  taxas: Taxa[] = [];
  //#endregion

  //#region signals
  $listaOferta = signal<ListaOfertaModel[]>([]);
  $isLoading = signal(true);
  $desabilitarDragDrop = signal(false);
  private _instrucoes = signal<any>(null);
  $instrucoes = this._instrucoes.asReadonly();
  setInstrucoes(data: any) {
    this._instrucoes.set(data);
  }

  $existeTaxa = signal(false);
  $grupoTaxa = signal<GrupoTaxaPorTipoCobranca>({} as GrupoTaxaPorTipoCobranca);
  $totalGeral = computed(() => {
    const selecao = this.selecaoOfertaDataService.$selecaoOferta();
    return selecao.totalGeral;
  });
  private _taxasPorInscricao = signal<TaxasSelecioandas>(
    {} as TaxasSelecioandas,
  );
  $taxasPorInscricao = this._taxasPorInscricao.asReadonly();

  private _taxasPorQuantidadeOferta = signal<TaxasSelecioandas>(
    {} as TaxasSelecioandas,
  );
  $taxasPorQuantidadeOferta = this._taxasPorQuantidadeOferta.asReadonly();

  private _taxasPorOferta = signal<TaxasSelecioandas[]>([]);
  $taxasPorOferta = this._taxasPorOferta.asReadonly();
  //#endregion

  //#region injeção de dependencia
  selecaoOfertaDataService = inject(SelecaoOfertaDataService);
  selecaoOfertaService = inject(SelecaoOfertaService);
  toolsService = inject(ToolsService);
  sanitizer = inject(DomSanitizer);
  LayoutProcessoDataService = inject(LayoutProcessoDataService);
  //#endregion

  constructor() {}

  ordernar(event: CdkDragDrop<ListaOfertaModel[]>) {
    moveItemInArray(
      this.$listaOferta(),
      event.previousIndex,
      event.currentIndex,
    );
    this.organizarLista();
  }

  apagarOferta(numeroOpcao: number) {
    this.$listaOferta.set(
      this.$listaOferta().filter((f) => f.oferta.numeroOpcao != numeroOpcao),
    );

    const ofertaAExcluir = this.selecaoOfertaDataService
      .$selecaoOferta()
      .ofertas.find((f) => f.numeroOpcao === numeroOpcao);

    //Atualizar a selecao de oferta
    this.selecaoOfertaDataService.$selecaoOferta().ofertas =
      this.selecaoOfertaDataService
        .$selecaoOferta()
        .ofertas.filter((f) => f.numeroOpcao != numeroOpcao);

    //Atulizar ofertas selecionadas
    this.selecaoOfertaDataService.setOfertasSelecionadas(
      this.selecaoOfertaDataService
        .$ofertasSelecionadas()
        .filter((f) => f.numeroOpcao != numeroOpcao),
    );

    //filtrar taxas
    this.selecaoOfertaDataService.$selecaoOferta().taxas =
      this.selecaoOfertaDataService
        .$selecaoOferta()
        .taxas.filter((f) => f.seqOferta !== ofertaAExcluir?.seqOferta.seq);

    this.selecaoOfertaDataService.updateListaOfertas(this.$listaOferta());

    this.selecaoOfertaDataService.setBotaoCancelarHierarquia(true);
    this.organizarLista();
    this.selecaoOfertaService.validarLiberarBotaoProximo();

    //foi nescessario fazer essa atualização pois quando o array é vazio ele destroi o componente pai e então o effect não propaga as mudanças.
    if (this.$listaOferta().length == 0) {
      this.taxas = this.selecaoOfertaDataService.$selecaoOferta().taxas;
      this.montarGrupoTaxas();
      this.montarTaxasPorOferta();
      this.montarTaxasPorInscricao();
      this.montarTaxasPorQuantidadeOferta();
      this.montarIstrucoesTaxas();
    }
  }

  organizarLista() {
    this.$listaOferta().forEach((item, index) => {
      item.oferta.numeroOpcao = index + 1;
    });
    this.selecaoOfertaDataService.setListaOfertas(this.$listaOferta());
    this.selecaoOfertaDataService.setOrdernarLista(true);
  }

  atulizarTextoJustificativa(event: any, numeroOpcao: number) {
    const textArea = event.target as HTMLTextAreaElement;
    const textoAtualizar = textArea.value;
    this.$listaOferta().forEach((oferta) => {
      if (oferta.oferta.numeroOpcao == numeroOpcao) {
        oferta.oferta.justificativaInscricao = textoAtualizar;
      }
    });
    this.selecaoOfertaService.validarLiberarBotaoProximo();
  }

  opcoesParaConvocacao(): number[] {
    let opcoesNumeroMaximoConvocacaoPorInscricao: number[] = [];
    if (
      this.selecaoOfertaDataService.$selecaoOferta()
        .numeroMaximoConvocacaoPorInscricao
    ) {
      for (
        var i = 1;
        i <=
        this.selecaoOfertaDataService.$selecaoOferta()
          .numeroMaximoConvocacaoPorInscricao;
        i++
      ) {
        opcoesNumeroMaximoConvocacaoPorInscricao.push(i);
      }
    }
    return opcoesNumeroMaximoConvocacaoPorInscricao;
  }

  opcaoSelecionadaConvocacao(event: any) {
    this.selecaoOferta.numeroOpcoesDesejadas = event.value;
  }

  verificaOfertaAtiva(listaOferta: ListaOfertaModel): string {
    if (listaOferta.oferta.ofertaImpedida) {
      return this.ofertaDesativada;
    }
    return '';
  }

  adcionarClasseMobile(event: any) {
    const elementoClicado = event.target as HTMLDivElement;

    //Pegar o primeiro pai ancestral que tenha o atributo cdkdrag
    const paiElemento = elementoClicado.closest('[cdkDrag]') as HTMLDivElement;
    this.timeoutDragdrop = setTimeout(() => {
      paiElemento.classList.add('smc-gpi-selecao-touch');
    }, 500);
  }

  removeClasseMobile(event: any) {
    const elementoClicado = event.target as HTMLDivElement;
    clearTimeout(this.timeoutDragdrop);

    //Pegar o primeiro pai ancestral que tenha o atributo cdkdrag
    const paiElemento = elementoClicado.closest('[cdkDrag]') as HTMLDivElement;
    setTimeout(() => {
      paiElemento.classList.remove('smc-gpi-selecao-touch');
    }, 100);
  }

  removeClasseDesktop() {
    // Seleciona todos os elementos com atributo cdkDrag
    const elementosArrastaveis = document.querySelectorAll('[cdkDrag]');
    elementosArrastaveis.forEach((elemento) => {
      elemento.classList.remove('smc-gpi-selecao-touch');
    });
  }

  adcionarClasseDesktop(event: any) {
    const elementoClicado = event.target as HTMLDivElement;

    //Pegar o primeiro pai ancestral que tenha o atributo cdkdrag
    const paiElemento = elementoClicado.closest('[cdkDrag]') as HTMLDivElement;

    paiElemento.classList.add('smc-gpi-selecao-touch');
  }

  numeroItens(
    event: any,
    seqTaxa: number,
    seqOferta: number,
    seqGrupo: number | null = null,
  ) {
    event.originalEvent.stopPropagation();
    const qntItens = event.value;

    const taxa = this.selecaoOfertaDataService
      .$selecaoOferta()
      .taxas.find((f) => f.seqTaxa === seqTaxa);
    let total = 0;

    if (seqGrupo) {
      const grupoTaxaOferta = this.$grupoTaxa().grupoTaxaOferta.map((grupo) =>
        this.validaMaximoAtingido(
          grupo,
          seqGrupo,
          seqTaxa,
          seqOferta,
          qntItens,
        ),
      );

      this.$grupoTaxa().grupoTaxaOferta = grupoTaxaOferta;

      this.validaMinimoAtingido();

      this.selecaoOfertaService.validarLiberarBotaoProximo();

      const todosGruposTaxas = [
        ...this.$grupoTaxa().grupoTaxaInscricao,
        ...this.$grupoTaxa().grupoTaxaOferta,
        ...this.$grupoTaxa().grupoTaxaQuantidadeOferta,
      ];

      this.selecaoOfertaDataService.updateTaxasPorGrupo(
        todosGruposTaxas.flatMap((m) => m.taxas),
      );

      total = this.calcularTaxasComGrupo(
        taxa,
        seqTaxa,
        seqOferta,
        qntItens,
        total,
        seqGrupo,
      );
    } else {
      total = this.calcularTaxasSemGrupo(
        taxa,
        seqTaxa,
        seqOferta,
        qntItens,
        total,
      );
    }
    const incrementar = event.originalEvent.currentTarget.classList.contains(
      'p-inputnumber-increment-button',
    );

    //Determinada validação seria para manter a mesma logica de somatória utlizando a inversão dos valores
    //onde quando decrementar um valor diminuimos a quantidade que ele removeu
    if (!incrementar) {
      total = total * -1;
    }

    const totalGeral =
      this.selecaoOfertaDataService.$selecaoOferta().totalGeral + total;

    this.selecaoOfertaDataService.updateTotalGeralSelecaoOferta(totalGeral);
  }

  validaMinimoAtingido() {
    const grupoTaxa = this.$grupoTaxa();
    this.selecaoOfertaService.descricaoGrupoTaxasNaoAtingidos = new Set();

    let minimoNaoAtingido = false;
    if (grupoTaxa && grupoTaxa.grupoTaxaInscricao) {
      const inscricaoNaoAtingiu = grupoTaxa.grupoTaxaInscricao.some((s) =>
        s.minimoNaoAtingido(),
      );
      if (inscricaoNaoAtingiu) {
        minimoNaoAtingido = true;
        grupoTaxa.grupoTaxaInscricao.forEach((f) => {
          if (f.minimoNaoAtingido()) {
            this.selecaoOfertaService.descricaoGrupoTaxasNaoAtingidos.add(
              f.descricao,
            );
          }
        });
      }
    }

    if (grupoTaxa && grupoTaxa.grupoTaxaOferta) {
      const ofertaNaoAtingiu = grupoTaxa.grupoTaxaOferta.some((s) =>
        s.minimoNaoAtingido(),
      );
      if (ofertaNaoAtingiu) {
        minimoNaoAtingido = true;
        grupoTaxa.grupoTaxaOferta.forEach((f) => {
          if (f.minimoNaoAtingido()) {
            this.selecaoOfertaService.descricaoGrupoTaxasNaoAtingidos.add(
              f.descricao,
            );
          }
        });
      }
    }

    this.selecaoOfertaService.setNumeroMinimoGrupoNaoAtingido(
      minimoNaoAtingido,
    );
  }

  validaMaximoAtingido(
    grupo: GrupoTaxa,
    seqGrupo: number,
    seqTaxa: number,
    seqOferta: number,
    qntItens: number,
  ) {
    // Se não for o grupo que estamos modificando, retorna ele sem alteração
    if (grupo.seq !== seqGrupo) {
      return grupo;
    }

    // É o grupo correto. Agora vamos atualizar a taxa específica dentro dele.
    const taxasAtualizadasComDuplicatas = grupo.taxas.map((taxa) => {
      if (taxa.seqTaxa !== seqTaxa) {
        return taxa; // Não é a taxa, retorna como está
      }
      // É a taxa correta, atualiza o numeroItens
      return { ...taxa, numeroItens: qntItens };
    });

    const mapaDeTaxasUnicas = new Map<number, Taxa>();

    taxasAtualizadasComDuplicatas.forEach((taxa) => {
      // A chave do Map será o seqTaxa. Se uma chave já existe,
      // o valor (o objeto da taxa) será substituído.
      if (taxa.seqOferta === seqOferta) {
        mapaDeTaxasUnicas.set(taxa.seqTaxa, taxa);
      }
    });

    const novasTaxasDoGrupo = Array.from(mapaDeTaxasUnicas.values());

    // Agora, com as taxas atualizadas, calculamos a soma total
    const somaTotalItens = novasTaxasDoGrupo.reduce(
      (s, t) => s + (t.numeroItens || 0),
      0,
    );

    const novoMaximoAtingido =
      grupo.numeroMaximoItens !== null &&
      somaTotalItens >= grupo.numeroMaximoItens;

    grupo.taxas.map((m) => {
      if (m.seqOferta === seqOferta) {
        m.maximoAtingido = novoMaximoAtingido;
        if (grupo.numeroMaximoItens) {
          m.quantidadeFaltante = grupo.numeroMaximoItens - somaTotalItens;
        }
      }
    });

    this.obterMensagemOrientacaoTaxasTag(grupo, seqOferta);

    return grupo;
  }

  calcularTaxasComGrupo(
    taxa: Taxa | undefined,
    seqTaxa: number,
    seqOferta: number,
    qntItens: number,
    total: number,
    seqGrupo: number,
  ): number {
    const grupo =
      this.$grupoTaxa().grupoTaxaInscricao.find((f) => f.seq == seqGrupo) ??
      this.$grupoTaxa().grupoTaxaOferta.find((f) => f.seq == seqGrupo);

    if (grupo) {
      if (taxa?.tipoCobranca === TipoCobranca.porInscricao) {
        grupo.taxas.forEach((taxa) => {
          if (taxa.seqTaxa === seqTaxa && taxa) {
            taxa.numeroItens = qntItens;
            taxa.valorTotalTaxa = taxa.valorItem! * qntItens;
            total += taxa.valorItem!;
          }
        });
      }

      if (taxa?.tipoCobranca === TipoCobranca.porOferta) {
        grupo.taxas
          .filter((f) => f.seqOferta == seqOferta)
          .forEach((taxa) => {
            if (taxa.seqTaxa === seqTaxa) {
              taxa.numeroItens = qntItens;
              taxa.valorTotalTaxa = taxa.valorItem! * qntItens;
              total += taxa.valorItem!;
            }
          });
      }
    }

    return total;
  }

  calcularTaxasComGrupoSemSeq(): number {
    let total = 0;
    total += this.$grupoTaxa().grupoTaxaInscricao.reduce(
      (acumulador, grupo) => {
        grupo.taxas.forEach((taxa) => {
          taxa.valorTotalTaxa = taxa.valorItem! * (taxa.numeroItens ?? 0);
          acumulador += taxa.valorTotalTaxa;
        });
        return acumulador;
      },
      0,
    );

    total += this.$grupoTaxa().grupoTaxaOferta.reduce((acumulador, grupo) => {
      grupo.taxas.forEach((taxa) => {
        taxa.valorTotalTaxa = taxa.valorItem! * (taxa.numeroItens ?? 0);
        acumulador += taxa.valorTotalTaxa;
      });
      return acumulador;
    }, 0);

    return total;
  }

  calcularTaxasSemGrupo(
    taxa: Taxa | undefined,
    seqTaxa: number,
    seqOferta: number,
    qntItens: number,
    total: number,
    tipoCobranca?: TipoCobranca,
  ): number {
    let valorTotalCalculadoParaItemEspecifico: number = 0;
    // Use o tipoCobranca passado como parâmetro ou pegue da taxa
    const tipo = tipoCobranca || taxa?.tipoCobranca;

    switch (tipo) {
      case TipoCobranca.porInscricao:
        this._taxasPorInscricao.update((grupoAtual) => {
          if (
            grupoAtual &&
            grupoAtual.taxasOferta &&
            grupoAtual.seqOferta === seqOferta
          ) {
            const novasTaxasOferta = grupoAtual.taxasOferta.map((itemTaxa) => {
              if (itemTaxa.seqTaxa === seqTaxa) {
                valorTotalCalculadoParaItemEspecifico =
                  (itemTaxa.valorItem || 0) * (qntItens || 0);
                total += itemTaxa.valorItem || 0;
                return {
                  ...itemTaxa,
                  numeroItens: qntItens,
                  valorTotalTaxa: valorTotalCalculadoParaItemEspecifico,
                };
              }
              return itemTaxa;
            });
            return { ...grupoAtual, taxasOferta: novasTaxasOferta };
          }
          return grupoAtual;
        });
        break;

      case TipoCobranca.porOferta:
        this._taxasPorOferta.update((listaDeGruposAtual) =>
          listaDeGruposAtual.map((grupo) => {
            if (grupo.seqOferta === seqOferta && grupo.taxasOferta) {
              const novasTaxasOferta = grupo.taxasOferta.map((itemTaxa) => {
                if (itemTaxa.seqTaxa === seqTaxa) {
                  valorTotalCalculadoParaItemEspecifico =
                    (itemTaxa.valorItem || 0) * (qntItens || 0);
                  total += itemTaxa.valorItem || 0;
                  return {
                    ...itemTaxa,
                    numeroItens: qntItens,
                    valorTotalTaxa: valorTotalCalculadoParaItemEspecifico,
                  };
                }
                return itemTaxa;
              });
              return { ...grupo, taxasOferta: novasTaxasOferta };
            }
            return grupo;
          }),
        );
        break;

      case TipoCobranca.porQuantidadeOfertas:
        this._taxasPorQuantidadeOferta.update((grupoAtual) => {
          if (
            grupoAtual &&
            grupoAtual.taxasOferta &&
            grupoAtual.seqOferta === seqOferta
          ) {
            const numOfertasSelecionadas =
              this.selecaoOfertaDataService.$selecaoOferta().ofertas.length;
            const novasTaxasOferta = grupoAtual.taxasOferta.map((itemTaxa) => {
              if (itemTaxa.seqTaxa === seqTaxa) {
                valorTotalCalculadoParaItemEspecifico =
                  (itemTaxa.valorItem || 0) * numOfertasSelecionadas;
                total += valorTotalCalculadoParaItemEspecifico;
                return {
                  ...itemTaxa,
                  numeroItens: numOfertasSelecionadas,
                  valorTotalTaxa: valorTotalCalculadoParaItemEspecifico,
                };
              }
              return itemTaxa;
            });
            return { ...grupoAtual, taxasOferta: novasTaxasOferta };
          }
          return grupoAtual;
        });
        break;
    }

    this.selecaoOfertaDataService.updateItemTaxaNaSelecaoOferta(
      seqTaxa,
      seqOferta,
      qntItens,
    );

    return total;
  }
  calcularTaxas() {
    let total = 0;
    if (this.$taxasPorInscricao()) {
      this.$taxasPorInscricao().taxasOferta.forEach((taxa) => {
        total += taxa.valorTotalTaxa;
      });
    }
    if (
      this.$taxasPorQuantidadeOferta() &&
      this.$taxasPorQuantidadeOferta().taxasOferta
    ) {
      this.$taxasPorQuantidadeOferta().taxasOferta.forEach((f) => {
        total +=
          f.valorItem! *
          this.selecaoOfertaDataService.$selecaoOferta().ofertas.length;
      });
    }

    this.$taxasPorOferta().forEach((taxas) => {
      taxas.taxasOferta.forEach((taxa) => {
        total += taxa.valorTotalTaxa;
      });
    });
    total += this.calcularTaxasComGrupoSemSeq();
    this.selecaoOfertaDataService.$selecaoOferta().totalGeral = total;
  }

  montarIstrucoesTaxas() {
    // Decodifica o html das instrucoes
    const instrucao = this.selecaoOfertaDataService
      .$selecaoOferta()
      .secoes.find((f: any) => f.token == 'INSTRUCOES_TAXA').texto;
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
        const sanitaze =
          this.sanitizer.bypassSecurityTrustHtml(instrucoesDecode);
        this.setInstrucoes(sanitaze);
      }
    }
  }

  montarTaxasPorOferta() {
    const taxasPorOfertaMap = new Map<number, TaxasSelecioandas>();

    this.taxas.forEach((taxa) => {
      const possuiGrupo = this.$grupoTaxa().grupoTaxaOferta.some((m) =>
        m.taxas.some(
          (s) => s.seqTaxa === taxa.seqTaxa && s.seqOferta === taxa.seqOferta,
        ),
      );

      if (taxa.tipoCobranca === TipoCobranca.porOferta) {
        let item = taxasPorOfertaMap.get(taxa.seqOferta);

        if (!item) {
          const descricaoOferta =
            this.$listaOferta().find((f) => f.hierquia?.seq === taxa.seqOferta)
              ?.hierquia?.descricao || '';

          const numeroOrdem =
            this.$listaOferta().find((f) => f.hierquia?.seq === taxa.seqOferta)
              ?.oferta.numeroOpcao || '';

          item = {
            seqOferta: taxa.seqOferta,
            descricaoOferta: descricaoOferta,
            numeroOpcao: +numeroOrdem!,
            taxasOferta: [],
          };

          taxasPorOfertaMap.set(taxa.seqOferta, item);

          if (possuiGrupo) {
            return;
          }

          item.taxasOferta.push({
            descricaoTaxa: taxa.descricao,
            descricaoComplementar: taxa.descricaoComplementar,
            valorTaxa: taxa.valorItem.toString(),
            seqTaxa: taxa.seqTaxa,
            valorItem: taxa.valorItem,
            numeroMinimo: taxa.numeroMinimo,
            numeroMaximo: taxa.numeroMaximo,
            valorTotalTaxa: taxa.valorTotalTaxa,
            numeroItens: taxa.numeroItens,
          });
        } else {
          if (possuiGrupo) {
            return;
          }

          item.taxasOferta.push({
            descricaoTaxa: taxa.descricao,
            descricaoComplementar: taxa.descricaoComplementar,
            valorTaxa: taxa.valorItem.toString(),
            seqTaxa: taxa.seqTaxa,
            valorItem: taxa.valorItem,
            numeroMinimo: taxa.numeroMinimo,
            numeroMaximo: taxa.numeroMaximo,
            valorTotalTaxa: taxa.valorTotalTaxa,
            numeroItens: taxa.numeroItens,
          });
        }
      }
    });

    this._taxasPorOferta.set(Array.from(taxasPorOfertaMap.values()));
    this._taxasPorOferta.update((u) =>
      u.sort((a, b) => a.numeroOpcao - b.numeroOpcao),
    );
  }

  montarTaxasPorQuantidadeOferta() {
    const taxaPorQuantidadeOferta: TaxasSelecioandas = {} as TaxasSelecioandas;
    const selecao = this.selecaoOfertaDataService.$selecaoOferta();
    const numeroItens = selecao.ofertas.length;
    taxaPorQuantidadeOferta.taxasOferta = [];
    const taxaOfertaUnico = new Set<number>();
    this.taxas.forEach((taxa) => {
      if (taxa.tipoCobranca !== TipoCobranca.porQuantidadeOfertas) return;

      if (!taxaOfertaUnico.has(taxa.seqTaxa)) {
        taxaPorQuantidadeOferta.taxasOferta.push({
          descricaoTaxa: taxa.descricao,
          descricaoComplementar: taxa.descricaoComplementar,
          valorTaxa: taxa.valorItem.toString(),
          seqTaxa: taxa.seqTaxa,
          valorItem: taxa.valorItem,
          numeroMinimo: taxa.numeroMinimo,
          numeroMaximo: taxa.numeroMaximo,
          valorTotalTaxa: taxa.valorItem * numeroItens,
          numeroItens,
        });
      }
      taxaOfertaUnico.add(taxa.seqTaxa);
      this.selecaoOfertaDataService.$selecaoOferta().taxas.forEach((f) => {
        if (f.seqTaxa === taxa.seqTaxa && f.seqOferta === taxa.seqOferta) {
          f.numeroItens = numeroItens;
          f.valorTotalTaxa = taxa.valorItem * numeroItens;
        }
      });
    });
    this._taxasPorQuantidadeOferta.set(taxaPorQuantidadeOferta);
  }

  montarTaxasPorInscricao() {
    const taxasPorInscircao = new Map<number, TaxasSelecioandas>();

    const ofertasSelecionadas = this.selecaoOfertaDataService
      .$ofertasSelecionadas()
      .map((f: any) => f.seqOferta.seq);

    if (ofertasSelecionadas.length === 0) {
      this._taxasPorInscricao.set({} as TaxasSelecioandas);
    }

    this.taxas.forEach((taxa) => {
      if (taxa.tipoCobranca === TipoCobranca.porInscricao) {
        if (
          this.$grupoTaxa().grupoTaxaInscricao.some((m) =>
            m.taxas.some((s) => s.seqTaxa === taxa.seqTaxa),
          )
        ) {
          return;
        }

        let item = taxasPorInscircao.get(taxa.seqOferta);

        if (!item) {
          const descricaoOferta =
            this.$listaOferta().find((f) => f.hierquia?.seq === taxa.seqOferta)
              ?.hierquia?.descricao || '';
          const numeroOrdem =
            this.$listaOferta().find((f) => f.hierquia?.seq === taxa.seqOferta)
              ?.oferta.numeroOpcao || '';
          item = {
            seqOferta: taxa.seqOferta,
            descricaoOferta: descricaoOferta,
            numeroOpcao: +numeroOrdem!,
            taxasOferta: [],
          };
        }

        taxasPorInscircao.set(taxa.seqOferta, item);

        let numeroItens = taxa.numeroItens;

        const taxasSelecioandas = untracked(() => this.$taxasPorInscricao());

        if (taxasSelecioandas) {
          const taxaOferta = taxasSelecioandas.taxasOferta?.find(
            (f) => f.seqTaxa === taxa.seqTaxa,
          );

          if (taxaOferta && ofertasSelecionadas.length > 0) {
            numeroItens = taxaOferta.numeroItens;
          }
        }

        item.taxasOferta.push({
          descricaoTaxa: taxa.descricao,
          descricaoComplementar: taxa.descricaoComplementar,
          valorTaxa: taxa.valorItem.toString(),
          seqTaxa: taxa.seqTaxa,
          valorItem: taxa.valorItem,
          numeroMinimo: taxa.numeroMinimo,
          numeroMaximo: taxa.numeroMaximo,
          valorTotalTaxa: (numeroItens ?? 0) * taxa.valorItem,
          numeroItens: numeroItens,
        });

        this.selecaoOfertaDataService.$selecaoOferta().taxas.forEach((f) => {
          if (f.seqTaxa === taxa.seqTaxa && f.seqOferta === taxa.seqOferta) {
            f.numeroItens = numeroItens;
            f.valorTotalTaxa = taxa.valorItem * (numeroItens ?? 0);
          }
        });
      }
    });
    const taxasDistinct = Array.from(taxasPorInscircao.values()).filter(
      (atual, indice, array) => {
        return (
          indice ===
          array.findIndex((o) =>
            o.taxasOferta.some((ot) =>
              atual.taxasOferta.some((at) => at.seqTaxa === ot.seqTaxa),
            ),
          )
        );
      },
    );

    this._taxasPorInscricao.set(taxasDistinct[0]);
  }

  getMaxPermitido(grupo: GrupoTaxa, taxaAtual: Taxa): number | null {
    // soma só as demais taxas
    const somaOutras = grupo.taxas
      .filter(
        (t) =>
          t.seqTaxa !== taxaAtual.seqTaxa &&
          t.seqOferta === taxaAtual.seqOferta,
      )
      .reduce((s, t) => s + (t.numeroItens || 0), 0);

    if (grupo.numeroMaximoItens) {
      const numerMaximoRestantePorGrupo = grupo.numeroMaximoItens - somaOutras;
      if (
        taxaAtual.numeroMaximo &&
        taxaAtual.numeroMaximo < numerMaximoRestantePorGrupo
      ) {
        return taxaAtual.numeroMaximo;
      }

      return numerMaximoRestantePorGrupo;
    }

    return taxaAtual.numeroMaximo;
  }
  bloquearInserirDadosPorTeclado(event: Event) {
    event.preventDefault();
    const inputElement = event.target as HTMLInputElement;
    inputElement.blur();
  }

  montarGrupoTaxas() {
    const gruposTaxas: GrupoTaxaPorTipoCobranca = {
      grupoTaxaInscricao: [],
      grupoTaxaOferta: [],
      grupoTaxaQuantidadeOferta: [],
    };

    const ofertasSelecionadas = this.selecaoOfertaDataService
      .$ofertasSelecionadas()
      .map((f: any) => f.seqOferta.seq);

    for (const grupoData of this.selecaoOfertaDataService.$selecaoOferta()
      .gruposTaxa) {
      let taxasParaGrupo: Taxa[];
      const seqTaxasDoGrupo = grupoData.itens.map((item: any) => item.seqTaxa);

      taxasParaGrupo = this.taxas.filter((taxa) =>
        seqTaxasDoGrupo.includes(taxa.seqTaxa),
      );

      const grupoTaxa = untracked(() => this.$grupoTaxa());

      // mantem a quantidade de itens previamente selecionadas
      taxasParaGrupo.forEach((f) => {
        let taxaEncontrada;

        // Verifica se grupoTaxaInscricao existe antes de chamar find
        if (grupoTaxa.grupoTaxaInscricao) {
          taxaEncontrada = grupoTaxa.grupoTaxaInscricao.map((fi) =>
            fi.taxas.find((s) => s.seqTaxa === f.seqTaxa),
          );
        }

        // Se ainda não encontrou e grupoTaxaOferta existe
        if (!taxaEncontrada && grupoTaxa.grupoTaxaOferta) {
          taxaEncontrada = grupoTaxa.grupoTaxaOferta.map((fi) =>
            fi.taxas.find((s) => s.seqTaxa === f.seqTaxa),
          );
        }

        // Se ainda não encontrou e grupoTaxaQuantidadeOferta existe
        if (!taxaEncontrada && grupoTaxa.grupoTaxaQuantidadeOferta) {
          taxaEncontrada = grupoTaxa.grupoTaxaQuantidadeOferta.map((fi) =>
            fi.taxas.find((s) => s.seqTaxa === f.seqTaxa),
          );
        }

        if (taxaEncontrada) {
          //f.numeroItens = taxaEncontrada[0]?.numeroItens ?? 0;
          f.numeroItens =
            taxaEncontrada.find((ff) => ff?.seqTaxa == f.seqTaxa)
              ?.numeroItens ?? 0;
        }
      });
      //Caso o modulo de edição, zera as taxas
      if (
        this.selecaoOfertaDataService.botaoSelecionarOfertaAcionado &&
        ofertasSelecionadas.length === 0
      ) {
        taxasParaGrupo.map((m) => {
          m.numeroItens = m.numeroMinimo;
        });
      }

      const filterAndDeduplicateTaxes = (
        currentTaxas: Taxa[],
        checkOfertaSelection: boolean = false,
      ): Taxa[] => {
        // Usaremos um Set para armazenar chaves únicas dos itens já processados.
        // A chave será uma string combinando seqTaxa e, opcionalmente, seqOferta.
        const seenKeys = new Set<string>();
        const deduplicatedTaxes: Taxa[] = [];

        for (const currentTaxa of currentTaxas) {
          // 1. Filtragem inicial baseada em ofertasSelecionadas, se aplicável
          if (checkOfertaSelection && ofertasSelecionadas.length > 0) {
            const isSelecionada = ofertasSelecionadas.includes(
              currentTaxa.seqOferta,
            );
            if (!isSelecionada) {
              // Se a oferta não está selecionada, pulamos este item.
              continue;
            }
          }

          // 2. Criação da chave única para deduplicação
          let uniqueKey: string;
          if (checkOfertaSelection) {
            // Se checkOfertaSelection é true, a chave inclui seqTaxa e seqOferta
            uniqueKey = `${currentTaxa.seqTaxa}-${currentTaxa.seqOferta}`;
          } else {
            // Caso contrário, a chave é apenas seqTaxa
            uniqueKey = `${currentTaxa.seqTaxa}`;
          }

          // 3. Verificação de duplicidade e adição ao resultado
          if (!seenKeys.has(uniqueKey)) {
            // Se esta combinação de seqTaxa (e seqOferta, se aplicável) ainda não foi vista
            seenKeys.add(uniqueKey); // Adiciona a chave ao Set
            deduplicatedTaxes.push(currentTaxa); // Adiciona a taxa ao array de resultado
          }
          // Se a chave já foi vista, simplesmente ignoramos a taxa atual (removendo a duplicata)
        }

        return deduplicatedTaxes;
      };

      const baseGrupoProps = {
        seq: grupoData.seq,
        descricao: grupoData.descricao,
        numeroMaximoItens: grupoData.numeroMaximoItens,
        numeroMinimoItens: grupoData.numeroMinimoItens,
        numeroItens: grupoData.numeroMinimoItens, // Valor inicial
      };

      let novoGrupo: GrupoTaxa; // Declare a variável para o novo grupo

      switch (grupoData.itens[0].taxa.tipoCobranca) {
        case TipoCobranca.porQuantidadeOfertas:
          novoGrupo = new GrupoTaxa({
            ...baseGrupoProps,
            taxas: filterAndDeduplicateTaxes(taxasParaGrupo, true),
          });
          gruposTaxas.grupoTaxaQuantidadeOferta.push(novoGrupo);
          break;
        case TipoCobranca.porOferta:
          novoGrupo = new GrupoTaxa({
            ...baseGrupoProps,
            taxas: filterAndDeduplicateTaxes(taxasParaGrupo, true),
          });
          gruposTaxas.grupoTaxaOferta.push(novoGrupo);
          break;
        default:
          novoGrupo = new GrupoTaxa({
            ...baseGrupoProps,
            taxas: filterAndDeduplicateTaxes(taxasParaGrupo, false),
          });
          gruposTaxas.grupoTaxaInscricao.push(novoGrupo);
          break;
      }
    }

    this.$grupoTaxa.set(gruposTaxas);

    const todosGruposTaxas = [
      ...this.$grupoTaxa().grupoTaxaInscricao,
      ...this.$grupoTaxa().grupoTaxaOferta,
      ...this.$grupoTaxa().grupoTaxaQuantidadeOferta,
    ];

    this.selecaoOfertaDataService.updateTaxasPorGrupo(
      todosGruposTaxas.flatMap((m) => m.taxas),
    );

    this.validaMinimoAtingido();

    this.selecaoOfertaDataService.botaoSelecionarOfertaAcionado = false;
  }

  readOnlyTaxasPorOferta(taxa: TaxasOferta): boolean {
    const result =
      (taxa.numeroMaximo != null &&
        taxa.numeroMinimo != null &&
        taxa.numeroMaximo == taxa.numeroMinimo) ||
      this.selecaoOfertaDataService.$selecaoOferta().possuiBoletoPago;
    return result;
  }

  readOnlyTaxasPorInscricao(taxa: any): boolean {
    const result =
      (taxa.numeroMaximo != null &&
        taxa.numeroMinimo != null &&
        taxa.numeroMaximo == taxa.numeroMinimo) ||
      this.selecaoOfertaDataService.$selecaoOferta().possuiBoletoPago;
    return result;
  }

  readOnlyTaxasGrupo() {
    return this.selecaoOfertaDataService.$selecaoOferta().possuiBoletoPago;
  }

  obterMensagemOrientacaoTaxasTag(
    grupo: GrupoTaxa,
    seqOferta: number | null | undefined,
  ): string | null {
    try {
      const cssClass = 'limite-taxa'; // Classe CSS definida para o span

      let taxas: Taxa[] = [];

      taxas = grupo.taxas;

      if (seqOferta) {
        taxas = grupo.taxas.filter((s) => s.seqOferta === seqOferta);
      }

      const somaTotalItens = taxas.reduce(
        (s, t) => s + (t.numeroItens || 0),
        0,
      );

      const maximoAtingido =
        grupo.numeroMaximoItens !== null &&
        somaTotalItens >= grupo.numeroMaximoItens;

      const minimoAtingido =
        grupo.numeroMinimoItens !== null &&
        somaTotalItens >= grupo.numeroMinimoItens;

      // Regra 1: Validação da quantidade máxima atingida (maior prioridade)
      if (maximoAtingido) {
        return `<span class="${cssClass}">Quantidade máxima atingida</span>`;
      }

      // Regra 2: Continua apenas se o número mínimo ainda não foi atingido
      if (!minimoAtingido) {
        // Cenário: Mínimo é IGUAL ao máximo
        if (grupo.numeroMinimoItens === grupo.numeroMaximoItens) {
          switch (grupo.numeroMinimoItens) {
            case 0:
              return null; // Não exibir a tag
            case 1:
              return `<span class="${cssClass}">Informe apenas um item</span>`;
            default: // maior que 1
              return `<span class="${cssClass}">Informe ${grupo.numeroMinimoItens} itens</span>`;
          }
        }

        // Cenário: Mínimo é DIFERENTE do máximo
        if (grupo.numeroMinimoItens !== grupo.numeroMaximoItens) {
          switch (grupo.numeroMinimoItens) {
            case 0:
              return null; // Não exibir a tag
            case 1:
              return `<span class="${cssClass}">Informe ao menos um item</span>`;
            default: // maior que 1
              return `<span class="${cssClass}">Informe ao menos ${grupo.numeroMinimoItens} itens</span>`;
          }
        }
      }
      return null;
    } catch {
      return null;
    }
  }
}
