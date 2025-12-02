using SMC.Framework.Mapper;
using SMC.Inscricoes.Common.Areas.INS;
using System;
using System.Collections.Generic;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class OfertaProrrogacaoVO
    {
        public long SeqOferta { get; set; }

        public string Descricao { get; set; }

        public DateTime? DataFimAntiga { get; set; }

        public DateTime DataFim { get; set; }

        public List<TaxaProrrogacaoVO> Taxas { get; set; }
    }
}
