using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    public class ProrrogacaoConfiguracaoData: ISMCMappable
    {
        public long SeqConfiguracaoEtapa { get; set; }

        public string Descricao { get; set; }

        public DateTime DataFimAntiga { get; set; }

        public DateTime? DataLimiteEntregaDocumentacaoAntiga { get; set; }

        public DateTime DataFim { get; set; }

        public DateTime? DataLimiteEntregaDocumentacao { get; set; }

        public DateTime DataFimMinina { get; set; }

        public List<long> SeqOfertas { get; set; }

        [SMCMapForceFromTo]
        public List<TaxaProrrogacaoData> Taxas { get; set; }

        [SMCMapForceFromTo]
        public List<OfertaProrrogacaoData> Ofertas { get; set; }
    }
}
