using SMC.Framework.Mapper;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    [DataContract]
    public class AlterarHistoricoSituacaoData : ISMCMappable
    {
        [DataMember]
        public long SeqProcesso { get; set; }

        [DataMember]
        public long SeqTipoProcessoSituacaoDestino { get; set; }

        [DataMember]
        public List<long> SeqInscricoesOferta { get; set; }

        [DataMember]
        public long? SeqMotivoSGF { get; set; }

        [DataMember]
        public string Justificativa { get; set; }

        [DataMember]
        public string TokenSituacaoDestino { get; set; }
    }
}