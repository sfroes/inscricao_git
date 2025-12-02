using SMC.Framework;
using SMC.Framework.Extensions;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.UI.Mvc.Html;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups
{
    public class LookupHierarquiaOfertaFiltroViewModel : SMCLookupFilterViewModel, ISMCFilter<LookupHierarquiaOfertaFiltroViewModel>
    {
        #region Data Sources
        public List<SMCDatasourceItem> TiposItens { get; set; }
        #endregion

        [SMCHidden]
        public long SeqProcesso { get; set; }

        [SMCSelect("TiposItens")]
        [SMCSize(SMCSize.Grid8_24)]
        public long? SeqTipoItem { get; set; }        
        
        [SMCDescription]
        [SMCSize(SMCSize.Grid8_24)]
        public string DescricaoHierarquia { get; set; }

        public LookupHierarquiaOfertaFiltroViewModel Filter(SMCControllerBase controllerBase, LookupHierarquiaOfertaFiltroViewModel filter)
        {
            var service = controllerBase.Create<ITipoItemHierarquiaOfertaService>();
            filter.TiposItens = service.BuscarTiposItemHierarquiaOfertaPorProcessoSelect(filter.SeqProcesso, false).TransformList<SMCDatasourceItem>();

            return filter;
        }
    }
}
