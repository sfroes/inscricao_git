import { HierarquiaModel } from './herarquia.model';
import { Ofertas } from './selecao-oferta.model';

export class ListaOfertaModel {
  oferta: Ofertas = {} as Ofertas;
  hierquia: HierarquiaModel | null = {} as HierarquiaModel;
}
