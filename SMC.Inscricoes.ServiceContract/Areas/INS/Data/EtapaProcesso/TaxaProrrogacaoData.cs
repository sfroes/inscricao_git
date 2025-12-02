using SMC.Framework.Mapper;
using System;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    public class TaxaProrrogacaoData : ISMCMappable
    {
        public long SeqTaxa { get; set; }

        public string Descricao { get; set; }

        public DateTime DataVencimentoAntiga { get; set; }

        public DateTime DataVencimento { get; set; }

        public decimal? ValorAntigo { get; set; }

        public decimal ValorNovo { get; set; }

        public long? SeqEventoTaxa { get; set; }
    }
}
