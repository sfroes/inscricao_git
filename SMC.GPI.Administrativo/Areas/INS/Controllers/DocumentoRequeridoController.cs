using SMC.Framework;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.Html;
using SMC.Framework.UI.Mvc.Security;
using SMC.GPI.Administrativo.App_GlobalResources;
using SMC.GPI.Administrativo.Areas.INS.Models;
using SMC.GPI.Administrativo.Areas.INS.Services;
using SMC.Inscricoes.Common.Areas.INS.Constants;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using System.Web.Mvc;

namespace SMC.GPI.Administrativo.Areas.INS.Controllers
{
    public class DocumentoRequeridoController : SMCControllerBase
    {
        #region Serviços

        private DocumentoRequeridoControllerService DocumentoRequeridoControllerService
        {
            get
            {
                return this.Create<DocumentoRequeridoControllerService>();
            }
        }

        private TipoDocumentoControllerService TipoDocumentoControllerService
        {
            get
            {
                return this.Create<TipoDocumentoControllerService>();
            }
        }

        private GrupoOfertaControllerService GrupoOfertaControllerService
        {
            get
            {
                return this.Create<GrupoOfertaControllerService>();
            }
        }

        private ProcessoControllerService ProcessoControllerService
        {
            get { return this.Create<ProcessoControllerService>(); }
        }

        private EtapaProcessoControllerService EtapaProcessoControllerService
        {
            get { return this.Create<EtapaProcessoControllerService>(); }
        }

        #endregion Serviços

        #region Pesquisa

        /// <summary>
        /// Listar as documentações requeridas para uma etapa
        /// </summary>
        /// <param name="seqEtapa"></param>
        /// <param name="seqProcesso"></param>
        /// <returns></returns>
        [SMCAuthorize(UC_INS_001_03_13.PESQUISAR_DOCUMENTACAO_REQUERIDA)]
        public ActionResult Index(SMCEncryptedLong seqConfiguracaoEtapa, SMCEncryptedLong seqEtapa, SMCEncryptedLong seqProcesso)
        {
            var modelo = new DocumentoRequeridoFiltroViewModel();
            modelo.SeqConfiguracaoEtapa = seqConfiguracaoEtapa;
            modelo.SeqEtapaProcesso = seqEtapa;
            modelo.SeqProcesso = seqProcesso;
            return View(modelo);
        }

        /// <summary>
        /// Listar as documentações requeridas
        /// </summary>
        /// <param name="filtros"></param>
        /// <returns></returns>
        [SMCAuthorize(UC_INS_001_03_13.PESQUISAR_DOCUMENTACAO_REQUERIDA)]
        public ActionResult ListarDocumentosRequeridos(DocumentoRequeridoFiltroViewModel filtros)
        {
            SMCPagerModel<DocumentoRequeridoListaViewModel> pager = this.DocumentoRequeridoControllerService.BuscarDocumentosRequeridos(filtros);
            return PartialView("_ListarDocumentosRequeridos", pager);
        }

        #endregion Pesquisa

        #region Incluir/Alterar

        /// <summary>
        /// Incluir uma documentação requerida
        /// </summary>
        /// <returns></returns>
        [SMCAuthorize(UC_INS_001_03_14.MANTER_DOCUMENTACAO_REQUERIDA)]
        public ActionResult Incluir(SMCEncryptedLong seqConfiguracaoEtapa,
            SMCEncryptedLong seqProcesso, SMCEncryptedLong seqEtapa)
        {
            var modelo = new DocumentoRequeridoViewModel();
            modelo.SeqConfiguracaoEtapa = seqConfiguracaoEtapa;
            modelo.SeqEtapaProcesso = seqEtapa;
            modelo.SeqProcesso = seqProcesso;
            modelo.ExibeTermoResponsabilidadeEntrega = false;
            PreencherModelo(modelo);

            return View(modelo);
        }

