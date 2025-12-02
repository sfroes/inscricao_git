using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Financeiro.ServiceContract.BLT;
using SMC.Financeiro.ServiceContract.BLT.Data;
using SMC.Formularios.UI.Mvc.Model;
using SMC.Formularios.UI.Mvc.Models;
using SMC.Framework;
using SMC.Framework.Exceptions;
using SMC.Framework.Extensions;
using SMC.Framework.Fake.Extensions;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc.Controllers.Service;
using SMC.Framework.Util;
using SMC.GPI.Inscricao.Areas.INS.Models;
using SMC.GPI.Inscricao.Areas.INS.Views.Inscricao.App_LocalResources;
using SMC.GPI.Inscricao.Models;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using SMC.Inscricoes.Common.Areas.INS.Exceptions;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;

namespace SMC.GPI.Inscricao.Areas.INS.Services
{
    public class InscricaoControllerService : SMCControllerServiceBase
    {
        #region Service

        private IInscritoService InscritoService => this.Create<IInscritoService>();

        private IInscricaoService InscricaoService
        {
            get { return this.Create<IInscricaoService>(); }
        }

        private IDocumentoRequeridoService DocumentoRequeridoService
        {
            get { return this.Create<IDocumentoRequeridoService>(); }
        }

        private IArquivoAnexadoService ArquivoAnexado
        {
            get { return this.Create<IArquivoAnexadoService>(); }
        }

        private IOfertaService OfertaService
        {
            get { return this.Create<IOfertaService>(); }
        }

        private IProcessoService ProcessoService
        {
            get { return this.Create<IProcessoService>(); }
        }

        private IIntegracaoFinanceiroService FinanceiroService
        {
            get { return this.Create<IIntegracaoFinanceiroService>(); }
        }

        private ITipoProcessoService TipoProcessoService { get => Create<ITipoProcessoService>(); }

        private IConfiguracaoEtapaService ConfiguracaoEtapaService => Create<IConfiguracaoEtapaService>();
        private IGrupoTaxaService GrupoTaxaService => Create<IGrupoTaxaService>();

        #endregion Service

        #region ControllerService

        private InscritoControllerService InscritoControllerService
        {
            get { return this.Create<InscritoControllerService>(); }
        }

        private ProcessoControllerService ProcessoControllerService
        {
            get { return this.Create<ProcessoControllerService>(); }
        }

        #endregion ControllerService

        #region Regras de Negócio

        /// <summary>
        /// Verificar a permissão para um inscrito iniciar ou continuar uma inscrição
        /// </summary>
        /// <param name="filtro">Filtro para verificar regra</param>
        /// <returns>TRUE caso é permitido iniciar ou continuar a inscrição, FALSE caso contrário</returns>
        public bool VerificarPermissaoIniciarContinuarInscricao(IniciarContinuarInscricaoFiltroViewModel filtro)
        {
            IniciarContinuarInscricaoFiltroData filtroData = SMCMapperHelper.Create<IniciarContinuarInscricaoFiltroData>(filtro);
            return InscricaoService.VerificarPermissaoIniciarContinuarInscricao(filtroData);
        }

        #endregion Regras de Negócio

        #region Buscar Inscrições

        /// <summary>
        /// Buscar as inscrições de um inscrito
        /// </summary>
        /// <param name="filtro">Filtro para pesquisa</param>
        /// <returns>Lista de inscrições do inscrito</returns>
        public SMCPagerModel<InscricaoListaViewModel> BuscarInscricoes(InscricoesFiltroViewModel filtro)
        {
            InscricaoFiltroData filtroData = SMCMapperHelper.Create<InscricaoFiltroData>(filtro);
            var inscricoesData = this.InscricaoService.BuscarInscricoesProcesso(filtroData);
            var pagerData = SMCMapperHelper.Create<SMCPagerData<InscricaoListaViewModel>>(inscricoesData);
            return new SMCPagerModel<InscricaoListaViewModel>(pagerData, filtro.PageSettings, filtro);
        }

        /// <summary>
        /// Busca as inscrições de um inscrito em um processo
        /// </summary>
        /// <param name="filtro">Filtro para pesquisa</param>
        /// <returns>Lista de inscrições do inscrito em um processo</returns>
        public SMCPagerModel<InscricaoProcessoItemViewModel> BuscarInscricoesProcesso(InscricoesFiltroViewModel filtro)
        {
            InscricaoFiltroData filtroData = SMCMapperHelper.Create<InscricaoFiltroData>(filtro);
            SMCPagerData<InscricoesProcessoData> inscricoesData = this.InscricaoService.BuscarInscricoesProcesso(filtroData);
            if (inscricoesData.Itens.Count == 1)
            {
                List<InscricaoProcessoItemViewModel> lista = inscricoesData.Itens.First().Inscricoes.TransformList<InscricaoProcessoItemViewModel>();
                return new SMCPagerModel<InscricaoProcessoItemViewModel>(lista, filtro.PageSettings, filtro);
            }
            else
            {
                return new SMCPagerModel<InscricaoProcessoItemViewModel>();
            }
        }

        #endregion Buscar Inscrições

        #region Buscar Informações de Páginas

        /// <summary>
        /// Buscar informações da primeira página
        /// </summary>
        /// <param name="seqConfiguracaoEtapa">Sequencial da configuração de etapa</param>
        /// <returns>Informações da primeira página do processo</returns>
        public ConfiguracaoEtapaPaginaViewModel BuscarConfiguracaoEtapaPrimeiraPagina(long seqConfiguracaoEtapa)
        {
            ConfiguracaoEtapaPaginaData data = InscricaoService.BuscarConfiguracaoEtapaPrimeiraPagina(seqConfiguracaoEtapa);
            return SMCMapperHelper.Create<ConfiguracaoEtapaPaginaViewModel>(data);
        }

        /// <summary>
        /// Buscar informações da página uma página
        /// </summary>
        /// <param name="seqConfiguracaoEtapa">Sequencial da configuração de etapa</param>
        /// <param name="tokenPagina">Tokena da página a ser recuperada</param>
        /// <returns>Informações da página de instruções finais</returns>
        public ConfiguracaoEtapaPaginaViewModel BuscarConfiguracaoEtapaPagina(long seqConfiguracaoEtapa, string tokenPagina)
        {
            var data = InscricaoService.BuscarConfiguracaoEtapaPagina(seqConfiguracaoEtapa, tokenPagina);
            return SMCMapperHelper.Create<ConfiguracaoEtapaPaginaViewModel>(data);
        }

