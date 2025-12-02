using SMC.Framework.Mapper;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data.InscricaoOferta
{
    public class CabecalhoCheckinLoteData : ISMCMappable
    {
        public long TotalInscritos { get; set; }
        public string DescricaoProcesso { get; set; }

        public string DescricaoOferta { get; set; }
        public long TotalChekinsRealizados { get; set; }
        public long TotalRestantes { get; set; }
    }
}
