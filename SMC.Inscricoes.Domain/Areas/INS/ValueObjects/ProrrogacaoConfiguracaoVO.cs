using SMC.Framework.Mapper;
using SMC.Inscricoes.Common.Areas.INS;
using System;
using System.Collections.Generic;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class ProrrogacaoConfiguracaoVO: ISMCMappable
    {
        public long SeqConfiguracaoEtapa { get; set; }

        public string Descricao { get; set; }

        public DateTime DataFimAntiga { get; set; }

        public DateTime? DataLimiteEntregaDocumentacaoAntiga { get; set; }

        public DateTime DataFim { get; set; }

        public DateTime DataFimMinina { get; set; }

        public DateTime? DataLimiteEntregaDocumentacao { get; set; }

        public List<long> SeqOfertas { get; set; }

        public List<OfertaProrrogacaoVO> Ofertas { get; set; }

        public List<TaxaProrrogacaoVO> Taxas { get; set; }
    }
}
