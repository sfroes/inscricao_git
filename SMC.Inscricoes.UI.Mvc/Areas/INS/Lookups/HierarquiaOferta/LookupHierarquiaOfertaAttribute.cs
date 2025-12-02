using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups
{
    public class LookupHierarquiaOfertaAttribute : SMCLookupAttribute
    {
        public LookupHierarquiaOfertaAttribute()
            : base("HierarquiaOferta")
        {
            Filter = typeof(LookupHierarquiaOfertaFiltroViewModel);
            Model = typeof(LookupHierarquiaOfertaViewModel);
            PrepareFilter = typeof(LookupHierarquiaOfertaFiltroViewModel);
            Service<IHierarquiaOfertaService>(nameof(IHierarquiaOfertaService.LookupBuscarHierarquiaOfertas));
            SelectService<IHierarquiaOfertaService>(nameof(IHierarquiaOfertaService.LookupBuscarHierarquiaOferta));
            HideSeq = true;
        }
    }
}
