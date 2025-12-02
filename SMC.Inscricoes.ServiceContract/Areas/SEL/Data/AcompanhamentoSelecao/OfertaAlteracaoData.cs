using SMC.Framework.Mapper;
using SMC.Framework.Model;
using System;

namespace SMC.Inscricoes.ServiceContract.Areas.SEL.Data
{
    public class OfertaAlteracaoData : ISMCMappable
    {
        public long? SeqInscricaoOferta { get; set; }

        public string Candidato { get; set; }

        public long? SeqOferta { get; set; }

        public string Oferta { get; set; }

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

        public long SeqProcesso { get; set; }

    }
}