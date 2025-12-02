using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    public class LookupOfertaFiltroData : ISMCMappable
    {
        public long? SeqProcesso { get; set; }
        public long? SeqGrupoOferta { get; set; }
        
        public long? SeqItemHierarquiaOferta { get; set; }
    }
}
