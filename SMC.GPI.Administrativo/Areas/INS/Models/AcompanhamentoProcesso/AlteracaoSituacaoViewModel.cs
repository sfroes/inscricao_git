using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.UI.Mvc.Html;
using System.Collections.Generic;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class AlteracaoSituacaoViewModel : SMCViewModelBase, ISMCMappable
    {
        [SMCHidden]
        public long SeqProcesso { get; set; }

        [SMCSize(SMCSize.Grid24_24)]
        public string DescricaoSituacaoAtual { get; set; }

        [SMCHidden]
        public long SeqTipoProcessoSituacao { get; set; }

        [SMCHidden]
        public bool Lote { get; set; }

        [SMCSize(SMCSize.Grid12_24)]
        [SMCSelect(nameof(SituacoesProcesso))]
        [SMCRequired]
        public long SeqTipoProcessoSituacaoDestino { get; set; }

        public List<SMCDatasourceItem> SituacoesProcesso { get; set; }

        [SMCSelect(nameof(Motivos))]
        [SMCDependency(nameof(SeqTipoProcessoSituacaoDestino), nameof(AcompanhamentoProcessoController.BuscarMotivosSituacao), "AcompanhamentoProcesso", true)]
        [SMCSize(SMCSize.Grid12_24)]
        public long? SeqMotivoSGF { get; set; }

        public List<SMCDatasourceItem> Motivos { get; set; }

        [SMCSize(SMCSize.Grid24_24)]
        [SMCMultiline]
        public string Justificativa { get; set; }

        [SMCDetail(SMCDetailType.Tabular)]
        public SMCMasterDetailList<DetalheAlteracaoSituacaoViewModel> Inscricoes { get; set; }

        [SMCHidden]
        public string BackUrl { get; set; }
    }
}