using Newtonsoft.Json;
using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.UI.Mvc.Html;
using SMC.GPI.Administrativo.Models;
using SMC.Localidades.UI.Mvc.DataAnnotation;
using SMC.Localidades.UI.Mvc.Models;
using System;
using System.Collections.Generic;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class OfertaViewModel : AssociarItemHierarquiaOfertaViewModel, ISMCMappable
    {
        #region Datasources

        [SMCDataSource(SMCStorageType.Session)]
        public List<SMCDatasourceItem> TaxasSelect { get; set; }

        [SMCDataSource(SMCStorageType.Session)]
        public List<SMCDatasourceItem> EventosTaxasSelect { get; set; }

        [SMCDataSource(SMCStorageType.Session)]
        public List<SMCDatasourceItem> ParametrosCreiSelect { get; set; }

        #endregion Datasources

        public OfertaViewModel()
        {
            TiposTelefone = new List<SMCDatasourceItem>();
            CodigosAutorizacao = new SMCMasterDetailList<CodigoAutorizacaoDetalheViewModel>();
            TaxasSelect = new List<SMCDatasourceItem>();
            Taxas = new SMCMasterDetailList<TaxaOfertaViewModel>();
            Ativo = true;
        }

        [SMCRequired]
        [SMCSize(SMCSize.Grid4_24)]
        [SMCRadioButtonList]
        [SMCMapForceFromTo]
        public bool Ativo { get; set; }

        [SMCRequired]
        [SMCSize(SMCSize.Grid4_24)]
        [SMCDateTimeMode(SMCDateTimeMode.DateTime)]
        public DateTime? DataInicio { get; set; }

        [SMCRequired]
        [SMCSize(SMCSize.Grid4_24)]
        [SMCDateTimeMode(SMCDateTimeMode.DateTime)]
        [SMCMinDate(minDateProperty: "DataInicio")]
        public DateTime? DataFim { get; set; }

        [SMCRequired]
       // [SMCSize(SMCSize.Grid4_24)]
        [SMCMinValue(0)]
        public int? NumeroVagas { get; set; }

        [SMCSize(SMCSize.Grid12_24)]
        [SMCSelect("GruposOferta", autoSelectSingleItem: true)]
        public long? SeqGrupoOferta { get; set; }
    
        public List<SMCDatasourceItem> GruposOferta { get; set; }

        [SMCRequired]
       // [SMCSize(SMCSize.Grid6_24)]
        [SMCRadioButtonList]
        public bool InscricaoSoPorCliente { get; set; }

       // [SMCSize(SMCSize.Grid4_24)]
        public int? CodigoOrigem { get; set; }

        [SMCRequired]
        //[SMCSize(SMCSize.Grid7_24)]
        [SMCDateTimeMode(SMCDateTimeMode.DateTime)]
        [SMCSize(SMCSize.Grid5_24)]
        public Nullable<DateTime> DataInicioAtividade { get; set; }

        [SMCRequired]
       // [SMCSize(SMCSize.Grid7_24)]
        [SMCMinDate(minDateProperty: nameof(DataInicioAtividade))]
        [SMCSize(SMCSize.Grid5_24)]
        [SMCDateTimeMode(SMCDateTimeMode.DateTime)]
        public Nullable<DateTime> DataFimAtividade { get; set; }

        [SMCRequired]
        [SMCSize(SMCSize.Grid6_24)]
        public int? CargaHorariaAtividade { get; set; }

        [SMCHidden]
        public bool HabilitaGestaoEventos { get; set; }


        [SMCSize(SMCSize.Grid6_24)]
        [SMCMaxValue(100)]
        [SMCMinValue(0)]
        [SMCDecimalDigits(2)]
        [SMCConditionalDisplay(nameof(HabilitaPercentualDesconto), SMCConditionalOperation.Equals, true)]
        [SMCConditionalRequired(nameof(HabilitaPercentualDesconto), SMCConditionalOperation.Equals,true)]
        public decimal? LimitePercentualDesconto { get; set; }

        //[SMCSize(SMCSize.Grid4_24)]
        [SMCMinValue(0)]
        [SMCConditionalDisplay(nameof(BolsaExAluno), true)]
        [SMCConditionalRequired(nameof(BolsaExAluno), true)]
        public int? NumeroVagasBolsa { get; set; }

        [SMCRadioButtonList]
        [SMCRequired]
        [SMCSize(SMCSize.Grid12_24)]
        public bool InscricaoSoComCodigo { get; set; }

        [SMCRadioButtonList]
        [SMCRequired]
        [SMCSize(SMCSize.Grid6_24)]
        public bool PermiteVariosCodigos { get; set; }

        [SMCRadioButtonList]
        [SMCMapForceFromTo]
        [SMCSize(SMCSize.Grid6_24)]
        [SMCConditionalRequired(nameof(HabilitaGestaoEventos), SMCConditionalOperation.Equals,true)]
        public bool? HabilitaCheckin { get; set; }

        public List<SMCDatasourceItem> CodigosAutorizacaoSelect { get; set; }

        [SMCMapForceFromTo]
        [SMCDetail]
        [SMCSize(SMCSize.Grid24_24)]
        public SMCMasterDetailList<CodigoAutorizacaoDetalheViewModel> CodigosAutorizacao { get; set; }

        [SMCSize(SMCSize.Grid12_24)]
        [SMCMaxLength(100)]
        public string NomeResponsavel { get; set; }

        [SMCMapForceFromTo]
        [Phone]
        [SMCSize(SMCSize.Grid24_24)]
        public PhoneList Telefones { get; set; }

        [JsonIgnore]
        public List<SMCDatasourceItem> TiposTelefone { get; set; }

        [SMCMapForceFromTo]
        [SMCDetail]
        [SMCSize(SMCSize.Grid24_24)]
        public SMCMasterDetailList<EnderecoEletronicoViewModel> EnderecosEletronicos { get; set; }

        [SMCDateTimeMode(SMCDateTimeMode.Date)]
        [SMCSize(SMCSize.Grid8_24)]
        public Nullable<DateTime> DataCancelamento { get; set; }

        [SMCSize(SMCSize.Grid24_24)]
        [SMCMultiline]
        public string MotivoCancelamento { get; set; }

        [SMCMapForceFromTo]
        public List<TaxaOfertaViewModel> Taxas { get; set; }

        [SMCDetail(SMCDetailType.Block)]
        public SMCMasterDetailList<TaxaOfertaViewModel> TaxasOferta { get; set; }

        public IEnumerable<TaxaOfertaViewModel> TaxasPermissoes { get; set; }

        [SMCSize(SMCSize.Grid24_24)]
        [SMCHtml]
        public string DescricaoComplementar { get; set; }
        [SMCHidden]
        public bool HabilitaPercentualDesconto { get; set; }
    }
}