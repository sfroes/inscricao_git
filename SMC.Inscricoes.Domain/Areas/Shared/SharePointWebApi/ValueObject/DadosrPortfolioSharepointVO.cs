using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.Shared.SharePointWebApi.ValueObject
{
    public class DadosrPortfolioSharepointVO
    {
        public string GuidBiblioteca { get; set; }
        public string SeqHierarquiaClassificacao { get; set; }
        public string NomePortfolio { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string Interessado { get; set; }
        public string NivelAcesso { get; set; }
        public string PrevisaoDesclassificacao { get; set; }
        public string TipoPortfolio { get; set; }
        public string BancoDadosOrigem { get; set; }
        public string TabelaIntegracaoOrigem { get; set; }
        public string AtributoChaveTabelaIntegracaoOrigem { get; set; }
        public string ValorChaveTabelaIntegracaoOrigem { get; set; }
        public string CodigoPessoa { get; set; }
        public string CPF { get; set; }
        public string NomePessoa { get; set; }
        public string NomeSocial { get; set; }
        public string TipoNacionalidade { get; set; }
        public string PaisNacionalidade { get; set; }
        public string Passaporte { get; set; }
        public string DataValidadePassaporte { get; set; }
        public string PaisEmissaoPassaporte { get; set; }
        public string DataNascimento { get; set; }
        public string Falecido { get; set; }
        public string Sexo { get; set; }
        public string UFNaturalidade { get; set; }
        public string CidadeNaturalidade { get; set; }
        public string NaturalidadeEstrangeira { get; set; }
        public string NumeroIdentidade { get; set; }
        public string OrgaoEmissorIdentidade { get; set; }
        public string UFIdentidade { get; set; }
        public string DataExpedicaoIdentidade { get; set; }
        public string NomeParente1 { get; set; }
        public string TipoParente1 { get; set; }
        public string NomeParente2 { get; set; }
        public string TipoParente2 { get; set; }
        public string UsuarioLogado { get; set; }
    }
}
