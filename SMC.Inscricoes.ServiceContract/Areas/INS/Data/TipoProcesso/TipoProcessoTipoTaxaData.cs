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
    public class TipoProcessoTipoTaxaData : ISMCMappable
    {
        [DataMember]
        [SMCMapProperty("Seq")]
        public long? SeqTipoProcessoTipoTaxa { get; set; }

        [DataMember]
        public long SeqTipoTaxa { get; set; }

        [DataMember]
        public bool Ativo { get; set; }
    }
}
