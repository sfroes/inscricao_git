using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using SMC.Formularios.Common.Areas.FRM.Includes;
using SMC.Formularios.ServiceContract.Areas.FRM.Interfaces;
using SMC.Formularios.ServiceContract.Areas.TMP.Interfaces;
using SMC.Framework;
using SMC.Framework.Domain;
using SMC.Framework.Extensions;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.Security;
using SMC.Framework.Service;
using SMC.Framework.Specification;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using SMC.Inscricoes.Common.Areas.INS.Exceptions;
using SMC.Inscricoes.Domain.Areas.INS.DomainServices;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using SMC.Inscricoes.Domain.Areas.INS.ValueObjects;
using SMC.Inscricoes.Domain.Areas.RES.DomainServices;
using SMC.Inscricoes.Domain.Areas.RES.Specifications;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data.Checkin;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;

namespace SMC.Inscricoes.Service.Areas.INS.Services
{
    public class ProcessoService : SMCServiceBase, IProcessoService
    {
        #region DomainService

        private ConfiguracaoEtapaPaginaIdiomaDomainService ConfiguracaoEtapaPaginaIdiomaDomainService => Create<ConfiguracaoEtapaPaginaIdiomaDomainService>();

        private ConfiguracaoEtapaDomainService ConfiguracaoEtapaDomainService
        {
            get { return this.Create<ConfiguracaoEtapaDomainService>(); }
        }

        private ProcessoDomainService ProcessoDomainService
        {
            get { return this.Create<ProcessoDomainService>(); }
        }

        private TipoDocumentoDomainService TipoDocumentoDomainService
        {
            get { return this.Create<TipoDocumentoDomainService>(); }
        }

        private ProcessoIdiomaDomainService ProcessoIdiomaDomainService
        {
            get { return this.Create<ProcessoIdiomaDomainService>(); }
        }

        private InscritoDomainService InscritoDomainService
        {
            get { return this.Create<InscritoDomainService>(); }
        }

        private TaxaDomainService TaxaDomainService
        {
            get { return Create<TaxaDomainService>(); }
        }

        private UnidadeResponsavelTipoProcessoDomainService UnidadeResponsavelTipoProcessoDomainService
        {
            get { return Create<UnidadeResponsavelTipoProcessoDomainService>(); }
        }

        private UnidadeResponsavelTipoHierarquiaOfertaDomainService UnidadeResponsavelTipoHierarquiaOfertaDomainService
        {
            get { return Create<UnidadeResponsavelTipoHierarquiaOfertaDomainService>(); }
        }

        private TipoProcessoTemplateDomainService TipoProcessoTemplateDomainService
        {
            get { return Create<TipoProcessoTemplateDomainService>(); }
        }

        private UnidadeResponsavelDomainService UnidadeResponsavelDomainService
        {
            get { return Create<UnidadeResponsavelDomainService>(); }
        }

        private ArquivoAnexadoDomainService ArquivoAnexadoDomainService
        {
            get { return Create<ArquivoAnexadoDomainService>(); }
        }

        private ProcessoCampoInscritoDomainService ProcessoCampoInscritoDomainService
        {
            get { return Create<ProcessoCampoInscritoDomainService>(); }
        }

        #endregion DomainService

        #region Services

        private IEtapaService EtapaService
        {
            get { return this.Create<IEtapaService>(); }
        }

        private IFormularioService FormularioService
        {
            get { return this.Create<IFormularioService>(); }
        }

        #endregion Services

        #region Home de Processos

        /// <summary>
        /// Busca os processos que estão com etapa de inscrição em aberto por idioma
        /// </summary>
        /// <param name="filtro">Filtro para pesquisa</param>
        /// <returns>Lista de etapas de inscrição de processos em aberto</returns>
        public SMCPagerData<EtapaProcessoAbertoListaData> BuscarProcessosComInscricoesEmAberto(EtapaProcessoAbertoFiltroData filtro)
        {
            int total;

            // Se não tem filtro pela descrição do processo, pesquisa em cache. Caso contrário faz sempre a busca.
            if (string.IsNullOrEmpty(filtro.DescricaoProcesso))
            {
                //FIX: Cache
                //return GetCacheResult(() =>
                //{
                return BuscarProcessosComInscricoesEmAberto(filtro, out total);
                //}, string.Format("{0}.{1}.{2}.ProcessosComInscricoesEmAberto", filtro.SeqProcesso.HasValue ? filtro.SeqProcesso.Value.ToString() : string.Empty, filtro.Idioma,filtro.PageSettings.PageIndex));
            }
            else
            {
                return BuscarProcessosComInscricoesEmAberto(filtro, out total);
            }
        }

