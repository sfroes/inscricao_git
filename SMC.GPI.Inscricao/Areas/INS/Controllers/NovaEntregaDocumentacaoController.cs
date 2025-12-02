using MC.Inscricoes.ServiceContract.Areas.INS.Data;
using SMC.Framework;
using SMC.Framework.Extensions;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.Html;
using SMC.Framework.UI.Mvc.Security;
using SMC.Framework.UI.Mvc.Util;
using SMC.GPI.Inscricao.App_GlobalResources;
using SMC.GPI.Inscricao.Areas.INS.Models;
using SMC.Inscricoes.Common.Areas.INS.Constants;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using SMC.Inscricoes.UI.Mvc.Attributes;
using System;
using System.Web.Mvc;

namespace SMC.GPI.Inscricao.Areas.INS.Controllers
{
    [GoogleTagManagerFilter]
    public class NovaEntregaDocumentacaoController : SMCControllerBase
    {
        #region Services

        private IInscricaoService InscricaoService { get => Create<IInscricaoService>(); }

        private IArquivoAnexadoService ArquivoAnexadoService { get => Create<IArquivoAnexadoService>(); }

        #endregion Services

        #region Index

        [SMCAuthorize(UC_INS_002_06_01.NOVA_ENTREGA_DOCUMENTACAO)]
        public ActionResult Index(PaginaFiltroViewModel filtro)
        {
            var model = this.InscricaoService.BuscarDocumentosNovaEntregaDocumentacao(filtro.SeqInscricao).Transform<NovaEntregaDocumentacaoViewModel>();

            model.SeqInscricao = filtro.SeqInscricao;
            model.SeqConfiguracaoEtapa = filtro.SeqConfiguracaoEtapa;
            model.SeqConfiguracaoEtapaPagina = filtro.SeqConfiguracaoEtapaPagina;
            model.SeqGrupoOferta = filtro.SeqGrupoOferta;
            model.Idioma = filtro.Idioma;
            model.UidProcesso = filtro.UidProcesso;

            VerificarIdioma(model);

            return View(model);
        }

        [SMCAuthorize(UC_INS_002_06_01.NOVA_ENTREGA_DOCUMENTACAO)]
        public ActionResult CabecalhoNovaEntregaDocumentacao(long seqInscricao)
        {
            var model = this.InscricaoService.BuscarCabecalhoNovaEntregaDocumentacao(seqInscricao).Transform<NovaEntregaDocumentacaoCabecalhoViewModel>();

            return PartialView("_Cabecalho", model);
        }

        #endregion Index

        #region Salvar

        [HttpPost]
        [SMCAuthorize(UC_INS_002_06_01.NOVA_ENTREGA_DOCUMENTACAO)]
        public ActionResult SalvarNovaEntregaDocumentacao(NovaEntregaDocumentacaoViewModel model)
        {
                   this.InscricaoService.SalvarNovaEntregaDocumentacao(model.Transform<NovaEntregaDocumentacaoData>());

                SetSuccessMessage(Views.NovaEntregaDocumentacao.App_LocalResources.UIResource.Mensagem_Sucesso_Inclusao_Nova_Entrega_Documentacao, MessagesResource.Titulo_Sucesso, SMCMessagePlaceholders.Centro);

                return SMCRedirectToAction("IndexProcesso", "Home", new { area = string.Empty, UidProcesso = model.UidProcesso });
            

        }

        #endregion Salvar

        #region Visualizar

        [SMCAuthorize(UC_INS_002_06_01.NOVA_ENTREGA_DOCUMENTACAO)]
        public FileResult VisualizarDocumentoAnterior(SMCEncryptedLong seqArquivoAnexadoAnterior)
        {
            var arquivoAnexado = this.ArquivoAnexadoService.BuscarArquivoAnexado(seqArquivoAnexadoAnterior);

            Response.AppendHeader("Content-Disposition", "inline; filename=" + "\"" + arquivoAnexado.Name + "\"");
            return File(arquivoAnexado.FileData, arquivoAnexado.Type);
        }

        #endregion Visualizar

        #region Download de arquivos

        [SMCAuthorize(UC_INS_002_06_01.NOVA_ENTREGA_DOCUMENTACAO)]
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
            var conteudo = ArquivoAnexadoService.BuscarArquivoAnexado(Id);
            Response.AppendHeader("Content-Disposition", "inline; filename=" + conteudo.Name);
            return File(conteudo.FileData, conteudo.Type);
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
    }
}