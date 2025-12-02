using SMC.Formularios.ServiceContract.Areas.TMP.Interfaces;
using SMC.Framework.Extensions;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.Service;
using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.INS.DomainServices;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SMC.Inscricoes.Service.Areas.INS.Services
{
    public class TipoProcessoService : SMCServiceBase, ITipoProcessoService
    {
        #region DomainService

        private TipoProcessoDomainService TipoProcessoDomainService
        {
            get { return this.Create<TipoProcessoDomainService>(); }
        }

        private TipoProcessoSituacaoDomainService TipoProcessoSituacaoDomainService
        {
            get { return this.Create<TipoProcessoSituacaoDomainService>(); }
        }

        private ITipoTemplateProcessoService TipoTemplateProcessoService
        {
            get { return this.Create<ITipoTemplateProcessoService>(); }
        }

        private TipoProcessoDocumentoDomainService TipoProcessoDocumentoDomainService
        {
            get { return this.Create<TipoProcessoDocumentoDomainService>(); }
        }

        #endregion DomainService

        public List<SMCDatasourceItem> BuscarTiposProcessoKeyValue(long? seqUnidadeResponsavel = null)
        {
            return TipoProcessoDomainService.BuscarTiposProcessoKeyValue(seqUnidadeResponsavel);
        }

        /// <summary>
        /// Busca os tipoProcessoSituacao de destino a partir de um tipo processo situaão de origem
        /// (usado para mudança de situação de inscrições)
        /// </summary>
        public SMCDatasourceItem[] BuscarTipoProcessoSitucaoDestinoKeyValue(long seqTipoProcessoSituacao, long? seqProcesso = null, bool throwWhenEmpty = true)
        {
            return TipoProcessoSituacaoDomainService.BuscarTipoProcessoSitucaoDestinoKeyValue(seqTipoProcessoSituacao, seqProcesso, throwWhenEmpty);
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
        public SMCDatasourceItem[] BuscarTipoProcessoSitucaoDestinoPorToken(long seqProcesso, string tokenSituacao, string tokenEtapa, bool verificaPermiteRetornar = false, bool retornaSituacaoSGF = false)
        {
            return TipoProcessoSituacaoDomainService.BuscarTipoProcessoSitucaoDestinoKeyValue(seqProcesso, tokenSituacao, tokenEtapa, verificaPermiteRetornar, retornaSituacaoSGF);
        }

        /// <summary>
        /// Busca o par chave e valor (Seq e Descriçãod da situação) para o sequencial informado
        /// </summary>
        public SMCDatasourceItem BuscarTipoProcessoSituacaoKeyValue(long seqTipoProcessoSituacao)
        {
            return TipoProcessoSituacaoDomainService.BuscarTipoProcessoSituacaoKeyValue(seqTipoProcessoSituacao);
        }

        public SMCPagerData<TipoProcessoListaData> BuscarTiposProcesso(TipoProcessoFiltroData filtroData)
        {
            var spec = SMCMapperHelper.Create<TipoProcessoFilterSpecification>(filtroData);
            int total = 0;
            var itens = this.TipoProcessoDomainService.SearchProjectionBySpecification(spec,
                        x => new TipoProcessoListaData
                        {
                            Seq = x.Seq,
                            Descricao = x.Descricao
                            
                        }, out total);
            return new SMCPagerData<TipoProcessoListaData>(itens, total);
        }

        public long SalvarTipoProcesso(TipoProcessoData modelo)
        {
            var tipoProcessoDomain = SMCMapperHelper.Create<TipoProcesso>(modelo);
            tipoProcessoDomain.CamposInscrito = new List<TipoProcessoCampoInscrito>();
            if (modelo.CamposInscrito != null)
            {
                foreach (var item in modelo.CamposInscrito)
                {
                    tipoProcessoDomain.CamposInscrito.Add(new TipoProcessoCampoInscrito()
                    {
                        CampoInscrito = (Common.Areas.INS.Enums.CampoInscrito)item,
                        SeqTipoProcesso = tipoProcessoDomain.Seq == 0 ? 0 : tipoProcessoDomain.Seq

                    });
                }
            }

            return TipoProcessoDomainService.Salvar(tipoProcessoDomain);
        }

        public void ExcluirTipoProcesso(long seqTipoProcesso)
        {
            TipoProcessoDomainService.DeleteEntity(seqTipoProcesso);
        }

        public TipoProcessoData BuscarTipoProcesso(long seqTipoProcesso)
        {
            var spec = new TipoProcessoFilterSpecification() { Seq = seqTipoProcesso };

            var tipoProcesso = TipoProcessoDomainService.SearchProjectionBySpecification(spec,
                                //NOTE: É necessário ser um objeto anônimo, pois o Linq To Entity não consegue converter o objeto TipoProcesso
                                x => new
                                {
                                    x.Seq,
                                    x.Descricao,
                                    x.Documentos,
                                    x.SeqTipoTemplateProcessoSGF,
                                    // TODO: Carol - O campo ExibeTermoConsentimentoLGPD não existe mais. Foi substituido por TermoConsentimentoLGPD
                                    // x.ExibeTermoConsentimentoLGPD,
                                    x.ExigeCodigoOrigemOferta,
                                    x.IntegraSGALegado,
                                    x.BolsaExAluno,
                                    x.IsencaoP1,
                                    x.PermiteRegerarTitulo,
                                    x.GestaoEventos,
                                    //NOTE: Ordenação da lista filha de TiposTaxa
                                    TiposTaxa = x.TiposTaxa.OrderBy(f => f.TipoTaxa.Descricao),
                                    x.Situacoes,
                                    x.Templates,
                                    Consistencias = x.Consistencias.OrderBy(f => f.TipoConsistencia.ToString()),
                                    x.IdsTagManager,
                                    x.IntegraGPC,
                                    x.TermoAceiteConversaoPDF,
                                    x.OrientacaoAceiteConversaoPDF,
                                    x.TermoConsentimentoLGPD,
                                    x.ValidaLimiteDesconto,
                                    x.HabilitaPercentualDesconto,
                                    x.SeqContextoBibliotecaGed,
                                    x.SeqHierarquiaClassificacaoGed,
                                    x.TokenResource,
                                    x.HabilitaGed,
                                    x.CamposInscrito,
                                    x.Token
                                }).First();

            var tipoProcessoData = SMCMapperHelper.Create<TipoProcessoData>(tipoProcesso);

            var situacoesSGF = TipoTemplateProcessoService.BuscarSituacaoPorTipoTemplate(tipoProcessoData.SeqTipoTemplateProcessoSGF);

            if (tipoProcessoData.Situacoes != null)
            {
                foreach (var situacao in tipoProcessoData.Situacoes)
                {
                    var situacaosgf = situacoesSGF.Where(f => f.Seq == situacao.SeqSituacao).FirstOrDefault();
                    if (situacaosgf != null)
                    {
                        situacao.SeqSituacao = situacaosgf.Seq;
                        situacao.DescricaoSGF = situacaosgf.Descricao;
                    }
                }
            }


            tipoProcessoData.CamposInscrito = new List<long>();
            foreach (var item in tipoProcesso.CamposInscrito)
            {
                tipoProcessoData.CamposInscrito.Add((long)item.CampoInscrito);
            }

            return tipoProcessoData;
        }

        public List<SMCDatasourceItem> BuscarTemplatesProcessoAssociados(long seqTipoProcesso)
        {
            var tipoProcesso = this.TipoProcessoDomainService.SearchByKey(new SMCSeqSpecification<TipoProcesso>(seqTipoProcesso),
                x => x.Templates);
            var seqTemplates = tipoProcesso.Templates.Select(x => x.SeqTemplateProcessoSGF);
            var templatesSGF = TipoTemplateProcessoService
                .BuscarTemplatesProcessoPorTipoTemplate(tipoProcesso.SeqTipoTemplateProcessoSGF);
            for (int i = 0; i < templatesSGF.Count; i++)
            {
                if (!seqTemplates.Contains(templatesSGF[i].Seq))
                {
                    templatesSGF.RemoveAt(i);
                    i--;
                }
            }
            return templatesSGF.TransformList<SMCDatasourceItem>();
        }

        public List<SMCDatasourceItem> BuscarTiposTaxaAssociados(long seqTipoProcesso)
        {
            return this.TipoProcessoDomainService.SearchProjectionByKey(
                new SMCSeqSpecification<TipoProcesso>(seqTipoProcesso),
                x => x.TiposTaxa.OrderBy(t => t.TipoTaxa.Descricao).Select(t => new SMCDatasourceItem
                {
                    Seq = t.SeqTipoTaxa,
                    Descricao = t.TipoTaxa.Descricao
                })).ToList();
        }

        public TipoProcessoSituacaoData BuscarTipoProcessoSituacao(long seqTipoProcessoSituacao)
        {
            return this.TipoProcessoSituacaoDomainService.SearchByKey<TipoProcessoSituacao, TipoProcessoSituacaoData>(seqTipoProcessoSituacao);
        }

        public bool VerificaPossuiConsistencia(TipoProcessoConsistenciaData filtro)
        {
            var spec = filtro.Transform<TipoProcessoConsistenciaSpecification>();
            var tipoProcesso = TipoProcessoDomainService.SearchByKey(spec);
            return TipoProcessoDomainService.PossuiConsistencia(tipoProcesso, filtro.TipoConsistencia);
        }

        public TipoProcessoSituacaoData BuscarTipoProcessoSituacaoAnterior(long seqInscricao)
        {
            return TipoProcessoSituacaoDomainService.BuscarTipoProcessoSituacaoAnterior(seqInscricao).Transform<TipoProcessoSituacaoData>();
        }

        /// <summary>
        /// Buscar tipo de processo de um processo
        /// </summary>
        /// <param name="seqProcesso">Sequencial Processo</param>
        /// <returns>Tipo de processo</returns>
        public TipoProcessoData BuscarTipoProcessoPorProcesso(long seqProcesso)
        {
            return TipoProcessoDomainService.BuscarTipoProcessoPorProcesso(seqProcesso).Transform<TipoProcessoData>();
        }

        /// <summary>
        /// Busca tipo de processo de uma inscrição
        /// </summary>
        /// <param name="seqInscricao">Sequencial da inscrição</param>
        /// <returns>Tipo de processo</returns>
        public TipoProcessoData BuscarTipoProcessoPorInscricao(long seqIncricao)
        {
            return TipoProcessoDomainService.BuscarTipoProcessoPorInscricao(seqIncricao).Transform<TipoProcessoData>();
        }

        /// <summary>
        /// Verifica se tem integração com o GPC
        /// </summary>
        /// <param name="seqProcesso">Sequencial do processo</param>
        /// <returns></returns>
        public bool VerificaIntegraGPC(long seqProcesso)
        {
            return TipoProcessoDomainService.VerificaIntegraGPC(seqProcesso);
        } 
        
        public List<SMCDatasourceItem> BuscarTiposDocumentoSelect(long seqTipoProcesso)
        {
            return TipoProcessoDocumentoDomainService.BuscarTiposDocumentoSelect(seqTipoProcesso);
        }

        public bool ConferirHabilitaDatasEvento(long seqTipoProcesso)
        {
            return this.TipoProcessoDomainService.ConferirHabilitaDatasEvento(seqTipoProcesso);
        }

        public bool ConferirHabilitaGestaoEvento(long seqTipoProcesso)
        {
            return this.TipoProcessoDomainService.ConferirHabilitaGestaoEvento(seqTipoProcesso);            
        }
    }
}