using SMC.Framework.Mapper;
using System.Collections.Generic;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class AlteracaoSituacaoVO : ISMCMappable
    {
        public long Seq { get; set; }

        public long SeqTipoProcessoSituacaoDestino { get; set; }

        public List<long> SeqInscricoes { get; set; }

        public long? SeqMotivoSGF { get; set; }
        
        public string Justificativa { get; set; }
    }
}
