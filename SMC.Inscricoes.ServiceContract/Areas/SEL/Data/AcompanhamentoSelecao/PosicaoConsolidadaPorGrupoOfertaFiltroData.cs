using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.SEL.Data
{
    public class PosicaoConsolidadaPorGrupoOfertaFiltroData : SMCPagerFilterData, ISMCMappable
    {
        public long SeqProcesso { get; set; }

        public long? SeqGrupoOferta { get; set; }

        //[SMCMapProperty("SeqItemHierarquiaOferta.Seq")]
        public long? SeqItemHierarquiaOferta { get; set; }

        //[SMCMapProperty("SeqOferta.Seq")]
        public long? SeqOferta { get; set; }
    }
}
