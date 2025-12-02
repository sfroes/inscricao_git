using SMC.Framework.Mapper;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data.AcompanhamentoCheckin
{
    public class CheckinLoteFiltroData : ISMCMappable
    {

        public long? SeqInscricao { get; set; }

        public string NomeInscrito { get; set; }

        public bool? CheckinRealizado { get; set; }

        public long SeqOferta { get; set; }

        public long? SeqProcesso { get; set; }
    }
}
