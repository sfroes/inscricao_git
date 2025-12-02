import { CommonModule } from '@angular/common';
import { Component, inject, ViewChild } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { Popover, PopoverModule } from 'primeng/popover';
import { Skeleton } from 'primeng/skeleton';
import { LayoutNavegacaoDataService } from './service/layout-navegacao-data.service';
import { LayoutNavegacaoService } from './service/layout-navegacao.service';
@Component({
  selector: 'app-layout-navegacao',
  imports: [Skeleton, ButtonModule, CommonModule, PopoverModule],
  templateUrl: './layout-navegacao.component.html',
})
export class LayoutNavegacaoComponent {
  //#region injeção de dependecias
  layoutNavegacaoService = inject(LayoutNavegacaoService);
  layoutDataNavegacaoService = inject(LayoutNavegacaoDataService);
  //#endregion

  @ViewChild('op') op!: Popover;

  botaoCancelar() {
    this.layoutNavegacaoService.botaoCancelar();
  }

  botaoProximo() {
    this.layoutNavegacaoService.botaoProximo(true);
  }

  botaoAnterior() {
    this.layoutNavegacaoService.botaoAnterior();
  }

  desativarTodosBotoes(ind: boolean) {
    this.layoutNavegacaoService.desativarTodosBotoes(ind);
  }

  errosBotaoProximo(event: any) {
    if (
      this.layoutNavegacaoService.desabilitarBotaoProximo$() &&
      this.layoutDataNavegacaoService.$errosBotaoProximo().length > 0
    ) {
      this.op.toggle(event);
    }
  }
}
