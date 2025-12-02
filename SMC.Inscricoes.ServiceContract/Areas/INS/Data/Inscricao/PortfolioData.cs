using SMC.DadosMestres.Common.Areas.GED.Enums;
using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.DadosMestres.Common.Areas.SHA.Enums;
using SMC.Framework.Mapper;
using System;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    public class PortfolioData : ISMCMappable
    {
        public Guid GuidBiblioteca { get; set; }
        public long SeqHierarquiaClassificacao { get; set; }
        public string NomePortfolio { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string Interessado { get; set; }
        public NivelAcesso NivelAcesso { get; set; }
        public PrevisaoDesclassificacao PrevisaoDesclassificacao { get; set; }
        public TipoPortfolio TipoPortfolio { get; set; }
        public string BancoDadosOrigem { get; set; }
        public string TabelaIntegracaoOrigem { get; set; }
        public string AtributoChaveTabelaIntegracaoOrigem { get; set; }
        public string ValorChaveTabelaIntegracaoOrigem { get; set; }
        public int? CodigoPessoa { get; set; }
        public string CPF { get; set; }
        public string NomePessoa { get; set; }
        public string NomeSocial { get; set; }
        public TipoNacionalidade TipoNacionalidade { get; set; }
        public string PaisNacionalidade { get; set; }
        public string Passaporte { get; set; }
        public string DataValidadePassaporte { get; set; }
        public string PaisEmissaoPassaporte { get; set; }
        public string DataNascimento { get; set; }
        public bool Falecido { get; set; }
        public Sexo Sexo { get; set; }
        public string UFNaturalidade { get; set; }
        public string CidadeNaturalidade { get; set; }
        public string NaturalidadeEstrangeira { get; set; }
        public string NumeroIdentidade { get; set; }
        public string OrgaoEmissorIdentidade { get; set; }
        public string UFIdentidade { get; set; }
        public string DataExpedicaoIdentidade { get; set; }
        public string NomeParente1 { get; set; }
        public TipoParentesco TipoParente1 { get; set; }
        public string NomeParente2 { get; set; }
        public TipoParentesco TipoParente2 { get; set; }
        public string UsuarioLogado { get; set; }
    }
}