        /// <summary>
        /// Editar uma documentação requerida
        /// </summary>
        /// <param name="seqDocumentacaoRequerida"></param>
        /// <returns></returns>
        [SMCAuthorize(UC_INS_001_03_14.MANTER_DOCUMENTACAO_REQUERIDA)]
        public ActionResult Editar(SMCEncryptedLong seq,
            SMCEncryptedLong seqProcesso, SMCEncryptedLong seqEtapa)
        {
            var modelo = this.DocumentoRequeridoControllerService.BuscarDocumentoRequerido(seq);
            modelo.SeqEtapaProcesso = seqEtapa;
            modelo.SeqProcesso = seqProcesso;
            PreencherModelo(modelo);

            return View(modelo);
        }

        private void PreencherModelo(DocumentoRequeridoViewModel modelo)
        {
            modelo.TiposDocumento = this.TipoDocumentoControllerService.BuscarTiposDocumentoSelect();
        }

        /// <summary>
        /// Salvar uma documentação requerida
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        [SMCAuthorize(UC_INS_001_03_14.MANTER_DOCUMENTACAO_REQUERIDA)]
        public ActionResult Salvar(DocumentoRequeridoViewModel modelo)
        {
            VerificaDocumento(modelo);
            return this.SaveEdit(modelo, DocumentoRequeridoControllerService.SalvarDocumentoRequerido, PreencherModelo,
               new
               {
                   @seqEtapa = (SMCEncryptedLong)modelo.SeqEtapaProcesso,
                   @seqProcesso = (SMCEncryptedLong)modelo.SeqProcesso
               });
        }

        /// <summary>
        /// Salvar uma documentação requerida e cadastrar uma nova
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        [SMCAuthorize(UC_INS_001_03_14.MANTER_DOCUMENTACAO_REQUERIDA)]
        public ActionResult SalvarNovo(DocumentoRequeridoViewModel modelo)
        {
            VerificaDocumento(modelo);
            return this.SaveNew(modelo, DocumentoRequeridoControllerService.SalvarDocumentoRequerido, PreencherModelo,
               new
               {
                   @seqConfiguracaoEtapa = (SMCEncryptedLong)modelo.SeqConfiguracaoEtapa,
                   @seqEtapa = (SMCEncryptedLong)modelo.SeqEtapaProcesso,
                   @seqProcesso = (SMCEncryptedLong)modelo.SeqProcesso
               });
        }

        /// <summary>
        /// Salvar uma documentação requerida e sair
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        [SMCAuthorize(UC_INS_001_03_14.MANTER_DOCUMENTACAO_REQUERIDA)]
        public ActionResult SalvarSair(DocumentoRequeridoViewModel modelo)
        {
            VerificaDocumento(modelo);
            return this.SaveQuit(modelo, DocumentoRequeridoControllerService.SalvarDocumentoRequerido, PreencherModelo,
               new
               {
                   @seqConfiguracaoEtapa = (SMCEncryptedLong)modelo.SeqConfiguracaoEtapa,
                   @seqEtapa = (SMCEncryptedLong)modelo.SeqEtapaProcesso,
                   @seqProcesso = (SMCEncryptedLong)modelo.SeqProcesso
               });
        }

