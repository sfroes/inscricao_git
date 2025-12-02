using SMC.Framework.Mapper;

namespace SMC.Inscricoes.Domain.Areas.Shared.SharePointWebApi.ValueObject
{
    public class BuscarIdPortifolioVO : ISMCMappable
    {
        public string GuidBiblioteca { get; set; }
        public string BancoDadosOrigem { get; set; }
        public string TabelaIntegracaoOrigem { get; set; }
        public string AtributoChaveTabelaIntegracaoOrigem { get; set; }
        public long ValorChaveTabelaIntegracaoOrigem { get; set; }
    }
}
