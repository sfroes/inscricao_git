import { GrupoTaxa } from "./grupo-taxa.model";

export interface GrupoTaxaPorTipoCobranca {
  grupoTaxaOferta: GrupoTaxa[];
  grupoTaxaQuantidadeOferta: GrupoTaxa[];
  grupoTaxaInscricao: GrupoTaxa[];
}
