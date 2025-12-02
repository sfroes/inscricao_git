using SMC.Framework.Mapper;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class AcompanhamentoInscritoFiltroVO :  ISMCMappable
    {


        public long? Seq { get; set; }

        public long? SeqUnidadeResponsavel { get; set; }

        public long? SeqProcesso { get; set; }

        public int? SemestreReferencia { get; set; }

        public int? AnoReferencia { get; set; }

        public string DescricaoProcesso { get; set; }

        public long? SeqInscrito { get; set; }

        public string NomeInscrito { get; set; }

        public string Cpf { get; set; }

        public string NumeroPassaporte { get; set; }

        public long? SeqInscricoes { get; set; }
    }
}
