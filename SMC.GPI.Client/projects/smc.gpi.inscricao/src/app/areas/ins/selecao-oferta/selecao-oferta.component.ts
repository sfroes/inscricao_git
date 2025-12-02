import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { CommonModule } from '@angular/common';
import { Component, effect, inject, OnDestroy, signal } from '@angular/core';
import { AccordionModule } from 'primeng/accordion';
import { ConfirmationService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { Dialog } from 'primeng/dialog';
import { SkeletonModule } from 'primeng/skeleton';
import { TagModule } from 'primeng/tag';
import { LayoutNavegacaoComponent } from '../../../core/layout-processo/componente/layout-navegacao/layout-navegacao.component';
import { LayoutProcessoDataService } from './../../../core/layout-processo/service/layout-processo-data.service';
import { SelecaoOfertaComTaxaComponent } from './selecao-oferta-com-taxa/selecao-oferta-com-taxa.component';
import { SelecaoOfertaComTaxaService } from './selecao-oferta-com-taxa/service/selecao-oferta-com-taxa.service';
import { SelecaoOfertaHierarquiaComponent } from './selecao-oferta-hierarquia/selecao-oferta-hierarquia.component';
import { SelecaoOfertaSemTaxaComponent } from './selecao-oferta-sem-taxa/selecao-oferta-sem-taxa.component';
import { SelecaoOfertaVazioComponent } from './selecao-oferta-vazio/selecao-oferta-vazio.component';
import { SelecaoOfertaDataService } from './service/selecao-oferta-data.service';
import { SelecaoOfertaService } from './service/selecao-oferta.service';

@Component({
  selector: 'app-selecao-oferta',
  imports: [
    SelecaoOfertaVazioComponent,
    SelecaoOfertaHierarquiaComponent,
    SelecaoOfertaSemTaxaComponent,
    LayoutNavegacaoComponent,
    SkeletonModule,
    CommonModule,
    Dialog,
    ButtonModule,
    TagModule,
    AccordionModule,
    SelecaoOfertaComTaxaComponent,
    ConfirmDialogModule,
  ],
  providers: [ConfirmationService],
  templateUrl: './selecao-oferta.component.html',
})
export class SelecaoOfertaComponent implements OnDestroy {
  //#region signals
  ehCelular = signal(false);
  //#endregion

  //injeção de dependências
  selecaoOfertaService = inject(SelecaoOfertaService);
  selecaoOfertaComTaxaService = inject(SelecaoOfertaComTaxaService);
  selecaoOfertaDataService = inject(SelecaoOfertaDataService);
  layoutProcessoDataService = inject(LayoutProcessoDataService);
  selecionaDataService = inject(SelecaoOfertaDataService);
  detectaDispositivo = inject(BreakpointObserver);
  confirmationService = inject(ConfirmationService);

  ngOnDestroy(): void {
    this.selecaoOfertaService.subs.unsubscribe();
  }
  constructor() {
    effect(() => {
      const botaoProximo =
        this.layoutProcessoDataService.botaoProximoAcionado$();
      if (botaoProximo) {
        this.salvarSelecaoOferta();
      }
    });

    this.buscarSelecaoOferta();

    this.detectaDispositivo.observe([Breakpoints.Handset]).subscribe((res) => {
      this.selecionaDataService.setDispositivoEhMobile(res.matches);
      this.ehCelular.set(res.matches);
    });

    effect(() => {
      const listaOferta = this.selecaoOfertaDataService.$listaOfertas();
      this.selecaoOfertaService.validarLiberarBotaoProximo();
    });
  }

  /**
   * Realização busca da seleção de oferta com base nos parametroes passados por
   * QueryString.
   *
   * @remarks
   * Os parametros passados por QueryString sao:
   * - seqConfiguracaoEtapa: sequencial da etapa da oferta
   * - idioma: idioma do candidato
   * - seqConfiguracaoEtapaPagina: sequencia da pagina da etapa da oferta
   * - seqGrupoOferta: sequencia do grupo de oferta
   * - seqInscricao: sequencia da inscricao do candidato
   */
  buscarSelecaoOferta() {
    this.selecaoOfertaService.buscarSelecaoOferta();
  }

  /**
   * Lida com a seleção de ofertas atualizando a lista de ofertas existentes com as ofertas selecionadas.
   *
   * - Remove ofertas existentes que não estão na lista de ofertas selecionadas.
   * - Adiciona ofertas selecionadas que não estão na lista de ofertas existentes.
   * - Reorganiza o número de ordenação das ofertas existentes.
   *
   * @remarks
   * Este método atualiza a propriedade `selecionaDataService.selecaoOferta.ofertas` com a nova lista de ofertas.
   * Ele também chama `listarOfertas` para atualizar a lista de ofertas e define `abrirModal` como falso para fechar o modal.
   */
  async botaoSelecionarOferta() {
    this.selecaoOfertaService.botaoSelecionarOferta();
  }

  botaoCancelarSelecionarOferta() {
    this.selecionaDataService.setBotaoCancelarHierarquia(true);
    this.selecionaDataService.setAbrirModalSelecaoOferta(false);
  }

  selecionarHierarquia() {
    this.selecionaDataService.setAtivarBotaoSelecionaModalHierarquia(true);
    this.selecionaDataService.setAbrirModalSelecaoOferta(true);
  }
  private salvarSelecaoOferta(permiteAlterarBoleto: boolean = false) {
    let retorno: boolean = permiteAlterarBoleto;
    this.selecaoOfertaService
      .salvarSelecaoOferta(permiteAlterarBoleto)
      .then((res) => {
        retorno = res;
        if (!retorno) {
          this.confirmationService.confirm({
            message: `Já existe um boleto registrado, porém as taxas foram alteradas. Ao prosseguir, o boleto anterior será cancelado e
                um novo boleto será gerado com os novos valores. O sistema reconhecerá somente o pagamento do novo boleto. Deseja continuar?`,
            header: 'Confirmação',
            closable: true,
            closeOnEscape: true,
            icon: 'pi pi-exclamation-circle',
            rejectButtonProps: {
              label: 'Cancelar',
              severity: 'secondary',
              outlined: true,
            },
            acceptButtonProps: {
              label: 'Continuar',
            },
            accept: () => {
              this.salvarSelecaoOferta(true);
            },
            reject: () => {
              this.layoutProcessoDataService.setLoadBotaoProximo(false);
              this.layoutProcessoDataService.setDesativarTodosBotoes(false);
              this.selecaoOfertaDataService.setDeabilitarBotaoAlterarHieraquia(
                false,
              );
            },
          });
        }
      });
  }
}
