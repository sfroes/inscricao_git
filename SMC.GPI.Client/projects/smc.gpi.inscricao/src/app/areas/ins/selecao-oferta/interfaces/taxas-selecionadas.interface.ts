import { TaxasOferta } from "./taxas-oferta.interface";

export interface TaxasSelecioandas {
  seqOferta: number;
  descricaoOferta: string;
  numeroOpcao: number;
  taxasOferta: TaxasOferta[];
}
