using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Areas.NOT.Enums;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.NOT.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class ParametroEnvioNotificacaoData : ISMCMappable
    {
        [DataMember]
        public long Seq { get; set; }

        [DataMember]
        public short QuantidadeDiasInicioEnvio { get; set; }

        [DataMember]
        public Temporalidade Temporalidade { get; set; }

        [DataMember]
        public AtributoAgendamento AtributoAgendamento { get; set; }

        [DataMember]
        public short? QuantidadeDiasRecorrencia { get; set; }

        [DataMember]
        public bool? ReenviarNotificacaoInscrito { get; set; }

        [DataMember]
        public bool Ativo { get; set; }
    }
}
