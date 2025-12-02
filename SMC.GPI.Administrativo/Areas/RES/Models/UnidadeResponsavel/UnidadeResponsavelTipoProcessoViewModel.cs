using SMC.DadosMestres.Common.Constants;
using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.UI.Mvc.Html;
using SMC.GPI.Administrativo.Areas.RES.Models.UnidadeResponsavel;
using SMC.GPI.Administrativo.Areas.RES.Views.UnidadeResponsavel.PartialsParametros.App_LocalResources;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SMC.GPI.Administrativo.Areas.RES.Models
{
    public class UnidadeResponsavelTipoProcessoViewModel : SMCViewModelBase, ISMCMappable
    {
        public UnidadeResponsavelTipoProcessoViewModel()
        {
            Ativo = true;
        }

        [SMCKey]
        [SMCHidden]
        public long Seq { get; set; }

        [SMCHidden]
        public long SeqUnidadeResponsavel { get; set; }

        [SMCRequired]
        [SMCReadOnly(SMCViewMode.Edit)]
        [SMCSelect("TiposProcesso")]
        [SMCSize(SMCSize.Grid18_24)]
        public long? SeqTipoProcesso { get; set; }

        [SMCReadOnly(SMCViewMode.Edit)]
        [SMCRequired]
        [SMCSize(SMCSize.Grid6_24)]
        [SMCRadioButtonList]
        [SMCMapForceFromTo]
        public bool? Ativo { get; set; }
        
        [SMCDetail]
        [SMCSize(SMCSize.Grid24_24)]
        // Lista de itens do detalhe
        public SMCMasterDetailList<UnidadeResponsavelDetalheTipoHierarquiaOfertaViewModel> DetalhesTiposHierarquiaOferta { get; set; }

        [SMCDetail(SMCDetailType.Block)]
        [SMCSize(SMCSize.Grid24_24)]
        // Lista de itens do detalhe
        public SMCMasterDetailList<UnidadeResponsavelTipoProcessoIdentidadeVisualViewModel> IdentidadesVisuais { get; set; }

        public List<SMCSelectListItem> TiposProcesso { get; set; }

        // Datasource pro select do detalhe        
        public List<SMCDatasourceItem> TiposHierarquiaOferta { get; set; }     // Datasource pro select do detalhe
                                                                               //         
        public List<SMCDatasourceItem> LayoutMensagemEmail { get; set; }        
    }
}