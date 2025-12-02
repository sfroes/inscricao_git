import { CommonModule } from '@angular/common';
import {
  Component,
  computed,
  effect,
  inject,
  input,
  Input,
} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { InputNumberModule } from 'primeng/inputnumber';
import { SingleClickEvent } from '../../../../../../../../shared/events/single-click.event';
import { ToolsService } from '../../../../../../../../shared/tools.service';
import { SelecaoOfertaDataService } from '../../service/selecao-oferta-data.service';
import { SelecaoOfertaComTaxaService } from '../service/selecao-oferta-com-taxa.service';
import { TipoCobranca } from './../../model/enums/tipo-cobranca.enum';

@Component({
  selector: 'app-selecao-oferta-com-taxa-simples',
  imports: [InputNumberModule, FormsModule, SingleClickEvent, CommonModule],
  templateUrl: './selecao-oferta-com-taxa-simples.component.html',
})
export class SelecaoOfertaComTaxaSimplesComponent {
  toolsService = inject(ToolsService);
  @Input() tipoCobranca: TipoCobranca = {} as TipoCobranca;

  $seqOferta = input.required<number>();
  $taxas = input.required<any>();

  selecaoOfertaComTaxaService = inject(SelecaoOfertaComTaxaService);
  selecaoOfertaDataService = inject(SelecaoOfertaDataService);

  taxasAtualizadas = computed(() => {
    const seqOferta = this.$seqOferta();

    // Garantir que tipoCobranca seja número
    const tipoCobrancaNum = Number(this.tipoCobranca);
    switch (tipoCobrancaNum) {
      case 1: // TipoCobranca.porInscricao
        const taxasPorInscricao =
          this.selecaoOfertaComTaxaService.$taxasPorInscricao();
        if (taxasPorInscricao && taxasPorInscricao.seqOferta === seqOferta) {
          return taxasPorInscricao.taxasOferta || [];
        }
        break;

      case 2: // TipoCobranca.porOferta
        const taxasPorOferta =
          this.selecaoOfertaComTaxaService.$taxasPorOferta();
        const ofertaEncontrada = taxasPorOferta.find(
          (f) => f.seqOferta === seqOferta,
        );
        if (ofertaEncontrada && ofertaEncontrada.taxasOferta) {
          // Importante: retornar as taxas com os valores atualizados
          return ofertaEncontrada.taxasOferta.map((taxa) => {
            // Buscar a taxa correspondente no selecaoOferta para pegar o valorTotalTaxa atualizado
            const taxaAtualizada = this.selecaoOfertaDataService
              .$selecaoOferta()
              .taxas.find(
                (t) => t.seqTaxa === taxa.seqTaxa && t.seqOferta === seqOferta,
              );

            if (taxaAtualizada) {
              return {
                ...taxa,
                numeroItens: taxaAtualizada.numeroItens,
                valorTotalTaxa:
                  taxaAtualizada.valorItem! * (taxaAtualizada.numeroItens || 0),
              };
            }
            return taxa;
          });
        }
        break;

      case 3: // TipoCobranca.porQuantidadeOfertas
        const taxasPorQuantidade =
          this.selecaoOfertaComTaxaService.$taxasPorQuantidadeOferta();
        if (taxasPorQuantidade && taxasPorQuantidade.seqOferta === seqOferta) {
          return taxasPorQuantidade.taxasOferta || [];
        }
        break;
    }

    // Fallback
    return this.$taxas() || [];
  });

  constructor() {
    effect(() => {
      const taxas = this.taxasAtualizadas();
    });
  }

  formartarTotal(value: number | null) {
    return this.toolsService.currencyMask(value);
  }

  /**
   * Chamado quando o p-inputnumber recebe foco.
   */
  onNumeroItensFocus(event: FocusEvent): void {
    this.selecaoOfertaComTaxaService.bloquearInserirDadosPorTeclado(event);
  }

  readonly(taxa: any) {
    if (this.tipoCobranca === TipoCobranca.porInscricao) {
      return this.selecaoOfertaComTaxaService.readOnlyTaxasPorInscricao(taxa);
    } else if (this.tipoCobranca === TipoCobranca.porOferta) {
      return this.selecaoOfertaComTaxaService.readOnlyTaxasPorOferta(taxa);
    }
    return true;
  }
}
