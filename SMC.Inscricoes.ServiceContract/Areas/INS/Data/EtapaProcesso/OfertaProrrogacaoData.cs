using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    public class OfertaProrrogacaoData :ISMCMappable
    {
        public long SeqOferta { get; set; }

        public string Descricao { get; set; }

        public DateTime? DataFimAntiga { get; set; }

        public DateTime DataFim { get; set; }


        public List<TaxaProrrogacaoData> Taxas { get; set; }
    }
}
