using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups
{
    public class LookupProcessoAttribute : SMCLookupAttribute
    {
        public LookupProcessoAttribute()
            : base("Processo")
        {
            this.Filter = typeof(LookupProcessoFiltroViewModel);
            this.Model = typeof(LookupProcessoViewModel);
            this.Service<IProcessoService>(nameof(IProcessoService.BuscarProcessoLookup));
            this.SelectService<IProcessoService>(nameof(IProcessoService.BuscarProcessoLookupSelect));
            this.PrepareFilter = typeof(LookupProcessoPrepareFilter);
            this.HideSeq = true;
            this.AutoCompleteSize = 1;
        }
    }
}
