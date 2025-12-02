using SMC.Framework.Mapper;

namespace SMC.Inscricoes.Domain.Areas.Shared.SharePointWebApi.ValueObject
{
    public class RetornoArquivoSharepointVO : RetornoBaseShrepointVO, ISMCMappable
    {
        public string IdGEDArquivo { get; set; }
        public string URLPublicaArquivo { get; set; }
        public string URLDownloadArquivo { get; set; }
        public string URLPrivadaArquivo { get; set; }
    }
}
