using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.Html;
using SMC.Framework.UI.Mvc.Security;
using SMC.GPI.Administrativo.Areas.NOT.Models;
using SMC.GPI.Administrativo.Areas.NOT.Services;
using SMC.GPI.Administrativo.Areas.NOT.Views.TipoNotificacao.App_LocalResources;
using SMC.Inscricoes.Common.Areas.NOT.Constants;
using System.Web.Mvc;

namespace SMC.GPI.Administrativo.Areas.NOT.Controllers
{
    public class TipoNotificacaoController : SMCControllerBase
    {
        #region Serviços

        private TipoNotificacaoControllerService TipoNotificacaoControllerService
        {
            get { return this.Create<TipoNotificacaoControllerService>(); }
        }

        #endregion Serviços

        #region Listar

        /// <summary>
        /// Pagina inicial do tipo de notificações
        /// </summary>
        /// <param name="filtro">Filtro de busca pode ser null ou nao</param>
        /// <returns></returns>
        [SMCAuthorize(UC_NOT_001_02_01.PESQUISAR_TIPO_NOTIFICACAO)]
        public ActionResult Index(TipoNotificacaoFiltroViewModel filtro = null)
        {
            filtro = new TipoNotificacaoFiltroViewModel();
            return View(filtro);
        }

        /// <summary>
        /// Lista os tipos de notificações
        /// </summary>
        /// <param name="filtros">Filtro para busca</param>
        /// <returns>Grid com os dados filtrados</returns>
        [SMCAuthorize(UC_NOT_001_02_01.PESQUISAR_TIPO_NOTIFICACAO)]
        public ActionResult ListarTipoNotificacoes(TipoNotificacaoFiltroViewModel filtros)
        {
            SMCPagerModel<TipoNotificacaoListaViewModel> pager = this.TipoNotificacaoControllerService.BuscarTipoNotificacoes(filtros);
            return PartialView("_ListarTipoNotificacoes", pager);
        }

        #endregion Listar

        #region Incluir / Editar

        /// <summary>
        /// Incluir Novo Parâmetro por Tipo de Notificação
        /// </summary>
        /// <returns>Campos para inclusao</returns>
        [SMCAuthorize(UC_NOT_001_02_02.MANTER_TIPO_NOTIFICACAO)]
        public ActionResult Incluir()
        {
            TipoNotificacaoViewModel modelo = new TipoNotificacaoViewModel();
            PreencherModelo(modelo);
            return View(modelo);
        }

        [SMCAuthorize(UC_NOT_001_02_02.MANTER_TIPO_NOTIFICACAO)]
        [HttpGet]
        public ActionResult Editar(SMCEncryptedLong seq)
        {
            TipoNotificacaoViewModel modelo = this.TipoNotificacaoControllerService.BuscarTipoNotificacao(seq);
            PreencherModelo(modelo);
            return View(modelo);
        }

        [HttpPost]
        [SMCAuthorize(UC_NOT_001_02_02.MANTER_TIPO_NOTIFICACAO)]
        public ActionResult Salvar(TipoNotificacaoViewModel modelo)
        {
            return this.SaveEdit(modelo, TipoNotificacaoControllerService.SalvarTipoNotificacao, PreencherModelo);
        }

        [HttpPost]
        [SMCAuthorize(UC_NOT_001_02_02.MANTER_TIPO_NOTIFICACAO)]
        public ActionResult SalvarNovo(TipoNotificacaoViewModel modelo)
        {
            return this.SaveNew(modelo, TipoNotificacaoControllerService.SalvarTipoNotificacao, PreencherModelo);
        }

        [HttpPost]
        [SMCAuthorize(UC_NOT_001_02_02.MANTER_TIPO_NOTIFICACAO)]
        public ActionResult SalvarSair(TipoNotificacaoViewModel modelo)
        {
            return this.SaveQuit(modelo, TipoNotificacaoControllerService.SalvarTipoNotificacao, PreencherModelo);
        }

        public void PreencherModelo(TipoNotificacaoViewModel modelo)
        {
            modelo.TipoNotificacao = this.TipoNotificacaoControllerService.BuscarTipoNotificacao();
        }

        #endregion Incluir / Editar

        #region Excluir

        /// <summary>
        /// Excluir Tipo de notificação
        /// </summary>
        /// <param name="seqNubente">Sequencial do Tipo de notificação</param>
        /// <returns>Excluir o registro e continua na mesma pagina</returns>
        [SMCAuthorize(UC_NOT_001_02_02.MANTER_TIPO_NOTIFICACAO)]
        [HttpPost]
        public ActionResult Excluir(SMCEncryptedLong seq)
        {
            this.TipoNotificacaoControllerService.Excluir(seq);

            SetSuccessMessage(string.Format(UIResource.Mensagem_Sucesso_Exclusao_Registro,
                                            UIResource.Entidade_Tipo_Notificação),
                                UIResource.Titulo_Sucesso,
                                SMCMessagePlaceholders.Centro);
            return RenderAction("ListarTipoNotificacoes");
        }

        #endregion Excluir
    }
}