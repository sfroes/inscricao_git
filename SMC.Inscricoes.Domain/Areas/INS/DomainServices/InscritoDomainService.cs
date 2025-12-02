using SMC.DadosMestres.Common;
using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.DadosMestres.ServiceContract.Areas.PES.Data;
using SMC.DadosMestres.ServiceContract.Areas.PES.Interfaces;
using SMC.Framework;
using SMC.Framework.Domain;
using SMC.Framework.Exceptions;
using SMC.Framework.Extensions;
using SMC.Framework.Mapper;
using SMC.Framework.Security;
using SMC.Framework.Specification;
using SMC.Framework.UnitOfWork;
using SMC.Framework.Util;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using SMC.Inscricoes.Common.Areas.INS.Exceptions;
using SMC.Inscricoes.Common.Exceptions;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using SMC.Inscricoes.Domain.Areas.INS.ValueObjects;
using SMC.Inscricoes.Domain.Areas.Shared.SharePointWebApi.Portfolio;
using SMC.Inscricoes.Domain.Areas.Shared.SharePointWebApi.ValueObject;
using SMC.Inscricoes.Domain.Models;
using SMC.Localidades.Common.Areas.LOC.Enums;
using SMC.Seguranca.ServiceContract.Areas.APL.Interfaces;
using SMC.Seguranca.ServiceContract.Areas.USU.Data;
using SMC.Seguranca.ServiceContract.Areas.USU.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace SMC.Inscricoes.Domain.Areas.INS.DomainServices
{
    public class InscritoDomainService : InscricaoContextDomain<Inscrito>
    {
        #region Services

        private IUsuarioService UsuarioService { get => Create<IUsuarioService>(); }

        private IAplicacaoService AplicacaoService => Create<IAplicacaoService>();
        private IIntegracaoDadoMestreService IntegracaoDadoMestreService { get => Create<IIntegracaoDadoMestreService>(); }

        #endregion Services

        #region DomainService

        private PortfolioApiDoaminService PortfolioApiDoaminService => Create<PortfolioApiDoaminService>();
        private ProcessoDomainService ProcessoDomainService => Create<ProcessoDomainService>();
        private BibliotecaApiDoaminService BibliotecaApiDoaminService => Create<BibliotecaApiDoaminService>();
        private ProcessoCampoInscritoDomainService ProcessoCampoInscritoDomainService => Create<ProcessoCampoInscritoDomainService>();

        #endregion

        /// <summary>
        /// Salva os dados de um inscrito (ESSE MÉTODO É ACIONADO DO GPI.INSCRICAO)
        /// </summary>
        /// <param name="inscrito">Inscrito a ser salvo</param>
        /// <returns>Sequencial do inscrito salvo</returns>
        public long SalvarInscrito(InscritoVO inscritoVO)
        {
            Inscrito inscrito = inscritoVO.Transform<Inscrito>();
            bool isNew = inscrito.Seq == 0;

            // Caso tenha sido marcado a flag na tela
            if (inscrito.ConsentimentoLGPD.HasValue)
            {
                if (isNew)
                    inscrito.DataConsentimentoLGPD = DateTime.Now;
                else
                {
                    // Verifica se alterou o valor da flag
                    var dadosConsentimentoAnterior = this.SearchProjectionByKey(inscrito.Seq, x => new
                    {
                        x.DataConsentimentoLGPD,
                        x.ConsentimentoLGPD
                    });

                    if (dadosConsentimentoAnterior.ConsentimentoLGPD != inscrito.ConsentimentoLGPD)
                        inscrito.DataConsentimentoLGPD = DateTime.Now;
                    else
                        inscrito.DataConsentimentoLGPD = dadosConsentimentoAnterior.DataConsentimentoLGPD ?? DateTime.Now;
                }
            }

            using (var unitOfWork = SMCUnitOfWork.Begin())
            {
                try
                {
                    // Chama método para incluir/alterar o inscrito
                    var seqInscrito = this.AlterarInscrito(inscrito, false, inscritoVO.UidProcesso);

                    // Recupera o inscrito que foi criado do banco
                    // Necessário para atualizar o objeto Inscrito
                    //                    inscrito = this.SearchByKey(seqInscrito);

                    // Se foi criado um novo usuário, realiza primeira integração com GDM
                    if (isNew)
                    {
                        CriarPessoaDadosMestres(inscrito);
                    }

                    // Realiza o commit
                    unitOfWork.Commit();
                }
                catch (Exception e)
                {
                    unitOfWork.Rollback();
                    throw e;
                }
            }

            // Se por ventura contiver o UidProcesso validar no GED o portifolio caso não exista
            // Cria o portifolio, se por ventura não contiver o seq do processo no ato ada inscrição não faça nada
            inscritoVO.Seq = inscrito.Seq;
            CriarPortifolioInscritoProcesso(inscritoVO);

            return inscrito.Seq;
        }

        /// <summary>
        /// Atualiza os dados de um inscrito 
        /// </summary>
        /// <param name="inscrito">Inscrito a ser salvo</param>
        /// <returns>Sequencial do inscrito salvo</returns>
        public long AlterarInscrito(Inscrito inscrito, bool sincronizarGDM = false, Guid? uidProceso = null)
        {
            // Retira mascara do CPF e do telefone
            inscrito.Cpf = inscrito.Cpf.SMCRemoveNonDigits();
            if (inscrito.Enderecos.SMCAny())
            {
                foreach (var endereco in inscrito.Enderecos)
                    endereco.Cep = endereco.Cep.SMCRemoveNonDigits();
            }

            if (inscrito.Telefones.SMCAny())
            {
                foreach (var telefone in inscrito.Telefones.Where(t => !string.IsNullOrEmpty(t.Numero)))
                    telefone.Numero = telefone.Numero.SMCRemoveNonDigits().TrimEnd();
            }

            var nameUsuario = SMCContext.User.Identity.Name;
            if (nameUsuario.Length == 0)
                throw new UsuarioInvalidoException();

            // Formata o nome conforme as regras:
            // - Retirar os espaços em branco antes e depois do nome.
            // - Colocar a primeira letra de cada palavra em maiúsculo e as restantes em minúsculo.
            // - Substituir acento agudo sozinho, por apóstrofo.
            // - Colocar a primeira letra após "-" ou apóstrofo em maiúsculo.
            // - Manter palavras como "I", "II" e "III" em maiúsculo.
            // - Manter palavras como "de", "da", "do", "das", "dos", "em" e "e" em minúsculo.
            inscrito.Nome = inscrito.Nome.SMCToPascalCaseName();
            inscrito.NomeSocial = inscrito.NomeSocial.SMCToPascalCaseName();
            inscrito.NomeMae = inscrito.NomeMae.SMCToPascalCaseName();
            inscrito.NomePai = inscrito.NomePai.SMCToPascalCaseName();
            inscrito.Email = inscrito.Email.Trim();
            // Verifica se a data de nascimento é válida
            if (inscrito.DataNascimento.Year <= 1)
            {
                throw new DataNascimentoInvalidaException();
            }

            if (ValidatorInscritoStep1(inscrito, uidProceso) &&
                ValidatorInscritoStep2(inscrito, uidProceso))
            {
                ValidaPaisOrigem(inscrito, uidProceso);

                using (var unitOfWork = SMCUnitOfWork.Begin())
                {
                    try
                    {
                        // Atualiza o usuário do SAS
                        if (inscrito.SeqUsuarioSas > 0)
                        {
                            var usuSas = this.UsuarioService.BuscarUsuario((long)(inscrito.SeqUsuarioSas));
                            if (usuSas == null)
                                throw new UsuarioSASNaoEncontradoException();

                            usuSas.Seq = (long)inscrito.SeqUsuarioSas;
                            usuSas.Nome = inscrito.Nome.SMCToPascalCaseName();
                            usuSas.NomeSocial = inscrito.NomeSocial.SMCToPascalCaseName();
                            usuSas.NomeMae = inscrito.NomeMae.SMCToPascalCaseName();
                            usuSas.Cpf = inscrito.Cpf;
                            usuSas.NumeroPassaporte = inscrito.NumeroPassaporte;
                            usuSas.DataNascimento = inscrito.DataNascimento;

                            // Caso o e-mail do inscrito não esteja no SAS, inclui
                            if (!usuSas.Emails.Any(a => a.Email.Trim() == inscrito.Email))
                            {
                                usuSas.Emails.Add(new UsuarioEmailData()
                                {
                                    Email = inscrito.Email,
                                    Ativo = true
                                });
                            }

                            // Caso o e-mail do inscrito esteja desativado no SAS, ativa
                            if (usuSas.Emails.Any(a => a.Email.Trim() == inscrito.Email && !a.Ativo.GetValueOrDefault()))
                            {
                                usuSas.Emails.First(a => a.Email.Trim() == inscrito.Email).Ativo = true;
                            }

                            this.UsuarioService.SalvarUsuario(usuSas);

                        }

                        // Realiza o sincronismo CAD/GDM
                        if (sincronizarGDM)
                        {
                            var enderecos = inscrito.Enderecos.TransformList<SMC.DadosMestres.ServiceContract.Areas.PES.Data.EnderecoData>();
                            var enderecosEletronicos = inscrito.EnderecosEletronicos.TransformList<SMC.DadosMestres.ServiceContract.Areas.PES.Data.EnderecoEletronicoData>();
                            var telefones = inscrito.Telefones.TransformList<SMC.DadosMestres.ServiceContract.Areas.PES.Data.TelefoneData>();

                            // Adiciona o email de contato ao CAD
                            enderecosEletronicos.Add(new EnderecoEletronicoData()
                            {
                                Descricao = inscrito.Email,
                                TipoEnderecoEletronico = TipoEnderecoEletronico.Email,
                            });

                            SincronizaPessoaFisicaData sincronizaPessoaFisicaData = new SincronizaPessoaFisicaData()
                            {
                                NomeBanco = TOKEN_DADOSMESTRES.BANCO_INSCRICAO,
                                NomeTabela = TOKEN_DADOSMESTRES.BANCO_INSCRICAO_INSCRITO,
                                NomeAtributoChave = TOKEN_DADOSMESTRES.BANCO_INSCRICAO_INSCRITO_SEQ,
                                SeqAtributoChaveIntegracao = inscrito.Seq,
                                CodigoPessoaCAD = null,
                                AtualizaCAD = true,
                                Cpf = inscrito.Cpf,
                                Nome = inscrito.Nome,
                                NomeSocial = inscrito.NomeSocial,
                                TipoNacionalidade = inscrito.Nacionalidade,
                                CodigoPaisNacionalidade = inscrito.CodigoPaisNacionalidade ?? 0,
                                NumeroPassaporte = inscrito.NumeroPassaporte,
                                DataValidadePassaporte = inscrito.DataValidadePassaporte,
                                CodigoPaisEmissaoPassaporte = inscrito.CodigoPaisEmissaoPassaporte,
                                DataNascimento = inscrito.DataNascimento,
                                Falecido = null,
                                Sexo = inscrito.Sexo == null ? Sexo.Nenhum : inscrito.Sexo.Value,
                                UfNaturalidade = inscrito.UfNaturalidade,
                                CodigoCidadeNaturalidade = inscrito.CodigoCidadeNaturalidade,
                                DescricaoNaturalidadeEstrangeira = inscrito.DescricaoNaturalidadeEstrangeira,
                                NumeroIdentidade = inscrito.NumeroIdentidade,
                                OrgaoEmissorIdentidade = inscrito.OrgaoEmissorIdentidade,
                                UfIdentidade = inscrito.UfIdentidade,
                                DataExpedicaoIdentidade = null,
                                ArquivoFoto = null,
                                Enderecos = enderecos,
                                Telefones = telefones,
                                EnderecosEletronicos = enderecosEletronicos,
                                UsuarioOperacao = nameUsuario
                            };

                            sincronizaPessoaFisicaData.Filiacao = new List<InserePessoaFisicaFiliacaoData>();
                            if (!string.IsNullOrEmpty(inscrito.NomeMae))
                            {
                                sincronizaPessoaFisicaData.Filiacao.Add(new InserePessoaFisicaFiliacaoData()
                                {
                                    NomePessoaParentesco = inscrito.NomeMae,
                                    TipoParentesco = DadosMestres.Common.Areas.PES.Enums.TipoParentesco.Mae
                                });
                            }
                            if (!string.IsNullOrEmpty(inscrito.NomePai))
                            {
                                sincronizaPessoaFisicaData.Filiacao.Add(new InserePessoaFisicaFiliacaoData()
                                {
                                    NomePessoaParentesco = inscrito.NomePai,
                                    TipoParentesco = DadosMestres.Common.Areas.PES.Enums.TipoParentesco.Pai
                                });
                            }
                            var erroSinc = IntegracaoDadoMestreService.SincronizaPessoaFisica(sincronizaPessoaFisicaData);
                            if (!string.IsNullOrEmpty(erroSinc))
                            {
                                throw new SMCApplicationException(erroSinc);
                            }
                        }

                        //Caso o Estado civil esteja com o valor zero salva ele como null
                        if (inscrito.EstadoCivil == EstadoCivil.Nenhum)
                        {
                            inscrito.EstadoCivil = null;
                        }

                        //Caso o sexo esteja com o valor zero salva ele como null
                        if (inscrito.Sexo == Sexo.Nenhum)
                        {
                            inscrito.Sexo = null;
                        }

                        //Caso o Pais de origem esteja com o valor zero salva ele como null
                        if (inscrito.CodigoPaisNacionalidade == 0)
                        {
                            inscrito.CodigoPaisNacionalidade = null;
                        }

                        this.SaveEntity(inscrito);

                        unitOfWork.Commit();
                    }
                    catch (Exception e)
                    {
                        unitOfWork.Rollback();
                        throw new SMCApplicationException(e.Message);
                    }
                }
            }

            return inscrito.Seq;
        }

        //Foi necessario criar esse metodo pois nesse ponto os sistemas possuiam regras diferentes. 
        /// <summary>
        /// Valida o Pais de Origem de acordo com o sistema solicitante
        /// </summary>
        /// <param name="inscrito"></param>
        /// <param name="uidProcesso"></param>
        private void ValidaPaisOrigem(Inscrito inscrito, Guid? uidProcesso)
        {
            var seqAplicacaoSAS = AplicacaoService.BuscarAplicacaoPelaSigla(SMCContext.ApplicationId).Sigla;
            ProcessoCampoInscritoVisiveisVO camposVisiveis = ConfigurarVisibilidadeCamposPorProcesso(uidProcesso);

            if (seqAplicacaoSAS != SIGLA_APLICACAO.GPI_ADMIN)
            {
                if (inscrito.Nacionalidade != TipoNacionalidade.Brasileira && !camposVisiveis.ExibirPaisOrigem)
                {
                    inscrito.CodigoPaisNacionalidade = null;
                }

            }
            else
            {
                // Verifica se o a nacionalidade é estrangeira e o pais não é o brasil
                if (inscrito.Nacionalidade != TipoNacionalidade.Brasileira && inscrito.CodigoPaisNacionalidade == CONSTANTS.CODIGO_PAIS_BRASIL)
                {
                    throw new PaisInvalidoParaNacionalidadeEstrangeiraException();
                }
            }

        }
        private void AtualizarDadosMestres(Inscrito inscrito)
        {
            IntegracaoDadoMestreData dadosIntegracao = new IntegracaoDadoMestreData()
            {
                NomeBanco = TOKEN_DADOSMESTRES.BANCO_INSCRICAO,
                NomeTabela = TOKEN_DADOSMESTRES.BANCO_INSCRICAO_INSCRITO,
                NomeAtributoChave = TOKEN_DADOSMESTRES.BANCO_INSCRICAO_INSCRITO_SEQ,
                SeqAtributoChaveIntegracao = inscrito.Seq
            };

            /*
                Executar a regra RN_GDM_001 - Salvar pessoa dados mestres, passando como parâmetro o tipo de pessoa "Física", o banco “INSCRICAO”, tabela “Inscrito” (não possui schema), chave de origem "seq_inscrito",
                o conteúdo da chave de origem “ID do inscrito” e todos os outros dados da pessoa informados.

                Executar a regra RN_GDM_002 - Integração do endereço, passando por parâmetro:
                    - nom_banco_dados_origem: "INSCRICAO".
                    - nom_tabela_origem: "inscrito".
                    - nom_atributo_chave_origem: "seq_inscrito".
                    - seq_atributo_chave_origem: valor do campo seq_inscrito.
                    - enderecos: json com a lista de endereços do inscrito.
                    - usu_inclusao: usuário logado.
            */
            IntegracaoDadoMestreService.IntegracaoEndereco(inscrito.Transform<IntegracaoEnderecoData>(dadosIntegracao));

            /*
            Executar a regra RN_GDM_003 - Integração do telefone, passado por parâmetro:
                    - nom_banco_dados_origem: "INSCRICAO".
                    - nom_tabela_origem: "inscrito".
                    - nom_atributo_chave_origem: "seq_inscrito".
                    - seq_atributo_chave_origem: valor do campo seq_inscrito.
                    - telefones: json com a lista de telefones do inscrito.
                    - usu_inclusao: usuário logado.
            */
            IntegracaoDadoMestreService.IntegracaoTelefone(inscrito.Transform<IntegracaoTelefoneData>(dadosIntegracao));

            /*

                Executar a regra RN_GDM_004 - Integração do endereço eletrônico, passando por parâmetro:
                    - nom_banco_dados_origem: "INSCRICAO".
                    - nom_tabela_origem: "inscrito".
                    - nom_atributo_chave_origem: "seq_inscrito".
                    - seq_atributo_chave_origem: valor do campo seq_inscrito.
                    - enderecos_eletronicos: json com a lista de endereços eletrônicos do inscrito. - usu_inclusao: usuário logado
             */
            var dadosEnderecosEletronicos = inscrito.Transform<IntegracaoEnderecoEletronicoData>(dadosIntegracao);
            if (!string.IsNullOrEmpty(inscrito.Email))
            {
                dadosEnderecosEletronicos.EnderecosEletronicos.Add(new EnderecoEletronicoData
                {
                    Descricao = inscrito.Email,
                    TipoEnderecoEletronico = TipoEnderecoEletronico.Email,
                    Seq = 0
                });
            }
            IntegracaoDadoMestreService.IntegracaoEnderecoEletronico(dadosEnderecosEletronicos);
        }

        private void CriarPessoaDadosMestres(Inscrito inscrito)
        {
            IntegracaoDadoMestreData dadosOrigem = new IntegracaoDadoMestreData()
            {
                NomeBanco = TOKEN_DADOSMESTRES.BANCO_INSCRICAO,
                NomeTabela = TOKEN_DADOSMESTRES.BANCO_INSCRICAO_INSCRITO,
                NomeAtributoChave = TOKEN_DADOSMESTRES.BANCO_INSCRICAO_INSCRITO_SEQ,
                SeqAtributoChaveIntegracao = inscrito.Seq
            };

            var dataDadosMestres = SMCMapperHelper.Create<InserePessoaFisicaData>(inscrito, dadosOrigem);
            dataDadosMestres.TipoNacionalidade = inscrito.Nacionalidade;

            dataDadosMestres.Filiacao = new List<InserePessoaFisicaFiliacaoData>();
            if (!string.IsNullOrWhiteSpace(inscrito.NomeMae))
            {
                dataDadosMestres.Filiacao.Add(new InserePessoaFisicaFiliacaoData()
                {
                    TipoParentesco = TipoParentesco.Mae,
                    NomePessoaParentesco = inscrito.NomeMae
                });
            }
            if (!string.IsNullOrWhiteSpace(inscrito.NomePai))
            {
                dataDadosMestres.Filiacao.Add(new InserePessoaFisicaFiliacaoData()
                {
                    TipoParentesco = TipoParentesco.Pai,
                    NomePessoaParentesco = inscrito.NomePai
                });
            }

            var msgErro = IntegracaoDadoMestreService.InserePessoaFisica(dataDadosMestres);

            if (!string.IsNullOrWhiteSpace(msgErro))
            {
                throw new SMCApplicationException(msgErro);
            }
        }

        private void CriarPortifolioInscritoProcesso(InscritoVO inscritoVO)
        {
            if (inscritoVO.UidProcesso != null && inscritoVO.UidProcesso != Guid.Empty)
            {
                var spec = new ProcessoFilterSpecification() { UidProcesso = inscritoVO.UidProcesso };
                var processo = ProcessoDomainService.SearchProjectionBySpecification(spec,
                    p => new
                    {
                        p.Seq,
                    }
                    ).FirstOrDefault();

                CriarPortfolioVO criarPortfolioVO = new CriarPortfolioVO();
                criarPortfolioVO.SeqProcesso = processo.Seq;
                criarPortfolioVO.SeqInscrito = inscritoVO.Seq;

                PortfolioApiDoaminService.CriarPortfolio(criarPortfolioVO);
            }
        }

        /// <summary>
        /// Busca o sequencial do inscrito do usuário logado
        /// </summary>
        /// <param name="seqUsuarioSas">Sequencial do usuário SAS</param>
        /// <returns>Sequencial do inscrito do usuário do SAS logado, ou NULL caso não encontre.</returns>
        public long? BuscarSeqInscrito(long seqUsuarioSas)
        {
            InscritoFilterSpecification spec = new InscritoFilterSpecification() { SeqUsuarioSas = seqUsuarioSas };
            long seq = this.SearchProjectionByKey(spec, i => i.Seq);
            return seq == 0 ? null : new Nullable<long>(seq);
        }

        public bool ValidaInscritoPrimeiroPasso(Inscrito inscrito, Guid? uidProcesso)
        {
            return ValidatorInscritoStep1(inscrito, uidProcesso);
        }

        public bool VerificaPermissaoInscricaoForaPrazo(long seqProcesso)
        {
            // Busca os dados do usuario logado
            var seqUsuario = SMCContext.User.SMCGetSequencialUsuario();
            if (!seqUsuario.HasValue)
                throw new UsuarioInvalidoException();

            var seqInscrito = BuscarSeqInscrito(seqUsuario.Value);

            // Verifica se o usuario possui permissão de inscrição fora do prazo para este processo
            var usuario = this.SearchProjectionByKey(new SMCSeqSpecification<Inscrito>(seqInscrito.Value),
                        f => new
                        {
                            PermissaoInscricaoForaPrazo = f.PermissoesInscricaoForaPrazo
                                                            .Any(x => x.PermissaoInscricaoForaPrazo.SeqProcesso == seqProcesso
                                                                && x.PermissaoInscricaoForaPrazo.DataFim >= DateTime.Now && x.PermissaoInscricaoForaPrazo.DataInicio <= DateTime.Now)
                        });
            return usuario.PermissaoInscricaoForaPrazo;
        }

        public int BuscarCodigoDePessoaNosDadosMestres(long seqInscrito)
        {
            var integracaoOrigem = new IntegracaoDadoMestreData()
            {
                NomeBanco = TOKEN_DADOSMESTRES.BANCO_INSCRICAO,
                NomeTabela = TOKEN_DADOSMESTRES.BANCO_INSCRICAO_INSCRITO,
                NomeAtributoChave = TOKEN_DADOSMESTRES.BANCO_INSCRICAO_INSCRITO_SEQ,
                SeqAtributoChaveIntegracao = seqInscrito
            };

            var tabelaDestino = new TabelaData()
            {
                NomeBanco = TOKEN_DADOSMESTRES.BANCO_CAD,
                NomeTabela = TOKEN_DADOSMESTRES.BANCO_CAD_PESSOA,
                NomeAtributoChave = TOKEN_DADOSMESTRES.BANCO_CAD_PESSOA_SEQ
            };

            int? codigoPessoa = (int?)IntegracaoDadoMestreService.BuscarSeqAtributoChaveIntegracao(integracaoOrigem, tabelaDestino);

            return (int)codigoPessoa.Value;
        }

        /// <summary>
        /// Busca os dados de um inscrito
        /// </summary>
        /// <param name="seqInscrito">Sequencial do inscrito</param>
        /// <returns>Dados do inscrito</returns>
        public Inscrito BuscarInscrito(long seqInscrito)
        {
            return this.SearchByKey(seqInscrito, IncludesInscrito.Enderecos |
                                                             IncludesInscrito.Telefones |
                                                             IncludesInscrito.EnderecosEletronicos);
        }

        #region Validação de dados inscrito Step 1

        /// <summary>
        /// Valicação dos campos no primeiro passo da inscrição
        /// </summary>
        /// <param name="inscrito">Dados do inscrito</param>
        /// <param name="uidProcesso">Sequencial do processo</param>
        /// <returns>Verdadeiro ou ex</returns>
        private bool ValidatorInscritoStep1(Inscrito inscrito, Guid? uidProcesso)
        {
            // Realiza a validação de inscrito
            // Regras:
            // 1) Deve informar o nome do pai ou da mãe
            // 2) Se a nacionalidade for "Brasileira", o CPF e a Identidade são obrigatórios
            // 3) Se a nacionalidade não for "Brasileira", o Passaporte é obrigatório
            // 4) Se o pais for "Brasil", verifica se informou a UF/Cidade de naturalidade
            // 5) Se o pais não for "Brasil", verifica se informou a descrição da nacionalidade
            // 6) Baseado nos campos visivies do processo, verifica se os campos obrigatórios foram informados

            ProcessoCampoInscritoVisiveisVO camposVisiveis = ConfigurarVisibilidadeCamposPorProcesso(uidProcesso);

            //Valaidar campos
            ValidarConfiguracaoCampos(inscrito, camposVisiveis);

            // Valida campos obrigatórios
            ValidacaoCamposObrigatorios(inscrito, camposVisiveis);

            // Verifica se a data de nascimento é válida
            if (inscrito.DataNascimento.Year <= 1500 && camposVisiveis.ExibirDataNascimento)
            {
                throw new DataNascimentoInvalidaException();
            }
            // Verifica se o a nacionalidade é estrangeira e o pais não é o brasil
            if (inscrito.Nacionalidade != TipoNacionalidade.Brasileira && inscrito.CodigoPaisNacionalidade == CONSTANTS.CODIGO_PAIS_BRASIL && camposVisiveis.ExibirNacionalidade && camposVisiveis.ExibirPaisOrigem)
            {
                throw new PaisInvalidoParaNacionalidadeEstrangeiraException();
            }

            if (camposVisiveis.ExibirCPF && string.IsNullOrEmpty(inscrito.Cpf) && !camposVisiveis.ExibirPassaporte)
            {
                throw new CpfObrigatorioException();
            }

            if (inscrito.Nacionalidade == TipoNacionalidade.Brasileira && camposVisiveis.ExibirNacionalidade)
            {
                // Verifica se CPF foi informado
                if (String.IsNullOrWhiteSpace(inscrito.Cpf) && camposVisiveis.ExibirCPF)
                {
                    throw new InscritoCpfObrigatorioException();
                }

                // Verifica se a Identidade foi informada
                if ((string.IsNullOrWhiteSpace(inscrito.NumeroIdentidade) && camposVisiveis.ExibirNumeroIdentidade) ||
                    (string.IsNullOrWhiteSpace(inscrito.OrgaoEmissorIdentidade) && camposVisiveis.ExibirOrgaoEmissorIdentidade) ||
                    (string.IsNullOrWhiteSpace(inscrito.UfIdentidade) && camposVisiveis.ExibirUfIdentidade))
                {
                    throw new IdentidadeObrigatoriaException();
                }
            }
            else
            {
                // Verifica se o passaporte foi informado
                if ((inscrito.Nacionalidade == TipoNacionalidade.Estrangeira && camposVisiveis.ExibirNacionalidade) && ((String.IsNullOrWhiteSpace(inscrito.NumeroPassaporte)
                                                                                                                    || String.IsNullOrWhiteSpace(inscrito.DataValidadePassaporte.ToString())
                                                                                                                    || String.IsNullOrWhiteSpace(inscrito.CodigoPaisEmissaoPassaporte.ToString()))) && camposVisiveis.ExibirPassaporte)
                {
                    throw new InscritoPassaporteObrigatorioException();
                }

                if ((inscrito.Nacionalidade == TipoNacionalidade.BrasileiraNaturalizado && camposVisiveis.ExibirNacionalidade) && (String.IsNullOrWhiteSpace(inscrito.Cpf) && camposVisiveis.ExibirCPF))
                {
                    throw new InscritoCpfObrigatorioException();
                }

                if ((inscrito.Nacionalidade == TipoNacionalidade.BrasileiraNaturalizado && camposVisiveis.ExibirNacionalidade) &&
                    ((string.IsNullOrWhiteSpace(inscrito.NumeroIdentidade) && camposVisiveis.ExibirNumeroIdentidade) ||
                    (string.IsNullOrWhiteSpace(inscrito.OrgaoEmissorIdentidade) && camposVisiveis.ExibirOrgaoEmissorIdentidade) ||
                    (string.IsNullOrWhiteSpace(inscrito.UfIdentidade) && camposVisiveis.ExibirUfIdentidade)))
                {

                    throw new IdentidadeObrigatoriaException();
                }
            }

            if (camposVisiveis.ExibirNaturalidade)
            {
                // Se o pais for Brasil, verifica se informou a UF e Cidade da Naturalidade
                if (inscrito.CodigoPaisNacionalidade.Equals(CONSTANTS.CODIGO_PAIS_BRASIL))
                {
                    if (string.IsNullOrEmpty(inscrito.UfNaturalidade) || !inscrito.CodigoCidadeNaturalidade.HasValue)
                    {
                        throw new NaturalidadeBrasileiraInvalidaException();
                    }
                    else
                    {
                        inscrito.DescricaoNaturalidadeEstrangeira = null;
                    }
                }
                else // Se pais diferente de Brasil, verifica se informou a descrição da naturalidade
                {
                    if (string.IsNullOrEmpty(inscrito.DescricaoNaturalidadeEstrangeira))
                    {
                        throw new NaturalidadeEstrangeiraInvalidaException();
                    }
                    else
                    {
                        inscrito.UfNaturalidade = null;
                        inscrito.CodigoCidadeNaturalidade = null;
                    }
                }
            }

            // O preenchimento do campo "Nome da mãe" ou "Nome do pai" é obrigatório
            if (string.IsNullOrEmpty(inscrito.NomeMae) && string.IsNullOrEmpty(inscrito.NomePai) && camposVisiveis.ExibirFiliacao)
            {
                throw new InscritoSemNomeMaeOuNomePaiException();
            }
            else if (inscrito.NomeMae?.ToLower() == inscrito.NomePai?.ToLower() && camposVisiveis.ExibirFiliacao)
            {
                throw new InscritoNomeMaeNomePaiIdenticosException();
            }

            //Se preencher um campo do passaporte os outros devem ser preenchidos
            if (camposVisiveis.ExibirPassaporte)
            {
                if (!ValidacaoPassaporte(inscrito))
                {
                    throw new DadosPassaporteExeption();
                }
            }

            return true;
        }

        /// <summary>
        /// Configurações para validação
        /// - Tamanho máximo do Nome = 100
        /// - Tamanho máximo do Nome Social = 100
        /// - Data de nascimento deve ser anterior a data de hoje
        /// - CPF é válido
        /// - Tamanho máximo do Nome da mãe = 100
        /// - Tamanho máximo do Nome do pai = 100
        /// - Tamanho máximo do email = 100
        /// - Nome, Nome Social, Nome do Pai e Nome da mãe devem ter pelo menos um sobrenome
        /// </summary>
        public void ValidarConfiguracaoCampos(Inscrito inscrito, ProcessoCampoInscritoVisiveisVO camposVisiveis)
        {
            if (string.IsNullOrEmpty(inscrito.Nome))
            {
                throw new NomeInscritoObrigatorioException();
            }

            // Regex para validação de Nome - Valida pelo menos um nome e um sobrenome separados por um espaço
            const string NomeRegex = @"^[^\s]+( +[^\s]+)+$";
            if (inscrito.Nome.Length > 100 && camposVisiveis.ExibirNome)
            {
                throw new CampoExcedeLimiteCaracteresException("Nome", "100");
            }

            if (!Regex.IsMatch(inscrito.Nome, NomeRegex) && camposVisiveis.ExibirNome)
            {
                throw new NomeSobrenomeObrigariorioException("Nome");

            }

            if (inscrito.DataNascimento > DateTime.Now && camposVisiveis.ExibirDataNascimento)
            {
                throw new DataNascimentoInvalidaException();
            }

            if (!string.IsNullOrEmpty(inscrito.Cpf) && !ValidarCPFValido(inscrito.Cpf) && camposVisiveis.ExibirCPF)
            {
                throw new InscritoCpfInvalidoException();
            }

            if (camposVisiveis.ExibirFiliacao)
            {
                if (inscrito.NomeMae?.Length > 100)
                {
                    throw new CampoExcedeLimiteCaracteresException("Nome da mãe", "100");
                }

                if (!string.IsNullOrEmpty(inscrito.NomeMae) && !Regex.IsMatch(inscrito.NomeMae, NomeRegex))
                {
                    throw new NomeSobrenomeObrigariorioException("Nome da mãe");

                }

                if (inscrito.NomePai?.Length > 100)
                {
                    throw new CampoExcedeLimiteCaracteresException("Nome do pai", "100");
                }

                if (!string.IsNullOrEmpty(inscrito.NomePai) && !Regex.IsMatch(inscrito.NomePai, NomeRegex))
                {
                    throw new NomeSobrenomeObrigariorioException("Nome do pai");

                }
            }
        }

        /// <summary>
        /// Valida se algum campo do passaporte veio vazio
        /// </summary>
        /// <param name="inscrito">Dados do Inscrito</param>
        private bool ValidacaoPassaporte(Inscrito inscrito)
        {
            bool dadosPassaporteValido = true;

            //Se preencheu somente o numero do passaporte e burlou os outros
            if (!String.IsNullOrWhiteSpace(inscrito.NumeroPassaporte) &&
                (String.IsNullOrWhiteSpace(inscrito.CodigoPaisEmissaoPassaporte.ToString()) ||
                 String.IsNullOrWhiteSpace(inscrito.DataValidadePassaporte.ToString())))
            {
                dadosPassaporteValido = false;
            }

            //Se preencheu somente o data de validade e burlou os outros
            if (!String.IsNullOrWhiteSpace(inscrito.DataValidadePassaporte.ToString()) &&
                (String.IsNullOrWhiteSpace(inscrito.CodigoPaisEmissaoPassaporte.ToString()) ||
                 String.IsNullOrWhiteSpace(inscrito.NumeroPassaporte)))
            {
                dadosPassaporteValido = false;
            }

            //Se preencheu somente o codigo do pais e burlou os outros
            if (!String.IsNullOrWhiteSpace(inscrito.CodigoPaisEmissaoPassaporte.ToString()) &&
                (String.IsNullOrWhiteSpace(inscrito.DataValidadePassaporte.ToString()) ||
                 String.IsNullOrWhiteSpace(inscrito.NumeroPassaporte)))
            {
                dadosPassaporteValido = false;
            }

            return dadosPassaporteValido;

        }

        /// <summary>
        /// Validacao de Campos Obrigatorios
        /// </summary>
        /// <param name="inscrito">Dados do Inscrito</param>
        private void ValidacaoCamposObrigatorios(Inscrito inscrito, ProcessoCampoInscritoVisiveisVO camposVisiveis)
        {
            if (String.IsNullOrWhiteSpace(inscrito.Nome) && camposVisiveis.ExibirNome)
            {
                throw new NomeInscritoObrigatorioException();
            }

            if (String.IsNullOrWhiteSpace(inscrito.DataNascimento.ToString()) && camposVisiveis.ExibirDataNascimento)
            {
                throw new DataNascimentoInscritoObrigatorioException();
            }

            if ((inscrito.Sexo == Sexo.Nenhum || inscrito.Sexo == null) && camposVisiveis.ExibirSexo)
            {
                throw new SexoInscritoObrigatorioException();
            }

            if ((inscrito.CodigoPaisNacionalidade == null || inscrito.CodigoPaisNacionalidade == 0) && camposVisiveis.ExibirPaisOrigem)
            {
                throw new PaisOrigemInscritoObrigatorioException();
            }

            if (inscrito.Nacionalidade == TipoNacionalidade.Nenhum && camposVisiveis.ExibirNacionalidade)
            {
                throw new NacionalidadeInscritoObrigatorioException();
            }

            if (string.IsNullOrEmpty(inscrito.Email))
            {
                throw new EmailObrigatorioException();
            }
        }

        /// <summary>
        /// Configura a visibilidade dos campos por processo
        /// </summary>
        /// <param name="uidProcesso">Guid Processo</param>
        /// <returns>Campos visiveis do processo</returns>
        private ProcessoCampoInscritoVisiveisVO ConfigurarVisibilidadeCamposPorProcesso(Guid? uidProcesso)
        {
            ProcessoCampoInscritoVisiveisVO retorno = new ProcessoCampoInscritoVisiveisVO();

            if (uidProcesso.HasValue)
            {
                List<ProcessoCampoInscritoVO> camposInscritoProcesso = ProcessoCampoInscritoDomainService.BuscarCamposInscritosPorUIIDProcesso(uidProcesso.Value).TransformList<ProcessoCampoInscritoVO>();

                foreach (var campo in camposInscritoProcesso)
                {
                    if (campo.CampoInscrito == CampoInscrito.CPF)
                    {
                        retorno.ExibirCPF = true;
                    }

                    if (campo.CampoInscrito == CampoInscrito.DataNascimento)
                    {
                        retorno.ExibirDataNascimento = true;
                    }

                    if (campo.CampoInscrito == CampoInscrito.Email)
                    {
                        retorno.ExibirEmail = true;
                    }

                    if (campo.CampoInscrito == CampoInscrito.Endereco)
                    {
                        retorno.ExibirEndereco = true;
                    }

                    if (campo.CampoInscrito == CampoInscrito.EstadoCivil)
                    {
                        retorno.ExibirEstadoCivil = true;
                    }

                    if (campo.CampoInscrito == CampoInscrito.Filiacao)
                    {
                        retorno.ExibirFiliacao = true;
                    }

                    if (campo.CampoInscrito == CampoInscrito.Nacionalidade)
                    {
                        retorno.ExibirNacionalidade = true;
                    }

                    if (campo.CampoInscrito == CampoInscrito.Naturalidade)
                    {
                        retorno.ExibirNaturalidade = true;
                    }

                    if (campo.CampoInscrito == CampoInscrito.Nome)
                    {
                        retorno.ExibirNome = true;
                    }

                    if (campo.CampoInscrito == CampoInscrito.NumeroIdentidade)
                    {
                        retorno.ExibirNumeroIdentidade = true;
                    }

                    if (campo.CampoInscrito == CampoInscrito.OrgaoEmissorIdentidade)
                    {
                        retorno.ExibirOrgaoEmissorIdentidade = true;
                    }

                    if (campo.CampoInscrito == CampoInscrito.OutrosEndereçosEletronicos)
                    {
                        retorno.ExibirOutrosEnderecosEletronicos = true;
                    }

                    if (campo.CampoInscrito == CampoInscrito.PaisOrigem)
                    {
                        retorno.ExibirPaisOrigem = true;
                    }

                    if (campo.CampoInscrito == CampoInscrito.Passaporte)
                    {
                        retorno.ExibirPassaporte = true;
                    }

                    if (campo.CampoInscrito == CampoInscrito.Sexo)
                    {
                        retorno.ExibirSexo = true;
                    }

                    if (campo.CampoInscrito == CampoInscrito.Telefone)
                    {
                        retorno.ExibirTelefone = true;
                    }

                    if (campo.CampoInscrito == CampoInscrito.UfIdentidade)
                    {
                        retorno.ExibirUfIdentidade = true;
                    }
                }
            }
            else
            {
                retorno.ExibirCPF = true;
                retorno.ExibirDataNascimento = true;
                retorno.ExibirEmail = true;
                retorno.ExibirEndereco = true;
                retorno.ExibirEstadoCivil = true;
                retorno.ExibirFiliacao = true;
                retorno.ExibirNacionalidade = true;
                retorno.ExibirNaturalidade = true;
                retorno.ExibirNome = true;
                retorno.ExibirNumeroIdentidade = true;
                retorno.ExibirOrgaoEmissorIdentidade = true;
                retorno.ExibirOutrosEnderecosEletronicos = true;
                retorno.ExibirPaisOrigem = true;
                retorno.ExibirPassaporte = true;
                retorno.ExibirSexo = true;
                retorno.ExibirTelefone = true;
                retorno.ExibirUfIdentidade = true;
            }

            return retorno;
        }

        /// <summary>
        /// Validates if a CPF number is valid.
        /// </summary>
        /// <param name="cpf">The CPF number to validate.</param>
        /// <returns>True if the CPF number is valid, otherwise false.</returns>
        private bool ValidarCPFValido(string cpf)
        {
            // Remove any non-digit characters from the CPF
            cpf = Regex.Replace(cpf, @"\D", "");

            // Check if the CPF has 11 digits
            if (cpf.Length != 11)
            {
                return false;
            }

            // Calculate the first verification digit
            int sum = 0;
            for (int i = 0; i < 9; i++)
            {
                sum += int.Parse(cpf[i].ToString()) * (10 - i);
            }
            int digit1 = 11 - (sum % 11);
            if (digit1 >= 10)
            {
                digit1 = 0;
            }

            // Calculate the second verification digit
            sum = 0;
            for (int i = 0; i < 10; i++)
            {
                sum += int.Parse(cpf[i].ToString()) * (11 - i);
            }
            int digit2 = 11 - (sum % 11);
            if (digit2 >= 10)
            {
                digit2 = 0;
            }

            // Check if the verification digits match the CPF
            if (int.Parse(cpf[9].ToString()) != digit1 || int.Parse(cpf[10].ToString()) != digit2)
            {
                return false;
            }

            return true;
        }

        #endregion

        #region Validação de dados inscrito Step 2

        /// <summary>
        /// Realiza a validação de inscrito
        /// </summary>
        /// <param name="inscrito">Inscrito a ser validado</param>
        /// <param name="uidProcesso">Guid do Processo</param>
        ///
        private bool ValidatorInscritoStep2(Inscrito inscrito, Guid? uidProcesso)
        {

            ProcessoCampoInscritoVisiveisVO camposVisiveis = ConfigurarVisibilidadeCamposPorProcesso(uidProcesso);

            ValidarConfiguracaoCamposStep2(inscrito, camposVisiveis);

            // Valida campos obrigatórios
            ValidacaoCamposObrigatoriosStep2(inscrito, camposVisiveis);

            if (camposVisiveis.ExibirEndereco)
            {
                if (inscrito.Enderecos == null || !inscrito.Enderecos.Any(f => f.TipoEndereco == TipoEndereco.Residencial))
                {
                    throw new EnderecoResidencialObrigatorio();
                }

                var endCorrepondency = inscrito.Enderecos.Count(f => f.Correspondencia.HasValue && f.Correspondencia.Value);
                if (endCorrepondency == 0)
                {
                    throw new EnderecoCorrespondenciaObrigatorioException();
                }
                else if (endCorrepondency > 1)
                {
                    throw new MultiploEnderecoCorrespondenciaException();
                }

                var enderecoDuplicado = inscrito.Enderecos.Any(a => EnderecoDuplicado(a, inscrito.Enderecos));
                if (enderecoDuplicado)
                {
                    throw new EnderecoDuplicadoException();
                }
            }
            else if (!camposVisiveis.ExibirEndereco && inscrito?.Enderecos?.Count > 0)
            {
                var enderecoDuplicado = inscrito.Enderecos.Any(a => EnderecoDuplicado(a, inscrito.Enderecos));
                if (enderecoDuplicado)
                {
                    throw new EnderecoDuplicadoException();
                }
            }

            if (camposVisiveis.ExibirTelefone)
            {
                if (inscrito.Telefones == null || inscrito.Telefones?.Count == 0)
                {
                    throw new TelefoneInvalidoException();
                }
                else
                {
                    //verifica se tem algum telefone inválido
                    bool telefoneInvalido = inscrito.Telefones.Any(a => (a.CodigoPais == 0 || a.CodigoArea == 0 || a.TipoTelefone == TipoTelefone.Nenhum || string.IsNullOrEmpty(a.Numero)));
                    if (telefoneInvalido)
                    {
                        throw new TelefonePreenchidoMasInvalidoException();
                    }
                }
            }

            if (camposVisiveis.ExibirEmail)
            {
                if (string.IsNullOrEmpty(inscrito.Email))
                {
                    throw new EmailInvalidoException();
                }
            }

            if (camposVisiveis.ExibirOutrosEnderecosEletronicos)
            {
                if (inscrito.EnderecosEletronicos.Count() > 0)
                {
                    bool enderecoEletronicoInvalido = inscrito.EnderecosEletronicos.Any(e => (e.TipoEnderecoEletronico == TipoEnderecoEletronico.Nenhum || string.IsNullOrEmpty(e.Descricao)));
                    if (enderecoEletronicoInvalido)
                    {
                        throw new EnderecoEletronicoObrigatorioException();
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Validacao de Campos Obrigatorios
        /// </summary>
        /// <param name="inscrito">Dados do Inscrito</param>
        private void ValidacaoCamposObrigatoriosStep2(Inscrito inscrito, ProcessoCampoInscritoVisiveisVO camposVisiveis)
        {
            ValidaCamposEnderecosObrigatorios(inscrito, camposVisiveis);
            var seqAplicacaoSAS = AplicacaoService.BuscarAplicacaoPelaSigla(SMCContext.ApplicationId).Sigla;

            //Para o GPI Administrativo, o campo telefone mesmo estando como não visivel, deve impedir que salve dados nulos no banco
            //Valida pelo ambiente para não causar impacto nas regras existentes do inscrição
            if (seqAplicacaoSAS == SIGLA_APLICACAO.GPI_ADMIN)
            {
                foreach (var telefone in inscrito.Telefones)
                {
                    if (telefone.CodigoArea == 0 ||
                        telefone.CodigoPais == 0 ||
                        string.IsNullOrEmpty(telefone.Numero) ||
                        telefone.TipoTelefone == TipoTelefone.Nenhum)
                    {
                        throw new TelefonePreenchidoMasInvalidoException();
                    }
                }

            }
            else
            {
                if (camposVisiveis.ExibirTelefone)
                {
                    foreach (var telefone in inscrito.Telefones)
                    {
                        if (telefone.CodigoArea == 0 ||
                            telefone.CodigoPais == 0 ||
                            string.IsNullOrEmpty(telefone.Numero) ||
                            telefone.TipoTelefone == TipoTelefone.Nenhum)
                        {
                            throw new TelefonePreenchidoMasInvalidoException();
                        }
                    }
                }
            }


            if (camposVisiveis.ExibirOutrosEnderecosEletronicos)
            {
                foreach (var enderecoEletronico in inscrito.EnderecosEletronicos)
                {
                    if (String.IsNullOrWhiteSpace(enderecoEletronico.TipoEnderecoEletronico.ToString()) ||
                        String.IsNullOrWhiteSpace(enderecoEletronico.Descricao))
                    {
                        throw new EnderecoEletronicoObrigatorioException();
                    }
                }
            }
        }

        /// <summary>
        /// Validar regras para os endereços
        /// </summary>
        /// <param name="inscrito">Dados do inscrito</param>
        private void ValidaCamposEnderecosObrigatorios(Inscrito inscrito, ProcessoCampoInscritoVisiveisVO camposVisiveis)
        {
            //Se o campo telefone estiver visivel é obrigatório informar pelo menos um telefone
            if (camposVisiveis.ExibirTelefone)
            {
                if (inscrito.Telefones.Count == 0)
                {
                    throw new TelefoneObrigatorioException();
                }
            }

            //se o campo endereço estiver visivel é obrigatório informar pelo menos um endereço
            if (camposVisiveis.ExibirEndereco)
            {
                if (inscrito.Enderecos.Count == 0)
                {
                    throw new EnderecoObrigatorioException();
                }
            }

            if (camposVisiveis.ExibirEndereco)
            {
                foreach (var item in inscrito.Enderecos)
                {
                    if (String.IsNullOrWhiteSpace(item.CodigoPais.ToString()))
                    {

                        throw new PaisEnderecoObrigatorioException();
                    }

                    if (String.IsNullOrWhiteSpace(item.Logradouro))
                    {
                        throw new LogradouroEnderecoObrigatorioException();
                    }

                    if ((!String.IsNullOrWhiteSpace(item.Cep) && item.CodigoPais == CONSTANTS.CODIGO_PAIS_BRASIL) && String.IsNullOrWhiteSpace(item.Numero))
                    {
                        throw new NumeroEnderecoObrigatorioException();
                    }

                    if ((!String.IsNullOrWhiteSpace(item.Cep) && item.CodigoPais == CONSTANTS.CODIGO_PAIS_BRASIL) && String.IsNullOrWhiteSpace(item.Bairro))
                    {
                        throw new BairroEnderecoException();
                    }

                    if (String.IsNullOrWhiteSpace(item.NomeCidade))
                    {
                        throw new CidadeEnderecoException();
                    }

                    if ((!String.IsNullOrWhiteSpace(item.Cep) && item.CodigoPais == CONSTANTS.CODIGO_PAIS_BRASIL) && String.IsNullOrWhiteSpace(item.Uf))
                    {
                        throw new EstadoEnderecoException();
                    }

                    if ((String.IsNullOrWhiteSpace(item.Cep) && item.CodigoPais == CONSTANTS.CODIGO_PAIS_BRASIL))
                    {
                        throw new CepEnderecoObrigatorioException();
                    }
                }
            }
        }

        private bool EnderecoDuplicado(Endereco enderecoBase, IEnumerable<Endereco> enderecos)
        {
            string[] IgnoredProperties =
                        new List<string>
                        {
                            nameof(Endereco.Seq),
                            nameof(Endereco.UsuarioAlteracao),
                            nameof(Endereco.UsuarioInclusao),
                            nameof(Endereco.DataAlteracao),
                            nameof(Endereco.DataInclusao),
                            nameof(Endereco.Correspondencia)
                        }.ToArray();

            return enderecos
                .Count(c => SMCReflectionHelper
                    .CompareExistingPrimitivePropertyValues(enderecoBase, c, IgnoredProperties)) > 1;
        }

        /// <summary>
        /// Configurações para validação
        /// - Validar campo emal
        /// - Se email é valido
        /// </summary>
        public void ValidarConfiguracaoCamposStep2(Inscrito inscrito, ProcessoCampoInscritoVisiveisVO camposVisiveis)
        {
            if (camposVisiveis.ExibirEmail)
            {
                if (inscrito.Email?.Length > 100)
                {
                    throw new CampoExcedeLimiteCaracteresException("Email", "100");
                }


                //var emailSAS = SMCContext.User.SMCGetEmail();

                //if (!(emailSAS.Contains(inscrito.Email) && inscrito.Email != emailSAS))
                //{
                //    modelo.Email = emailSAS;
                //}

                if (!ValidarEmail(inscrito.Email))
                {
                    throw new EmailInvalidoException();
                }
            }
        }

        //validar se inscrito.email e um email valido
        public bool ValidarEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        /// <summary>
        /// Valida se os dados necessários para o processo especificado estão preenchidos para o inscrito indicado.
        /// </summary>
        /// <param name="seqInscrito">O sequência do inscrito.</param>
        /// <param name="uidProcesso">O identificador exclusivo do processo.</param>
        /// <returns>Verdadeiro se os dados forem válidos</returns>
        public bool ValidarDadosInscritoPreenchidosParaProcesso(long seqInscrito, Guid uidProcesso)
        {
            var inscrito = BuscarInscrito(seqInscrito);

            ValidatorInscritoStep1(inscrito, uidProcesso);
            ValidatorInscritoStep2(inscrito, uidProcesso);

            return true;
        }
    }

}

