using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups.Oferta;

namespace SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups
{
    public class LookupSelecaoOfertaInscricaoAttribute : SMCLookupAttribute
    {
        public LookupSelecaoOfertaInscricaoAttribute()
            : base("Oferta", SMCDisplayModeType.TreeView)
        {
            Model = typeof(LookupSelecaoOfertaInscricaoViewModel);
            Filter = typeof(LookupOfertaFiltroViewModel);
            PrepareFilter = typeof(LookupOfertaPrepareFilter);
            this.Service<IOfertaService>(nameof(IOfertaService.BuscarArvoreSelecaoOfertasInscricao));
            this.SelectService<IOfertaService>(nameof(IOfertaService.BuscarSelecaoOfertasInscricaoKeyValue));
            Transformer = typeof(LookupSelecaoOfertaInscricaoFolhaTransformer);
            CustomView = @"SMC.Inscricoes.UI.Mvc.dll/Areas/INS/Lookups/Oferta/Views/_LookupSelecaoOfertaInscricaoLista";
            this.HideSeq = true;
        }
    }
}
