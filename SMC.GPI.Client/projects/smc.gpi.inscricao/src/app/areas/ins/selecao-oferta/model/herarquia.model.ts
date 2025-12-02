export class HierarquiaModel {
  seq: number = 0;
  descricao: string = '';
  seqPai: number = 0;
  isLeaf: boolean = false;
  descricaoComplementar?: string | null;
}
