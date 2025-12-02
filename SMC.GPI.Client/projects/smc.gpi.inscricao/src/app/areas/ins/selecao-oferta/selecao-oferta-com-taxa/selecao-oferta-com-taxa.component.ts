import { CdkAccordionModule } from '@angular/cdk/accordion';
import { CdkDragDrop, DragDropModule } from '@angular/cdk/drag-drop';
import { CommonModule } from '@angular/common';
import {
  Component,
  effect,
  inject,
  signal,
  WritableSignal,
} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccordionModule } from 'primeng/accordion';
import { ButtonModule } from 'primeng/button';
import { InputNumberModule } from 'primeng/inputnumber';
import { OrderListModule } from 'primeng/orderlist';
import { SelectModule } from 'primeng/select';
import { Skeleton } from 'primeng/skeleton';
import { TagModule } from 'primeng/tag';
import { ToolsService } from '../../../../../../../shared/tools.service';
import { TaxasOferta } from '../interfaces/taxas-oferta.interface';
import { ListaOfertaModel } from '../model/lista-oferta.model';
import { SelecaoOfertaDataService } from '../service/selecao-oferta-data.service';
import { SelecaoOfertaService } from '../service/selecao-oferta.service';
import { SelecaoOfertaComTaxaGrupoComponent } from './selecao-oferta-com-taxa-grupo/selecao-oferta-com-taxa-grupo.component';
import { SelecaoOfertaComTaxaSimplesComponent } from './selecao-oferta-com-taxa-simples/selecao-oferta-com-taxa-simples.component';
import { SelecaoOfertaComTaxaService } from './service/selecao-oferta-com-taxa.service';

@Component({
  selector: 'app-selecao-oferta-com-taxa',
  imports: [
    TagModule,
    OrderListModule,
    ButtonModule,
    Skeleton,
    SelectModule,
    FormsModule,
    DragDropModule,
    CdkAccordionModule,
    AccordionModule,
    InputNumberModule,
    SelecaoOfertaComTaxaGrupoComponent,
    SelecaoOfertaComTaxaSimplesComponent,
    CommonModule,
  ],
  templateUrl: './selecao-oferta-com-taxa.component.html',
})
export class SelecaoOfertaComTaxaComponent {
  opcoesNumeroMaximoConvocacaoPorInscricao: number[] = [];
  selecaoOfertaComTaxaService = inject(SelecaoOfertaComTaxaService);
  selecaoOfertaDataService = inject(SelecaoOfertaDataService);
  selecaoOfertaService = inject(SelecaoOfertaService);

  toolsService = inject(ToolsService);
  eventoMostrarOcultarTaxas = signal<string>('');
  houveCliqueOrdenar = signal<boolean>(false);

  private taxaSignalsCache = new Map<number, WritableSignal<TaxasOferta>>();