        [NonAction]
        private void VerificaDocumento(DocumentoRequeridoViewModel modelo)
        {
            if (modelo.Seq == 0)
            {
                Assert(modelo, Views.DocumentoRequerido.App_LocalResources.UIResource.Assert_ModificaUpload, () =>
                {
                    return DocumentoRequeridoControllerService.VerificaApenasInscricoesTeste(modelo.SeqConfiguracaoEtapa);
                });
            }
            else
            {
                Assert(modelo, Views.DocumentoRequerido.App_LocalResources.UIResource.Assert_ModificaTipoDocumento, () =>
                {
                    if (modelo.SeqTipoDocumento != modelo.TipoDocumentoOriginal)
                    {
                        return DocumentoRequeridoControllerService.VerificaApenasInscricoesTeste(modelo.SeqConfiguracaoEtapa);
                    }
                    return false;
                });

                Assert(modelo, Views.DocumentoRequerido.App_LocalResources.UIResource.Assert_ModificaVersaoDocumento, () =>
                {
                    if ((short)modelo.VersaoDocumento != modelo.VersaoDocumentoOriginal)
                    {
                        return DocumentoRequeridoControllerService.VerificaApenasInscricoesTeste(modelo.SeqConfiguracaoEtapa);
                    }
                    return false;
                });

                Assert(modelo, Views.DocumentoRequerido.App_LocalResources.UIResource.Assert_ModificaUpload, () =>
                {
                    if (modelo.UploadObrigatorio.GetValueOrDefault() && modelo.UploadObrigatorio != modelo.UploadObrigatorioOriginal)
                    {
                        return DocumentoRequeridoControllerService.VerificaApenasInscricoesTeste(modelo.SeqConfiguracaoEtapa);
                    }
                    return false;
                });

            }
        }

        #endregion Incluir/Alterar

        #region Excluir

        /// <summary>
        /// Excluir uma documentação requerida
        /// </summary>
        /// <param name="seqEtapa"></param>
        /// <param name="seqProcesso"></param>
        /// <param name="seqDocumentacaoRequerida"></param>
        /// <returns></returns>
        [HttpPost]
        [SMCAuthorize(UC_INS_001_03_14.MANTER_DOCUMENTACAO_REQUERIDA)]
        public ActionResult Excluir(DocumentoRequeridoViewModel modelo)
        {
            DocumentoRequeridoViewModel documentoRequerido = null;
            Assert(modelo, Views.DocumentoRequerido.App_LocalResources.UIResource.Assert_ExclusaoDocumentoRequerido, () =>
            {
                documentoRequerido = this.DocumentoRequeridoControllerService.BuscarDocumentoRequerido(modelo.Seq);
                if (documentoRequerido.UploadObrigatorio.GetValueOrDefault())
                {
                    return DocumentoRequeridoControllerService.VerificaApenasInscricoesTeste(modelo.SeqConfiguracaoEtapa);
                }
                return false;
            });

            Assert(modelo, Views.DocumentoRequerido.App_LocalResources.UIResource.Assert_ExclusaoDocumentoRequeridoSeExistir, () =>
            {
                if (documentoRequerido == null)
                    documentoRequerido = this.DocumentoRequeridoControllerService.BuscarDocumentoRequerido(modelo.Seq);

                if (documentoRequerido.PermiteUploadArquivo.GetValueOrDefault() && !documentoRequerido.UploadObrigatorio.GetValueOrDefault())
                {
                    return DocumentoRequeridoControllerService.VerificaInscricaoComDocumentoCadastrado(modelo.Seq);
                }

                return false;
            });

            this.DocumentoRequeridoControllerService.ExcluirDocumentoRequerido(modelo.Seq);

            SetSuccessMessage(string.Format(MessagesResource.Mensagem_Sucesso_Exclusao_Registro,
                MessagesResource.Entidade_DocumentacaoRequerida),
                MessagesResource.Titulo_Sucesso,
                SMCMessagePlaceholders.Centro);

            return RenderAction("ListarDocumentosRequeridos", new DocumentoRequeridoFiltroViewModel
            {
                SeqConfiguracaoEtapa = modelo.SeqConfiguracaoEtapa,
                SeqEtapaProcesso = modelo.SeqEtapaProcesso,
                SeqProcesso = modelo.SeqProcesso
            });
        }

        #endregion Excluir

        [SMCAuthorize(UC_INS_001_03_14.MANTER_DOCUMENTACAO_REQUERIDA)]
        public ActionResult ValorPadraoCampoExibeTermoResponsabilidadeEntrega(bool uploadObrigatorio, bool permiteEntregaPosterior)
        {
            if (uploadObrigatorio && permiteEntregaPosterior)
                return Json("False");
            else
                return null;
        }
    }
}