        private SMCPagerData<EtapaProcessoAbertoListaData> BuscarProcessosComInscricoesEmAberto(EtapaProcessoAbertoFiltroData filtro, out int total)
        {
            ConfiguracaoEtapaInscricaoEmAbertoSpecification spec = SMCMapperHelper.Create<ConfiguracaoEtapaInscricaoEmAbertoSpecification>(filtro);
            var seqUsuario = SMCContext.User.SMCGetSequencialUsuario();
            if (!seqUsuario.HasValue)
                throw new UsuarioInvalidoException();

            spec.SeqInscrito = InscritoDomainService.BuscarSeqInscrito(seqUsuario.Value).Value;
            List<EtapaProcessoVO> vos = ConfiguracaoEtapaDomainService.BuscarProcessosComConfiguracaoEtapaInscricaoAberto(spec, out total, filtro.Idioma);
            List<EtapaProcessoAbertoListaData> datas = vos.TransformList<EtapaProcessoAbertoListaData>();
            return new SMCPagerData<EtapaProcessoAbertoListaData>(datas, total);
        }

        /// <summary>
        /// Busca as informações de um processo para exibição na home
        /// </summary>
        /// <param name="uidProcesso">Uid do processo a ser recuperado</param>
        /// <param name="idioma">Idioma para recuperar as informações</param>
        /// <returns>Informações de um processo para exibição na home</returns>
        public ProcessoHomeData BuscarProcessoHome(Guid uidProcesso, SMCLanguage idioma, long? seqInscrito)
        {
            //FIX: Cache
            //return GetCacheResult(() =>
            //{
            ProcessoFilterSpecification spec = new ProcessoFilterSpecification() { UidProcesso = uidProcesso, SeqInscrito = seqInscrito };
            ProcessoHomeVO processo = ProcessoDomainService.BuscarProcessoHome(spec, idioma);
            return SMCMapperHelper.Create<ProcessoHomeData>(processo);
            //}, string.Format("{0}.{1}.HomeProcesso", uidProcesso, idioma));
        }

        public string BuscarCssProcesso(Guid uidProcesso)
        {
            return ProcessoDomainService.BuscarCssProcesso(uidProcesso);
        }

        #endregion Home de Processos

        #region Posição consolidada

        /// <summary>
        /// Busca a posição consolidada para cada processo de acordo com o filtro informado
        /// </summary>
        /// <param name="filtro">Filtro da pesquisa</param>
        /// <returns>Lista de processos com a posição consolidada sumarizada</returns>
        public SMCPagerData<PosicaoConsolidadaProcessoData> BuscarPosicaoConsolidadaProcesso(ProcessoFiltroData filtro)
        {
            var spec = SMCMapperHelper.Create<ProcessoFilterSpecification>(filtro);
            spec.TokenEtapa = new string[] { TOKENS.ETAPA_INSCRICAO };
            var processos = this.ProcessoDomainService.BuscarPosicaoConsolidadaProcesso(spec, out int total);
            return new SMCPagerData<PosicaoConsolidadaProcessoData>(
                processos.TransformList<PosicaoConsolidadaProcessoData>(), total);
        }

        /// <summary>
        /// Busca a posição consolidada de um processo
        /// </summary>
        /// <param name="filtro">Filtro da pesquisa</param>
        /// <returns>Lista de processos com a posição consolidada sumarizada</returns>
        public PosicaoConsolidadaProcessoData BuscarPosicaoConsolidadaProcesso(long seqProcesso)
        {
            int total;
            var posicao = this.ProcessoDomainService.BuscarPosicaoConsolidadaProcesso(
                new ProcessoFilterSpecification { SeqProcesso = seqProcesso }, out total).FirstOrDefault();
            return SMCMapperHelper.Create<PosicaoConsolidadaProcessoData>(posicao);
        }

        #endregion Posição consolidada

        #region Busca dados processo

