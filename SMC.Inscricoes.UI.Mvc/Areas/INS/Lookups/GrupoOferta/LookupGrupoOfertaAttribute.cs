using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups
{
    public class LookupGrupoOfertaAttribute : SMCLookupAttribute
    {

        /// <summary>
        /// Construtor padrão. Base requer uma string que faz referência ao serviço no IoC.
        /// </summary>
        public LookupGrupoOfertaAttribute()
            : base("GrupoOferta", SMCDisplayModeType.Grid)
        {
            Model = typeof(LookupGrupoOfertaViewModel);
            Filter = typeof(LookupGrupoOfertaFiltroViewModel);
            Service<IGrupoOfertaService>();
            this.HideSeq = true;
        }

    }
}
