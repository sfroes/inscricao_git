using SMC.Framework.Mapper;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class JustificativaSituacaoInscricaoVO : ISMCMappable
    {
        public long? SeqMotivo { get; set; }
        public string NomeInscrito { get; set; }

        public string Motivo { get; set; }

        public string Justificativa { get; set; }
    }
}
