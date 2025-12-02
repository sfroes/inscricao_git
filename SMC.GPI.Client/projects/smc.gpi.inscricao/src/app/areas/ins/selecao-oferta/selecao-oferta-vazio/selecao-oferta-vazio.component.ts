import { SelecaoOfertaComTaxaComponent } from './../selecao-oferta-com-taxa/selecao-oferta-com-taxa.component';
import { Component, inject, OnDestroy, signal } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { Subscription } from 'rxjs';
import { SelecaoOfertaDataService } from '../service/selecao-oferta-data.service';
import { SelecaoOfertaService } from '../service/selecao-oferta.service';
import { SelecaoOfertaComTaxaService } from '../selecao-oferta-com-taxa/service/selecao-oferta-com-taxa.service';

@Component({
  selector: 'app-selecao-oferta-vazio',
  imports: [ButtonModule],
  templateUrl: './selecao-oferta-vazio.component.html',
})
export class SelecaoOfertaVazioComponent implements OnDestroy {
  desabilitarBotaoSelecionar = signal(false);

  //#region injeção de dependencia
  selecionarOfertaService = inject(SelecaoOfertaService);
  selecaoOfertaDataService = inject(SelecaoOfertaDataService);

  selecaoOfertaComTaxaService = inject(SelecaoOfertaComTaxaService)
  //#endregion
  constructor() {
    this.selecaoOfertaDataService.setDesativarBotaoSelecionaModalHierarquia(
      false,
    );
  }

  ngOnInit(){

    //sempre que ele iniciar ele zera todos os itens das taxas selecionadas anteriormente.
    this.selecaoOfertaComTaxaService.taxas = this.selecaoOfertaDataService.$selecaoOferta().taxas;
    this.selecaoOfertaComTaxaService.montarGrupoTaxas();
    this.selecaoOfertaComTaxaService.montarTaxasPorOferta();
    this.selecaoOfertaComTaxaService.montarTaxasPorInscricao();
    this.selecaoOfertaComTaxaService.montarTaxasPorQuantidadeOferta();
    this.selecaoOfertaComTaxaService.montarIstrucoesTaxas();
  }

  ngOnDestroy(): void {
  }

  selecionarHierarquia() {
    this.selecaoOfertaDataService.setAbrirModalSelecaoOferta(true);
  }
}
