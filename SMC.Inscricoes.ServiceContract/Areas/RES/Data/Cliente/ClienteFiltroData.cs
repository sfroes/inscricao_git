using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Inscricoes.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.RES.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL)]
    public class ClienteFiltroData : SMCPagerFilterData, ISMCMappable
    {
        [DataMember]
        public long? Seq { get; set; }

        [DataMember]
        public string Nome { get; set; }

        [DataMember]
        public string Sigla { get; set; }
    }
}
