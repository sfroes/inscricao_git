using SMC.Framework.Mapper;
using SMC.Inscricoes.Domain.Areas.INS.Models;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class InscritoAtividadeRelatorioVO : ISMCMappable
    {
        public long SeqOferta { get; set; }
        public string DescricaoOferta { get; set; }
        public long SeqProcesso { get; set; }
        public string DescricaoProcesso { get; set; }
        public string NumeroInscricao { get; set; }
        public string NomeInscrito { get; set; }
        public Oferta Oferta { get; set; }

        public bool? ExibirPeriodoOferta { get; set; }
    }
}
