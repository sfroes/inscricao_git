using SMC.DadosMestres.Common;
using SMC.DadosMestres.Common.Areas.GED.Enums;
using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.DadosMestres.Common.Areas.SHA.Enums;
using SMC.DadosMestres.ServiceContract.Areas.PES.Data;
using SMC.Framework;
using SMC.Framework.Domain;
using SMC.Framework.Security;
using SMC.Inscricoes.Common.Exceptions;
using SMC.Inscricoes.Domain.Areas.INS.DomainServices;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.Shared.SharePointWebApi.Const;
using SMC.Inscricoes.Domain.Areas.Shared.SharePointWebApi.Models;
using SMC.Inscricoes.Domain.Areas.Shared.SharePointWebApi.ValueObject;
using System;
using System.Net;

namespace SMC.Inscricoes.Domain.Areas.Shared.SharePointWebApi.Portfolio
{
    public class PortfolioApiDoaminService : InscricaoContextDomain<SharepointApi>
    {
        #region DOMAINSERVICE
        InscritoDomainService InscritoDomainService => Create<InscritoDomainService>();
        ProcessoDomainService ProcessoDomainService => Create<ProcessoDomainService>();
        BibliotecaApiDoaminService BibliotecaApiDoaminService => Create<BibliotecaApiDoaminService>();
        #endregion

        /// <summary>
        /// Buscar identificador de um inscrito caso exista
        /// </summary>
        /// <param name="seqInscrito">Sequencial do inscrito</param>
        /// <param name="guidBiblioteca">Guid da Biblioteca</param>
        /// <returns>Dados do portfolio</returns>
        public RetornoPortfolioSharepointVO BuscarIdPortifolio(long seqInscrito, string guidBiblioteca)
        {

            BuscarIdPortifolioVO dadosOrigem = new BuscarIdPortifolioVO()
            {
                BancoDadosOrigem = TOKEN_DADOSMESTRES.BANCO_INSCRICAO,
                TabelaIntegracaoOrigem = TOKEN_DADOSMESTRES.BANCO_INSCRICAO_INSCRITO,
                AtributoChaveTabelaIntegracaoOrigem = TOKEN_DADOSMESTRES.BANCO_INSCRICAO_INSCRITO_SEQ,
                ValorChaveTabelaIntegracaoOrigem = seqInscrito,
                GuidBiblioteca = guidBiblioteca
            };

            var retorno = RequisicoesSharePoint.EnviarPostApiGed<RetornoPortfolioSharepointVO>(dadosOrigem, ACOES_WEBAPI_SHAREPOINT.BUSCAR_ID_PORTFOLIO);

            if (retorno.StatusCode == HttpStatusCode.OK)
            {
                retorno.ExistePortfolio = true;
            }
            else if (retorno.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new Exception(retorno.ErroMessage);
            }

            return retorno;
        }

