import { CommonModule } from '@angular/common';
import { Component, Directive, HostListener, inject, input } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { InputNumberModule } from 'primeng/inputnumber';
import { SelecaoOfertaDataService } from '../../service/selecao-oferta-data.service';
import {
  SelecaoOfertaComTaxaService,
} from '../service/selecao-oferta-com-taxa.service';
import { Taxa } from '../../model/selecao-oferta.model';
import { SingleClickEvent } from '../../../../../../../../shared/events/single-click.event';
import { ToolsService } from '../../../../../../../../shared/tools.service';
import { GrupoTaxa } from '../../model/grupo-taxa.model';


@Component({
  selector: 'app-selecao-oferta-com-taxa-grupo',
  imports: [InputNumberModule, FormsModule,SingleClickEvent,CommonModule],
  templateUrl: './selecao-oferta-com-taxa-grupo.component.html',
})
export class SelecaoOfertaComTaxaGrupoComponent {
  //#region variaveis
  $grupoTaxas = input.required<GrupoTaxa[]>();
  $seqOferta = input<number | null>();
  //#endregion


  //#region injeção de depencias
  selecaoOfertaComTaxaService = inject(SelecaoOfertaComTaxaService);
  selecaoOfertaDataService = inject(SelecaoOfertaDataService);

  toolsService = inject(ToolsService)
  //#endregion

  readonly(grupo: GrupoTaxa, taxa: Taxa): boolean {
    const result =
      (grupo.numeroMaximoItens != null &&
        grupo.numeroMinimoItens != null &&
        grupo.numeroMaximoItens == grupo.numeroMinimoItens) ||
      (taxa.numeroMaximo != null &&
        taxa.numeroMinimo != null &&
        taxa.numeroMaximo == taxa.numeroMinimo) ||
        taxa.numeroMaximo == 1 ||
      this.selecaoOfertaDataService.$selecaoOferta().possuiBoletoPago;
    return result;
  }

  formartarTotal(value:number | null){
    return this.toolsService.currencyMask(value);
  }
}

