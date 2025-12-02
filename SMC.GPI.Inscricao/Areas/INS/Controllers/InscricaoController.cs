using SMC.Formularios.ServiceContract.Areas.FRM.Interfaces;
using SMC.Formularios.UI.Mvc;
using SMC.Framework;
using SMC.Framework.Exceptions;
using SMC.Framework.Extensions;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.Rest;
using SMC.Framework.Security;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.Html;
using SMC.Framework.UI.Mvc.Security;
using SMC.Framework.UI.Mvc.Util;
using SMC.GPI.Inscricao.Areas.INS.Models;
using SMC.GPI.Inscricao.Areas.INS.Services;
using SMC.GPI.Inscricao.Areas.INS.Views.Inscricao.App_LocalResources;
using SMC.GPI.Inscricao.Models;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Areas.INS.Constants;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using SMC.Inscricoes.Common.Areas.INS.Exceptions;
using SMC.Inscricoes.Common.Areas.INS.Exceptions.Inscricao;
using SMC.Inscricoes.Common.Areas.INS.Resources;
using SMC.Inscricoes.Common.Shared;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using SMC.Inscricoes.UI.Mvc.Attributes;
using SMC.PDF;
using SMC.PDFData.Models;

//using SMC.QRCode.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using Unity.Interception.Utilities;

namespace SMC.GPI.Inscricao.Areas.INS.Controllers
{
    [GoogleTagManagerFilterAttribute]
    public class InscricaoController : SGFController
    {
        private const string TOKEN_TESTE = "ci0LBAsaZ7OHPiGB8AksCwXKd5dWHKQL";
        private static readonly string GuidProcesso = "GuidProcesso";

        #region Serviços

        private InscritoControllerService InscritoControllerService { get => Create<InscritoControllerService>(); }

        // ControllerService ficaram obsoletos do FW 4.0, mas mantidos para compatibilidade de aplicações que foram migradas.
        private InscricaoControllerService InscricaoControllerService { get => Create<InscricaoControllerService>(); }

        private IInscricaoService InscricaoService { get => Create<IInscricaoService>(); }

        private IOfertaService OfertaService { get => Create<IOfertaService>(); }

        private ITipoProcessoService TipoProcessoService { get => Create<ITipoProcessoService>(); }

        private IFormularioService FormularioService => this.Create<IFormularioService>();

        private IProcessoService ProcessoService => this.Create<IProcessoService>();
        private IProcessoCampoInscritoService ProcessoCampoInscritoService => this.Create<IProcessoCampoInscritoService>();
        private IInscritoService InscritoService => this.Create<IInscritoService>();

        private IGrupoOfertaService GrupoOfertaService => this.Create<IGrupoOfertaService>();

        #endregion Serviços

        #region Fluxo Página

        /// <summary>
        /// Action que apartir de um token, redireciona para a action que deste token
        /// </summary>
        /// <param name="token">Token da página para redirecionar</param>
        /// <param name="filtro">Filtro para a página</param>
        [SMCAuthorize(UC_INS_002_02_01.CONFIRMACAO_DADOS_INSCRITO,
                      UC_INS_002_02_02.SELECAO_OFERTA,
                      UC_INS_002_02_03.INSTRUCOES_INICIAIS,
                      UC_INS_002_02_04.CODIGO_AUTORIZACAO,
                      UC_INS_002_02_05.FORMULARIO_INSCRICAO,
                      UC_INS_002_02_06.UPLOAD_DOCUMENTOS,
                      UC_INS_002_02_07.CONFIRMACAO_INSCRICAO,
                      UC_INS_002_02_08.FINALIZACAO_INSCRICAO)]
        public ActionResult UrlPagina(string token, PaginaFiltroViewModel filtro)
        {
            token = string.IsNullOrEmpty(token) ? string.Empty : SMCDESCrypto.DecryptForURL(token);

            //var encryptFiltroAngular = new FiltroAngularModel()
            //{
            //    seqConfiguracaoEtapaPagina = (SMCEncryptedLong)filtro.SeqConfiguracaoEtapaPagina,
            //    seqConfiguracaoEtapa = (SMCEncryptedLong)filtro.SeqConfiguracaoEtapa,
            //    seqGrupoOferta = (SMCEncryptedLong)filtro.SeqGrupoOferta,
            //    seqInscricao = (SMCEncryptedLong)filtro.SeqInscricao,
            //    idioma = filtro.Idioma,
            //    tokenAngular=filtro.TokenAngular
            //};
            // Busca a action do token
            string action = string.Empty;
            switch (token)
            {
                case TOKENS.PAGINA_INSTRUCOES_INICAIS:
                    action = "InstrucoesIniciais";
                    break;

                case TOKENS.PAGINA_CONFIRMACAO_DADOS_INSCRITO:
                    action = "ConfirmarDadosInscrito";
                    break;

                case TOKENS.PAGINA_SELECAO_OFERTA:
                    filtro.TokenAngular = "selecao-oferta";
                    return this.UrlPaginaAngular(filtro);
                //action = "SelecaoOferta";
                //  break;

                case TOKENS.PAGINA_CODIGO_AUTORIZACAO:
                    action = "CodigoAutorizacao";
                    break;

                case TOKENS.PAGINA_FORMULARIO_INSCRICAO:
                    action = "FormularioInscricao";
                    break;

                case TOKENS.PAGINA_UPLOAD_DOCUMENTOS:
                    action = "UploadDocumentos";
                    break;

                case TOKENS.PAGINA_CONFIRMACAO_INSCRICAO:
                    action = "ConfirmarInscricao";
                    break;

                case TOKENS.PAGINA_INSTRUCOES_FINAIS:
                    action = "InstrucoesFinais";
                    break;

                case TOKENS.PAGINA_COMPROVANTE_INSCRICAO:
                    action = "ComprovanteInscricao";
                    break;
            }

            // Se não encontrou a action, redireciona para página home
            if (string.IsNullOrEmpty(action))
            {
                return RedirectToAction("Index", "Home", new { area = string.Empty });
            }
            else
            {
                // Redireciona para a action do token informado
                return RedirectToAction(action, "Inscricao", new
                {
                    seqConfiguracaoEtapaPagina = (SMCEncryptedLong)filtro.SeqConfiguracaoEtapaPagina,
                    seqConfiguracaoEtapa = (SMCEncryptedLong)filtro.SeqConfiguracaoEtapa,
                    seqGrupoOferta = (SMCEncryptedLong)filtro.SeqGrupoOferta,
                    seqInscricao = (SMCEncryptedLong)filtro.SeqInscricao,
                    idioma = filtro.Idioma,

                });
            }
        }

        #endregion Fluxo Página

        #region Inscrever

        /// <summary>
        /// Action que inicia uma inscrição
        /// </summary>
        /// <param name="filtro">Filtro para inscrição</param>
        [SMCAuthorize(UC_INS_002_02_01.CONFIRMACAO_DADOS_INSCRITO,
                      UC_INS_002_02_02.SELECAO_OFERTA,
                      UC_INS_002_02_03.INSTRUCOES_INICIAIS,
                      UC_INS_002_02_04.CODIGO_AUTORIZACAO,
                      UC_INS_002_02_05.FORMULARIO_INSCRICAO,
                      UC_INS_002_02_06.UPLOAD_DOCUMENTOS,
                      UC_INS_002_02_07.CONFIRMACAO_INSCRICAO,
                      UC_INS_002_02_08.FINALIZACAO_INSCRICAO)]
        public ActionResult Inscrever(PaginaFiltroViewModel filtro)
        {
            // Verifica se o usuário logado possui cadastro de inscrito
            long? seqInscrito = InscritoControllerService.BuscarSeqInscritoLogado();

            // Se usuário não possui inscrito, redireciona para o cadastro de um
            if (!seqInscrito.HasValue)
            {
                return RedirectToAction("Cadastrar", "Inscrito", new { area = "INS" });
            }

            try
            {
                // Verifica se o inscrito pode iniciar uma nova inscricao
                // Caso tenha algum impedimento para iniciar nova inscrição o serviço dispara Exception
                IniciarContinuarInscricaoFiltroViewModel filtroRegra = new IniciarContinuarInscricaoFiltroViewModel()
                {
                    SeqInscrito = seqInscrito.Value,
                    SeqConfiguracaoEtapa = filtro.SeqConfiguracaoEtapa,
                    SeqGrupoOferta = filtro.SeqGrupoOferta
                };
                InscricaoControllerService.VerificarPermissaoIniciarContinuarInscricao(filtroRegra);

                // Busca o token da primeira página da configuração
                ConfiguracaoEtapaPaginaViewModel primeiraPagina = InscricaoControllerService.BuscarConfiguracaoEtapaPrimeiraPagina(filtro.SeqConfiguracaoEtapa);

                // Redireciona para a primeira página
                return RedirectToAction("UrlPagina",
                        new
                        {
                            token = SMCDESCrypto.EncryptForURL(primeiraPagina.Token),
                            seqConfiguracaoEtapaPagina = (SMCEncryptedLong)primeiraPagina.Seq,
                            seqConfiguracaoEtapa = (SMCEncryptedLong)filtro.SeqConfiguracaoEtapa,
                            seqGrupoOferta = (SMCEncryptedLong)filtro.SeqGrupoOferta,
                            seqInscricao = (SMCEncryptedLong)filtro.SeqInscricao,
                            idioma = filtro.Idioma
                        });
            }
            catch (SMCApplicationException e)
            {
                SetErrorMessage(e.Message);
                string guidProcesso = string.Empty;
                if (e.InnerException != null)
                {
                    guidProcesso = e.InnerException.Message.Split(':')[1];
                }
                if (!string.IsNullOrEmpty(guidProcesso))
                {
                    return RedirectToAction("IndexProcesso", "Home", new { area = string.Empty, uidProcesso = guidProcesso });
                }

                return RedirectToAction("Index", "Home", new { area = string.Empty });
            }
        }

        #endregion Inscrever

        #region Visualizar

