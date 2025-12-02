using SMC.Framework.Mapper;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class CheckinLoteVO : ISMCMappable
    {
        public string DescricaoProcesso { get; set; }

        public string DescricaoOferta { get; set; }

        public long TotalInscritos { get; set; }
        public long TotalChekinsRealizados { get; set; }

        public long TotalRestantes { get; set; }

        public long SeqProcesso { get; set; }

        public long SeqOferta { get; set; }
    }
}
