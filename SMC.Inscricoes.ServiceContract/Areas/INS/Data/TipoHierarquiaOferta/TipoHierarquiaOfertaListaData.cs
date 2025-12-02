using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    public class TipoHierarquiaOfertaListaData : ISMCMappable
    {        
        public long Seq { get; set; }
        
        public string Descricao { get; set; }
    }
}