        /// <summary>
        /// Cria o portfolio caso atenda aos parametros necessarios
        /// </summary>
        /// <param name="modelo">Modelo para a criação do portfolio</param>
        /// <returns>Dados do portfolio</returns>
        public RetornoPortfolioSharepointVO CriarPortfolio(CriarPortfolioVO modelo)
        {
            RetornoPortfolioSharepointVO retorno = new RetornoPortfolioSharepointVO();

            var processo = this.ProcessoDomainService.SearchProjectionByKey(modelo.SeqProcesso,
                p => new
                {
                    p.TipoProcesso.SeqHierarquiaClassificacaoGed,
                    p.TipoProcesso.SeqContextoBibliotecaGed,
                    p.TipoProcesso.HabilitaGed
                });

            if (!processo.HabilitaGed)
            {
                return retorno;
            }

            if (processo.SeqContextoBibliotecaGed != null && processo.SeqContextoBibliotecaGed > 0)
            {
                var retornoApiBiblioteca = BibliotecaApiDoaminService.BuscarGuidBiblioteca(processo.SeqContextoBibliotecaGed.Value);

                if (retornoApiBiblioteca.StatusCode == HttpStatusCode.OK)
                {
                    var retornoApiPortfolio = BuscarIdPortifolio(modelo.SeqInscrito, retornoApiBiblioteca.GuidBiblioteca);

                    if (!retornoApiPortfolio.ExistePortfolio)
                    {
                        Inscrito inscrito = InscritoDomainService.BuscarInscrito(modelo.SeqInscrito);

                        DadosrPortfolioSharepointVO dadosPortfolioSharepointVO = new DadosrPortfolioSharepointVO();

                        dadosPortfolioSharepointVO.GuidBiblioteca = retornoApiBiblioteca.GuidBiblioteca;
                        dadosPortfolioSharepointVO.SeqHierarquiaClassificacao = processo.SeqHierarquiaClassificacaoGed.ToString();
                        dadosPortfolioSharepointVO.NomePortfolio = MontarNomePortfolio(inscrito);
                        dadosPortfolioSharepointVO.Titulo = MontarTituloPortfolio(inscrito);
                        dadosPortfolioSharepointVO.Autor = MontarAutorPortfolio(inscrito);
                        dadosPortfolioSharepointVO.Interessado = MontarInteressadoPortfolio(inscrito);
                        dadosPortfolioSharepointVO.NivelAcesso = NivelAcesso.Reservado.ToString();
                        dadosPortfolioSharepointVO.PrevisaoDesclassificacao = PrevisaoDesclassificacao.Permanente.ToString();
                        dadosPortfolioSharepointVO.TipoPortfolio = TipoPortfolio.Pessoa.ToString();
                        dadosPortfolioSharepointVO.BancoDadosOrigem = TOKEN_DADOSMESTRES.BANCO_INSCRICAO;
                        dadosPortfolioSharepointVO.TabelaIntegracaoOrigem = TOKEN_DADOSMESTRES.BANCO_INSCRICAO_INSCRITO;
                        dadosPortfolioSharepointVO.AtributoChaveTabelaIntegracaoOrigem = TOKEN_DADOSMESTRES.BANCO_INSCRICAO_INSCRITO_SEQ;
                        dadosPortfolioSharepointVO.ValorChaveTabelaIntegracaoOrigem = inscrito.Seq.ToString();
                        dadosPortfolioSharepointVO.CodigoPessoa = "";
                        dadosPortfolioSharepointVO.CPF = inscrito.Cpf;
                        dadosPortfolioSharepointVO.NomePessoa = inscrito.Nome;
                        dadosPortfolioSharepointVO.NomeSocial = inscrito.NomeSocial;
                        dadosPortfolioSharepointVO.TipoNacionalidade = inscrito.Nacionalidade.ToString();
                        dadosPortfolioSharepointVO.PaisNacionalidade = inscrito.CodigoPaisNacionalidade.ToString();
                        dadosPortfolioSharepointVO.Passaporte = inscrito.NumeroPassaporte;
                        dadosPortfolioSharepointVO.DataValidadePassaporte = inscrito.DataValidadePassaporte?.ToString("dd/MM/yyyy");
                        dadosPortfolioSharepointVO.PaisEmissaoPassaporte = inscrito.CodigoPaisNacionalidade.ToString();
                        dadosPortfolioSharepointVO.DataNascimento = inscrito.DataNascimento.ToString("dd/MM/yyy");
                        dadosPortfolioSharepointVO.Falecido = "";
                        dadosPortfolioSharepointVO.Sexo = inscrito.Sexo.ToString();
                        dadosPortfolioSharepointVO.UFNaturalidade = inscrito.UfNaturalidade;
                        dadosPortfolioSharepointVO.CidadeNaturalidade = inscrito.CodigoCidadeNaturalidade.ToString();
                        dadosPortfolioSharepointVO.NaturalidadeEstrangeira = inscrito.DescricaoNaturalidadeEstrangeira;
                        dadosPortfolioSharepointVO.NumeroIdentidade = inscrito.NumeroIdentidade;
                        dadosPortfolioSharepointVO.OrgaoEmissorIdentidade = inscrito.OrgaoEmissorIdentidade;
                        dadosPortfolioSharepointVO.UFIdentidade = inscrito.UfIdentidade;
                        dadosPortfolioSharepointVO.DataExpedicaoIdentidade = "";

                        if (!string.IsNullOrEmpty(dadosPortfolioSharepointVO.NomeParente1))
                        {
                            dadosPortfolioSharepointVO.NomeParente1 = inscrito.NomeMae;
                            dadosPortfolioSharepointVO.TipoParente1 = TipoParentesco.Mae.ToString();
                        }

                        if (!string.IsNullOrEmpty(dadosPortfolioSharepointVO.NomeParente2))
                        {
                            dadosPortfolioSharepointVO.NomeParente2 = inscrito.NomePai;
                            dadosPortfolioSharepointVO.TipoParente2 = TipoParentesco.Pai.ToString();
                        }
                        dadosPortfolioSharepointVO.UsuarioLogado = $"{SMCContext.User.SMCGetSequencialUsuario()}/{SMCContext.User.SMCGetNome()}";

                        retorno = RequisicoesSharePoint.EnviarPostApiGed<RetornoPortfolioSharepointVO>(dadosPortfolioSharepointVO, ACOES_WEBAPI_SHAREPOINT.SALVAR_PORTFOLIO);

                        if (retorno.StatusCode == HttpStatusCode.OK)
                        {
                            retorno.GuidBiblioteca = retornoApiBiblioteca.GuidBiblioteca;
                            retorno.ExistePortfolio = true;
                        }

                        if (retorno.StatusCode == HttpStatusCode.BadRequest)
                        {
                            throw new Exception(retorno.ErroMessage);
                        }
                    }
                    else
                    {
                        retorno = retornoApiPortfolio;
                        retorno.GuidBiblioteca = retornoApiBiblioteca.GuidBiblioteca;
                    }
                }

            }

            return retorno;
        }

