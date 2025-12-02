using SMC.Framework.Mapper;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using System;
using System.Collections.Generic;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class TituloInscricaoVO : ISMCMappable
    {

        public long SeqInscricaoBoletoTitulo { get; set; }

        public long SeqInscricaoBoleto { get; set; }

        public int SeqTitulo { get; set; }

        public TipoBoleto TipoBoleto { get; set; }

        public decimal Valor { get; set; }

        public List<TaxaTituloInscricaoVO> Taxas { get; set; }

        public DateTime? DataCancelamento { get; set; }

        public DateTime DataVencimento { get; set; }

        public DateTime DataGeracao { get; set; }

        public DateTime? DataPagamento { get; set; }
    }
}
