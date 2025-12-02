using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class OfertaTaxaVO : ISMCMappable
    {
        public long SeqOferta { get; set; }

        public string Descricao { get; set; }

        public string Periodo
        {
            get
            {
                if (DataInicio.HasValue && DataFim.HasValue)
                    return string.Format("{0} - {1}", DataInicio, DataFim);
                return string.Empty;
            }
        }
        public DateTime? DataInicio { get; set; }

        public DateTime? DataFim { get; set; }

        public List<TaxaPeriodoOfertaVO> Taxas { get; set; }
    }
}