        /// <summary>
        /// Busca as informações da ultima página acessada por uma inscrição
        /// </summary>
        /// <param name="seqInscricao">Sequencial da inscrição</param>
        /// <returns>Informações da ultima página acessada em uma inscrição</returns>
        public ContinuarInscricaoViewModel BuscarUltimaPaginaInscricao(long seqInscricao)
        {
            ContinuarInscricaoData data = InscricaoService.BuscarUltimaPaginaInscricao(seqInscricao);
            return SMCMapperHelper.Create<ContinuarInscricaoViewModel>(data);
        }

        /// <summary>
        /// Busca as informações de uma página
        /// </summary>
        /// <param name="filtro">Filtro para pagina</param>
        /// <returns>Informações da página</returns>
        private TPagina BuscarPagina<TPagina>(PaginaFiltroViewModel filtro)
            where TPagina : PaginaViewModel, new()
        {
            // Monta o ViewModel de filtro
            PaginaFiltroData filtroData = new PaginaFiltroData()
            {
                SeqConfiguracaoEtapaPagina = filtro.SeqConfiguracaoEtapaPagina,
                SeqGrupoOferta = filtro.SeqGrupoOferta,
                SeqInscricao = filtro.SeqInscricao,
                Idioma = filtro.Idioma
            };

            // Busca os dados da página
            PaginaData dataPagina = InscricaoService.BuscarPagina(filtroData);

            // Faz o mapeamento em ViewModel
            var paginaViewModel = SMCMapperHelper.Create<TPagina>(dataPagina);

            // Faz o mapeamento manual de seções e fluxos pois o mapper não sabe como mapear
            paginaViewModel.Secoes = new List<ITemplateSecaoPagina>();
            if (dataPagina != null && dataPagina.Secoes != null)
            {
                foreach (var item in dataPagina.Secoes)
                {
                    var secao = SMCMapperHelper.Create<TemplateSecaoPaginaViewModel>(item);
                    paginaViewModel.Secoes.Add(secao);
                }
            }

            paginaViewModel.FluxoPaginas = new List<ITemplateFluxoPagina>();
            if (dataPagina != null && dataPagina.FluxoPaginas != null)
            {
                foreach (var item in dataPagina.FluxoPaginas)
                {
                    var fluxo = SMCMapperHelper.Create<FluxoPaginaViewModel>(item);
                    paginaViewModel.FluxoPaginas.Add(fluxo);
                }
            }
            return paginaViewModel;
        }

        /// <summary>
        /// Busca os dados da página de instruções iniciais de uma configuração de etapa
        /// </summary>
        /// <param name="filtro">Filtro para página</param>
        /// <returns>Dados da página de instruções iniciais</returns>
        public PaginaInstrucaoInicialViewModel BuscarPaginaInstrucoesIniciais(PaginaFiltroViewModel filtro)
        {
            long? seqInscrito = InscritoControllerService.BuscarSeqInscritoLogado();

            var pagina = BuscarPagina<PaginaInstrucaoInicialViewModel>(filtro);


            return pagina;
        }

        /// <summary>
        /// Busca os dados da página de confirmação de dados do inscrito, bem como os dados do inscrito logado
        /// </summary>
        /// <param name="filtro">Filtro para página</param>
        /// <returns>Dados da página de dados do inscrito</returns>
        public PaginaConfirmarDadosInscritoViewModel BuscarPaginaDadosInscrito(PaginaFiltroViewModel filtro)
        {
            // Busca os dados da página
            PaginaConfirmarDadosInscritoViewModel pagina = BuscarPagina<PaginaConfirmarDadosInscritoViewModel>(filtro);

            // Busca os dados do inscrito
            pagina.DadosInscrito = InscritoControllerService.BuscarDadosInscrito();

            return pagina;
        }

        /// <summary>
        /// Busca os dados da página de seleção de oferta
        /// </summary>
        /// <param name="filtro">Filtro para página</param>
        /// <returns>Dados da página de seleção de oferta</returns>
        public PaginaSelecaoOfertaViewModel BuscarPaginaSelecaoOferta(PaginaFiltroViewModel filtro)
        {
            // Busca os dados da página
            PaginaSelecaoOfertaViewModel pagina = BuscarPagina<PaginaSelecaoOfertaViewModel>(filtro);

            // Busca as ofertas da inscrição
            pagina.Ofertas = InscricaoService.BuscarInscricaoOfertas(filtro.SeqInscricao)
                                                .TransformMasterDetailList<InscricaoOfertaViewModel>();
            pagina.Taxas = this.BuscarTaxasOfertaInscricao(filtro.SeqInscricao);
            pagina.LabelOferta = this.ProcessoControllerService.BuscarDescricaoOfertaProcesso(filtro.SeqConfiguracaoEtapa, pagina.Idioma);
            pagina.CobrancaPorOferta = pagina.Taxas?.Any(t => t.CobrarPorOferta.HasValue && t.CobrarPorOferta.Value) ?? false;
            pagina.TipoCobrancaPorQuantidadeOferta = pagina.Taxas?.Any(t => t.TipoCobranca == TipoCobranca.PorQuantidadeOfertas) ?? false;
            pagina.BolsaExAluno = TipoProcessoService.BuscarTipoProcessoPorInscricao(filtro.SeqInscricao).BolsaExAluno;

            pagina.GruposTaxa = this.GrupoTaxaService.BuscarGruposTaxaPorSeqProcesso(pagina.SeqProcesso);
            // Carrega o número de opções informadas pelo usuário.
            pagina.NumeroOpcoesDesejadas = InscricaoService.BuscarInscricaoResumida(filtro.SeqInscricao).NumeroOpcoesDesejadas;

            pagina.PossuiBoletoPago = this.InscricaoService.PossuiBoletoPago(filtro.SeqInscricao);
            pagina.ProcessoPossuiTaxa = this.ProcessoService.ProcessoPossuiTaxa(pagina.SeqProcesso);

            // Verifica se não existe nenhuma oferta selecionada
            if (pagina.Ofertas.Count == 0)
            {
                // Busca as ofertas vigentes do processo
                var seqOfertas = BuscarSeqOfertasVigentesAtivas(filtro.SeqGrupoOferta);
                if (seqOfertas.Count == 1)
                {
                    pagina.Ofertas.Add(new InscricaoOfertaViewModel()
                    {
                        SeqOferta = new Inscricoes.UI.Mvc.Areas.INS.Lookups.GPILookupViewModel()
                        {
                            Seq = seqOfertas.First()
                        }
                    });

                    // Se existir apenas uma oferta e a taxa estiver vigente, define os campos automaticamente
                    var taxas = this.BuscarTaxasInscricaoOfertaVigentes(seqOfertas.First(), filtro.SeqInscricao);
                    if (taxas != null && taxas.Count > 0)
                    {
                        pagina.Taxas = taxas;
                    }
                }
            }

            if (pagina.BolsaExAluno)
            {
                foreach (var ofertaView in pagina.Ofertas)
                {
                    ofertaView.MensagemOferta = UIResource.MensagemBolsaExAluno;
                    if (ofertaView.SeqOferta != null && ofertaView.SeqOferta.Seq.GetValueOrDefault() > 0)
                    {
                        ofertaView.ExibirMensagemOferta = ValidarBolsaExAlunoOferta(ofertaView.SeqOferta.Seq.Value, filtro.SeqInscricao);
                    }
                }
                pagina.Ofertas.DefaultModel = new InscricaoOfertaViewModel { MensagemOferta = UIResource.MensagemBolsaExAluno };
            }

            return pagina;
        }

