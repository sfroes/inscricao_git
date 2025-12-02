using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups;
using System;
using System.Collections.Generic;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{


    public class TaxaProrrogacaoViewModel : SMCViewModelBase
    {
        [SMCHidden]
        public long SeqTaxa { get; set; }

        [SMCSize(SMCSize.Grid9_24)]
        [SMCSelect("EventosTaxa")]        
        public long? SeqEventoTaxa { get; set; }  

        [SMCSize(SMCSize.Grid8_24)]
        public string Descricao { get; set; }

        [SMCSize(SMCSize.Grid4_24)]
        [SMCRequired]
        public DateTime DataVencimento { get; set; }

        [SMCSize(SMCSize.Grid4_24)]
        public DateTime DataVencimentoAntiga { get; set; }

        public string CssClassData { get { return DataVencimento < DataVencimentoAntiga ? "smc-gpi-antecipacao-data" : (DataVencimento > DataVencimentoAntiga ? "smc-gpi-prorrogacao-data" : ""); } }

        [SMCSize(SMCSize.Grid4_24)]
        public decimal? ValorAntigo { get; set; }

        [SMCSize(SMCSize.Grid4_24)]
        public decimal? ValorNovo { get; set; }

        public string CssClassValor { get { return ValorNovo < ValorAntigo ? "smc-gpi-antecipacao-data" : (ValorNovo > ValorAntigo ? "smc-gpi-prorrogacao-data" : ""); } }
    }
}