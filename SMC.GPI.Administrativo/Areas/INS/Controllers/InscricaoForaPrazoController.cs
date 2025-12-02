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
    public class InscricaoForaPrazoController : SMCControllerBase
    {
        #region Controller Services

        private InscricaoForaPrazoControllerService InscricaoForaPrazoControllerService
        {
            get { return this.Create<InscricaoForaPrazoControllerService>(); }
        }

        #endregion Controller Services

        #region Listar

        [SMCAuthorize(UC_INS_001_11_01.PESQUISAR_PERMISSAO_INSCRICAO_FORA_PRAZO)]
        public ActionResult Index(InscricaoForaPrazoFiltroViewModel filtro)
        {
            return View(filtro);
        }

        [SMCAuthorize(UC_INS_001_11_01.PESQUISAR_PERMISSAO_INSCRICAO_FORA_PRAZO)]
        public ActionResult ListarInscricoesForaPrazo(InscricaoForaPrazoFiltroViewModel filtro)
        {
            var model = InscricaoForaPrazoControllerService.BuscarInscricoesForaPrazo(filtro);
            return PartialView("_ListarInscricoesForaPrazo", model);
        }

        #endregion Listar

        #region Incluir / Editar

        [SMCAuthorize(UC_INS_001_11_02.MANTER_PERMISSAO_INSCRICAO_FORA_PRAZO)]
        public ActionResult Incluir(SMCEncryptedLong seqProcesso)
        {
            var model = new InscricaoForaPrazoViewModel();
            model.SeqProcesso = seqProcesso;
            return View(model);
        }

        [SMCAuthorize(UC_INS_001_11_02.MANTER_PERMISSAO_INSCRICAO_FORA_PRAZO)]
        public ActionResult Editar(SMCEncryptedLong seq)
        {
            var modelo = InscricaoForaPrazoControllerService.BuscarInscricaoForaPrazo(seq);
            return View(modelo);
        }

        /// <summary>
        /// Salva um grupo de oferta para um processo e redireciona o usuário para a tela de alteração
        /// </summary>
        [HttpPost]
        [SMCAuthorize(UC_INS_001_11_02.MANTER_PERMISSAO_INSCRICAO_FORA_PRAZO)]
        public ActionResult Salvar(InscricaoForaPrazoViewModel modelo)
        {
            this.InscricaoForaPrazoControllerService.SalvarPermissoes(modelo);

            SetSuccessMessage(string.Format(modelo.Seq == 0 ? MessagesResource.Mensagem_Sucesso_Inclusao_Registro : MessagesResource.Mensagem_Sucesso_Alteracao_Registro,
                MessagesResource.Entidade_PermissaoForaPrazo),
                MessagesResource.Titulo_Sucesso,
                SMCMessagePlaceholders.Centro);

            return RedirectToAction("Index", new { seqProcesso = (SMCEncryptedLong)modelo.SeqProcesso });
        }

        #endregion Incluir / Editar

        #region Exclusao

        [SMCAuthorize(UC_INS_001_11_01.PESQUISAR_PERMISSAO_INSCRICAO_FORA_PRAZO)]
        public ActionResult Excluir(SMCEncryptedLong seq)
        {
            this.InscricaoForaPrazoControllerService.ExcluirPermissao(seq);

            SetSuccessMessage(string.Format(MessagesResource.Mensagem_Sucesso_Exclusao_Registro,
                MessagesResource.Entidade_PermissaoForaPrazo),
                MessagesResource.Titulo_Sucesso,
                SMCMessagePlaceholders.Centro);

            return RenderAction("ListarInscricoesForaPrazo");
        }

        #endregion Exclusao
    }
}