        /// <summary>
        /// Busca os dados da página de codigo de  autorização para uma inscrição
        /// </summary>
        /// <param name="filtro">Filtro para buscar página</param>
        /// <returns>Dados da página de código de autorização</returns>
        public PaginaCodigoAutorizacaoViewModel BuscarPaginaCodigosAutorizacao(PaginaFiltroViewModel filtro)
        {
            // Se não foi informada uma inscrição erro
            if (filtro.SeqInscricao <= 0)
                throw new InscricaoInvalidaException();

            // Busca os dados da página
            PaginaCodigoAutorizacaoViewModel pagina = BuscarPagina<PaginaCodigoAutorizacaoViewModel>(filtro);

            // Busca os códigos de autorização da inscrição
            List<InscricaoCodigoAutorizacaoData> codigos = InscricaoService.BuscarInscricaoCodigoAutorizacao(filtro.SeqInscricao);
            pagina.CodigosAutorizacao = codigos.TransformMasterDetailList<InscricaoCodigoAutorizacaoViewModel>();

            return pagina;
        }

        /// <summary>
        /// Busca as informações do formulário para inscrição
        /// </summary>
        /// <param name="filtro">Filtro da página</param>
        /// <returns>Dados da página de formulários da inscrição</returns>
        public PaginaFormularioInscricaoViewModel BuscarPaginaFormularioInscricao(PaginaFiltroViewModel filtro)
        {
            // Se não foi informada uma inscrição erro
            if (filtro.SeqInscricao <= 0)
                throw new InscricaoInvalidaException();

            // Busca os dados da página
            PaginaFormularioInscricaoViewModel pagina = BuscarPagina<PaginaFormularioInscricaoViewModel>(filtro);

            // Se não indentificou o SeqFormulario e SeqVisao, erro
            if (!pagina.SeqFormularioSGF.HasValue || !pagina.SeqVisaoSGF.HasValue)
                throw new SMCApplicationException("Não foi encontrado o formulário ou a visão para esta inscrição.");

            // Verifica se o usuário já preencheu o formulário. Se sim, carrega o formulário preenchido pelo usuário,
            // ao invés do formulário da inscrição.
            InscricaoDadoFormularioFiltroData filtroData = InscricaoService.BuscarFormularioInscrito(filtro.SeqInscricao, filtro.SeqConfiguracaoEtapaPagina);

            if (filtroData == null)
            {
                // Busca o dado formulário correspondente ao formulário da página para a inscrição
                filtroData = new InscricaoDadoFormularioFiltroData()
                {
                    SeqInscricao = pagina.SeqInscricao,
                    SeqFormulario = pagina.SeqFormularioSGF.Value,
                    SeqVisao = pagina.SeqVisaoSGF.Value
                };

                pagina.SeqDadoFormulario = InscricaoService.BuscarSeqDadoFormulario(filtroData);
            }
            else
            {
                pagina.SeqFormularioSGF = filtroData.SeqFormulario;
                pagina.SeqVisaoSGF = filtroData.SeqVisao;
                pagina.SeqDadoFormulario = filtroData.SeqDadoFormulario;
            }

            return pagina;
        }

        public void PreencheEscolaSerieComprovanteQrCode(IngressoViewModel modelo, List<string> tokens)
        {
            modelo.DadosCampo = InscricaoService.BuscarCamposDadoFormularioPorSeqInscricao(modelo.SeqInscricao, tokens).TransformList<DadoCampoViewModel>();
        }

        /// <summary>
        /// Buscar os dados da página de upload de documentos
        /// </summary>
        /// <param name="filtro">Filtro da página</param>
        /// <returns>Dados da página de upload de documentos</returns>
        public PaginaUploadDocumentosViewModel BuscarPaginaUploadDocumentos(PaginaFiltroViewModel filtro)
        {
            // Se não foi informada uma inscrição erro
            if (filtro.SeqInscricao <= 0)
                throw new InscricaoInvalidaException();

            // Busca os dados da página
            PaginaUploadDocumentosViewModel pagina = BuscarPagina<PaginaUploadDocumentosViewModel>(filtro);

            //// Busca os documentos de uma inscrição
            //List<InscricaoDocumentoData> listaData = InscricaoService.BuscarListaInscricaoDocumentoOpcionaisUpload(filtro.SeqInscricao);
            //pagina.DocumentosOpcionais = listaData.TransformMasterDetailList<InscricaoDocumentoOpcionalViewModel>();

            var uploadData = this.InscricaoService.BuscarDocumentosUploadInscricao(filtro.SeqInscricao);

            pagina.DocumentosGrupo = uploadData.DocumentosEmGruposObrigatorios.TransformList<InscricaoDocumentoGrupoViewModel>();
            pagina.DocumentosObrigatorios = uploadData.DocumentosObrigatorios.TransformList<InscricaoDocumentoObrigatorioViewModel>();
            pagina.DocumentosOpcionais = uploadData.DocumentosOpcionais.TransformMasterDetailList<InscricaoDocumentoOpcionalViewModel>();
            pagina.DocumentosOpcionaisUpload = uploadData.TiposDocumentosOpcionais.TransformList<SMCDatasourceItem>();
            pagina.DocumentosAdicionais = uploadData.DocumentosAdicionais.TransformMasterDetailList<InscricaoDocumentoAdicionalViewModel>();
            pagina.DocumentosAdicionaisUpload = uploadData.TiposDocumentosAdicionais.TransformList<SMCDatasourceItem>();
            pagina.DescricaoTermoEntregaDocumentacao = uploadData.DescricaoTermoEntregaDocumentacao;
            pagina.ExibirMensagemInformativaConversaoPDF = uploadData.ExibirMensagemInformativaConversaoPDF;

            return pagina;
        }

