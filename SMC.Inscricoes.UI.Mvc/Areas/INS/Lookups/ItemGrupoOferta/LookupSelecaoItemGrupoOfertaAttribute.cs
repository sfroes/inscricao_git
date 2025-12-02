using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;

namespace SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups
{
    public class LookupSelecaoItemGrupoOfertaAttribute : SMCLookupAttribute
    {
        /// <summary>
        /// Construtor padrão. Base requer uma string que faz referência ao serviço no IoC.
        /// </summary>
        public LookupSelecaoItemGrupoOfertaAttribute()
            : base("HierarquiaOferta", SMCDisplayModeType.TreeView)
        {
            Model = typeof(LookupItemGrupoOfertaViewModel);
            Filter = typeof(LookupItemGrupoOfertaFiltroViewModel);
            Service<IOfertaService>(nameof(IOfertaService.BuscarArvoreHierarquiaOfertaProcesso));
            SelectService<IOfertaService>(nameof(IOfertaService.BuscarHierarquiaOfertaComGrupo));
            Transformer = typeof(LookupItemGrupoOfertaFolhaTransformer);
            PrepareFilter = typeof(LookupItemGrupoOfertaPrepareFilter);
            CustomView = @"SMC.Inscricoes.UI.Mvc.dll/Areas/INS/Lookups/ItemGrupoOferta/Views/_LookupItemGrupoCustomView";
            ModalWindowSize = Framework.SMCModalWindowSize.Large;
        }

    }
}
