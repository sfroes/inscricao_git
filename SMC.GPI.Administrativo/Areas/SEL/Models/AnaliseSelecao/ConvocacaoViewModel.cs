using SMC.Framework;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.GPI.Administrativo.Areas.INS;
using System.Collections.Generic;

namespace SMC.GPI.Administrativo.Areas.SEL.Models
{
    public class ConvocacaoViewModel : SMCViewModelBase
    {
        public string GrupoOferta { get; set; }

        public string DescricaoOferta { get; set; }

        public string SituacaoAtual { get; set; }

        [SMCHidden]
        public long SeqProcesso { get; set; }

        [SMCDataSource(SMCStorageType.TempData)]
        public List<SMCDatasourceItem> SituacoesDestino { get; set; }

        [SMCRequired]
        [SMCSelect(nameof(SituacoesDestino))]
        [SMCSize(SMCSize.Grid12_24)]
        public long SeqTipoProcessoSituacaoDestino { get; set; }

        [SMCSelect]
        [SMCDependency(nameof(SeqTipoProcessoSituacaoDestino), nameof(AcompanhamentoProcessoController.BuscarMotivosSituacao), "AcompanhamentoProcesso", "INS", true)]
        [SMCSize(SMCSize.Grid12_24)]
        public long? SeqMotivoSGF { get; set; }

        [SMCMultiline]
        [SMCSize(SMCSize.Grid24_24)]
        public string Justificativa { get; set; }

        public List<ConvocacaoItemViewModel> Convocados { get; set; }
    }
}