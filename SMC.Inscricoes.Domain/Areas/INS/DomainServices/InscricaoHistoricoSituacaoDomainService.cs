using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.DadosMestres.ServiceContract.Areas.PES.Interfaces;
using SMC.Formularios.ServiceContract.Areas.TMP.Interfaces;
using SMC.Formularios.ServiceContract.TMP.Data;
using SMC.Framework;
using SMC.Framework.Domain;
using SMC.Framework.Exceptions;
using SMC.Framework.Extensions;
using SMC.Framework.Logging;
using SMC.Framework.Specification;
using SMC.Framework.UnitOfWork;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Inscricoes.Common.Areas.INS.Exceptions;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using SMC.Inscricoes.Domain.Areas.INS.ValueObjects;
using SMC.Inscricoes.Domain.Areas.SEL.DomainServices;
using SMC.Inscricoes.Domain.Areas.SEL.Models;
using SMC.IntegracaoAcademico.ServiceContract.Areas.IAC.Data;
using SMC.IntegracaoAcademico.ServiceContract.Areas.IAC.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SMC.Inscricoes.Domain.Areas.INS.DomainServices
{
    public class InscricaoHistoricoSituacaoDomainService : InscricaoContextDomain<InscricaoHistoricoSituacao>
    {
        #region DomainService

        private InscritoDomainService InscritoDomainService => Create<InscritoDomainService>();

        private IIntegracaoAcademicoService IntegracaoAcademicoService => Create<IIntegracaoAcademicoService>();

        private ProcessoDomainService ProcessoDomainService => this.Create<ProcessoDomainService>();

        private InscricaoDomainService InscricaoDomainService
        {
            get { return this.Create<InscricaoDomainService>(); }
        }

        private EtapaProcessoDomainService EtapaProcessoDomainService
        {
            get { return this.Create<EtapaProcessoDomainService>(); }
        }

        private TipoProcessoSituacaoDomainService TipoProcessoSituacaoDomainService
        {
            get { return this.Create<TipoProcessoSituacaoDomainService>(); }
        }

        private InscricaoOfertaHistoricoSituacaoDomainService InscricaoOfertaHistoricoSituacaoDomainService
        {
            get { return Create<InscricaoOfertaHistoricoSituacaoDomainService>(); }
        }

        InscricaoEnvioNotificacaoDomainService InscricaoEnvioNotificacaoDomainService => Create<InscricaoEnvioNotificacaoDomainService>();

        InscricaoOfertaDomainService InscricaoOfertaDomainService => Create<InscricaoOfertaDomainService>();

        #endregion DomainService

        #region Services

        private IIntegracaoDadoMestreService IntegracaoDadoMestreService => Create<IIntegracaoDadoMestreService>();

        private IEtapaService EtapaService
        {
            get { return this.Create<IEtapaService>(); }
        }

        private ISituacaoService SituacaoService
        {
            get { return this.Create<ISituacaoService>(); }
        }

        private ITemplateProcessoService TemplateProcessoService
        {
            get { return Create<ITemplateProcessoService>(); }
        }

        #endregion Services

        public void ExisteCheckinRegistradaParaEssaInscricao(long seqInscricao, long seqTipoSituacaoDestino)
        {
            var includes = IncludesInscricao.HistoricosSituacao_TipoProcessoSituacao |
                          IncludesInscricao.Ofertas_HistoricosSituacao_TipoProcessoSituacao;

            var inscricao = InscricaoDomainService.SearchByKey(new SMCSeqSpecification<Inscricao>(seqInscricao), includes);

            var possuiCheckin = InscricaoOfertaDomainService.VerificaPossuiCkeckin(inscricao.Seq);
            var tokenSituacaoDestino = TipoProcessoSituacaoDomainService.BuscarTipoProcessoSituacao(seqTipoSituacaoDestino);

            if (possuiCheckin && tokenSituacaoDestino.Token == TOKENS.SITUACAO_INSCRICAO_CANCELADA)
            {
                throw new ExisteInscricaoRegistradaParaEssaInscricaoException();
            }
        }

        /// <summary>
        /// Altera a situação das inscrições informadas
        /// Todas as situações informadas devem estar na mesma situação atual e no mesmo processo e etapa
        /// </summary>
        /// <param name="seqTipoProcessoSituacaoDestino">Sequencial do tipo Processo Situação para destino das inscrições</param>
        /// <param name="seqInscricoes">Lista de sequencial das inscrições a serem alteradas</param>
        public void AlterarSituacaoInscricoes(AlteracaoSituacaoVO alterarSituacaoDto)
        {
            using (var unitOfWork = SMCUnitOfWork.Begin())
            {
                try
                {
                    // O -1 é uma gambiarra que indica que o a situação esta retornando para situação anterior
                    //TODO: CORRIGIR ISSO O QUANTO ANTES
                    if (alterarSituacaoDto.SeqTipoProcessoSituacaoDestino != -1)
                        ExisteCheckinRegistradaParaEssaInscricao(alterarSituacaoDto.SeqInscricoes.FirstOrDefault(), alterarSituacaoDto.SeqTipoProcessoSituacaoDestino);

                    //Resgatando a situação atual (ela será a mesma para todas as inscrições informadas)
                    var dadosBusca = InscricaoDomainService.SearchProjectionByKey(alterarSituacaoDto.SeqInscricoes.FirstOrDefault(), x => new
                    {
                        x.Processo.SeqTemplateProcessoSGF,
                        SeqSituacaoSGF = x.HistoricosSituacao.Where(s => s.Atual).Select(t => t.TipoProcessoSituacao.SeqSituacao).FirstOrDefault(),
                        x.SeqProcesso,
                        x.Processo.SeqTipoProcesso,
                        x.HistoricosSituacao.Where(s => s.Atual).FirstOrDefault().SeqTipoProcessoSituacao,
                        Exportado = x.Ofertas.Any(o => o.Exportado ?? false),
                        x.Processo.AnoReferencia,
                        x.Processo.SemestreReferencia
                    });

                    // Busca os dados da situação de destino
                    var dadosSituacaoDestino = TipoProcessoSituacaoDomainService.SearchProjectionByKey(alterarSituacaoDto.SeqTipoProcessoSituacaoDestino, f => new
                    {
                        SeqSituacaoDestino = f.SeqSituacao,
                        f.Token
                    });

                    if (alterarSituacaoDto.SeqTipoProcessoSituacaoDestino == -1)
                        DesfazerSituacaoInscricoes(alterarSituacaoDto.SeqInscricoes, dadosBusca.SeqTemplateProcessoSGF, dadosBusca.SeqSituacaoSGF);
                    else
                    {
                        if (dadosSituacaoDestino != null)
                        {
                            VerificaRequisitosSituacao(dadosSituacaoDestino.SeqSituacaoDestino, alterarSituacaoDto.SeqMotivoSGF, alterarSituacaoDto.Justificativa);

                            AlterarSituacoesEmLote(alterarSituacaoDto, dadosBusca.SeqTemplateProcessoSGF, dadosBusca.SeqSituacaoSGF, dadosSituacaoDestino.SeqSituacaoDestino);
                        }
                        else
                        {
                            VerificaRequisitosSituacao(0, alterarSituacaoDto.SeqMotivoSGF, alterarSituacaoDto.Justificativa);
                            AlterarSituacoesEmLote(alterarSituacaoDto, dadosBusca.SeqTemplateProcessoSGF, dadosBusca.SeqSituacaoSGF, 0);
                        }
                    }

                    var integrarLegado = ProcessoDomainService.VerificarIntegracaoLegado(dadosBusca.SeqProcesso);
                    if (integrarLegado)
                    {
                        // Busca os dados da situação ATUAL da solicitação
                        var dadosSituacaoAtual = TipoProcessoSituacaoDomainService.SearchProjectionByKey(dadosBusca.SeqTipoProcessoSituacao, f => new
                        {
                            SeqSituacaoDestino = f.SeqSituacao,
                            f.Token,
                        });

                        // 1. Verificar se a situação está sendo alterada para "Inscrição Deferida".
                        if (dadosSituacaoDestino != null && dadosSituacaoDestino.Token == TOKENS.SITUACAO_INSCRICAO_DEFERIDA)
                        {
                            // 1.1 Recupera se aplica bolsa de ex aluno pelo tipodo do processo
                            var bolsaExAluno = new TipoProcessoDomainService().BuscarTipoProcessoPorProcesso(dadosBusca.SeqProcesso).BolsaExAluno;
                            var contains = new SMCContainsSpecification<Inscricao, long>(x => x.Seq, alterarSituacaoDto.SeqInscricoes.ToArray());
                            var dadosInscritos = InscricaoDomainService.SearchProjectionBySpecification(contains, x => new TransformaInscritoAlunoData
                            {
                                Ano = (short)x.Processo.AnoReferencia,
                                Semestre = (short)x.Processo.SemestreReferencia,
                                CodigoCursoTurno = x.Ofertas.FirstOrDefault().Oferta.CodigoOrigem ?? 0,
                                SeqTipoProcesso = (int)x.Processo.SeqTipoProcesso,
                                SeqInscricao = (int)x.Seq,
                                SeqInscricaoOferta = (long?)x.Ofertas.FirstOrDefault().Seq ?? 0,
                                CodigoPais = x.Inscrito.CodigoPaisNacionalidade ?? 0,
                                CPF = x.Inscrito.Cpf,
                                DataNascimento = x.Inscrito.DataNascimento,
                                Email = x.Inscrito.Email,
                                Enderecos = x.Inscrito.Enderecos.Select(e => new TransformaInscritoAlunoEnderecoData
                                {
                                    Bairro = e.Bairro,
                                    CEP = e.Cep,
                                    CodigoCidade = e.CodigoCidade,
                                    CodigoPais = e.CodigoPais,
                                    Complemento = e.Complemento,
                                    Correspondencia = e.Correspondencia ?? false,
                                    Logradouro = e.Logradouro,
                                    NomeCidadeEstrangeiro = e.NomeCidade,
                                    Numero = e.Numero,
                                    UF = e.Uf,
                                    Residencial = e.TipoEndereco == Localidades.Common.Areas.LOC.Enums.TipoEndereco.Residencial
                                }).ToList(),
                                EstadoCivil = x.Inscrito.EstadoCivil ?? DadosMestres.Common.Areas.PES.Enums.EstadoCivil.Nenhum,
                                IdentidadeNumero = x.Inscrito.NumeroIdentidade,
                                IdentidadeOrgaoEmissor = x.Inscrito.OrgaoEmissorIdentidade,
                                IdentidadeUF = x.Inscrito.UfIdentidade,
                                NacionalidadeTipo = x.Inscrito.Nacionalidade,
                                NaturalCidadeEstadoEstrangeira = x.Inscrito.DescricaoNaturalidadeEstrangeira,
                                NaturalCodigoCidade = x.Inscrito.CodigoCidadeNaturalidade ?? 0,
                                NaturalCodigoUF = x.Inscrito.UfNaturalidade,
                                Nome = x.Inscrito.Nome,
                                NomeMae = x.Inscrito.NomeMae,
                                NomePai = x.Inscrito.NomePai,
                                Passaporte = x.Inscrito.NumeroPassaporte,
                                Sexo = x.Inscrito.Sexo ?? Sexo.Nenhum,
                                Telefones = x.Inscrito.Telefones.Select(t => new TransformaInscritoAlunoTelefoneData
                                {
                                    DDD = t.CodigoArea.ToString(),
                                    Numero = t.Numero,
                                    TipoTelefone = t.TipoTelefone
                                }).ToList()
                            }).ToList();

                            foreach (var item in dadosInscritos)
                            {
                                // Recupera o código de pessoa no CAD
                                //item.CodigoPessoa = InscritoDomainService.BuscarCodigoDePessoaNosDadosMestres(item.SeqInscrito);
                                item.Usuario = SMCContext.User.Identity.Name;

                                // 1.1.1 Verificação de vagas de bolsta
                                // 1.1.1.1 chamada st_candidato_apoto_bolsta_obtencao_novo_titulo
                                var seqOferta = InscricaoOfertaDomainService.SearchProjectionByKey(item.SeqInscricaoOferta, p => p.SeqOferta);
                                item.BolsaObtencaoNovoTitulo = bolsaExAluno && InscricaoDomainService.ValidarAptoBolsaNovoTitulo(item.SeqInscricao, seqOferta);

                                // 1.1.1.1.1 (trow caso a proc retorne erro)
                                IntegracaoAcademicoService.TransformaInscritoAluno(item);

                                if (item.BolsaObtencaoNovoTitulo)
                                {
                                    // 1.1.1.1.2.1 Registar que o candidato recebeu bolsa
                                    var updateRecebeuBolsta = new Inscricao()
                                    {
                                        Seq = item.SeqInscricao,
                                        RecebeuBolsa = true
                                    };
                                    InscricaoDomainService.UpdateFields(updateRecebeuBolsta, p => p.RecebeuBolsa);

                                    // 1.1.1.1.2.2 Enviar notificação recebimento bolsa
                                    InscricaoEnvioNotificacaoDomainService.EnviarNotificacaoRecebimentoBolsa(item.SeqInscricao, seqOferta);
                                }

                                // 1.1.2. Incluir no histórico de situação da inscrição oferta as situações na seguinte ordem: CANDIDATO_SELECIONADO (atual: 0 / atual etapa: 1), CONVOCADO (atual: 1, atual etapa: 1).
                                InscricaoOfertaHistoricoSituacaoDomainService.AlterarHistoricoSituacao(new AlterarHistoricoSituacaoVO()
                                {
                                    SeqProcesso = dadosBusca.SeqProcesso,
                                    SeqInscricoesOferta = new List<long> { item.SeqInscricaoOferta },
                                    TokenSituacaoDestino = TOKENS.SITUACAO_CANDIDATO_SELECIONADO,
                                });

                                InscricaoOfertaHistoricoSituacaoDomainService.AlterarHistoricoSituacao(new AlterarHistoricoSituacaoVO()
                                {
                                    SeqProcesso = dadosBusca.SeqProcesso,
                                    SeqInscricoesOferta = new List<long> { item.SeqInscricaoOferta },
                                    TokenSituacaoDestino = TOKENS.SITUACAO_CONVOCADO
                                });
                            }
                        }
                    }

                    unitOfWork.Commit();
                }
                catch (Exception ex)
                {
                    unitOfWork.Rollback();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Altera a justificativa e o motivo de uma situação
        /// </summary>
        /// <param name="seqHistoricoSituacao">Historico da situação a ser alterada</param>
        /// <param name="justificativa">Justificativa informada</param>
        /// <param name="seqMotivo">Motivo informado</param>
        public void AlterarMotivoEJustificativaSituacao(long seqHistoricoSituacao, string justificativa, long? seqMotivo)
        {
            // Busca a situação do SGF correspondente ao histórico de situação sendo alterado
            var spec = new SMCSeqSpecification<InscricaoHistoricoSituacao>(seqHistoricoSituacao);
            var seqSituacaoSGF = this.SearchProjectionByKey(spec, x => x.TipoProcessoSituacao.SeqSituacao);

            // Realiza as validações necessárias
            VerificaRequisitosSituacao(seqSituacaoSGF, seqMotivo, justificativa);

            // Se passou nas validações e chegou até aqui, atualiza a justificativa e o motivo
            var historico = new InscricaoHistoricoSituacao()
            {
                Seq = seqHistoricoSituacao,
                Justificativa = justificativa,
                SeqMotivoSituacaoSGF = seqMotivo
            };
            this.UpdateFields(historico, x => x.Justificativa, x => x.SeqMotivoSituacaoSGF);
        }

        private void VerificaRequisitosSituacao(long seqSituacaoSGF, long? seqMotivo, string justificativa)
        {
            var requisitosSituacao = SituacaoService.VerificarRequisitosSituacao(seqSituacaoSGF, seqMotivo);

            if (requisitosSituacao.ExigeMotivo && seqMotivo.GetValueOrDefault() == 0)
            {
                throw new SituacaoExigeMotivoException();
            }

            if (requisitosSituacao.ExigeJustificativa.GetValueOrDefault() && string.IsNullOrEmpty(justificativa))
            {
                throw new SitucaoExigeJustificativaException();
            }
        }

        /// <summary>
        /// Realiza efetivamente as alterações de situação
        /// </summary>
        private void AlterarSituacoesEmLote(AlteracaoSituacaoVO alterarSituacaoDto,
            long seqTemplateProcessoSGF, long seqSituacaoOrigemSGF, long seqSituacaoDestinoSGF)
        {
            SituacaoEtapaFiltroData filtro = new SituacaoEtapaFiltroData
            {
                SeqTemplateProcesso = seqTemplateProcessoSGF,
                SeqSituacao = seqSituacaoOrigemSGF
            };
            var etapaOrigem = EtapaService.BuscarEtapaPorSituacao(filtro);
            filtro.SeqSituacao = seqSituacaoDestinoSGF;

            var etapaDestino = EtapaService.BuscarEtapaPorSituacao(filtro);
            bool mesmaEtapa = true;
            bool etapaAnterior = false;
            if (etapaDestino != null && seqSituacaoDestinoSGF != 0)
            {
                mesmaEtapa = etapaOrigem != null && etapaDestino.Ordem == etapaOrigem.Ordem;
                etapaAnterior = etapaOrigem != null && etapaOrigem.Ordem > etapaDestino.Ordem;
            }

            var tokenSituacaoDestino = TipoProcessoSituacaoDomainService.SearchProjectionByKey(
                                        new SMCSeqSpecification<TipoProcessoSituacao>(alterarSituacaoDto.SeqTipoProcessoSituacaoDestino),
                                        x => x.Token);

            //Para cada uma das inscriões a terem a situação alterada
            foreach (long seqInscricao in alterarSituacaoDto.SeqInscricoes)
            {
                var includes = IncludesInscricaoHistoricoSituacao.TipoProcessoSituacao
                             | IncludesInscricaoHistoricoSituacao.Inscricao
                             | IncludesInscricaoHistoricoSituacao.Inscricao_Inscrito;

                var situacaoAtualInscricao = BuscarSituacaoAtualInscricao(seqInscricao, includes);

                // Armazenado o seq do processo pois ao dar update entity, limpa o objeto de navegação da propriedade inscricao
                var seqProcessoInscricao = situacaoAtualInscricao.Inscricao.SeqProcesso;

                // Verifica se o usuário está saindo de inscrito para se tornar um candidato
                VerificaAlteracoesSelecao(seqInscricao,
                                                seqTemplateProcessoSGF,
                                                tokenSituacaoDestino,
                                                situacaoAtualInscricao.TipoProcessoSituacao.Token,
                                                situacaoAtualInscricao.Inscricao.SeqProcesso,
                                                situacaoAtualInscricao.TipoProcessoSituacao.SeqTipoProcesso);

                if (situacaoAtualInscricao.TipoProcessoSituacao.SeqSituacao == seqSituacaoDestinoSGF)
                {
                    var situacaoDestinoSGF = this.SituacaoService.BuscarSituacao(seqSituacaoDestinoSGF);
                    throw new SituacaoInscricaoAlteradaIgualAtualException(situacaoDestinoSGF.Descricao, situacaoAtualInscricao.Inscricao.Inscrito.Nome);
                }

                if (situacaoAtualInscricao.TipoProcessoSituacao.SeqSituacao != seqSituacaoOrigemSGF)
                {
                    var situacaoSGF = this.SituacaoService.BuscarSituacao(seqSituacaoOrigemSGF);
                    var situacaoDestinoSGF = this.SituacaoService.BuscarSituacao(seqSituacaoDestinoSGF);
                    throw new SituacaoInscricaoAlteradaAnteriormenteException(situacaoAtualInscricao.Inscricao.Inscrito.Nome,
                                situacaoSGF.Descricao, situacaoDestinoSGF.Descricao);
                }

                if (mesmaEtapa)
                {
                    //Se a etapa de destino for a mesma da etapa de origem
                    //Alterar a situação "atual" antiga para não atual
                    situacaoAtualInscricao.AtualEtapa = false;
                }
                else if (etapaAnterior)
                {
                    //Se a etapa de destino for anterior a etapa de origem
                    situacaoAtualInscricao.AtualEtapa = false;
                    //Buscar a stiuação atual da etapa =etapa de destino
                    var specEtapa = new InscricaoHistoricoSituacaoFilterSpecification
                    {
                        SeqInscricao = seqInscricao,
                        AtualEtapa = true,
                        SeqEtapaSGF = etapaDestino.Seq
                    };
                    var situacaoAnteriorEtapaDestino = this.SearchBySpecification(specEtapa).FirstOrDefault();
                    situacaoAnteriorEtapaDestino.AtualEtapa = false;
                    this.UpdateEntity(situacaoAnteriorEtapaDestino);
                }

                //Caso a etapa de destino seja posterior a de origem, apena mudammos o a Situação Atual
                situacaoAtualInscricao.Atual = false;
                this.UpdateEntity(situacaoAtualInscricao);
                //Criar novo histórico de situação de destino , setada como Atual e Atual da Etapa a qual pertencer
                //Fazer isso independete da mudança de caso
                situacaoAtualInscricao.Seq = 0;
                situacaoAtualInscricao.TipoProcessoSituacao = null;
                situacaoAtualInscricao.SeqTipoProcessoSituacao = alterarSituacaoDto.SeqTipoProcessoSituacaoDestino;
                if (!mesmaEtapa)
                {
                    //Recuperar a etapa de Destino
                    var specEtapaProcesso = new EtapaProcessoFilterSpecification(seqProcessoInscricao)
                    {
                        SeqEtapaSGF = etapaDestino.Seq
                    };
                    situacaoAtualInscricao.SeqEtapaProcesso = this.EtapaProcessoDomainService.SearchProjectionByKey(specEtapaProcesso,
                        x => x.Seq);
                }
                situacaoAtualInscricao.AtualEtapa = true;
                situacaoAtualInscricao.Atual = true;
                situacaoAtualInscricao.SeqMotivoSituacaoSGF = alterarSituacaoDto.SeqMotivoSGF;
                situacaoAtualInscricao.Justificativa = alterarSituacaoDto.Justificativa;
                situacaoAtualInscricao.DataSituacao = DateTime.Now;

                if (alterarSituacaoDto.Seq == 0)
                {
                    this.InsertEntity(situacaoAtualInscricao);
                }
                else
                {
                    situacaoAtualInscricao.Seq = alterarSituacaoDto.Seq;
                    this.UpdateEntity(situacaoAtualInscricao);
                }
            }
        }

        public InscricaoHistoricoSituacao BuscarSituacaoAtualInscricao(long seqInscricao, IncludesInscricaoHistoricoSituacao includes = IncludesInscricaoHistoricoSituacao.Nenhum)
        {
            var filtroSituacao = new InscricaoHistoricoSituacaoFilterSpecification
            {
                Atual = true,
                SeqInscricao = seqInscricao
            };

            return this.SearchBySpecification(filtroSituacao, includes).FirstOrDefault();
        }

        public string BuscarTipoProcessoSituacaoAtual(long seqInscricao)
        {
            var spec = new InscricaoHistoricoSituacaoFilterSpecification() { SeqInscricao = seqInscricao, Atual = true };
            var situacaoAtual = SearchProjectionBySpecification(spec, i => i.TipoProcessoSituacao).FirstOrDefault();

            if (situacaoAtual == null) 
                return string.Empty;

            return situacaoAtual.Token;
        }

        private void VerificaAlteracoesSelecao(long seqInscricao, long seqTemplateProcessoSGF, string tokenSituacaoDestino, string tokenSituacaoAtual, long seqProcesso, long seqTipoProcesso)
        {
            var situacoesTemplateProcesso = TemplateProcessoService.BuscarSituacoesPorTemplateProcesso(seqTemplateProcessoSGF);
            if (seqProcesso == 256)
            {
                SMCLogger.Information($"Inscrição: {seqInscricao}; Situacao Destino: {tokenSituacaoDestino}; Situacao Atual: {tokenSituacaoAtual}; Possui Deferimento {situacoesTemplateProcesso.Contains(TOKENS.SITUACAO_INSCRICAO_DEFERIDA)}");
            }
            if (
                // Verifica se o template de processo não possui inscrição deferida, e se a situação está trocando de finalizada para confirmada
                (!situacoesTemplateProcesso.Contains(TOKENS.SITUACAO_INSCRICAO_DEFERIDA) &&
                  (tokenSituacaoAtual == TOKENS.SITUACAO_INSCRICAO_FINALIZADA || tokenSituacaoAtual == TOKENS.SITUACAO_INSCRICAO_CANCELADA) &&
                  tokenSituacaoDestino == TOKENS.SITUACAO_INSCRICAO_CONFIRMADA
                )
                ||
                // Se o template possui inscrição deferida, verifica se está mudando a situação de confirmada para deferida
                (
                  (tokenSituacaoAtual == TOKENS.SITUACAO_INSCRICAO_CONFIRMADA || tokenSituacaoAtual == TOKENS.SITUACAO_INSCRICAO_CANCELADA) &&
                  tokenSituacaoDestino == TOKENS.SITUACAO_INSCRICAO_DEFERIDA
                )
            )
            {
                CriaRegistroInscricaoOfertaHistoricoSituacao(seqInscricao, seqProcesso, seqTipoProcesso);
            }
            else if (tokenSituacaoDestino == TOKENS.SITUACAO_INSCRICAO_CANCELADA
                        || (tokenSituacaoAtual == TOKENS.SITUACAO_INSCRICAO_DEFERIDA)
                        || !situacoesTemplateProcesso.Contains(TOKENS.SITUACAO_INSCRICAO_DEFERIDA) && (tokenSituacaoAtual == TOKENS.SITUACAO_INSCRICAO_CONFIRMADA))

            {
                // Remove os indicadores de atual da IOHS se a situação do candidato for alterada para cancelada, ou se estiver alterando um candidato que estava deferido ou
                // confirmado sem haver deferimento no template (ambos casos são os últimos possíveis da etapa de inscrição, então se está alterando é para voltar para uma
                // situação anterior, devendo remover a IOHS.)
                InscricaoOfertaHistoricoSituacaoDomainService.CancelarInscricaoOferta(seqInscricao);
            }
        }

        private void CriaRegistroInscricaoOfertaHistoricoSituacao(long seqInscricao, long seqProcesso, long seqTipoProcesso)
        {
            // Cria o registro no histórico da inscrição oferta
            var seqEtapaProcesso = EtapaProcessoDomainService.SearchProjectionByKey(
                        new EtapaProcessoFilterSpecification(seqProcesso) { Token = TOKENS.ETAPA_SELECAO }, x => x.Seq);
            // Verifica se existe uma etapa de seleção para o processo, para evitar que um erro genérico seja exibido para o usuário
            // durante a inscrição.
            if (seqEtapaProcesso == 0)
            {
                throw new SMCApplicationException("Erro ao confirmar a inscrição. Etapa de seleção não encontrada.");
            }

            var situacaoCandidatoConfirmado = TipoProcessoSituacaoDomainService.SearchProjectionByKey(new TipoProcessoSituacaoFilterSpecification
            {
                SeqTipoProcesso = seqTipoProcesso,
                Token = TOKENS.SITUACAO_CANDIDATO_CONFIRMADO
            }, x => x.Seq);

            var inscricao = this.InscricaoDomainService.SearchByKey(
                                new SMCSeqSpecification<Inscricao>(seqInscricao), x => x.Ofertas);
            if (seqProcesso == 256)
            {
                SMCLogger.Information($"Inscrição: {seqInscricao}; Qtd Ofertas: {inscricao.Ofertas?.Count}");
            }
            foreach (var oferta in inscricao.Ofertas)
            {
                var iohs = new InscricaoOfertaHistoricoSituacao()
                {
                    SeqTipoProcessoSituacao = situacaoCandidatoConfirmado,
                    SeqEtapaProcesso = seqEtapaProcesso,
                    SeqInscricaoOferta = oferta.Seq,
                    DataSituacao = DateTime.Now,
                    Atual = true,
                    AtualEtapa = true
                };
                InscricaoOfertaHistoricoSituacaoDomainService.SaveEntity(iohs);
            }
        }

        /// <summary>
        /// Desfaz a situação das inscrições segundo a regra de negócio
        /// </summary>
        private void DesfazerSituacaoInscricoes(List<long> seqInscricoes, long seqTemplateProcessoSGF, long seqSituacaoAtualSGF)
        {
            long seqInscricao = seqInscricoes.FirstOrDefault();
            var seqInscricaoSpec = new SMCSeqSpecification<Inscricao>(seqInscricao);

            //Implementar regras de atingir o número máximo de inscrições em um processo
            if (InscricaoDomainService.SearchProjectionByKey(seqInscricaoSpec,
                x => x.HistoricosSituacao.Any(h => h.Atual
                    && h.TipoProcessoSituacao.Token == TOKENS.SITUACAO_INSCRICAO_CANCELADA)))
            {
                var containsSpec = new SMCContainsSpecification<Inscricao, long>(x => x.Seq, seqInscricoes.ToArray());
                var contadorInscricoesNaoCanceladas =
                    InscricaoDomainService.SearchProjectionBySpecification(containsSpec, x => x.Inscrito.Inscricoes
                    .Count(z => z.SeqProcesso == x.SeqProcesso
                        && z.HistoricosSituacao.Any(h => h.Atual
                            && h.TipoProcessoSituacao.Token != TOKENS.SITUACAO_INSCRICAO_CANCELADA))).ToList();

                var numMaxProcesso = InscricaoDomainService.SearchProjectionByKey(seqInscricaoSpec, x => x.Processo.MaximoInscricoesPorInscrito);
                if (numMaxProcesso > 0 && contadorInscricoesNaoCanceladas.Any(x => x >= numMaxProcesso))
                {
                    throw new DesfazerSituacaoMaximoExcedidoException();
                }

                ValidarFormularioSeminario(seqInscricoes, seqSituacaoAtualSGF);
            }

            //Recuperar o Seq do Tipo de Processo para encontrar a situação no banco do GPI
            var seqTipoProcesso =
                InscricaoDomainService.SearchProjectionByKey(seqInscricaoSpec, x => x.Processo.SeqTipoProcesso);
            var seqSituacoesOrigemPossiveisSGF = this.SituacaoService.BuscarSituacoesOrigem(seqSituacaoAtualSGF)
                .Select(x => x.Seq).ToArray();
            var tipoProcessoOrigemContainsSpec = new SMCContainsSpecification<TipoProcessoSituacao, long>(x => x.SeqSituacao,
                 seqSituacoesOrigemPossiveisSGF);
            var seqTipoProcesoSituacaoOrigem = this.TipoProcessoSituacaoDomainService
                .SearchProjectionBySpecification(tipoProcessoOrigemContainsSpec, x => x.Seq).ToList();
            foreach (var seq in seqInscricoes)
            {
                //Recuperar a situação atual da inscrição
                seqInscricaoSpec = new SMCSeqSpecification<Inscricao>(seq);
                var historico = InscricaoDomainService
                    .SearchProjectionByKey(seqInscricaoSpec, x => x.HistoricosSituacao).OrderByDescending(s => s.DataSituacao)
                    .ToList();
                //Recuperar a situação anterior
                var anterior = historico.FirstOrDefault(x => seqTipoProcesoSituacaoOrigem.Any(t => t == x.SeqTipoProcessoSituacao));

                if (anterior == null) { throw new InscricaoHistoricoSituacaoSemHistoricoAnteriorException(); }

                var tipoProcessoSituacaoDestino = TipoProcessoSituacaoDomainService.SearchByKey(
                    new SMCSeqSpecification<TipoProcessoSituacao>(anterior.SeqTipoProcessoSituacao));

                long seqSituacaoDestinoSGF = tipoProcessoSituacaoDestino.SeqSituacao;
                AlterarSituacoesEmLote(new AlteracaoSituacaoVO()
                {
                    SeqTipoProcessoSituacaoDestino = tipoProcessoSituacaoDestino.Seq,
                    SeqInscricoes = new List<long> { seq }
                }, seqTemplateProcessoSGF, seqSituacaoAtualSGF, seqSituacaoDestinoSGF);
            }
        }

        /// <summary>
        /// Se o tipo de processo em questão estiver configurado para integrar com o GPC
        /// e existir o campo PROJETO no formulário, verificar se existe alguma outra 
        /// inscrição para o processo em questão, com a situação atual da inscrição 
        /// igual a INSCRICAO_FINALIZADA ou INSCRICAO_CONFIRMADA, e com o mesmo projeto 
        /// selecionado. Em caso afirmativo, abortar a operação e emitir a mensagem de erro:
        ///"Já existe uma inscrição para o projeto "<nome do projeto>".
        /// </summary>
        /// <param name="seqsIncricoes">Sequenciais de incricao</param>
        private void ValidarFormularioSeminario(List<long> seqsIncricoes, long seqSituacaoAtualSGF)
        {
            var dadosInscricao = this.InscricaoDomainService.SearchProjectionByKey(seqsIncricoes.FirstOrDefault(), p => new
            {
                p.SeqProcesso,
                p.Processo.TipoProcesso.IntegraGPC
            });

            if (dadosInscricao.IntegraGPC)
            {

                //Validar se todas as inscrições estão canceladas, uma vez que foi feita em lote
                //Desta forma se houver mais que uma inscrição sendo enviada para validação e todas estiverem canceladas não poderemos retornar
                //para situação anterior pois infrige a regra que somente uma pode estar confirmada ou finalizada
                if (seqsIncricoes.Count > 1)
                {
                    var spec = new InscricaoHistoricoSituacaoFilterSpecification()
                    {
                        SeqsInscricoes = seqsIncricoes,
                        Atual = true,
                        TokenTipoProcessoSituacao = TOKENS.SITUACAO_INSCRICAO_CANCELADA
                    };

                    //verifica se todas a inscrições que vieram estão canceladas
                    var inscricoesCanceladas = SearchBySpecification(spec).ToList();

                    //Valida se o mesmo numero de inscrições é o mesmo numero de inscrições canceldas
                    if (seqsIncricoes.Count == inscricoesCanceladas.Count)
                    {
                        var inscricoesConfirmadasFinalizadas = 0;
                        //Recuperar lista de inscrições e seus projetos
                        var listaInscricoesProjeto = RecuperarIncricoesProjetos(seqsIncricoes);
                        //Agrupar as inscrições do projeto para poder validar as inscrições do mesmo projeto
                        var projetosInscricoesAgrupados = listaInscricoesProjeto.GroupBy(g => g.SeqProjeto).ToList();

                        foreach (var grupoProjetos in projetosInscricoesAgrupados)
                        {
                            //valida se no projeto de mais que uma inscrição
                            if (grupoProjetos.Count() > 1)
                            {
                                //Percorre todas as inscrições para avaliar se todos as situações anteriores 
                                //Pois caso exista mais que uma com situação anterior commo cancelada ou finalizada 
                                //Não permitirá a alteração
                                foreach (var projeto in grupoProjetos)
                                {
                                    var tokenSituacaoInscricaoAnterior = RecuperarTokenSituacaoAnterior(projeto.SeqInscricao, seqSituacaoAtualSGF, dadosInscricao.SeqProcesso, TOKENS.CAMPO_PROJETO);

                                    if (tokenSituacaoInscricaoAnterior == TOKENS.SITUACAO_INSCRICAO_CONFIRMADA ||
                                        tokenSituacaoInscricaoAnterior == TOKENS.SITUACAO_INSCRICAO_FINALIZADA)
                                    {
                                        inscricoesConfirmadasFinalizadas++;
                                    }
                                }

                                //seleciona uma inscrição deste projeto para buscar o nome do projeto para mensagem de erro
                                var seqInscricaoMensagem = grupoProjetos.Select(s => s.SeqInscricao).FirstOrDefault();

                                //Caso exita mais que uma não poderemos fazer a recupeeração da situação anterior
                                if (inscricoesConfirmadasFinalizadas > 1)
                                {
                                    var descricaoProjeto = RawQuery<string>(string.Format(InscricaoDomainService._query_formulario_gpc_campo_projeo, seqInscricaoMensagem)).FirstOrDefault();
                                    var splitDescricaoProjeto = descricaoProjeto.Split('|');
                                    throw new InscricaoSeminarioInvalidaException(splitDescricaoProjeto[1]);
                                }
                            }
                        }
                    }
                }

                //Caso as situações sejam diferentes para este lote de inscrições irá validar uma a uma para saber se já existe alguém com a situação confirmada no processo
                foreach (var seqIncricao in seqsIncricoes)
                {
                    var descricaoProjeto = RawQuery<string>(string.Format(InscricaoDomainService._query_formulario_gpc_campo_projeo, seqIncricao)).FirstOrDefault();
                    if (!string.IsNullOrEmpty(descricaoProjeto))
                    {
                        InscricaoDomainService.ValidarFormularioSeminario(dadosInscricao.SeqProcesso, seqIncricao, descricaoProjeto);
                    }
                }
            }
        }

        /// <summary>
        /// Recupera o token da situação anterior
        /// </summary>
        /// <param name="seqInscricao">Sequencial da Inscrição</param>
        /// <param name="seqSituacaoAtualSGF">Sequencial da situação atual no SGF</param>
        /// <param name="seqProcesso">Sequencial do processo</param>
        /// <param name="token">Token do elemento</param>
        /// <returns>Token da situação anterior</returns>
        private string RecuperarTokenSituacaoAnterior(long seqInscricao, long seqSituacaoAtualSGF, long seqProcesso, string token)
        {
            var seqInscricaoSpec = new SMCSeqSpecification<Inscricao>(seqInscricao);
            //Recuperar o Seq do Tipo de Processo para encontrar a situação no banco do GPI
            var seqTipoProcesso =
                InscricaoDomainService.SearchProjectionByKey(seqInscricaoSpec, x => x.Processo.SeqTipoProcesso);
            var seqSituacoesOrigemPossiveisSGF = this.SituacaoService.BuscarSituacoesOrigem(seqSituacaoAtualSGF)
                .Select(x => x.Seq).ToArray();
            var tipoProcessoOrigemContainsSpec = new SMCContainsSpecification<TipoProcessoSituacao, long>(x => x.SeqSituacao,
                 seqSituacoesOrigemPossiveisSGF);
            var seqTipoProcesoSituacaoOrigem = this.TipoProcessoSituacaoDomainService
                .SearchProjectionBySpecification(tipoProcessoOrigemContainsSpec, x => x.Seq).ToList();

            var specIncricaoDescricao = new InscricaoFilterSpecification()
            {
                TokenElemento = token,
                SeqInscricao = seqInscricao,
            };
            specIncricaoDescricao.SetOrderByDescending(x => x.DataInscricao);

            var descricaoProjetoGPC = RawQuery<string>(string.Format(InscricaoDomainService._query_formulario_gpc_campo_projeo, seqInscricao)).FirstOrDefault();

            var splitDescricaoProjeto = descricaoProjetoGPC.Split('|');
            string seqProjeto = splitDescricaoProjeto[0];

            var specIncricao = new InscricaoFilterSpecification()
            {
                TokenElemento = token,
                SeqInscricao = seqInscricao,
                SeqProcesso = seqProcesso,
                ValorElemento = seqProjeto
            };
            specIncricao.SetOrderByDescending(x => x.DataInscricao);
            var historico = InscricaoDomainService
                .SearchProjectionByKey(specIncricao, x => x.HistoricosSituacao).OrderByDescending(s => s.DataSituacao)
                .ToList();

            //Recuperar a situação anterior
            var anterior = historico.FirstOrDefault(x => seqTipoProcesoSituacaoOrigem.Any(t => t == x.SeqTipoProcessoSituacao));

            if (anterior == null) { throw new InscricaoHistoricoSituacaoSemHistoricoAnteriorException(); }

            var tipoProcessoSituacaoDestino = TipoProcessoSituacaoDomainService.SearchByKey(
                new SMCSeqSpecification<TipoProcessoSituacao>(anterior.SeqTipoProcessoSituacao));

            return tipoProcessoSituacaoDestino.Token;
        }

        private List<InscricaoProjetoVO> RecuperarIncricoesProjetos(List<long> seqInscricoes)
        {
            List<InscricaoProjetoVO> retorno = new List<InscricaoProjetoVO>();
            foreach (var seqInscricao in seqInscricoes)
            {
                var descricaoProjetoGPC = RawQuery<string>(string.Format(InscricaoDomainService._query_formulario_gpc_campo_projeo, seqInscricao)).FirstOrDefault();
                if (!string.IsNullOrEmpty(descricaoProjetoGPC))
                {
                    var splitDescricaoProjeto = descricaoProjetoGPC.Split('|');
                    string seqProjeto = splitDescricaoProjeto[0];
                    retorno.Add(new InscricaoProjetoVO() { SeqInscricao = seqInscricao, SeqProjeto = Int64.Parse(seqProjeto) });
                }
            }

            return retorno;
        }
    }
}