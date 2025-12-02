using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class OfertaVO : ISMCMappable
    {
        public long? SeqInscricaoOferta { get; set; }

        public string Nome { get; set; }

        public long? SeqOferta { get; set; }

        public string Oferta { get; set; }

        public string Candidato { get; set; }

        public long? SeqOfertaAtual { get; set; }

        public string OfertaAtual { get; set; }

        public long? SeqGrupoOferta { get; set; }

        public long? SeqInscricao { get; set; }

        public long? SeqOfertaOriginal { get; set; }

        public string JustificativaInscricao { get; set; }

        public string JustificativaAlteracaoOferta { get; set; }

        public string OfertaOriginal { get; set; }

        public string UsuarioAlteracaoOferta { get; set; }

        public DateTime? DataAlteracaoOferta { get; set; }

        public bool OfertasIguais { get; set; }

        public bool? Exportado { get; set; }

        public string NomeInscrito { get;  set; }

        public string DescricaoOferta { get;  set; }

        public decimal? LimitePercentualDesconto { get; set; }


        public DateTime? DataInicioAtividade { get; set; }

        public DateTime? DataFimAtividade { get; set; }

        public int? CargaHorariaAtividade { get; set; }

        public bool? ExibirPeriodoAtividadeOferta { get; set; }

        public long SeqProcesso { get; set; }

        public string HierarquiaCompleta { get; set; }

        public long? SeqInscrito { get; set; }

    }
}