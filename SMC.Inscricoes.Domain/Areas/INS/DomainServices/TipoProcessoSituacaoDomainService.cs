using SMC.Formularios.ServiceContract.Areas.TMP.Interfaces;
using SMC.Formularios.ServiceContract.TMP.Data;
using SMC.Framework.Domain;
using SMC.Framework.Extensions;
using SMC.Framework.Model;
using SMC.Framework.Specification;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using SMC.Inscricoes.Common.Areas.INS.Exceptions;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using System.Collections.Generic;
using System.Linq;

namespace SMC.Inscricoes.Domain.Areas.INS.DomainServices
{
    public class TipoProcessoSituacaoDomainService : InscricaoContextDomain<TipoProcessoSituacao>
    {
        #region DomainServices

        private InscricaoDomainService InscricaoDomainService => this.Create<InscricaoDomainService>();

        private TipoProcessoDomainService TipoProcessoDomainService
        {
            get { return this.Create<TipoProcessoDomainService>(); }
        }

        private TipoProcessoTemplateDomainService TipoProcessoTemplateDomainService
        {
            get { return this.Create<TipoProcessoTemplateDomainService>(); }
        }

        private ITipoTemplateProcessoService TipoTemplateProcessoService
        {
            get { return this.Create<ITipoTemplateProcessoService>(); }
        }

        private ProcessoDomainService ProcessoDomainService
        {
            get { return this.Create<ProcessoDomainService>(); }
        }

        #endregion DomainServices

        #region Services

        private ISituacaoService SituacaoService
        {
            get { return this.Create<ISituacaoService>(); }
        }

        #endregion Services

        /// <summary>
        /// Retorna a chave valor, para as situações de um tipo processo (seq da situação e descrião)
        /// </summary>
        /// <param name="seqTipoProcesso"></param>
        /// <returns></returns>
        public IEnumerable<SMCDatasourceItem> BuscarTiposProcessoSituacaoKeyValue(long seqTipoProcesso)
        {
            TipoProcessoSituacaoFilterSpecification spec = new TipoProcessoSituacaoFilterSpecification
            {
                SeqTipoProcesso = seqTipoProcesso
            };
            spec.SetOrderBy(x => x.Descricao);
            var items = this.SearchProjectionBySpecification(spec, x => new SMCDatasourceItem
            {
                Descricao = x.Descricao,
                Seq = x.SeqSituacao
            });
            return items;
        }

        /// <summary>
        /// Retorna os TipoProcessoSituação permitidos para uma inscrição no processo e etapas informados
        /// </summary>
        /// <param name="seqProcesso">Sequencial do processo</param>
        /// <returns>Sequencial e descrição dos TipoProcessoSituação</returns>
        public IEnumerable<SMCDatasourceItem> BuscarTiposProcessoSituacaoPorEtapaKeyValue(long seqTipoProcesso, long seqTemplateProcesso, params string[] tokensEtapa)
        {
            TipoProcessoSituacaoFilterSpecification spec = new TipoProcessoSituacaoFilterSpecification
            {
                SeqTipoProcesso = seqTipoProcesso
            };
            spec.SetOrderBy(x => x.Descricao);
            var items = this.SearchProjectionBySpecification(spec, x => new
            {
                Descricao = x.Descricao,
                Seq = x.Seq,
                SeqSituacaoSgf = x.SeqSituacao
            }).ToList();

            List<SMCDatasourceItem> response = new List<SMCDatasourceItem>();
            List<long> seqsEtapas = new List<long>();
            foreach (var token in tokensEtapa)
            {
                var seqs = SituacaoService.BuscarSeqSituacoesPorTokenEtapa(token, seqTemplateProcesso);
                if (token != null)
                    seqsEtapas.AddRange(seqs);
            }
            foreach (var item in items)
            {
                if (seqsEtapas.Contains(item.SeqSituacaoSgf))
                    response.Add(new SMCDatasourceItem() { Seq = item.Seq, Descricao = item.Descricao });
            }

            return response;
        }

