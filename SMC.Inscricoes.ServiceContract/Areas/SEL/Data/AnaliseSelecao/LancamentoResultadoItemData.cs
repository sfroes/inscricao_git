using SMC.Framework.Mapper;

namespace SMC.Inscricoes.ServiceContract.Areas.SEL.Data
{
    public class LancamentoResultadoItemData : ISMCMappable
    {
        public long SeqTipoProcessoSituacao { get; set; }

        public long SeqInscricaoOferta { get; set; }

        public long SeqInscricao { get; set; }

        public string Candidato { get; set; }

        public string Opcao { get; set; }

        public short? NumeroOpcoesDesejadas { get; set; }

        public string Justificativa { get; set; }

        public long? SeqResultadoSelecao { get; set; }

        public string DescricaoResultadoSelecao { get; set; }

        public long? SeqMotivo { get; set; }

        public string Motivo { get; set; }

        public decimal? Nota { get; set; }

        public decimal? SegundaNota { get; set; }

        public int? Classificacao { get; set; }

        public string ParecerResponsavel { get; set; }
    }
}