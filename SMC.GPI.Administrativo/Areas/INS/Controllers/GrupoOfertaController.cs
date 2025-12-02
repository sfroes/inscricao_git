using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.Html;
using SMC.Framework.UI.Mvc.Security;
using SMC.GPI.Administrativo.App_GlobalResources;
using SMC.GPI.Administrativo.Areas.INS.Models;
using SMC.GPI.Administrativo.Areas.INS.Services;
using SMC.Inscricoes.Common.Areas.INS.Constants;
using System.Linq;
using System.Web.Mvc;

namespace SMC.GPI.Administrativo.Areas.INS.Controllers
{
    public class GrupoOfertaController : SMCControllerBase
    {
        #region Serviços

        private ProcessoControllerService ProcessoControllerService
        {
            get
            {
                return this.Create<ProcessoControllerService>();
            }
        }

        private GrupoOfertaControllerService GrupoOfertaControllerService
        {
            get
            {
                return this.Create<GrupoOfertaControllerService>();
            }
        }

        #endregion Serviços

        #region Pesquisar

        [SMCAuthorize(UC_INS_001_08_01.PESQUISAR_GRUPO_OFERTAS)]
        public ActionResult Index(SMCEncryptedLong seqProcesso)
        {
            var modelo = new GrupoOfertaFiltroViewModel();

            modelo.SeqProcesso = seqProcesso;
            modelo.Cabecalho = this.ProcessoControllerService.BuscarCabecalhoProcesso(seqProcesso);

            return View(modelo);
        }

        /// <summary>
        /// Action para listar os grupos de oferta
        /// </summary>
        /// <param name="filtros">Filtros de pesquisa</param>
        [SMCAuthorize(UC_INS_001_08_01.PESQUISAR_GRUPO_OFERTAS)]
        public ActionResult ListarGrupoOferta(GrupoOfertaFiltroViewModel filtros)
        {
            SMCPagerModel<GrupoOfertaListaViewModel> pager = this.GrupoOfertaControllerService.BuscarGruposOferta(filtros);
            return PartialView("_ListarGrupoOferta", pager);
        }

        #endregion Pesquisar

        #region Incluir/Alterar

        /// <summary>
        /// Exibe tela de inclusão de um grupo de oferta
        /// </summary>
        /// <param name="seqProcesso">Sequencial do processo</param>
        [HttpGet]
        [SMCAuthorize(UC_INS_001_08_02.MANTER_GRUPO_OFERTAS)]
        public ActionResult Incluir(SMCEncryptedLong seqProcesso)
        {
            GrupoOfertaViewModel modelo = new GrupoOfertaViewModel();

            modelo.SeqProcesso = seqProcesso;

            return View(modelo);
        }

        /// <summary>
        /// Exibe tela de edição para um grupo de oferta
        /// </summary>
        /// <param name="seq">Sequencial do grupo de oferta a ser editado</param>
        /// <param name="seqProcesso">Sequencial do processo</param>
        [HttpGet]
        [SMCAuthorize(UC_INS_001_08_02.MANTER_GRUPO_OFERTAS)]
        public ActionResult Editar(SMCEncryptedLong seq, SMCEncryptedLong seqProcesso)
        {
            GrupoOfertaViewModel modelo = this.GrupoOfertaControllerService.BuscarGrupoOferta(seq);

            modelo.SeqProcesso = seqProcesso;

            return View(modelo);
        }

        /// <summary>
        /// Salva um grupo de oferta para um processo e redireciona o usuário para a tela de alteração
        /// </summary>
        [HttpPost]
        [SMCAuthorize(UC_INS_001_08_02.MANTER_GRUPO_OFERTAS)]
        public ActionResult Salvar(GrupoOfertaViewModel modelo)
        {
            this.Assert(modelo);

            return this.SaveEdit(modelo, this.GrupoOfertaControllerService.SalvarGrupoOferta,
                routeValues: new { @seqProcesso = (SMCEncryptedLong)modelo.SeqProcesso });
        }

        /// <summary>
        /// Salva um grupo de oferta para um processo e redireciona o usuário para a tela de novo registro
        /// </summary>
        [HttpPost]
        [SMCAuthorize(UC_INS_001_08_02.MANTER_GRUPO_OFERTAS)]
        public ActionResult SalvarNovo(GrupoOfertaViewModel modelo)
        {
            this.Assert(modelo);

            return this.SaveNew(modelo, this.GrupoOfertaControllerService.SalvarGrupoOferta,
                routeValues: new { @seqProcesso = (SMCEncryptedLong)modelo.SeqProcesso });
        }

        /// <summary>
        /// Salva um grupo de oferta para um processo e redireciona o usuário para a tela de listagem
        /// </summary>
        [HttpPost]
        [SMCAuthorize(UC_INS_001_08_02.MANTER_GRUPO_OFERTAS)]
        public ActionResult SalvarSair(GrupoOfertaViewModel modelo)
        {
            this.Assert(modelo);

            return this.SaveQuit(modelo, this.GrupoOfertaControllerService.SalvarGrupoOferta,
                routeValues: new { @seqProcesso = (SMCEncryptedLong)modelo.SeqProcesso });
        }

        private void Assert(GrupoOfertaViewModel modelo)
        {
            this.Assert(modelo, string.Format(Views.GrupoOferta.App_LocalResources.UIResource.Mensagem_Confirma_Alteracao, modelo.Descricao), () =>
            {
                return modelo.Ofertas.Any(f => f.PossuiGrupo == true);
            });
        }

        #endregion Incluir/Alterar

        #region Excluir

        /// <summary>
        /// Exclui9 um grupo de oferta
        /// </summary>
        /// <param name="seq">Sequencial do grupo de oferta a ser excluido</param>
        /// <param name="seqProcesso">Sequencial do processo</param>
        [HttpPost]
        [SMCAuthorize(UC_INS_001_08_02.MANTER_GRUPO_OFERTAS)]
        public ActionResult Excluir(SMCEncryptedLong seq, SMCEncryptedLong seqProcesso)
        {
            this.GrupoOfertaControllerService.ExcluirGrupoOferta(seq);

            SetSuccessMessage(string.Format(MessagesResource.Mensagem_Sucesso_Exclusao_Registro,
                MessagesResource.Entidade_GrupoOferta),
                MessagesResource.Titulo_Sucesso,
                SMCMessagePlaceholders.Centro);

            return RenderAction("ListarGrupoOferta", new GrupoOfertaFiltroViewModel { SeqProcesso = seqProcesso });
        }

        #endregion Excluir
    }
}