  constructor() {
    effect(() => {
      const listaOfertas = this.selecaoOfertaDataService.$listaOfertas();
      this.selecaoOfertaComTaxaService.$listaOferta.set(listaOfertas);
      this.selecaoOfertaComTaxaService.selecaoOferta =
        this.selecaoOfertaDataService.$selecaoOferta();

      const todasOfertasJaCarregadas =
        this.selecaoOfertaDataService.$selecaoOferta().ofertas;

      const idsOfertasJaCarregadas = new Set(
        todasOfertasJaCarregadas.map((oferta) => oferta.seqOferta.seq ?? 0),
      );

      this.selecaoOfertaComTaxaService.selecaoOferta.ofertas.forEach(
        (oferta, index) => {
          oferta.numeroOpcao = index + 1;
          if (
            (oferta.seqOferta.seq &&
              !idsOfertasJaCarregadas.has(oferta.seqOferta.seq)) ||
            this.selecaoOfertaComTaxaService.numeroNoFolha == 1
          ) {
            this.selecaoOfertaComTaxaService.selecaoOfertaService
              .listarTaxasOfertaInscricaoAngular(
                '/INS/Inscricao/ListarTaxasOfertaInscricaoAngular',
                oferta.seqOferta.seq!,
                this.selecaoOfertaComTaxaService.selecaoOferta.seqInscricao,
              )
              .then((taxas) => {
                taxas.forEach((novaTaxa) => {
                  const index =
                    this.selecaoOfertaComTaxaService.selecaoOferta.taxas.findIndex(
                      (t) => t.seqTaxa === novaTaxa.seqTaxa,
                    );

                  if (index === -1) {
                    // Adiciona a nova taxa se não existir
                    this.selecaoOfertaComTaxaService.selecaoOferta.taxas.push(
                      novaTaxa,
                    );
                  }

                  if (this.selecaoOfertaComTaxaService.numeroNoFolha == 1) {
                    this.selecaoOfertaComTaxaService.$isLoading.set(true);
                    this.selecaoOfertaComTaxaService.organizarLista();
                    this.selecaoOfertaComTaxaService.taxas =
                      this.selecaoOfertaDataService.$selecaoOferta().taxas;
                    this.selecaoOfertaComTaxaService.montarGrupoTaxas();
                    this.selecaoOfertaComTaxaService.montarTaxasPorOferta();
                    this.selecaoOfertaComTaxaService.montarTaxasPorInscricao();
                    this.selecaoOfertaComTaxaService.montarTaxasPorQuantidadeOferta();
                    this.selecaoOfertaComTaxaService.montarIstrucoesTaxas();
                    this.selecaoOfertaComTaxaService.calcularTaxas();
                    this.selecaoOfertaComTaxaService.$isLoading.set(false);
                  }
                });
              });
          }
        },
      );
    });

    effect(() => {
      const carregar = this.selecaoOfertaDataService.$carregarListaOferta();
      this.selecaoOfertaComTaxaService.$isLoading.set(carregar);
    });

    effect(() => {
      const numeroNoFolha = this.selecaoOfertaDataService.$numeroNoFolha();
      this.selecaoOfertaComTaxaService.numeroNoFolha = numeroNoFolha;
      if (
        numeroNoFolha == 1 ||
        this.selecaoOfertaDataService.$selecaoOferta()
          .numeroMaximoOfertaPorInscricao == 1
      ) {
        this.selecaoOfertaComTaxaService.$desabilitarDragDrop.set(true);
      }
    });

    effect(
      () => {
        this.selecaoOfertaDataService.$listaOfertas();

        // this.selecaoOfertaComTaxaService.listaOfertas = subTaxas;

        // this.selecaoOfertaComTaxaService.listaOfertas.sort(
        //   (a, b) => a.oferta.numeroOpcao - b.oferta.numeroOpcao,
        // );
        this.selecaoOfertaComTaxaService.organizarLista();
        this.selecaoOfertaComTaxaService.taxas =
          this.selecaoOfertaDataService.$selecaoOferta().taxas;
        this.selecaoOfertaComTaxaService.montarGrupoTaxas();
        this.selecaoOfertaComTaxaService.montarTaxasPorOferta();
        this.selecaoOfertaComTaxaService.montarTaxasPorInscricao();
        this.selecaoOfertaComTaxaService.montarTaxasPorQuantidadeOferta();
        this.selecaoOfertaComTaxaService.montarIstrucoesTaxas();
        this.selecaoOfertaComTaxaService.calcularTaxas();
        this.selecaoOfertaComTaxaService.$isLoading.set(false);
      },
      { allowSignalWrites: true },
    );

    effect(() => {
      const atualizarLista =
        this.selecaoOfertaDataService.$carregarListaOferta();
      this.selecaoOfertaComTaxaService.$isLoading.set(atualizarLista);
    });

    effect(() => {
      const ordernarLista = this.selecaoOfertaDataService.$ordernarLista();
      if (ordernarLista) {
        this.selecaoOfertaComTaxaService.organizarLista();
      }
    });

    document.addEventListener('mouseup', () => {
      this.validarHouveCliqueOrdernar();
    });
  }

