using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups;
using System;
using System.Collections.Generic;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class ProrrogacaoEtapaViewModel : SMCWizardViewModel, ISMCStatefulView, ISMCMappable
    {
        [SMCHidden]
        public long SeqEtapaProcesso { get; set; }

        [SMCHidden]
        [SMCFilter]
        public long SeqProcesso { get; set; }

        [SMCDataSource(SMCStorageType.Session)]
        public List<SMCDatasourceItem> EventosTaxa { get; set; }

        public string Descricao { get; set; }

        [SMCDateTimeMode(SMCDateTimeMode.DateTime)]
        public DateTime DataFim { get; set; }

        [SMCDateTimeMode(SMCDateTimeMode.DateTime)]
        public DateTime DataFimAntiga { get; set; }

        public string CssClassData { get { return DataFim < DataFimAntiga ? "smc-gpi-antecipacao-data" : (DataFim > DataFimAntiga ? "smc-gpi-prorrogacao-data" : ""); } }

        [SMCDependency("SeqProcesso")]
        [LookupOfertaPeriodoInscricao]
        [SMCRequired]
        public List<LookupOfertaPeriodoInscricaoViewModel> Ofertas { get; set; }

        public List<ConfiguracaoProrrogacaoViewModel> Configuracoes { get; set; }

        public int Passo { get; set; }

    }

}