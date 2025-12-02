using SMC.Framework.Mapper;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    public class CabecalhoProcessoEtapaData : ISMCMappable
    {        
        public long SeqProcesso { get; set; }
        
        public long SeqEtapaProcesso { get; set; }

        public long SeqEtapaSGF {get;set;}

        public string DescricaoTipoProcesso { get; set; }

        public string DescricaoEtapa { get; set; }

        public string DescricaoProcesso { get; set; }


    }
}