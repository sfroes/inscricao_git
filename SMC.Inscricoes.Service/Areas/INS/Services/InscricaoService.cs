using MC.Inscricoes.ServiceContract.Areas.INS.Data;
using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Financeiro.ServiceContract.BLT.Data;
using SMC.Formularios.ServiceContract.Areas.TMP.Interfaces;
using SMC.Framework;
using SMC.Framework.Domain;
using SMC.Framework.Extensions;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.Service;
using SMC.Framework.Specification;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Inscricoes.Common.Constants;
using SMC.Inscricoes.Domain.Areas.INS.DomainServices;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using SMC.Inscricoes.Domain.Areas.INS.ValueObjects;
using SMC.Inscricoes.Domain.Areas.SEL.DomainServices;
using SMC.Inscricoes.Domain.Areas.SEL.Models;
using SMC.Inscricoes.Domain.Areas.SEL.Specifications;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data.Inscricao;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using SMC.Seguranca.ServiceContract.Areas.USU.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace SMC.Inscricoes.Service.Areas.INS.Services
{
    public class InscricaoService : SMCServiceBase, IInscricaoService
    {
        #region DomainService

        private InscricaoDomainService InscricaoDomainService
        {
            get { return this.Create<InscricaoDomainService>(); }
        }

        private InscricaoHistoricoSituacaoDomainService InscricaoHistoricoSituacaoDomainService
        {
            get { return this.Create<InscricaoHistoricoSituacaoDomainService>(); }
        }

        private ConfiguracaoEtapaDomainService ConfiguracaoEtapaDomainService
        {
            get { return this.Create<ConfiguracaoEtapaDomainService>(); }
        }

        private ConfiguracaoEtapaPaginaDomainService ConfiguracaoEtapaPaginaDomainService
        {
            get { return this.Create<ConfiguracaoEtapaPaginaDomainService>(); }
        }

        private ConfiguracaoEtapaPaginaIdiomaDomainService ConfiguracaoEtapaPaginaIdiomaDomainService
        {
            get { return this.Create<ConfiguracaoEtapaPaginaIdiomaDomainService>(); }
        }

        private GrupoOfertaDomainService GrupoOfertaDomainService
        {
            get { return this.Create<GrupoOfertaDomainService>(); }
        }

        private InscricaoCodigoAutorizacaoDomainService InscricaoCodigoAutorizacaoDomainService
        {
            get { return this.Create<InscricaoCodigoAutorizacaoDomainService>(); }
        }

        private OfertaDomainService OfertaDomainService
        {
            get { return this.Create<OfertaDomainService>(); }
        }

        private InscricaoHistoricoPaginaDomainService InscricaoHistoricoPaginaDomainService
        {
            get { return this.Create<InscricaoHistoricoPaginaDomainService>(); }
        }

        private InscricaoOfertaDomainService InscricaoOfertaDomainService
        {
            get { return this.Create<InscricaoOfertaDomainService>(); }
        }

        private InscricaoDadoFormularioDomainService InscricaoDadoFormularioDomainService
        {
            get { return this.Create<InscricaoDadoFormularioDomainService>(); }
        }

        private InscricaoDocumentoDomainService InscricaoDocumentoDomainService
        {
            get { return this.Create<InscricaoDocumentoDomainService>(); }
        }

        private OfertaPeriodoTaxaDomainService OfertaPeriodoTaxaDomainService
        {
            get { return this.Create<OfertaPeriodoTaxaDomainService>(); }
        }

        private InscricaoBoletoDomainService InscricaoBoletoDomainService
        {
            get { return this.Create<InscricaoBoletoDomainService>(); }
        }

        private InscricaoBoletoTituloDomainService InscricaoBoletoTituloDomainService
        {
            get { return this.Create<InscricaoBoletoTituloDomainService>(); }
        }

        private InscritoDomainService InscritoDomainService
        {
            get { return this.Create<InscritoDomainService>(); }
        }

        private GrupoDocumentoRequeridoDomainService GrupoDocumentoRequeridoDomainService
        {
            get { return this.Create<GrupoDocumentoRequeridoDomainService>(); }
        }

        private InscricaoOfertaHistoricoSituacaoDomainService InscricaoOfertaHistoricoSituacaoDomainService
        {
            get { return Create<InscricaoOfertaHistoricoSituacaoDomainService>(); }
        }

        private ProcessoDomainService ProcessoDomainService
        {
            get { return Create<ProcessoDomainService>(); }
        }

        #endregion DomainService

        #region Service

        private DadosMestres.ServiceContract.Areas.GED.Interfaces.ITipoDocumentoService TipoDocumentoService
        {
            get { return this.Create<DadosMestres.ServiceContract.Areas.GED.Interfaces.ITipoDocumentoService>(); }
        }

        private IUsuarioService UsuarioService
        {
            get { return this.Create<IUsuarioService>(); }
        }

        private ISituacaoService SituacaoService
        {
            get { return this.Create<ISituacaoService>(); }
        }

        #endregion Service

        #region Regras de Negócio

        /// <summary>
        /// Verifica regras de negócio para iniciar ou continuar uma nova inscrição
        /// </summary>
        /// <param name="filtro">Filtros para validação</param>
        /// <returns>TRUE caso possa iniciar ou continuar a inscrição, FALSE caso contrário</returns>
        public bool VerificarPermissaoIniciarContinuarInscricao(IniciarContinuarInscricaoFiltroData filtro)
        {
            return InscricaoDomainService.VerificarPermissaoIniciarContinuarInscricao(filtro.SeqInscrito, filtro.SeqConfiguracaoEtapa, filtro.SeqGrupoOferta, filtro.SeqInscricao);
        }

        /// <summary>
        /// Verifica a regra para registro de documentação entregue
        /// </summary>
        public bool VerificarSituacaoRegistrarEntregaDocumentos(long seqInscricao)
        {
            return InscricaoDomainService.VerificarSituacaoRegistrarEntregaDocumentos(seqInscricao);
        }

        #endregion Regras de Negócio

        #region Buscar Informações de Inscricoes

        /// <summary>
        /// Busca os dados da inscrição resumidos par aa exibição
        /// </summary>
        /// <param name="seqInscricao"></param>
        public DadosInscricaoData BuscarInscricaoResumida(long seqInscricao, bool exibirDescricaoOfertaPorNome = true)
        {
            return SMCMapperHelper.Create<DadosInscricaoData>(
                this.InscricaoDomainService.BuscarInscricaoResumida(seqInscricao, exibirDescricaoOfertaPorNome));
        }

        /// <summary>
        /// Busca as inscrições em processos para um determinado inscrito
        /// </summary>
        /// <param name="filtro">Filtro para pesquisa</param>
        /// <returns>Lista de inscrições por processo</returns>
        public SMCPagerData<InscricoesProcessoData> BuscarInscricoesProcesso(InscricaoFiltroData filtro)
        {
            int total;
            InscricaoFilterSpecification spec = SMCMapperHelper.Create<InscricaoFilterSpecification>(filtro);
            var lista = InscricaoDomainService.BuscarInscricoes(spec, out total).TransformList<InscricoesProcessoData>();
            return new SMCPagerData<InscricoesProcessoData>(lista, total);
        }

        /// <summary>
        /// Busca as situação das isncrições de um processo sumarizadas
        /// </summary>
        /// <param name="filtro">Filtros para pesquisa</param>
        /// <returns>Lista de situações das inscrições de um processo sumarizadas</returns>
        public SMCPagerData<SituacaoInscricaoProcessoData> BuscarSituacaoInscricaoProcesso(SituacaoInscricaoProcessoFiltroData filtro)
        {
            int total;
            var lista = InscricaoDomainService.BuscarSituacaoInscricaoProcesso(
                SMCMapperHelper.Create<SituacaoInscricaoProcessoFilterSpecification>(filtro), out total)
                .TransformList<SituacaoInscricaoProcessoData>();
            return new SMCPagerData<SituacaoInscricaoProcessoData>(lista, total);
        }

        /// <summary>
        /// Busca as situação das isncrições de um processo sumarizadas e com os dados do inscrito
        /// </summary>
        /// <param name="filtro">Filtros para pesquisa</param>
        public SMCPagerData<SituacaoInscricaoInscritoProcessoData> BuscarSituacaoInscricaoInscritoProcesso(SituacaoInscricaoProcessoFiltroData filtro)
        {
            int total;
            var lista = InscricaoDomainService.BuscarSituacaoInscricaoInscritoProcesso(
                SMCMapperHelper.Create<SituacaoInscricaoProcessoFilterSpecification>(filtro), out total)
                .TransformList<SituacaoInscricaoInscritoProcessoData>();
            return new SMCPagerData<SituacaoInscricaoInscritoProcessoData>(lista, total);
        }

        /// <summary>
        /// Busca as ofertas de uma inscrição
        /// </summary>
        /// <param name="seqInscricao">Sequencial da inscrição</param>
        /// <returns>Lista de ofertas de uma inscrição</returns>
        public List<InscricaoOfertaData> BuscarInscricaoOfertas(long seqInscricao)
        {
            InscricaoOfertaFilterSpecification spec = new InscricaoOfertaFilterSpecification()
            {
                SeqInscricao = seqInscricao
            };
            spec.SetOrderBy(o => o.NumeroOpcao);

            var ofertas = InscricaoOfertaDomainService.SearchProjectionBySpecification(spec,
                                                        x => new InscricaoOfertaVO
                                                        {
                                                            SeqOferta = x.SeqOferta,
                                                            Oferta = x.Oferta,
                                                            SeqProcesso = x.Oferta.SeqProcesso,
                                                            ExibirPeriodoAtividadeOferta = x.Oferta.Processo.ExibirPeriodoAtividadeOferta
                                                        }).ToList();

            foreach (var item in ofertas)
            {
                OfertaDomainService.AdicionarDescricaoCompleta(item.Oferta, item.ExibirPeriodoAtividadeOferta, false);
            }

            bool inscricaoForaPrazo = false;

            if (ofertas.SMCAny())
            {
                inscricaoForaPrazo = InscritoDomainService.VerificaPermissaoInscricaoForaPrazo(ofertas.FirstOrDefault().SeqProcesso);
            }

            /*(!inscricaoForaPrazo && (DateTime.Now < x.Oferta.DataInicio || DateTime.Now > x.Oferta.DataFim))
             *Essa parte só será avaliada se a inscrição NÃO for fora do prazo (inscricaoForaPrazo == false).
             *Se inscricaoForaPrazo == true, essa parte será ignorada.
             */
            var inscricaoOferta = InscricaoOfertaDomainService.SearchProjectionBySpecification(spec,
                 x => new InscricaoOfertaData
                 {
                     Seq = x.Seq,
                     SeqInscricao = x.SeqInscricao,
                     SeqOferta = x.SeqOferta,
                     NumeroOpcao = x.NumeroOpcao,
                     JustificativaInscricao = x.JustificativaInscricao,
                     Ativo = x.Oferta.Ativo,
                     OfertaImpedida = x.Oferta.DataCancelamento.HasValue
                                      || !x.Oferta.Ativo
                                      || (!inscricaoForaPrazo
                                      && (DateTime.Now < x.Oferta.DataInicio || DateTime.Now > x.Oferta.DataFim))
                 }).ToList();



            foreach (var item in inscricaoOferta)
            {
                item.DescricaoCompleta = ofertas.Where(w => w.SeqOferta == item.SeqOferta).FirstOrDefault().Oferta.DescricaoCompleta;
            }

            return inscricaoOferta;
        }

        public InscricaoOfertaData BuscarInscricaoOferta(long seqInscricaoOferta)
        {
            return InscricaoOfertaDomainService.SearchByKey<InscricaoOferta, InscricaoOfertaData>(seqInscricaoOferta);
        }

        public long BuscarSeqInscricaoPorSeqInscricaoOferta(long seqInscricaoOferta)
        {
            return InscricaoOfertaDomainService.SearchProjectionByKey(new SMCSeqSpecification<InscricaoOferta>(seqInscricaoOferta), x => x.SeqInscricao);
        }

        /// <summary>
        /// Busca a lista de codigos de autorização de uma inscrição
        /// </summary>
        /// <param name="seqInscricao">Sequencial da inscrição</param>
        /// <returns>Lista de códigos de autorização de uma inscrição</returns>
        public List<InscricaoCodigoAutorizacaoData> BuscarInscricaoCodigoAutorizacao(long seqInscricao)
        {
            InscricaoCodigoAutorizacaoFilterSpecification spec = new InscricaoCodigoAutorizacaoFilterSpecification()
            {
                SeqInscricao = seqInscricao
            };
            var lista = this.InscricaoCodigoAutorizacaoDomainService
                        .SearchBySpecification(spec, IncludesInscricaoCodigoAutorizacao.CodigoAutorizacao).ToList();
            return lista.TransformList<InscricaoCodigoAutorizacaoData>();
        }

        /// <summary>
        /// Busca o sequencial do dado formulário e uma inscrição
        /// </summary>
        /// <param name="filtroData">Filtro para pesquisa</param>
        /// <returns>Sequencial do dado formulário de uma inscrição</returns>
        public long BuscarSeqDadoFormulario(InscricaoDadoFormularioFiltroData filtroData)
        {
            InscricaoDadoFormularioFilterSpecification spec = SMCMapperHelper.Create<InscricaoDadoFormularioFilterSpecification>(filtroData);
            return InscricaoDadoFormularioDomainService.SearchProjectionBySpecification(spec, i => i.Seq).SingleOrDefault();
        }

        /// <summary>
        /// Busca os inscrições dados formulários para uma inscrição
        /// </summary>
        public List<InscricaoDadoFormularioData> BuscarInscricaoDadoFormulario(long seqInscricao)
        {
            return InscricaoDadoFormularioDomainService.BuscarInscricaoDadosFormulario(seqInscricao)
                                                        .TransformList<InscricaoDadoFormularioData>();
        }

        /// <summary>
        /// Buscar os documentos liberados para upload de uma inscrição
        /// </summary>
        /// <param name="seqInscricao">Sequencial da inscrição</param>
        /// <param name="situacoesEntregaDocumentoDiferente">Situações do documento difetente</param>
        /// <returns>Documentos com os dados informados</returns>
        public List<InscricaoDocumentoData> BuscarListaInscricaoDocumentoOpcionaisUpload(long seqInscricao, SituacaoEntregaDocumento[] situacoesEntregaDocumentoDiferente)
        {
            InscricaoDocumentoFilterSpecification spec = new InscricaoDocumentoFilterSpecification()
            {
                SeqInscricao = seqInscricao,
                PermiteUploadArquivo = true,
                SituacoesEntregaDocumentoDiferentes = situacoesEntregaDocumentoDiferente
            };
            IEnumerable<InscricaoDocumento> lista = InscricaoDocumentoDomainService.SearchBySpecification(spec, IncludesInscricaoDocumento.ArquivoAnexado | IncludesInscricaoDocumento.DocumentoRequerido);

            var exibirTermo = lista.Any(a => a.EntregaPosterior);
            List<InscricaoDocumentoData> listaData = new List<InscricaoDocumentoData>();
            foreach (var doc in lista)
            {
                InscricaoDocumentoData item = SMCMapperHelper.Create<InscricaoDocumentoData>(doc);
                item.ExibeTermoResponsabilidadeEntrega = exibirTermo;
                item.DescricaoTipoDocumento = this.TipoDocumentoService.BuscarTipoDocumento(doc.DocumentoRequerido.SeqTipoDocumento).Descricao;
                item.DataLimiteEntrega = item.DataLimiteEntrega.HasValue ? item.DataLimiteEntrega : item.DataPrazoEntrega;
                item.ConvertidoParaPDF = doc.ConvertidoParaPDF;
                listaData.Add(item);
            }

            return listaData;
        }

        /// <summary>
        /// Busca o sequencial do arquivo do comprovante de uma inscrição
        /// </summary>
        /// <param name="seqInscricao">Sequencial da inscrição</param>
        /// <returns>Sequencial do arquivo do comprovante</returns>
        public long? BuscarSeqArquivoComprovante(long seqInscricao)
        {
            SMCSeqSpecification<Inscricao> spec = new SMCSeqSpecification<Inscricao>(seqInscricao);
            return InscricaoDomainService.SearchProjectionByKey(spec, i => i.SeqArquivoComprovante);
        }

        /// <summary>
        /// Busca o formulário já preenchido pelo inscrito.
        /// </summary>
        public InscricaoDadoFormularioFiltroData BuscarFormularioInscrito(long seqInscricao, long seqConfiguracaoEtapaPagina)
        {
            var spec = new InscricaoDadoFormularioFilterSpecification() { SeqInscricao = seqInscricao, SeqConfiguracaoEtapaPaginaIdioma = seqConfiguracaoEtapaPagina };
            return InscricaoDadoFormularioDomainService.SearchProjectionBySpecification(spec,
                                x => new InscricaoDadoFormularioFiltroData
                                {
                                    SeqInscricao = seqInscricao,
                                    SeqFormulario = x.SeqFormulario,
                                    SeqVisao = x.SeqVisao,
                                    SeqDadoFormulario = x.Seq
                                }).FirstOrDefault();
        }

        public bool VerificaApenasInscricoesTeste(long[] seqInscricoes)
        {
            return InscricaoDomainService.VerificaApenasInscricoesTeste(seqInscricoes);
        }

        public List<string> BuscarNomesInscritos(List<long> seqInscricoes)
        {
            var spec = new SMCContainsSpecification<InscricaoOferta, long>(x => x.Seq, seqInscricoes.ToArray());
            spec.SetOrderBy(o => o.Inscricao.Inscrito.Nome);
            return InscricaoOfertaDomainService.SearchProjectionBySpecification(spec,
                                                                    x => (x.Inscricao.Inscrito.NomeSocial != null) ?
                                                                            x.Inscricao.Inscrito.NomeSocial + " (" + x.Inscricao.Inscrito.Nome + ")" :
                                                                            x.Inscricao.Inscrito.Nome).ToList();
        }

        public ObservacaoInscricaoData BuscarObservacaoInscricao(long seqInscricao)
        {
            return InscricaoDomainService.BuscarObservacaoInscricao(seqInscricao).Transform<ObservacaoInscricaoData>();
        }

        public bool PossuiBoletoPago(long seqInscricao)
        {
            return this.InscricaoDomainService.PossuiBoletoPago(seqInscricao);
        }

        public bool PossuiBoleto(long seqInscricao)
        {
            return this.InscricaoDomainService.PossuiBoleto(seqInscricao);
        }

        public bool PossuiOfertaVigente(long seqInscricao)
        {
            return this.InscricaoDomainService.PossuiOfertaVigente(seqInscricao);
        }

        #endregion Buscar Informações de Inscricoes

        #region Buscar Informações Página

        /// <summary>
        /// Busca as informações da primeira página da etapa de inscrição
        /// </summary>
        /// <param name="seqConfiguracaoEtapa">Sequencial da configuração de etapa</param>
        /// <returns>Informações da primeira página do processo de inscrição</returns>
        public ConfiguracaoEtapaPaginaData BuscarConfiguracaoEtapaPrimeiraPagina(long seqConfiguracaoEtapa)
        {
            ConfiguracaoEtapaPaginaFilterSpecification spec = new ConfiguracaoEtapaPaginaFilterSpecification()
            {
                SeqConfiguracaoEtapa = seqConfiguracaoEtapa,
            };
            spec.SetOrderBy(p => p.Ordem);
            ConfiguracaoEtapaPagina pagina = ConfiguracaoEtapaPaginaDomainService.SearchBySpecification(spec).FirstOrDefault();
            return SMCMapperHelper.Create<ConfiguracaoEtapaPaginaData>(pagina);
        }

        /// <summary>
        /// Busca as informações da uma página
        /// </summary>
        /// <param name="seqConfiguracaoEtapa">Sequencial da configuração de etapa</param>
        /// <param name="tokenPagina">Token da página a ser recuperada</param>
        /// <returns>Informações de uma página</returns>
        public ConfiguracaoEtapaPaginaData BuscarConfiguracaoEtapaPagina(long seqConfiguracaoEtapa, string tokenPagina)
        {
            ConfiguracaoEtapaPaginaFilterSpecification spec = new ConfiguracaoEtapaPaginaFilterSpecification()
            {
                SeqConfiguracaoEtapa = seqConfiguracaoEtapa,
                Token = tokenPagina
            };
            spec.SetOrderBy(p => p.Ordem);
            ConfiguracaoEtapaPagina pagina = ConfiguracaoEtapaPaginaDomainService.SearchBySpecification(spec).FirstOrDefault();
            return SMCMapperHelper.Create<ConfiguracaoEtapaPaginaData>(pagina);
        }

        /// <summary>
        /// Busca as informações da ultima página acessada por uma inscrição
        /// </summary>
        /// <param name="seqInscricao">Sequencial da inscrição</param>
        /// <returns>Informações da ultima página acessada por uma inscrição</returns>
        public ContinuarInscricaoData BuscarUltimaPaginaInscricao(long seqInscricao)
        {
            InscricaoHistoricoPagina historico = InscricaoHistoricoPaginaDomainService.BuscarUltimaPaginaInscricao(seqInscricao);

            return historico != null ? historico.Transform<ContinuarInscricaoData>() : null;
        }

        /// <summary>
        /// Busca as informações de uma página e suas seções
        /// </summary>
        /// <param name="filtro">filtros para a página</param>
        /// <returns>Informações da página</returns>
        public PaginaData BuscarPagina(PaginaFiltroData filtro)
        {
            // Busca os dados da página
            ConfiguracaoEtapaPaginaIdiomaFilterSpecification spec = new ConfiguracaoEtapaPaginaIdiomaFilterSpecification()
            {
                SeqConfiguracaoEtapaPagina = filtro.SeqConfiguracaoEtapaPagina,
                Idioma = filtro.Idioma
            };
            PaginaVO paginaVO = ConfiguracaoEtapaPaginaIdiomaDomainService.BuscarPagina(spec);

            // Busca as informações da configuração da etapa (fluxo de páginas)
            ConfiguracaoEtapaInfoVO confVO = ConfiguracaoEtapaDomainService.BuscarInformacoesConfiguracaEtapa(paginaVO.SeqConfiguracaoEtapa, filtro.Idioma);

            // Cria o Data de retorno
            PaginaData data = SMCMapperHelper.Create<PaginaData>(paginaVO, confVO);

            // Busca as informações do grupo de ofertas
            GrupoOfertaData grupo = GrupoOfertaDomainService.SearchByKey<GrupoOferta, GrupoOfertaData>(filtro.SeqGrupoOferta);
            data.SeqGrupoOferta = grupo.Seq;

            var processo = ProcessoDomainService.SearchProjectionByKey(grupo.SeqProcesso, p =>
             new
             {
                 NumeroGrupoOferta = p.GruposOferta.Count,
                 p.TipoProcesso.TokenResource,
                 UrlCss = p.UnidadeResponsavelTipoProcessoIdVisual.CssAplicacao

             });

            data.TokenResource = processo.TokenResource;

            if (!processo.UrlCss.Contains(CSS_PROCESSO.CSS_PROCESSO_INSCRICAO))
            {
                data.UrlCss += processo.UrlCss[processo.UrlCss.Length - 1] == '/' ? CSS_PROCESSO.CSS_PROCESSO_INSCRICAO : "/" + CSS_PROCESSO.CSS_PROCESSO_INSCRICAO;
            }
            else
            {
                data.UrlCss = processo.UrlCss;
            }

            if (processo.NumeroGrupoOferta > 1)
            {
                data.DescricaoGrupoOferta = grupo.Nome;
            }

            //// Define a descrição do grupo apenas se o processo não possuir mais de um grupo
            //if (ProcessoDomainService.SearchProjectionByKey(new SMCSeqSpecification<Processo>(grupo.SeqProcesso),
            //                                                                x => x.GruposOferta.Count) > 1)
            //{
            //    data.DescricaoGrupoOferta = grupo.Nome;
            //}

            // Busca as informações da inscrição caso informada
            if (filtro.SeqInscricao <= 0)
            {
                data.Idioma = filtro.Idioma;
            }
            else
            {
                // Busca as informações da inscrição
                SMCSeqSpecification<Inscricao> specInsc = new SMCSeqSpecification<Inscricao>(filtro.SeqInscricao);
                var includes = IncludesInscricao.Ofertas | IncludesInscricao.Ofertas_Oferta |
                                IncludesInscricao.HistoricosSituacao |
                                IncludesInscricao.Processo |
                                IncludesInscricao.Processo_TipoProcesso |
                                IncludesInscricao.HistoricosSituacao_TipoProcessoSituacao;
                Inscricao inscricao = InscricaoDomainService.SearchByKey(specInsc, includes);

                // Informa os dados da inscrição
                data.Idioma = inscricao.Idioma;
                data.SeqInscricao = inscricao.Seq;
                data.UidProcesso = inscricao.Processo.UidProcesso;

                var ofertaHabilitaCheckin = InscricaoDomainService.CriarListaOfertas(inscricao);

                // Preenche a descrição das ofertas da inscrição
                data.DescricaoOfertas = ofertaHabilitaCheckin.Item1;

                data.HabilitaCheckin = ofertaHabilitaCheckin.Item2;

                //Preenche se o tipo do processo possui gestão de eventos
                data.GestaoEventos = inscricao.Processo.TipoProcesso.GestaoEventos;

                // Caso alguma oferta da inscrição esteja inválida, apresenta mensagem de alerta no fluxo da página
                // de seleção de oferta
                bool ofertaInvalida = false;
                InscricaoOferta inscricaoOfertaInvalida = new InscricaoOferta();
                for (int i = 0; i < inscricao.Ofertas.Count; i++)
                {
                    if (inscricao.Ofertas[i].Oferta.Cancelada || !inscricao.Ofertas[i].Oferta.Ativo ||
                        (!inscricao.Ofertas[i].Oferta.Vigente && !InscritoDomainService.VerificaPermissaoInscricaoForaPrazo(inscricao.SeqProcesso))
                         && inscricao.Ofertas[i].Inscricao.HistoricosSituacao.All(h => h.TipoProcessoSituacao.Token != TOKENS.SITUACAO_INSCRICAO_FINALIZADA))
                    {
                        ofertaInvalida = true;
                        inscricaoOfertaInvalida = inscricao.Ofertas[i];
                        break;
                    }
                }

                if (ofertaInvalida)
                {
                    var oferta = inscricaoOfertaInvalida;
                    string descricaoOferta = OfertaDomainService.BuscarHierarquiaOfertaCompleta(oferta.SeqOferta, false).DescricaoCompleta;

                    FluxoPaginaData paginaSelecao = data.FluxoPaginas.Where(f => f.Token.Equals(TOKENS.PAGINA_SELECAO_OFERTA)).First();
                    paginaSelecao.Alerta = string.Format(
                        Domain.Resources.MessagesResource.ResourceManager.GetString(
                        "AlertaInscricaoComOfertaInvalida", System.Threading.Thread.CurrentThread.CurrentCulture)
                    );
                }

                // Preenche o token da situação atual da inscrição
                data.TokenSituacaoAtual = inscricao.HistoricosSituacao.Where(s => s.Atual).First().TipoProcessoSituacao.Token;
                data.DescricaoSituacaoAtual = inscricao.HistoricosSituacao.Where(s => s.Atual).First().TipoProcessoSituacao.Descricao;
                data.HabilitaCheckin = inscricao.Ofertas.Any(a => a.Oferta.HabilitaCheckin);
            }

            return data;
        }

        public string BuscarUrlCss(long seqInscricao)
        {
            var urlCss = InscricaoDomainService.BuscarUrlCss(seqInscricao);
            return urlCss;
        }

        #endregion Buscar Informações Página

        #region Incluir/Alterar Inscrição

        /// <summary>
        /// Inclui uma inscrição
        /// </summary>
        /// <param name="inscricao">Inscrição a ser incluida</param>
        /// <returns>Sequencial da inscrição criada</returns>
        public long IncluirInscricao(InscricaoInicialData inscricao)
        {
            return this.InscricaoDomainService.IncluirInscricao(SMCMapperHelper.Create<Inscricao>(inscricao), inscricao.SeqGrupoOferta, inscricao.ConsentimentoLGPD);
        }

        /// <summary>
        /// Inclui um historico de acesso a página
        /// </summary>
        /// <param name="historico">Historico a ser incluido</param>
        public void IncluirInscricaoHistoricoPagina(InscricaoHistoricoPaginaData historico)
        {
            InscricaoHistoricoPaginaDomainService.InsertEntity(SMCMapperHelper.Create<InscricaoHistoricoPagina>(historico));
        }

        /// <summary>
        /// Salva as ofertas de uma inscrição
        /// </summary>
        /// <param name="ofertas">Ofertas a serem salvas</param>
        public void SalvarInscricaoOferta(List<InscricaoOfertaData> ofertas, short? numeroOpcoesDesejadas, List<InscricaoTaxaOfertaData> taxas = null)
        {
            InscricaoOfertaDomainService.SalvarListaInscricaoOferta(ofertas.TransformList<InscricaoOferta>(),
                                                                        numeroOpcoesDesejadas,
                                                                        taxas?.TransformList<InscricaoTaxaOfertaVO>());
        }
        
        /// <summary>
        /// Salva as ofertas de uma inscrição
        /// </summary>
        /// <param name="ofertas">Ofertas a serem salvas</param>
        public void SalvarInscricaoOfertaAngular(List<InscricaoOfertaData> ofertas, short? numeroOpcoesDesejadas, long seqGrupoOferta, List<InscricaoTaxaOfertaData> taxas = null)
        {
            InscricaoOfertaDomainService.SalvarListaInscricaoOfertaAngular(ofertas.TransformList<InscricaoOferta>(),
                                                                        numeroOpcoesDesejadas,
                                                                        seqGrupoOferta,
                                                                        taxas?.TransformList<InscricaoTaxaOfertaVO>());
        }

        /// <summary>
        /// Salva os códigos de autorização de uma inscrição
        /// </summary>
        /// <param name="seqInscricao">Sequencial da inscrição</param>
        /// <param name="codigos">Códigos a serem salvos</param>
        public void SalvarInscricaoCodigosAutorizacao(long seqInscricao, List<InscricaoCodigoAutorizacaoData> codigos)
        {
            InscricaoCodigoAutorizacaoDomainService.SalvarListaInscricaoCodigoAutorizacao(seqInscricao, codigos.TransformList<InscricaoCodigoAutorizacaoVO>());
        }

        /// <summary>
        /// Salva os documentos realizados upload na inscrição
        /// </summary>
        /// <param name="seqInscricao">Sequencial da inscrição</param>
        /// <param name="documentos">Lista de documentos para salvar</param>
        public void SalvarInscricaoDocumentoUpload(long seqInscricao, List<InscricaoDocumentoData> documentos)
        {
            InscricaoDocumentoDomainService.SalvarInscricaoDocumentoUpload(seqInscricao, documentos.TransformList<InscricaoDocumento>());
        }

        /// <summary>
        /// Finaliza uma inscrição
        /// </summary>
        /// <param name="seqInscricao">Sequencial da inscrição</param>
        /// <param name="arquivoComprovante">Conteudo do arquivo de comprovante</param>
        /// /// <param name="aceiteConversaoPDF">Aceite do termo conversão do PDF</param>
        public void FinalizarInscricao(long seqInscricao, bool aceiteConversaoPDF, bool ConcentimentoLGPD, byte?[] arquivoComprovante)
        {
            InscricaoDomainService.FinalizarInscricao(seqInscricao, aceiteConversaoPDF, ConcentimentoLGPD, arquivoComprovante);
        }

        public void AlterarFormularioInscricao(InscricaoDadoFormularioData dadoFormularioData)
        {
            InscricaoDadoFormularioDomainService.AlterarFormularioInscricao(dadoFormularioData.Transform<InscricaoDadoFormulario>());
        }
        public List<InscricaoDadoFormularioCampoData> BuscarCamposDadoFormularioPorSeqInscricao(long seqInscricao, List<string> tokensCampo)
        {
            return InscricaoDadoFormularioDomainService.BuscarCamposDadoFormularioPorSeqInscricao(seqInscricao, tokensCampo).TransformList<InscricaoDadoFormularioCampoData>();
        }

        public void ExcluirInscricao(long seqInscricao)
        {
            InscricaoDomainService.ExcluirInscricao(seqInscricao);
        }

        #endregion Incluir/Alterar Inscrição

        #region Inscrição/ Documentos

        /// <summary>
        /// Busca a lista de documentos da inscrição
        /// </summary>
        public InscricaoDocumentosUploadData BuscarDocumentosUploadInscricao(long seqInscricao)
        {
            return SMCMapperHelper.Create<InscricaoDocumentosUploadData>(
                this.InscricaoDocumentoDomainService.BuscarDocumentosInscricaoUpload(seqInscricao));
        }

        /// <summary>
        /// Retorna a lista de documentos, com situação e informações de entrega para uma inscrição informada
        /// </summary>
        public SumarioDocumentosEntreguesData BuscarSumarioDocumentosEntregue(long seqInscricao)
        {
            var spec = new InscricaoDocumentoFilterSpecification { SeqInscricao = seqInscricao };
            return InscricaoDocumentoDomainService.BuscarSumarioDocumentosEntregue(spec)
                .Transform<SumarioDocumentosEntreguesData>();
        }

        /// <summary>
        /// Retorna a lista de documentos, com situação e informações de entrega para uma inscrição informada
        /// </summary>
        public List<InscricaoDocumentoData> BuscarDocumentosInscricao(long seqInscricao)
        {
            var spec = new InscricaoDocumentoFilterSpecification { SeqInscricao = seqInscricao };
            return InscricaoDocumentoDomainService.BuscarInscricaoDocumentos(spec)
                .TransformList<InscricaoDocumentoData>();
        }

        /// <summary>
        /// Realiza a entrega de um documento para uma determinada inscrição
        /// </summary>
        public InscricaoDocumentoData SalvarDocumentoInscricao(InscricaoDocumentoData inscricaoDocumento)
        {
            return SMCMapperHelper.Create<InscricaoDocumentoData>(
                this.InscricaoDocumentoDomainService.SalvarDocumento(
                    SMCMapperHelper.Create<InscricaoDocumento>(inscricaoDocumento)));
        }

        /// <summary>
        /// Salva todos os documentos entregues
        /// </summary>
        /// <param name="SumarioDocumentosEntreguesViewModel"></param>
        /// <returns>SeqProcesso</returns>
        public long SalvarSumarioDocumentosEntreguesInscricao(SumarioDocumentosEntreguesData documentosEntregues)
        {
            return this.InscricaoDocumentoDomainService.SalvarSumarioDocumentosEntreguesInscricao(documentosEntregues.Transform<SumarioDocumentosEntreguesVO>());
        }

        public bool ValidarSituacaoAtualCandidatoOfertasConfirmadas(SumarioDocumentosEntreguesData documentosEntregues)
        {
            return this.InscricaoDocumentoDomainService.ValidarSituacaoAtualCandidatoOfertasConfirmadas(documentosEntregues.Transform<SumarioDocumentosEntreguesVO>());
        }

        public bool ValidarSituacaoAtualCandidatoOfertasDeferidas(SumarioDocumentosEntreguesData documentosEntregues)
        {
            return this.InscricaoDocumentoDomainService.ValidarSituacaoAtualCandidatoOfertasDeferidas(documentosEntregues.Transform<SumarioDocumentosEntreguesVO>());
        }

        public NovaEntregaDocumentacaoData BuscarDocumentosNovaEntregaDocumentacao(long seqInscricao)
        {
            return this.InscricaoDomainService.BuscarDocumentosNovaEntregaDocumentacao(seqInscricao).Transform<NovaEntregaDocumentacaoData>();
        }

        /// <summary>
        /// Salvar as novas entregas de documentação de uma inscrição
        /// </summary>
        /// <param name="novaEntregaDocumentacao">Documentos enviados pelo usuário</param>
        /// <returns>Sequencial da inscrição</returns>
        public long SalvarNovaEntregaDocumentacao(NovaEntregaDocumentacaoData novaEntregaDocumentacao)
        {
            return this.InscricaoDocumentoDomainService.SalvarNovaEntregaDocumentacao(novaEntregaDocumentacao.Transform<NovaEntregaDocumentacaoVO>());
        }

        /// <summary>
        /// Duplica o documento em questão
        /// </summary>
        public void DuplicarEntregaDocumento(long seqInscricaoDocumento)
        {
            InscricaoDocumentoDomainService.DuplicarEntregaDocumento(seqInscricaoDocumento);
        }

        /// <summary>
        /// Exclui um documento para uma inscrição
        /// A operação só deve ser possível se o documento em questão permirtir a entrega de mais de um documento
        /// e se houver outro documento do mesmo tipo já entregue, acs contrário deve lançar uma exceção com
        /// a mensagem
        /// "Para o documento {0} é necessário existir ao menos um registroPara o documento <descrição do tipo de documento excluído>" é necessário existir ao menos um registro"
        /// </summary>
        /// <param name="seqInscricaoDocumento"></param>
        public void ExcluirInscricaoDocumento(long seqInscricaoDocumento)
        {
            InscricaoDocumentoDomainService.ExcluirInscricaoDocumento(seqInscricaoDocumento);
        }

        public NovaEntregaDocumentacaoCabecalhoData BuscarCabecalhoNovaEntregaDocumentacao(long seqInscricao)
        {
            return this.InscricaoDomainService.BuscarCabecalhoNovaEntregaDocumentacao(seqInscricao).Transform<NovaEntregaDocumentacaoCabecalhoData>();
        }

        #endregion Inscrição/ Documentos

        #region Buscar / Persistir Dado Formulario

        /// <summary>
        /// Busca um dado formulário
        /// </summary>
        /// <param name="seqDadoFormulario">Sequencial do dado formulário a ser recuperado</param>
        /// <returns></returns>
        public InscricaoDadoFormularioData BuscarDadoFormulario(long seqDadoFormulario)
        {
            return InscricaoDadoFormularioDomainService.SearchByKey<InscricaoDadoFormulario, InscricaoDadoFormularioData>(seqDadoFormulario, IncludesDadoFormulario.DadosCampos);
        }

        /// <summary>
        /// Salva os dados do formulário
        /// </summary>
        /// <param name="dados"></param>
        public long SalvarFormularioInscricao(InscricaoDadoFormularioData dados)
        {
            return InscricaoDadoFormularioDomainService.SalvarInscricaoDadoFormulario(SMCMapperHelper.Create<InscricaoDadoFormulario>(dados));
        }

        /// <summary>
        /// Salva os dados do formulário Impacto
        /// </summary>
        /// <param name="dados">Dados formulário</param>
        public long SalvarFormularioImpacto(InscricaoDadoFormularioData dados)
        {
            return InscricaoDadoFormularioDomainService.SalvarInscricaoDadoFormularioImpacto(SMCMapperHelper.Create<InscricaoDadoFormulario>(dados));
        }

        #endregion Buscar / Persistir Dado Formulario

        #region Alteração de Situação

        public bool VerificaSituacaoInscricoesOfertaNaSituacao(List<long> inscricoes, string tokenSituacao)
        {
            return InscricaoOfertaHistoricoSituacaoDomainService.Count(
                new InscricaoOfertaPorTokenSpecification() { Inscricoes = inscricoes, Token = tokenSituacao }) > 0;
        }

        /// <summary>
        /// Retorna uma lista de isncrições contendo Seq e Nome do Inscrito de acordo
        /// com os sequenciais informados
        /// </summary>
        public List<DetalheAlteracaoSituacaoData> BuscarInscricoesPorSequencial(InscricaoSelecionadaData inscricoes)
        {
            var spec = new SMCContainsSpecification<Inscricao, long>(x => x.Seq, inscricoes.GridAnaliseInscricaoLote.ToArray());

            var projection = InscricaoDomainService.SearchProjectionBySpecification(spec,
                i => new DetalheAlteracaoSituacaoData
                {
                    SeqInscricao = i.Seq,
                    NomeInscrito = (i.Inscrito.NomeSocial != null) ? i.Inscrito.NomeSocial + " (" + i.Inscrito.Nome + ")" : i.Inscrito.Nome
                }).ToList();

            var retorno = new List<DetalheAlteracaoSituacaoData>();
            foreach (var inscricao in inscricoes.GridAnaliseInscricaoLote)
            {
                // Ordena o resultado, baseado na ordem que chegou as inscrições oferta.
                retorno.Add(projection.First(f => f.SeqInscricao == inscricao));
            }
            return retorno;
        }

        /// <summary>
        /// Altera a situação das inscrições informadas
        /// Todas as situações informadas devem estar na mesma situação atual e no mesmo processo e etapa
        /// </summary>
        /// <param name="seqTipoProcessoSituacaoDestino">Sequencial do tipo Processo Situação para destino das inscrições</param>
        /// <param name="seqInscricoes">Lista de sequencial das inscrições a serem alteradas</param>
        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        public void AlterarSituacaoInscricoes(AlteracaoSituacaoData data)
        {
            InscricaoHistoricoSituacaoDomainService.AlterarSituacaoInscricoes(data.Transform<AlteracaoSituacaoVO>());
        }

        /// <summary>
        /// Altera a justificativa e o motivo de uma situação
        /// </summary>
        /// <param name="seqHistoricoSituacao">Situação a ser alterada</param>
        /// <param name="justificativa">Justificativa informada</param>
        /// <param name="seqMotivo">Motivo informado</param>
        public void AlterarMotivoEJustificativaSituacao(long seqHistoricoSituacao, string justificativa, long? seqMotivo)
        {
            InscricaoHistoricoSituacaoDomainService.AlterarMotivoEJustificativaSituacao(seqHistoricoSituacao, justificativa, seqMotivo);
        }

        #endregion Alteração de Situação

        #region Histório de Situação

        public List<InscricaoHistoricoSituacaoData> BuscarSituacoesInscricao(long seqInscricao)
        {
            var spec = new InscricaoHistoricoSituacaoFilterSpecification() { SeqInscricao = seqInscricao };
            var situacoes = InscricaoHistoricoSituacaoDomainService.SearchProjectionBySpecification(spec,
                        f => new InscricaoHistoricoSituacaoData
                        {
                            Seq = f.Seq,
                            Situacao = f.TipoProcessoSituacao.Descricao,
                            TokenSituacao = f.TipoProcessoSituacao.Token,
                            SeqMotivo = f.SeqMotivoSituacaoSGF,
                            Justificativa = f.Justificativa,
                            Responsavel = f.UsuarioInclusao,
                            Data = f.DataSituacao,
                            Atual = f.Atual
                        }).ToList();

            var motivos = SituacaoService.BuscarDescricaoMotivos(situacoes.Where(f => f.SeqMotivo.HasValue).Select(f => f.SeqMotivo.Value).ToArray());

            foreach (var situacao in situacoes)
            {
                var descricaoMotivo = motivos.Where(f => f.Seq == situacao.SeqMotivo).FirstOrDefault();
                if (descricaoMotivo != null)
                {
                    situacao.Motivo = descricaoMotivo.Descricao;
                }
                long seqUsuarioInclusao;
                if (long.TryParse(situacao.Responsavel.Split('/')[0], out seqUsuarioInclusao))
                {
                    var usuario = UsuarioService.BuscarUsuario(seqUsuarioInclusao);
                    if (usuario != null)
                        situacao.Responsavel = usuario.Nome;
                }
            }

            return situacoes;
        }

        public List<InscricaoHistoricoSituacaoData> BuscarSituacoesInscricaoParaValidacaoDeTokens(long seqInscricao)
        {
            var spec = new InscricaoHistoricoSituacaoFilterSpecification() { SeqInscricao = seqInscricao };
            return InscricaoHistoricoSituacaoDomainService.SearchProjectionBySpecification(spec,
                        f => new InscricaoHistoricoSituacaoData
                        {
                            Seq = f.Seq,
                            Situacao = f.TipoProcessoSituacao.Descricao,
                            TokenSituacao = f.TipoProcessoSituacao.Token,
                            Atual = f.Atual,
                            TokenEtapa = f.EtapaProcesso.Token
                        }).ToList();
        }

        public List<InscricaoOfertaHistoricoSituacaoData> BuscarSituacoesInscricaoOferta(long seqInscricao)
        {
            var spec = new InscricaoOfertaHistoricoSituacaoInscricaoSpecification(seqInscricao);
            return InscricaoOfertaHistoricoSituacaoDomainService.SearchProjectionBySpecification(spec,
                        f => new InscricaoOfertaHistoricoSituacaoData
                        {
                            Seq = f.Seq,
                            Atual = f.Atual,
                            TokenSituacao = f.TipoProcessoSituacao.Token
                        }).ToList();
        }

        public InscricaoOfertaHistoricoSituacaoData BuscarSituacaoInscricaoOferta(long seqInscricaoHistoricoSituacao)
        {
            return InscricaoOfertaHistoricoSituacaoDomainService
                        .SearchByKey<InscricaoOfertaHistoricoSituacao, InscricaoOfertaHistoricoSituacaoData>(seqInscricaoHistoricoSituacao);
        }

        public InscricaoHistoricoSituacaoData BuscarHistoricoSituacao(long seqHistoricoSituacao)
        {
            var data = InscricaoHistoricoSituacaoDomainService.SearchProjectionByKey(new SMCSeqSpecification<InscricaoHistoricoSituacao>(seqHistoricoSituacao),
                                f => new InscricaoHistoricoSituacaoData
                                {
                                    Seq = f.Seq,
                                    SeqSituacao = f.SeqTipoProcessoSituacao,
                                    Situacao = f.TipoProcessoSituacao.Descricao,
                                    SeqMotivo = f.SeqMotivoSituacaoSGF,
                                    Justificativa = f.Justificativa,
                                    Responsavel = f.UsuarioInclusao,
                                    Data = f.DataSituacao
                                });

            long seqUsuarioInclusao;
            if (long.TryParse(data.Responsavel.Split('/')[0], out seqUsuarioInclusao))
            {
                var usuario = UsuarioService.BuscarUsuario(seqUsuarioInclusao);
                if (usuario != null)
                    data.Responsavel = usuario.Nome;
            }
            return data;
        }

        #endregion Histório de Situação

        #region Inscrição / Taxa

        /// <summary>
        /// Busca as informações de taxa pra uma ofeta
        /// </summary>
        /// <param name="seqOferta">Sequencial da oferta</param>
        /// <returns>Lista de taxas vigentes para a oferta</returns>
        public IEnumerable<InscricaoTaxaOfertaData> BuscarTaxaInscricaoOfertaVigente(long seqOferta, long seqInscricao)
        {
            return OfertaPeriodoTaxaDomainService.BuscarTaxaInscricaoOfertaVigente(seqOferta, seqInscricao)
                        .TransformList<InscricaoTaxaOfertaData>();
        }

        /// <summary>
        /// Busca as informações de taxa pra uma inscricao
        /// </summary>
        public IEnumerable<InscricaoTaxaOfertaData> BuscarTaxasOfertaInscricao(long seqInscricao)
        {
            return InscricaoBoletoDomainService.BuscarTaxasOfertaInscricao(seqInscricao)
                        .TransformList<InscricaoTaxaOfertaData>();
        }

        /// Busca as informações de taxa pra uma inscricao
        /// </summary>
        public IEnumerable<InscricaoTaxaOfertaData> BuscarTaxasOfertaInscricaoConfirmacao(long seqInscricao)
        {
            return InscricaoBoletoDomainService.BuscarTaxasOfertaInscricaoConfirmacao(seqInscricao)
                        .TransformList<InscricaoTaxaOfertaData>();
        }

        /// <summary>
        /// Verifica se uma determinada inscrição pode gerar boleto.
        /// </summary>
        public string VerificaPermissaoGerarBoletoInscricao(long seqInscricao)
        {
            try
            {
                this.InscricaoBoletoTituloDomainService.VerificaPermissaoGerarBoletoInscricao(seqInscricao);
                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Verifica se uma determinada inscrição pode emitir comprovante.
        /// </summary>
        public string VerificarPermissaoEmitirComprovante(long seqInscricao)
        {
            try
            {
                this.InscricaoDomainService.VerificarPermissaoEmitirComprovante(seqInscricao);
                return string.Empty;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }


        /// <summary>
        /// Busca o boleto para uma determinada inscrição
        /// </summary>
        public BoletoData GerarBoletoInscricao(long seqInscricao)
        {
            var boleto = this.InscricaoBoletoDomainService.GerarBoletoInscricao(seqInscricao);
            return boleto;
        }

        /// <summary>
        /// Busca os títulos existentes para um inscrição
        /// </summary>
        public List<TituloInscricaoData> BuscarTitulosInscricao(long seqInscricao)
        {
            return this.InscricaoBoletoTituloDomainService.BuscarTitulosInscricao(seqInscricao)
                .TransformList<TituloInscricaoData>();
        }

        /// <summary>
        /// Verifica se existe taxa para uma determinada inscricao
        /// </summary>
        public bool VerificarExistenciaTaxaInscricao(long seqInscricao)
        {
            return InscricaoBoletoDomainService.VerificarExistenciaTaxaInscricao(seqInscricao);
        }

        /// <summary>
        /// Verifica se o boleto ja foi pago
        /// </summary>
        public bool? VerificarPagamentoTaxaInscricao(long seqInscricao)
        {
            return InscricaoBoletoDomainService.VerificarPagamentoTaxaInscricao(seqInscricao);
        }

        #endregion Inscrição / Taxa

        #region Verificações de informações

        public bool VerificaDocumentoObrigatorio(long seqInscricaoDocumento, long seqInscricao)
        {
            // Verifica se o documento é obrigatório
            var documento = InscricaoDocumentoDomainService.SearchProjectionByKey(new SMCSeqSpecification<InscricaoDocumento>(seqInscricaoDocumento),
                                            x => new
                                            {
                                                Obrigatorio = x.DocumentoRequerido.Obrigatorio,
                                                SeqDocumentoRequerido = x.SeqDocumentoRequerido
                                            });

            if (!documento.Obrigatorio)
            {
                // Verifica se o documento faz parte de um grupo obrigatório e se existe pelo menos um item entregue
                long seqConfiguracaoEtapa = this.InscricaoDomainService
                            .SearchProjectionByKey(new SMCSeqSpecification<Inscricao>(seqInscricao), x => x.SeqConfiguracaoEtapa);

                var grupoSpec = new GrupoDocumentoRequeridoFilterSpecification
                {
                    SeqConfiguracaoEtapa = seqConfiguracaoEtapa
                };

                var grupoDocumento = GrupoDocumentoRequeridoDomainService.SearchBySpecification(grupoSpec,
                                                            x => x.Itens).First();

                var itens = grupoDocumento.Itens.Select(f => f.SeqDocumentoRequerido);

                var novoDocGrupo = InscricaoDocumentoDomainService.SearchBySpecification(
                                        new DocumentoRequeridoInscricaoSpecification()
                                        {
                                            SeqDocumentoRequerido = itens,
                                            SeqInscricao = seqInscricao
                                        });

                if (novoDocGrupo != null && novoDocGrupo.Any(f => f.SituacaoEntregaDocumento == SituacaoEntregaDocumento.Deferido && f.Seq != seqInscricaoDocumento))
                    return false;
            }

            // Verifica se o documento possui algum do mesmo SeqDocumentoRequerido já entregue
            var documentosMesmoTipo = InscricaoDocumentoDomainService.SearchProjectionBySpecification(new DocumentoRequeridoInscricaoSpecification()
            {
                SeqInscricao = seqInscricao,
                SeqDocumentoRequerido = new List<long> { documento.SeqDocumentoRequerido }
            },
                                        x => new
                                        {
                                            SituacaoEntregua = x.SituacaoEntregaDocumento
                                        });
            if (documentosMesmoTipo.Count(c => c.SituacaoEntregua == SituacaoEntregaDocumento.Deferido) > 1)
                return false;

            return true;
        }

        public bool VerificaDocumentacaoEntregue(long seqInscricao)
        {
            return InscricaoDomainService.SearchProjectionByKey(new SMCSeqSpecification<Inscricao>(seqInscricao),
                                                x => x.ConfiguracaoEtapa.DocumentosRequeridos.Any(d => d.Obrigatorio)
                                                 || x.ConfiguracaoEtapa.GruposDocumentoRequerido.Any(g => g.MinimoObrigatorio > 0) ?
                                                    x.DocumentacaoEntregue : false);
        }

        public bool VerificaFormularioEmUso(long seqConfiguracaoEtapaPaginaIdioma)
        {
            var conf = ConfiguracaoEtapaPaginaIdiomaDomainService.SearchProjectionByKey(
                            new SMCSeqSpecification<ConfiguracaoEtapaPaginaIdioma>(seqConfiguracaoEtapaPaginaIdioma),
                            x => new
                            {
                                x.SeqFormularioSGF,
                                x.SeqVisaoSGF,
                                x.SeqVisaoGestaoSGF
                            });

            if (!conf.SeqFormularioSGF.HasValue)
                return false;

            return InscricaoDadoFormularioDomainService.Count(new InscricaoDadoFormularioEmUsoSpecification()
            {
                SeqConfiguracaoEtapaPaginaIdioma = seqConfiguracaoEtapaPaginaIdioma,
                SeqFormulario = conf.SeqFormularioSGF.Value
            }) > 0;
        }

        public bool VerificaBoletoInscricaoAlteracaoTaxa(long seqInscricao, List<InscricaoTaxaData> taxas)
        {
            return this.InscricaoDomainService.VerificaBoletoInscricaoAlteracaoTaxa(seqInscricao, taxas.TransformList<InscricaoTaxaVO>());
        }

        #endregion Verificações de informações

        #region Liberação da Alteração da Inscrição

        public void LiberarAlteracaoInscricao(long seqInscricao)
        {
            InscricaoDomainService.LiberarAlteracaoInscricao(seqInscricao);
        }

        /// <summary>
        /// Validar RN_INS_141 Liberação da alteração de inscrição
        /// Regras 1 e 2
        /// </summary>
        /// <param name="seqInscricao"></param>
        public void ValidarLiberacaoAlteracaoInscricao(long seqInscricao)
        {
            InscricaoDomainService.ValidarLiberacaoAlteracaoInscricao(seqInscricao);
        }

        #endregion Liberação da Alteração da Inscrição

        public void AlterarObservacaoInscricao(ObservacaoInscricaoData observacaoInscricaoData)
        {
            InscricaoDomainService.AlterarObservacaoInscricao(observacaoInscricaoData.Seq, observacaoInscricaoData.Observacao);
        }

        public DadosProcessoInscricaoData BuscarDadosProcessoInscricao(long seqInscricao)
        {
            return InscricaoDomainService.SearchProjectionByKey(seqInscricao, x => new DadosProcessoInscricaoData
            {
                SeqProcesso = x.SeqProcesso,
                SeqTipoTemplateProcessoSGF = x.Processo.TipoProcesso.SeqTipoTemplateProcessoSGF
            });
        }

        /// <summary>
        /// Recupera os dados de uma inscrição necessários para emissão do comprovante
        /// </summary>
        /// <param name="seqInscricao">Sequencial da inscrição</param>
        /// <returns>Dados da inscrição</returns>
        public DadosComprovanteInscricaoData BuscarDadosComprovanteInscricao(long seqInscricao)
        {
            return InscricaoDomainService.SearchProjectionByKey(seqInscricao, p => new DadosComprovanteInscricaoData()
            {
                SeqInscricao = p.Seq,
                SeqInscrito = p.SeqInscrito,
                SeqConfiguracaoEtapa = p.SeqConfiguracaoEtapa,
                SeqGrupoOferta = p.SeqGrupoOferta,
                Idioma = p.Idioma
            });
        }

        /// <summary>
        /// Atualiza o arquivo do comprovante
        /// </summary>
        /// <param name="dadosComprovanteInscricao">Dados do comprovante</param>
        public void AlterarComprovanteInscricao(DadosComprovanteInscricaoData dadosComprovanteInscricao)
        {
            var inscricao = InscricaoDomainService.SearchByKey(dadosComprovanteInscricao.SeqInscricao);
            inscricao.ArquivoComprovante = new Domain.Models.ArquivoAnexado()
            {
                Conteudo = dadosComprovanteInscricao.DadosComprovante,
                Nome = $"ComprovanteInscricao{dadosComprovanteInscricao.SeqInscricao}.pdf",
                Tipo = "application/pdf",
                Tamanho = dadosComprovanteInscricao.DadosComprovante.Length
            };

            InscricaoDomainService.SaveEntity(inscricao);
        }

        public void ValidarBoletoPagoAlteracaoValor(long seqInscricao, long seqOferta)
        {
            OfertaPeriodoTaxaDomainService.ValidarBoletoPagoAlteracaoValor(seqInscricao, seqOferta);
        }

        public bool VerificaInscricaoApenasPrimeiraPagina(long seqInscricao, long seqConfiguracaoEtapaPagina)
        {
            return InscricaoDomainService.SearchProjectionByKey(seqInscricao, x => x.HistoricosPagina.Any(h => h.SeqConfiguracaoEtapaPagina != seqConfiguracaoEtapaPagina));
        }

        /// <summary>
        /// Valida se o inscrito está apto a receber a bolsta ex-aluno
        /// </summary>
        /// <param name="seqInscricao">Sequencial da inscrição</param>
        /// <param name="seqOferta">Sequencial da oferta a ser validada</param>
        /// <returns>True caso esteja apto</returns>
        public bool ValidarAptoBolsaNovoTitulo(long seqInscricao, long seqOferta)
        {
            return InscricaoDomainService.ValidarAptoBolsaNovoTitulo(seqInscricao, seqOferta);
        }

        /// <summary>
        /// Buscar dados Formulario Seminario
        /// </summary>
        /// <param name="seqAcao">Sequencial da Ação</param>
        /// <param name="seqProcesso">Sequencial Processo</param>
        /// <param name="seqIncricao">Sequencial da Inscrição</param>
        public DadosFormularioSeminarioData DadosFormularioSeminarioSGF(long seqAcao, long seqProcesso, long seqIncricao)
        {
            return InscricaoDomainService.DadosFormularioSeminarioSGF(seqAcao, seqProcesso, seqIncricao).Transform<DadosFormularioSeminarioData>();
        }

        /// <summary>
        /// Valida se por ventura o formulario é consistente para proseguir
        /// Incluir no botão “Próximo” a regra “RN_INS_187 - Consistência seminários de iniciação científica”:
        /// Se o tipo de processo em questão estiver configurado para integrar com o GPC 
        /// e existir o campo PROJETO no formulário, verificar se existe alguma outra 
        /// inscrição para o processo em questão, com a situação atual da inscrição 
        /// igual a INSCRICAO_FINALIZADA ou INSCRICAO_CONFIRMADA, e com o mesmo projeto 
        /// selecionado.Em caso afirmativo, abortar a operação e emitir a mensagem de erro:
        ///"Já existe uma inscrição para o projeto 'nome do projeto'".
        /// </summary>
        /// <param name="seqProcesso">Sequencial do proceesso no GPI</param>
        /// <param name="seqInscricao">Sequencial da Inscrição</param>
        /// <param name="descricaoProjeto">Descrição do Projeto GPC</param>
        public void ValidarFormularioSeminario(long seqProcesso, long seqInscricao, string descricaoProjeto)
        {
            InscricaoDomainService.ValidarFormularioSeminario(seqProcesso, seqInscricao, descricaoProjeto);
        }

        public void IniciarPortfolio(PortfolioData dadosPortifolio)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Buscar situação atual da inscrição
        /// </summary>
        /// <param name="seqInscricao">Sequencial da inscricao</param>
        /// <returns>Token de segurança</returns>
        public string BuscarSituacaoAtualInscricao(long seqInscricao)
        {
            return InscricaoDomainService.BuscarSituacaoAtualInscricao(seqInscricao);
        }

        public void AlterarAptoBolsa(long seqInscricao, bool apto)
        {

            InscricaoDomainService.AlterarAptoBolsa(seqInscricao, apto);
        }

        public string BuscarNomeInscritosSeqInscricao(long seqInscricao)
        {
            return InscricaoDomainService.BuscarNomeInscritosSeqInscricao(seqInscricao);
        }

        /// <summary>
        /// Buscar ingressos
        /// </summary>
        /// <param name="seqInscricao">Sequencial da Inscricao</param>
        /// <returns>Retorna os ingresso da Inscricao</returns>
        public IngressoData BuscarIngressos(long seqInscricao)
        {
            return InscricaoDomainService.BuscarIngressos(seqInscricao).Transform<IngressoData>();
        }

        public JustificativaSituacaoInscricaoData BuscarJustificativaSituacao(long seqInscricao)
        {
            return InscricaoDomainService.BuscarJustificativaSituacao(seqInscricao).Transform<JustificativaSituacaoInscricaoData>();
        }
        public bool PossuiDocumentoRequerido(long seqConfiguracaoEtapa)
        {
            return InscricaoDomainService.PossuiDocumentoRequerido(seqConfiguracaoEtapa);
        }

        public string BuscarDescricaoProcessoInscricao(long seqInscricao)
        {
            return InscricaoDomainService.BuscarDescricaoProcessoInscricao(seqInscricao);
        }

        public byte[] EmitirDocumentacao(long seqInscricao, long seqTipoDocumento)
        {
            return InscricaoDomainService.EmitirDocumentacao(seqInscricao, seqTipoDocumento);
        }
    }
}