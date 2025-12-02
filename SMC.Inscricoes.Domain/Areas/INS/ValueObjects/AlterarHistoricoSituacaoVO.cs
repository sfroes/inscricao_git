using SMC.Framework.Mapper;
using System.Collections.Generic;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class AlterarHistoricoSituacaoVO : ISMCMappable
    {
        public long SeqProcesso { get; set; }

        public long SeqTipoProcessoSituacaoDestino { get; set; }

        public List<long> SeqInscricoesOferta { get; set; }

        public long? SeqMotivoSGF { get; set; }

        public string Justificativa { get; set; }

        public string TokenSituacaoDestino { get; set; }
    }
}