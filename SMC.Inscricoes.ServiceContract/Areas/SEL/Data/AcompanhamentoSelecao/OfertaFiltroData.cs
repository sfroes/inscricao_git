using SMC.Framework.Mapper;
using SMC.Framework.Model;

namespace SMC.Inscricoes.ServiceContract.Areas.SEL.Data
{
    public class OfertaFiltroData : SMCPagerFilterData, ISMCMappable
    {
        public long? SeqInscricaoOferta { get; set; }

        public long? SeqOferta { get; set; }

        public long? SeqInscricao { get; set; }
    }
}