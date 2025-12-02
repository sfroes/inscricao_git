using SMC.Framework.Mapper;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class CabecalhoCheckinLoteVO : ISMCMappable
    {
        public long TotalInscritos { get; set; }
        public string DescricaoProcesso { get; set; }
        public string DescricaoOferta { get; set; }
        public long TotalChekinsRealizados { get; set; }
        public long TotalRestantes { get; set; }
    }
}
