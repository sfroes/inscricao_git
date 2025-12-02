import { Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { SelecaoOfertaComponent } from './areas/ins/selecao-oferta/selecao-oferta.component';
import { PaginaErrosComponent } from './core/componentes/pagina-erros/pagina-erros.component';
import { LayoutProcessoComponent } from './core/layout-processo/layout-processo.component';

export const routes: Routes = [
  { path: '', component: AppComponent },

  {
    path: 'selecao-oferta',
    component: LayoutProcessoComponent,
    children: [{ path: '', component: SelecaoOfertaComponent }],
  },
  {
    path: 'pagina-erros',
    component: LayoutProcessoComponent,
    children: [{ path: '', component: PaginaErrosComponent }],
  },
];
