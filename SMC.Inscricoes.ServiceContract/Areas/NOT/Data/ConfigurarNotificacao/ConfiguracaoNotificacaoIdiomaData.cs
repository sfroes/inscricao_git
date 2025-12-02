using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using SMC.Notificacoes.ServiceContract.Areas.NTF.Data;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.NOT.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class ConfiguracaoNotificacaoIdiomaData : ISMCMappable
    {
        [DataMember]
        public SMCLanguage Idioma { get; set; }

        [DataMember]
        public ConfiguracaoNotificacaoEmailData ConfiguracaoNotificacao { get; set; }
    }
}
