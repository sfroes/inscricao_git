using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    public class LookupHierarquiaOfertaData : ISMCMappable
    {
        public long Seq { get; set; }

        public string TipoItemHierarquiaOferta { get; set; }

        public string HierarquiaOferta { get; set; }
    }
}