        [HttpGet]
        [SMCAuthorize(UC_INS_002_02_01.CONFIRMACAO_DADOS_INSCRITO,
                      UC_INS_002_02_02.SELECAO_OFERTA,
                      UC_INS_002_02_03.INSTRUCOES_INICIAIS,
                      UC_INS_002_02_04.CODIGO_AUTORIZACAO,
                      UC_INS_002_02_05.FORMULARIO_INSCRICAO,
                      UC_INS_002_02_06.UPLOAD_DOCUMENTOS,
                      UC_INS_002_02_07.CONFIRMACAO_INSCRICAO,
                      UC_INS_002_02_08.FINALIZACAO_INSCRICAO)]
        public ActionResult Visualizar(PaginaFiltroViewModel filtro)
        {
            // Busca as informações da página de instruções finais
            var pagina = InscricaoControllerService.BuscarConfiguracaoEtapaPagina(filtro.SeqConfiguracaoEtapa, TOKENS.PAGINA_INSTRUCOES_FINAIS);

            // Redireciona para a pagina de instruções finais
            return RedirectToAction("UrlPagina",
                    new
                    {
                        token = SMCDESCrypto.EncryptForURL(pagina.Token),
                        seqConfiguracaoEtapaPagina = (SMCEncryptedLong)pagina.Seq,
                        seqConfiguracaoEtapa = (SMCEncryptedLong)filtro.SeqConfiguracaoEtapa,
                        seqGrupoOferta = (SMCEncryptedLong)filtro.SeqGrupoOferta,
                        seqInscricao = (SMCEncryptedLong)filtro.SeqInscricao,
                        idioma = filtro.Idioma
                    });
        }
        [SMCAuthorize(UC_INS_002_02_08.FINALIZACAO_INSCRICAO)]
        public ActionResult VisualizarPdfQrCode(SMCEncryptedLong seqInscricao)
        {
            IngressoViewModel modelo = InscricaoService.BuscarIngressos(seqInscricao).Transform<IngressoViewModel>();



            var tokens = new List<string>()
            {
                "ESCOLA",
                "SERIE"
            };

            InscricaoControllerService.PreencheEscolaSerieComprovanteQrCode(modelo, tokens);

            var options = new GridOptions(PDFPageSize.A4)
            {

                PageMargins = new MarginInfo
                {
                    Top = 0,
                    Bottom = 0,
                    Left = 0,
                    Right = 0

                },
            };

            //renderiza somente as ofertas que habilita checkin
            modelo.Ofertas = modelo.Ofertas.Where(s => s.HabilitaCheckin).ToList();

            // return arquivoComprovantePDF;
            return RenderPdfView("PdfListaQrCode", options: options, model: modelo);
        }

        /// <summary>
        /// IMPORTANTE : REMOVER ESSE METODO ASSIM QUE OS DESIGNERS TERMINAREM DE FAZER A TELA DO COMPROVANTE DE QRCODE
        /// </summary>
        /// <param name="seqInscricao"></param>
        /// <returns></returns>
        [SMCAllowAnonymous]
        public ActionResult VisualizarViewImprmirQrCode(SMCEncryptedLong seqInscricao)
        {
            IngressoViewModel modelo = InscricaoService.BuscarIngressos(seqInscricao).Transform<IngressoViewModel>();

            var tokens = new List<string>()
            {
                "ESCOLA",
                "SERIE"
            };

            InscricaoControllerService.PreencheEscolaSerieComprovanteQrCode(modelo, tokens);

            var options = new GridOptions(PDFPageSize.A4)
            {

                PageMargins = new MarginInfo
                {
                    Top = 0,
                    Bottom = 0,
                    Left = 0,
                    Right = 0

                },
            };
            return PartialView("PdfListaQrCode", modelo);
        }

        #endregion Visualizar

        #region ContinuarInscricao

        [HttpGet]
        [SMCAuthorize(UC_INS_002_02_01.CONFIRMACAO_DADOS_INSCRITO,
                      UC_INS_002_02_02.SELECAO_OFERTA,
                      UC_INS_002_02_03.INSTRUCOES_INICIAIS,
                      UC_INS_002_02_04.CODIGO_AUTORIZACAO,
                      UC_INS_002_02_05.FORMULARIO_INSCRICAO,
                      UC_INS_002_02_06.UPLOAD_DOCUMENTOS,
                      UC_INS_002_02_07.CONFIRMACAO_INSCRICAO,
                      UC_INS_002_02_08.FINALIZACAO_INSCRICAO)]
        public ActionResult ContinuarInscricao(PaginaFiltroViewModel filtro)
        {
            // Verifica se o usuário logado possui cadastro de inscrito
            long? seqInscrito = InscritoControllerService.BuscarSeqInscritoLogado();

            // Se usuário não possui inscrito, redireciona para o cadastro de um
            if (!seqInscrito.HasValue)
            {
                return RedirectToAction("Cadastrar", "Inscrito", new { area = "INS" });
            }

            try
            {
                // Busca informações da ultima página acessada pela inscrição
                ContinuarInscricaoViewModel modelo = InscricaoControllerService.BuscarUltimaPaginaInscricao(filtro.SeqInscricao);

                if (modelo == null)
                {
                    // Busca o token da primeira página da configuração
                    ConfiguracaoEtapaPaginaViewModel primeiraPagina = InscricaoControllerService.BuscarConfiguracaoEtapaPrimeiraPagina(filtro.SeqConfiguracaoEtapa);

                    // Redireciona para a primeira página
                    return RedirectToAction("UrlPagina",
                            new
                            {
                                token = SMCDESCrypto.EncryptForURL(primeiraPagina.Token),
                                seqConfiguracaoEtapaPagina = (SMCEncryptedLong)primeiraPagina.Seq,
                                seqConfiguracaoEtapa = (SMCEncryptedLong)filtro.SeqConfiguracaoEtapa,
                                seqGrupoOferta = (SMCEncryptedLong)filtro.SeqGrupoOferta,
                                seqInscricao = (SMCEncryptedLong)filtro.SeqInscricao,
                                idioma = filtro.Idioma
                            });
                }

                VerificarIdioma(modelo);

                // Verifica se o inscrito pode iniciar uma nova inscricao
                // Caso tenha algum impedimento para iniciar nova inscrição o serviço dispara Exception
                IniciarContinuarInscricaoFiltroViewModel filtroRegra = new IniciarContinuarInscricaoFiltroViewModel()
                {
                    SeqInscrito = seqInscrito.Value,
                    SeqConfiguracaoEtapa = modelo.SeqConfiguracaoEtapa,
                    SeqGrupoOferta = modelo.SeqGrupoOferta,
                    SeqInscricao = filtro.SeqInscricao
                };
                InscricaoControllerService.VerificarPermissaoIniciarContinuarInscricao(filtroRegra);

                // Redireciona para a ultima página da inscrição
                return RedirectToAction("UrlPagina",
                        new
                        {
                            token = SMCDESCrypto.EncryptForURL(modelo.TokenPagina),
                            seqConfiguracaoEtapaPagina = (SMCEncryptedLong)modelo.SeqConfiguracaoEtapaPagina,
                            seqConfiguracaoEtapa = (SMCEncryptedLong)modelo.SeqConfiguracaoEtapa,
                            seqGrupoOferta = (SMCEncryptedLong)modelo.SeqGrupoOferta,
                            seqInscricao = (SMCEncryptedLong)modelo.SeqInscricao,
                            idioma = modelo.Idioma
                        });
            }
            catch (Exception e)
            {
                SetErrorMessage(e.Message);
                return RedirectToAction("Index", "Home", new { area = string.Empty });
            }
        }

        #endregion ContinuarInscricao

        #region Instruções Iniciais

        [HttpGet]
        [SMCAuthorize(UC_INS_002_02_03.INSTRUCOES_INICIAIS)]
        public ActionResult InstrucoesIniciais(PaginaFiltroViewModel filtro)
        {
            // Busca as informações da página
            try
            {
                PaginaInstrucaoInicialViewModel modelo = InscricaoControllerService.BuscarPaginaInstrucoesIniciais(filtro);

                ValidarContinuarInscricao<PaginaInstrucaoInicialViewModel>(modelo);

                // No momento da geração da pagina cria a inscrição, caso não exista
                modelo.SeqInscricao = InscricaoControllerService.IncluirInscricao(filtro.Transform<PaginaInstrucaoInicialViewModel>());

                // Registra entrada na página
                InscricaoControllerService.IncluirHistoricoPagina(modelo);

                VerificarIdioma(modelo);

                return View(modelo);
            }
            catch (Exception ex)
            {
                var uidProcesso = Session[GuidProcesso];
                return ThrowRedirect(new SMCApplicationException(ex.Message), "IndexProcesso", "Home", new System.Web.Routing.RouteValueDictionary { { "uidProcesso", uidProcesso }, { "area", "" } });
            }
        }

        [HttpPost]
        [SMCAuthorize(UC_INS_002_02_03.INSTRUCOES_INICIAIS)]
        public ActionResult SalvarInstrucoesIniciais(PaginaInstrucaoInicialViewModel modelo)
        {
            try
            {
                // Redireciona para a próxima página
                return RedirectToAction("UrlPagina",
                        new
                        {
                            token = modelo.TokenProximaPaginaEncrypted,
                            seqConfiguracaoEtapaPagina = modelo.SeqConfiguracaoEtapaPaginaProximaEncrypted,
                            seqConfiguracaoEtapa = modelo.SeqConfiguracaoEtapaEncrypted,
                            seqGrupoOferta = modelo.SeqGrupoOfertaEncrypted,
                            seqInscricao = modelo.SeqInscricaoEncrypted,
                            idioma = modelo.Idioma
                        });
            }
            catch (Exception e)
            {
                SetErrorMessage(e.Message, target: SMCMessagePlaceholders.Centro);

                // Busca as informações da página
                PaginaFiltroViewModel filtro = new PaginaFiltroViewModel()
                {
                    SeqConfiguracaoEtapaPagina = modelo.SeqConfiguracaoEtapaPagina,
                    SeqConfiguracaoEtapa = modelo.SeqConfiguracaoEtapa,
                    SeqGrupoOferta = modelo.SeqGrupoOferta,
                    SeqInscricao = modelo.SeqInscricao,
                    Idioma = modelo.Idioma
                };
                PaginaInstrucaoInicialViewModel modeloNovo = InscricaoControllerService.BuscarPaginaInstrucoesIniciais(filtro);
                return View("InstrucoesIniciais", modeloNovo);
            }
        }

        #endregion Instruções Iniciais

        #region Confirmar Dados Inscrito

        [HttpGet]
        [SMCAuthorize(UC_INS_002_02_01.CONFIRMACAO_DADOS_INSCRITO)]
        public ActionResult ConfirmarDadosInscrito(PaginaFiltroViewModel filtro)
        {
            try
            {
                // Busca as informações da página
                PaginaConfirmarDadosInscritoViewModel modelo = InscricaoControllerService.BuscarPaginaDadosInscrito(filtro);

                ValidarContinuarInscricao<PaginaConfirmarDadosInscritoViewModel>(modelo);

                // No momento da geração da pagina cria a inscrição, caso ela não exista.
                modelo.SeqInscricao = InscricaoControllerService.IncluirInscricao(filtro.Transform<PaginaConfirmarDadosInscritoViewModel>());

                // Registra entrada na página
                InscricaoControllerService.IncluirHistoricoPagina(modelo);

                modelo.DadosInscrito.CamposInscritoVisiveis = ConfigurarVisibilidadeCamposPorProcesso(modelo.SeqProcesso);

                return View(modelo);
            }
            catch (Exception ex)
            {

                var uidProcesso = Session[GuidProcesso];
                return ThrowRedirect(new SMCApplicationException(ex.Message), "IndexProcesso", "Home", new System.Web.Routing.RouteValueDictionary { { "uidProcesso", uidProcesso }, { "area", "" } });
            }
        }

