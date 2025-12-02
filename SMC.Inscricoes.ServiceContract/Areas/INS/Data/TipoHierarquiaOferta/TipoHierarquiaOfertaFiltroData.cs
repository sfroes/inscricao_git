using SMC.Framework.Mapper;
using SMC.Framework.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    public class TipoHierarquiaOfertaFiltroData : SMCPagerFilterData,ISMCMappable
    {        
        public long? Seq { get; set; }
        
        public string Descricao { get; set; }
    }
}
