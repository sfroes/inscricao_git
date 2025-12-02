using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using SMC.DadosMestres.ServiceContract.Areas.SHA.Data;
using SMC.Framework;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.Security;
using SMC.Framework.UI.Mvc.Util;
using SMC.GPI.Inscricao.Areas.INS.Services;
using SMC.GPI.Inscricao.Models;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Areas.INS.Constants;
using SMC.Inscricoes.Service.Areas.INS.Services;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using SMC.Inscricoes.UI.Mvc.Attributes;

namespace SMC.GPI.Inscricao.Controllers
{
    public class HomeController : SMCControllerBase
    {
        private static readonly string GuidProcesso = "GuidProcesso";
        private static readonly string ProcessoAtualSeq = "ProcessoAtualSeqFromLink";
        #region Services

        private ProcessoControllerService ProcessoControllerService
        {
            get { return this.Create<ProcessoControllerService>(); }
        }

        private InscritoControllerService InscritoControllerService
        {
            get { return this.Create<InscritoControllerService>(); }
        }

        private InscricaoControllerService InscricaoControllerService
        {
            get { return this.Create<InscricaoControllerService>(); }
        }
        private ProcessoService ProcessoService
        {
            get { return this.Create<ProcessoService>(); }
        }

        private InscricaoOfertaService InscricaoOfertaService
        {
            get { return this.Create<InscricaoOfertaService>(); }
        }


        private DadosMestres.ServiceContract.Areas.GED.Interfaces.ITipoDocumentoService TipoDocumentoDadosMestresService
        {
            get
            {
                return this.Create<DadosMestres.ServiceContract.Areas.GED.Interfaces.ITipoDocumentoService>();
            }
        }
        private IArquivoAnexadoService ArquivoAnexadoService => Create<IArquivoAnexadoService>();
        private IInscritoService InscritoService => Create<IInscritoService>();
        private IInscricaoService InscricaoService { get => Create<IInscricaoService>(); }


        #endregion Services

        #region Home

        /// <summary>
        /// Página Home
        /// </summary>
        [SMCAuthorize(UC_INS_002_03_01.HOME_INSCRICOES)]
        public ActionResult Index()
        {
            string returnUrl;
            SMCFederationHelper.GetUrl(out returnUrl);
            // Verifica se o usuário logado possui cadastro de inscrito
            long? seqInscrito = InscritoControllerService.BuscarSeqInscritoLogado();
            // Se usuário não possui inscrito, redireciona para o cadastro de um
            if (!seqInscrito.HasValue)
            {
                //verifica se tem o uidProcesso no get da url de retorno
                string pattern = @"uidProcesso=([\w-]+)";
                Match match = Regex.Match(returnUrl, pattern);
                if (match.Success)
                {
                    Guid uidProcesso = Guid.Parse(match.Groups[1].Value);
                    TempData["___seqProcessoAtual___"] = uidProcesso;
                    return RedirectToAction("Cadastrar", "Inscrito", new { area = "INS", uidProcesso = uidProcesso });
                }
                else
                {
                    return RedirectToAction("Cadastrar", "Inscrito", new { area = "INS" });
                }
            }

            if (returnUrl?.Length > 0)
            {
                return Redirect(returnUrl);
            }


            //Verifica se a home do usuário possui um uid
            if (Session[GuidProcesso] != null)
            {
                return RedirectToAction("IndexProcesso", new { area = string.Empty, uidProcesso = Session[GuidProcesso] });
            }
            // Se encontrou o inscrito, apresenta as inscrições e processos abertos
            return View(new ProcessoAbertoFiltroViewModel());
        }

        /// <summary>
        /// Apaga o uid da sessão do usuário e redireciona para a home padrão.
        /// </summary>
        [SMCAuthorize(UC_INS_002_03_01.HOME_INSCRICOES)]
        public ActionResult IndexHome()
        {
            Session[GuidProcesso] = null;
            Session[ProcessoAtualSeq] = null;
            Session["__CSSLayout__"] = null;
            return RedirectToAction("Index", "Home", new { area = string.Empty });
        }

        [SMCAuthorize(UC_INS_002_03_01.HOME_INSCRICOES)]
        public ActionResult ListarEtapaInscricao(ProcessoAbertoFiltroViewModel filtros)
        {
            filtros.Idioma = this.GetCurrentLanguage().HasValue ? this.GetCurrentLanguage().Value : SMCLanguage.Portuguese;
            SMCPagerModel<ProcessoAbertoListaViewModel> dados = ProcessoControllerService.BuscarProcessosComInscricoesEmAberto(filtros);
            return PartialView("_ListarEtapaInscricao", dados);
        }

