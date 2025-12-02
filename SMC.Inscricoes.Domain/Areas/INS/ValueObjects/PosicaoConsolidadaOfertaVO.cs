namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    /// <summary>
    /// Value Objec para representar a posição consolidade de um processo
    /// </summary>
    public class PosicaoConsolidadaOfertaVO : PosicaoConsolidadaVO
    {
        public long? SeqGrupoOferta { get; set; }

        public long SeqProcesso { get; set; }

        public string NomeGrupo { get; set; }

        public string HierarquiaCompleta { get; set; }
    }
}