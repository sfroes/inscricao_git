using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class TaxaPeriodoOfertaVO : ISMCMappable
    {
        public string TipoTaxa { get; set; }        

        public string Periodo {
            get 
            {
                return String.Format("{0} - {1}", DataInicio.ToShortDateString(), DataFim.ToShortDateString());
            }
        }
        public DateTime DataInicio { get; set; }

        public DateTime DataFim { get; set; }
        
        public short? NumeroMinimo { get; set; }

        public short? NumeroMaximo { get; set; }

        public long SeqEventoTaxa { get; set; }

        public string EventoTaxa { get; set; }

        public decimal? Valor { get; set; }

        public long SeqParametroCrei { get; set; }

        public string VencimentoTitulo { get; set; }

     
    }
}
