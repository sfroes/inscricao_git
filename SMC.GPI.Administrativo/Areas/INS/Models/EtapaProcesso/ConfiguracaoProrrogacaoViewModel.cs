using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups;
using System;
using System.Collections.Generic;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{


    public class ConfiguracaoProrrogacaoViewModel : SMCViewModelBase
    {
        public ConfiguracaoProrrogacaoViewModel()
        {
            Taxas = new List<TaxaProrrogacaoViewModel>();
        }

        [SMCMapForceFromTo]
        public List<TaxaProrrogacaoViewModel> Taxas { get; set; }

        [SMCMapForceFromTo]
        public List<OfertaProrrogacaoViewModel> Ofertas { get; set; }

        public List<long> SeqOfertas { get; set; }

        [SMCHidden]
        public long SeqConfiguracaoEtapa { get; set; }

        [SMCSize(SMCSize.Grid24_24)]
        public string Descricao { get; set; }

        [SMCSize(SMCSize.Grid5_24)]
        [SMCDateTimeMode(SMCDateTimeMode.DateTime)]
        [SMCMinDate("DataFimMinina")]
        [SMCRequired]
        public DateTime DataFim { get; set; }

        [SMCDateTimeMode(SMCDateTimeMode.DateTime)]
        [SMCSize(SMCSize.Grid5_24)]        
        [SMCReadOnly]
        public DateTime DataFimAntiga { get; set; }

        [SMCDateTimeMode(SMCDateTimeMode.DateTime)]
        [SMCHidden]
        public DateTime DataFimMinina { get; set; }

        public string CssClassData { get { return DataFim < DataFimAntiga ? "smc-gpi-antecipacao-data" : (DataFim > DataFimAntiga ? "smc-gpi-prorrogacao-data" : ""); } }

        [SMCSize(SMCSize.Grid5_24)]
        [SMCDateTimeMode(SMCDateTimeMode.DateTime)]
        [SMCMinDate("DataFimMinina")]
        public DateTime? DataLimiteEntregaDocumentacao { get; set; }

        [SMCDateTimeMode(SMCDateTimeMode.DateTime)]
        public DateTime? DataLimiteEntregaDocumentacaoAntiga { get; set; }

        public string CssClassDocumentacao { get { return DataLimiteEntregaDocumentacao < DataLimiteEntregaDocumentacaoAntiga ? "smc-gpi-antecipacao-data" : (DataLimiteEntregaDocumentacao > DataLimiteEntregaDocumentacaoAntiga ? "smc-gpi-prorrogacao-data" : ""); } }


    }


}