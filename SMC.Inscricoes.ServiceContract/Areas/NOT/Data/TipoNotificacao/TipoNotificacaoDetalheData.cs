using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Areas.NOT.Enums;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.NOT.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class TipoNotificacaoDetalheData : ISMCMappable
    {
        [DataMember]
        [SMCMapProperty("SeqAtributo")]
        public long Seq { get; set; }

        [DataMember]
        [SMCMapProperty("AtributoAgendamento")]
        public AtributoAgendamento Atributo { get; set; }
    }
}