        /// <summary>
        /// Busca os dados da página de confirmação de inscrição
        /// </summary>
        /// <param name="filtro">Filtro da página</param>
        /// <returns>Dados da página de confirmação de inscrição</returns>
        public PaginaConfirmarInscricaoViewModel BuscarPaginaConfirmarInscricao(PaginaFiltroViewModel filtro)
        {
            // Se não foi informada uma inscrição erro
            if (filtro.SeqInscricao <= 0)
                throw new InscricaoInvalidaException();

            // Busca os dados da página
            PaginaConfirmarInscricaoViewModel pagina = BuscarPagina<PaginaConfirmarInscricaoViewModel>(filtro);
            var inscricaoResumida = InscricaoService.BuscarInscricaoResumida(filtro.SeqInscricao);

            pagina.NumeroOpcoesDesejadas = inscricaoResumida.NumeroOpcoesDesejadas;
            pagina.DescricaoTermoEntregaDocumentacao = inscricaoResumida.DescricaoTermoEntregaDocumentacao;
            pagina.TermoAceiteConversaoArquivosPDF = inscricaoResumida.TermoAceiteConversaoArquivosPDF;
            pagina.OrientacaoAceiteConversaoArquivosPDF = inscricaoResumida.OrientacaoAceiteConversaoArquivosPDF;

            // Busca todas as ofertas da inscrição
            List<InscricaoOfertaData> listaOfertas = InscricaoService.BuscarInscricaoOfertas(filtro.SeqInscricao);
            pagina.Ofertas = listaOfertas.TransformList<InscricaoOfertaListaViewModel>();
            if (pagina.Ofertas.Count == 1)
            {
                pagina.LabelOferta = this.ProcessoControllerService.BuscarDescricaoOfertaProcesso(pagina.SeqConfiguracaoEtapa, pagina.Idioma);
            }

            // Busca as taxas para exibir na página
            pagina.Taxas = new SMCPagerModel<InscricaoTaxaViewModel>(this.BuscarTaxasOfertaInscricaoConfirmacao(filtro.SeqInscricao)?.OrderBy(t => t.Descricao));

            // Se a página exibe os códigos de autorização, busca os códigos da inscrição
            if (pagina.ExibeCodigosAutorizacao)
            {
                List<InscricaoCodigoAutorizacaoData> codigos = InscricaoService.BuscarInscricaoCodigoAutorizacao(filtro.SeqInscricao);
                pagina.CodigosAutorizacao = codigos.TransformList<InscricaoCodigoAutorizacaoViewModel>();
            }

            // Se a página exibe os documentos, busca os mesmos
            if (pagina.ExibeDocumentacao)
            {
                SituacaoEntregaDocumento[] situacoesDiferente = new SituacaoEntregaDocumento[] { SituacaoEntregaDocumento.Nenhum, SituacaoEntregaDocumento.AguardandoEntrega };
                List<InscricaoDocumentoData> listaData = InscricaoService.BuscarListaInscricaoDocumentoOpcionaisUpload(filtro.SeqInscricao, situacoesDiferente);
                pagina.Documentos = new SMCPagerModel<InscricaoDocumentoListaViewModel>(listaData.TransformList<InscricaoDocumentoListaViewModel>());
                pagina.ExibeTermoPrincipalResponsabilidadeEntrega = pagina.Documentos.Any(a => a.EntregaPosterior);
            }

            // Se a página exibe formulários, busca o sequencial do dado formulário para exibição
            // Busca do fluxo de páginas os formulários para exibição
            foreach (var fluxo in pagina.FluxoPaginas
                                        .Where(f => f.Token.Equals(TOKENS.PAGINA_FORMULARIO_INSCRICAO) && (f as FluxoPaginaViewModel).ExibeConfirmacaoInscricao)
                                        .OrderBy(f => f.Ordem))
            {
                InscricaoDadoFormularioFiltroData filtroData = new InscricaoDadoFormularioFiltroData()
                {
                    SeqInscricao = pagina.SeqInscricao,
                    SeqFormulario = fluxo.SeqFormularioSGF.Value,
                    SeqVisao = fluxo.SeqVisaoSGF.Value
                };
                long seqDadoFormulario = InscricaoService.BuscarSeqDadoFormulario(filtroData);
                pagina.Formularios.Add(new InscricaoDadoFormularioListaViewModel()
                {
                    SeqDadoFormulario = seqDadoFormulario,
                    SeqInscricao = pagina.SeqInscricao,
                    TituloFormulario = fluxo.Titulo,
                    SeqFormularioSGF = fluxo.SeqFormularioSGF.Value,
                    SeqVisaoSGF = fluxo.SeqVisaoSGF.Value
                });
            }

            // Recupera dados da LGPD
            long? seqInscrito = InscritoControllerService.BuscarSeqInscritoLogado();

            var dadosLGPD = InscritoService.BuscarInscritoLGPD(seqInscrito.Value, pagina.SeqProcesso);
            pagina.ExibeTermoConsentimentoLGPD = dadosLGPD.ExibeTermoConsentimentoLGPD;
            pagina.Idade = dadosLGPD.Idade;
            pagina.TermoLGPD = dadosLGPD.TermoLGPD;

            // Se o termo não estiver visível, o valor padrão deverá ser o mesmo que já estava salvo no banco.
            if (string.IsNullOrEmpty(pagina.TermoLGPD))
                pagina.ConsentimentoLGPD = dadosLGPD.ConsentimentoLGPD != null ? dadosLGPD.ConsentimentoLGPD.Value : false;

            //Se o Termo de consentimento LGPD estiver visível, o valor padrão deverá ser "Sim", independente do valor que já estiver registrado no banco de dados
            else
                pagina.ConsentimentoLGPD = true;

            // Gambiarra da Maira para que seja sempre true a flag caso seja o primeiro acesso a inscricao
            //var jaNavegouProximo = InscricaoService.VerificaInscricaoApenasPrimeiraPagina(filtro.SeqInscricao, filtro.SeqConfiguracaoEtapaPagina);
            //if (!jaNavegouProximo)
            //{
            //    if (dadosLGPD.ConsentimentoLGPD == true || (dadosLGPD.ConsentimentoLGPD == null && pagina.ExibeTermoConsentimentoLGPD == true))
            //    {
            //        pagina.ConsentimentoLGPD = true;
            //    }
            //    else
            //    {
            //        pagina.ConsentimentoLGPD = false;
            //    }
            //}
            //else
            //{
            //    pagina.ConsentimentoLGPD = dadosLGPD.ConsentimentoLGPD ?? false;
            //}

            return pagina;
        }