        /// <summary>
        ///  {tipo de código} + ' ' + {código}. Se o inscrito possuir o CPF cadastrado, informar
        ///  o CPF, se não possuir, informar o passaporte.A descrição do tipo de código deverá 
        ///  ser em caixa alta. Exemplos: CPF 73310549615, PASSAPORTE M5460487
        /// </summary>
        /// <param name="inscrito"></param>
        /// <returns></returns>
        private string MontarNomePortfolio(Inscrito inscrito)
        {
            string retorno = string.Empty;

            if (!string.IsNullOrEmpty(inscrito.Cpf))
            {
                retorno = $"{nameof(inscrito.Cpf).ToUpper()} {inscrito.Cpf}";
            }
            else if (!string.IsNullOrEmpty(inscrito.NumeroPassaporte))
            {
                retorno = $"PASSAPORTE {inscrito.NumeroPassaporte}";
            }
            else
            {
                throw new DadosInconsistenteApiGEDException();
            }

            return retorno;
        }

        /// <summary>
        ///  “Nome: “ + {nome do inscrito} + “ – “ + {tipo de código} + “: “ + {código}. Se o inscrito possuir
        /// o CPF cadastrado, informar o CPF no código, se não possuir, informar o passaporte.Exemplos:
        /// Nome: João da Silva - CPF: 733.105.496-15
        /// Nome: Fulano de Tal - Passaporte: M546048
        /// </summary>
        /// <param name="inscrito"></param>
        /// <returns></returns>
        private string MontarTituloPortfolio(Inscrito inscrito)
        {
            string retorno = string.Empty;

            retorno = $"Nome: {inscrito.Nome} - {MontarNomePortfolio(inscrito)}";

            return retorno;
        }

        /// <summary>
        ///  “Nome: “ + {nome do inscrito} + “ – “ + {tipo de código} + “: “ + {código}. Se o inscrito possuir
        /// o CPF cadastrado, informar o CPF no código, se não possuir, informar o passaporte.Exemplos:
        /// Nome: João da Silva - CPF: 733.105.496-15
        /// Nome: Fulano de Tal - Passaporte: M546048
        /// </summary>
        /// <param name="inscrito"></param>
        /// <returns></returns>
        private string MontarAutorPortfolio(Inscrito inscrito)
        {
            string retorno = string.Empty;

            retorno = $"Nome: {inscrito.Nome} - {MontarNomePortfolio(inscrito)}";

            return retorno;
        }

        /// <summary>
        ///  “Nome: “ + {nome do inscrito} + “ – “ + {tipo de código} + “: “ + {código}. Se o inscrito possuir
        /// o CPF cadastrado, informar o CPF no código, se não possuir, informar o passaporte.Exemplos:
        /// Nome: João da Silva - CPF: 733.105.496-15
        /// Nome: Fulano de Tal - Passaporte: M546048
        /// </summary>
        /// <param name="inscrito"></param>
        /// <returns></returns>
        private string MontarInteressadoPortfolio(Inscrito inscrito)
        {
            string retorno = string.Empty;

            retorno = $"Nome: {inscrito.Nome} - {MontarNomePortfolio(inscrito)}";

            return retorno;
        }
    }
}
