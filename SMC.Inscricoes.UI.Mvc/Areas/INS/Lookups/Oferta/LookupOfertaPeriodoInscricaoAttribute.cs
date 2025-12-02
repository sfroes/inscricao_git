using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups.Oferta;

namespace SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups
{
    public class LookupOfertaPeriodoInscricaoAttribute : SMCLookupAttribute
    {

        /// <summary>
        /// Construtor padrão. Base requer uma string que faz referência ao serviço no IoC.
        /// </summary>
        public LookupOfertaPeriodoInscricaoAttribute()
            : base("Oferta", SMCDisplayModeType.TreeView)
        {
            Model = typeof(LookupOfertaPeriodoInscricaoViewModel);
            Filter = typeof(LookupOfertaPeriodoInscricaoFiltroViewModel);
            this.Service<IOfertaService>(nameof(IOfertaService.BuscarArvoreHierarquiaOfertaProcessoComGrupo));
            this.SelectService<IOfertaService>(nameof(IOfertaService.BuscarOfertasInscricoes));
            Transformer = typeof(LookupOfertaPeriodoInscricaoFolhaTransformer);
            CustomView = @"SMC.Inscricoes.UI.Mvc.dll/Areas/INS/Lookups/Oferta/Views/_LookupOfertaPeriodoInscricao";
            this.HideSeq = true;
            PrepareFilter = typeof(LookupOfertaPeriodoInscricaoPrepareFilter);
        }

    }
}