        /// <summary>
        /// Busca os dados da página de instruções finais da inscrição
        /// </summary>
        /// <param name="filtro">Filtro da página</param>
        /// <returns>Dados da página de instruções finais</returns>
        public PaginaInstrucoesFinaisViewModel BuscarPaginaInstrucoesFinais(PaginaFiltroViewModel filtro)
        {
            // Se não foi informada uma inscrição erro
            if (filtro.SeqInscricao <= 0)
                throw new InscricaoInvalidaException();

            // Busca os dados da página
            PaginaInstrucoesFinaisViewModel pagina = BuscarPagina<PaginaInstrucoesFinaisViewModel>(filtro);

            // Busca o sequencial do arquivo do comprovante
            pagina.SeqArquivoComprovante = InscricaoService.BuscarSeqArquivoComprovante(filtro.SeqInscricao);

            // TODO: Verificar taxas|
            pagina.PossuiTaxas = InscricaoService.VerificarExistenciaTaxaInscricao(filtro.SeqInscricao);

            var validaBotaoIngresso = ValidaBotaoIngressos(filtro.SeqInscricao);
            pagina.MensagemInformativaCheckin = validaBotaoIngresso.Item1;
            pagina.HabilitarBotaoCheckin = validaBotaoIngresso.Item2;
            pagina.MensagemImpressaoBoleto = InscricaoService.VerificaPermissaoGerarBoletoInscricao(filtro.SeqInscricao);
            pagina.MensagemEmissaoComprovante = InscricaoService.VerificarPermissaoEmitirComprovante(filtro.SeqInscricao);

            return pagina;
        }

        public (string, bool) ValidaBotaoIngressos(long seqInscricao)
        {
            string menssagem = string.Empty;
            bool habilitaCheckin = false;

            var inscricaoProcesso = InscricaoService.BuscarInscricoesProcesso(new InscricaoFiltroData() { SeqInscricao = seqInscricao }).FirstOrDefault();
            var inscricao = inscricaoProcesso.Inscricoes.FirstOrDefault();
            var possuiTaxas = InscricaoService.VerificarExistenciaTaxaInscricao(seqInscricao);
            var taxaPaga = InscricaoService.VerificarPagamentoTaxaInscricao(seqInscricao);
            var ProcessoPossuiDocumentacaoObrigatoria = InscricaoService.PossuiDocumentoRequerido(inscricao.SeqConfiguracaoEtapa);
            var situacoesTemplateProcesso = ProcessoService.BuscarSituacoesProcesso(inscricaoProcesso.SeqProcesso);            

 
            /*  Se houver boleto de inscrição para o inscrito e o título referente a esse boleto não possuir data de pagamento,
             *  exibir a mensagem "Pagamento de taxa em aberto."
             */
            if (possuiTaxas && (taxaPaga.HasValue && !taxaPaga.Value))
            {
                menssagem = UIResource.Mensagem_Informativa_Checkin_Possui_Boleto;
            }

            /* Se houver algum documento requerido obrigatório
              * OU grupo de documentação requerida 
              *  e a situação da entrega da documentação
              * na inscrição for diferente de "Entregue" ou "Entregue com pendência", exibir a mensagem "Documentação " + <descrição da situação
              * da documentação em caixa baixa> + "."
              */
            else if (ProcessoPossuiDocumentacaoObrigatoria && !inscricao.DocumentacaoEntregue &&
                     (inscricaoProcesso.Inscricoes.FirstOrDefault().SituacaoDocumentacao == SituacaoDocumentacao.EntregueComPendencia ||
                     inscricaoProcesso.Inscricoes.FirstOrDefault().SituacaoDocumentacao != SituacaoDocumentacao.Entregue))
            {

                menssagem = string.Format(UIResource.Mensagem_Informativa_Checkin_Possui_Documentacao_Pendente, inscricaoProcesso.Inscricoes.FirstOrDefault().SituacaoDocumentacao.SMCGetDescription().ToLower());


            }

            /* Se o template do processo em questão possuir deferimento de inscrição e a situação atual da inscrição estiver com a
             * situação INSCRICAO_CONFIRMADA, exibir a mensagem "Aguardando deferimento da inscrição."
             */
            else if (situacoesTemplateProcesso.Contains(TOKENS.SITUACAO_INSCRICAO_DEFERIDA) && inscricao.TokenSituacaoAtual == TOKENS.SITUACAO_INSCRICAO_CONFIRMADA)
            {
                menssagem = UIResource.Mensagem_Informativa_Checkin_Possui_Deferimento_Inscricao;
            }

            /*Se a inscrição estiver com a situação INSCRICAO_INDEFERIDA ou INSCRICAO_CANCELADA, exibir a mensagem: "Ingresso indisponível.
             *<Descrição da situação atual da inscrição>."
             */
            else if (inscricao.TokenSituacaoAtual == TOKENS.SITUACAO_INSCRICAO_INDEFERIDA || inscricao.TokenSituacaoAtual == TOKENS.SITUACAO_INSCRICAO_CANCELADA)
            {
                menssagem = string.Format(UIResource.Mensagem_Informativa_Checkin_Possui_Situacao_Indeferida_Cancelada, inscricaoProcesso.Inscricoes.FirstOrDefault().DescricaoSituacaoAtual.ToLower().Capitalise());
            }
            else
            {
                menssagem = Inscricao.Views.Home.App_LocalResources.UIResource.Mensagem_Informativa_Checkin_Habilitada;
                habilitaCheckin = true;
            }

            return (menssagem, habilitaCheckin);
        }


