import { Component, inject } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute, RouterOutlet } from '@angular/router';
import { Skeleton } from 'primeng/skeleton';
import { LayoutFooterComponent } from './componente/layout-footer/layout-footer.component';
import { LayoutHeaderComponent } from './componente/layout-header/layout-header.component';
import { LayoutMenuLateralComponent } from './componente/layout-menu-lateral/layout-menu-lateral.component';
import { LayoutProcessoDataService } from './service/layout-processo-data.service';
import { LayoutProcessoService } from './service/layout-processo.service';

@Component({
  selector: 'app-layout-processo',
  imports: [
    RouterOutlet,
    LayoutHeaderComponent,
    LayoutFooterComponent,
    LayoutMenuLateralComponent,
    Skeleton,
  ],
  templateUrl: './layout-processo.component.html',
})
export class LayoutProcessoComponent {
  //injeção de dependência
  sanitizer = inject(DomSanitizer);
  serviceLayoutProcesso = inject(LayoutProcessoService);
  route = inject(ActivatedRoute);
  layoutProcessoDataService = inject(LayoutProcessoDataService);

  /**
   * Inicializa o componente.
   * Busca a descrição do do do processo pela sequencia de inscrição e armazena o resultado.
   * O metodo `buscarDescricaoProcesso` é executado apenas uma vez, quando o componente é inicializado.
   */
  ngOnInit(): void {
    this.buscarDescricaoProcesso();
    setTimeout(() => {
      this.validarUrlCss();
    }, 100);
  }

  /**
   * Busca a descrição do do processo pela sequencia de inscrição e armazena o resultado.
   * A descrição do processo é obtida pelo método `buscarDescricaoProcessoInscricao` do servico
   * `LayoutProcessoService`.
   * O resultado é armazenado na propriedade `descricaoProcesso` do componente.
   */
  buscarDescricaoProcesso() {
    let queryParams: any = {};
    const carregarRota = this.route.queryParams.subscribe((params) => {
      queryParams = params;
      this.serviceLayoutProcesso.buscarDescricaoProcessoInscricao(
        queryParams['seqInscricao'],
      );
    });
  }

  validarUrlCss() {
    this.serviceLayoutProcesso.validarUrlCss();
  }
}