        /// <summary>
        /// Retorna as situações configuradas como destino para a situação informada
        /// no formato chave e valor, retorna apenas os que permitem mudança manual
        /// </summary>
        public SMCDatasourceItem[] BuscarTipoProcessoSitucaoDestinoKeyValue(long seqTipoProcessoSituacao, long? seqProcesso = null, bool throwWhenEmpty = true)
        {
            var tipoProcessoSituacao = this.SearchByKey(new SMCSeqSpecification<TipoProcessoSituacao>(seqTipoProcessoSituacao));
            //Consultar as situações de destino no SGF
            var situacaoAtual = SituacaoService.BuscarSituacao(tipoProcessoSituacao.SeqSituacao);
            var listaSituacoes = new List<SMCDatasourceItem>();
            ///Tratamento para a regra de retorno
            if (situacaoAtual.PermiteRetornar)
            {
                listaSituacoes.Add(new SMCDatasourceItem { Seq = -1, Descricao = "Situação anterior" });
            }
            SituacaoData[] situacoesDestino = null;
            if (seqProcesso.HasValue)
            {
                //recuperar etapas
                var seqEtapas = this.ProcessoDomainService.SearchProjectionByKey(
                    new SMCSeqSpecification<Processo>(seqProcesso.Value),
                    x => x.EtapasProcesso.Where(e => e.SituacaoEtapa == SituacaoEtapa.Liberada && e.Token == TOKENS.ETAPA_INSCRICAO)
                        .Select(e => e.SeqEtapaSGF)).ToArray();
                // Se não houver etapas liberadas, retorna vazio
                if (seqEtapas.Length == 0)
                {
                    return new List<SMCDatasourceItem>().ToArray();
                }
                situacoesDestino = SituacaoService.BuscarSituacoesDestino(tipoProcessoSituacao.SeqSituacao, true, seqEtapas);
            }
            else
            {
                situacoesDestino = SituacaoService.BuscarSituacoesDestino(tipoProcessoSituacao.SeqSituacao, true);
            }
            if (throwWhenEmpty && listaSituacoes.Count == 0 && (situacoesDestino == null || situacoesDestino.Count() == 0))
            {
                throw new SituacaoNaoPossuiDestinosManuaisException(tipoProcessoSituacao.Descricao);
            }
            var seqSitucaoesDestinoSGF = situacoesDestino.Select(x => x.Seq).ToArray();
            var containsSpec = new SMCContainsSpecification<TipoProcessoSituacao, long>(x => x.SeqSituacao,
                seqSitucaoesDestinoSGF);
            var filterSpec = new TipoProcessoSituacaoFilterSpecification { SeqTipoProcesso = tipoProcessoSituacao.SeqTipoProcesso };
            listaSituacoes.AddRange(this.SearchProjectionBySpecification(containsSpec & filterSpec, x =>
                new SMCDatasourceItem
                {
                    Seq = x.Seq,
                    Descricao = x.Descricao,
                }
            ));
            return listaSituacoes.ToArray();
        }

        /// <summary>
        /// Busca as situações destino permitidas para uma inscrição.
        /// </summary>
        /// <param name="seqProcesso">Sequencial do processo</param>
        /// <param name="tokenSituacao">Token da situação atual</param>
        /// <param name="tokenEtapa">Token da etapa</param>
        /// <param name="verificaPermiteRetornar">Flag de permissão para retornar</param>
        /// <param name="retornaSituacaoSGF">Flag para retornar a situação do SGF ou do tipo-processo-situação</param>
        /// <returns>Lista de situações</returns>
        public SMCDatasourceItem[] BuscarTipoProcessoSitucaoDestinoKeyValue(long seqProcesso, string tokenSituacao, string tokenEtapa, bool verificaPermiteRetornar = false, bool retornaSituacaoSGF = false)
        {
            var listaSituacoes = new List<SMCDatasourceItem>();

            // Busca o processo, suas etapas e situações
            long seqEtapaSGF = 0;
            if (!string.IsNullOrEmpty(tokenEtapa))
                seqEtapaSGF = this.ProcessoDomainService.SearchProjectionByKey(new SMCSeqSpecification<Processo>(seqProcesso), x =>
                                                                                x.EtapasProcesso.FirstOrDefault(f => f.Token == tokenEtapa).SeqEtapaSGF);

            return BuscarTipoProcessoSitucaoDestinoKeyValue(seqProcesso, tokenSituacao, seqEtapaSGF, verificaPermiteRetornar, retornaSituacaoSGF);
        }

