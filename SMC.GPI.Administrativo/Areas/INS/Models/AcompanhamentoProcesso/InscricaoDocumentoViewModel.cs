using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.Util;
using SMC.GPI.Administrativo.Areas.INS.Views.AcompanhamentoProcesso.App_LocalResources;
using System;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class InscricaoDocumentoViewModel : SMCViewModelBase, ISMCMappable
    {
        [SMCHidden]
        public long Seq { get; set; }

        [SMCHidden]
        [SMCMapProperty(nameof(Seq))]
        public long SeqDocumentoRequerido { get; set; }

        [SMCHidden]
        [SMCMapProperty("SeqInscricao")]
        public long SeqInscricao { get; set; }

        [SMCHidden]
        public long SeqTipoDocumento { get; set; }

        [SMCHidden]
        public long? SeqArquivoAnexado { get; set; }

        [SMCHidden]
        public bool BloquearTodosOsCampos { get; set; }

        [SMCHidden]
        public bool Obrigatorio { get; set; }

        [SMCHidden]
        public VersaoDocumento VersaoDocumentoExigido { get; set; }

        [SMCHidden]
        public bool PermiteEntregaPosterior { get; set; }

        [SMCHidden]
        public bool? PossuiArquivoAnexado { get { return ArquivoAnexado != null; } }

        [SMCHidden]
        public bool ExibirExibirObservacaoParaInscrito { get; set; }

        [SMCHidden]
        public bool ExibirInformacaoExibirObservacaoParaInscrito { get; set; }

        [SMCLegendItemDisplay(GenerateLabel = false)]
        [SMCGridLegend(DisplayDescription = false)]
        [SMCSize(SMCSize.Grid1_24, SMCSize.Grid1_24, SMCSize.Grid1_24, SMCSize.Grid1_24)]
        [SMCHideLabel]
        [SMCCssClass("smc-gpi-documento-legenda")]
        public SituacaoEntregaDocumento SituacaoEntregaDocumentoInicialLegenda { get { return SituacaoEntregaDocumentoInicial; } }

        [SMCHidden]
        [SMCMapForceFromTo]
        public SituacaoEntregaDocumento SituacaoEntregaDocumentoInicial { get; set; }

        [SMCSize(SMCSize.Grid5_24, SMCSize.Grid24_24, SMCSize.Grid5_24, SMCSize.Grid3_24)]
        [SMCCssClass("smc-gpi-situacao-entrega-documento smc-gpi-documento-legenda")]
        [SMCSelect(AjaxLoadAction = "BuscarSituacoesEntregaDocumento",
            AjaxLoadArea = "INS",
            AjaxLoadController = "AcompanhamentoProcesso",
            AjaxLoadProperties = new string[] { nameof(SeqDocumentoRequerido), nameof(SeqInscricao), nameof(SituacaoEntregaDocumento) },
            IgnoredEnumItems = new object[] { SituacaoEntregaDocumento.AguardandoAnaliseSetorResponsavel, SituacaoEntregaDocumento.AguardandoEntrega, SituacaoEntregaDocumento.AguardandoValidacao, SituacaoEntregaDocumento.Deferido, SituacaoEntregaDocumento.Indeferido, SituacaoEntregaDocumento.Pendente })]
        [SMCConditionalReadonly(nameof(BloquearTodosOsCampos), SMCConditionalOperation.Equals, true, PersistentValue = true)]
        [SMCConditionalRequired(nameof(BloquearTodosOsCampos), SMCConditionalOperation.NotEqual, true)]
        public SituacaoEntregaDocumento SituacaoEntregaDocumento { get; set; }
                
        [SMCSize(SMCSize.Grid5_24, SMCSize.Grid12_24, SMCSize.Grid5_24, SMCSize.Grid5_24)]
        [SMCConditionalReadonly(nameof(BloquearTodosOsCampos), SMCConditionalOperation.Equals, true, RuleName = "R1", PersistentValue = true)]
        [SMCConditionalReadonly(nameof(SituacaoEntregaDocumento), SMCConditionalOperation.NotEqual, nameof(SituacaoEntregaDocumento.Pendente), RuleName = "R2", PersistentValue = true)]
        [SMCConditionalRequired(nameof(SituacaoEntregaDocumento), SMCConditionalOperation.Equals, nameof(SituacaoEntregaDocumento.Pendente))]
        [SMCConditionalRule("R1 || R2")]
        [SMCMinDateNow(Exclusive = true)]
        [SMCCssClass("smc-gpi-documento-data-prazo-entrega")]
        [SMCDependency(nameof(SituacaoEntregaDocumento), nameof(AcompanhamentoProcessoController.PreencherDependencySituacaoEntrega), "AcompanhamentoProcesso", true, includedProperties: new string[] { nameof(Observacao), nameof(FormaEntregaDocumento), nameof(VersaoDocumento), nameof(VersaoDocumentoExigido), nameof(DataEntrega), nameof(ArquivoAnexado), nameof(DataPrazoEntrega), nameof(SeqDocumentoRequerido), nameof(ExibirObservacaoParaInscrito) })]
        public DateTime? DataPrazoEntrega { get; set; }

        [SMCSize(SMCSize.Grid5_24, SMCSize.Grid12_24, SMCSize.Grid5_24, SMCSize.Grid4_24)]
        [SMCConditionalReadonly(nameof(BloquearTodosOsCampos), SMCConditionalOperation.Equals, true, PersistentValue = true, RuleName = "R1")]
        [SMCConditionalRequired(nameof(SituacaoEntregaDocumento), SMCConditionalOperation.NotEqual, new object[] { nameof(SituacaoEntregaDocumento.Pendente), nameof(SituacaoEntregaDocumento.AguardandoEntrega) })]
        [SMCConditionalReadonly(nameof(SituacaoEntregaDocumento), SMCConditionalOperation.Equals, new object[] { nameof(SituacaoEntregaDocumento.Pendente), nameof(SituacaoEntregaDocumento.AguardandoEntrega) }, RuleName = "R2")]
        [SMCConditionalReadonly(nameof(PossuiArquivoAnexado), SMCConditionalOperation.Equals, true, RuleName = "R3", PersistentValue = true)]
        [SMCConditionalRule("R1 || R2 || (R2 && R3)")]
        [SMCDependency(nameof(SituacaoEntregaDocumento), nameof(AcompanhamentoProcessoController.PreencherDependencySituacaoEntrega), "AcompanhamentoProcesso", true, includedProperties: new string[] { nameof(Observacao), nameof(FormaEntregaDocumento), nameof(VersaoDocumento), nameof(VersaoDocumentoExigido), nameof(DataEntrega), nameof(ArquivoAnexado), nameof(DataPrazoEntrega), nameof(SeqDocumentoRequerido), nameof(ExibirObservacaoParaInscrito) })]
        [SMCCssClass("smc-gpi-documento-data-entrega")]
        public DateTime? DataEntrega { get; set; }

        // Valores: "Original", "Cópia simples" e "Cópia autenticada".
        [SMCSize(SMCSize.Grid6_24, SMCSize.Grid22_24, SMCSize.Grid7_24, SMCSize.Grid6_24)]
        [SMCSelect(IncludedEnumItems = new Object[] { DadosMestres.Common.Areas.PES.Enums.VersaoDocumento.CopiaAutenticada, DadosMestres.Common.Areas.PES.Enums.VersaoDocumento.CopiaSimples, DadosMestres.Common.Areas.PES.Enums.VersaoDocumento.Original })]
        [SMCConditionalReadonly(nameof(BloquearTodosOsCampos), SMCConditionalOperation.Equals, true, PersistentValue = true, RuleName = "R1")]
        [SMCConditionalRequired(nameof(SituacaoEntregaDocumento), SMCConditionalOperation.NotEqual, new object[] { nameof(SituacaoEntregaDocumento.Pendente), nameof(SituacaoEntregaDocumento.AguardandoEntrega) })]
        [SMCDependency(nameof(SituacaoEntregaDocumento), nameof(AcompanhamentoProcessoController.PreencherDependencySituacaoEntrega), "AcompanhamentoProcesso", true, includedProperties: new string[] { nameof(Observacao), nameof(FormaEntregaDocumento), nameof(VersaoDocumento), nameof(VersaoDocumentoExigido), nameof(DataEntrega), nameof(ArquivoAnexado), nameof(DataPrazoEntrega), nameof(SeqDocumentoRequerido), nameof(ExibirObservacaoParaInscrito) })]
        [SMCConditionalReadonly(nameof(SituacaoEntregaDocumento), SMCConditionalOperation.Equals, new object[] { nameof(SituacaoEntregaDocumento.Pendente), nameof(SituacaoEntregaDocumento.AguardandoEntrega) }, RuleName = "R2")]
        [SMCConditionalReadonly(nameof(PossuiArquivoAnexado), SMCConditionalOperation.Equals, true, RuleName = "R3", PersistentValue = true)]
        [SMCConditionalRule("R1 || R2 || (R2 && R3)")]
        [SMCCssClass("smc-gpi-documento-versao-documento")]
        public VersaoDocumento? VersaoDocumento { get; set; }

        [SMCHidden(SMCViewMode.List)]
        [SMCDisplay(DisplayAsInstructions = true)]
        [SMCSize(SMCSize.Grid1_24, SMCSize.Grid1_24, SMCSize.Grid1_24, SMCSize.Grid1_24)]
        [SMCCssClass("smc-gpi-documento-informacao-versao-exigida")]
        public string InformacaoVersaoExigida { get { return $"Versão exigida: {SMCEnumHelper.GetDescription(this.VersaoDocumentoExigido)}"; } }

        // Valores: "Correios", "E-mail",  "Presencial" e "Upload".
        [SMCSize(SMCSize.Grid5_24, SMCSize.Grid24_24, SMCSize.Grid5_24, SMCSize.Grid3_24)]
        [SMCSelect(IncludedEnumItems = new Object[] { DadosMestres.Common.Areas.PES.Enums.FormaEntregaDocumento.Correios, DadosMestres.Common.Areas.PES.Enums.FormaEntregaDocumento.Email, DadosMestres.Common.Areas.PES.Enums.FormaEntregaDocumento.Presencial, DadosMestres.Common.Areas.PES.Enums.FormaEntregaDocumento.Upload })]
        [SMCConditionalReadonly(nameof(BloquearTodosOsCampos), SMCConditionalOperation.Equals, true, PersistentValue = true, RuleName = "R1")]
        [SMCConditionalRequired(nameof(SituacaoEntregaDocumento), SMCConditionalOperation.NotEqual, new object[] { nameof(SituacaoEntregaDocumento.Pendente), nameof(SituacaoEntregaDocumento.AguardandoEntrega) })]
        [SMCConditionalReadonly(nameof(SituacaoEntregaDocumento), SMCConditionalOperation.Equals, new object[] { nameof(SituacaoEntregaDocumento.Pendente), nameof(SituacaoEntregaDocumento.AguardandoEntrega) }, RuleName = "R2")]
        [SMCConditionalReadonly(nameof(PossuiArquivoAnexado), SMCConditionalOperation.Equals, true, RuleName = "R3", PersistentValue = true)]
        [SMCConditionalRule("R1 || R2 || (R2 && R3)")]
        [SMCDependency(nameof(SituacaoEntregaDocumento), nameof(AcompanhamentoProcessoController.PreencherDependencySituacaoEntrega), "AcompanhamentoProcesso", true, includedProperties: new string[] { nameof(Observacao), nameof(FormaEntregaDocumento), nameof(VersaoDocumento), nameof(VersaoDocumentoExigido), nameof(DataEntrega), nameof(ArquivoAnexado), nameof(DataPrazoEntrega), nameof(SeqDocumentoRequerido), nameof(ExibirObservacaoParaInscrito) })]
        [SMCCssClass("smc-gpi-documento-forma-entrega-documento")]
        public FormaEntregaDocumento? FormaEntregaDocumento { get; set; }

        /// <summary>
        /// Campo de preenchimento obrigatório somente se tiver sido solicitada uma versão "Original" ou "Cópia autenticada"
        /// e o usuário registrar que a entrega foi feita em "cópia simples".
        /// </summary>
        [SMCSize(SMCSize.Grid7_24, SMCSize.Grid24_24, SMCSize.Grid7_24, SMCSize.Grid12_24)]
        [SMCDependency(nameof(SituacaoEntregaDocumento), nameof(AcompanhamentoProcessoController.PreencherDependencySituacaoEntrega), "AcompanhamentoProcesso", true, includedProperties: new string[] { nameof(Observacao), nameof(FormaEntregaDocumento), nameof(VersaoDocumento), nameof(VersaoDocumentoExigido), nameof(DataEntrega), nameof(ArquivoAnexado), nameof(DataPrazoEntrega), nameof(SeqDocumentoRequerido), nameof(ExibirObservacaoParaInscrito) })]
        [SMCConditionalReadonly(nameof(BloquearTodosOsCampos), SMCConditionalOperation.Equals, true, PersistentValue = true, RuleName = "R1")]
        [SMCConditionalRequired(nameof(VersaoDocumentoExigido), SMCConditionalOperation.Equals, nameof(DadosMestres.Common.Areas.PES.Enums.VersaoDocumento.CopiaAutenticada), RuleName = "Rule1")]
        [SMCConditionalRequired(nameof(VersaoDocumentoExigido), SMCConditionalOperation.Equals, nameof(DadosMestres.Common.Areas.PES.Enums.VersaoDocumento.Original), RuleName = "Rule2")]
        [SMCConditionalRequired(nameof(VersaoDocumento), SMCConditionalOperation.Equals, (int)DadosMestres.Common.Areas.PES.Enums.VersaoDocumento.CopiaSimples, RuleName = "Rule3")]
        [SMCConditionalReadonly(nameof(SituacaoEntregaDocumento), SMCConditionalOperation.Equals, nameof(SituacaoEntregaDocumento.AguardandoEntrega), PersistentValue = true, RuleName = "R2")]
        [SMCConditionalRule("Rule3 && (Rule1 || Rule2)")]
        [SMCConditionalRule("R1 || R2")]
        [SMCCssClass("smc-gpi-documento-observacao")]
        public string Observacao { get; set; }

        [SMCHidden(SMCViewMode.List)]
        [SMCSize(SMCSize.Grid6_24, SMCSize.Grid12_24, SMCSize.Grid7_24, SMCSize.Grid5_24)]
        [SMCRadioButtonList]
        [SMCDependency(nameof(SituacaoEntregaDocumento), nameof(AcompanhamentoProcessoController.PreencherDependencySituacaoEntrega), "AcompanhamentoProcesso", true, includedProperties: new string[] { nameof(Observacao), nameof(FormaEntregaDocumento), nameof(VersaoDocumento), nameof(VersaoDocumentoExigido), nameof(DataEntrega), nameof(ArquivoAnexado), nameof(DataPrazoEntrega), nameof(SeqDocumentoRequerido), nameof(ExibirObservacaoParaInscrito) })]
        [SMCDependency(nameof(Observacao), nameof(AcompanhamentoProcessoController.PreencherDependencySituacaoEntrega), "AcompanhamentoProcesso", true, includedProperties: new string[] { nameof(SituacaoEntregaDocumento), nameof(FormaEntregaDocumento), nameof(VersaoDocumento), nameof(VersaoDocumentoExigido), nameof(DataEntrega), nameof(ArquivoAnexado), nameof(DataPrazoEntrega), nameof(SeqDocumentoRequerido), nameof(ExibirObservacaoParaInscrito) })]
        [SMCConditionalReadonly(nameof(BloquearTodosOsCampos), SMCConditionalOperation.Equals, true, PersistentValue = true, RuleName = "R1")]
        [SMCConditionalReadonly(nameof(SituacaoEntregaDocumento), SMCConditionalOperation.NotEqual, new object[] { nameof(SituacaoEntregaDocumento.Indeferido), nameof(SituacaoEntregaDocumento.Pendente) }, PersistentValue = true, RuleName = "R2")]
        [SMCConditionalReadonly(nameof(Observacao), SMCConditionalOperation.Equals, "", PersistentValue = true, RuleName = "R3")]
        [SMCConditionalDisplay(nameof(ExibirExibirObservacaoParaInscrito), SMCConditionalOperation.Equals,true)]
        [SMCConditionalRule("R1 || R2 || R3")]
        [SMCCssClass("smc-gpi-documento-exibir-observacao-para-inscrito")]
        public bool ExibirObservacaoParaInscrito { get; set; }

        [SMCHidden(SMCViewMode.List)]
        [SMCDisplay(DisplayAsInstructions = true)]
        [SMCSize(SMCSize.Grid1_24, SMCSize.Grid3_24, SMCSize.Grid1_24, SMCSize.Grid2_24)]
        [SMCCssClass("smc-gpi-documento-informacao-exibir-observacao-para-inscrito")]
        [SMCHideLabel]
        [SMCConditionalDisplay(nameof(ExibirInformacaoExibirObservacaoParaInscrito), SMCConditionalOperation.Equals, true)]
        public string InformacaoExibirObservacaoParaInscrito { get { return UIResource.MSG_ExibirObservacaoParaInscrito; } }

        /*Se o documento for requerido como obrigatório e a forma de entrega for "Anexado" ou "E-mail", passa a ser obrigatório anexar o arquivo. */

        [SMCSize(SMCSize.Grid3_24, SMCSize.Grid24_24, SMCSize.Grid4_24, SMCSize.Grid3_24)]
        [SMCFile(ActionDownload = "DownloadDocumento", ControllerDownload = "Home", AreaDownload = "", HideDescription = true, DisplayFilesInContextWindow = true, MaxFileSize = 5242880)]
        [SMCConditionalReadonly(nameof(BloquearTodosOsCampos), SMCConditionalOperation.Equals, true, PersistentValue = true, RuleName = "R1")]
        [SMCConditionalRequired(nameof(FormaEntregaDocumento), SMCConditionalOperation.Equals, (int)DadosMestres.Common.Areas.PES.Enums.FormaEntregaDocumento.Email, RuleName = "Rule1")]
        [SMCConditionalRequired(nameof(FormaEntregaDocumento), SMCConditionalOperation.Equals, (int)DadosMestres.Common.Areas.PES.Enums.FormaEntregaDocumento.Upload, RuleName = "Rule2")]
        [SMCConditionalRequired(nameof(SituacaoEntregaDocumento), SMCConditionalOperation.NotEqual, new object[] { nameof(SituacaoEntregaDocumento.Pendente), nameof(SituacaoEntregaDocumento.AguardandoEntrega) }, RuleName ="Rule3")]
        [SMCConditionalReadonly(nameof(SituacaoEntregaDocumento), SMCConditionalOperation.Equals, new object[] { nameof(SituacaoEntregaDocumento.Pendente), nameof(SituacaoEntregaDocumento.AguardandoEntrega) }, PersistentValue = false, RuleName = "R2")]
        [SMCConditionalReadonly(nameof(PossuiArquivoAnexado), SMCConditionalOperation.Equals, true, RuleName = "R3", PersistentValue = true)]
        [SMCConditionalRule("R1 || R2 || (R2 && R3)")]
        [SMCConditionalRule("(Rule1 || Rule2) && Rule3")]
        [SMCCssClass("smc-gpi-documento-arquivo-anexado")]
        public SMCUploadFile ArquivoAnexado { get; set; }

        [SMCHidden(SMCViewMode.List)]
        [SMCConditionalDisplay(nameof(EntregaPosterior), SMCConditionalOperation.Equals, true)]
        [SMCSize(SMCSize.Grid1_24)]
        [SMCCssClass("smc-gpi-entrega-posterior")]
        [SMCHideLabel]       
        [SMCReadOnly]
        public bool EntregaPosterior { get; set; }
    }
}