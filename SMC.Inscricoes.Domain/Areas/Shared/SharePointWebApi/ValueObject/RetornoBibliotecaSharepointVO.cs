using SMC.Framework.Mapper;

namespace SMC.Inscricoes.Domain.Areas.Shared.SharePointWebApi.ValueObject
{
    public class RetornoBibliotecaSharepointVO : RetornoBaseShrepointVO, ISMCMappable
    {
        public string GuidBiblioteca { get; set; }
    }
}
