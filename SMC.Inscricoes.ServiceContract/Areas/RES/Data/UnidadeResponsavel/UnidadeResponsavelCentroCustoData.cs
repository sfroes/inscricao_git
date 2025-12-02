using SMC.Framework;
using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.RES.Data
{
    [DataContract]
    public class UnidadeResponsavelCentroCustoData : ISMCMappable, ISMCLookupData
    {
        [DataMember]
        public long Seq { get; set; }

        [DataMember]
        [SMCMapProperty("CodigoCentroCusto")]
        public int CentroCusto { get; set; }

        [DataMember]
        public bool Ativo { get; set; }
    }
}