        /// <summary>
        /// Buscar os dados da página de comprovante
        /// </summary>
        /// <param name="filtro">Filtro da página</param>
        /// <param name="seqInscrito">Sequencial do inscrito. Caso não seja informado, assume como o inscrito logado</param>
        /// <returns>Dados da página de comprovante</returns>
        public PaginaComprovanteInscricaoViewModel BuscarPaginaComprovanteInscricao(PaginaFiltroViewModel filtro, long? seqInscrito = null)
        {
            // Se não foi informada uma inscrição erro
            if (filtro.SeqInscricao <= 0)
                throw new InscricaoInvalidaException();

            // Busca os dados da página
            PaginaComprovanteInscricaoViewModel pagina = BuscarPagina<PaginaComprovanteInscricaoViewModel>(filtro);

            // Busca os dados do inscrito
            pagina.DadosInscrito = InscritoControllerService.BuscarDadosInscrito(seqInscrito);

            var inscricaoResumida = InscricaoService.BuscarInscricaoResumida(filtro.SeqInscricao);
            pagina.NumeroOpcoesDesejadas = inscricaoResumida.NumeroOpcoesDesejadas;
            pagina.DescricaoTermoEntregaDocumentacao = inscricaoResumida.DescricaoTermoEntregaDocumentacao;
            pagina.TermoAceiteConversaoArquivosPDF = inscricaoResumida.TermoAceiteConversaoArquivosPDF;
            pagina.OrientacaoAceiteConversaoArquivosPDF = inscricaoResumida.OrientacaoAceiteConversaoArquivosPDF;
            pagina.GestaoEventos = inscricaoResumida.GestaoEventos;
            pagina.UidInscricaoOferta = inscricaoResumida.UidInscricaoOferta;

            // Busca todas as ofertas da inscrição
            List<InscricaoOfertaData> listaOfertas = InscricaoService.BuscarInscricaoOfertas(filtro.SeqInscricao);
            pagina.Ofertas = listaOfertas.TransformList<InscricaoOfertaListaViewModel>();

            // Se a página exibe os códigos de autorização, busca os códigos da inscrição
            if (pagina.ExibeCodigosAutorizacao)
            {
                List<InscricaoCodigoAutorizacaoData> codigos = InscricaoService.BuscarInscricaoCodigoAutorizacao(filtro.SeqInscricao);
                pagina.CodigosAutorizacao = codigos.TransformList<InscricaoCodigoAutorizacaoViewModel>();
            }

            // Se a página exibe os documentos, busca os mesmos
            if (pagina.ExibeDocumentacao)
            {
                SituacaoEntregaDocumento[] situacoesDiferente = new SituacaoEntregaDocumento[] { SituacaoEntregaDocumento.Nenhum, SituacaoEntregaDocumento.AguardandoEntrega };
                List<InscricaoDocumentoData> listaData = InscricaoService.BuscarListaInscricaoDocumentoOpcionaisUpload(filtro.SeqInscricao, situacoesDiferente);
                pagina.Documentos = new SMCPagerModel<InscricaoDocumentoListaViewModel>(listaData.TransformList<InscricaoDocumentoListaViewModel>());
                pagina.ExibeTermoPrincipalResponsabilidadeEntrega = pagina.Documentos.Any(a => a.EntregaPosterior);
            }

            // Se a página exibe formulários, busca o sequencial do dado formulário para exibição
            // Busca do fluxo de páginas os formulários para exibição
            foreach (var fluxo in pagina.FluxoPaginas
                                        .Where(f => f.Token.Equals(TOKENS.PAGINA_FORMULARIO_INSCRICAO) && (f as FluxoPaginaViewModel).ExibeComprovanteInscricao)
                                        .OrderBy(f => f.Ordem))
            {
                InscricaoDadoFormularioFiltroData filtroData = new InscricaoDadoFormularioFiltroData()
                {
                    SeqInscricao = pagina.SeqInscricao,
                    SeqFormulario = fluxo.SeqFormularioSGF.Value,
                    SeqVisao = fluxo.SeqVisaoSGF.Value
                };
                long seqDadoFormulario = InscricaoService.BuscarSeqDadoFormulario(filtroData);
                pagina.Formularios.Add(new InscricaoDadoFormularioListaViewModel()
                {
                    SeqDadoFormulario = seqDadoFormulario,
                    SeqInscricao = pagina.SeqInscricao,
                    TituloFormulario = fluxo.Titulo,
                    SeqFormularioSGF = fluxo.SeqFormularioSGF.Value,
                    SeqVisaoSGF = fluxo.SeqVisaoSGF.Value
                });
            }

            bool? exibeDadosPessoais = ConfiguracaoEtapaService.BuscarConfiguracaoEtapaPaginaEdicao(filtro.SeqConfiguracaoEtapaPagina).ExibeDadosPessoais;

            pagina.ExibeDadosPessoais = exibeDadosPessoais.HasValue ? exibeDadosPessoais.Value : false;

            return pagina;
        }

        public string BuscarUrlCss(long seqInscricao)
        {
            var urlCss = InscricaoService.BuscarUrlCss(seqInscricao);
            return urlCss;
        }

        #endregion Buscar Informações de Páginas

        #region Inclusao/Alteração inscrição

        /// <summary>
        /// Inclui uma inscrição com sua situação
        /// </summary>
        /// <param name="modelo">Modelo para inclusão de inscrição</param>
        /// <returns>Sequencial da inscrição criada</returns>
        public long IncluirInscricao(PaginaViewModel modelo)
        {
            // Busca o sequencial do inscrito do usuário logado
            long? seqInscrito = InscritoControllerService.BuscarSeqInscritoLogado();
            if (!seqInscrito.HasValue)
                throw new InscritoInvalidoException();

            // Cria o InscricaoInicialData
            InscricaoInicialData data = SMCMapperHelper.Create<InscricaoInicialData>(modelo);
            data.SeqInscrito = seqInscrito.Value;

            return InscricaoService.IncluirInscricao(data);
        }

        /// <summary>
        /// Inclui um historico de acesso a pagina
        /// </summary>
        /// <param name="modelo">Modelo para inclusão de historico de página</param>
        public void IncluirHistoricoPagina(PaginaViewModel modelo)
        {
            InscricaoService.IncluirInscricaoHistoricoPagina(SMCMapperHelper.Create<InscricaoHistoricoPaginaData>(modelo));
        }

        /// <summary>
        /// Salva uma inscrição com suas ofertas
        /// </summary>
        /// <param name="modelo">Modelo com os dados para a inscrição</param>
        /// <returns>Sequencial da inscrição criada</returns>
        public void SalvarInscricaoOferta(PaginaSelecaoOfertaViewModel modelo)
        {
            // Preenche o Data com o sequencial da inscrição. Remove as ofertas que não foram selecionadas da lista.
            List<InscricaoOfertaData> lista = modelo.Ofertas.Where(o => o.SeqOferta != null && o.SeqOferta.Seq != 0).TransformList<InscricaoOfertaData>();
            foreach (var item in lista)
            {
                item.SeqInscricao = modelo.SeqInscricao;
            }

            //Gambiarra, só serve para um processo em especifico, OBTENÇÃO DE NOVO TÍTULO. 
            //De acordo com a Maira, quando esse processo permitir mais de uma oferta, as regras precisam ser revalidadas, 
            //atualmente ele tem somente uma unica oferta.
            if (TipoProcessoService.BuscarTipoProcessoPorProcesso(modelo.SeqProcesso).BolsaExAluno && modelo.Ofertas.Count() == 1)
            {
                InscricaoService.AlterarAptoBolsa(modelo.SeqInscricao, modelo.Ofertas.FirstOrDefault().ExibirMensagemOferta);
            }
            else if (!TipoProcessoService.BuscarTipoProcessoPorProcesso(modelo.SeqProcesso).BolsaExAluno && modelo.Ofertas.Count() == 1)
            {
                InscricaoService.AlterarAptoBolsa(modelo.SeqInscricao, modelo.Ofertas.FirstOrDefault().ExibirMensagemOferta);
            }

            // Chama o serviço para salvar as ofertas da inscrição
            InscricaoService.SalvarInscricaoOferta(lista, modelo.NumeroOpcoesDesejadas, modelo.Taxas?.TransformList<InscricaoTaxaOfertaData>());
        }

