using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.GPI.Administrativo.Areas.INS.Controllers;
using SMC.GPI.Administrativo.Models;
using SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class TaxaOfertaViewModel : SMCViewModelBase, ISMCMappable
    {        
        [SMCHidden]
        public long Seq { get; set; }

        [SMCSelect("TaxasSelect")]
        [SMCSize(SMCSize.Grid6_24)]
        [SMCRequired]   
        [SMCReadOnly(SMCViewMode.Insert)]     
        public long SeqTaxa { get; set; }

        [SMCHidden]
        public long SeqOferta { get; set; }

        [SMCRequired]
        [SMCDateTimeMode(SMCDateTimeMode.Date)]
        [SMCSize(SMCSize.Grid4_24)]
        public DateTime? DataInicio { get; set; }

        [SMCRequired]
        [SMCDateTimeMode(SMCDateTimeMode.Date)]
        [SMCSize(SMCSize.Grid4_24)]
        [SMCMinDate(minDateProperty: "DataInicio")]
        public DateTime? DataFim { get; set; }

        [SMCConditionalRequired(nameof(CobrarPorQtdOferta), false)]
        [SMCConditionalReadonly(nameof(CobrarPorQtdOferta), true)]
        [SMCRequired]
        [SMCSize(SMCSize.Grid4_24)]
        [SMCMinValue(0)]
        [SMCMaxValue(999)]
        [SMCMask("999")]
        public short? NumeroMinimo { get; set; }

        [SMCConditionalReadonly(nameof(CobrarPorQtdOferta), true)]
        [SMCSize(SMCSize.Grid4_24)]
        [SMCMinValue(1)]
        [SMCMaxValue(999)]
        [SMCMask("999")]
        public short? NumeroMaximo { get; set; }
        
        [SMCRequired]
        [SMCSize(SMCSize.Grid16_24)]
        [SMCSelect("EventosTaxasSelect")]
        public int SeqEventoTaxa { get; set; }

        [SMCRequired]
        [SMCSize(SMCSize.Grid6_24)]
        [SMCSelect("ParametrosCreiSelect")]
        public int SeqParametroCrei { get; set; }

        [SMCHidden]
        public long? SeqPermissaoInscricaoForaPrazo { get; set; }

        [SMCHidden]
        [SMCDependency(nameof(SeqTaxa), nameof(HierarquiaOfertaController.BuscarTaxaCobraPorQtdOferta), "HierarquiaOferta", false)]
        public bool? CobrarPorQtdOferta { get; set; }
    }
}