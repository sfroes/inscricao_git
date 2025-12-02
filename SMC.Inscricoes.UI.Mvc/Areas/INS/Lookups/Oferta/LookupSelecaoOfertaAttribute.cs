using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups.Oferta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups
{
    public class LookupSelecaoOfertaAttribute : SMCLookupAttribute
    {

        /// <summary>
        /// Construtor padrão. Base requer uma string que faz referência ao serviço no IoC.
        /// </summary>
        public LookupSelecaoOfertaAttribute()
            : base("Oferta", SMCDisplayModeType.TreeView)
        {
            Model = typeof(LookupOfertaViewModel);
            Filter = typeof(LookupOfertaFiltroViewModel);
            PrepareFilter = typeof(LookupOfertaPrepareFilter);
            this.Service<IOfertaService>(nameof(IOfertaService.BuscarArvoreOfertas));
            this.SelectService<IOfertaService>(nameof(IOfertaService.BuscarOfertaKeyValue));
            Transformer = typeof(LookupOfertaFolhaTransformer);
            CustomView = @"SMC.Inscricoes.UI.Mvc.dll/Areas/INS/Lookups/Oferta/Views/_LookupSelecaoOfertaLista";
            this.HideSeq = true;
        }

    }
}
