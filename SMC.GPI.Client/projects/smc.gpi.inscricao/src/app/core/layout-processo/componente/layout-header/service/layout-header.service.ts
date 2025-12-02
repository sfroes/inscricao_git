import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { map } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { LayoutProcessoService } from '../../../service/layout-processo.service';

@Injectable({
  providedIn: 'root',
})
export class LayoutHeaderService {
  //#region signals

  private _isLoading = signal(true);
  isLoading$ = this._isLoading.asReadonly();

  private _conteudo = signal<any>(null);
  conteudo$ = this._conteudo.asReadonly();

  private _rodarScripts = signal(false);
  rodarScripts$ = this._rodarScripts.asReadonly();
  setRodarScripts(data: boolean) {
    this._rodarScripts.set(data);
  }

  //#endregion

  //#region injeção de dependencia
  sanitizer = inject(DomSanitizer);
  serviceLayoutProcesso = inject(LayoutProcessoService);
  http = inject(HttpClient);
  //#endregion

  constructor() {}

  carregarHeader() {
    const caminho = 'Ins/Front/PartialHeader';
    const url = environment.frontUrl + caminho;
    this.http
      .get(url, { responseType: 'text' })
      .pipe(map((response) => response as string))
      .subscribe((res) => {
        this._conteudo.set(this.sanitizer.bypassSecurityTrustHtml(res));
        this._isLoading.set(false);
        this._rodarScripts.set(true);
      });
  }
}