        /// <summary>
        /// Busca as informações de um processo para um cabeçalho
        /// </summary>
        public ProcessoCabecalhoData BuscarCabecalhoProcesso(long seqProcesso)
        {
            return ProcessoDomainService.SearchByKey<Processo, ProcessoCabecalhoData>(seqProcesso,
                IncludesProcesso.Cliente | IncludesProcesso.TipoProcesso);
        }

        /// <summary>
        /// Buscar as quantidades de oferta do processo
        /// </summary>
        /// <param name="seqConfiguracaoEtapa">Sequencial da configuração de etapa do processo</param>
        /// <returns>Quantidades de oferta do processo</returns>
        public QuantidadeOfertaProcessoData BuscarQuantidadeOfertaProcesso(long seqConfiguracaoEtapa)
        {
            IncludesConfiguracaoEtapa includes = IncludesConfiguracaoEtapa.EtapaProcesso |
                                                 IncludesConfiguracaoEtapa.EtapaProcesso_Processo;
            ConfiguracaoEtapa config = ConfiguracaoEtapaDomainService.SearchByKey(new SMCSeqSpecification<ConfiguracaoEtapa>(seqConfiguracaoEtapa), includes);
            return SMCMapperHelper.Create<QuantidadeOfertaProcessoData>(config);
        }

        public string BuscarDescricaoOfertaProcesso(long seqConfiguracaoEtapa, SMCLanguage idioma)
        {
            return ProcessoIdiomaDomainService.BuscarDescricaoOfertaProcesso(seqConfiguracaoEtapa, idioma);
        }

        /// <summary>
        /// Busca as informações de processo de acordo com os filtros
        /// </summary>
        public SMCPagerData<ProcessoListaData> BuscarProcessos(ProcessoFiltroData filtro)
        {
            var spec = SMCMapperHelper.Create<ProcessoFilterSpecification>(filtro);
            int total = 0;
            var listaData = this.ProcessoDomainService.SearchProjectionBySpecification(spec,
                x => new ProcessoListaData
                {
                    AnoReferencia = x.AnoReferencia,
                    SemestreReferencia = x.SemestreReferencia,
                    Seq = x.Seq,
                    Descricao = x.Descricao,
                    DescricaoTipoProcesso = x.TipoProcesso.Descricao
                }, out total);
            return new SMCPagerData<ProcessoListaData>(listaData, total);
        }

        /// <summary>
        /// Busca um processo completo para edição
        /// </summary>
        public ProcessoData BuscarProcesso(long seqProcesso)
        {
            var retorno = this.ProcessoDomainService.SearchByKey<Processo, ProcessoData>(seqProcesso,
                IncludesProcesso.Taxas | IncludesProcesso.Taxas_TipoTaxa | IncludesProcesso.Idiomas |
                IncludesProcesso.Telefones | IncludesProcesso.EnderecosEletronicos | IncludesProcesso.TipoProcesso |
                IncludesProcesso.ConfiguracoesModeloDocumento | IncludesProcesso.ConfiguracoesFormulario);

            foreach (var item in retorno.ConfiguracoesModeloDocumento)
            {
                var arquivo = ArquivoAnexadoDomainService.SearchByKey(item.SeqArquivoModelo);
                item.ArquivoModelo = arquivo.Transform<SMCUploadFile>();
            }
            foreach (var item in retorno.ConfiguracoesFormulario)
            {
                var tipoFormulario = FormularioService.BuscarFormulario(item.SeqFormularioSgf, IncludesFormulario.Nenhum);
                item.SeqTipoFormularioSgf = tipoFormulario.SeqTipoFormulario;
            }

            retorno.CamposInscrito = new List<long>();
            var camposInscritos = ProcessoCampoInscritoDomainService.BuscarCamposIncritosPorProcesso(retorno.Seq);
            foreach (var item in camposInscritos)
            {
                retorno.CamposInscrito.Add((long)item.CampoInscrito);
            }

            return retorno;
        }