        /// <summary>
        /// Salva uma inscrição com suas ofertas
        /// </summary>
        /// <param name="modelo">Modelo com os dados para a inscrição</param>
        /// <returns>Sequencial da inscrição criada</returns>
        public void SalvarInscricaoOfertaAngular(PaginaSelecaoOfertaViewModel modelo)
        {
            // Preenche o Data com o sequencial da inscrição. Remove as ofertas que não foram selecionadas da lista.
            List<InscricaoOfertaData> lista = modelo.Ofertas.Where(o => o.SeqOferta != null && o.SeqOferta.Seq != 0).TransformList<InscricaoOfertaData>();
            foreach (var item in lista)
            {
                item.SeqInscricao = modelo.SeqInscricao;
            }

            //Gambiarra, só serve para um processo em especifico, OBTENÇÃO DE NOVO TÍTULO. 
            //De acordo com a Maira, quando esse processo permitir mais de uma oferta, as regras precisam ser revalidadas, 
            //atualmente ele tem somente uma unica oferta.
            if (TipoProcessoService.BuscarTipoProcessoPorProcesso(modelo.SeqProcesso).BolsaExAluno && modelo.Ofertas.Count() == 1)
            {
                InscricaoService.AlterarAptoBolsa(modelo.SeqInscricao, modelo.Ofertas.FirstOrDefault().ExibirMensagemOferta);
            }
            else if (!TipoProcessoService.BuscarTipoProcessoPorProcesso(modelo.SeqProcesso).BolsaExAluno && modelo.Ofertas.Count() == 1)
            {
                InscricaoService.AlterarAptoBolsa(modelo.SeqInscricao, modelo.Ofertas.FirstOrDefault().ExibirMensagemOferta);
            }

            // Chama o serviço para salvar as ofertas da inscrição
            InscricaoService.SalvarInscricaoOfertaAngular(lista, modelo.NumeroOpcoesDesejadas, modelo.SeqGrupoOferta, modelo.Taxas?.TransformList<InscricaoTaxaOfertaData>());
        }

        public bool VerificaBoletoInscricaoAlteracaoTaxa(long seqInscricao, List<InscricaoTaxaViewModel> taxas)
        {
            return this.InscricaoService.VerificaBoletoInscricaoAlteracaoTaxa(seqInscricao, taxas.TransformList<InscricaoTaxaData>());
        }

        ///// <summary>
        ///// Salva as taxas para uma inscrição
        ///// </summary>
        //public void SalvarInscricaoTaxasOferta(long seqInscricao, List<InscricaoTaxaViewModel> inscricaoTaxasOferta)
        //{
        //    InscricaoService.SalvarInscricaoTaxasOferta(seqInscricao, inscricaoTaxasOferta.
        //        TransformList<InscricaoTaxaOfertaData>());
        //}

        /// <summary>
        /// Salva as informações de código de autorização para uma inscrição
        /// </summary>
        /// <param name="modelo">Codigos de autorização a serem salvos</param>
        public void SalvarInscricaoCodigosAutorizacao(PaginaCodigoAutorizacaoViewModel modelo)
        {
            // Retira do modelo os códigos que não foram informados
            List<InscricaoCodigoAutorizacaoViewModel> lista = modelo.CodigosAutorizacao.Where(c => !string.IsNullOrEmpty(c.Codigo)).ToList();

            // Preenche o Data com o sequencial da inscrição
            List<InscricaoCodigoAutorizacaoData> listaData = lista.TransformList<InscricaoCodigoAutorizacaoData>();
            foreach (var item in listaData)
            {
                item.SeqInscricao = modelo.SeqInscricao;
            }

            // Chama o serviço para salvar os códigos da inscrição
            InscricaoService.SalvarInscricaoCodigosAutorizacao(modelo.SeqInscricao, listaData);
        }

        /// <summary>
        /// Salva os documentos de uma inscrição
        /// </summary>
        /// <param name="modelo">DocumentosOpcionais a serem salvos</param>
        public void SalvarUploadDocumentos(PaginaUploadDocumentosViewModel modelo)
        {
            // Cria o Data com os documentos para upload
            List<InscricaoDocumentoData> listaData = modelo.DocumentosOpcionais.TransformList<InscricaoDocumentoData>();
            listaData.AddRange(modelo.DocumentosObrigatorios.TransformList<InscricaoDocumentoData>());
            listaData.AddRange(modelo.DocumentosGrupo.TransformList<InscricaoDocumentoData>());
            listaData.AddRange(modelo.DocumentosAdicionais.TransformList<InscricaoDocumentoData>());
            foreach (var item in listaData)
            {
                item.SeqInscricao = modelo.SeqInscricao;
                item.DataPrazoEntrega = item.DataLimiteEntrega;
            }

            // Chama o serviço para salvar os documentos da inscriçao
            InscricaoService.SalvarInscricaoDocumentoUpload(modelo.SeqInscricao, listaData);
        }

        /// <summary>
        /// Finaliza uma inscrição
        /// </summary>
        /// <param name="seqInscricao">Sequencial da inscrição a ser Finalizada</param>
        /// <param name="arquivoComprovante">Conteudo do arquivo de comprovante</param>
        /// <param name="aceiteConversaoPDF">Aceite do termo conversão do PDF</param>
        public void FinalizarInscricao(long seqInscricao, bool aceiteConversaoPDF, bool ConcentimentoLGPD, byte?[] arquivoComprovante)
        {
            InscricaoService.FinalizarInscricao(seqInscricao, aceiteConversaoPDF, ConcentimentoLGPD, arquivoComprovante);
        }

        #endregion Inclusao/Alteração inscrição

        #region Buscar / Persistir Dado Formulario

        /// <summary>
        /// Busca um dado formulário
        /// </summary>
        /// <param name="seqDadoFormulario">Sequencial do dado formulário a ser recuperado</param>
        /// <returns>Dado formulário</returns>
        public InscricaoDadoFormularioViewModel BuscarDadoFormulario(long seqDadoFormulario)
        {
            var dados = InscricaoService.BuscarDadoFormulario(seqDadoFormulario);
            return SMCMapperHelper.Create<InscricaoDadoFormularioViewModel>(dados);
        }