        [HttpPost]
        [SMCAuthorize(UC_INS_002_02_01.CONFIRMACAO_DADOS_INSCRITO)]
        public ActionResult SalvarConfirmarDadosInscrito(PaginaConfirmarDadosInscritoViewModel modelo)
        {
            try
            {
                // Redireciona para a próxima página
                return RedirectToAction("UrlPagina",
                        new
                        {
                            token = modelo.TokenProximaPaginaEncrypted,
                            seqConfiguracaoEtapaPagina = modelo.SeqConfiguracaoEtapaPaginaProximaEncrypted,
                            seqConfiguracaoEtapa = modelo.SeqConfiguracaoEtapaEncrypted,
                            seqGrupoOferta = modelo.SeqGrupoOfertaEncrypted,
                            seqInscricao = modelo.SeqInscricaoEncrypted,
                            idioma = modelo.Idioma
                        });
            }
            catch (Exception e)
            {
                SetErrorMessage(e.Message, target: SMCMessagePlaceholders.Centro);

                // Busca as informações da página
                PaginaFiltroViewModel filtro = new PaginaFiltroViewModel()
                {
                    SeqConfiguracaoEtapaPagina = modelo.SeqConfiguracaoEtapaPagina,
                    SeqConfiguracaoEtapa = modelo.SeqConfiguracaoEtapa,
                    SeqGrupoOferta = modelo.SeqGrupoOferta,
                    SeqInscricao = modelo.SeqInscricao,
                    Idioma = modelo.Idioma
                };
                PaginaConfirmarDadosInscritoViewModel modeloNovo = InscricaoControllerService.BuscarPaginaDadosInscrito(filtro);
                return View("ConfirmarDadosInscrito", modeloNovo);
            }
        }

        #endregion Confirmar Dados Inscrito

        #region Seleção de Oferta

        [HttpGet]
        [SMCAuthorize(UC_INS_002_02_02.SELECAO_OFERTA)]
        public ActionResult SelecaoOferta(PaginaFiltroViewModel filtro)
        {
            try
            {
                ValidarSessionSelecaoOferta(filtro.SeqGrupoOferta);
                // No momento da geração da pagina cria a inscrição, caso não exista
                filtro.SeqInscricao = InscricaoControllerService.IncluirInscricao(filtro.Transform<PaginaSelecaoOfertaViewModel>());

                // Busca as informações da página
                PaginaSelecaoOfertaViewModel modelo = InscricaoControllerService.BuscarPaginaSelecaoOferta(filtro);
                modelo.Taxas?.ForEach(f => f.TituloPago = modelo.TituloPago);

                ValidarContinuarInscricao<PaginaSelecaoOfertaViewModel>(modelo);

                // Se a inscrição já está criada
                if (filtro.SeqInscricao > 0)
                {
                    // Registra entrada na página
                    InscricaoControllerService.IncluirHistoricoPagina(modelo);
                }
                else
                {
                    // É obrigatório que a inscrição estaja criada, senão erro!
                    throw new InscricaoInvalidaException();
                }
                return View(modelo);
            }
            catch (Exception ex)
            {
                var uidProcesso = Session[GuidProcesso];
                return ThrowRedirect(new SMCApplicationException(ex.Message), "IndexProcesso", "Home", new System.Web.Routing.RouteValueDictionary { { "uidProcesso", uidProcesso }, { "area", "" } });
            }
        }




        [HttpPost]
        [SMCAuthorize(UC_INS_002_02_02.SELECAO_OFERTA)]
        public ActionResult SalvarSelecaoOferta(PaginaSelecaoOfertaViewModel modelo)
        {
            try
            {
                // Se a inscrição não está informada erro
                if (modelo.SeqInscricao <= 0)
                    throw new InscricaoInvalidaException();

                // Verifica se possui boleto pago e se a taxa alterou os valores pagos
                long seqOferta = 0;
                if (modelo.Ofertas.Count == 1)
                {
                    seqOferta = modelo.Ofertas.FirstOrDefault().SeqOferta.Seq.GetValueOrDefault();
                }
                else
                {
                    seqOferta = modelo.Ofertas.FirstOrDefault(f => f.NumeroOpcao == 1).SeqOferta.Seq.GetValueOrDefault();
                }
                //InscricaoService.ValidarBoletoPagoAlteracaoValor(modelo.SeqInscricao, modelo.Ofertas.FirstOrDefault(o => o.NumeroOpcao == 1).SeqOferta.Seq.GetValueOrDefault());
                InscricaoService.ValidarBoletoPagoAlteracaoValor(modelo.SeqInscricao, seqOferta);

                if (modelo.Taxas != null && modelo.Taxas.Any())
                {
                    this.Assert(modelo, UIResource.Assert_ExisteBoletoInscricaoAlteracaoTaxa, () =>
                    {
                        return InscricaoControllerService.VerificaBoletoInscricaoAlteracaoTaxa(modelo.SeqInscricao, modelo.Taxas);
                    });
                }

                // Salva a(s) oferta(s) da inscrição
                //modelo.Ofertas = modelo.Ofertas.SMCRemove(x => x.SeqOferta.Seq == null).ToList();
                InscricaoControllerService.SalvarInscricaoOferta(modelo);

                // Redireciona para a próxima página
                return SMCRedirectToAction("UrlPagina", routeValues:
                    new
                    {
                        token = modelo.TokenProximaPaginaEncrypted,
                        seqConfiguracaoEtapaPagina = modelo.SeqConfiguracaoEtapaPaginaProximaEncrypted,
                        seqConfiguracaoEtapa = modelo.SeqConfiguracaoEtapaEncrypted,
                        seqGrupoOferta = modelo.SeqGrupoOfertaEncrypted,
                        seqInscricao = modelo.SeqInscricaoEncrypted,
                        idioma = modelo.Idioma
                    });
            }
            catch (Exception e)
            {
                SetErrorMessage(e.Message, target: SMCMessagePlaceholders.Centro, persistentMessage: true);

                // Busca as informações da página
                PaginaFiltroViewModel filtro = new PaginaFiltroViewModel()
                {
                    SeqConfiguracaoEtapaPagina = modelo.SeqConfiguracaoEtapaPagina,
                    SeqConfiguracaoEtapa = modelo.SeqConfiguracaoEtapa,
                    SeqGrupoOferta = modelo.SeqGrupoOferta,
                    SeqInscricao = modelo.SeqInscricao,
                    Idioma = modelo.Idioma
                };
                PaginaSelecaoOfertaViewModel modeloNovo = InscricaoControllerService.BuscarPaginaSelecaoOferta(filtro);
                modeloNovo.Ofertas = modelo.Ofertas;
                modeloNovo.Taxas = modelo.Taxas;
                return View("SelecaoOferta", modeloNovo);
            }
        }

        [HttpPost]
        [SMCAuthorize(UC_INS_002_02_02.SELECAO_OFERTA)]
        public ActionResult ListarTaxasOfertaInscricao(long seqOferta, long seqInscricao)
        {
            var taxasOferta = InscricaoControllerService.BuscarTaxasInscricaoOfertaVigentes(seqOferta, seqInscricao);
            var model = new PaginaSelecaoOfertaViewModel { Taxas = taxasOferta };
            return PartialView("_ListarTaxasOferta", model);
        }

        [SMCAuthorize(UC_INS_002_02_02.SELECAO_OFERTA)]
        public ActionResult VisualizarDetalhesOferta(SMCEncryptedLong seqOferta)
        {
            var oferta = OfertaService.BuscarOferta(seqOferta).Transform<OfertaViewModel>();
            return PartialView("_VisualizarDetalhesOferta", oferta);
        }