        public CopiaProcessoData BuscarProcessoCopia(long seqProcesso)
        {
            var spec = new SMCSeqSpecification<Processo>(seqProcesso);
            var processo = this.ProcessoDomainService.SearchProjectionByKey(spec,
                        x => new CopiaProcessoData
                        {
                            SeqProcessoOrigem = seqProcesso,
                            SeqProcessoGpi = x.Seq,
                            Descricao = x.Descricao,
                            DescricaoTipoProcesso = x.TipoProcesso.Descricao,
                            AnoReferencia = x.AnoReferencia,
                            SemestreReferencia = x.SemestreReferencia,
                            TipoHierarquiaOfertaDesativado = x.UnidadeResponsavel.TiposProcesso.Any(
                                t => t.SeqTipoProcesso == x.SeqTipoProcesso && t.TiposHierarquiaOferta.Any(h => x.SeqTipoHierarquiaOferta == h.SeqTipoHierarquiaOferta && !h.Ativo)),
                            TemplateProcessoDesativado =
                                x.TipoProcesso.Templates.Any(t => t.SeqTemplateProcessoSGF == x.SeqTemplateProcessoSGF && !t.Ativo),
                            TipoProcessoDesativado = x.UnidadeResponsavel.TiposProcesso.Any(
                                t => t.SeqTipoProcesso == x.SeqTipoProcesso && !t.Ativo),
                            Etapas = x.EtapasProcesso.Select(f => new CopiaEtapaProcessoData
                            {
                                SeqEtapa = f.Seq,
                                SeqEtapaSGF = f.SeqEtapaSGF,
                                Copiar = false,
                                CopiarConfiguracoes = true
                            }).ToList(),

                            PossuiFormularioConfigurado = x.ConfiguracoesFormulario.Any()
                        });

            var etapasSGF = EtapaService.BuscarEtapasKeyValue(processo.Etapas.Select(f => f.SeqEtapaSGF).ToArray());
            foreach (var etapa in etapasSGF)
            {
                var obj = processo.Etapas.Where(f => f.SeqEtapaSGF == etapa.Seq).FirstOrDefault();
                if (obj != null)
                    obj.Etapa = etapa.Descricao;
            }
            return processo;
        }

        public List<ProcessoTemplateProcessoSGFData> BuscarSeqsTemplatesProcessosSGF(long[] seqsProcessos)
        {
            return ProcessoDomainService.BuscarSeqsTemplatesProcessosSGF(seqsProcessos).TransformList<ProcessoTemplateProcessoSGFData>();
        }

        #endregion Busca dados processo

        #region Situacao processo

        public IEnumerable<SMCDatasourceItem> BuscarSituacoesProcessoKeyValue(long seqProcesso)
        {
            return this.ProcessoDomainService.BuscarSituacoesProcessoKeyValue(seqProcesso);
        }

        /// <summary>
        /// Retorna os TipoProcessoSituação permitidos para uma inscrição no processo e etapas informados
        /// </summary>
        /// <param name="seqProcesso">Sequencial do processo</param>
        /// <returns>Sequencial e descrição dos TipoProcessoSituação</returns>
        public IEnumerable<SMCDatasourceItem> BuscarSituacoesProcessoPorEtapaKeyValue(long seqProcesso, params string[] tokensEtapa)
        {
            return this.ProcessoDomainService.BuscarSituacoesProcessoPorEtapaKeyValue(seqProcesso, tokensEtapa);
        }

        public bool VerificaProcessoPossuiIntegracao(long seqProcesso)
        {
            return ProcessoDomainService.SearchProjectionByKey(new SMCSeqSpecification<Processo>(seqProcesso), x => x.PossuiIntegracao);
        }

        #endregion Situacao processo

        #region Inclusão/Alteração/Exclusão

        public long SalvarProcesso(ProcessoData processo)
        {
            var processoDomain = SMCMapperHelper.Create<Processo>(processo);
            //Mapeia campo inscrito
            processoDomain.CamposInscrito = new List<ProcessoCampoInscrito>();
            if (processo.CamposInscrito != null)
            {
                foreach (var item in processo.CamposInscrito)
                {
                    processoDomain.CamposInscrito.Add(new ProcessoCampoInscrito()
                    {
                        CampoInscrito = (Common.Areas.INS.Enums.CampoInscrito)item,
                        SeqProcesso = processoDomain.Seq == 0 ? 0 : processoDomain.Seq

                    });
                }
            }



            return this.ProcessoDomainService.SalvarProcesso(processoDomain);
        }

        public SMCUploadFile BuscarArquivoAnexadoConfigurancaoEmissaoDocumento(long seqArquivo)
        {
            return this.ProcessoDomainService.BuscarArquivoAnexadoConfigurancaoEmissaoDocumento(seqArquivo);
        }

