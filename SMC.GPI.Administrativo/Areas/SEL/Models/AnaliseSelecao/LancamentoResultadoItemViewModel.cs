using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.GPI.Administrativo.Areas.SEL.Controllers;

namespace SMC.GPI.Administrativo.Areas.SEL.Models
{
    public class LancamentoResultadoItemViewModel : SMCViewModelBase
    {
        [SMCHidden]
        public long SeqTipoProcessoSituacao { get; set; }

        [SMCHidden]
        public long SeqInscricaoOferta { get; set; }

        [SMCSize(SMCSize.Grid5_24)]
        public long SeqInscricao { get; set; }

        [SMCSize(SMCSize.Grid19_24)]
        public string Candidato { get; set; }

        [SMCSize(SMCSize.Grid5_24)]
        public string Opcao { get; set; }

        [SMCSize(SMCSize.Grid16_24)]
        public short? NumeroOpcoesDesejadas { get; set; }

        [SMCSize(SMCSize.Grid24_24)]
        [SMCMultiline]
        [SMCReadOnly]
        public string Justificativa { get; set; }

        [SMCSize(SMCSize.Grid7_24)]
        [SMCRequired]
        [SMCSelect("Situacoes", NameDescriptionField = nameof(DescricaoResultadoSelecao))]
        public long? SeqResultadoSelecao { get; set; }

        [SMCSize(SMCSize.Grid7_24)]
        [SMCReadOnly]
        public string DescricaoResultadoSelecao { get; set; }

        [SMCSize(SMCSize.Grid8_24)]
        [SMCSelect]
        [SMCDependency(nameof(SeqResultadoSelecao), nameof(AnaliseSelecaoController.BuscarMotivosSituacao), "AnaliseSelecao", "SEL", true)]
        public long? SeqMotivo { get; set; }

        [SMCReadOnly]
        [SMCSize(SMCSize.Grid8_24)]
        public string Motivo { get; set; }

        [SMCSize(SMCSize.Grid3_24)]
        [SMCMinValue(0)]
        [SMCMaxValue(100)]
        [SMCDecimalDigits(2)]
        public decimal? Nota { get; set; }

        [SMCSize(SMCSize.Grid4_24)]
        [SMCMinValue(0)]
        [SMCMaxValue(100)]
        [SMCDecimalDigits(2)]
        public decimal? SegundaNota { get; set; }

        [SMCSize(SMCSize.Grid2_24)]
        [SMCMinValue(1)]
        public int? Classificacao { get; set; }

        [SMCMultiline]
        [SMCSize(SMCSize.Grid24_24)]
        public string ParecerResponsavel { get; set; }
    }
}