        [HttpPost]
        [SMCAuthorize(UC_INS_002_02_02.SELECAO_OFERTA)]
        public ActionResult ValidarBolsaExAlunoOferta(SMCEncryptedLong seqInscricaoEncrypted, long seqOferta, bool bolsaExAluno)
        {
            var result = bolsaExAluno && InscricaoControllerService.ValidarBolsaExAlunoOferta(seqOferta, seqInscricaoEncrypted);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion Seleção de Oferta

        #region Seleção de Oferta Angular



        [HttpGet]
        [SMCAuthorize(UC_INS_002_02_02.SELECAO_OFERTA)]
        public ContentResult SelecaoOfertaAngular(PaginaFiltroViewModel filtro)
        {
            try
            {
                ValidarSessionSelecaoOferta(filtro.SeqGrupoOferta);
                // No momento da geração da pagina cria a inscrição, caso não exista
                filtro.SeqInscricao = InscricaoControllerService.IncluirInscricao(filtro.Transform<PaginaSelecaoOfertaViewModel>());

                // Busca as informações da página
                PaginaSelecaoOfertaViewModel modelo = InscricaoControllerService.BuscarPaginaSelecaoOferta(filtro);
                modelo.Taxas?.ForEach(f => f.TituloPago = modelo.TituloPago);

                ValidarContinuarInscricao<PaginaSelecaoOfertaViewModel>(modelo);

                // Se a inscrição já está criada
                if (filtro.SeqInscricao > 0)
                {
                    // Registra entrada na página
                    InscricaoControllerService.IncluirHistoricoPagina(modelo);
                }
                else
                {
                    // É obrigatório que a inscrição estaja criada, senão erro!
                    throw new InscricaoInvalidaException();
                }

                modelo.ExibirArvoreFechada = ProcessoService.ExibeArvoreFechada(modelo.SeqProcesso);

                return SMCJsonResultAngular(modelo);
            }
            catch (Exception ex)
            {
                var uidProcesso = Session[GuidProcesso];
                var url = Url.Action("IndexProcesso", "Home", new { uidProcesso, area = "" });
                object Data = new
                {
                    Message = ex.Message,
                    ErrorStack = ex.ToString(),
                    UrlRetorno = url
                };

                return SMCJsonResultAngular(Data, System.Net.HttpStatusCode.Redirect);
            }
        }

        [HttpPost]
        [SMCAuthorize(UC_INS_002_02_02.SELECAO_OFERTA)]
        public ActionResult SalvarSelecaoOfertaAngular(PaginaSelecaoOfertaViewModel modelo)
        {
            try
            {
                // Se a inscrição não está informada erro
                if (modelo.SeqInscricao <= 0)
                    throw new InscricaoInvalidaException();

                // Verifica se possui boleto pago e se a taxa alterou os valores pagos
                long seqOferta = 0;
                if (modelo.Ofertas == null)
                {
                    throw new SelecionarPeloMenosUmaOfertaException();
                }

                if (modelo.Ofertas.Count == 1)
                {
                    seqOferta = modelo.Ofertas.FirstOrDefault().SeqOferta.Seq.GetValueOrDefault();
                }
                else
                {
                    seqOferta = modelo.Ofertas.FirstOrDefault(f => f.NumeroOpcao == 1).SeqOferta.Seq.GetValueOrDefault();
                }
                //InscricaoService.ValidarBoletoPagoAlteracaoValor(modelo.SeqInscricao, modelo.Ofertas.FirstOrDefault(o => o.NumeroOpcao == 1).SeqOferta.Seq.GetValueOrDefault());
                InscricaoService.ValidarBoletoPagoAlteracaoValor(modelo.SeqInscricao, seqOferta);

                if (modelo.Taxas != null && modelo.Taxas.Any())
                {
                    var possuiBoletoAlterado = InscricaoControllerService.VerificaBoletoInscricaoAlteracaoTaxa(modelo.SeqInscricao, modelo.Taxas);

                    if (possuiBoletoAlterado && !modelo.PermiteAlterarBoleto)
                    {
                        return SMCJsonResultAngular(new
                        {
                            data = new
                            {
                                token = modelo.TokenProximaPaginaEncrypted,
                                seqConfiguracaoEtapaPagina = modelo.SeqConfiguracaoEtapaPaginaProximaEncrypted,
                                seqconfiguracaoetapa = modelo.SeqConfiguracaoEtapaEncrypted,
                                seqGrupoOferta = modelo.SeqGrupoOfertaEncrypted,
                                seqInscricao = modelo.SeqInscricaoEncrypted,
                                idioma = modelo.Idioma
                            },
                            statusCode = 0
                        });
                    }


                    //this.Assert(modelo, UIResource.Assert_ExisteBoletoInscricaoAlteracaoTaxa, () =>
                    //{
                    //    return InscricaoControllerService.VerificaBoletoInscricaoAlteracaoTaxa(modelo.SeqInscricao, modelo.Taxas);
                    //});
                }

                // Salva a(s) oferta(s) da inscrição
                //modelo.Ofertas = modelo.Ofertas.SMCRemove(x => x.SeqOferta.Seq == null).ToList();
                InscricaoControllerService.SalvarInscricaoOfertaAngular(modelo);

                return SMCJsonResultAngular(new
                {
                    data = new
                    {
                        token = modelo.TokenProximaPaginaEncrypted,
                        seqConfiguracaoEtapaPagina = modelo.SeqConfiguracaoEtapaPaginaProximaEncrypted,
                        seqconfiguracaoetapa = modelo.SeqConfiguracaoEtapaEncrypted,
                        seqGrupoOferta = modelo.SeqGrupoOfertaEncrypted,
                        seqInscricao = modelo.SeqInscricaoEncrypted,
                        idioma = modelo.Idioma
                    },
                    statusCode = System.Net.HttpStatusCode.OK
                });


                // Redireciona para a próxima página
                //return SMCRedirectToAction("UrlPagina", routeValues:
                //    new
                //    {
                //        token = modelo.TokenProximaPaginaEncrypted,
                //        seqConfiguracaoEtapaPagina = modelo.SeqConfiguracaoEtapaPaginaProximaEncrypted,
                //        seqConfiguracaoEtapa = modelo.SeqConfiguracaoEtapaEncrypted,
                //        seqGrupoOferta = modelo.SeqGrupoOfertaEncrypted,
                //        seqInscricao = modelo.SeqInscricaoEncrypted,
                //        idioma = modelo.Idioma
                //    });
            }
            catch (Exception e)
            {
                //SetErrorMessage(e.Message, target: SMCMessagePlaceholders.Centro, persistentMessage: true);

                // Busca as informações da página
                //PaginaFiltroViewModel filtro = new PaginaFiltroViewModel()
                //{
                //    SeqConfiguracaoEtapaPagina = modelo.SeqConfiguracaoEtapaPagina,
                //    SeqConfiguracaoEtapa = modelo.SeqConfiguracaoEtapa,
                //    SeqGrupoOferta = modelo.SeqGrupoOferta,
                //    SeqInscricao = modelo.SeqInscricao,
                //    Idioma = modelo.Idioma
                //};
                //PaginaSelecaoOfertaViewModel modeloNovo = InscricaoControllerService.BuscarPaginaSelecaoOferta(filtro);
                //modeloNovo.Ofertas = modelo.Ofertas;
                //modeloNovo.Taxas = modelo.Taxas;

                return SMCJsonResultAngular(new
                {
                    data = e.Message,
                    statusCode = System.Net.HttpStatusCode.InternalServerError
                });


                // return View("SelecaoOferta", modeloNovo);
            }
        }

        [SMCAuthorize(UC_INS_002_02_02.SELECAO_OFERTA)]
        public ActionResult BuscarUrlCss(SMCEncryptedLong seqInscricao)
        {
            var urlCss = InscricaoControllerService.BuscarUrlCss(seqInscricao);
            return SMCJsonResultAngular(urlCss);
        }

        [HttpGet]
        [SMCAuthorize(UC_INS_002_02_02.SELECAO_OFERTA)]
        public ContentResult ListarTaxasOfertaInscricaoAngular(long seqOferta, long seqInscricao)
        {
            var taxasOferta = InscricaoControllerService.BuscarTaxasInscricaoOfertaVigentes(seqOferta, seqInscricao);
            return SMCJsonResultAngular(taxasOferta);
        }

        [SMCAuthorize(UC_INS_002_02_02.SELECAO_OFERTA)]
        public ActionResult VisualizarDetalhesOfertaAngular(SMCEncryptedLong seqOferta)
        {
            var oferta = OfertaService.BuscarOferta(seqOferta).Transform<OfertaViewModel>();
            return PartialView("_VisualizarDetalhesOferta", oferta);
        }

        [HttpPost]
        [SMCAuthorize(UC_INS_002_02_02.SELECAO_OFERTA)]
        public ActionResult ValidarBolsaExAlunoOfertaAngular(SMCEncryptedLong seqInscricaoEncrypted, long seqOferta, bool bolsaExAluno)
        {
            var result = bolsaExAluno && InscricaoControllerService.ValidarBolsaExAlunoOferta(seqOferta, seqInscricaoEncrypted);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [SMCAuthorize(UC_INS_002_02_02.SELECAO_OFERTA)]
        public ContentResult BuscarHierarquiaAngular(long seqGrupoOferta)
        {
            var hierarquia = OfertaService.BuscarArvoreSelecaoOfertasInscricao(seqGrupoOferta);
            return SMCJsonResultAngular(hierarquia);
        }
        [HttpGet]
        [SMCAuthorize(UC_INS_002_02_02.SELECAO_OFERTA)]
        public ContentResult BuscarHierarquiaCompletaAngular(long seqOferta)
        {
            var hierarquia = OfertaService.BuscarHierarquiaCompletaAngular(seqOferta);
            return SMCJsonResultAngular(hierarquia);
        }

        [HttpGet]
        [SMCAuthorize(UC_INS_002_02_02.SELECAO_OFERTA)]
        public ContentResult BuscarDescricaoSelecaoOfertasInscricaoSeqsOfertas(long[] seqsOfertas)
        {
            var hierarquia = OfertaService.BuscarDescricaoSelecaoOfertasInscricaoSeqsOfertas(seqsOfertas);
            return SMCJsonResultAngular(hierarquia);
        }


        public RedirectResult UrlPaginaAngular(PaginaFiltroViewModel filtro)
        {

            var parametros = new
            {
                seqConfiguracaoEtapaPagina = (SMCEncryptedLong)filtro.SeqConfiguracaoEtapaPagina,
                seqConfiguracaoEtapa = (SMCEncryptedLong)filtro.SeqConfiguracaoEtapa,
                seqGrupoOferta = (SMCEncryptedLong)filtro.SeqGrupoOferta,
                seqInscricao = (SMCEncryptedLong)filtro.SeqInscricao,
                idioma = filtro.Idioma,
                tokenAngular = filtro.TokenAngular
            };

            var url = HttpContextAmbiente.UrlAmbiente("GPI.Inscricao/App/") + filtro.TokenAngular + "?" + this.ObjectToQueryString(parametros);
            return Redirect(url);

        }

        [HttpGet]
        [SMCAuthorize(UC_INS_002_02_01.CONFIRMACAO_DADOS_INSCRITO,
               UC_INS_002_02_02.SELECAO_OFERTA,
               UC_INS_002_02_03.INSTRUCOES_INICIAIS,
               UC_INS_002_02_04.CODIGO_AUTORIZACAO,
               UC_INS_002_02_05.FORMULARIO_INSCRICAO,
               UC_INS_002_02_06.UPLOAD_DOCUMENTOS,
               UC_INS_002_02_07.CONFIRMACAO_INSCRICAO,
               UC_INS_002_02_08.FINALIZACAO_INSCRICAO)]
        public ActionResult RedirecionamentoAngular(PaginaFiltroViewModel filtro)
        {
            switch (filtro.TokenAngular)
            {
                case "selecao-oferta":
                    return UrlPaginaAngular(filtro);
                default:
                    return View();
            }

            //return View();
        }

        private string ObjectToQueryString(object obj)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            foreach (var prop in obj.GetType().GetProperties())
            {
                query[prop.Name] = prop.GetValue(obj)?.ToString();
            }
            return query.ToString();
        }

        #endregion Seleção de Oferta

        #region Código Autorização

        [HttpGet]
        [SMCAuthorize(UC_INS_002_02_04.CODIGO_AUTORIZACAO)]
        public ActionResult CodigoAutorizacao(PaginaFiltroViewModel filtro)
        {
            // Busca os dados da página
            PaginaCodigoAutorizacaoViewModel modelo = InscricaoControllerService.BuscarPaginaCodigosAutorizacao(filtro);

            ValidarContinuarInscricao<PaginaCodigoAutorizacaoViewModel>(modelo);

            // Se a inscrição já está criada
            if (filtro.SeqInscricao > 0)
            {
                // Registra entrada na página
                InscricaoControllerService.IncluirHistoricoPagina(modelo);
            }
            else
            {
                // É obrigatório que a inscrição estaja criada, senão erro!
                throw new InscricaoInvalidaException();
            }

            return View(modelo);
        }

        [HttpPost]
        [SMCAuthorize(UC_INS_002_02_04.CODIGO_AUTORIZACAO)]
        public ActionResult SalvarCodigoAutorizacao(PaginaCodigoAutorizacaoViewModel modelo)
        {
            try
            {
                // Se a inscrição não está informada erro
                if (modelo.SeqInscricao <= 0)
                    throw new InscricaoInvalidaException();

                // Salva o código de autorizaçãoo da inscrição
                InscricaoControllerService.SalvarInscricaoCodigosAutorizacao(modelo);

                // Redireciona para a próxima página
                return RedirectToAction("UrlPagina",
                    new
                    {
                        token = modelo.TokenProximaPaginaEncrypted,
                        seqConfiguracaoEtapaPagina = modelo.SeqConfiguracaoEtapaPaginaProximaEncrypted,
                        seqConfiguracaoEtapa = modelo.SeqConfiguracaoEtapaEncrypted,
                        seqGrupoOferta = modelo.SeqGrupoOfertaEncrypted,
                        seqInscricao = modelo.SeqInscricaoEncrypted,
                        idioma = modelo.Idioma
                    });
            }
            catch (Exception e)
            {
                SetErrorMessage(e.Message, target: SMCMessagePlaceholders.Centro);

                // Busca os dados da página
                PaginaFiltroViewModel filtro = new PaginaFiltroViewModel()
                {
                    SeqConfiguracaoEtapaPagina = modelo.SeqConfiguracaoEtapaPagina,
                    SeqConfiguracaoEtapa = modelo.SeqConfiguracaoEtapa,
                    SeqGrupoOferta = modelo.SeqGrupoOferta,
                    SeqInscricao = modelo.SeqInscricao,
                    Idioma = modelo.Idioma
                };
                PaginaCodigoAutorizacaoViewModel modeloNovo = InscricaoControllerService.BuscarPaginaCodigosAutorizacao(filtro);
                modeloNovo.CodigosAutorizacao = modelo.CodigosAutorizacao;
                return View("CodigoAutorizacao", modeloNovo);
            }
        }

        #endregion Código Autorização

        #region Formulário Inscrição

        [HttpGet]
        [SMCAuthorize(UC_INS_002_02_05.FORMULARIO_INSCRICAO)]
        public ActionResult FormularioInscricao(PaginaFiltroViewModel filtro)
        {
            // Busca os dados da página
            PaginaFormularioInscricaoViewModel modelo = InscricaoControllerService.BuscarPaginaFormularioInscricao(filtro);

            ValidarContinuarInscricao<PaginaFormularioInscricaoViewModel>(modelo);

            // Se a inscrição já está criada
            if (filtro.SeqInscricao > 0)
            {
                // Registra entrada na página
                InscricaoControllerService.IncluirHistoricoPagina(modelo);
            }
            else
            {
                // É obrigatório que a inscrição estaja criada, senão erro!
                throw new InscricaoInvalidaException();
            }

            return View(modelo);
        }

        [HttpPost]
        [SMCAuthorize(UC_INS_002_02_05.FORMULARIO_INSCRICAO)]
        public ActionResult SalvarFormularioInscricao(InscricaoDadoFormularioViewModel dados, PaginaFormularioInscricaoViewModel modelo)
        {
            try
            {
                dados.SeqConfiguracaoEtapaPaginaIdioma = modelo.SeqPaginaIdioma.Value;
                // atualiza os dados do candidato
                InscricaoControllerService.SalvarFormularioInscricao(dados, modelo.SeqInscricao);

                // Redireciona para a próxima página
                return RedirectToAction("UrlPagina",
                    new
                    {
                        token = modelo.TokenProximaPaginaEncrypted,
                        seqConfiguracaoEtapaPagina = modelo.SeqConfiguracaoEtapaPaginaProximaEncrypted,
                        seqConfiguracaoEtapa = modelo.SeqConfiguracaoEtapaEncrypted,
                        seqGrupoOferta = modelo.SeqGrupoOfertaEncrypted,
                        seqInscricao = modelo.SeqInscricaoEncrypted,
                        idioma = modelo.Idioma
                    });
            }
            catch (Exception e)
            {
                SetErrorMessage(e.Message, target: SMCMessagePlaceholders.Centro);

                // Busca os dados da página
                PaginaFiltroViewModel filtro = new PaginaFiltroViewModel()
                {
                    SeqConfiguracaoEtapaPagina = modelo.SeqConfiguracaoEtapaPagina,
                    SeqConfiguracaoEtapa = modelo.SeqConfiguracaoEtapa,
                    SeqGrupoOferta = modelo.SeqGrupoOferta,
                    SeqInscricao = modelo.SeqInscricao,
                    Idioma = modelo.Idioma
                };
                TempData["__CamposDadoFormulario"] = dados;
                PaginaFormularioInscricaoViewModel modeloNovo = InscricaoControllerService.BuscarPaginaFormularioInscricao(filtro);
                return View("FormularioInscricao", modeloNovo);
            }
        }

        [AllowAnonymous]
        [SMCAuthorize(UC_INS_002_02_05.FORMULARIO_INSCRICAO)]
        public ActionResult CarregarFormulario(long seqDadoFormulario, long seqInscricao, long seqFormularioSGF, long seqVisaoSGF, long seqProcesso, string tokenCssAlternativoSas, Guid? uidInscricaoOferta)
        {
            InscricaoDadoFormularioViewModel modelo = (TempData.ContainsKey("__CamposDadoFormulario")) ?
                                                       TempData["__CamposDadoFormulario"] as InscricaoDadoFormularioViewModel :
                                                       BuscarDadoFormulario(seqDadoFormulario, seqInscricao, seqFormularioSGF, seqVisaoSGF);

            modelo.TokenCssAlternativoSas = tokenCssAlternativoSas;
            if (TipoProcessoService.VerificaIntegraGPC(seqProcesso))
            {
                var seqElementoProcesso = FormularioService.BuscarSeqElemento(seqFormularioSGF, FONTE_EXTERNA.PROCESSO_GPI);
                var seqElementoUsuarioSAS = FormularioService.BuscarSeqElemento(seqFormularioSGF, FONTE_EXTERNA.USUARIO_SAS);

                var seqProcessoInscricao = seqProcesso.ToString();
                var seqUsuarioSAS = InscritoControllerService.BuscarInscrito().SeqUsuarioSas.ToString();

                modelo.AdicionarDado(seqElementoProcesso, seqProcessoInscricao, null, FONTE_EXTERNA.PROCESSO_GPI, null);
                modelo.AdicionarDado(seqElementoUsuarioSAS, seqUsuarioSAS, null, FONTE_EXTERNA.USUARIO_SAS, null);
            }

            modelo.UidInscricaoOferta = uidInscricaoOferta;
            return PartialView("_SGFIndex", modelo);
        }

        [SMCAuthorize(UC_INS_002_02_07.CONFIRMACAO_INSCRICAO)]
        public ActionResult RelatorioFormulario(long seqDadoFormulario, long seqInscricao, long seqFormularioSGF, long seqVisaoSGF)
        {
            InscricaoDadoFormularioViewModel modelo = this.BuscarDadoFormulario(seqDadoFormulario, seqInscricao, seqFormularioSGF, seqVisaoSGF);
            return PartialView("_SGFReport", modelo);
        }

        [NonAction]
        private InscricaoDadoFormularioViewModel BuscarDadoFormulario(long seqDadoFormulario, long seqInscricao, long seqFormularioSGF, long seqVisaoSGF)
        {
            InscricaoDadoFormularioViewModel dados = InscricaoControllerService.BuscarDadoFormulario(seqDadoFormulario);
            if (dados == null)
            {
                dados = new InscricaoDadoFormularioViewModel
                {
                    SeqVisao = seqVisaoSGF,
                    SeqFormulario = seqFormularioSGF,
                    SeqInscricao = seqInscricao
                };
            }

            // Busca o sequencial da primeira seção do formulário para apresentar
            // TODO: Permitir apresentação de formulários com várias seções
            //ViewBag.SeqSecao = formulario.Secoes.FirstOrDefault().Seq;
            ViewBag.SeqInscricao = seqInscricao;
            return dados;
        }

        #endregion Formulário Inscrição

        #region Upload Documentos

        [HttpGet]
        [SMCAuthorize(UC_INS_002_02_06.UPLOAD_DOCUMENTOS)]
        public ActionResult UploadDocumentos(PaginaFiltroViewModel filtro)
        {
            PaginaUploadDocumentosViewModel modelo = InscricaoControllerService.BuscarPaginaUploadDocumentos(filtro);

            ValidarContinuarInscricao<PaginaUploadDocumentosViewModel>(modelo);

            // Se a inscrição já está criada
            if (filtro.SeqInscricao > 0)
            {
                // Registra entrada na página
                InscricaoControllerService.IncluirHistoricoPagina(modelo);
            }
            else
            {
                // É obrigatório que a inscrição estaja criada, senão erro!
                throw new InscricaoInvalidaException();
            }

            return View(modelo);
        }

        [HttpPost]
        [SMCAuthorize(UC_INS_002_02_06.UPLOAD_DOCUMENTOS)]
        public ActionResult SalvarUploadDocumentos(PaginaUploadDocumentosViewModel modelo)
        {
            try
            {
                foreach (var item in modelo.DocumentosAdicionais)
                {
                    // Verifica se algum arquivo foi marcado como entrega posterior e se foi marcado o aceite do termo
                    if (modelo.DocumentosObrigatorios.Any(a => a.SeqDocumentoRequerido == item.SeqDocumentoRequerido && a.EntregaPosterior == true))
                    {
                        var tipoDescricao = modelo.DocumentosObrigatorios.First(a => a.SeqDocumentoRequerido == item.SeqDocumentoRequerido && a.EntregaPosterior == true).DescricaoTipoDocumento;
                        throw new DocumentoRequeridoAdicionalObrigatorioPosteriormenteException(tipoDescricao);
                    }

                    if (modelo.DocumentosGrupo.Any(a => a.SeqDocumentoRequerido == item.SeqDocumentoRequerido && a.EntregaPosterior == true))
                    {
                        var tipoDescricao = modelo.DocumentosGrupo.First(a => a.SeqDocumentoRequerido == item.SeqDocumentoRequerido && a.EntregaPosterior == true).DescricaoGrupoDocumentos;
                        throw new DocumentoRequeridoAdicionalObrigatorioPosteriormenteException(tipoDescricao);
                    }

                    item.ArquivoAnexado.FileData = SMCUploadHelper.GetFileData(item.ArquivoAnexado);
                }

                foreach (var item in modelo.DocumentosOpcionais)
                {
                    if (item.ArquivoAnexado != null)
                    {
                        item.ArquivoAnexado.FileData = SMCUploadHelper.GetFileData(item.ArquivoAnexado);
                    }
                }

                string messageErro = string.Empty;
                foreach (var item in modelo.DocumentosObrigatorios)
                {
                    if (item.ArquivoAnexado != null)
                    {
                        item.ArquivoAnexado.FileData = SMCUploadHelper.GetFileData(item.ArquivoAnexado);
                    }
                    else if (item.EntregaPosterior == false)
                    {
                        messageErro += string.Format(ExceptionsResource.DocumentoRequeridoObrigatorioNaoSubmetidoException,
                        item.DescricaoTipoDocumento);
                    }
                }

                foreach (var item in modelo.DocumentosGrupo)
                {
                    if (item.ArquivoAnexado != null)
                    {
                        item.ArquivoAnexado.FileData = SMCUploadHelper.GetFileData(item.ArquivoAnexado);
                    }
                    else if (item.EntregaPosterior == false)
                    {
                        messageErro += string.Format(ExceptionsResource.DocumentoRequeridoObrigatorioNaoSubmetidoException,
                        item.DescricaoGrupoDocumentos);
                    }
                }

                if (!string.IsNullOrEmpty(messageErro))
                {
                    throw new Exception(messageErro);
                }

                // Verifica se algum arquivo foi marcado como entrega posterior e se foi marcado o aceite do termo
                if (modelo.DocumentosObrigatorios.Any(a => a.EntregaPosterior == true) || modelo.DocumentosGrupo.Any(a => a.EntregaPosterior == true))
                {
                    if (modelo.ExibeTermoPrincipalResponsabilidadeEntrega == false)
                        throw new DocumentoRequeridoExibirTermoResponsabilidadeEntregaException();
                }

                // Se a inscrição não está informada erro
                if (modelo.SeqInscricao <= 0)
                    throw new InscricaoInvalidaException();

                // Salva o código de autorizaçãoo da inscrição
                InscricaoControllerService.SalvarUploadDocumentos(modelo);

                // Redireciona para a próxima página
                return RedirectToAction("UrlPagina",
                    new
                    {
                        token = modelo.TokenProximaPaginaEncrypted,
                        seqConfiguracaoEtapaPagina = modelo.SeqConfiguracaoEtapaPaginaProximaEncrypted,
                        seqConfiguracaoEtapa = modelo.SeqConfiguracaoEtapaEncrypted,
                        seqGrupoOferta = modelo.SeqGrupoOfertaEncrypted,
                        seqInscricao = modelo.SeqInscricaoEncrypted,
                        idioma = modelo.Idioma
                    });
            }
            catch (Exception e)
            {
                SetErrorMessage(e.Message, target: SMCMessagePlaceholders.Centro);
                foreach (var grupo in modelo.DocumentosGrupo)
                {
                    grupo.DocumentosRequeridosGrupo = this.InscricaoControllerService.BuscarTiposDocumentoGrupo(grupo.SeqGrupoDocumentoRequerido);
                }
                modelo.DocumentosOpcionaisUpload = InscricaoControllerService.BuscarDocumentosOpcionais(modelo.SeqConfiguracaoEtapa);
                //return View("UploadDocumentos", modelo);
                // Redireciona para a próxima página
                // Busca os dados da página
                PaginaFiltroViewModel filtro = new PaginaFiltroViewModel()
                {
                    SeqConfiguracaoEtapaPagina = modelo.SeqConfiguracaoEtapaPagina,
                    SeqConfiguracaoEtapa = modelo.SeqConfiguracaoEtapa,
                    SeqGrupoOferta = modelo.SeqGrupoOferta,
                    SeqInscricao = modelo.SeqInscricao,
                    Idioma = modelo.Idioma
                };
                PaginaUploadDocumentosViewModel modeloNovo = InscricaoControllerService.BuscarPaginaUploadDocumentos(filtro);
                modeloNovo.DocumentosOpcionais = modelo.DocumentosOpcionais;
                modeloNovo.DocumentosGrupo = modelo.DocumentosGrupo;
                modeloNovo.DocumentosObrigatorios = modelo.DocumentosObrigatorios;
                //modeloNovo.DocumentosOpcionaisUpload = modelo.DocumentosOpcionaisUpload;
                modeloNovo.DocumentosAdicionais = modelo.DocumentosAdicionais;
                //modeloNovo.DocumentosAdicionaisUpload = modelo.DocumentosAdicionaisUpload;
                modeloNovo.ExibeTermoPrincipalResponsabilidadeEntrega = modelo.ExibeTermoPrincipalResponsabilidadeEntrega;
                modeloNovo.ExibirMensagemInformativaConversaoPDF = modelo.ExibirMensagemInformativaConversaoPDF;

                return View("UploadDocumentos", modeloNovo);
            }
        }

        /// <summary>
        /// Action para download de arquivo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[SMCAuthorize(UC_INS_002_02_06.UPLOAD_DOCUMENTOS)]
        //public ActionResult DownloadArquivo(string guidFile, string name, string type)
        //{
        //    if (Guid.TryParse(guidFile, out Guid guid))
        //    {
        //        var data = SMCUploadHelper.GetFileData(new SMCUploadFile { GuidFile = guidFile });
        //        if (data != null)
        //        {
        //            return File(data, type, name);
        //        }
        //    }
        //    return DownloadDocumento(new SMCEncryptedLong(guidFile));
        //}

        #endregion Upload Documentos

        #region Confirmar Inscrição

        [HttpGet]
        [SMCAuthorize(UC_INS_002_02_07.CONFIRMACAO_INSCRICAO)]
        public ActionResult ConfirmarInscricao(PaginaFiltroViewModel filtro)
        {
            PaginaConfirmarInscricaoViewModel modelo = InscricaoControllerService.BuscarPaginaConfirmarInscricao(filtro);
            ValidarContinuarInscricao<PaginaConfirmarInscricaoViewModel>(modelo);

            // Se a inscrição já está criada
            if (filtro.SeqInscricao > 0)
            {
                // Registra entrada na página
                InscricaoControllerService.IncluirHistoricoPagina(modelo);
            }
            else
            {
                // É obrigatório que a inscrição estaja criada, senão erro!
                throw new InscricaoInvalidaException();
            }

            ViewBag.Confirmacao = true;

            return View(modelo);
        }

        [HttpPost]
        [SMCAuthorize(UC_INS_002_02_07.CONFIRMACAO_INSCRICAO)]
        public ActionResult SalvarConfirmarInscricao(PaginaConfirmarInscricaoViewModel modelo)
        {
            try
            {
                // Se a inscrição não está informada erro
                if (modelo.SeqInscricao <= 0)
                    throw new InscricaoInvalidaException();

                var dadosInscricaoResumida = InscricaoService.BuscarInscricaoResumida(modelo.SeqInscricao);

                if (!ValidarDadosInscritoPreenchidosParaProcesso(dadosInscricaoResumida.SeqInscrito, dadosInscricaoResumida.UidProcesso))
                {
                    TempData["__cadastroIncompleto__"] = true;
                    return SMCRedirectToAction("Cadastrar", "Inscrito", new { area = "INS", uidProcesso = dadosInscricaoResumida.UidProcesso });
                }

                ValidarContinuarConfirmacaoInscrito(modelo);

                //verifica se é necessario confirmar a conversao de não pdf para pdf
                if (!string.IsNullOrEmpty(modelo.OrientacaoAceiteConversaoArquivosPDF) &&
                    !string.IsNullOrEmpty(modelo.TermoAceiteConversaoArquivosPDF))
                {
                    if (modelo.AceiteConversaoPDF == false)
                    {
                        throw new ArquivoAssociadoInscricaoConvertidoPDFException();
                    }
                }

                var pagina = InscricaoControllerService.BuscarConfiguracaoEtapaPagina(modelo.SeqConfiguracaoEtapa, TOKENS.PAGINA_COMPROVANTE_INSCRICAO);

                if (pagina != null)
                {
                    // Gera o array de bytes do arquivo do comprovante
                    PaginaFiltroViewModel filtro = new PaginaFiltroViewModel()
                    {
                        SeqConfiguracaoEtapaPagina = pagina.Seq,
                        SeqConfiguracaoEtapa = modelo.SeqConfiguracaoEtapa,
                        SeqGrupoOferta = modelo.SeqGrupoOferta,
                        SeqInscricao = modelo.SeqInscricao,
                        Idioma = modelo.Idioma
                    };
                    var arquivo = GerarArquivoPdfComprovante(filtro).TransformToArray<byte?>();

                    // Confirma a inscrição
                    InscricaoControllerService.FinalizarInscricao(modelo.SeqInscricao, modelo.AceiteConversaoPDF, modelo.ConsentimentoLGPD, arquivoComprovante: arquivo);
                }
                else
                {
                    // Confirma a inscrição
                    InscricaoControllerService.FinalizarInscricao(modelo.SeqInscricao, modelo.AceiteConversaoPDF, modelo.ConsentimentoLGPD, arquivoComprovante: null);
                }

                // Verifica consistência de Ocupação de vagas do financiamento estudantil
                if (TipoProcessoService.VerificaPossuiConsistencia(new TipoProcessoConsistenciaData() { SeqInscricao = modelo.SeqInscricao, TipoConsistencia = TipoConsistencia.OcupacaoVagasFinanciamentoEstudantil }))
                {
                    var ofertas = InscricaoService.BuscarInscricaoOfertas(modelo.SeqInscricao).Select(f => f.SeqOferta).ToArray();
                    var ofertaIncricoes = OfertaService.BuscarContagemInscrioesDasOfertasPorSituacao(ofertas, new string[] { TOKENS.SITUACAO_CANDIDATO_CONFIRMADO, TOKENS.SITUACAO_CANDIDATO_SELECIONADO, TOKENS.SITUACAO_CANDIDATO_EXCEDENTE, TOKENS.SITUACAO_CONVOCADO });
                    var lista = ofertaIncricoes.Where(f => f.Vagas <= f.InscricoesConfirmadas).Select(f => f.Descricao);
                    if (lista.Any())
                    {
                        SetAlertMessage(string.Format(UIResource.Mensagem_VagasOcupadas, string.Join(", ", lista)));
                    }
                }

                // Redireciona para a próxima página
                return SMCRedirectToAction("UrlPagina", "Inscricao",
                    new
                    {
                        token = modelo.TokenProximaPaginaEncrypted,
                        seqConfiguracaoEtapaPagina = modelo.SeqConfiguracaoEtapaPaginaProximaEncrypted,
                        seqConfiguracaoEtapa = modelo.SeqConfiguracaoEtapaEncrypted,
                        seqGrupoOferta = modelo.SeqGrupoOfertaEncrypted,
                        seqInscricao = modelo.SeqInscricaoEncrypted,
                        idioma = modelo.Idioma
                    });
            }
            catch (Exception e)
            {
                SetErrorMessage(e.Message, target: SMCMessagePlaceholders.Centro);

                // Busca as informações da página
                PaginaFiltroViewModel filtro = new PaginaFiltroViewModel()
                {
                    SeqConfiguracaoEtapaPagina = modelo.SeqConfiguracaoEtapaPagina,
                    SeqConfiguracaoEtapa = modelo.SeqConfiguracaoEtapa,
                    SeqGrupoOferta = modelo.SeqGrupoOferta,
                    SeqInscricao = modelo.SeqInscricao,
                    Idioma = modelo.Idioma
                };
                PaginaConfirmarInscricaoViewModel modeloNovo = InscricaoControllerService.BuscarPaginaConfirmarInscricao(filtro);
                if (modeloNovo.TokenSituacaoAtual == TOKENS.SITUACAO_INSCRICAO_CANCELADA)
                {
                    return SMCRedirectToAction("Index", "Home", new { area = "" });
                }
                return View("ConfirmarInscricao", modeloNovo);
            }
        }

        #endregion Confirmar Inscrição

        #region Instruções Finais

        [HttpGet]
        [SMCAuthorize(UC_INS_002_02_08.FINALIZACAO_INSCRICAO)]
        public ActionResult InstrucoesFinais(PaginaFiltroViewModel filtro)
        {
            PaginaInstrucoesFinaisViewModel modelo = InscricaoControllerService.BuscarPaginaInstrucoesFinais(filtro);

            //Caso a oferta esteja indiponivel mas o aluno ja tenha feito a inscrição
            foreach (var item in modelo.FluxoPaginas.OfType<FluxoPaginaViewModel>())
            {
                if (item.Token == TOKENS.PAGINA_SELECAO_OFERTA)
                {
                    item.Alerta = string.Empty;
                }
            }

            // Bug 37910
            // Quando volta a inscrição do inscrito para uma situação anterior após ter sido concluída, o mesmo dava F5 na tela de instruções finais e mudava a navegação para página final sem finalizar a inscrição novamente
            // Não permitir acessar esta página caso a inscrição não esteja finalizada ou concluída.
            if (modelo.TokenSituacaoAtual == TOKENS.SITUACAO_INSCRICAO_INICIADA)
                return ThrowRedirect(new SMCApplicationException($"Não é possível acessar a página {modelo.Titulo} pois sua inscrição encontra-se na situação {modelo.DescricaoSituacaoAtual}."), "Index", "Home", new System.Web.Routing.RouteValueDictionary { { "area", "" } });

            // Se a inscrição já está criada
            if (filtro.SeqInscricao > 0)
            {
                // Registra entrada na página
                InscricaoControllerService.IncluirHistoricoPagina(modelo);
            }
            else
            {
                // É obrigatório que a inscrição estaja criada, senão erro!
                throw new InscricaoInvalidaException();
            }

            if (Session["__CSSLayout__"] == null)
            {
                Session["__CSSLayout__"] = modelo.UrlCss;
            }



            return View(modelo);
        }

        #endregion Instruções Finais

        #region Comprovante Inscrição

        [NonAction]
        private byte[] GerarArquivoPdfComprovante(PaginaFiltroViewModel filtro)
        {
            PaginaComprovanteInscricaoViewModel modelo = this.InscricaoControllerService.BuscarPaginaComprovanteInscricao(filtro);
            modelo.DadosInscrito.CamposInscritoVisiveis = ConfigurarVisibilidadeCamposPorProcesso(modelo.SeqProcesso);
            //SMCViewAsPdf ret = new SMCViewAsPdf("ComprovanteInscricao", modelo);
            //ret.Settings.EnableIntelligentShrinking = false;
            //return ret.BuildPdf(this.ControllerContext);
            //var payload = new PayloadGenerator.SMS(filtro.SeqInscricao.ToString());

            if (!string.IsNullOrEmpty(modelo.UidInscricaoOferta.ToString()))
            {
                //var qrCode = SMCQRCodeHelper.GenerateQRCode(modelo.UidInscricaoOferta.ToString());
                //modelo.Qrcode = string.Format("data:image/gif;base64,{0}", Convert.ToBase64String(qrCode));
            }

            string nomeComprovante = "ComprovanteInscricao";

            var arquivoComprovantePDF = RenderPdfView(nomeComprovante, string.Empty, modelo, new GridOptions()
            {
                PageMargins = new MarginInfo()
                {
                    Left = 3,
                    Right = 2,
                    Top = 3,
                    Bottom = 2
                }
            }).FileContents;

            var options = new SMCPdfSaveOptions()
            {
                File = new SMCPdfFile()
                {
                    FileData = arquivoComprovantePDF,
                    Name = "ComprovanteInscricao.pdf",
                    Type = "pdf"

                }
            };

            return SMCPDFHelper.ConvertFilePDF(options);
        }

        // Obs.: Não apagar a action abaixo comentada. Caso necessário realizar testes
        [HttpGet]
        [SMCAuthorize(UC_INS_002_02_10.COMPROVANTE_INSCRICAO)]
        public ActionResult ComprovanteInscricao(PaginaFiltroViewModel filtro, string token, int pdf = 0)
        {
            if (token != TOKEN_TESTE)
            {
                RedirectToAction("Index", "Home");
            }

            var dadosInscricao = InscricaoService.BuscarDadosComprovanteInscricao(filtro.SeqInscricao);
            filtro.Idioma = dadosInscricao.Idioma;
            filtro.SeqConfiguracaoEtapa = dadosInscricao.SeqConfiguracaoEtapa;
            filtro.SeqGrupoOferta = dadosInscricao.SeqGrupoOferta;

            var pagina = InscricaoControllerService.BuscarConfiguracaoEtapaPagina(filtro.SeqConfiguracaoEtapa, TOKENS.PAGINA_COMPROVANTE_INSCRICAO);
            filtro.SeqConfiguracaoEtapaPagina = pagina.Seq;

            PaginaComprovanteInscricaoViewModel modelo = this.InscricaoControllerService.BuscarPaginaComprovanteInscricao(filtro, dadosInscricao.SeqInscrito);

            modelo.DadosInscrito.CamposInscritoVisiveis = ConfigurarVisibilidadeCamposPorProcesso(modelo.SeqProcesso);

            string nomeComprovante = "ComprovanteInscricao";

            if (!string.IsNullOrEmpty(modelo.UidInscricaoOferta.ToString()))
            {
                //var qrCode = SMCQRCodeHelper.GenerateQRCode(modelo.UidInscricaoOferta.ToString(), null, null, null, 10, 5);
                //modelo.Qrcode = string.Format("data:image/gif;base64,{0}", Convert.ToBase64String(qrCode));
            }

            if (pdf == 1)
            {
                return RenderPdfView(nomeComprovante, "ComprovanteInscricao.pdf", modelo, new GridOptions()
                {
                    PageMargins = new MarginInfo()
                    {
                        Left = 3,
                        Right = 2,
                        Top = 3,
                        Bottom = 2
                    }
                });
            }
            else
            {
                return View(nomeComprovante, modelo);
            }
        }

        [HttpGet]
        [SMCAuthorize(UC_INS_002_02_10.COMPROVANTE_INSCRICAO)]
        public ActionResult AtualizarComprovanteInscricao(SMCEncryptedLong seqInscricao, string token)
        {
            if (token != TOKEN_TESTE)
            {
                RedirectToAction("Index", "Home");
            }

            var comprovante = ((FileContentResult)ComprovanteInscricao(new PaginaFiltroViewModel() { SeqInscricao = seqInscricao }, token, 1));
            var options = new SMCPdfSaveOptions()
            {
                File = new SMCPdfFile()
                {
                    FileData = comprovante.FileContents,
                    Name = "ComprovanteInscricao.pdf",
                    Type = "pdf"
                }
            };
            var comprovantePDF = SMCPDFHelper.ConvertFilePDF(options);
            InscricaoService.AlterarComprovanteInscricao(new DadosComprovanteInscricaoData() { SeqInscricao = seqInscricao, DadosComprovante = comprovantePDF });

            return comprovante;
        }

        #endregion Comprovante Inscrição

        #region Taxas

        [SMCAuthorize(UC_INS_002_02_09.TAXA_INSCRICAO)]
        public ActionResult BoletoInscricao(SMCEncryptedLong id, int pdf = 1)
        {
            try
            {
                if (!Convert.ToBoolean(ConfigurationManager.AppSettings["ApiBoleto"]))
                {
                    var modelo = this.InscricaoControllerService.GerarBoletoInscricao(id);
                    ViewBag.Evento = this.InscricaoControllerService.BuscarDescricaoEventoFinanceiroInscricao(id.Value);
                    if (pdf == 1)
                    {
                        return RenderPdfView(modelo, new GridOptions(proxy: "http://proxy.pucminas.br:80") { PageMargins = new MarginInfo { Left = 10, Right = 10, Top = 10, Bottom = 10 } });
                    }
                    else
                    {
                        return View(modelo);
                    }
                }

                // Caso o ApiBoleto esteja ativo, será retornado apenas o modelo.Titulo.NumeroDocumento
                var seqTitulo = this.InscricaoControllerService.BuscarSeqTitulo(id);
                var filtro = new { SeqTitulo = seqTitulo, Html = pdf == 0, Token = SMCDESCrypto.Encrypt(ConfigurationManager.AppSettings[WEB_API_REST.TOKEN_BOLETO_KEY]) };
                var url = $"{ConfigurationManager.AppSettings[WEB_API_REST.BASE_URL_KEY]}{WEB_API_REST.EMITIR_BOLETO_INSCRITO}";
                var boleto = SMCRest.PostJson(url, filtro, cancellationTimer: int.Parse(ConfigurationManager.AppSettings[WEB_API_REST.CANCELLATION_TIME_KEY]));
                return File(Convert.FromBase64String(boleto), "application/pdf");
            }
            catch (Exception e)
            {
                this.SetErrorMessage(e.Message);
                return BackToAction();
            }
        }

        #endregion Taxas

        #region Download de arquivos

        /// <summary>
        /// Action para download de arquivo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [SMCAllowAnonymous]
        public ActionResult DownloadDocumento(SMCEncryptedLong Id)
        {
            return RedirectToAction("DownloadDocumento", "Home", new { Area = "", Id = Id });
        }

        #endregion Download de arquivos

        #region Idioma

        [NonAction]
        public void VerificarIdioma(IIdioma modelo)
        {
            if (modelo.Idioma != (GetCurrentLanguage().HasValue ? GetCurrentLanguage().Value : SMCLanguage.Portuguese))
            {
                base.ChangeCulture(modelo.Idioma);
            }
        }

        #endregion Idioma

        #region Ingressos

        /// <summary>
        /// Pagina de Ingressos
        /// </summary>
        /// <param name="seqInscricao">Sequencial da Inscrição</param>
        /// <returns>Pagina dos ingressos</returns>    
        [SMCAuthorize(UC_INS_002_02_10.COMPROVANTE_INSCRICAO)]
        public ActionResult Ingressos(SMCEncryptedLong seqInscricao)
        {
            IngressoViewModel modelo = InscricaoService.BuscarIngressos(seqInscricao).Transform<IngressoViewModel>();

            //var horas = modelo.QuantidadeHorasAberturaCheckin.Val



            var validaBotao = InscricaoControllerService.ValidaBotaoIngressos(seqInscricao);

            if (!TipoProcessoService.BuscarTipoProcessoPorProcesso(modelo.SeqProcesso).GestaoEventos)
            {
                return BackToAction();
            }

            if (!validaBotao.Item2)
            {
                SetErrorMessage(validaBotao.Item1, "Erro", SMCMessagePlaceholders.Centro);
                return BackToAction();
            }

            //Se a situação for diferente do das situações permitidas para visualização dos ingressos
            if (!(modelo.TokenSituacaoAtual == TOKENS.SITUACAO_INSCRICAO_CONFIRMADA || modelo.TokenSituacaoAtual == TOKENS.SITUACAO_INSCRICAO_DEFERIDA))
            {
                return RedirectToAction("IndexProcesso", "Home", new { area = "", @uidProcesso = modelo.UidProcesso });
            }

            return View(modelo);
        }

        public ActionResult MenuLateralDireitaIngressos(InscricoesFiltroViewModel filtro)
        {
            // Busca o sequencial do inscrito logado
            long? seqInscrito = InscritoControllerService.BuscarSeqInscritoLogado();

            // Se usuário não possui inscrito, redireciona para o cadastro de um
            if (!seqInscrito.HasValue)
            {
                return RedirectToAction("Cadastrar", "Inscrito", new { area = "INS" });
            }

            // Busca as inscrições do inscrito logado no processo
            filtro.SeqInscrito = seqInscrito.Value;
            SMCPagerModel<InscricaoProcessoItemViewModel> dados = InscricaoControllerService.BuscarInscricoesProcesso(filtro);

            dados.SelectedValues = new List<object>();

            dados.SelectedValues.Add(filtro.TokenResource);

            dados.ForEach(f =>
            {

                var validaBotaoIngresso = InscricaoControllerService.ValidaBotaoIngressos(f.SeqInscricao);
                f.MensagemInformativaCheckin = validaBotaoIngresso.Item1;
                f.HabilitarBotaoCheckin = validaBotaoIngresso.Item2;
            });

            ValidarBotoesVisuaslizarEmitirComprovante(dados);

            return PartialView("_MenuLateralDireitaIngressos", dados);
        }

        private void ValidarBotoesVisuaslizarEmitirComprovante(SMCPagerModel<InscricaoProcessoItemViewModel> modelo)
        {
            foreach (var item in modelo)
            {
                item.PosssuiTaxas = InscricaoService.VerificarExistenciaTaxaInscricao(item.SeqInscricao);
                var mensagemEmissaoComprovante = InscricaoService.VerificarPermissaoEmitirComprovante(item.SeqInscricao);
                item.PermiteEmitirComprovante = string.IsNullOrEmpty(mensagemEmissaoComprovante);
                item.SeqArquivoComprovante = InscricaoService.BuscarSeqArquivoComprovante(item.SeqInscricao).GetValueOrDefault();
            }
        }

        #endregion Ingressos

        #region Formulario Impacto

        [SMCAuthorize(UC_INS_002_07_01.VISUALIZAR_FORMULARIO_IMPACTO)]
        public ActionResult CarregarFormularioImpacto(long seqDadoFormulario, long seqInscricao, long seqFormularioSGF, long seqVisaoSGF, long seqProcesso, string uidProcesso)
        {
            InscricaoDadoFormularioViewModel modelo = (TempData.ContainsKey("__CamposDadoFormulario")) ?
                                                       TempData["__CamposDadoFormulario"] as InscricaoDadoFormularioViewModel :
                                                       BuscarDadoFormulario(seqDadoFormulario, seqInscricao, seqFormularioSGF, seqVisaoSGF);
            modelo.UidProcesso = uidProcesso;

            return PartialView("_SGFImpacto", modelo);
        }

        [SMCAuthorize(UC_INS_002_07_01.VISUALIZAR_FORMULARIO_IMPACTO)]
        public ActionResult SalvarFormularioImpacto(InscricaoDadoFormularioViewModel dados)
        {
            InscricaoService.SalvarFormularioImpacto(SMCMapperHelper.Create<InscricaoDadoFormularioData>(dados));

            SetSuccessMessage("Agradecemos por avaliar como foi a experiência em nosso evento.", "Recebemos sua avaliação!", closeTimer: 3000);

            return SMCRedirectToAction("indexProcesso", "Home", new { uidProcesso = dados.UidProcesso, area = "" });
        }
        #endregion


        /// <summary>
        /// Valida a continuaçao confirmação do inscrito
        ///<paramref name="model"/>
        /// </summary>
        private void ValidarContinuarConfirmacaoInscrito(PaginaConfirmarInscricaoViewModel model)
        {
            string tokenSituacaoAtual = InscricaoService.BuscarSituacaoAtualInscricao(model.SeqInscricao);

            if (tokenSituacaoAtual != null && tokenSituacaoAtual == TOKENS.SITUACAO_INSCRICAO_CANCELADA)
            {
                string artigo;
                string tipoInscricao;
                switch (model.TokenResource)
                {
                    case TOKENS.TOKEN_RESOURCE_AGENDAMENTO:
                        artigo = "e";
                        tipoInscricao = "agendamento";
                        break;
                    case TOKENS.TOKEN_RESOURCE_ENTREGA_DOCUMENTACAO:
                        artigo = "a";
                        tipoInscricao = "entrega de documentação";
                        break;
                    default:
                        artigo = "a";
                        tipoInscricao = "inscrição";
                        break;
                }

                ThrowRedirect(new SMCApplicationException($"Ess{artigo} {tipoInscricao} está cancelada, não é permitido realizar alterações."), "Index", "Home", new System.Web.Routing.RouteValueDictionary { { "area", "" } });
            }
        }

        /// <summary>
        /// Valida a continuaçao da inscriçao
        /// <paramref name="model"/>
        /// </summary>
        private void ValidarContinuarInscricao<T>(T model)
        {
            Type modelo = model.GetType();
            ValidarProcesso((long)modelo.GetProperty("SeqProcesso").GetValue(model));
            if (modelo.GetProperty("TokenSituacaoAtual").GetValue(model) != null && modelo.GetProperty("TokenSituacaoAtual").GetValue(model).ToString() == TOKENS.SITUACAO_INSCRICAO_CANCELADA)
            {
                ThrowRedirect(new SMCApplicationException($"Não é possível acessar a página {modelo.GetProperty("Titulo").GetValue(model)}. {modelo.GetProperty("DescricaoSituacaoAtual").GetValue(model)}."), "Index", "Home", new System.Web.Routing.RouteValueDictionary { { "area", "" } });
            }

            if (!(bool)modelo.GetProperty("InscricaoIniciada").GetValue(model))
            {
                switch (modelo.GetProperty("TokenResource").GetValue(model))
                {
                    case TOKENS.TOKEN_RESOURCE_AGENDAMENTO:
                        throw new InscricaoJaFinalizadaAgendamentoException();
                    case TOKENS.TOKEN_RESOURCE_ENTREGA_DOCUMENTACAO:
                        throw new InscricaoJaFinalizadaEntregaDocumentacaoException();
                    default:
                        throw new InscricaoJaFinalizadaInscricaoException();
                }
            }

            ValidarSession(modelo.GetProperty("UrlCss").GetValue(model).ToString(), Guid.Parse(modelo.GetProperty("UidProcesso").GetValue(model).ToString()));
        }

        /// <summary>
        /// Valida status do processo cancelado ou encerrado
        /// </summary>
        /// <param name="seqProcesso">Sequencial do processo</param>
        private void ValidarProcesso(long seqProcesso)
        {
            var processo = ProcessoService.BuscarProcesso(seqProcesso);

            if (processo.DataEncerramento.HasValue && processo.DataEncerramento <= DateTime.Now.Date)
            {
                ThrowRedirect(new ProcessoEncerradoException(), "Index", "Home", new System.Web.Routing.RouteValueDictionary { { "area", "" } });
            }

            if (processo.DataCancelamento.HasValue && processo.DataCancelamento <= DateTime.Now.Date)
            {
                ThrowRedirect(new ProcessoCanceladoException(), "Index", "Home", new System.Web.Routing.RouteValueDictionary { { "area", "" } });
            }
        }

        [SMCAllowAnonymous]
        [HttpPost]
        public JsonResult DadosFormularioSeminarioSGF(long seqProjeto, long seqProcesso, SMCEncryptedLong seqInscricao)
        {
            var dadosFormulario = InscricaoService.DadosFormularioSeminarioSGF(seqProjeto, seqProcesso, seqInscricao.Value);

            var retorno = new
            {
                AreaConhecimento = dadosFormulario.AreaConhecimento,
                NomeOrientador = dadosFormulario.NomeOrientador,
                EmailOrientador = dadosFormulario.EmailOrientador,
                Alunos = dadosFormulario.Alunos.Select(s => new { Nome = s })
            };

            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Configura a visibilidade dos campos por processo
        /// </summary>
        /// <param name="seqProcesso">Sequencial do processo</param>
        /// <return>Dados da visibilidade do processo</return>
        private ProcessoCamposInscritoViewModel ConfigurarVisibilidadeCamposPorProcesso(long seqProcesso)
        {
            ProcessoCamposInscritoViewModel modelo = new ProcessoCamposInscritoViewModel();

            List<ProcessoCampoInscritoData> camposInscritoProcesso = ProcessoCampoInscritoService.BuscarCamposIncritosPorProcesso(seqProcesso);

            foreach (var campo in camposInscritoProcesso)
            {
                if (campo.CampoInscrito == CampoInscrito.CPF)
                {
                    modelo.ExibirCPF = true;
                }

                if (campo.CampoInscrito == CampoInscrito.DataNascimento)
                {
                    modelo.ExibirDataNascimento = true;
                }

                if (campo.CampoInscrito == CampoInscrito.Email)
                {
                    modelo.ExibirEmail = true;
                }

                if (campo.CampoInscrito == CampoInscrito.Endereco)
                {
                    modelo.ExibirEndereco = true;
                }

                if (campo.CampoInscrito == CampoInscrito.EstadoCivil)
                {
                    modelo.ExibirEstadoCivil = true;
                }

                if (campo.CampoInscrito == CampoInscrito.Filiacao)
                {
                    modelo.ExibirFiliacao = true;
                }

                if (campo.CampoInscrito == CampoInscrito.Nacionalidade)
                {
                    modelo.ExibirNacionalidade = true;
                }

                if (campo.CampoInscrito == CampoInscrito.Naturalidade)
                {
                    modelo.ExibirNaturalidade = true;
                }

                if (campo.CampoInscrito == CampoInscrito.Nome)
                {
                    modelo.ExibirNome = true;
                }

                if (campo.CampoInscrito == CampoInscrito.NumeroIdentidade)
                {
                    modelo.ExibirNumeroIdentidade = true;
                }

                if (campo.CampoInscrito == CampoInscrito.OrgaoEmissorIdentidade)
                {
                    modelo.ExibirOrgaoEmissorIdentidade = true;
                }

                if (campo.CampoInscrito == CampoInscrito.OutrosEndereçosEletronicos)
                {
                    modelo.ExibirOutrosEnderecosEletronicos = true;
                }

                if (campo.CampoInscrito == CampoInscrito.PaisOrigem)
                {
                    modelo.ExibirPaisOrigem = true;
                }

                if (campo.CampoInscrito == CampoInscrito.Passaporte)
                {
                    modelo.ExibirPassaporte = true;
                }

                if (campo.CampoInscrito == CampoInscrito.Sexo)
                {
                    modelo.ExibirSexo = true;
                }

                if (campo.CampoInscrito == CampoInscrito.Telefone)
                {
                    modelo.ExibirTelefone = true;
                }

                if (campo.CampoInscrito == CampoInscrito.UfIdentidade)
                {
                    modelo.ExibirUfIdentidade = true;
                }
            }
            return modelo;
        }

        private bool ValidarDadosInscritoPreenchidosParaProcesso(long seqInscrito, Guid uidProcesso)
        {
            try
            {
                return InscritoService.ValidarDadosInscritoPreenchidosParaProcesso(seqInscrito, uidProcesso);
            }
            catch (Exception)
            {
                //Se existir alguma exeção significa que a os dados não estão preenchidos corretamente
                return false;
            }
        }

        private void ValidarSession(string urlCss, Guid uidProcesso)
        {
            if (Session["__CSSLayout__"] == null)
            {
                Session["__CSSLayout__"] = urlCss;
            }

            if (Session[GuidProcesso] == null)
            {
                Session[GuidProcesso] = uidProcesso;
            }
        }

        private void ValidarSessionSelecaoOferta(long seqGrupoOferta)
        {
            if (Session[GuidProcesso] == null)
            {
                var uidProcesso = GrupoOfertaService.BuscarSeqProcessoPorSeqGrupoOferta(seqGrupoOferta);
                Session[GuidProcesso] = uidProcesso;
            }
        }

    }
}