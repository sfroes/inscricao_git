using SMC.Financeiro.ServiceContract.Areas.TXA.Data;
using SMC.Financeiro.ServiceContract.BLT;
using SMC.Formularios.Common.Areas.FRM.Includes;
using SMC.Formularios.Common.Areas.TMP.Enums;
using SMC.Formularios.ServiceContract.Areas.FRM.Interfaces;
using SMC.Formularios.ServiceContract.Areas.TMP.Interfaces;
using SMC.Framework;
using SMC.Framework.Domain;
using SMC.Framework.Extensions;
using SMC.Framework.Model;
using SMC.Framework.Specification;
using SMC.Framework.UnitOfWork;
using SMC.Framework.Util;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using SMC.Inscricoes.Common.Areas.INS.Exceptions;
using SMC.Inscricoes.Common.Enums;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using SMC.Inscricoes.Domain.Areas.INS.ValueObjects;
using SMC.Notificacoes.ServiceContract.Areas.NTF.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SMC.Inscricoes.Domain.Areas.INS.DomainServices
{
    public class ConfiguracaoEtapaDomainService : InscricaoContextDomain<ConfiguracaoEtapa>
    {
        #region DomainServices

        private ProcessoIdiomaDomainService ProcessoIdiomaDomainService
        {
            get { return this.Create<ProcessoIdiomaDomainService>(); }
        }

        private ArquivoAnexadoDomainService ArquivoAnexadoDomainService
        {
            get { return this.Create<ArquivoAnexadoDomainService>(); }
        }

        private ProcessoDomainService ProcessoDomainService
        {
            get { return this.Create<ProcessoDomainService>(); }
        }

        private IPaginaService PaginaService
        {
            get { return this.Create<IPaginaService>(); }
        }

        private ConfiguracaoEtapaPaginaIdiomaDomainService ConfiguracaoEtapaPaginaIdiomaDomainService
        {
            get { return this.Create<ConfiguracaoEtapaPaginaIdiomaDomainService>(); }
        }

        private ConfiguracaoEtapaPaginaDomainService ConfiguracaoEtapaPaginaDomainService
        {
            get { return this.Create<ConfiguracaoEtapaPaginaDomainService>(); }
        }

        private GrupoOfertaDomainService GrupoOfertaDomainService
        {
            get { return this.Create<GrupoOfertaDomainService>(); }
        }

        private EtapaProcessoDomainService EtapaProcessoDomainService
        {
            get { return this.Create<EtapaProcessoDomainService>(); }
        }

        private INotificacaoService NotificacaoService
        {
            get { return this.Create<INotificacaoService>(); }
        }

        private TextoSecaoPaginaDomainService TextoSecaoPaginaDomainService
        {
            get { return this.Create<TextoSecaoPaginaDomainService>(); }
        }

        private GrupoDocumentoRequeridoDomainService GrupoDocumentoRequeridoDomainService
        {
            get { return this.Create<GrupoDocumentoRequeridoDomainService>(); }
        }

        private DocumentoRequeridoDomainService DocumentoRequeridoDomainService
        {
            get { return this.Create<DocumentoRequeridoDomainService>(); }
        }

        private IEtapaService EtapaService
        {
            get { return this.Create<IEtapaService>(); }
        }

        private IFormularioService FormularioService
        {
            get { return this.Create<IFormularioService>(); }
        }

        private ArquivoSecaoPaginaDomainService ArquivoSecaoPaginaDomainService
        {
            get { return this.Create<ArquivoSecaoPaginaDomainService>(); }
        }

        private InscricaoDomainService InscricaoDomainService => Create<InscricaoDomainService>();

        private IIntegracaoFinanceiroService FinanceiroService
        {
            get { return this.Create<IIntegracaoFinanceiroService>(); }
        }

        #endregion DomainServices

        #region Métodos de Busca

        /// <summary>
        /// Busca as informações de uma configuração etapa
        /// </summary>
        /// <param name="seqConfiguracaoEtapa">Sequencial da configuração</param>
        /// <param name="idioma">Idioma para recuperar as informações de página</param>
        /// <returns>Informações de uma configuração etap</returns>
        public ConfiguracaoEtapaInfoVO BuscarInformacoesConfiguracaEtapa(long seqConfiguracaoEtapa, SMCLanguage idioma)
        {
            //FIX : Cache
            //return GetCacheResult(() =>
            //{
            // Busca a configuração
            IncludesConfiguracaoEtapa includes = IncludesConfiguracaoEtapa.EtapaProcesso |
                                                    IncludesConfiguracaoEtapa.EtapaProcesso_Processo |
                                                    IncludesConfiguracaoEtapa.Paginas;
            ConfiguracaoEtapa config = this.SearchByKey(new SMCSeqSpecification<ConfiguracaoEtapa>(seqConfiguracaoEtapa), includes);
            if (config == null)
                throw new ConfiguracaoEtapaInvalidaException();

            // Cria o objeto de retorno
            ConfiguracaoEtapaInfoVO infoConfig = new ConfiguracaoEtapaInfoVO()
            {
                ExigeJustificativaOferta = config.ExigeJustificativaOferta,
                NumeroMaximoOfertaPorInscricao = config.NumeroMaximoOfertaPorInscricao,
                NumeroMaximoConvocacaoPorInscricao = config.NumeroMaximoConvocacaoPorInscricao,
                SeqProcesso = config.EtapaProcesso.SeqProcesso,
                UidProcesso = config.EtapaProcesso.Processo.UidProcesso,
                DescricaoProcesso = config.EtapaProcesso.Processo.Descricao,
                FluxoPaginas = new List<FluxoPaginaVO>()
            };

            // Busca as informações de label por idioma do processo
            ProcessoIdiomaFilterSpecification specProc = new ProcessoIdiomaFilterSpecification(config.EtapaProcesso.SeqProcesso, idioma);
            ProcessoIdioma processo = ProcessoIdiomaDomainService.SearchByKey(specProc);
            if (processo == null)
                throw new ProcessoNaoConfiguradoNoIdiomaException();
            infoConfig.LabelCodigoAutorizacao = processo.LabelCodigoAutorizacao;
            infoConfig.LabelGrupoOferta = processo.LabelGrupoOferta;
            infoConfig.LabelOferta = processo.LabelOferta;

            // Busca o título das páginas no idioma informado
            // Retira da listagem de páginas a de comprovante.
            foreach (var pagina in config.Paginas.Where(p => !p.Token.Equals(TOKENS.PAGINA_COMPROVANTE_INSCRICAO)).OrderBy(x => x.Ordem))
            {
                ConfiguracaoEtapaPaginaIdiomaFilterSpecification spec = new ConfiguracaoEtapaPaginaIdiomaFilterSpecification()
                {
                    SeqConfiguracaoEtapaPagina = pagina.Seq,
                    Idioma = idioma
                };
                ConfiguracaoEtapaPaginaIdioma paginaIdioma = ConfiguracaoEtapaPaginaIdiomaDomainService.SearchByKey(spec);
                infoConfig.FluxoPaginas.Add(new FluxoPaginaVO()
                {
                    SeqConfiguracaoEtapaPagina = pagina.Seq,
                    Ordem = pagina.Ordem,
                    Token = pagina.Token,
                    Titulo = paginaIdioma.Titulo,
                    SeqPaginaIdioma = paginaIdioma.Seq,
                    SeqFormularioSGF = paginaIdioma.SeqFormularioSGF,
                    SeqVisaoSGF = paginaIdioma.SeqVisaoSGF,
                    ExibeComprovanteInscricao = pagina.ExibeComprovanteInscricao.HasValue ? pagina.ExibeComprovanteInscricao.Value : false,
                    ExibeConfirmacaoInscricao = pagina.ExibeConfirmacaoInscricao.HasValue ? pagina.ExibeConfirmacaoInscricao.Value : false
                });
            }

            return infoConfig;
            // }, string.Format("{0}.{1}.ConfiguracaoEtapa", seqConfiguracaoEtapa, idioma));
        }

        /// <summary>
        /// Busca as configurações de etapa de inscrições em aberto por idioma
        /// </summary>
        /// <param name="filtro">Specification para pesquisa</param>
        /// <param name="total">Finalizadas de registros encontrados</param>
        /// <param name="idioma">Idioma para pesquisa</param>
        /// <returns>Lista de EtapaProcessoVO</returns>
        public List<EtapaProcessoVO> BuscarProcessosComConfiguracaoEtapaInscricaoAberto(ConfiguracaoEtapaInscricaoEmAbertoSpecification filtro, out int total, SMCLanguage? idioma = null)
        {
            var numeroPagina = filtro.PageNumber;
            //Feito este ajuste para trazer todos os itens do banco desconsiderando o filtro filtro
            var tamanhoPagina = Int32.MaxValue; //filtro.MaxResults;
            filtro.PageNumber = 1;
            filtro.MaxResults = Int32.MaxValue;
            //filtro.SetOrderByDescending(c => c.DataInicio);
            //filtro.SetOrderBy(c => c.DataFim);
            // FIX: Erro no framework não deixa ordenar pelo nome do grupo de oferta
            // filtro.SetOrderBy(c => c.GruposOferta[0].GrupoOferta.Nome);

            // Filtra as configurações de inscrição que estão abertas
            IncludesConfiguracaoEtapa includes = IncludesConfiguracaoEtapa.EtapaProcesso |
                                                IncludesConfiguracaoEtapa.EtapaProcesso_Processo_Inscricoes |
                                                IncludesConfiguracaoEtapa.EtapaProcesso_Processo_GruposOferta |
                                                IncludesConfiguracaoEtapa.GruposOferta |
                                                IncludesConfiguracaoEtapa.GruposOferta_GrupoOferta |
                                                IncludesConfiguracaoEtapa.EtapaProcesso_Processo_TipoProcesso;

            List<ConfiguracaoEtapa> configs = this.SearchBySpecification(filtro, includes).ToList();

            // Agrupa as configurações encontradas por processo incluindo ordenação
            SMCLanguage idiomaProcesso;
            List<EtapaProcessoVO> lista = configs.Select(c => new EtapaProcessoVO
            {
                SeqProcesso = c.EtapaProcesso.SeqProcesso,
                DescricaoProcesso = c.EtapaProcesso.Processo.Descricao,
                DescricaoComplementarProcesso = this.ProcessoIdiomaDomainService.BuscarDescricaoComplementarProcesso(new ProcessoIdiomaFilterSpecification(c.EtapaProcesso.SeqProcesso, idioma), out idiomaProcesso),
                IdiomasDisponiveis = this.ProcessoIdiomaDomainService.BuscarIdiomasDisponiveis(c.EtapaProcesso.SeqProcesso),
                UrlInformacaoComplementar = this.ProcessoDomainService.BuscarURLComplementar(c.EtapaProcesso.Processo.Seq),
                IdiomaAtual = idiomaProcesso,
                DataInicioEtapa = c.EtapaProcesso.DataInicioEtapa,
                DataFimEtapa = c.EtapaProcesso.DataFimEtapa,
                QuantidadeGrupos = c.EtapaProcesso.Processo.GruposOferta.Count,
                Grupos = new List<GrupoOfertaConfiguracaoEtapaVO>(),
                UidProcesso = c.EtapaProcesso.Processo.UidProcesso,
                TokenResource = c.EtapaProcesso.Processo.TipoProcesso.TokenResource,

            }).Distinct(new EtapaProcessoVOComparer())
              .OrderByDescending(p => p.DataInicioEtapa)
              .ThenBy(p => p.DataFimEtapa)
              .ThenBy(p => p.DescricaoProcesso)
              .ToList();

            total = lista.Count;
            lista = lista.Skip((numeroPagina - 1) * tamanhoPagina).Take(tamanhoPagina).ToList();

            // Cada configuração encontrada, associa os grupos ao processo da processosInscrito de VOs ordenada
            foreach (var config in configs.Where(x => lista.Any(l => l.SeqProcesso == x.EtapaProcesso.SeqProcesso)))
            {
                config.Inscricoes = new List<Inscricao>();
                EtapaProcessoVO processo = lista.Where(p => p.SeqProcesso == config.EtapaProcesso.SeqProcesso).Single();
                foreach (var g in config.GruposOferta)
                {
                    GrupoOfertaConfiguracaoEtapaVO grupo = new GrupoOfertaConfiguracaoEtapaVO()
                    {
                        SeqConfiguracaoEtapa = config.Seq,
                        SeqGrupo = g.SeqGrupoOferta,
                        // Somente exibe o grupo de ofertas caso o processo possua mais de um.
                        NomeGrupo = (processo.QuantidadeGrupos > 1) ? g.GrupoOferta.Nome : "",
                        DataInicioConfiguracaoEtapa = config.DataInicio,
                        DataFimConfiguracaoEtapa = config.DataFim,
                        IdiomaAtual = processo.IdiomaAtual,
                        Inscricoes = config.EtapaProcesso.Processo.Inscricoes.Where(w => w.SeqInscrito == filtro.SeqInscrito).TransformList<InscricaoVO>()
                    };
                    processo.Grupos.Add(grupo);
                }
                processo.Grupos = processo.Grupos.OrderByDescending(g => g.DataInicioConfiguracaoEtapa)
                                                 .ThenBy(g => g.DataFimConfiguracaoEtapa)
                                                 .ThenBy(g => g.NomeGrupo).ToList();
            }
            //Valida se esta vindo da tela index padrão ou da index de processo
            if (filtro.SeqProcesso.HasValue)
            {
                EspecificarBotaoInscrever(filtro.SeqInscrito, lista);
            }
            return lista;
        }

        #endregion Métodos de Busca

        public void ExcluirConfiguracaoEtapa(long seqConfiguracaoEtapa)
        {
            var configuracao = this.SearchByKey(
                new SMCSeqSpecification<ConfiguracaoEtapa>(seqConfiguracaoEtapa),
                x => x.Paginas,
                x => x.Paginas[0].Idiomas,
                x => x.Paginas[0].Idiomas[0].Textos,
                x => x.Paginas[0].Idiomas[0].Arquivos,
                x => x.DocumentosRequeridos,
                x => x.GruposDocumentoRequerido,
                x => x.EtapaProcesso);
            if (configuracao.EtapaProcesso.SituacaoEtapa == SituacaoEtapa.Liberada)
            {
                throw new ExclusaoConfiguracaoEtapaLiberadaException();
            }
            ExcluirConfiguracaoEtapa(configuracao);
        }

        internal void ExcluirConfiguracaoEtapa(ConfiguracaoEtapa configuracao)
        {
            using (var unitOfWork = SMCUnitOfWork.Begin())
            {
                try
                {
                    foreach (var pagina in configuracao.Paginas)
                    {
                        this.ConfiguracaoEtapaPaginaDomainService.ExcluirConfiguracaoEtapaPagina(pagina);
                    }
                    configuracao.Paginas = null;
                    foreach (var grupo in configuracao.GruposDocumentoRequerido)
                    {
                        this.GrupoDocumentoRequeridoDomainService.DeleteEntity(grupo);
                    }
                    configuracao.GruposDocumentoRequerido = null;
                    foreach (var doc in configuracao.DocumentosRequeridos)
                    {
                        this.DocumentoRequeridoDomainService.DeleteEntity(doc);
                    }
                    configuracao.DocumentosRequeridos = null;
                    this.DeleteEntity(configuracao);
                    unitOfWork.Commit();
                }
                catch (Exception)
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Salva uma Configura
        /// </summary>
        /// <param name="configuracaoEtapa"></param>
        /// <returns></returns>
        public long SalvarConfiguracaoEtapa(ConfiguracaoEtapa configuracaoEtapa)
        {
            if (configuracaoEtapa.Seq != default(long))
            {


                //Validações para alteração
                var configuracaoEtapaOld = this
                    .SearchByKey(new SMCSeqSpecification<ConfiguracaoEtapa>(configuracaoEtapa.Seq)
                        , x => x.EtapaProcesso
                        , x => x.GruposOferta);
                if (configuracaoEtapaOld.EtapaProcesso.SituacaoEtapa == SituacaoEtapa.Liberada
                    && (configuracaoEtapa.DataInicio != configuracaoEtapaOld.DataInicio
                    || configuracaoEtapa.DataFim != configuracaoEtapaOld.DataFim
                    || configuracaoEtapa.Nome != configuracaoEtapaOld.Nome
                    || !configuracaoEtapa.GruposOferta.SMCContainsList(configuracaoEtapaOld.GruposOferta, x => x.SeqGrupoOferta)
                    || configuracaoEtapa.GruposOferta.Count != configuracaoEtapaOld.GruposOferta.Count
                    || configuracaoEtapa.DescricaoTermoEntregaDocumentacao != configuracaoEtapaOld.DescricaoTermoEntregaDocumentacao))
                {
                    throw new AlteracaoConfiguracaoEtapaLiberadaException();
                }
                IEnumerable<long> missing;
                var contemListaCompleta = configuracaoEtapa.GruposOferta
                    .SMCContainsList(configuracaoEtapaOld.GruposOferta, x => x.SeqGrupoOferta, out missing);
                if (!contemListaCompleta)
                {
                    foreach (var seqGrupo in missing)
                    {
                        var existeOferta = this.GrupoOfertaDomainService.SearchProjectionByKey(
                            new SMCSeqSpecification<GrupoOferta>(seqGrupo),
                            x => x.Ofertas.Any(o => o.InscricoesOferta.Any()));
                        if (existeOferta)
                        {
                            var nomeGrupo = this.GrupoOfertaDomainService.SearchProjectionByKey(
                                new SMCSeqSpecification<GrupoOferta>(seqGrupo)
                                , x => x.Nome);
                            throw new RemocaoGrupoComOfertaException(nomeGrupo);
                        }
                    }
                }
            }

            //Validações para inserção/alteração
            if (configuracaoEtapa.GruposOferta != null)
            {
                foreach (var seqGrupo in configuracaoEtapa.GruposOferta.Select(x => x.SeqGrupoOferta))
                {
                    var existeOfertaForaPeriodo = this.GrupoOfertaDomainService.SearchProjectionByKey(
                        new SMCSeqSpecification<GrupoOferta>(seqGrupo),
                        x => x.Ofertas.Any(o => o.DataInicio < configuracaoEtapa.DataInicio
                            || o.DataFim > configuracaoEtapa.DataFim));
                    if (existeOfertaForaPeriodo)
                    {
                        var nomeGrupo = this.GrupoOfertaDomainService.SearchProjectionByKey(
                                    new SMCSeqSpecification<GrupoOferta>(seqGrupo)
                                    , x => x.Nome);
                        throw new ConfiguracaoOfertaForaPeriodoException(nomeGrupo);
                    }
                }
            }

            var etapaConfiguracao = this.EtapaProcessoDomainService.SearchByKey(
                new SMCSeqSpecification<EtapaProcesso>(configuracaoEtapa.SeqEtapaProcesso), IncludesEtapaProcesso.Configuracoes_GruposOferta_GrupoOferta);

            if (configuracaoEtapa.DataInicio < etapaConfiguracao.DataInicioEtapa ||
                configuracaoEtapa.DataFim > etapaConfiguracao.DataFimEtapa)
            {
                throw new ConfiguracaoVigenciaForaEtapaException();
            }

            var grupoEmUso = etapaConfiguracao.Configuracoes.SelectMany(
                        f => f.GruposOferta.Where(
                            o => configuracaoEtapa.GruposOferta.Any(
                                x => x.SeqGrupoOferta == o.SeqGrupoOferta && x.Seq != o.Seq)));
            if (grupoEmUso.Any())
            {
                var etapas = this.EtapaService.BuscarEtapasKeyValue(new long[] { etapaConfiguracao.SeqEtapaSGF });
                throw new GrupoJaAssociadoOutraConfiguracaoException(etapas.First().Descricao);
            }

            if (configuracaoEtapa.NumeroMaximoOfertaPorInscricao > 1)
            {
                var integraSGALegado = EtapaProcessoDomainService
                    .SearchProjectionByKey(configuracaoEtapa.SeqEtapaProcesso, p => p.Processo.TipoProcesso.IntegraSGALegado);
                if (integraSGALegado)
                {
                    throw new ConfiguracaoEtapaIntegracaoNumeroOfertasException();
                }
            }
            var seqEvento = EtapaProcessoDomainService
                            .SearchProjectionByKey(configuracaoEtapa.SeqEtapaProcesso, p => p.Processo.SeqEvento);


            if (configuracaoEtapa.NumeroMaximoOfertaPorInscricao != 1 && seqEvento.HasValue)
            {
                // Obtém os grupos de oferta vinculados à configuração da etapa
                var seqsGruposOferta = configuracaoEtapa.GruposOferta.Select(s => s.SeqGrupoOferta).ToList();

                // Busca os eventos de taxa e parâmetros CREI para o evento informado
                var eventoTaxaParametros = this.FinanceiroService.BuscarEnvetoTaxaEParamentroCREIPorSeqEvento(seqEvento.Value);

                // Obtém todas as ofertas associadas aos grupos de oferta
                var grupoOfertas = this.GrupoOfertaDomainService.BuscarOfertaSeqsGrupoOferta(seqsGruposOferta);

                foreach (var grupoOferta in grupoOfertas)
                {                   

                    foreach (var ofertaBase in grupoOferta.Ofertas)
                    {
                        foreach (var taxaBase in ofertaBase.Taxas)
                        {
                            if (taxaBase.Taxa.TipoCobranca == TipoCobranca.PorOferta)
                            {
                                continue;
                            }

                            var valorTaxaBase = eventoTaxaParametros.EventosTaxa
                                .FirstOrDefault(x => x.SeqEventoTaxa == taxaBase.SeqEventoTaxa);

                            var dataVencimentoBase = eventoTaxaParametros.ParametrosCREI
                                .FirstOrDefault(f => f.SeqParametroCREI == taxaBase.SeqParametroCrei);

                            if (valorTaxaBase == null || dataVencimentoBase == null)
                                continue; // Se os dados forem nulos, pula para a próxima iteração

                            // Filtra ofertas com períodos coincidentes
                            var ofertas = grupoOferta.Ofertas
                                .Where(o => o != ofertaBase)
                                .ToList();

                            foreach (var ofertaComparacao in ofertas)
                            {

                                var taxasCoincidentes = ofertaComparacao.Taxas.Where(w => w.DataInicio < taxaBase.DataFim && w.DataFim > taxaBase.DataInicio).ToList();

                                if (!taxasCoincidentes.Any(a => a.Taxa.SeqTipoTaxa == taxaBase.Taxa.SeqTipoTaxa))
                                {
                                    throw new DevePossuirPeriodoTaxaIguaisException();
                                }

                                foreach (var taxaCoincidente in taxasCoincidentes)
                                {
                                    if (!ValidarRegrasDevePossuirPeriodoTaxaIguais(taxaBase, taxaCoincidente, eventoTaxaParametros))
                                    {
                                        throw new DevePossuirPeriodoTaxaIguaisException();
                                    }

                                }
                            }
                        }
                    }
                }
            }
            using (var uniOfWork = SMCUnitOfWork.Begin())
            {
                try
                {
                    if (configuracaoEtapa.Seq == 0)
                    {
                        this.InsertEntity(configuracaoEtapa);
                        CriarPaginasPadraoConfiguracao(configuracaoEtapa.Seq);
                    }
                    else
                    {
                        this.UpdateEntity(configuracaoEtapa);
                    }
                    uniOfWork.Commit();
                }
                catch (Exception)
                {
                    uniOfWork.Rollback();
                    throw;
                }
            }
            return configuracaoEtapa.Seq;
        }


        public bool ValidarRegrasDevePossuirPeriodoTaxaIguais(OfertaPeriodoTaxa periodoTaxa, OfertaPeriodoTaxa taxaComparada, EventoComTaxaEParametrosCREIData eventoTaxaParametros)
        {


            var valorTaxa = eventoTaxaParametros.EventosTaxa
                .FirstOrDefault(x => x.SeqEventoTaxa == taxaComparada.SeqEventoTaxa);

            var dataVencimento = eventoTaxaParametros.ParametrosCREI
                .FirstOrDefault(f => f.SeqParametroCREI == taxaComparada.SeqParametroCrei);

        
            if (periodoTaxa.Taxa.Seq != taxaComparada.Taxa.Seq)
                return true;

            if (valorTaxa == null || dataVencimento == null)
                return true; // Se dados estiverem ausentes, não considerar erro

            return valorTaxa.ValorTaxa == eventoTaxaParametros.EventosTaxa.FirstOrDefault(x => x.SeqEventoTaxa == periodoTaxa.SeqEventoTaxa)?.ValorTaxa &&
                   taxaComparada.NumeroMinimo == periodoTaxa.NumeroMinimo &&
                   taxaComparada.NumeroMaximo == periodoTaxa.NumeroMaximo &&
                   dataVencimento.DataVencimentoTitulo == eventoTaxaParametros.ParametrosCREI.FirstOrDefault(f => f.SeqParametroCREI == periodoTaxa.SeqParametroCrei)?.DataVencimentoTitulo;
        }

        //Pepara os items para a árvore de configuração de etapa Página > Idiomas > Seções
        public List<NoArvoreConfiguracaoEtapaVO> BuscarArvoreConfiguracaoEtapa(long seqConfiguracaoEtapa)
        {
            List<NoArvoreConfiguracaoEtapaVO> ret = new List<NoArvoreConfiguracaoEtapaVO>();
            var specPagina = new ConfiguracaoEtapaPaginaFilterSpecification
            {
                SeqConfiguracaoEtapa = seqConfiguracaoEtapa
            };
            specPagina.SetOrderBy(x => x.Ordem);
            var paginas = this.ConfiguracaoEtapaPaginaDomainService.SearchBySpecification(specPagina,
                x => x.ConfiguracaoEtapa, x => x.ConfiguracaoEtapa.EtapaProcesso);
            int seqNo = 1;
            foreach (var pagina in paginas)
            {
                //Criar nó da página
                var paginaEtapaSGF = this.PaginaService.BuscarPaginaEtapa(pagina.SeqPaginaEtapaSGF);
                var paginaVO = new NoArvoreConfiguracaoEtapaVO
                {
                    SeqConfiguracaoEtapa = pagina.SeqConfiguracaoEtapa,
                    SeqEtapaProcesso = pagina.ConfiguracaoEtapa.SeqEtapaProcesso,
                    SeqProcesso = pagina.ConfiguracaoEtapa.EtapaProcesso.SeqProcesso,
                    SeqItem = pagina.Seq,
                    Tipo = TipoItemPaginaEtapa.Pagina,
                    PaginaExibeFormulario = paginaEtapaSGF.Pagina.ExibeFormulario,
                    PaginaToken = paginaEtapaSGF.Pagina.Token,
                    PaginaPermiteExibicaoOutrasPaginas = paginaEtapaSGF.Pagina.PermiteExibirEmOutraPagina,
                    PaginaPermiteDuplicar = paginaEtapaSGF.PermiteDuplicar,
                    PaginaObrigatoria = paginaEtapaSGF.Obrigatorio,
                    Descricao = paginaEtapaSGF.Pagina.Titulo,
                    Seq = seqNo++
                };
                ret.Add(paginaVO);

                var specIdiomas = new ConfiguracaoEtapaPaginaIdiomaFilterSpecification
                {
                    SeqConfiguracaoEtapaPagina = pagina.Seq
                };

                var idiomasPagina = this.ConfiguracaoEtapaPaginaIdiomaDomainService
                    .SearchBySpecification(specIdiomas, x => x.Textos);

                foreach (var idioma in idiomasPagina)
                {
                    //Criar nó do idioma
                    var idiomaVO = new NoArvoreConfiguracaoEtapaVO
                    {
                        Seq = seqNo++,
                        SeqPai = paginaVO.Seq,
                        SeqItem = idioma.Seq,
                        Descricao = SMCEnumHelper.GetDescription(idioma.Idioma),
                        Tipo = TipoItemPaginaEtapa.Idioma,
                        SeqProcesso = paginaVO.SeqProcesso,
                        SeqConfiguracaoEtapa = paginaVO.SeqConfiguracaoEtapa,
                        SeqEtapaProcesso = paginaVO.SeqEtapaProcesso,
                        PaginaToken = paginaVO.PaginaToken,
                    };
                    ret.Add(idiomaVO);

                    foreach (var secaoTexto in idioma.Textos)
                    {
                        //Criar nó da seção de texto
                        var secaoTextoVO = new NoArvoreConfiguracaoEtapaVO
                        {
                            Seq = seqNo++,
                            SeqPai = idiomaVO.Seq,
                            Tipo = TipoItemPaginaEtapa.Secao,
                            SeqProcesso = paginaVO.SeqProcesso,
                            SeqConfiguracaoEtapa = paginaVO.SeqConfiguracaoEtapa,
                            SeqEtapaProcesso = paginaVO.SeqEtapaProcesso,
                            SeqItem = secaoTexto.Seq,
                            PaginaToken = paginaVO.PaginaToken,
                        };
                        var secaoSGF = this.PaginaService.BuscarSecaoPagina(secaoTexto.SeqSecaoPaginaSGF);
                        secaoTextoVO.Descricao = secaoSGF.Descricao;
                        ret.Add(secaoTextoVO);
                    }
                    var secoesArquivo = this.PaginaService.BuscarSecoesPagina(paginaEtapaSGF.SeqPagina, TipoSecaoPagina.Arquivo);
                    //Buscar secoes de Arquivo no SGF
                    foreach (var secaoArquivo in secoesArquivo)
                    {
                        //Criar nó da seção de arquivo
                        var secaoArquivoVO = new NoArvoreConfiguracaoEtapaVO
                        {
                            Seq = seqNo++,
                            SeqPai = idiomaVO.Seq,
                            Tipo = TipoItemPaginaEtapa.Arquivo,
                            SeqProcesso = paginaVO.SeqProcesso,
                            SeqConfiguracaoEtapa = paginaVO.SeqConfiguracaoEtapa,
                            SeqEtapaProcesso = paginaVO.SeqEtapaProcesso,
                            SeqItem = secaoArquivo.Seq,
                            Descricao = secaoArquivo.Descricao,
                            PaginaToken = paginaVO.PaginaToken,
                        };
                        ret.Add(secaoArquivoVO);
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Cria as páginas para uma configuração de etapa de acordo com o template do SGF
        /// e os idiomas configurados para o processo
        /// </summary>
        public void CriarPaginasPadraoConfiguracao(long seqConfiguracaoEtapa)
        {
            //Recuperar os idiomas do processo e o SeqEtapaSGF da configuracão

            var etapaComIdiomas = this.SearchProjectionByKey(
                new SMCSeqSpecification<ConfiguracaoEtapa>(seqConfiguracaoEtapa),
                x => new
                {
                    SeqEtapaSGF = x.EtapaProcesso.SeqEtapaSGF,
                    Idiomas = x.EtapaProcesso.Processo.Idiomas.Select(i => i.Idioma)
                });
            var seqEtapa = etapaComIdiomas.SeqEtapaSGF;
            var idiomasProcesso = etapaComIdiomas.Idiomas;

            var paginasEtapa = this.PaginaService.BuscarPaginasCompletasPorEtapa(seqEtapa);
            using (var unitOfWork = SMCUnitOfWork.Begin())
            {
                try
                {
                    foreach (var paginaEtapaSGF in paginasEtapa)
                    {
                        ConfiguracaoEtapaPaginaDomainService
                            .CriarPaginaPadraoSGF(seqConfiguracaoEtapa, idiomasProcesso, paginaEtapaSGF);
                    }
                    unitOfWork.Commit();
                }
                catch (Exception)
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Verifica se a configuração possui páginas
        /// </summary>
        public bool VerificarConfiguracaoPossuiPaginas(long seqConfiguracaoEtapa)
        {
            return this.ConfiguracaoEtapaPaginaDomainService.Count(
                new ConfiguracaoEtapaPaginaFilterSpecification { SeqConfiguracaoEtapa = seqConfiguracaoEtapa })
                > 0;
        }

        /// <summary>
        /// Busca um VO com os dados de idioma do processo
        /// </summary>
        public IdiomasPaginasProcessoVO BuscarIdiomasPaginasProcesso(long seqConfiguracaoEtapa)
        {
            var idiomas = this.SearchProjectionByKey(
                new SMCSeqSpecification<ConfiguracaoEtapa>(seqConfiguracaoEtapa),
                x => new
                {
                    idiomasProcesso = x.EtapaProcesso.Processo.Idiomas,
                    PossuiPaginas = x.Paginas.Any()
                });
            var vo = new IdiomasPaginasProcessoVO();

            if (idiomas.PossuiPaginas)
            {
                vo.IdiomasEmUso = this.SearchProjectionByKey(
                new SMCSeqSpecification<ConfiguracaoEtapa>(seqConfiguracaoEtapa),
                x => x.Paginas.FirstOrDefault().Idiomas.Select(i => i.Idioma)).ToList();
            }
            vo.IdiomaPadrao = idiomas.idiomasProcesso.First(x => x.Padrao).Idioma;
            vo.IdiomasDisponiveis = idiomas.idiomasProcesso.Where(x => !x.Padrao).Select(x => x.Idioma).Distinct().ToList();
            if (vo.IdiomasEmUso.Count > 0) vo.IdiomasEmUso.Remove(vo.IdiomaPadrao);
            vo.SeqConfiguracaoEtapa = seqConfiguracaoEtapa;
            return vo;
        }

        /// <summary>
        /// Adiciona e/ou remove idiomas de TODAS AS Páginas configuradas para uma configuração de etapa
        /// </summary>
        public void AlterarIdiomasPaginas(IdiomasPaginasProcessoVO idiomasPagina)
        {
            using (var unitOfWork = SMCUnitOfWork.Begin())
            {
                try
                {
                    idiomasPagina.IdiomasEmUso.Remove(SMCLanguage.Nenhum);
                    var idiomasBanco = this.SearchProjectionByKey(
                        new SMCSeqSpecification<ConfiguracaoEtapa>(idiomasPagina.SeqConfiguracaoEtapa),
                        x => x.Paginas.FirstOrDefault().Idiomas.Select(i => i.Idioma));
                    //PAra cada idioma nova adicionado adicionar as páginas no idioma
                    //para cada idioma removido, remover as páginas no idioma
                    foreach (var idioma in idiomasBanco)
                    {
                        if (!idiomasPagina.IdiomasEmUso.Contains(idioma))
                        {
                            var paginasIdioma = ConfiguracaoEtapaPaginaDomainService
                                .SearchProjectionBySpecification(
                                    new ConfiguracaoEtapaPaginaFilterSpecification
                                    {
                                        SeqConfiguracaoEtapa = idiomasPagina.SeqConfiguracaoEtapa
                                    },
                                    x => x.Idiomas.Where(i => i.Idioma == idioma).Select(d => d.Seq));
                            foreach (var seqsIdiomasPagina in paginasIdioma)
                            {
                                foreach (var seqIdioma in seqsIdiomasPagina)
                                {
                                    this.ConfiguracaoEtapaPaginaIdiomaDomainService
                                        .ExcluirConfiguracaoEtapaPaginaIdioma(seqIdioma);
                                }
                            }
                        }
                        else
                        {
                            idiomasPagina.IdiomasEmUso.Remove(idioma);
                        }
                    }
                    //Adicionar todos os idiomas restantes
                    if (idiomasPagina.IdiomasEmUso.Count > 0)
                    {
                        var dadosEtapa = this.SearchProjectionByKey(
                            new SMCSeqSpecification<ConfiguracaoEtapa>(idiomasPagina.SeqConfiguracaoEtapa),
                            x => new
                            {
                                SeqEtapaSGF = x.EtapaProcesso.SeqEtapaSGF,
                                Paginas = x.Paginas
                            });
                        var paginasSGF = this.PaginaService.BuscarPaginasCompletasPorEtapa(dadosEtapa.SeqEtapaSGF);
                        foreach (var pagina in dadosEtapa.Paginas)
                        {
                            //Cria os idiomas para as páginas
                            ConfiguracaoEtapaPaginaDomainService.CriarPaginaIdioma(idiomasPagina.IdiomasEmUso,
                                   paginasSGF.FirstOrDefault(x => x.Seq == pagina.SeqPaginaEtapaSGF), pagina.Seq);
                        }
                    }
                    unitOfWork.Commit();
                }
                catch (Exception)
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Retora a lista de páginas não criadas com o sequencial das páginas no SGF
        /// </summary>
        public List<SMCDatasourceItem> BuscarPaginasNaoCriadas(long seqConfiguracaoEtapa)
        {
            //Recuperar páginas existentes para a configuração
            var dados = this.SearchProjectionByKey(
                new SMCSeqSpecification<ConfiguracaoEtapa>(seqConfiguracaoEtapa),
                x => new
                {
                    Paginas = x.Paginas.Select(p => p.SeqPaginaEtapaSGF).Distinct(),
                    SeqEtapaSgf = x.EtapaProcesso.SeqEtapaSGF,
                });
            //Buscar no SGF as páginas que não estão criadas
            return this.PaginaService
                .BuscarPaginasEtapaForaIntervaloKeyValue(dados.SeqEtapaSgf, dados.Paginas.ToArray());
        }

        /// <summary>
        /// Adiciona as páginas segundo o modelo do SGF na configuracao etapa passada
        /// (para todos os idiomas em uso na configuração)
        /// </summary>
        public void AdicionarPaginasConfiguracao(long seqConfiguracaoEtapa, IEnumerable<long> seqPaginasSGF)
        {
            using (var unitOfWork = SMCUnitOfWork.Begin())
            {
                try
                {
                    //Teste de situação da etapa
                    var situacao = this.SearchProjectionByKey(new SMCSeqSpecification<ConfiguracaoEtapa>(seqConfiguracaoEtapa),
                        x => x.EtapaProcesso.SituacaoEtapa);
                    if (situacao == SituacaoEtapa.Liberada)
                    {
                        throw new AdicionarPaginaEtapaLiberadaException();
                    }

                    //Recuperar páginas existentes para a configuração
                    var idiomas = this.SearchProjectionByKey(
                        new SMCSeqSpecification<ConfiguracaoEtapa>(seqConfiguracaoEtapa),
                        x => x.Paginas.FirstOrDefault().Idiomas.Select(i => i.Idioma));

                    //Buscar as páginas no SGF
                    var paginasSGF = this.PaginaService.BuscarPaginasEtapaCompletas(seqPaginasSGF.ToArray());
                    //Adicionar as páginas na configuracao Etapa
                    foreach (var pagina in paginasSGF)
                    {
                        this.ConfiguracaoEtapaPaginaDomainService.CriarPaginaPadraoSGF(
                            seqConfiguracaoEtapa,
                            idiomas,
                            pagina);
                    }
                    unitOfWork.Commit();
                }
                catch (Exception)
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }

        public void CopiarConfiguracoesEtapa(CopiarConfiguracoesEtapaVO dadosCopia)
        {
            if (dadosCopia.Configuracoes != null)
            {
                var configuracaoEtapaBanco = EtapaProcessoDomainService.SearchProjectionByKey(
                                                     new SMCSeqSpecification<EtapaProcesso>(dadosCopia.SeqEtapaProcesso),
                                                 x => x.Configuracoes.Select(
                                                     c => new CopiarConfiguracoesEtapaDetalheVO
                                                     {
                                                         SeqConfiguracaoEtapa = c.Seq,
                                                         NumeroMaximoOfertaPorInscricao = c.NumeroMaximoOfertaPorInscricao,
                                                         ExigeJustificativaOferta = c.ExigeJustificativaOferta,
                                                         NumeroMaximoConvocacaoPorInscricao = c.NumeroMaximoConvocacaoPorInscricao,
                                                         PermiteNovaEntregaDocumentacao = c.PermiteNovaEntregaDocumentacao
                                                     })).ToList();

                foreach (var config in dadosCopia.Configuracoes)
                {
                    var configCopia = configuracaoEtapaBanco.Where(f => f.SeqConfiguracaoEtapa == config.SeqConfiguracaoEtapa).First();
                    config.NumeroMaximoOfertaPorInscricao = configCopia.NumeroMaximoOfertaPorInscricao;
                    config.ExigeJustificativaOferta = configCopia.ExigeJustificativaOferta;
                    config.NumeroMaximoConvocacaoPorInscricao = configCopia.NumeroMaximoConvocacaoPorInscricao;
                    config.PermiteNovaEntregaDocumentacao = configCopia.PermiteNovaEntregaDocumentacao;
                }
            }
            CopiarConfiguracoesEtapa(dadosCopia, null);
        }

        /// <summary>
        /// Implementa a cópia de configurações de etapa de uma etapa de origem para uma etapa de um processo de destinho
        /// </summary>
        public void CopiarConfiguracoesEtapa(CopiarConfiguracoesEtapaVO dadosCopia, Dictionary<long, long> grupoOfertaMapping)
        {
            using (var unitOfWork = SMCUnitOfWork.Begin())
            {
                try
                {
                    #region Validações para cópia

                    //Todo o processo segue a RN_INS_120
                    //Buscar a etapa de origem
                    var etapaOrigem = EtapaProcessoDomainService.SearchByKey(
                        new SMCSeqSpecification<EtapaProcesso>(dadosCopia.SeqEtapaProcesso),
                        x => x.Processo, x => x.Processo.Idiomas, x => x.Configuracoes);

                    //Buscar processo de destino.
                    var processoDestino = ProcessoDomainService.SearchByKey(
                        new SMCSeqSpecification<Processo>(dadosCopia.SeqProcessoDestino),
                        x => x.EtapasProcesso, x => x.Idiomas, x => x.UnidadeResponsavel, x => x.UnidadeResponsavel.TiposProcesso
                        , x => x.UnidadeResponsavel.TiposFormulario);

                    var etapaDestino = processoDestino.EtapasProcesso.FirstOrDefault(x => x.Token == etapaOrigem.Token);

                    if (etapaDestino == null)
                    {
                        throw new ProcessoDestinoNaoPossuiEtapaException(processoDestino.Descricao,
                            this.EtapaService.BuscarEtapasKeyValue(new long[] { etapaOrigem.SeqEtapaSGF })[0].Descricao);
                    }
                    else
                    {
                        if (etapaDestino.SituacaoEtapa == SituacaoEtapa.Liberada)
                        {
                            throw new CopiaConfiguracaoEtapaLiberadaException();
                        }
                        if (dadosCopia.Configuracoes.Any(x => x.DataInicio < etapaDestino.DataInicioEtapa
                            || x.DataFim > etapaDestino.DataFimEtapa))
                        {
                            throw new ConfiguracaoVigenciaForaEtapaException();
                        }
                    }

                    #endregion Validações para cópia

                    foreach (var config in dadosCopia.Configuracoes)
                    {
                        #region Cópia da configuração de etapa

                        var configCopia = this.SearchByKey(
                            new SMCSeqSpecification<ConfiguracaoEtapa>(config.SeqConfiguracaoEtapa),
                            x => x.GruposDocumentoRequerido[0].Itens,
                            x => x.GruposOferta, x => x.DocumentosRequeridos);
                        var gruposDocumentos = configCopia.GruposDocumentoRequerido != null ? configCopia.GruposDocumentoRequerido : new List<GrupoDocumentoRequerido>();
                        var documentos = configCopia.DocumentosRequeridos != null ? configCopia.DocumentosRequeridos : new List<DocumentoRequerido>();

                        configCopia.DocumentosRequeridos = null;
                        configCopia.GruposDocumentoRequerido = null;
                        configCopia.SeqEtapaProcesso = etapaDestino.Seq;
                        configCopia.SeqArquivoImagem = null;
                        configCopia.Seq = 0;
                        configCopia.DataInicio = config.DataInicio;
                        configCopia.DataFim = config.DataFim;
                        configCopia.DataLimiteEntregaDocumentacao = config.DataLimiteDocumentacao;
                        configCopia.NumeroEdital = config.NumeroEdital;
                        configCopia.Nome = config.Descricao;
                        configCopia.NumeroMaximoOfertaPorInscricao = config.NumeroMaximoOfertaPorInscricao;
                        configCopia.ExigeJustificativaOferta = config.ExigeJustificativaOferta;
                        configCopia.NumeroMaximoConvocacaoPorInscricao = config.NumeroMaximoConvocacaoPorInscricao;
                        configCopia.PermiteNovaEntregaDocumentacao = config.PermiteNovaEntregaDocumentacao;

                        if (grupoOfertaMapping != null && grupoOfertaMapping.Keys.Count > 0 &&
                            configCopia.GruposOferta.Count > 0)
                        {
                            foreach (var grupo in configCopia.GruposOferta)
                            {
                                grupo.Seq = 0;
                                grupo.SeqConfiguracaoEtapa = 0;
                                grupo.SeqGrupoOferta = grupoOfertaMapping[grupo.SeqGrupoOferta];
                            }
                        }
                        else
                        {
                            configCopia.GruposOferta = null;
                        }

                        configCopia = this.InsertEntity(configCopia);

                        #endregion Cópia da configuração de etapa

                        #region Cópia de documentação

                        if (dadosCopia.CopiarDocumentacao)
                        {
                            //Se for copiar a documentação copiar documentos e grupos de documentos nessa ordem
                            Dictionary<long, long> mapaSeqsDocumentos = new Dictionary<long, long>();
                            foreach (var doc in documentos)
                            {
                                var seqDoc = doc.Seq;
                                doc.Seq = 0;
                                doc.SeqConfiguracaoEtapa = configCopia.Seq;
                                var novoDoc = DocumentoRequeridoDomainService.InsertEntity(doc);
                                mapaSeqsDocumentos.Add(seqDoc, novoDoc.Seq);
                            }

                            //Após inseridos todos os documentos inserir todos os grupos
                            foreach (var grupo in gruposDocumentos)
                            {
                                grupo.Seq = 0;
                                grupo.SeqConfiguracaoEtapa = configCopia.Seq;
                                foreach (var item in grupo.Itens)
                                {
                                    item.Seq = 0;
                                    item.SeqDocumentoRequerido = mapaSeqsDocumentos[item.SeqDocumentoRequerido];
                                    item.SeqGrupoDocumentoRequerido = 0;
                                }
                                GrupoDocumentoRequeridoDomainService.InsertEntity(grupo);
                            }
                        }

                        #endregion Cópia de documentação

                        #region Verificação de idiomas para cópia dé páginas/notificões

                        //Verificar idiomas dos processos para utilizar nas cópias de noticações e páginas
                        var idiomasOrigem = etapaOrigem.Processo.Idiomas.Select(x => x.Idioma);
                        List<SMCLanguage> idiomasACopiar = new List<SMCLanguage>();
                        //Pesquisa quais idiomas devem ser copiados
                        //bool contemIdiomaPadrao = false;
                        foreach (var idioma in processoDestino.Idiomas)
                        {
                            if (idiomasOrigem.Contains(idioma.Idioma))
                            {
                                idiomasACopiar.Add(idioma.Idioma);
                            }
                        }
                        //Verifica se o processo de origem contém o idioma padrão do processo de destino
                        var idiomaPadraoDestino = processoDestino.Idiomas.FirstOrDefault(x => x.Padrao);
                        //if (idiomasOrigem.Contains(idiomaPadraoDestino.Idioma))
                        //{
                        //    contemIdiomaPadrao = true;
                        //}

                        #endregion Verificação de idiomas para cópia dé páginas/notificões

                        #region Cópia de páginas

                        //Se for copiar páginas realizar cópia

                        if (dadosCopia.CopiarPaginas)
                        {
                            var paginasOrigem = ConfiguracaoEtapaPaginaDomainService.SearchBySpecification(
                                new ConfiguracaoEtapaPaginaFilterSpecification
                                {
                                    SeqConfiguracaoEtapa = config.SeqConfiguracaoEtapa,
                                }, x => x.Idiomas, x => x.Idiomas[0].Arquivos
                                , x => x.Idiomas[0].Arquivos[0].Arquivo
                                , x => x.Idiomas[0].Textos).ToList();
                            var paginasSGF = PaginaService.BuscarPaginasCompletasPorEtapa(etapaDestino.SeqEtapaSGF);
                            foreach (var pagina in paginasOrigem)
                            {
                                //Adicionar todas as páginas da origem
                                pagina.Seq = 0;
                                pagina.SeqConfiguracaoEtapa = configCopia.Seq;
                                var idiomasDaPagina = pagina.Idiomas;
                                pagina.Idiomas = null;
                                var novaPagina = ConfiguracaoEtapaPaginaDomainService.InsertEntity(pagina);
                                var idiomasSomenteDestino = processoDestino.Idiomas.Where(
                                    x => !idiomasOrigem.Contains(x.Idioma)).Select(x => x.Idioma);
                                if (idiomasSomenteDestino != null && idiomasSomenteDestino.Count() > 0)
                                {
                                    //Criar a página com o padrão do SGF para cada idioma do destino que não esteja na origem
                                    var paginaSGF = paginasSGF.FirstOrDefault(x => x.Seq == pagina.SeqPaginaEtapaSGF);
                                    ConfiguracaoEtapaPaginaDomainService.CriarPaginaIdioma(
                                        idiomasSomenteDestino,
                                        paginaSGF,
                                        novaPagina.Seq);
                                }
                                foreach (var idiomaPagina in idiomasDaPagina.Where(x => idiomasACopiar.Contains(x.Idioma)))
                                {
                                    //Para todo o idioma da origem que estiver no destino criar a págona no idioma
                                    idiomaPagina.Seq = 0;
                                    idiomaPagina.SeqConfiguracaoEtapaPagina = novaPagina.Seq;
                                    var textos = idiomaPagina.Textos;
                                    var arquivos = idiomaPagina.Arquivos;
                                    idiomaPagina.Textos = null;
                                    idiomaPagina.Arquivos = null;
                                    //Caso a unidade responsável do processo de origem seja diferente da unidade responsável do processo
                                    //destino ou caso o tipo de formulário do processo de origem esteja desativado para a unidade
                                    //responsável, os dados do formulário e da visão não devem ser copiados
                                    if (idiomaPagina.SeqFormularioSGF.HasValue)
                                    {
                                        var seqTipoFormulario = FormularioService.BuscarFormulario(idiomaPagina.SeqFormularioSGF.Value, IncludesFormulario.Nenhum).SeqTipoFormulario;
                                        if (processoDestino.SeqUnidadeResponsavel != etapaOrigem.Processo.SeqUnidadeResponsavel
                                            || processoDestino.UnidadeResponsavel.TiposFormulario.Any(x => x.SeqTipoFormularioSGF == seqTipoFormulario && !x.Ativo))
                                        {
                                            idiomaPagina.SeqFormularioSGF = null;
                                            idiomaPagina.SeqVisaoSGF = null;
                                        }
                                    }
                                    var novoIdiomaPagina = ConfiguracaoEtapaPaginaIdiomaDomainService.
                                        InsertEntity(idiomaPagina);

                                    //Copiar as seções de texto e de arquivo das páginas
                                    foreach (var texto in textos)
                                    {
                                        texto.SeqConfiguracaoEtapaPaginaIdioma = novoIdiomaPagina.Seq;
                                        texto.Seq = 0;
                                        TextoSecaoPaginaDomainService.InsertEntity(texto);
                                    }

                                    foreach (var arquivo in arquivos)
                                    {
                                        arquivo.SeqConfiguracaoEtapaPaginaIdioma = novoIdiomaPagina.Seq;
                                        arquivo.Seq = 0;
                                        ArquivoSecaoPaginaDomainService.InsertEntity(arquivo);
                                    }

                                }
                            }
                        }
                        else
                        {
                            CriarPaginasPadraoConfiguracao(configCopia.Seq);
                        }

                        #endregion Cópia de páginas
                    }
                    unitOfWork.Commit();
                }
                catch (Exception)
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }

        private void EspecificarBotaoInscrever(long seqInscrito, List<EtapaProcessoVO> lista)
        {
            foreach (var processo in lista)
            {
                foreach (var grupo in processo.Grupos)
                {
                    grupo.BotaoInscrever = $"Inscrever{processo.TokenResource.ToLower()}";
                    grupo.BotaoInscreverTootip = $"Inscrever{processo.TokenResource.ToLower()}Tooltip";
                    grupo.HabilitarBotaoInscrever = grupo.Inscricoes.Count == 0;

                    foreach (var inscricao in grupo.Inscricoes)
                    {
                        var (mensagem, habilitarBotao) = InscricaoDomainService.VerificarPermissaoIniciarContinuarInscricaoBotoes(seqInscrito,
                                                                                grupo.SeqConfiguracaoEtapa,
                                                                                grupo.SeqGrupo,
                                                                                inscricao.SeqInscricao);
                        grupo.HabilitarBotaoInscrever = habilitarBotao;
                        grupo.MensagemBotaoInscrever = mensagem;

                        if (habilitarBotao)
                        {
                            break;
                        }
                    }
                }
            }
        }
    }
}