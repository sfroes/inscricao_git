import { Component, inject, OnInit } from '@angular/core';
import { Skeleton } from 'primeng/skeleton';
import { LayoutFooterService } from './service/layout-footer.service';

@Component({
  selector: 'app-layout-footer',
  imports: [Skeleton],
  templateUrl: './layout-footer.component.html',
})
export class LayoutFooterComponent implements OnInit {
  //#region injeção de dependência
  layoutFooterServe = inject(LayoutFooterService);
  //#endregion

  ngOnInit(): void {
    this.layoutFooterServe.carregarFooter();
  }
}
