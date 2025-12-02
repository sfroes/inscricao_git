import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { map } from 'rxjs';
import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class LayoutFooterService {
  //#region signals
  private _isLoading = signal(true);
  isLoading$ = this._isLoading.asReadonly();

  private _conteudo = signal<any>(null);
  conteudo$ = this._conteudo.asReadonly();
  //#endregion

  //#region injeção
  sanitizer = inject(DomSanitizer);
  http = inject(HttpClient);
  //#endregion

  constructor() {}
  carregarFooter() {
    const caminho = 'INS/Front/PartialFooter';
    const url = environment.frontUrl + caminho;
    this.http
      .get(url, { responseType: 'text' })
      .pipe(map((response) => response as string))
      .subscribe((res) => {
        this._conteudo.set(this.sanitizer.bypassSecurityTrustHtml(res));
        this._isLoading.set(false);
      });
  }
}
