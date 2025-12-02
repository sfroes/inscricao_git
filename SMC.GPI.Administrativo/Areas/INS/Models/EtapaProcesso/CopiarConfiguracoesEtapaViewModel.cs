using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.UI.Mvc.Html;
using SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups;
using System.Collections.Generic;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class CopiarConfiguracoesEtapaViewModel : SMCViewModelBase, ISMCMappable
    {
        public CopiarConfiguracoesEtapaViewModel()
        {
            Configuracoes = new SMCMasterDetailList<CopiarConfiguracoesEtapaDetalheViewModel>();
            ConfiguracaoesEtapaOrigem = new List<SMCDatasourceItem>();
        }


        [SMCHidden]
        public long SeqEtapaProcesso { get; set; }

        [SMCHidden]
        public long SeqTipoTemplateProcessoOrigem { get; set; }

        [SMCHidden]
        [SMCDependency("ProcessoDestino", "BuscarTemplateProcesso", "ConfiguracaoEtapa", false)]
        public long SeqTipoTemplateProcessoDestino { get; set; }

        [SMCHidden]
        public long SeqProcessoOrigem { get; set; }

        [SMCHidden]
        [SMCDependency("ProcessoDestino", "VerificarIdiomasDestino", "ConfiguracaoEtapa", false, "SeqProcessoOrigem")]
        public bool NaoPossuiIdiomasComuns { get; set; }

        [SMCRequired]
        [LookupProcesso]
        [SMCSize(SMCSize.Grid15_24)]
        public GPILookupViewModel ProcessoDestino { get; set; }

        [SMCSize(SMCSize.Grid24_24)]
        public bool CopiarPaginas { get; set; }

        [SMCSize(SMCSize.Grid24_24)]
        public bool CopiarDocumentacao { get; set; }

        public List<SMCDatasourceItem> ConfiguracaoesEtapaOrigem { get; set; }

        [SMCDetail(SMCDetailType.Block, min: 1)]
        public SMCMasterDetailList<CopiarConfiguracoesEtapaDetalheViewModel> Configuracoes { get; set; }
    }
}