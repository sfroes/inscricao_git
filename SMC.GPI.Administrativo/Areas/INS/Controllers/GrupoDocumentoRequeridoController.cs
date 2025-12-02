using SMC.Framework;
using SMC.Framework.Extensions;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.Html;
using SMC.Framework.UI.Mvc.Security;
using SMC.GPI.Administrativo.App_GlobalResources;
using SMC.GPI.Administrativo.Areas.INS.Models;
using SMC.GPI.Administrativo.Areas.INS.Services;
using SMC.Inscricoes.Common.Areas.INS.Constants;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SMC.GPI.Administrativo.Areas.INS.Controllers
{
    public class GrupoDocumentoRequeridoController : SMCControllerBase
    {
        #region Serviços

        private GrupoDocumentoRequeridoControllerService GrupoDocumentoRequeridoControllerService
        {
            get
            {
                return this.Create<GrupoDocumentoRequeridoControllerService>();
            }
        }

        private DocumentoRequeridoControllerService DocumentoRequeridoControllerService
        {
            get
            {
                return this.Create<DocumentoRequeridoControllerService>();
            }
        }

        #endregion Serviços

        #region Pesquisar

        /// <summary>
        /// Listar os grupos de documentação requedidas
        /// </summary>
        /// <param name="seqEtapa"></param>
        /// <param name="seqProcesso"></param>
        /// <returns></returns>
        [SMCAuthorize(UC_INS_001_03_15.PESQUISAR_GRUPO_DOCUMENTACAO_REQUERIDA)]
        public ActionResult Index(SMCEncryptedLong seqConfiguracaoEtapa, SMCEncryptedLong seqEtapa, SMCEncryptedLong seqProcesso)
        {
            var modelo = new GrupoDocumentosRequeridosFiltroViewModel();
            modelo.SeqConfiguracaoEtapa = seqConfiguracaoEtapa;
            modelo.SeqProcesso = seqProcesso;
            modelo.SeqEtapa = seqEtapa;

            return View(modelo);
        }

        /// <summary>
        /// Listar os grupos de documentação requedidas
        /// </summary>
        /// <param name="filtros"></param>
        /// <returns></returns>
        [SMCAuthorize(UC_INS_001_03_15.PESQUISAR_GRUPO_DOCUMENTACAO_REQUERIDA)]
        public ActionResult ListarGrupoDocumentacaoRequerida(GrupoDocumentosRequeridosFiltroViewModel filtros)
        {
            List<GrupoDocumentosRequeridosListaViewModel> model = this.GrupoDocumentoRequeridoControllerService.BuscarGruposDocumentacoesRequeridas(filtros);
            return PartialView("_ListarGrupoDocumentosRequeridos", model);
        }

        #endregion Pesquisar

        #region Incluir/Alterar

        /// <summary>
        /// Incluir um grupo de documentação requedida
        /// </summary>
        /// <returns></returns>
        [SMCAuthorize(UC_INS_001_03_16.MANTER_GRUPO_DOCUMENTACAO_REQUERIDA)]
        public ActionResult Incluir(SMCEncryptedLong seqConfiguracaoEtapa)
        {
            var modelo = new GrupoDocumentosRequeridosViewModel();
            modelo.SeqConfiguracaoEtapa = seqConfiguracaoEtapa;
            modelo.ExibeTermoResponsabilidadeEntrega = false;
            modelo.DocumentosRequeridos = this.DocumentoRequeridoControllerService.
                BuscarDocumentosRequeridosSelect(seqConfiguracaoEtapa, false, false);
            return PartialView("_EditarGrupoDocumentosRequeridos", modelo);
        }

        /// <summary>
        /// Alterar um grupo de documentação requerida
        /// </summary>
        /// <param name="seqGrupoDocumentacaoRequerida"></param>
        /// <returns></returns>
        [SMCAuthorize(UC_INS_001_03_16.MANTER_GRUPO_DOCUMENTACAO_REQUERIDA)]
        public ActionResult Editar(SMCEncryptedLong seqGrupoDocumentacaoRequerida)
        {
            var modelo = this.GrupoDocumentoRequeridoControllerService.BuscarGrupoDocumentacaoRequerida(seqGrupoDocumentacaoRequerida);
            modelo.DocumentosRequeridos = this.DocumentoRequeridoControllerService.
                BuscarDocumentosRequeridosSelect(modelo.SeqConfiguracaoEtapa, false, false);

            modelo.ItensHash = modelo.Itens.SMCGetHash();

            return PartialView("_EditarGrupoDocumentosRequeridos", modelo);
        }

        /// <summary>
        /// Salvar um grupo de documentação requerida
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [SMCAuthorize(UC_INS_001_03_16.MANTER_GRUPO_DOCUMENTACAO_REQUERIDA)]
        public ActionResult Salvar(GrupoDocumentosRequeridosViewModel modelo)
        {
            this.Assert(modelo, Views.GrupoDocumentoRequerido.App_LocalResources.UIResource.Assert_UploadObrigatorio, () =>
            {
                if (modelo.Seq == 0 || (modelo.UploadObrigatorio.GetValueOrDefault() && !modelo.UploadObrigatorioOriginal.GetValueOrDefault()))
                {
                    return GrupoDocumentoRequeridoControllerService.VerificaApenasInscricoesTeste(modelo.SeqConfiguracaoEtapa);
                }
                return false;
            });

            this.Assert(modelo, Views.GrupoDocumentoRequerido.App_LocalResources.UIResource.Assert_NumeroMinimoObrigatorio, () =>
            {
                // Esta alterando o numero mínimo de um cadastro que já existe
                if (modelo.Seq != 0 && modelo.MinimoObrigatorio != modelo.MinimoObrigatorioOriginal)
                {
                    return GrupoDocumentoRequeridoControllerService.VerificaApenasInscricoesTeste(modelo.SeqConfiguracaoEtapa);
                }
                return false;
            });

            Assert(modelo, Views.GrupoDocumentoRequerido.App_LocalResources.UIResource.Assert_AlterarItemGrupoDocumento, () =>
            {
                if (modelo.UploadObrigatorio.GetValueOrDefault() && modelo.ItensHash != modelo.Itens.SMCGetHash())
                {
                    return GrupoDocumentoRequeridoControllerService.VerificaApenasInscricoesTeste(modelo.SeqConfiguracaoEtapa);
                }
                return false;
            });

            this.GrupoDocumentoRequeridoControllerService.SalvarGrupoDocumentacaoRequerida(modelo);

            SetSuccessMessage(string.Format(modelo.Seq == 0 ? MessagesResource.Mensagem_Sucesso_Inclusao_Registro : MessagesResource.Mensagem_Sucesso_Alteracao_Registro,
                MessagesResource.Entidade_GrupoDocumentacaoRequerida),
                    MessagesResource.Titulo_Sucesso,
                    SMCMessagePlaceholders.Centro);

            return RenderAction("ListarGrupoDocumentacaoRequerida",
            new GrupoDocumentosRequeridosFiltroViewModel
            {
                SeqEtapa = modelo.SeqEtapa,
                SeqProcesso = modelo.SeqProcesso,
                SeqConfiguracaoEtapa = modelo.SeqConfiguracaoEtapa
            });
        }

        #endregion Incluir/Alterar

        #region Excluir

        /// <summary>
        /// Excluir um grupo de documentação requedida
        /// </summary>
        /// <param name="seqEtapa"></param>
        /// <param name="seqProcesso"></param>
        /// <param name="seqGrupoDocumentacaoRequerida"></param>
        /// <returns></returns>
        [HttpPost]
        [SMCAuthorize(UC_INS_001_03_16.MANTER_GRUPO_DOCUMENTACAO_REQUERIDA)]
        public ActionResult Excluir(SMCEncryptedLong seqConfiguracaoEtapa, SMCEncryptedLong seqGrupoDocumentacaoRequerida)
        {
            this.GrupoDocumentoRequeridoControllerService.ExcluirGrupoDocumentacaoRequerida(seqGrupoDocumentacaoRequerida);

            SetSuccessMessage(string.Format(MessagesResource.Mensagem_Sucesso_Exclusao_Registro,
                MessagesResource.Entidade_GrupoDocumentacaoRequerida),
                MessagesResource.Titulo_Sucesso,
                SMCMessagePlaceholders.Centro);

            return RenderAction("ListarGrupoDocumentacaoRequerida",
            new GrupoDocumentosRequeridosFiltroViewModel
            {
                SeqConfiguracaoEtapa = seqConfiguracaoEtapa
            });
        }

        #endregion Excluir

        [SMCAuthorize(UC_INS_001_03_16.MANTER_GRUPO_DOCUMENTACAO_REQUERIDA)]
        public ActionResult ValorPadraoCampoExibeTermoResponsabilidadeEntrega(bool uploadObrigatorio)
        {
            if (uploadObrigatorio)
                return Json("False");
            else
                return null;
        }
    }
}