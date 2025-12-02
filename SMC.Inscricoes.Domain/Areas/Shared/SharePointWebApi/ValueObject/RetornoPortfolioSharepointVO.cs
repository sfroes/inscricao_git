using SMC.Framework.Mapper;

namespace SMC.Inscricoes.Domain.Areas.Shared.SharePointWebApi.ValueObject
{
    public class RetornoPortfolioSharepointVO : RetornoBaseShrepointVO, ISMCMappable
    {
        public string IdGedPortfolio { get; set; }

        public string GuidBiblioteca { get; set; }

        public bool ExistePortfolio { get; set; }
    }
}
