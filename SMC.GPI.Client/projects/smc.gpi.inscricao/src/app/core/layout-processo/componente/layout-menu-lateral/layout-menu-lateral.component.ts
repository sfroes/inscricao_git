import { Component, inject, OnInit } from '@angular/core';
import { Skeleton } from 'primeng/skeleton';
import { LayoutMenuLateralService } from './service/layout-menu-lateral.service';

@Component({
  selector: 'app-layout-menu-lateral',
  imports: [Skeleton],
  templateUrl: './layout-menu-lateral.component.html',
})
export class LayoutMenuLateralComponent implements OnInit {
  //#region injeção de dependência
  servieLayoutMenuLateral = inject(LayoutMenuLateralService);
  //#endregion

  ngOnInit(): void {}
}
