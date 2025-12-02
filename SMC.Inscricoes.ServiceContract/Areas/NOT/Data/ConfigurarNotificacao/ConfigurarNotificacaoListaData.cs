using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.NOT.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class ConfigurarNotificacaoListaData : ISMCMappable
    {
        [DataMember]
        public long Seq { get; set; }

        [DataMember]
        public string TipoNotificacao { get; set; }

        [DataMember]
        public long SeqTipoNotificacao { get; set; }

        [DataMember]
        public bool EnvioAutomatico { get; set; }

        [DataMember]
        public bool PermiteAgendamento { get; set; }

        [DataMember]
        public long SeqProcesso { get; set; }

        [DataMember]
        public long SeqUnidadeResponsavel { get; set; }
    }
}