  ngAfertContentChecked(): void {
    this.selecaoOfertaService.validarLiberarBotaoProximo();
  }

  ordernar(event: CdkDragDrop<ListaOfertaModel[]>) {
    this.selecaoOfertaComTaxaService.ordernar(event);
  }

  apagarOferta(numeroOpcao: number) {
    this.selecaoOfertaComTaxaService.apagarOferta(numeroOpcao);
  }

  atulizarTextoJustificativa(event: any, numeroOpcao: number) {
    this.selecaoOfertaComTaxaService.atulizarTextoJustificativa(
      event,
      numeroOpcao,
    );
  }

  opcoesParaConvocacao(): number[] {
    return this.selecaoOfertaComTaxaService.opcoesParaConvocacao();
  }

  opcaoSelecionadaConvocacao(event: any) {
    this.selecaoOfertaComTaxaService.opcaoSelecionadaConvocacao(event);
    this.selecaoOfertaService.validarLiberarBotaoProximo();
  }

  verificaOfertaAtiva(listaOferta: ListaOfertaModel): string {
    return this.selecaoOfertaComTaxaService.verificaOfertaAtiva(listaOferta);
  }

  adcionarClasseMobile(event: any) {
    this.selecaoOfertaComTaxaService.adcionarClasseMobile(event);
  }

  removeClasseMobile(event: any) {
    this.selecaoOfertaComTaxaService.removeClasseMobile(event);
  }

  removeClasseDesktop() {
    this.selecaoOfertaComTaxaService.removeClasseDesktop();
    this.houveCliqueOrdenar.set(false);
  }

  adcionarClasseDesktop(event: any) {
    this.selecaoOfertaComTaxaService.adcionarClasseDesktop(event);
    this.houveCliqueOrdenar.set(true);
  }

  taxasFiltradas(seqOferta: number) {
    const retorno = this.selecaoOfertaComTaxaService
      .$taxasPorOferta()
      .filter((f) => f.seqOferta === seqOferta);
    return retorno;
  }

  ocultarTaxasOferta() {
    this.eventoMostrarOcultarTaxas.set('smc-gpi-esconde-oferta-content');
  }

  mostrarTaxasOferta() {
    this.eventoMostrarOcultarTaxas.set('');
  }
  validarHouveCliqueOrdernar() {
    if (this.houveCliqueOrdenar()) {
      this.houveCliqueOrdenar.set(false);
      this.removeClasseDesktop();
    }
  }

  teste(taxa: any[]) {
    return taxa;
  }

  moverOfertasDescktop(): void {
    const elemento = document.getElementById('ancora-desktop');
    if (elemento) {
      elemento.scrollIntoView({ behavior: 'smooth' });
    }
  }

  moverOfertasMobile(): void {
    const elemento = document.getElementById('ancora-mobile');
    if (elemento) {
      elemento.scrollIntoView({
        behavior: 'smooth',
        block: 'start',
        inline: 'nearest',
      });
    }
  }

  ajustarOffset(event: any) {
    // Espera um pequeno tempo para o Angular criar o elemento .cdk-drag-preview
    setTimeout(() => {
      const previewEl = document.querySelector(
        '.cdk-drag-preview',
      ) as HTMLElement;

      if (previewEl) {
        const largura = previewEl.offsetWidth;

        // Exemplo: centralizar o clique horizontalmente
        const offsetX = largura / 2;

        const dragRef = (event.source as any)._dragRef;
        dragRef._pickupPositionInElement = {
          x: offsetX,
          y: 0,
        };
      }
    }, 0); // Delay mínimo para garantir que o preview foi renderizado
  }

  formartarTotal(value: number | null) {
    return this.toolsService.currencyMask(value);
  }
}
