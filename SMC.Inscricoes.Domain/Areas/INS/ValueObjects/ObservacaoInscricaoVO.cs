using SMC.Framework.Mapper;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class ObservacaoInscricaoVO : ISMCMappable
    {
        public long Seq { get; set; }
        public long SeqInscrito { get; set; }
        public long SeqProcesso { get; set; }
        public string Observacao { get; set; }
        public string Nome { get; set; }
    }
}
