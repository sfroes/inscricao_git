using SMC.Framework.Mapper;

namespace SMC.Inscricoes.Domain.Areas.Shared.SharePointWebApi.ValueObject
{
    public class RetornoProcessoSharepointVO : RetornoBaseShrepointVO, ISMCMappable
    {
        public string IdGedProcesso { get; set; }
        public bool ExisteProcesso { get; set; }
    }
}
