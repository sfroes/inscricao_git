import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class LayoutMenuLateralDataService {
  private _exibirComponenteMenuLateral = signal<boolean>(true);
  $exibirComponenteMenuLateral = this._exibirComponenteMenuLateral.asReadonly();
  setExbirComponenteMenuLateral(data: boolean) {
    this._exibirComponenteMenuLateral.set(data);
  }
}