        public void ExcluirProcesso(long seqProcesso)
        {
            this.ProcessoDomainService.DeleteEntity(seqProcesso);
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        public CopiaProcessoRetornoData CopiarProcesso(CopiaProcessoData copiaProcessoData)
        {
            return this.ProcessoDomainService.CopiarProcesso(copiaProcessoData.Transform<CopiaProcessoVO>()).Transform<CopiaProcessoRetornoData>();
        }

        #endregion Inclusão/Alteração/Exclusão

        /// <summary>
        /// Busca os tipo de item de hierarquia oferta para um processo
        /// </summary>
        public List<SMCDatasourceItem> BuscarTiposItemHierarquiaOfertaKeyValue(long seqProcesso, long? seqPai, bool HabilitaCadastroOferta)
        {
            return ProcessoDomainService
                .BuscarTiposItemHierarquiaOfertaKeyValue(seqProcesso, seqPai, HabilitaCadastroOferta).ToList();
        }

        public IEnumerable<SMCDatasourceItem> BuscarTaxasKeyValue(long seqProcesso)
        {
            return ProcessoDomainService.SearchProjectionByKey(
                new SMCSeqSpecification<Processo>(seqProcesso),
                        x => x.Taxas.Select(t => new SMCDatasourceItem
                        {
                            Seq = t.Seq,
                            Descricao = t.TipoTaxa.Descricao
                        }).OrderBy(o => o.Descricao));
        }

        /// <summary>
        /// Busca o evento financeiro para um processo
        /// </summary>
        public int? BuscarEventoProcesso(long seqProcesso)
        {
            return this.ProcessoDomainService.SearchProjectionByKey(
                new SMCSeqSpecification<Processo>(seqProcesso),
                x => x.SeqEvento);
        }

        public ProcessoLookupData BuscarProcessoLookupSelect(long seq)
        {
            return BuscarProcessoLookup(new ProcessoLookupFiltroData() { Seq = seq }).FirstOrDefault();
        }

        public SMCPagerData<ProcessoLookupData> BuscarProcessoLookup(ProcessoLookupFiltroData filtro)
        {
            var spec = filtro.Transform<ProcessoFilterSpecification>();

            var data = this.ProcessoDomainService.SearchProjectionBySpecification(spec,
                        x => new ProcessoLookupData
                        {
                            Seq = x.Seq,
                            Descricao = x.Descricao,
                            UnidadeResponsavel = x.UnidadeResponsavel.Nome,
                            TipoProcesso = x.TipoProcesso.Descricao,
                            AnoReferencia = x.AnoReferencia,
                            SemestreReferencia = x.SemestreReferencia
                        }, out int total).ToList();

            return new SMCPagerData<ProcessoLookupData>(data, total);
        }

        public long? BuscarUnidadeResponsavelNotificacao(long seqProcesso)
        {
            var spec = new SMCSeqSpecification<Processo>(seqProcesso);

            return ProcessoDomainService.SearchProjectionByKey(spec,
                        x => x.UnidadeResponsavel).SeqUnidadeResponsavelNotificacao;
        }

        /// <summary>
        /// REtorna o sequencial do tipo de template de processo de um determinado processo
        /// </summary>
        public long BuscarTipoTemplateProcesso(long seqProcesso)
        {
            return this.ProcessoDomainService.SearchProjectionByKey(
                new SMCSeqSpecification<Processo>(seqProcesso),
                x => x.SeqTemplateProcessoSGF
            );
        }

        /// <summary>
        /// Verifica se é permitido cadastrar período taxa em lote para um processo
        /// </summary>
        public void VerificarConsistenciaCadastroPeriodoTaxaEmLote(long seqProcesso)
        {
            this.ProcessoDomainService.VerificarConsistenciaCadastroPeriodoTaxaEmLote(seqProcesso);
        }

        /// <summary>
        /// Compara se dois processos possuem algum idioma em comum
        /// </summary>
        /// <returns>
        /// false: se os processos não tiverem NENHUM idioma em comum
        /// true : se os processos tiverem AO MENOS UM idioma em comum
        /// </returns>
        public bool CompararIdiomasProcesso(long seqProcessoDestino, long seqProcessoOrigem)
        {
            return this.ProcessoDomainService.CompararIdiomasProcesso(seqProcessoDestino, seqProcessoOrigem);
        }

        public IEnumerable<TotalInscricoesProcessoData> BuscarTotalInscricaoesProcessos(int rangeDias)
        {
            this.ProcessoDomainService.DisableFilter(FILTERS.UNIDADE_RESPONSAVEL);
            return this.ProcessoDomainService.BuscarTotalInscricaoesProcessos(rangeDias)
                .TransformList<TotalInscricoesProcessoData>();
        }

        public ResumoInscricoesProcessoData BuscarSituacaoInscricoesProcessoSumarizada(long seqProcesso)
        {
            this.ProcessoDomainService.DisableFilter(FILTERS.UNIDADE_RESPONSAVEL);
            return this.ProcessoDomainService.BuscarSituacaoInscricoesProcessoSumarizada(seqProcesso)
                .Transform<ResumoInscricoesProcessoData>();
        }

        public long[] BuscarInscricoesProcesso(long seqProcesso)
        {
            return ProcessoDomainService.SearchProjectionByKey(new SMCSeqSpecification<Processo>(seqProcesso),
                            x => x.Inscricoes.Select(f => f.Seq)).ToArray();
        }

        public bool? VerificaTipoTaxaCobraPorQtdOferta(long seqTipoTaxa)
        {
            return TaxaDomainService.SearchProjectionByKey(new SMCSeqSpecification<Taxa>(seqTipoTaxa),
                                            f => f.TipoCobranca == TipoCobranca.PorQuantidadeOfertas);
        }

        public void IntegracaoProcesso(long seqProcesso, bool possuiIntegracao)
        {
            ProcessoDomainService.IntegracaoProcesso(seqProcesso, possuiIntegracao);
        }

        public bool ExibeArvoreFechada(long seqProcesso)
        {
            return ProcessoDomainService.SearchProjectionByKey(seqProcesso, p => p.ExibeArvoreFechada);
        }

        public IEnumerable<SituacaoCopiaCampanhaProcessoGpiData> BuscarSituacoesCopiaCampanhaProcessoGpi(long[] seqsUnidadesResponsaveis, long[] seqsProcessos)
        {
            var lista = new List<SituacaoCopiaCampanhaProcessoGpiData>();

            var specProcesso = new ProcessoFilterSpecification() { SeqsProcessos = seqsProcessos };

            var seqsTiposProcessos = ProcessoDomainService.SearchProjectionBySpecification(specProcesso, p => p.SeqTipoProcesso, isDistinct: true).ToArray();

            var spec = new ProcessoSituacaoTipoHierarquiaTipoProcessoUnidadeResponsavelSpecification()
            {
                SeqsTiposProcessos = seqsTiposProcessos,
                SeqsUnidadesResponsaveis = seqsUnidadesResponsaveis
            };

            var result = ProcessoDomainService.SearchProjectionBySpecification(spec, p => new
            {
                SeqProcessoSeletivo = p.Seq,
                p.SeqTipoProcesso,
                p.SeqUnidadeResponsavel,
                p.SeqTipoHierarquiaOferta,
                p.SeqTemplateProcessoSGF,
            }).ToList();

            foreach (var item in result)
            {
                var specUnidadeResponsavelTipoProcesso = new UnidadeResponsavelTipoProcessoFilterSpecification() { SeqTipoProcesso = item.SeqTipoProcesso, SeqUnidadeResponsavel = item.SeqUnidadeResponsavel };

                var unidadeResponsavelTipoProcesso = UnidadeResponsavelTipoProcessoDomainService.SearchProjectionByKey(specUnidadeResponsavelTipoProcesso, u => new { u.Seq, u.Ativo });

                var specUnidadeResponsavelTipoHierarquiaOferta = new UnidadeResponsavelTipoHierarquiaOfertaFilterSpecification() { SeqTipoHierarquiaOferta = item.SeqTipoHierarquiaOferta, SeqUnidadeResponsavelTipoProcesso = unidadeResponsavelTipoProcesso.Seq };

                var unidadeResponsavelTipoHierarquiaOferta = UnidadeResponsavelTipoHierarquiaOfertaDomainService.SearchProjectionByKey(specUnidadeResponsavelTipoHierarquiaOferta, u => new { u.Seq, u.Ativo });

                var specTipoProcessoTemplate = new TipoProcessoTemplateFilterSpecification() { SeqTipoProcesso = item.SeqTipoProcesso, SeqTemplateProcessoSGF = item.SeqTemplateProcessoSGF };

                var tipoProcessoTemplate = this.TipoProcessoTemplateDomainService.SearchProjectionByKey(specTipoProcessoTemplate, t => new { t.Seq, t.Ativo });

                lista.Add(new SituacaoCopiaCampanhaProcessoGpiData()
                {
                    SeqProcessoSeletivo = item.SeqProcessoSeletivo,
                    SeqTipoProcesso = item.SeqTipoProcesso,
                    SeqUnidadeResponsavel = item.SeqUnidadeResponsavel,
                    TipoProcessoAtivo = unidadeResponsavelTipoProcesso.Ativo,
                    TipoHierarquiaOfertaAtivo = unidadeResponsavelTipoHierarquiaOferta.Ativo,
                    TipoProcessoTemplateAtivo = tipoProcessoTemplate.Ativo
                });
            }

            return lista;
        }

        public List<long> BuscarSeqsFormulariosDoProcesso(long? seqOferta, long? seqGrupoOferta, long seqProcesso)
        {
            return ConfiguracaoEtapaPaginaIdiomaDomainService.SearchProjectionBySpecification(new ConfiguracaoEtapaPaginaIdiomaFilterSpecification { ComFormulario = true, SeqProcesso = seqProcesso, SeqGrupoOferta = seqGrupoOferta, SeqOferta = seqOferta }, x => x.SeqFormularioSGF.Value).Distinct().ToList();
        }

        public bool VerificarIntegracaoLegado(long seqProcesso)
        {
            return ProcessoDomainService.VerificarIntegracaoLegado(seqProcesso);
        }

        public List<SMCDatasourceItem> BuscarProcessosIntegraGPC(bool integraGPC)
        {
            return ProcessoDomainService.BuscarProcessosIntegraGPC(integraGPC);
        }

        public List<SMCDatasourceItem> BuscarProcessoSelect(ProcessoCandidatoFiltroData filtro)
        {
            return ProcessoDomainService.BuscarProcessoSelect(filtro.Transform<ProcessoSelectFilterSpecification>());
        }

        public List<SMCDatasourceItem<string>> BuscarConfiguracoesAssinaturaGadSelect(long seqUnidadeResponsavel)
        {
            return UnidadeResponsavelDomainService.BuscarConfiguracoesAssinaturaGadSelect(seqUnidadeResponsavel);
        }

        public bool ValidarFormulariosAssert(ProcessoData processo)
        {
            return ProcessoDomainService.ValidarFormulariosAssert(processo.Transform<Processo>());
        }

        public bool ValidarAssertDocumentoEmitido(ProcessoData processo)
        {
            return ProcessoDomainService.ValidarAssertDocumentoEmitido(processo.Transform<Processo>());
        }

        public List<ProcessoGestaoEventoQRCodeData> BuscarProcessoHierarquiaLeituraQRCode()
        {
            return ProcessoDomainService.BuscarProcessoHierarquiaLeituraQRCode().TransformList<ProcessoGestaoEventoQRCodeData>();
        }
        public List<string> BuscarSituacoesProcesso(long seqProcesso)
        {
            return ProcessoDomainService.BuscarSituacoesProcesso(seqProcesso);
        }
        public bool ProcessoPossuiTaxa(long seqProcesso)
        {
            return ProcessoDomainService.ProcessoPossuiTaxa(seqProcesso);

        }

        #region Home do Processo Inscrito

        public bool ValidarPermissaoEmitirDocumentacao(long seqProcesso, long seqInscricao, long seqInscrito, long seqTipoDocumento)
        {
            return ProcessoDomainService.ValidarPermissaoEmitirDocumentacao(seqProcesso, seqInscricao, seqInscrito, seqTipoDocumento);
        }

        public bool ValidarUsuarioInscritoAssociadoInscricao(long seqInscricao, long seqInscrito)
        {
            return ProcessoDomainService.ValidarUsuarioInscritoAssociadoInscricao(seqInscricao, seqInscrito);
        }
        
        public long BuscarSeqProcessoPorSeqInscricao(long seqInscricao)
        {
            return ProcessoDomainService.BuscarSeqProcessoPorSeqInscricao(seqInscricao);
        }

        #endregion Home do Processo Inscrito

    }
}