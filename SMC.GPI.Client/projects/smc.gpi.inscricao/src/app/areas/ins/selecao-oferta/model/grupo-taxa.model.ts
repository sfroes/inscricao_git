import { Taxa } from "./selecao-oferta.model";

export class GrupoTaxa {
  seq: number; // No default value here, let the constructor set it
  descricao: string;
  numeroMinimoItens: number;
  numeroMaximoItens: number | null;
  numeroItens: number;
  taxas: Taxa[];

  // Constructor to initialize properties
  constructor(data: any = {}) {
    this.seq = data.seq || 0;
    this.descricao = data.descricao || '';
    this.numeroMinimoItens = data.numeroMinimoItens || 0;
    this.numeroMaximoItens =
      data.numeroMaximoItens === undefined ? null : data.numeroMaximoItens;
    this.numeroItens = data.numeroItens || 0;
    this.taxas = data.taxas;
  }

  minimoNaoAtingido(): boolean {
    if (!this.taxas || this.taxas.length === 0) {
      // Se não há taxas, e o mínimo for > 0, então o mínimo NÃO foi atingido.
      return this.numeroMinimoItens > 0;
    }

    // 1. Agrupar as taxas por seqOferta
    const gruposPorOferta = this.taxas.reduce(
      (acc, taxa) => {
        const seqOferta = taxa.seqOferta;
        if (!acc[seqOferta]) {
          acc[seqOferta] = [];
        }
        acc[seqOferta].push(taxa);
        return acc;
      },
      {} as { [key: number]: Taxa[] },
    );

    // 2. Para cada grupo de seqOferta, verificar se a soma dos numeroItens NÃO atinge o this.numeroMinimoItens
    for (const seqOferta in gruposPorOferta) {
      const taxasDaOferta = gruposPorOferta[seqOferta];
      const somaNumeroItens = taxasDaOferta.reduce(
        (sum, t) => sum + (t.numeroItens || 0),
        0,
      );

      // Se QUALQUER um dos grupos de seqOferta NÃO atingir o mínimo, retornamos true (mínimo NÃO atingido)
      if (somaNumeroItens < this.numeroMinimoItens) {
        return true; // Mínimo NÃO atingido para este subgrupo de oferta
      }
    }

    // Se chegamos aqui, significa que TODOS os grupos de seqOferta atingiram ou excederam o mínimo.
    return false; // O mínimo foi atingido em todos os subgrupos (ou seja, NÃO está "não atingido")
  }
}
