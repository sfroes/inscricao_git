using Newtonsoft.Json;
using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.UI.Mvc.Html;
using SMC.GPI.Administrativo.Models;
using SMC.Inscricoes.Common.Shared;
using SMC.Localidades.UI.Mvc.DataAnnotation;
using SMC.Localidades.UI.Mvc.Models;
using System;
using System.Collections.Generic;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class ProcessoViewModel : SMCViewModelBase, ISMCMappable
    {
        #region DataSources

        public List<SMCDatasourceItem> EventosGRA { get; set; }

        public List<SMCSelectListItem> Clientes { get; set; }

        public List<SMCDatasourceItem> TemplatesProcesso { get; set; }

        public List<SMCDatasourceItem> TiposHierarquiaOferta { get; set; }

        public List<SMCDatasourceItem> TiposProcesso { get; set; }

        public List<SMCSelectListItem> UnidadesResponsaveis { get; set; }

        //Documentos e Formulários
        public List<SMCDatasourceItem> TiposDocumento { get; set; }

        public List<SMCDatasourceItem<string>> ConfiguracoesAssinaturaGad { get; set; }

        public List<SMCDatasourceItem> TiposFormulario { get; set; }

        public List<SMCDatasourceItem> Formularios { get; set; }

        public List<SMCDatasourceItem> VisoesFormulario { get; set; }

        public List<SMCSelectListItem> ListaCamposInscrito { get; set; }

        public List<SMCDatasourceItem> EventosSae { get; set; }

        public List<SMCDatasourceItem> IdentidadesVisuais { get; set; }


        #endregion DataSources

        public ProcessoViewModel()
        {
            Idiomas = new SMCMasterDetailList<IdiomaProcessoViewModel>();
            TiposTelefone = new List<SMCDatasourceItem>();
            Taxas = new SMCMasterDetailList<TaxaProcessoViewModel>();
        }

        [SMCReadOnly]
        [SMCSize(SMCSize.Grid6_24)]
        public long? Seq { get; set; }

        [SMCRequired]
        [SMCSize(SMCSize.Grid18_24)]
        [SMCMaxLength(255)]
        public string Descricao { get; set; }

        [SMCSize(SMCSize.Grid8_24)]
        [SMCSelect(nameof(UnidadesResponsaveis))]
        [SMCRequired]
        public long SeqUnidadeResponsavel { get; set; }

        [SMCSize(SMCSize.Grid8_24)]
        [SMCSelect(nameof(TiposProcesso), AutoSelectSingleItem = true)]
        [SMCRequired]
        [SMCDependency("SeqUnidadeResponsavel", "BuscarTiposProcesso", "Processo", true)]
        public long SeqTipoProcesso { get; set; }

        [SMCSize(SMCSize.Grid8_24)]
        [SMCSelect(nameof(TiposHierarquiaOferta), AutoSelectSingleItem = true)]
        [SMCRequired]
        [SMCDependency("SeqTipoProcesso", "BuscarTiposHierarquiaOferta", "Processo", true, "SeqUnidadeResponsavel")]
        public long SeqTipoHierarquiaOferta { get; set; }

        [SMCSize(SMCSize.Grid8_24)]
        [SMCSelect(nameof(TemplatesProcesso), AutoSelectSingleItem = true)]
        [SMCRequired]
        [SMCDependency("SeqTipoProcesso", "BuscarTemplatesProcesso", "Processo", true)]
        public long SeqTemplateProcessoSGF { get; set; }

        [SMCSelect(nameof(IdentidadesVisuais), AutoSelectSingleItem = true)]
        [SMCSize(SMCSize.Grid8_24)]
        [SMCDependency(nameof(SeqUnidadeResponsavel), "BuscarIdentidadesVisuais", "Processo", true, new string[] {nameof(SeqTipoProcesso)})]
        [SMCDependency(nameof(SeqTipoProcesso), "BuscarIdentidadesVisuais", "Processo", true, new string[] {nameof(SeqUnidadeResponsavel)})]
        [SMCRequired]
        public long SeqUnidadeResponsavelTipoProcessoIdVisual { get; set; }

        [SMCSize(SMCSize.Grid8_24)]
        [SMCSelect(nameof(Clientes))]
        public long? SeqCliente { get; set; }

        [SMCRequired]
        [SMCMask("0000")]
        [SMCSize(SMCSize.Grid6_24)]
        public int? AnoReferencia { get; set; }

        [SMCRequired]
        [SMCMinValue(1)]
        [SMCMaxValue(2)]
        [SMCMask("0")]
        [SMCSize(SMCSize.Grid6_24)]
        public int? SemestreReferencia { get; set; }

        [SMCSize(SMCSize.Grid6_24)]
        [SMCDateTimeMode(SMCDateTimeMode.Date)]
        public DateTime? DataCancelamento { get; set; }

        [SMCSize(SMCSize.Grid6_24)]
        [SMCDateTimeMode(SMCDateTimeMode.Date)]
        public DateTime? DataEncerramento { get; set; }

        [SMCRequired]
        [SMCRadioButtonList]
        [SMCSize(SMCSize.Grid8_24)]
        public bool? ExibeRelacaoGeral { get; set; }

        [SMCRadioButtonList]
        [SMCSize(SMCSize.Grid8_24)]
        public bool? ExisteNumeroMaximoInscricoesPessoa
        {
            get
            {
                if (!this.Seq.HasValue)
                    return new Nullable<bool>();

                if (MaximoInscricoesPorInscrito.HasValue)
                    return MaximoInscricoesPorInscrito.Value > 0;
                return false;
            }
        }

        [SMCSize(SMCSize.Grid8_24)]
        [SMCConditionalReadonly("ExisteNumeroMaximoInscricoesPessoa", SMCConditionalOperation.NotEqual, true)]
        [SMCConditionalRequired("ExisteNumeroMaximoInscricoesPessoa", true)]
        [SMCMinValue(1)]
        public short? MaximoInscricoesPorInscrito { get; set; }

        [SMCDependency("SeqUnidadeResponsavel", "BuscarEventos", "Processo", true)]
        [SMCSelect(nameof(EventosGRA))]
        [SMCSize(SMCSize.Grid8_24)]
        public int? SeqEvento { get; set; }

        [SMCSize(SMCSize.Grid8_24)]
        [SMCRadioButtonList]
        public bool ControlaVagaInscricao { get; set; }

        [SMCSize(SMCSize.Grid8_24)]
        [SMCRadioButtonList]
        public bool ExibeArvoreFechada { get; set; }

        [SMCSize(SMCSize.Grid8_24)]
        public DateTime? DataInicioAtividade { get; set; }

        [SMCHidden]
        [SMCDependency("SeqTipoProcesso", "ConferirHabilitaDatasEvento", "Processo", true)]
        public bool HabilitaDatasEvento { get; set; }

        [SMCHidden]
        [SMCDependency(nameof(SeqTipoProcesso), "ConferirHabilitaGestaoEvento", "Processo", true)]
        public bool HabilitaGestaoEvento { get; set; }

        [SMCSize(SMCSize.Grid4_24)]
        [SMCConditionalDisplay(nameof(HabilitaDatasEvento), SMCConditionalOperation.Equals, true)]
        [SMCConditionalRequired(nameof(HabilitaDatasEvento), SMCConditionalOperation.Equals, true)]
        public DateTime? DataInicioEvento { get; set; }

        [SMCSize(SMCSize.Grid4_24)]
        [SMCConditionalDisplay(nameof(HabilitaDatasEvento), SMCConditionalOperation.Equals, true)]
        [SMCConditionalRequired(nameof(HabilitaDatasEvento), SMCConditionalOperation.Equals, true)]
        [SMCMinDate(nameof(DataInicioEvento))]
        public DateTime? DataFimEvento { get; set; }

        [SMCSize(SMCSize.Grid16_24)]
        [SMCSelect(nameof(EventosSae))]
        [SMCDependency(nameof(SeqUnidadeResponsavel), "BuscarEventosSae", "Processo", true, new string[] { nameof(AnoReferencia) })]
        [SMCDependency(nameof(AnoReferencia), "BuscarEventosSae", "Processo", true, new string[] { nameof(SeqUnidadeResponsavel) })]
        public long? CodigoEventoSAE { get; set; }

        [SMCSize(SMCSize.Grid10_24)]
        [SMCDateTimeMode(SMCDateTimeMode.Time)]
        public TimeSpan? HoraAberturaCheckin { get; set; }

        [SMCRadioButtonList]
        [SMCSize(SMCSize.Grid8_24)]
        [SMCConditionalDisplay(nameof(HabilitaDatasEvento), SMCConditionalOperation.Equals, true)]
        [SMCConditionalRequired(nameof(HabilitaDatasEvento), SMCConditionalOperation.Equals, true)]
        public bool? ExibirPeriodoAtividadeOferta { get; set; }

        [SMCRadioButtonList]
        [SMCSize(SMCSize.Grid6_24)]
        [SMCConditionalDisplay(nameof(HabilitaDatasEvento), SMCConditionalOperation.Equals, true)]
        [SMCConditionalRequired(nameof(HabilitaDatasEvento), SMCConditionalOperation.Equals, true)]
        public bool? VerificaCoincidenciaHorario { get; set; }



        [SMCMapForceFromTo]
        [SMCDetail(SMCDetailType.Modal)]
        [SMCSize(SMCSize.Grid24_24)]
        public SMCMasterDetailList<IdiomaProcessoViewModel> Idiomas { get; set; }

        #region Documentos e Formulários

        [SMCMapForceFromTo]
        [SMCSize(SMCSize.Grid24_24)]
        [SMCDetail(SMCDetailType.Block)]
        public SMCMasterDetailList<ConfiguracoesModeloDocumentoViewModel> ConfiguracoesModeloDocumento { get; set; }

        [SMCMapForceFromTo]
        [SMCDetail(SMCDetailType.Block, max: 1)]
        [SMCSize(SMCSize.Grid24_24)]

        public SMCMasterDetailList<ConfiguracoesFormularioViewModel> ConfiguracoesFormulario { get; set; }


        #endregion

        [SMCSize(SMCSize.Grid8_24)]
        [SMCMaxLength(100)]
        public string NomeContato { get; set; }

        [Phone]
        [SMCMapForceFromTo]
        [SMCSize(SMCSize.Grid24_24)]
        public PhoneList Telefones { get; set; }

        [JsonIgnore]
        public List<SMCDatasourceItem> TiposTelefone { get; set; }

        [SMCDetail]
        [SMCMapForceFromTo]
        [SMCSize(SMCSize.Grid24_24)]
        public SMCMasterDetailList<EnderecoEletronicoViewModel> EnderecosEletronicos { get; set; }

        [SMCDependency(nameof(SeqTipoProcesso), "BuscarCamposInscritoTipoProcesso", "Processo", false)]
        [SMCCheckBoxList(nameof(ListaCamposInscrito), StorageType = SMCStorageType.Session)]
        [SMCOrientation(SMCOrientation.Vertical)]
        [SMCHideLabel]
        public List<long> CamposInscrito { get; set; }

        [SMCDetail]
        [SMCMapForceFromTo]
        public SMCMasterDetailList<TaxaProcessoViewModel> Taxas { get; set; }

        public List<SMCDatasourceItem> TiposTaxa { get; set; }

        [SMCHidden]
        public Guid? UidProcesso { get; set; }

        [SMCHidden]
        [SMCDependency(nameof(SeqTipoProcesso), "BuscarIdsTagManager", "Processo", false)]
        public string IdsTagManager { get; set; }

     

        [SMCSize(SMCSize.Grid22_24)]
        [SMCLink(SMCLinkTarget.NewWindow)]
        public string LinkProcesso
        {
            get
            {
                if (this.UidProcesso != null)
                {
                    var link = HttpContextAmbiente.UrlAmbiente($"GPI.Inscricao/Home/IndexProcesso?uidProcesso={UidProcesso.ToString()}");

                    if (!string.IsNullOrEmpty(IdsTagManager))
                    {
                        link += $"&idsTagManager={IdsTagManager}";
                    }

                    if (!string.IsNullOrEmpty(TokenCssAlternativoSas))
                    {
                        link += $"&TokenCss={TokenCssAlternativoSas}";
                    }

                    return link;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        [SMCHidden]
        public bool PossuiIntegracao { get; set; }

        [SMCHidden]
        public string TokenCssAlternativoSas { get; set; }

    }
}