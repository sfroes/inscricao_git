using SMC.Framework.Mapper;
using SMC.Framework.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    public class LookupHierarquiaOfertaFiltroData : SMCPagerFilterData, ISMCMappable
    {
        public long? SeqProcesso { get; set; }

        public long? SeqTipoItem { get; set; }

        public string DescricaoHierarquia { get; set; }

        //public long? SeqItem { get; set; }
    }
}
