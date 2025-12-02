using SMC.Framework.Mapper;
using SMC.Inscricoes.Common.Areas.INS;
using System;
using System.Collections.Generic;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class TaxaProrrogacaoVO : ISMCMappable
    {
        public long SeqTaxa { get; set; }

        public string Descricao { get; set; }

        public DateTime DataVencimentoAntiga { get; set; }

        public DateTime DataVencimento { get; set; }

        public decimal? ValorAntigo { get; set; }

        public decimal? ValorNovo { get; set; }

        public long? SeqEventoTaxa { get; set; }
    }
}
