using SMC.Framework;
using SMC.Framework.Exceptions;
using SMC.Framework.Extensions;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.Html;
using SMC.Framework.UI.Mvc.Security;
using SMC.GPI.Administrativo.App_GlobalResources;
using SMC.GPI.Administrativo.Areas.INS.Models;
using SMC.GPI.Administrativo.Areas.INS.Services;
using SMC.Inscricoes.Common.Areas.INS.Constants;
using SMC.Inscricoes.Service.Areas.INS.Services;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using System.Linq;
using System.Web.Mvc;

namespace SMC.GPI.Administrativo.Areas.INS.Controllers
{
    public class GrupoTaxaController : SMCControllerBase
    {
        #region Serviços

        private ProcessoControllerService ProcessoControllerService
        {
            get
            {
                return this.Create<ProcessoControllerService>();
            }
        }


        private GrupoTaxaService GrupoTaxaService
        {
            get
            {
                return this.Create<GrupoTaxaService>();
            }
        }



        #endregion Serviços

        #region Pesquisar

        [SMCAuthorize(UC_INS_001_12_01.PESQUISAR_GRUPO_TAXAS)]
        public ActionResult Index(SMCEncryptedLong seqProcesso)
        {
            //RN_INS_220 - Pré-condição para o cadastro do grupo de taxa
            if (!ProcessoControllerService.BuscarProcesso(seqProcesso).Taxas.Any())
            {
                SetErrorMessage(Views.GrupoTaxa.App_LocalResources.UIResource.Mensagem_Erro_Processo_Sem_Taxa);
                return BackToAction();
            }

            var modelo = new GrupoTaxaFiltroViewModel();
            modelo.SeqProcesso = seqProcesso;
            modelo.Cabecalho = this.ProcessoControllerService.BuscarCabecalhoProcesso(seqProcesso);

            return View(modelo);
        }

        /// <summary>
        /// Action para listar os grupos de Taxas
        /// </summary>
        /// <param name="filtros">Filtros de pesquisa</param>
        [SMCAuthorize(UC_INS_001_12_01.PESQUISAR_GRUPO_TAXAS)]
        public ActionResult ListarGrupoTaxa(GrupoTaxaFiltroViewModel filtros)
        {
            var filtroData = filtros.Transform<GrupoTaxaFiltroData>();
            var model = this.GrupoTaxaService.BuscarGruposTaxa(filtroData).TransformList<GrupoTaxaListaViewModel>();

            return PartialView("_ListarGrupoTaxa", model);
        }


        #endregion Pesquisar

        #region Incluir/Alterar

        /// <summary>
        /// Exibe tela de inclusão de um grupo de taxa
        /// </summary>
        /// <param name="seqProcesso">Sequencial do processo</param>
        [HttpGet]
        [SMCAuthorize(UC_INS_001_12_02.MANTER_GRUPO_TAXAS)]
        public ActionResult Incluir(SMCEncryptedLong seqProcesso)
        {
            GrupoTaxaViewModel modelo = new GrupoTaxaViewModel();
            modelo.SeqProcesso = seqProcesso;
            modelo.Taxas = this.GrupoTaxaService.BuscarTipoTaxaSelect(seqProcesso);

            return PartialView("_EditarGrupoTaxa", modelo);
        }

        /// <summary>
        /// Exibe tela de edição para um grupo de taxa
        /// </summary>
        /// <param name="seqGrupoTaxa">Sequencial do grupo de taxa a ser editado</param>        
        [HttpGet]
        [SMCAuthorize(UC_INS_001_12_02.MANTER_GRUPO_TAXAS)]
        public ActionResult Editar(SMCEncryptedLong seqGrupoTaxa)
        {
            var retorno = this.GrupoTaxaService.BuscarGrupoTaxa(seqGrupoTaxa);

            GrupoTaxaViewModel modelo = retorno.Transform<GrupoTaxaViewModel>();

            modelo.Taxas = this.GrupoTaxaService.BuscarTipoTaxaSelect(retorno.SeqProcesso);

            return PartialView("_EditarGrupoTaxa", modelo);
        }

        /// <summary>
        /// Salva um grupo de taxa para um processo e redireciona o usuário para a tela de alteração
        /// </summary>
        [HttpPost]
        [SMCAuthorize(UC_INS_001_12_02.MANTER_GRUPO_TAXAS)]
        public ActionResult Salvar(GrupoTaxaViewModel modelo)
        {

            var grupoTaxaData = modelo.Transform<GrupoTaxaData>();

            this.GrupoTaxaService.SalvarGrupoTaxa(grupoTaxaData);

            SetSuccessMessage(string.Format(modelo.Seq == 0 ? MessagesResource.Mensagem_Sucesso_Inclusao_Registro : MessagesResource.Mensagem_Sucesso_Alteracao_Registro,
                MessagesResource.Entidade_GrupoTaxa),
                    MessagesResource.Titulo_Sucesso,
                    SMCMessagePlaceholders.Centro);


            return RedirectToAction("ListarGrupoTaxa", new GrupoTaxaFiltroViewModel { SeqProcesso = modelo.SeqProcesso });

        }

        #endregion Incluir/Alterar

        #region Selects

        [HttpPost]
        [SMCAllowAnonymous]
        public JsonResult ListaGrupoTaxaItem(long? seqGrupoTaxa)
        {
            var listaGrupoTaxaItem = this.GrupoTaxaService.BuscarGrupoTaxaItemPorGrupoTaxaSelect(seqGrupoTaxa);

            return Json(listaGrupoTaxaItem);
        }

        #endregion

        #region Excluir
        /// <summary>
        /// Excluir um grupo de Taxa
        /// </summary>
        /// <param name="seqGrupoTaxa"></param>  
        /// <param name="seqProcesso"></param>
        /// <returns></returns>
        [HttpPost]
        [SMCAuthorize(UC_INS_001_12_02.MANTER_GRUPO_TAXAS)]
        public ActionResult Excluir(SMCEncryptedLong seqGrupoTaxa, SMCEncryptedLong seqProcesso)
        {
            this.GrupoTaxaService.ExcluirGrupoTaxa(seqGrupoTaxa);

            SetSuccessMessage(string.Format(MessagesResource.Mensagem_Sucesso_Exclusao_Registro,
                MessagesResource.Entidade_GrupoDocumentacaoRequerida),
                MessagesResource.Titulo_Sucesso,
                SMCMessagePlaceholders.Centro);

            return RedirectToAction("ListarGrupoTaxa", new GrupoTaxaFiltroViewModel { SeqProcesso = seqProcesso });
        }


        #endregion Excluir
    }
}