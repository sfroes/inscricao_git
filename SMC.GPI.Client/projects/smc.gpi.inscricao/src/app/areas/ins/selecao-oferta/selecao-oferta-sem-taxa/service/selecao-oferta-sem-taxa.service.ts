import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { inject, Injectable, signal } from '@angular/core';
import { ListaOfertaModel } from '../../model/lista-oferta.model';
import { SelecaoOfertaModel } from '../../model/selecao-oferta.model';
import { SelecaoOfertaDataService } from '../../service/selecao-oferta-data.service';
import { SelecaoOfertaService } from '../../service/selecao-oferta.service';
import { LayoutProcessoDataService } from './../../../../../core/layout-processo/service/layout-processo-data.service';

@Injectable({
  providedIn: 'root',
})
export class SelecaoOfertaSemTaxaService {
  //#region variaveis
  selecaoOferta: SelecaoOfertaModel = {} as SelecaoOfertaModel;
  ofertaDesativada: string = 'smc-gpi-listbox-item-desativado';
  itemUnico: string = 'smc-gpi-listbox-item-unico';
  numeroNoFolha: number = 0;
  timeoutDragdrop: any;
  //#endregion

  //#region signals
  $listaOferta = signal<ListaOfertaModel[]>([]);
  $isLoading = signal(true);
  $desabilitarDragDrop = signal(false);
  //#endregion

  //#region injeção de dependencia
  selecaoOfertaDataService = inject(SelecaoOfertaDataService);
  selecaoOfertaService = inject(SelecaoOfertaService);
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
    //Atualiza a lista de ofertas
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

    this.selecaoOfertaDataService.setListaOfertas(this.$listaOferta());
    this.organizarLista();
    this.selecaoOfertaService.validarLiberarBotaoProximo();
  }

  organizarLista() {
    this.$listaOferta().forEach((item, index) => {
      item.oferta.numeroOpcao = index + 1;
    });
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
}