        [SMCAuthorize(UC_INS_002_03_01.HOME_INSCRICOES)]
        public ActionResult ListarInscricoes(InscricoesFiltroViewModel filtro)
        {
            // Busca o sequencial do inscrito logado
            long? seqInscrito = InscritoControllerService.BuscarSeqInscritoLogado();

            // Se usuário não possui inscrito, redireciona para o cadastro de um
            if (!seqInscrito.HasValue)
            {
                return RedirectToAction("Cadastrar", "Inscrito", new { area = "INS" });
            }

            // Busca as inscrições do inscrito logado
            filtro.SeqInscrito = seqInscrito.Value;
            SMCPagerModel<InscricaoListaViewModel> dados = InscricaoControllerService.BuscarInscricoes(filtro);

            return PartialView("_ListarInscricoes", dados);
        }

        #endregion Home

        #region Home do Processo Inscrito

        [HttpGet]
        [SMCAuthorize(UC_INS_002_04_01.HOME_PROCESSO_INSCRITO)]
        [GoogleTagManagerFilter]
        public ActionResult IndexProcesso(Guid? uidProcesso)
        {
            // Armazena o uid na sessão, para permitir o retorno ao home correto
            Session[GuidProcesso] = uidProcesso;

            // Verifica se o usuário logado possui cadastro de inscrito
            long? seqInscrito = InscritoControllerService.BuscarSeqInscritoLogado();
            // Busca as informações do processo
            SMCLanguage idioma = this.GetCurrentLanguage().HasValue ? this.GetCurrentLanguage().Value : SMCLanguage.Portuguese;
            ProcessoHomeViewModel modelo = null;
            if (uidProcesso != null)
            {
                modelo = ProcessoControllerService.BuscarProcessoHome((Guid)uidProcesso, idioma, seqInscrito);
                Session["__CSSLayout__"] = modelo.UrlCss;
                modelo.UidProcesso = uidProcesso.ToString();

            }

            TempData["___seqProcessoAtual___"] = uidProcesso;
            // Se usuário não possui inscrito, redireciona para o cadastro de um
            if (!seqInscrito.HasValue)
            {
                return RedirectToAction("Cadastrar", "Inscrito", new { area = "INS", uidProcesso = uidProcesso });
            }

            if (modelo == null)
            {
                return RedirectToAction("Index", "Home", new { area = string.Empty });
            }

            if (uidProcesso.HasValue)
            {
                if (!ValidarDadosInscritoPreenchidosParaProcesso(seqInscrito.Value, uidProcesso.Value))
                {
                    TempData["__cadastroIncompleto__"] = true;
                    return RedirectToAction("Cadastrar", "Inscrito", new { area = "INS", uidProcesso = uidProcesso });
                }
            }

            return View(modelo);
        }

        [SMCAuthorize(UC_INS_002_04_01.HOME_PROCESSO_INSCRITO)]
        public ActionResult ListarEtapaInscricaoProcesso(ProcessoAbertoFiltroViewModel filtros)
        {
            filtros.Idioma = this.GetCurrentLanguage().HasValue ? this.GetCurrentLanguage().Value : SMCLanguage.Portuguese;
            SMCPagerModel<GrupoOfertaProcessoListaVewModel> dados = ProcessoControllerService.BuscarGrupoOfertaInscricaoAberta(filtros);
            return PartialView("_ListarEtapaInscricaoProcesso", dados);
        }

