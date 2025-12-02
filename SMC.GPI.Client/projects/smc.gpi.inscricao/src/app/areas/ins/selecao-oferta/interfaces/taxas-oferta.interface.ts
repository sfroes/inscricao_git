
export interface TaxasOferta {
  descricaoTaxa: string;
  descricaoComplementar: string;
  valorTaxa: string;
  seqTaxa: number;
  numeroMaximo: number | null;
  numeroMinimo: number | null;
  valorItem: number | null;
  valorTotalTaxa: number;
  numeroItens: number | null;
}