        /// <summary>
        /// Salva os dados do formulário
        /// </summary>
        /// <param name="dados">Modelo com os dados do formulário.</param>
        public void SalvarFormularioInscricao(InscricaoDadoFormularioViewModel dados, long seqInscricao)
        {
            // Verifica consistência de legitimidade fiador
            if (TipoProcessoService.VerificaPossuiConsistencia(new TipoProcessoConsistenciaData() { SeqInscricao = seqInscricao, TipoConsistencia = TipoConsistencia.LegitimidadeFiador }))
            {
                DadosInscritoViewModel inscrito = InscritoControllerService.BuscarDadosInscrito();
                var cpf = dados.DadosCampos.FirstOrDefault(f => f.TokenElemento == TOKENS.CAMPO_AVALISTA_FIADOR_CPF).Valor;
                if (cpf != null)
                {
                    if (inscrito.Cpf == cpf.SMCRemoveNonDigits())
                    {
                        throw new SMCApplicationException(UIResource.Mensagem_CpfFiador);
                    }
                }
                var nomeFiador = dados.DadosCampos.FirstOrDefault(f => f.TokenElemento == TOKENS.CAMPO_AVALISTA_FIADOR_NOME).Valor.SMCRemoveAccents();
                if (nomeFiador != null)
                {
                    if ((inscrito.NomePai != null && inscrito.NomePai.SMCRemoveAccents().Equals(nomeFiador, StringComparison.InvariantCultureIgnoreCase))
                        || (inscrito.NomeMae != null && inscrito.NomeMae.SMCRemoveAccents().Equals(nomeFiador, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        throw new SMCApplicationException(UIResource.Mensagem_NomeFiador);
                    }
                }
            }

            //Valida dados do caso o formulario seja de seminario
            ValidarFormularioSeminario(dados);

            InscricaoService.SalvarFormularioInscricao(SMCMapperHelper.Create<InscricaoDadoFormularioData>(dados));
        }

        private void ValidarFormularioSeminario(InscricaoDadoFormularioViewModel dados)
        {
            if (dados.DadosCampos.Any(f => f.TokenElemento == "PROJETO"))
            {
                var seqProcesso = Convert.ToInt64(dados.DadosCampos.FirstOrDefault(f => f.TokenElemento == "PROCESSO_GPI").Valor);
                var descricaoProjetoGPC = dados.DadosCampos.FirstOrDefault(f => f.TokenElemento == "PROJETO").Valor;
                if (!string.IsNullOrEmpty(descricaoProjetoGPC))
                {
                    InscricaoService.ValidarFormularioSeminario(seqProcesso, dados.SeqInscricao, descricaoProjetoGPC);
                }
            }
        }

        #endregion Buscar / Persistir Dado Formulario

        #region Arquivos

        /// <summary>
        /// Busca um arquivo anexado para download
        /// </summary>
        /// <param name="seq">Sequencial do arquivo</param>
        /// <returns>Arquivo para download</returns>
        public SMCUploadFile BuscarArquivo(long seq)
        {
            return ArquivoAnexado.BuscarArquivoAnexado(seq);
        }

        #endregion Arquivos

        #region Ofertas

        /// <summary>
        /// Retorna os sequenciais das ofertas vigentes e ativas para um grupo de oferta
        /// </summary>
        public List<long> BuscarSeqOfertasVigentesAtivas(long SeqGrupoOferta)
        {
            return this.OfertaService.BuscarSeqOfertasVigentesAtivas(SeqGrupoOferta);
        }

        /// <summary>
        /// Buscas as taxas vigentes para uma determinada oferta
        /// </summary>
        public List<InscricaoTaxaViewModel> BuscarTaxasInscricaoOfertaVigentes(long seqOferta, long seqInscricao)
        {
            return this.InscricaoService.BuscarTaxaInscricaoOfertaVigente(seqOferta, seqInscricao)
                .TransformList<InscricaoTaxaViewModel>();
        }

        public bool ValidarBolsaExAlunoOferta(long seqOferta, long seqInscricao)
        {
            return InscricaoService.ValidarAptoBolsaNovoTitulo(seqInscricao, seqOferta);
        }

        #endregion Ofertas

        #region Taxas/Boletos

        public List<InscricaoTaxaViewModel> BuscarTaxasOfertaInscricao(long seqInscricao)
        {
            return InscricaoService.BuscarTaxasOfertaInscricao(seqInscricao)
                .TransformList<InscricaoTaxaViewModel>();
        }

        public List<InscricaoTaxaViewModel> BuscarTaxasOfertaInscricaoConfirmacao(long seqInscricao)
        {
            return InscricaoService.BuscarTaxasOfertaInscricaoConfirmacao(seqInscricao)
                .TransformList<InscricaoTaxaViewModel>();
        }

        public TemplateArquivoSecaoViewModel BuscarTaxaInscricao(Framework.UI.Mvc.SMCEncryptedLong seqTaxa)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Busca o boleto para uma determinada inscrição
        /// </summary>
        public BoletoData GerarBoletoInscricao(long seqInscricao)
        {
            var boleto = this.InscricaoService.GerarBoletoInscricao(seqInscricao);
            boleto.ImagemBanco = SMCImageHelper.ImageToBase64(BuscarImagemBanco(boleto.Banco.Numero));
            boleto.ImagemCodigoBarras = SMCImageHelper.ImageToBase64(BuscarImagemCodigoBarras(boleto.CodigoBarras));

            return boleto;
        }

        /// <summary>
        /// Recupera o numero do titulo do GRA
        /// </summary>
        /// <param name="seqInscricao">Sequencial da inscrição</param>
        /// <returns>Número do titulo no GRA</returns>
        public long BuscarSeqTitulo(long seqInscricao)
        {
            return this.InscricaoService.GerarBoletoInscricao(seqInscricao).Titulo.NumeroDocumento;
        }

        // GET: /BoletoBancario/ImagemBanco/?codigoBanco=1
        public byte[] BuscarImagemBanco(int codigoBanco)
        {
            return FinanceiroService.BuscarImagemBanco(codigoBanco);
        }

        // GET: /BoletoBancario/ImagemCodigoBarras/codigoBarras=123456
        public byte[] BuscarImagemCodigoBarras(string codigoBarras)
        {
            return FinanceiroService.BuscarImagemCodigoBarras(codigoBarras);
        }

        public string BuscarDescricaoEventoFinanceiroInscricao(long seqInscricao)
        {
            var seqProcesso = this.InscricaoService.BuscarInscricaoResumida(seqInscricao).SeqProcesso;
            var seqEvento = this.ProcessoService.BuscarEventoProcesso(seqProcesso);
            var evento = this.FinanceiroService.BuscarEvento(seqEvento.Value);
            return evento.Descricao;
        }

        #endregion Taxas/Boletos

        #region Documentos

        /// <summary>
        /// Busca a lista de documetnos permitidas para um grupo
        /// </summary>
        public List<SMCDatasourceItem> BuscarTiposDocumentoGrupo(long seqGrupoDocumentos)
        {
            return DocumentoRequeridoService.BuscarTiposDocumentoGrupo(seqGrupoDocumentos).TransformList<SMCDatasourceItem>();
        }

        /// <summary>
        /// Busca a lista de documetnos opicionais para upload
        /// </summary>
        public List<SMCDatasourceItem> BuscarDocumentosOpcionais(long seqConfiguracaoEtapa)
        {
            return DocumentoRequeridoService.BuscarDocumentosOpcionais(seqConfiguracaoEtapa).TransformList<SMCDatasourceItem>();
        }

        #endregion Documentos
    }
}