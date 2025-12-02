using SMC.Framework;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.Html;
using SMC.Framework.UI.Mvc.Security;
using SMC.GPI.Administrativo.App_GlobalResources;
using SMC.GPI.Administrativo.Areas.INS.Models;
using SMC.GPI.Administrativo.Areas.INS.Services;
using SMC.Inscricoes.Common.Areas.INS.Constants;
using System.Web.Mvc;

namespace SMC.GPI.Administrativo.Areas.INS.Controllers
{
    public class TipoDocumentoController : SMCControllerBase
    {
        #region Serviços

        private TipoDocumentoControllerService TipoDocumentoControllerService
        {
            get
            {
                return this.Create<TipoDocumentoControllerService>();
            }
        }

        #endregion Serviços

        #region Listagem

        /// <summary>
        /// Exibe tela de listagem de tipos de processo
        /// </summary>
        /// <param name="filtros">Filtros de pesquisa</param>
        [SMCAuthorize(UC_INS_001_05_01.PESQUISAR_TIPO_DOCUMENTO)]
        public ActionResult Index(TipoDocumentoFiltroViewModel filtros = null)
        {
            return View(filtros);
        }

        /// <summary>
        /// Action para listar os tipos de processo
        /// </summary>
        /// <param name="filtros">Filtros de pesquisa</param>
        [SMCAuthorize(UC_INS_001_05_01.PESQUISAR_TIPO_DOCUMENTO)]
        public ActionResult ListarTipoDocumento(TipoDocumentoFiltroViewModel filtros)
        {
            // Caso tenha sido clicado no botão limpar do filtro, retorna a mensagem padrão de selecione o filtro
            if (CheckPostClearSubmit())
                return DisplayFilterMessage();

            SMCPagerModel<TipoDocumentoListaViewModel> pager = this.TipoDocumentoControllerService.BuscarTiposDocumento(filtros);
            return PartialView("_ListarTipoDocumento", pager);
        }

        #endregion Listagem

        #region Edição/Inclusão

        /// <summary>
        /// Exibe a tela de inclusão de um tipo de processo
        /// </summary>
        [HttpGet]
        [SMCAuthorize(UC_INS_001_05_02.MANTER_TIPO_DOCUMENTO)]
        public ActionResult Incluir()
        {
            this.SetViewMode(SMCViewMode.Insert);
            TipoDocumentoViewModel modelo = new TipoDocumentoViewModel() { Novo = true };            
            PreencheModelo(modelo);            
            return View("Incluir", modelo);
        }

        [HttpGet]
        [SMCAuthorize(UC_INS_001_05_02.MANTER_TIPO_DOCUMENTO)]
        public ActionResult Editar(SMCEncryptedLong seq)
        {
            this.SetViewMode(SMCViewMode.Edit);
            TipoDocumentoViewModel modelo = TipoDocumentoControllerService.BuscarTipoDocumento(seq);
            PreencheModelo(modelo);
            return View(modelo);
        }

        [HttpPost]
        [SMCAuthorize(UC_INS_001_05_02.MANTER_TIPO_DOCUMENTO)]
        public ActionResult Salvar(TipoDocumentoViewModel modelo)
        {
            return this.SaveEdit(modelo, this.TipoDocumentoControllerService.SalvarTipoDocumento, PreencheModelo);
        }

        /// <summary>
        /// Salva um tipo de processo e redireciona o usuário para a tela de novo registro
        /// </summary>
        [HttpPost]
        [SMCAuthorize(UC_INS_001_05_02.MANTER_TIPO_DOCUMENTO)]
        public ActionResult SalvarNovo(TipoDocumentoViewModel modelo)
        {
            return this.SaveNew(modelo, this.TipoDocumentoControllerService.SalvarTipoDocumento, PreencheModelo);
        }

        /// <summary>
        /// Salva um tipo de processo e redireciona o usuário para a tela de listagem
        /// </summary>
        [HttpPost]
        [SMCAuthorize(UC_INS_001_05_02.MANTER_TIPO_DOCUMENTO)]
        public ActionResult SalvarSair(TipoDocumentoViewModel modelo)
        {
            return this.SaveQuit(modelo, this.TipoDocumentoControllerService.SalvarTipoDocumento, PreencheModelo);
        }

        #endregion Edição/Inclusão

        #region Excluir

        /// <summary>
        /// Action para excluir um tipo de processo
        /// </summary>
        /// <param name="seqTipoProcesso">Sequencial do tipo de processo a ser excluído</param>
        [HttpPost]
        [SMCAuthorize(UC_INS_001_05_02.MANTER_TIPO_DOCUMENTO)]
        public ActionResult Excluir(SMCEncryptedLong seqTipoDocumento)
        {
            TipoDocumentoControllerService.ExcluirTipoDocumento(seqTipoDocumento);

            SetSuccessMessage(string.Format(MessagesResource.Mensagem_Sucesso_Exclusao_Registro,
                                            MessagesResource.Entidade_TipoDocumento),
                                MessagesResource.Titulo_Sucesso,
                                SMCMessagePlaceholders.Centro);

            return RenderAction("ListarTipoDocumento");
        }

        #endregion Excluir

        private void PreencheModelo(TipoDocumentoViewModel modelo)
        {
            if (modelo.Novo)
            {
                modelo.TiposDocumento = this.TipoDocumentoControllerService.BuscarTiposDocumentoNaoUtilizadosSelect();
            }
            else
            {
                modelo.TiposDocumento = this.TipoDocumentoControllerService.BuscarTiposDocumentoSelect();
            }
        }
    }
}