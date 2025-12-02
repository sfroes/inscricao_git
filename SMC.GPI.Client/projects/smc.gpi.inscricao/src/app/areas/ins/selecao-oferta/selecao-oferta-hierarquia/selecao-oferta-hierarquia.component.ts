import { Component, effect, inject, Input } from '@angular/core';
import { AccordionModule } from 'primeng/accordion';
import { ButtonModule } from 'primeng/button';
import { Dialog } from 'primeng/dialog';
import { SkeletonModule } from 'primeng/skeleton';
import { TreeModule } from 'primeng/tree';
import { HierarquiaModel } from '../model/herarquia.model';
import { SelecaoOfertaHierarquiaService } from './service/selecao-oferta-hierarquia.service';

@Component({
  selector: 'app-selecao-oferta-hierarquia',
  imports: [TreeModule, SkeletonModule, ButtonModule, Dialog, AccordionModule],
  templateUrl: './selecao-oferta-hierarquia.component.html',
  styles: `
     ::ng-deep .n-tree-dynamic {
      .p-tree {
        height: auto !important;
        min-height: 150px;

        .p-tree-container {
          height: auto !important;
          min-height: auto !important;
          overflow: visible !important;
        }

        .p-tree-wrapper {
          height: auto !important;
          overflow: visible !important;
        }
      }
    }
  `
})

export class SelecaoOfertaHierarquiaComponent {
  @Input() set dadosHierarquia(data: HierarquiaModel[]) {
    this.selecaoOfertaHierarquiaService.validarInputComponente(data);
  }

  selecaoOfertaHierarquiaService = inject(SelecaoOfertaHierarquiaService);

  constructor() {
    effect(() => {
      this.selecaoOfertaHierarquiaService.selecaoOfertaDataService.$botaoCancelarHierarquia();

      this.selecaoOfertaHierarquiaService.ofertasSelecionados = [];
      this.selecaoOfertaHierarquiaService.marcarArvoreSelecionada(
        this.selecaoOfertaHierarquiaService.files,
      );
      this.atualizarOfertasSeleciondas('');
      this.selecaoOfertaHierarquiaService.tituloAtividade =
        this.selecaoOfertaHierarquiaService.selecaoOfertaDataService.$selecaoOferta().labelOferta;
    });
  }

  atualizarOfertasSeleciondas(event: any) {
    this.selecaoOfertaHierarquiaService.atualizarOfertasSeleciondas(event);
  }
}
