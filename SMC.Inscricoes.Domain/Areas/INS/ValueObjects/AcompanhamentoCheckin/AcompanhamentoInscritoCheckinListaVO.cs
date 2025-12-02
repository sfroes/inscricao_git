using SMC.Framework.Mapper;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class AcompanhamentoInscritoCheckinListaVO : ISMCMappable
    {
        public long Seq { get; set; }
        public long SeqProcesso { get; set; }
        public string DescricaoOferta { get; set; }
        public int NumeroInscrito { get; set; }
        public int NumeroChecinRealizado { get; set; }
        public int RestanteVagas { get; set; }
        public long NumeroVagasOferta { get; set; }
        public string DescricaoProcesso { get; set; }
    }
}
