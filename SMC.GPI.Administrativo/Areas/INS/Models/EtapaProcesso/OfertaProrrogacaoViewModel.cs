using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class OfertaProrrogacaoViewModel : ISMCMappable
    {
        public long SeqOferta { get; set; }

        public string Descricao { get; set; }

        [SMCDateTimeMode(SMCDateTimeMode.DateTime)]
        public DateTime? DataFimAntiga { get; set; }

        [SMCDateTimeMode(SMCDateTimeMode.DateTime)]
        public DateTime DataFim { get; set; }

        public string CssClassData { get { return DataFim < DataFimAntiga.Value ? "smc-gpi-antecipacao-data" : (DataFim > DataFimAntiga.Value ? "smc-gpi-prorrogacao-data" : ""); } }


        public List<TaxaProrrogacaoViewModel> Taxas { get; set; }
    }
}
