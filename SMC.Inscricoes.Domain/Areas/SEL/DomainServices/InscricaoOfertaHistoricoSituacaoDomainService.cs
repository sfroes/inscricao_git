using SMC.Formularios.ServiceContract.Areas.TMP.Interfaces;
using SMC.Formularios.ServiceContract.TMP.Data;
using SMC.Framework.Domain;
using SMC.Framework.Model;
using SMC.Framework.Specification;
using SMC.Framework.UnitOfWork;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Areas.INS.Exceptions;
using SMC.Inscricoes.Common.Areas.SEL.Exceptions;
using SMC.Inscricoes.Domain.Areas.INS.DomainServices;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using SMC.Inscricoes.Domain.Areas.INS.ValueObjects;
using SMC.Inscricoes.Domain.Areas.SEL.Models;
using SMC.Inscricoes.Domain.Areas.SEL.Specifications;
using SMC.Inscricoes.Domain.Areas.SEL.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SMC.Inscricoes.Domain.Areas.SEL.DomainServices
{
    public class InscricaoOfertaHistoricoSituacaoDomainService : InscricaoContextDomain<InscricaoOfertaHistoricoSituacao>
    {
        #region DomainServices

        private TipoProcessoSituacaoDomainService TipoProcessoSituacaoDomainService
        {
            get { return Create<TipoProcessoSituacaoDomainService>(); }
        }

        private InscricaoDomainService InscricaoDomainService
        {
            get { return Create<InscricaoDomainService>(); }
        }

        private ProcessoDomainService ProcessoDomainService
        {
            get { return Create<ProcessoDomainService>(); }
        }

        private InscricaoOfertaDomainService InscricaoOfertaDomainService
        {
            get { return Create<InscricaoOfertaDomainService>(); }
        }

        private EtapaProcessoDomainService EtapaProcessoDomainService
        {
            get { return Create<EtapaProcessoDomainService>(); }
        }

        #endregion DomainServices

        #region Services

        private ISituacaoService SituacaoService
        {
            get { return Create<ISituacaoService>(); }
        }

        private IEtapaService EtapaService
        {
            get { return this.Create<IEtapaService>(); }
        }

        #endregion Services

        public List<HistoricoSituacaoVO> BuscarHistoricosSituacao(long seqInscricaoOferta)
        {
            var spec = new InscricaoOfertaHistoricoSituacaoSpecification() { SeqInscricaoOferta = seqInscricaoOferta };
            spec.SetOrderBy(o => o.DataSituacao);
            var historico = SearchProjectionBySpecification(spec,
                                                    x => new HistoricoSituacaoVO
                                                    {
                                                        Seq = x.Seq,
                                                        Situacao = x.TipoProcessoSituacao.Descricao,
                                                        Data = x.DataSituacao,
                                                        Responsavel = x.UsuarioInclusao,
                                                        SeqMotivo = x.SeqMotivoSituacao,
                                                        Justificativa = x.Justificativa
                                                    }).ToList();

            SMCDatasourceItem[] motivos = null;
            var historicoComMotivos = historico.Where(f => f.SeqMotivo.HasValue);
            if (historicoComMotivos.Any())
                motivos = SituacaoService.BuscarDescricaoMotivos(historicoComMotivos.Select(f => f.SeqMotivo.Value).ToArray());

            foreach (var situacao in historico)
            {
                situacao.SeqInscricaoOferta = seqInscricaoOferta;

                if (!string.IsNullOrWhiteSpace(situacao.Responsavel))
                {
                    situacao.Responsavel = situacao.Responsavel.Split('/').Last();
                }

                if (situacao.SeqMotivo.HasValue)
                {
                    situacao.Motivo = motivos.First(f => f.Seq == situacao.SeqMotivo.Value).Descricao;
                }
            }

            return historico;
        }

        /// <summary>
        /// Buscar o historico da inscrição mais atual
        /// </summary>
        /// <param name="seqInscricaoOferta">Sequencial incrição oferta</param>
        /// <returns>Retorna os dados da incrição</returns>
        public InscricaoOfertaHistoricoSituacaoVO BuscarHistoricosSituacaoAtual(long seqInscricaoOferta)
        {
            var spec = new InscricaoOfertaHistoricoSituacaoSpecification() { SeqInscricaoOferta = seqInscricaoOferta, Atual = true };
            spec.SetOrderBy(o => o.DataSituacao);
            var historico = SearchProjectionBySpecification(spec,
                                                    x => new InscricaoOfertaHistoricoSituacaoVO
                                                    {
                                                        Seq = x.Seq,
                                                        SeqMotivoSituacao = x.SeqMotivoSituacao,
                                                        SeqSituacao = x.TipoProcessoSituacao.SeqSituacao,
                                                        Atual = x.Atual,
                                                        Justificativa = x.Justificativa,
                                                        SeqInscricao = x.InscricaoOferta.SeqInscricao,
                                                        TokenSituacao = x.TipoProcessoSituacao.Token
                                                    }).FirstOrDefault();
            return historico;
        }

        public HistoricoSituacaoVO BuscarHistoricoSituacao(long seqHistoricoSituacao)
        {
            var historicoSituacao = SearchProjectionByKey(new SMCSeqSpecification<InscricaoOfertaHistoricoSituacao>(seqHistoricoSituacao),
                                                            x => new HistoricoSituacaoVO
                                                            {
                                                                Seq = x.Seq,
                                                                SeqSituacao = x.TipoProcessoSituacao.SeqSituacao,
                                                                Situacao = x.TipoProcessoSituacao.Descricao,
                                                                Data = x.DataSituacao,
                                                                Responsavel = x.UsuarioInclusao,
                                                                SeqMotivo = x.SeqMotivoSituacao,
                                                                Justificativa = x.Justificativa
                                                            });
            if (historicoSituacao.SeqMotivo.HasValue)
            {
                var motivos = SituacaoService.BuscarDescricaoMotivos(new long[] { historicoSituacao.SeqMotivo.Value });
                historicoSituacao.Motivo = motivos.First().Descricao;

                if (!string.IsNullOrWhiteSpace(historicoSituacao.Responsavel))
                {
                    historicoSituacao.Responsavel = historicoSituacao.Responsavel.Split('/').Last();
                }
            }

            return historicoSituacao;
        }

        public long SalvarAlteracaoSituacao(HistoricoSituacaoVO historicoSituacaoVO)
        {
            var historicoSituacao = SearchByKey(new SMCSeqSpecification<InscricaoOfertaHistoricoSituacao>(historicoSituacaoVO.Seq),
                                                    x => x.TipoProcessoSituacao);
            VerificaRequisitosSituacao(historicoSituacaoVO.SeqSituacao, historicoSituacaoVO.SeqMotivo, historicoSituacaoVO.Justificativa);

            historicoSituacao.SeqMotivoSituacao = historicoSituacaoVO.SeqMotivo;
            historicoSituacao.Justificativa = historicoSituacaoVO.Justificativa;
            SaveEntity(historicoSituacao);

            return historicoSituacao.Seq;
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
        /// Apaga os indicadores de atual e atual etapa de todas as inscricoes_oferta_historico_situacao de uma inscrição.
        /// </summary>
        /// <param name="seqInscricao">Id da inscrição.</param>
        public void CancelarInscricaoOferta(long seqInscricao)
        {
            var inscricao = InscricaoDomainService.SearchProjectionByKey(new SMCSeqSpecification<Inscricao>(seqInscricao),
                                                    x => new
                                                    {
                                                        HistoricosSituacoes = x.Ofertas.SelectMany(f => f.HistoricosSituacao.Where(w => w.Atual))
                                                    });

            foreach (var historicoSituacao in inscricao.HistoricosSituacoes)
            {
                historicoSituacao.Atual = false;
                historicoSituacao.AtualEtapa = false;
                SaveEntity(historicoSituacao);
            }
        }

        public void AlterarHistoricoSituacao(AlterarHistoricoSituacaoVO alteracaoSituacaoVO)
        {
            if (!string.IsNullOrEmpty(alteracaoSituacaoVO.TokenSituacaoDestino))
            {
                var spec = new TipoProcessoSituacaoFilterSpecification() { SeqProcesso = alteracaoSituacaoVO.SeqProcesso, Token = alteracaoSituacaoVO.TokenSituacaoDestino };
                alteracaoSituacaoVO.SeqTipoProcessoSituacaoDestino = TipoProcessoSituacaoDomainService.SearchProjectionByKey(spec, p => p.Seq);
            }

            var seqSituacaoDestinoSGF = TipoProcessoSituacaoDomainService.SearchProjectionByKey(
                                            new SMCSeqSpecification<TipoProcessoSituacao>(alteracaoSituacaoVO.SeqTipoProcessoSituacaoDestino),
                                            x => x.SeqSituacao);
            VerificaRequisitosSituacao(seqSituacaoDestinoSGF, alteracaoSituacaoVO.SeqMotivoSGF, alteracaoSituacaoVO.Justificativa);

            var processo = ProcessoDomainService.SearchProjectionByKey(new SMCSeqSpecification<Processo>(alteracaoSituacaoVO.SeqProcesso),
                                                                        x => new
                                                                        {
                                                                            x.SeqTemplateProcessoSGF,
                                                                            x.SeqTipoProcesso,
                                                                            x.TipoProcesso.Situacoes
                                                                        });
            var seqTipoProcessoSituacaoDestinoLote = alteracaoSituacaoVO.SeqTipoProcessoSituacaoDestino;

            using (var unitOfWork = SMCUnitOfWork.Begin())
            {
                try
                {
                    foreach (var seqInscricaoOferta in alteracaoSituacaoVO.SeqInscricoesOferta)
                    {
                        var inscricao = InscricaoOfertaDomainService.SearchProjectionByKey(new SMCSeqSpecification<InscricaoOferta>(seqInscricaoOferta),
                                                x => new
                                                {
                                                    HistoricoSituacaoAtual = x.HistoricosSituacao.FirstOrDefault(f => f.Atual),
                                                    SeqSituacaoAtual = x.HistoricosSituacao.FirstOrDefault(f => f.Atual).TipoProcessoSituacao.SeqSituacao,
                                                    TokenSituacao = x.HistoricosSituacao.FirstOrDefault(f => f.Atual).TipoProcessoSituacao.Token,
                                                    Exportado = x.Exportado
                                                });
                        long seqSituacaoDestino = -1;

                        // Se estiver voltando uma situação
                        if (seqTipoProcessoSituacaoDestinoLote == -1)
                        {
                            // Se o indicador de exportado da inscrição-oferta for "Sim", e a situação atual
                            // do candidato for "CONVOCADO", abortar a operação e emitir a mensagem de erro:
                            // "A convocação não pode ser desfeita pois o candidato já foi exportado."
                            if (inscricao.TokenSituacao == TOKENS.SITUACAO_CONVOCADO && inscricao.Exportado.GetValueOrDefault())
                                throw new ConvocacaoNaoPodeSerDesfeitaException();

                            // Buscar os dados da situação anterior
                            var historicoAnterior = this.BuscarHistoricoDestinoAnterior(seqInscricaoOferta, inscricao.SeqSituacaoAtual, processo.SeqTipoProcesso);

                            alteracaoSituacaoVO.SeqTipoProcessoSituacaoDestino = historicoAnterior.SeqTipoProcessoSituacao;
                            ///Verifica se a justificativa veio preenchida caso contrario preenche com a que estava no registro que esta retrocedendo
                            alteracaoSituacaoVO.Justificativa = string.IsNullOrEmpty(alteracaoSituacaoVO.Justificativa) ? historicoAnterior.Justificativa : alteracaoSituacaoVO.Justificativa;
                            alteracaoSituacaoVO.SeqMotivoSGF = historicoAnterior.SeqMotivoSituacao;
                            seqSituacaoDestino = TipoProcessoSituacaoDomainService.SearchProjectionByKey(historicoAnterior.SeqTipoProcessoSituacao, p => p.SeqSituacao);
                        }
                        else
                        {
                            seqSituacaoDestino = TipoProcessoSituacaoDomainService.SearchProjectionByKey(seqTipoProcessoSituacaoDestinoLote, p => p.SeqSituacao);
                        }

                        var etapaOrigem = EtapaService.BuscarEtapaPorSituacao(new SituacaoEtapaFiltroData
                        {
                            SeqTemplateProcesso = processo.SeqTemplateProcessoSGF,
                            SeqSituacao = inscricao.SeqSituacaoAtual
                        });
                        var etapaDestino = EtapaService.BuscarEtapaPorSituacao(new SituacaoEtapaFiltroData
                        {
                            SeqTemplateProcesso = processo.SeqTemplateProcessoSGF,
                            SeqSituacao = seqSituacaoDestino
                        });

                        bool mesmaEtapa = etapaDestino.Ordem == etapaOrigem.Ordem;
                        bool etapaAnterior = etapaOrigem.Ordem > etapaDestino.Ordem;

                        if (mesmaEtapa)
                        {
                            //Se a etapa de destino for a mesma da etapa de origem
                            //Alterar a situação "atual" antiga para não atual
                            inscricao.HistoricoSituacaoAtual.AtualEtapa = false;
                        }
                        else if (etapaAnterior)
                        {
                            //Se a etapa de destino for anterior a etapa de origem
                            inscricao.HistoricoSituacaoAtual.AtualEtapa = false;
                            //Buscar a stiuação atual da etapa =etapa de destino
                            var specEtapa = new InscricaoOfertaHistoricoSituacaoFilterSpecification
                            {
                                SeqInscricaoOferta = seqInscricaoOferta,
                                AtualEtapa = true,
                                SeqEtapaSGF = etapaDestino.Seq
                            };
                            var situacaoAnteriorEtapaDestino = this.SearchBySpecification(specEtapa).FirstOrDefault();
                            situacaoAnteriorEtapaDestino.AtualEtapa = false;
                            this.UpdateEntity(situacaoAnteriorEtapaDestino);
                        }

                        //Caso a etapa de destino seja posterior a de origem, apena mudammos o a Situação Atual
                        inscricao.HistoricoSituacaoAtual.Atual = false;
                        this.UpdateEntity(inscricao.HistoricoSituacaoAtual);

                        //Criar novo histórico de situação de destino , setada como Atual e Atual da Etapa a qual pertencer
                        //Fazer isso independete da mudança de caso
                        CriaNovaInscricaoOfertaHistoricoSituacao(alteracaoSituacaoVO, etapaDestino, seqInscricaoOferta);
                    }
                    unitOfWork.Commit();
                }
                catch (Exception e)
                {
                    unitOfWork.Rollback();
                    throw e;
                }
            }
        }

        private void CriaNovaInscricaoOfertaHistoricoSituacao(AlterarHistoricoSituacaoVO alteracaoSituacaoVO, EtapaData etapaDestino, long seqInscricaoOferta)
        {
            var situacaoAtualInscricao = new InscricaoOfertaHistoricoSituacao()
            {
                TipoProcessoSituacao = null,
                SeqTipoProcessoSituacao = alteracaoSituacaoVO.SeqTipoProcessoSituacaoDestino,
                AtualEtapa = true,
                Atual = true,
                SeqMotivoSituacao = alteracaoSituacaoVO.SeqMotivoSGF,
                Justificativa = alteracaoSituacaoVO.Justificativa,
                DataSituacao = DateTime.Now,
                SeqInscricaoOferta = seqInscricaoOferta
            };

            //Recuperar a etapa de Destino
            var specEtapaProcesso = new EtapaProcessoFilterSpecification(alteracaoSituacaoVO.SeqProcesso)
            {
                SeqEtapaSGF = etapaDestino.Seq
            };
            var seqEtapaProcesso = this.EtapaProcessoDomainService.SearchProjectionByKey(specEtapaProcesso, x => x.Seq);
            situacaoAtualInscricao.SeqEtapaProcesso = seqEtapaProcesso;

            SaveEntity(situacaoAtualInscricao);
        }

        private long BuscarSeqSituacaoDestinoAnterior(long seqInscricaoOferta, long seqSituacaoAtual, long seqTipoProcesso)
        {
            var seqSituacoesOrigemPossiveisSGF = this.SituacaoService.BuscarSituacoesOrigem(seqSituacaoAtual)
                .Select(x => x.Seq).ToArray();
            var tipoProcessoOrigemContainsSpec = new SMCContainsSpecification<TipoProcessoSituacao, long>(x => x.SeqSituacao,
                 seqSituacoesOrigemPossiveisSGF);
            var seqTipoProcesoSituacaoOrigem = this.TipoProcessoSituacaoDomainService
                .SearchProjectionBySpecification(tipoProcessoOrigemContainsSpec, x => x.Seq);

            var historico = InscricaoOfertaDomainService.SearchProjectionByKey(new SMCSeqSpecification<InscricaoOferta>(seqInscricaoOferta),
                                    x => x.HistoricosSituacao)
                                .OrderByDescending(s => s.DataSituacao)
                                .ToList();

            var anterior = historico.First(x => seqTipoProcesoSituacaoOrigem.Any(t => t == x.SeqTipoProcessoSituacao));
            var tipoProcessoSituacaoDestino = TipoProcessoSituacaoDomainService.SearchByKey(
                                                new SMCSeqSpecification<TipoProcessoSituacao>(anterior.SeqTipoProcessoSituacao));

            return tipoProcessoSituacaoDestino.SeqSituacao;
        }

        private InscricaoOfertaHistoricoSituacao BuscarHistoricoDestinoAnterior(long seqInscricaoOferta, long seqSituacaoAtual, long seqTipoProcesso)
        {
            var seqSituacoesOrigemPossiveisSGF = this.SituacaoService.BuscarSituacoesOrigem(seqSituacaoAtual)
                .Select(x => x.Seq).ToArray();
            var tipoProcessoOrigemSpec = new TipoProcessoSituacaoFilterSpecification() { SeqTipoProcesso = seqTipoProcesso, SeqsSituacoSGF = seqSituacoesOrigemPossiveisSGF };

            var seqTipoProcesoSituacaoOrigem = this.TipoProcessoSituacaoDomainService
                .SearchProjectionBySpecification(tipoProcessoOrigemSpec, x => x.Seq);

            var historico = InscricaoOfertaDomainService.SearchProjectionByKey(new SMCSeqSpecification<InscricaoOferta>(seqInscricaoOferta),
                                    x => x.HistoricosSituacao)
                                .OrderByDescending(s => s.DataSituacao)
                                .ToList();

            var anterior = historico.First(x => seqTipoProcesoSituacaoOrigem.Any(t => t == x.SeqTipoProcessoSituacao));

            return anterior;
        }
    }
}