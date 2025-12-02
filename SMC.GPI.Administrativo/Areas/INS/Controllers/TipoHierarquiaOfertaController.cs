using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.Html;
using SMC.Framework.UI.Mvc.Security;
using SMC.GPI.Administrativo.App_GlobalResources;
using SMC.GPI.Administrativo.Areas.INS.Models;
using SMC.GPI.Administrativo.Areas.INS.Services;
using SMC.Inscricoes.Common.Areas.INS.Constants;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SMC.GPI.Administrativo.Areas.INS.Controllers
{
    public class TipoHierarquiaOfertaController : SMCControllerBase
    {
        #region Serviços

        private TipoHierarquiaOfertaControllerService TipoHierarquiaOfertaControllerService
        {
            get
            {
                return this.Create<TipoHierarquiaOfertaControllerService>();
            }
        }

        private ITipoItemHierarquiaOfertaService TipoItemHierarquiaOferta
        {
            get
            {
                return this.Create<ITipoItemHierarquiaOfertaService>();
            }
        }

        #endregion Serviços

        #region Listagem

        /// <summary>
        /// Exibe tela de listagem de tipos de hierarquia de oferta
        /// </summary>
        /// <param name="filtros">Filtros de pesquisa</param>
        [SMCAuthorize(UC_INS_001_04_01.PESQUISAR_TIPO_HIERARQUIA_OFERTA)]
        public ActionResult Index(TipoHierarquiaOfertaFiltroViewModel filtros = null)
        {
            return View(filtros);
        }

        /// <summary>
        /// Action para listar os tipos de hierarquia de oferta
        /// </summary>
        /// <param name="filtros">Filtros de pesquisa</param>
        [SMCAuthorize(UC_INS_001_04_01.PESQUISAR_TIPO_HIERARQUIA_OFERTA)]
        public ActionResult ListarTipoHierarquiaOferta(TipoHierarquiaOfertaFiltroViewModel filtros)
        {
            SMCPagerModel<TipoHierarquiaOfertaListaViewModel> pager = this.TipoHierarquiaOfertaControllerService.BuscarTiposHierarquiaOferta(filtros);
            return PartialView("_ListarTipoHierarquiaOferta", pager);
        }

        #endregion Listagem

        #region Edição/Inclusão

        /// <summary>
        /// Exibe a tela de inclusão de um tipo de hierarquia de oferta
        /// </summary>
        [HttpGet]
        [SMCAuthorize(UC_INS_001_04_02.MANTER_TIPO_HIERARQUIA_OFERTA)]
        public ActionResult Incluir()
        {
            TipoHierarquiaOfertaViewModel modelo = new TipoHierarquiaOfertaViewModel();
            return View(modelo);
        }

        /// <summary>
        /// Exibe tela de edição para um tipo de hierarquia de oferta
        /// </summary>
        /// <param name="seq">Sequencial do tipo de hierarquia de oferta a ser editado</param>
        [HttpGet]
        [SMCAuthorize(UC_INS_001_04_02.MANTER_TIPO_HIERARQUIA_OFERTA)]
        public ActionResult Editar(SMCEncryptedLong seq)
        {
            TipoHierarquiaOfertaViewModel modelo = this.TipoHierarquiaOfertaControllerService.BuscarTipoHierarquiaOferta(seq);
            return View(modelo);
        }

        /// <summary>
        /// Salva um tipo de hierarquia de oferta e redireciona o usuário para a tela de alteração
        /// </summary>
        [HttpPost]
        [SMCAuthorize(UC_INS_001_04_02.MANTER_TIPO_HIERARQUIA_OFERTA)]
        public ActionResult Salvar(TipoHierarquiaOfertaViewModel modelo)
        {
            var seqTipo = this.TipoHierarquiaOfertaControllerService.SalvarTipoHierarquiaOferta(modelo);
            return RedirectToAction("MontarArvoreTipoHierarquiaOferta", new { seqTipoHierarquiaOferta = ((SMCEncryptedLong)seqTipo) });
            //  return this.SaveEdit(modelo, this.TipoHierarquiaOfertaControllerService.SalvarTipoHierarquiaOferta);
        }

        /// <summary>
        /// Salva um tipo de hierarquia de oferta e redireciona o usuário para a tela de novo registro
        /// </summary>
        [HttpPost]
        [SMCAuthorize(UC_INS_001_04_02.MANTER_TIPO_HIERARQUIA_OFERTA)]
        public ActionResult SalvarNovo(TipoHierarquiaOfertaViewModel modelo)
        {
            return this.SaveNew(modelo, this.TipoHierarquiaOfertaControllerService.SalvarTipoHierarquiaOferta);
        }

        /// <summary>
        /// Salva um tipo de hierarquia de oferta e redireciona o usuário para a tela de listagem
        /// </summary>
        [HttpPost]
        [SMCAuthorize(UC_INS_001_04_02.MANTER_TIPO_HIERARQUIA_OFERTA)]
        public ActionResult SalvarSair(TipoHierarquiaOfertaViewModel modelo)
        {
            return this.SaveQuit(modelo, this.TipoHierarquiaOfertaControllerService.SalvarTipoHierarquiaOferta);
        }

        #endregion Edição/Inclusão

        #region Excluir

        /// <summary>
        /// Action para excluir um tipo de hierarquia de oferta
        /// </summary>
        /// <param name="seqTipoHierarquiaOferta">Sequencial do tipo de hierarquia de oferta a ser excluído</param>
        [HttpPost]
        [SMCAuthorize(UC_INS_001_04_02.MANTER_TIPO_HIERARQUIA_OFERTA)]
        public ActionResult Excluir(SMCEncryptedLong seqTipoHierarquiaOferta)
        {
            TipoHierarquiaOfertaControllerService.ExcluirTipoHierarquiaOferta(seqTipoHierarquiaOferta);

            SetSuccessMessage(string.Format(MessagesResource.Mensagem_Sucesso_Exclusao_Registro,
                                            MessagesResource.Entidade_TipoHierarquiaOferta),
                                MessagesResource.Titulo_Sucesso,
                                SMCMessagePlaceholders.Centro);

            return RenderAction("ListarTipoHierarquiaOferta");
        }

        #endregion Excluir

        #region Árvore de Tipos de Hierarquia

        /// <summary>
        /// Action para listar os tipos de hierarquia de oferta para associação a árvore
        /// </summary>
        [SMCAuthorize(UC_INS_001_04_04.ASSOCIAR_TIPO_ITEM)]
        public ActionResult AssociarTipoHierarquiaOferta(SMCEncryptedLong seqTipoHieraquiaOferta,
            SMCEncryptedLong seqPai = null, string descricaoPai = null)
        {
            var modelo = new ItemHierarquiaOfertaViewModel();
            modelo.SeqTipoHierarquiaOferta = seqTipoHieraquiaOferta;
            if (seqPai != null) modelo.SeqPai = seqPai;
            if (!string.IsNullOrEmpty(descricaoPai)) modelo.DescricaoPai = descricaoPai;
            modelo.TiposItemHierarquiaOferta = TipoItemHierarquiaOferta.BuscarTiposItemHierarquiaOfertaSelect();
            return PartialView("_AssociarTipoHierarquiaOferta", modelo);
        }

        /// <summary>
        /// Action para listar os tipos de hierarquia de oferta para associação a árvore
        /// </summary>
        [SMCAuthorize(UC_INS_001_04_04.ASSOCIAR_TIPO_ITEM)]
        public ActionResult EditarAssocicaoTipoHierarquiaOferta(SMCEncryptedLong seq)
        {
            var modelo = this.TipoHierarquiaOfertaControllerService.BuscarItemHierarquiaOferta(seq);
            modelo.TiposItemHierarquiaOferta = TipoItemHierarquiaOferta.BuscarTiposItemHierarquiaOfertaSelect();
            return PartialView("_AssociarTipoHierarquiaOferta", modelo);
        }

        /// <summary>
        /// Salva a associação de um tipo de hierarquia a árvore
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        [SMCAuthorize(UC_INS_001_04_04.ASSOCIAR_TIPO_ITEM)]
        public ActionResult SalvarAssociacaoTipoHierarquiaOferta(ItemHierarquiaOfertaViewModel itemSelecionado)
        {
            this.TipoHierarquiaOfertaControllerService.SalvarAssociacaoTipoHierarquiaOferta(itemSelecionado);
            return RenderAction("ArvoreTipoHierarquiaOferta", (SMCEncryptedLong)itemSelecionado.SeqTipoHierarquiaOferta);
        }

        /// <summary>
        /// Renderiza treeview de hierarquia de tipo de entidade
        /// </summary>
        /// <param name="seqTipoHierarquiaEntidade">Sequencial de hierarquia de tipo de entidade</param>
        [SMCAuthorize(UC_INS_001_04_03.MONTAR_TIPO_HIERARQUIA_OFERTA)]
        public ActionResult ArvoreTipoHierarquiaOferta(SMCEncryptedLong seqTipoHierarquiaOferta)
        {
            List<NoArvoreTipoHierarquiaOfertaViewModel> itens = TipoHierarquiaOfertaControllerService.BuscarItensArvoreTipoHierarquiaOferta(seqTipoHierarquiaOferta);

            List<SMCTreeViewNode<NoArvoreTipoHierarquiaOfertaViewModel>> itensArvore = SMCTreeView.For<NoArvoreTipoHierarquiaOfertaViewModel>(itens);

            return PartialView("_ArvoreTipoHierarquiaOferta", itensArvore);
        }

        /// <summary>
        /// Exibe visão de árvore de hierarquia de tipo de entidade
        /// </summary>
        /// <param name="seqTipoHierarquiaEntidade">Sequencial de hierarquia de tipo de entidade</param>
        [SMCAuthorize(UC_INS_001_04_03.MONTAR_TIPO_HIERARQUIA_OFERTA)]
        public ActionResult MontarArvoreTipoHierarquiaOferta(SMCEncryptedLong seqTipoHierarquiaOferta)
        {
            TipoHierarquiaOfertaViewModel modelo = TipoHierarquiaOfertaControllerService.BuscarTipoHierarquiaOferta(seqTipoHierarquiaOferta);
            return View("MontarArvoreTipoHierarquiaOferta", modelo);
        }

        /// <summary>
        /// Excluir um item da árvore de hierarquia
        /// </summary>
        /// <param name="seqTipoHierarquiaOferta"></param>
        /// <returns></returns>
        [SMCAuthorize(UC_INS_001_04_02.MANTER_TIPO_HIERARQUIA_OFERTA)]
        public ActionResult ExcluirItemHierarquiaOferta(SMCEncryptedLong seqTipoHierarquiaOferta, SMCEncryptedLong seqItemHierarquiaOferta)
        {
            this.TipoHierarquiaOfertaControllerService.ExcluirAssociaoTipoHierarquiaOferta(seqItemHierarquiaOferta);
            return ArvoreTipoHierarquiaOferta(seqTipoHierarquiaOferta);
        }

        #endregion Árvore de Tipos de Hierarquia
    }
}