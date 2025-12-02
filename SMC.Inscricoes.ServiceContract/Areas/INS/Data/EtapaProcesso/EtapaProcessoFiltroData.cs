using SMC.Framework.Mapper;
using SMC.Framework.Model;
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
    public class EtapaProcessoFiltroData : SMCPagerFilterData, ISMCMappable
    {
        [DataMember]
        public long SeqProcesso { get; set; }

        [DataMember]
        public long? SeqEtapaSGF { get; set; }

        [DataMember]
        public string Token { get; set; }
    }
}
