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
    public class TipoNotificacaoListaData : ISMCMappable
    {
        [DataMember]
        public long Seq { get; set; }

        [DataMember]
        public string TipoNotificacao { get; set; }

        [DataMember]
        public bool PermiteAgendamento { get; set; }
    }
}
