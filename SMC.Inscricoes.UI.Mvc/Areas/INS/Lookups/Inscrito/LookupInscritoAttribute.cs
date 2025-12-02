using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups
{
    public class LookupInscritoAttribute : SMCLookupAttribute
    {
        public LookupInscritoAttribute() 
            : base("Inscrito", SMCDisplayModeType.Grid)
        {
            Filter = typeof(LookupInscritoFiltroViewModel);
            Model = typeof(LookupInscritoListaViewModel);
            Service<IInscritoService>(nameof(IInscritoService.BuscarInscritosLookup));
            SelectService<IInscritoService>(nameof(IInscritoService.BuscarInscritoLookup));
        }
    }
}
