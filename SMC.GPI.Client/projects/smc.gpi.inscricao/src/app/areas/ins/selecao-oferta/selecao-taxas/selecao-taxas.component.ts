import { Component, effect, inject, OnDestroy, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { DomSanitizer } from '@angular/platform-browser';
import { AccordionModule } from 'primeng/accordion';
import { InputNumber } from 'primeng/inputnumber';
import { ListboxModule } from 'primeng/listbox';
import { OrderListModule } from 'primeng/orderlist';
import { SkeletonModule } from 'primeng/skeleton';
import { Subscription } from 'rxjs';
import { ToolsService } from '../../../../../../../shared/tools.service';
import { LayoutProcessoDataService } from '../../../../core/layout-processo/service/layout-processo-data.service';
import { ListaOfertaModel } from '../model/lista-oferta.model';
import { SelecaoOfertaDataService } from '../service/selecao-oferta-data.service';
import { SelecaoOfertaService } from '../service/selecao-oferta.service';
import { TipoCobranca } from './../model/enums/tipo-cobranca.enum';
import { Taxa } from './../model/selecao-oferta.model';

export interface TaxasSelecioandas {
  seqOferta: number;
  descricaoOferta: string;
  numeroOpcao: number;
  taxasOferta: TaxasOferta[];
}

export interface TaxasOferta {
  descricaoTaxa: string;
  valorTaxa: string;
  seqTaxa: number;
  numeroMaximo: number | null;
  numeroMinimo: number | null;
  valorItem: number | null;
  valorTotalTaxa: number;
  numeroItens: number | null;
}

export interface IGrupoTaxa {
  seq: number;
  descricao: string;
  numeroMinimoItens: number;
  numeroMaximoItens: number | null;
  numeroItens: number;
  taxas: Taxa[];
}
export interface GrupoTaxaPorTipoCobranca {
  grupoTaxaOferta: IGrupoTaxa[];
  grupoTaxaQuantidadeOferta: IGrupoTaxa[];
  grupoTaxaInscricao: IGrupoTaxa[];
}

@Component({
  imports: [
    SkeletonModule,
    AccordionModule,
    ListboxModule,
    InputNumber,
    OrderListModule,
    FormsModule,
  ],
  selector: 'app-selecaoTaxas',
  standalone: true,
  templateUrl: './selecao-taxas.component.html',
})
export class SelecaoTaxasComponent implements OnDestroy {
  isLoading = signal(false);
  totalGeral = signal(0);
  instrucoes = signal<any>(null);
  existeTaxa = signal(false);
  grupoTaxa = signal<GrupoTaxaPorTipoCobranca>({} as GrupoTaxaPorTipoCobranca);
  private subs: Subscription = new Subscription();
  taxas: Taxa[] = [];

  taxasPorOferta: TaxasSelecioandas[] = [];
  taxasPorQuantidadeOferta: TaxasSelecioandas[] = [];
  taxasPorInscircao: TaxasSelecioandas[] = [];
  listaOfertas: ListaOfertaModel[] = [];

  //injeção de dependecia
  selecaoOfertaDataService = inject(SelecaoOfertaDataService);
  toolsService = inject(ToolsService);
  sanitizer = inject(DomSanitizer);
  layoutProcesso = inject(LayoutProcessoDataService);
  selecaoOfertaService = inject(SelecaoOfertaService);

  constructor() {
    effect(() => {
      const ofertas = this.selecaoOfertaDataService.$listaOfertas();

      this.selecaoOfertaService.validarLiberarBotaoProximo();

      this.listaOfertas = ofertas;

      this.listaOfertas.sort(
        (a, b) => a.oferta.numeroOpcao - b.oferta.numeroOpcao,
      );

      this.taxas = this.selecaoOfertaDataService.$selecaoOferta().taxas;

      this.montarGrupoTaxas();
      this.montarTaxasPorOferta();
      this.montarTaxasPorInscricao();
      this.montarTaxasPorQuantidadeOferta();

      this.existeTaxa.set(this.taxas.length > 0);

      this.totalGeral.set(
        this.selecaoOfertaDataService.$selecaoOferta().totalGeral,
      );

      this.calcularTaxas();
      this.instrucoes.set(
        this.selecaoOfertaDataService.$selecaoOferta().instrucaoTaxa,
      );
      this.isLoading.set(false);
    });

    effect(() => {
      const atualizarLista =
        this.selecaoOfertaDataService.$carregarListaOferta();
      this.isLoading.set(atualizarLista);
    });

    effect(() => {
      const ordernarLista = this.selecaoOfertaDataService.$ordernarLista();
      if (ordernarLista) {
        this.ordenarLista();
      }
    });
  }

  montarGrupoTaxas() {
    let gruposTaxas: GrupoTaxaPorTipoCobranca = {
      grupoTaxaInscricao: [],
      grupoTaxaOferta: [],
      grupoTaxaQuantidadeOferta: [],
    };

    for (const grupo of this.selecaoOfertaDataService.$selecaoOferta()
      .gruposTaxa) {
      let seqTaxas: number[];
      let taxas: Taxa[];

      switch (grupo.itens[0].taxa.tipoCobranca) {
        case TipoCobranca.porQuantidadeOfertas:
          seqTaxas = grupo.itens.map((f) => f.seqTaxa);
          taxas = this.taxas.filter((f) => seqTaxas.includes(f.seqTaxa));

          gruposTaxas.grupoTaxaQuantidadeOferta.push({
            seq: grupo.seq,
            descricao: grupo.descricao,
            numeroMaximoItens: grupo.numeroMaximoItens,
            numeroMinimoItens: grupo.numeroMinimoItens,
            numeroItens: grupo.numeroMinimoItens,
            taxas: taxas.filter((atual, indice, array) => {
              atual.numeroItens = grupo.numeroMinimoItens;
              return (
                indice === array.findIndex((o) => o.seqTaxa === atual.seqTaxa)
              );
            }),
          } as IGrupoTaxa);

          break;
        case TipoCobranca.porOferta:
          seqTaxas = grupo.itens.map((f) => f.seqTaxa);
          taxas = this.taxas.filter((f) => seqTaxas.includes(f.seqTaxa));

          gruposTaxas.grupoTaxaOferta.push({
            seq: grupo.seq,
            descricao: grupo.descricao,
            numeroMaximoItens: grupo.numeroMaximoItens,
            numeroMinimoItens: grupo.numeroMinimoItens,
            numeroItens: grupo.numeroMinimoItens,
            taxas: taxas.filter((atual, indice, array) => {
              atual.numeroItens = grupo.numeroMinimoItens;
              return (
                indice ===
                array.findIndex(
                  (o) =>
                    o.seqTaxa === atual.seqTaxa &&
                    o.seqOferta === atual.seqOferta,
                )
              );
            }),
          } as IGrupoTaxa);

          break;
        default:
          seqTaxas = grupo.itens.map((f) => f.seqTaxa);
          taxas = this.taxas.filter((f) => seqTaxas.includes(f.seqTaxa));

          gruposTaxas.grupoTaxaInscricao.push({
            seq: grupo.seq,
            descricao: grupo.descricao,
            numeroMaximoItens: grupo.numeroMaximoItens,
            numeroMinimoItens: grupo.numeroMinimoItens,
            numeroItens: grupo.numeroMinimoItens,
            taxas: taxas.filter((atual, indice, array) => {
              atual.numeroItens = grupo.numeroMinimoItens;
              return (
                indice === array.findIndex((o) => o.seqTaxa === atual.seqTaxa)
              );
            }),
          } as IGrupoTaxa);

          break;
      }
    }

    this.grupoTaxa.set(gruposTaxas);
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ordenarLista() {
    this.taxasPorOferta.forEach((f) => {
      const oferta = this.selecaoOfertaDataService
        .$selecaoOferta()
        .ofertas.find((oferta) => oferta.seqOferta.seq === f.seqOferta);
      f.numeroOpcao = oferta?.numeroOpcao!;
    });

    this.taxasPorOferta.sort((a, b) => a.numeroOpcao - b.numeroOpcao);

    this.taxasPorInscircao.forEach((f) => {
      const oferta = this.selecaoOfertaDataService
        .$selecaoOferta()
        .ofertas.find((oferta) => oferta.seqOferta.seq === f.seqOferta);
      f.numeroOpcao = oferta?.numeroOpcao!;
    });

    this.taxasPorInscircao.sort((a, b) => a.numeroOpcao - b.numeroOpcao);
  }

  numeroItens(
    event: any,
    seqTaxa: number,
    seqOferta: number,
    seqGrupo: number | null = null,
  ) {
    const qntItens = event.value;
    const taxa = this.selecaoOfertaDataService
      .$selecaoOferta()
      .taxas.find((f) => f.seqTaxa === seqTaxa);
    let total = 0;

    if (seqGrupo) {
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
    this.selecaoOfertaDataService.$selecaoOferta().totalGeral += total;

    this.totalGeral.set(
      this.selecaoOfertaDataService.$selecaoOferta().totalGeral,
    );
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
      this.grupoTaxa().grupoTaxaInscricao.find((f) => f.seq == seqGrupo) ??
      this.grupoTaxa().grupoTaxaOferta.find((f) => f.seq == seqGrupo);

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

    total += this.grupoTaxa().grupoTaxaInscricao.reduce((acumulador, grupo) => {
      grupo.taxas.forEach((taxa) => {
        taxa.valorTotalTaxa = taxa.valorItem! * (taxa.numeroItens ?? 0);
        acumulador += taxa.valorItem!;
      });
      return acumulador;
    }, 0);

    total += this.grupoTaxa().grupoTaxaOferta.reduce((acumulador, grupo) => {
      grupo.taxas.forEach((taxa) => {
        taxa.valorTotalTaxa = taxa.valorItem! * (taxa.numeroItens ?? 0);
        acumulador += taxa.valorItem!;
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
  ): number {
    if (taxa?.tipoCobranca === TipoCobranca.porInscricao) {
      this.taxasPorInscircao[0].taxasOferta.forEach((taxa) => {
        if (taxa.seqTaxa === seqTaxa && taxa) {
          taxa.numeroItens = qntItens;
          taxa.valorTotalTaxa = taxa.valorItem! * qntItens;
          total += taxa.valorItem!;
        }
      });
    }

    if (taxa?.tipoCobranca === TipoCobranca.porOferta) {
      this.taxasPorOferta
        .filter((f) => f.seqOferta == seqOferta)
        .forEach((taxas) => {
          taxas.taxasOferta.forEach((taxa) => {
            if (taxa.seqTaxa === seqTaxa) {
              taxa.numeroItens = qntItens;
              taxa.valorTotalTaxa = taxa.valorItem! * qntItens;
              total += taxa.valorItem!;
            }
          });
        });
    }

    if (taxa?.tipoCobranca === TipoCobranca.porQuantidadeOfertas) {
      this.taxasPorQuantidadeOferta.forEach((taxas) => {
        taxas.taxasOferta.forEach((taxa) => {
          if (taxa.seqTaxa === seqTaxa) {
            taxa.numeroItens = qntItens;
            taxa.valorTotalTaxa = taxa.valorItem! * qntItens;
            total += taxa.valorTotalTaxa;
          }
        });
      });
    }

    return total;
  }

  calcularTaxas() {
    const taxas = this.selecaoOfertaDataService.$selecaoOferta().taxas;
    let total = 0;
    this.taxasPorInscircao.forEach((taxas) => {
      taxas.taxasOferta.forEach((taxa) => {
        total += taxa.valorTotalTaxa;
      });
    });

    if (this.taxasPorQuantidadeOferta.length > 0) {
      total +=
        this.taxasPorQuantidadeOferta[0].taxasOferta[0].valorItem! *
        this.selecaoOfertaDataService.$selecaoOferta().ofertas.length;
    }

    this.taxasPorOferta.forEach((taxas) => {
      taxas.taxasOferta.forEach((taxa) => {
        total += taxa.valorTotalTaxa;
      });
    });
    total += this.calcularTaxasComGrupoSemSeq();
    this.totalGeral.set(total);
    this.selecaoOfertaDataService.$selecaoOferta().totalGeral = total;
  }

  montarIstrucoesTaxas() {
    // Decodifica o html das instrucoes
    const instrucao = this.selecaoOfertaDataService
      .$selecaoOferta()
      .secoes.find((f: any) => f.token == 'INSTRUCOES_TAXA').texto;
    if (instrucao) {
      const instrucoesDecode = this.toolsService.decodeHtml(instrucao);
      this.instrucoes.set(
        this.sanitizer.bypassSecurityTrustHtml(instrucoesDecode),
      );
    }
  }

  montarTaxasPorOferta() {
    const taxasPorOfertaMap = new Map<number, TaxasSelecioandas>();

    this.taxas.forEach((taxa) => {
      if (taxa.tipoCobranca === TipoCobranca.porOferta) {
        let item = taxasPorOfertaMap.get(taxa.seqOferta);

        if (!item) {
          const descricaoOferta =
            this.listaOfertas.find((f) => f.hierquia?.seq === taxa.seqOferta)
              ?.hierquia?.descricao || '';
          const numeroOrdem =
            this.listaOfertas.find((f) => f.hierquia?.seq === taxa.seqOferta)
              ?.oferta.numeroOpcao || '';
          item = {
            seqOferta: taxa.seqOferta,
            descricaoOferta: descricaoOferta,
            numeroOpcao: +numeroOrdem!,
            taxasOferta: [],
          };

          taxasPorOfertaMap.set(taxa.seqOferta, item);

          if (this.grupoTaxa().grupoTaxaOferta.length > 0) {
            return;
          }

          item.taxasOferta.push({
            descricaoTaxa: taxa.descricao,
            valorTaxa: taxa.valorItem.toString(),
            seqTaxa: taxa.seqTaxa,
            valorItem: taxa.valorItem,
            numeroMinimo: taxa.numeroMinimo,
            numeroMaximo: taxa.numeroMaximo,
            valorTotalTaxa: taxa.valorTotalTaxa,
            numeroItens: taxa.numeroItens,
          });
        } else {
          if (taxa.possuiGrupoTaxas) {
            return;
          }

          item.taxasOferta.push({
            descricaoTaxa: taxa.descricao,
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

    this.taxasPorOferta = Array.from(taxasPorOfertaMap.values());
    this.taxasPorOferta.sort((a, b) => a.numeroOpcao - b.numeroOpcao);
    console.log(this.taxasPorOferta);
  }

  montarTaxasPorQuantidadeOferta() {
    const taxasPorQuantidadeOferta = new Map<number, TaxasSelecioandas>();

    this.taxas.forEach((taxa) => {
      if (taxa.tipoCobranca === TipoCobranca.porQuantidadeOfertas) {
        let item = taxasPorQuantidadeOferta.get(taxa.seqOferta);

        if (!item) {
          const descricaoOferta =
            this.listaOfertas.find((f) => f.hierquia?.seq === taxa.seqOferta)
              ?.hierquia?.descricao || '';
          const numeroOrdem =
            this.listaOfertas.find((f) => f.hierquia?.seq === taxa.seqOferta)
              ?.oferta.numeroOpcao || '';
          item = {
            seqOferta: taxa.seqOferta,
            descricaoOferta: descricaoOferta,
            numeroOpcao: +numeroOrdem!,
            taxasOferta: [],
          };
          taxasPorQuantidadeOferta.set(taxa.seqOferta, item);

          if (this.grupoTaxa().grupoTaxaQuantidadeOferta.length > 0) {
            return;
          }
        }

        item.taxasOferta.push({
          descricaoTaxa: taxa.descricao,
          valorTaxa: taxa.valorItem.toString(),
          seqTaxa: taxa.seqTaxa,
          valorItem: taxa.valorItem,
          numeroMinimo: taxa.numeroMinimo,
          numeroMaximo: taxa.numeroMaximo,
          valorTotalTaxa:
            taxa.valorItem *
            this.selecaoOfertaDataService.$selecaoOferta().ofertas.length,
          numeroItens:
            this.selecaoOfertaDataService.$selecaoOferta().ofertas.length,
        });
      }
    });

    this.taxasPorQuantidadeOferta = Array.from(
      taxasPorQuantidadeOferta.values(),
    );
    this.taxasPorQuantidadeOferta.sort((a, b) => a.numeroOpcao - b.numeroOpcao);
    console.log(this.taxasPorQuantidadeOferta);
  }

  montarTaxasPorInscricao() {
    const taxasPorInscircao = new Map<number, TaxasSelecioandas>();

    this.taxas.forEach((taxa) => {
      if (taxa.tipoCobranca === TipoCobranca.porInscricao) {
        let item = taxasPorInscircao.get(taxa.seqOferta);

        if (!item) {
          const descricaoOferta =
            this.listaOfertas.find((f) => f.hierquia?.seq === taxa.seqOferta)
              ?.hierquia?.descricao || '';
          const numeroOrdem =
            this.listaOfertas.find((f) => f.hierquia?.seq === taxa.seqOferta)
              ?.oferta.numeroOpcao || '';
          item = {
            seqOferta: taxa.seqOferta,
            descricaoOferta: descricaoOferta,
            numeroOpcao: +numeroOrdem!,
            taxasOferta: [],
          };
          taxasPorInscircao.set(taxa.seqOferta, item);

          if (this.grupoTaxa().grupoTaxaInscricao.length > 0) {
            return;
          }
        }

        item.taxasOferta.push({
          descricaoTaxa: taxa.descricao,
          valorTaxa: taxa.valorItem.toString(),
          seqTaxa: taxa.seqTaxa,
          valorItem: taxa.valorItem,
          numeroMinimo: taxa.numeroMinimo,
          numeroMaximo: taxa.numeroMaximo,
          valorTotalTaxa: taxa.valorTotalTaxa,
          numeroItens: taxa.numeroItens,
        });
      }
    });

    this.taxasPorInscircao = Array.from(taxasPorInscircao.values());
    this.taxasPorInscircao.sort((a, b) => a.numeroOpcao - b.numeroOpcao);
    console.log(this.taxasPorInscircao);
  }

  getMaxPermitido(grupo: IGrupoTaxa, taxaAtual: Taxa): number | null {
    // soma só as demais taxas
    const somaOutras = grupo.taxas
      .filter((t) => t.seqTaxa !== taxaAtual.seqTaxa)
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

    return grupo.numeroMaximoItens;
  }
  bloquearInserirDadosPorTeclado(event: Event) {
    event.preventDefault();
    const inputElement = event.target as HTMLInputElement;
    inputElement.blur();
  }

  bloquearInserirDadosPorTouch(event: Event) {
    event.preventDefault();
    const inputElement = event.target as HTMLInputElement;
    inputElement.blur();
  }

  formartarTotal(value:number | null){
    return this.toolsService.currencyMask(value);
  }
}
