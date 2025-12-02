using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class TipoProcessoTemplateData : ISMCMappable
    {
        [DataMember]
        [SMCMapProperty("SeqTipoProcessoTemplate")]
        public long Seq { get; set; }

        [DataMember]
        public long SeqTipoProcesso { get; set; }

        [DataMember]
        public long SeqTemplateProcessoSGF { get; set; }

        [DataMember]
        public bool Ativo { get; set; }
    }
}
