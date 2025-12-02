import { CdkDragDrop, DragDropModule } from '@angular/cdk/drag-drop';
import { Component, effect, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccordionModule } from 'primeng/accordion';
import { ButtonModule } from 'primeng/button';
import { InputNumberModule } from 'primeng/inputnumber';
import { OrderListModule } from 'primeng/orderlist';
import { SelectModule } from 'primeng/select';
import { Skeleton } from 'primeng/skeleton';
import { TagModule } from 'primeng/tag';
import { ListaOfertaModel } from '../model/lista-oferta.model';
import { SelecaoOfertaDataService } from '../service/selecao-oferta-data.service';
import { SelecaoOfertaService } from '../service/selecao-oferta.service';
import { SelecaoOfertaSemTaxaService } from './service/selecao-oferta-sem-taxa.service';

@Component({
  selector: 'app-selecao-oferta-sem-taxa',
  imports: [
    TagModule,
    OrderListModule,
    ButtonModule,
    Skeleton,
    SelectModule,
    FormsModule,
    DragDropModule,
    AccordionModule,
    InputNumberModule,
  ],
  templateUrl: './selecao-oferta-sem-taxa.component.html',
})
export class SelecaoOfertaSemTaxaComponent {
  selecaoOfertaDataService = inject(SelecaoOfertaDataService);
  selecaoOfertaSemTaxaService = inject(SelecaoOfertaSemTaxaService);
  selecaoOfertaService = inject(SelecaoOfertaService);
  houveCliqueOrdenar = signal<boolean>(false);

  constructor() {
    effect(() => {
      const listaOfertas = this.selecaoOfertaDataService.$listaOfertas();

      this.selecaoOfertaSemTaxaService.$listaOferta.set(listaOfertas);
      this.selecaoOfertaSemTaxaService.selecaoOferta =
        this.selecaoOfertaDataService.$selecaoOferta();

      this.selecaoOfertaSemTaxaService.selecaoOferta.ofertas?.forEach(
        (oferta, index) => {
          oferta.numeroOpcao = index + 1;
          if (oferta.seqOferta.seq) {
            this.selecaoOfertaSemTaxaService.selecaoOfertaService
              .listarTaxasOfertaInscricaoAngular(
                '/INS/Inscricao/ListarTaxasOfertaInscricaoAngular',
                oferta.seqOferta.seq!,
                this.selecaoOfertaSemTaxaService.selecaoOferta.seqInscricao,
              )
              .then((taxas) => {
                taxas.forEach((novaTaxa) => {
                  const index =
                    this.selecaoOfertaSemTaxaService.selecaoOferta.taxas.findIndex(
                      (t) => t.seqTaxa === novaTaxa.seqTaxa,
                    );

                  if (index === -1) {
                    // Adiciona a nova taxa se não existir
                    this.selecaoOfertaSemTaxaService.selecaoOferta.taxas.push(
                      novaTaxa,
                    );
                  }
                });
              });
          }
        },
      );
    });

    effect(() => {
      const carregar = this.selecaoOfertaDataService.$carregarListaOferta();
      this.selecaoOfertaSemTaxaService.$isLoading.set(carregar);
    });

    effect(() => {
      const numeroNoFolha = this.selecaoOfertaDataService.$numeroNoFolha();
      this.selecaoOfertaSemTaxaService.numeroNoFolha = numeroNoFolha;
      if (
        numeroNoFolha == 1 ||
        this.selecaoOfertaDataService.$selecaoOferta()
          .numeroMaximoOfertaPorInscricao == 1
      ) {
        this.selecaoOfertaSemTaxaService.$desabilitarDragDrop.set(true);
      }
    });

    document.addEventListener('mouseup', () => {
      this.validarHouveCliqueOrdernar();
    });
  }
  opcoesNumeroMaximoConvocacaoPorInscricao: number[] = [];

  ordernar(event: CdkDragDrop<ListaOfertaModel[]>) {
    this.selecaoOfertaSemTaxaService.ordernar(event);
  }

  apagarOferta(numeroOpcao: number) {
    this.selecaoOfertaSemTaxaService.apagarOferta(numeroOpcao);
  }

  atulizarTextoJustificativa(event: any, numeroOpcao: number) {
    this.selecaoOfertaSemTaxaService.atulizarTextoJustificativa(
      event,
      numeroOpcao,
    );
  }

  opcoesParaConvocacao(): number[] {
    return this.selecaoOfertaSemTaxaService.opcoesParaConvocacao();
  }

  opcaoSelecionadaConvocacao(event: any) {
    this.selecaoOfertaSemTaxaService.opcaoSelecionadaConvocacao(event);
    this.selecaoOfertaService.validarLiberarBotaoProximo();
  }

  verificaOfertaAtiva(listaOferta: ListaOfertaModel): string {
    return this.selecaoOfertaSemTaxaService.verificaOfertaAtiva(listaOferta);
  }

  adcionarClasseMobile(event: any) {
    this.selecaoOfertaSemTaxaService.adcionarClasseMobile(event);
  }

  removeClasseMobile(event: any) {
    this.selecaoOfertaSemTaxaService.removeClasseMobile(event);
  }

  removeClasseDesktop() {
    this.selecaoOfertaSemTaxaService.removeClasseDesktop();
    this.houveCliqueOrdenar.set(false);
  }

  adcionarClasseDesktop(event: any) {
    this.selecaoOfertaSemTaxaService.adcionarClasseDesktop(event);
    this.houveCliqueOrdenar.set(true);
  }

  validarHouveCliqueOrdernar() {
    if (this.houveCliqueOrdenar()) {
      this.houveCliqueOrdenar.set(false);
      this.removeClasseDesktop();
    }
  }

  ajustarOffset(event: any) {
    const dragRef = (event.source as any)._dragRef;
    const root = dragRef.getRootElement();
    const rect = root.getBoundingClientRect();

    // Corrige a posição de arrasto
    dragRef._pickupPositionInElement = { x: 100, y: 0 };
  }
}
