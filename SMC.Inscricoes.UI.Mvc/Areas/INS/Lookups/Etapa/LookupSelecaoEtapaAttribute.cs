using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups
{
    public class LookupSelecaoEtapaAttribute : SMCLookupAttribute
    {

        /// <summary>
        /// Construtor padrão. Base requer uma string que faz referência ao serviço no IoC.
        /// </summary>
        public LookupSelecaoEtapaAttribute()
            : base("Processo", SMCDisplayModeType.Grid)
        {
            Model = typeof(LookupEtapaViewModel);
            Filter = typeof(LookupEtapaFiltroViewModel);
            PrepareFilter = typeof(LookupEtapaFiltroExemploViewModel);
        }

    }
}