        [SMCAuthorize(UC_INS_002_04_01.HOME_PROCESSO_INSCRITO)]
        public ActionResult ListarInscricoesProcesso(InscricoesFiltroViewModel filtro)
        {
            // Busca o sequencial do inscrito logado
            long? seqInscrito = InscritoControllerService.BuscarSeqInscritoLogado();

            // Se usuário não possui inscrito, redireciona para o cadastro de um
            if (!seqInscrito.HasValue)
            {
                return RedirectToAction("Cadastrar", "Inscrito", new { area = "INS" });
            }

            filtro.SeqInscrito = seqInscrito.Value;
            // Busca as inscrições do inscrito logado no processo
            SMCPagerModel<InscricaoProcessoItemViewModel> dados = InscricaoControllerService.BuscarInscricoesProcesso(filtro);

            dados.SelectedValues = new List<object>();
            dados.SelectedValues.Add(filtro.TokenResource);
            
            dados.ForEach(f =>
            {
                if (f.GestaoEventos)
                {
                    var validaBotaoIngresso = InscricaoControllerService.ValidaBotaoIngressos(f.SeqInscricao);
                    f.MensagemInformativaCheckin = validaBotaoIngresso.Item1;
                    f.HabilitarBotaoCheckin = validaBotaoIngresso.Item2;
                }
                f.ExibirBotaoEmitirDocumentacao = ProcessoService.ValidarPermissaoEmitirDocumentacao(f.SeqProcesso, f.SeqInscricao, seqInscrito.Value, f.SeqTipoDocumento);

                if (f.SeqTipoDocumento > 0)
                    f.DescricaoTipoDocumento = TipoDocumentoDadosMestresService.BuscarDescricaoTipoDocumento(f.SeqTipoDocumento);
            });

            ValidarBotoesVisuaslizarEmitirComprovante(dados);

            return PartialView("_ListarInscricoesProcesso", dados);
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
        #region Emitir Documentação

        [SMCAuthorize(UC_INS_002_04_01.HOME_PROCESSO_INSCRITO)]
        public ActionResult EmitirDocumentacao(long seqInscricao, long seqTipoDocumento)
        {
            try
            {
                return File(InscricaoService.EmitirDocumentacao(seqInscricao, seqTipoDocumento), "application/pdf");
            }
            catch (Exception ex)
            {
                return ThrowRedirect(ex, "Index", "Home");
            }
        }
        #endregion Emitir Documentação

        #endregion Home do Processo Inscrito

        #region Logout

        /// <summary>
        /// Logout da aplicação
        /// </summary>
        [SMCAllowAnonymous]
        public ActionResult Logout()
        {
            // Desconecta o usuário
            SMCFederationHelper.SignOut("Index", "Home");

            return new EmptyResult();
        }

        #endregion Logout

        [HttpGet]
        // GET: /BoletoBancario/ImagemBanco/?codigoBanco=1
        [SMCAllowAnonymous]
        public FileResult ImagemBanco(int codigoBanco)
        {
            var imagem = InscricaoControllerService.BuscarImagemBanco(codigoBanco);
            return File(imagem, "image/png");
        }

        [HttpGet]
        // GET: /BoletoBancario/ImagemCodigoBarras/codigoBarras=123456
        [SMCAllowAnonymous]
        public FileResult ImagemCodigoBarras(string codigoBarras)
        {
            var imagem = InscricaoControllerService.BuscarImagemCodigoBarras(codigoBarras);
            return File(imagem, "image/png");
        }

        /// <summary>
        /// Utiliza este metodo de redirecionamento pelo chamando o outro por causa do audience
        /// Para assim ser valido por qualquer ambiente que o chame
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SMCAllowAnonymous]
        public ActionResult AngularErro()
        {
            return RedirectToAction("Angular");
        }

        /// <summary>
        /// Metodo que volta a url para o pagina inicial do angular e faça o redirecionamento correto da pagina 
        /// Com seus devidos parametros que ficam na APP.componente do angular
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SMCAuthorize(UC_INS_002_03_01.HOME_INSCRICOES)]
        public ActionResult Angular()
        {
            return Redirect("../App");
        }

        #region Download de arquivos

        /// <summary>
        /// Action para download de arquivo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [SMCAuthorize(UC_INS_002_02_06.UPLOAD_DOCUMENTOS)]
        public ActionResult DownloadArquivo(string guidFile, string name, string type)
        {
            if (Guid.TryParse(guidFile, out Guid guid))
            {
                var data = SMCUploadHelper.GetFileData(new SMCUploadFile { GuidFile = guidFile });
                if (data != null)
                {
                    return File(data, type, name);
                }
            }
            return DownloadDocumento(new SMCEncryptedLong(guidFile));
        }

        /// <summary>
        /// Action para download de arquivo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [SMCAllowAnonymous]
        public ActionResult DownloadDocumento(SMCEncryptedLong Id)
        {
            SMCUploadFileGED arquivo = ArquivoAnexadoService.BuscarArquivoAnexado(Id);

            if (arquivo.UrlPublicaGed != null)
            {
                return Redirect(arquivo.UrlPublicaGed);
            }

            if (arquivo.FileData == null)
            {
                Response.AppendHeader("Content-Disposition", "inline; filename=" + arquivo.Name + "_expurgo.pdf");
                return File(CONSTANTS.CONTEUDO_ARQUIVO_EXPURGO_PDF, "application/pdf");
            }

            Response.AppendHeader("Content-Disposition", "inline; filename=" + arquivo.Name);
            return File(arquivo.FileData, arquivo.Type);
        }

        #endregion Download de arquivos

        [NonAction]
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
    }
}