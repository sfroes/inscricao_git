using SMC.Framework.Mapper;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    [DataContract]
    public class InscricaoOfertaHistoricoSituacaoData : ISMCMappable
    {
        [DataMember]
        public long Seq { get; set; }

        [DataMember]
        public long SeqSituacao { get; set; }

        [DataMember]
        public long SeqInscricao { get; set; }

        [DataMember]
        public string TokenSituacao { get; set; }

        [DataMember]
        public bool Atual { get; set; }

        [DataMember]
        public string Justificativa { get; set; }

        [DataMember]
        public long? SeqMotivoSituacao { get; set; }
    }
}