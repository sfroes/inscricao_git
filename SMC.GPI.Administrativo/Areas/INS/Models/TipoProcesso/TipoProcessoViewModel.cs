using SMC.DadosMestres.Common.Constants;
using SMC.DadosMestres.UI.Mvc.Area.GED.Lookups;
using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.UI.Mvc.Html;
using SMC.GPI.Administrativo.Areas.INS.Controllers;
using SMC.GPI.Administrativo.Areas.INS.Models.TipoProcesso;
using System.Collections.Generic;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class TipoProcessoViewModel : SMCViewModelBase, ISMCMappable
    {
        #region DataSources
        public List<SMCSelectListItem> ListaCamposInscrito { get; set; }
        #endregion

        [SMCReadOnly]
        [SMCFilter]
        [SMCSize(SMCSize.Grid2_24, SMCSize.Grid24_24, SMCSize.Grid2_24, SMCSize.Grid2_24)]
        public long? Seq { get; set; }

        [SMCRequired]

        [SMCFilter]
        [SMCSize(SMCSize.Grid10_24, SMCSize.Grid24_24, SMCSize.Grid10_24, SMCSize.Grid10_24)]
        [SMCMaxLength(100)]
        public string Descricao { get; set; }

        [SMCRegularExpression(REGEX.TOKEN, FormatErrorResourceKey = nameof(INS.Views.TipoProcesso.App_LocalResources.MetadataResource.MSG_Token_Expression_Error))]
        [SMCRequired]
        [SMCMinLength(3)]
        [SMCSize(SMCSize.Grid6_24, SMCSize.Grid24_24, SMCSize.Grid6_24, SMCSize.Grid6_24)]
        public string Token { get; set; }

        [SMCSelect("TiposTemplateProcessoSelect")]
        [SMCSize(SMCSize.Grid6_24, SMCSize.Grid24_24, SMCSize.Grid6_24, SMCSize.Grid6_24)]
        [SMCRequired]
        public long? SeqTipoTemplateProcessoSGF { get; set; }

        [SMCRegularExpression(REGEX.TOKEN, FormatErrorResourceKey = nameof(INS.Views.TipoProcesso.App_LocalResources.MetadataResource.MSG_Token_Expression_Error))]
        [SMCSize(SMCSize.Grid6_24, SMCSize.Grid24_24, SMCSize.Grid6_24, SMCSize.Grid6_24)]
        [SMCRequired]
        public string TokenResource { get; set; }

        [SMCRequired]
        [SMCSize(SMCSize.Grid6_24, SMCSize.Grid24_24, SMCSize.Grid6_24, SMCSize.Grid6_24)]
        [SMCRadioButtonList]
        public bool? ExigeCodigoOrigemOferta { get; set; }

        [SMCRequired]
        [SMCSize(SMCSize.Grid6_24, SMCSize.Grid24_24, SMCSize.Grid6_24, SMCSize.Grid6_24)]
        [SMCRadioButtonList]
        public bool? IntegraSGALegado { get; set; }

        [SMCSize(SMCSize.Grid6_24, SMCSize.Grid24_24, SMCSize.Grid6_24, SMCSize.Grid6_24)]
        [SMCRegularExpression("^(GTM-[A-Z0-9]{1,7})*(?:,(GTM-[A-Z0-9]{1,7}))*$")]
        public string IdsTagManager { get; set; }

        [SMCRequired]
        [SMCSize(SMCSize.Grid8_24)]
        [SMCRadioButtonList]
        public bool? BolsaExAluno { get; set; }

        [SMCRequired]
        [SMCSize(SMCSize.Grid6_24, SMCSize.Grid24_24, SMCSize.Grid6_24, SMCSize.Grid6_24)]
        [SMCRadioButtonList]
        public bool? IntegraGPC { get; set; }

        [SMCRequired]
        [SMCSize(SMCSize.Grid6_24, SMCSize.Grid24_24, SMCSize.Grid6_24, SMCSize.Grid6_24)]
        [SMCRadioButtonList]
        public bool? GestaoEventos { get; set; }

        [SMCRequired]
        [SMCSize(SMCSize.Grid8_24)]
        [SMCRadioButtonList]
        public bool? IsencaoP1 { get; set; }

        [SMCRequired]
        [SMCSize(SMCSize.Grid8_24)]
        [SMCRadioButtonList]
        public bool? PermiteRegerarTitulo { get; set; }

        public List<SMCDatasourceItem> TiposTemplateProcessoSelect { get; set; }
        //public TipoProcessoConversaodeArquivoViewModel ConversaodeArquivo { get; set; }

        [SMCCheckBoxList(nameof(ListaCamposInscrito), StorageType = SMCStorageType.Session)]
        [SMCOrientation(SMCOrientation.Vertical)]
        [SMCHideLabel]
        public List<long> CamposInscrito { get; set; }

        [SMCHtml]
        public string OrientacaoAceiteConversaoPDF { get; set; }

        [SMCHtml]
        public string TermoAceiteConversaoPDF { get; set; }

        [SMCHtml]
        public string TermoConsentimentoLGPD { get; set; }

        [SMCHidden]
        public List<SMCSelectListItem> ContextoBibliotecaSelect { get; set; }


        [SMCRequired]
        [SMCSize(SMCSize.Grid4_24)]
        [SMCRadioButtonList]
        public bool? HabilitaGed { get; set; }

        [SMCSelect(nameof(ContextoBibliotecaSelect), autoSelectSingleItem: true, SortBy = SMCSortBy.Description)]
        [SMCSize(SMCSize.Grid10_24)]
        [SMCConditionalRequired(nameof(HabilitaGed), SMCConditionalOperation.Equals, true)]
        public long? SeqContextoBibliotecaGed { get; set; }


        [SMCHidden]
        [SMCDependency(nameof(SeqContextoBibliotecaGed), nameof(TipoProcessoController.BuscarSeqContexto), "TipoProcesso", true)]
        public long? SeqContexto { get; set; }


        [LookupHierarquiaClassificacao]
        [SMCSize(SMCSize.Grid10_24)]
        [SMCDependency(nameof(SeqContexto))]
        [SMCConditionalRequired(nameof(SeqContexto), SMCConditionalOperation.GreaterThen, 0)]
        [SMCConditionalReadonly(nameof(SeqContexto), SMCConditionalOperation.Equals, 0)]
        public LookupHierarquiaClassificacaoViewModel SeqHierarquiaClassificacaoGed { get; set; }


        public SMCMasterDetailList<TipoProcessoTipoTaxaViewModel> TiposTaxa { get; set; }
        public List<SMCDatasourceItem> TiposTaxaSelect { get; set; }

        [SMCRequired]
        public SMCMasterDetailList<TipoProcessoTemplateProcessoViewModel> Templates { get; set; }
        public List<SMCSelectListItem> TemplatesProcessoSelect { get; set; }

        [SMCRequired]
        // FIX: (CAROL) Os valores máximos e mínimos não podem ser fixos. Vão variar dependendo da quantidade de situações
        public SMCMasterDetailList<TipoProcessoSituacaoViewModel> Situacoes { get; set; }

        [SMCDetail]
        public SMCMasterDetailList<TipoProcessoDocumentoViewModel> Documentos { get; set; }
        public List<SMCDatasourceItem> TiposDocumentoSelect { get; set; }

        [SMCDetail]
        public SMCMasterDetailList<ConsistenciaTipoProcessoViewModel> Consistencias { get; set; }

        [SMCRequired]
        [SMCSize(SMCSize.Grid8_24)]
        [SMCRadioButtonList]
        public bool? HabilitaPercentualDesconto { get; set; }

        [SMCRequired]
        [SMCSize(SMCSize.Grid8_24)]
        [SMCRadioButtonList]
        [SMCConditionalReadonly(nameof(HabilitaPercentualDesconto), SMCConditionalOperation.Equals, false)]
        [SMCDependency(nameof(HabilitaPercentualDesconto), nameof(TipoProcessoController.VerficaHabilitaPercentualDesconto), "TipoProcesso", true)]

        public bool? ValidaLimiteDesconto { get; set; }


    }


}