        public TipoProcessoSituacao BuscarTipoProcessoSituacaoAnterior(long seqInscricao)
        {
            var dadosProcesso = InscricaoDomainService.SearchProjectionByKey(seqInscricao, x => new
            {
                x.Processo.SeqTipoProcesso,
                SeqSituacaoAtualSGF = x.HistoricosSituacao.Where(s => s.Atual).Select(t => t.TipoProcessoSituacao.SeqSituacao).FirstOrDefault(),
                HistoricosSituacao = x.HistoricosSituacao.OrderByDescending(s => s.DataSituacao).Select(h => h.SeqTipoProcessoSituacao)
            });

            var seqSituacoesOrigemPossiveisSGF = this.SituacaoService.BuscarSituacoesOrigem(dadosProcesso.SeqSituacaoAtualSGF).Select(x => x.Seq).ToArray();
            var tipoProcessoOrigemContainsSpec = new SMCContainsSpecification<TipoProcessoSituacao, long>(x => x.SeqSituacao, seqSituacoesOrigemPossiveisSGF);
            var seqTipoProcesoSituacaoOrigem = this.SearchProjectionBySpecification(tipoProcessoOrigemContainsSpec, x => x.Seq).ToList();

            //Recuperar a situação anterior
            long? seqSituacaoAnterior = dadosProcesso.HistoricosSituacao.FirstOrDefault(x => seqTipoProcesoSituacaoOrigem.Any(t => t == x));

            if (!seqSituacaoAnterior.HasValue) { throw new InscricaoHistoricoSituacaoSemHistoricoAnteriorException(); }

            var tipoProcessoSituacaoDestino = this.SearchByKey(seqSituacaoAnterior.Value);
            return tipoProcessoSituacaoDestino;
        }

        /// <summary>
        /// Busca as situações destino permitidas para uma inscrição.
        /// </summary>
        /// <param name="seqProcesso">Sequencial do processo</param>
        /// <param name="tokenSituacao">Token da situação atual</param>
        /// <param name="seqEtapaSGF">Seq da etapa do SGF</param>
        /// <param name="verificaPermiteRetornar">Flag de permissão para retornar</param>
        /// <param name="retornaSituacaoSGF">Flag para retornar a situação do SGF ou do tipo-processo-situação</param>
        /// <returns>Lista de situações</returns>
        public SMCDatasourceItem[] BuscarTipoProcessoSitucaoDestinoKeyValue(long seqProcesso, string tokenSituacao, long seqEtapaSGF, bool verificaPermiteRetornar = false, bool retornaSituacaoSGF = false)
        {
            var listaSituacoes = new List<SMCDatasourceItem>();

            // Busca o processo, suas etapas e situações
            var processo = this.ProcessoDomainService.SearchProjectionByKey(new SMCSeqSpecification<Processo>(seqProcesso),
                                        x => new
                                        {
                                            Situacao = x.TipoProcesso.Situacoes.FirstOrDefault(f => f.Token == tokenSituacao),
                                            SeqEtapa = seqEtapaSGF
                                        });

            // Verifica se o usuário deseja conferir no SGF se a situação permite retornar
            if (verificaPermiteRetornar)
            {
                var situacaoSGF = SituacaoService.BuscarSituacao(processo.Situacao.SeqSituacao);
                if (situacaoSGF.PermiteRetornar)
                {
                    listaSituacoes.Add(new SMCDatasourceItem { Seq = -1, Descricao = "Situação anterior" });
                }
            }

            // Busca as situações de destino
            var seqEtapas = (processo.SeqEtapa > 0) ? new long[] { processo.SeqEtapa } : null;
            SituacaoData[] situacoesDestino = SituacaoService.BuscarSituacoesDestino(processo.Situacao.SeqSituacao, true, seqEtapas: seqEtapas);
            var seqSitucaoesDestinoSGF = situacoesDestino.Select(x => x.Seq).ToArray();
            var containsSpec = new SMCContainsSpecification<TipoProcessoSituacao, long>(x => x.SeqSituacao, seqSitucaoesDestinoSGF);
            var filterSpec = new TipoProcessoSituacaoFilterSpecification { SeqTipoProcesso = processo.Situacao.SeqTipoProcesso };
            listaSituacoes.AddRange(this.SearchProjectionBySpecification(containsSpec & filterSpec, x =>
                new SMCDatasourceItem
                {
                    Seq = !retornaSituacaoSGF ? x.Seq : x.SeqSituacao,
                    Descricao = x.Descricao,
                }
            ));
            return listaSituacoes.ToArray();
        }

        /// <summary>
        /// Busca o par chave e valor (Seq e Descriçãod da situação) para o sequencial informado
        /// </summary>
        public SMCDatasourceItem BuscarTipoProcessoSituacaoKeyValue(long seqTipoProcessoSituacao)
        {
            var spec = new SMCSeqSpecification<TipoProcessoSituacao>(seqTipoProcessoSituacao);
            return this.SearchProjectionByKey(spec,
                s => new SMCDatasourceItem
                {
                    Seq = s.Seq,
                    Descricao = s.Descricao,
                });
        }

        public TipoProcessoSituacao BuscarTipoProcessoSituacao(long seqTipoProcessoSituacao)
        {
            return this.SearchByKey(seqTipoProcessoSituacao);
        }
    }
}