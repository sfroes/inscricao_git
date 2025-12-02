using SMC.Framework;
using SMC.Framework.DataFilters;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.Security;
using SMC.GPI.Administrativo.Areas.INS.Services;
using SMC.GPI.Administrativo.Areas.NOT.Models;
using SMC.GPI.Administrativo.Areas.NOT.Services;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Areas.NOT.Constants;
using System.Linq;
using System.Web.Mvc;

namespace SMC.GPI.Administrativo.Areas.NOT
{
    public class ConsultaNotificacaoController : SMCControllerBase
    {
        #region Serviços

        private ConsultaNotificacaoControllerService ConsultaNotificacaoControllerService
        {
            get { return this.Create<ConsultaNotificacaoControllerService>(); }
        }

        private GrupoOfertaControllerService GrupoOfertaControllerService
        {
            get { return this.Create<GrupoOfertaControllerService>(); }
        }

        private InscritoControllerService InscritoControllerService
        {
            get { return this.Create<InscritoControllerService>(); }
        }

        #endregion Serviços

        #region Listar

        [SMCAuthorize(UC_NOT_001_01_01.CONSULTAR_NOTIFICACAO)]
        public ActionResult Index(ConsultaNotificacaoFiltroViewModel filtros = null)
        {
            if (filtros != null)
            {
                if (filtros.SeqProcesso.HasValue)
                {
                    filtros.GruposOferta = GrupoOfertaControllerService.BuscarGruposOfertasSelect(filtros.SeqProcesso.Value);
                }

                if (filtros.SeqInscrito.HasValue)
                {
                    var inscrito = InscritoControllerService.BuscarNomesDadosInscrito(filtros.SeqInscrito.Value);
                    filtros.Inscrito = (!string.IsNullOrWhiteSpace(inscrito.NomeSocial)) ? inscrito.NomeSocial : inscrito.Nome;
                }
            }
            else
            {
                filtros = new ConsultaNotificacaoFiltroViewModel();
            }

            filtros.TiposNotificacao = ConsultaNotificacaoControllerService.BuscarTiposNotificacao(filtros.SeqProcesso);

            return View(filtros);
        }

        [SMCAuthorize(UC_NOT_001_01_01.CONSULTAR_NOTIFICACAO)]
        public ActionResult ListarNotificacoes(ConsultaNotificacaoFiltroViewModel filtros)
        {
            // Caso tenha sido clicado no botão limpar do filtro, retorna a mensagem padrão de selecione o filtro
            if (CheckPostClearSubmit())
                return DisplayFilterMessage();

            filtros.SeqUnidadeResponsavel = SMCDataFilterHelper.GetFilters()[FILTERS.UNIDADE_RESPONSAVEL].ToList();
            SMCPagerModel<ConsultaNotificacaoListaViewModel> pager = ConsultaNotificacaoControllerService.BuscarNotificacoes(filtros);

            if (!string.IsNullOrEmpty(filtros.BackURL))
            {
                pager.SMCForEach(f => f.BackUrl = filtros.BackURL);
            }

            return PartialView("_ListarNotificacoes", pager);
        }

        #endregion Listar

        #region Visualizar

        [SMCAuthorize(UC_NOT_001_01_01.CONSULTAR_NOTIFICACAO)]
        public ActionResult Visualizar(SMCEncryptedLong seq)
        {
            var modelo = ConsultaNotificacaoControllerService.BuscarNotificacao(seq);
         
            return View(modelo);
        }

        #endregion Visualizar

        [HttpPost]
        [SMCAllowAnonymous]
        public JsonResult BuscarGrupoOferta(long processo)
        {
            var itens = GrupoOfertaControllerService.BuscarGruposOfertasSelect(processo);
            return Json(itens);
        }

        [HttpPost]
        [SMCAllowAnonymous]
        public JsonResult BuscarTipoNotificacao(long? processo)
        {
            var itens = ConsultaNotificacaoControllerService.BuscarTiposNotificacao(processo);
            return Json(itens);
        }

        [SMCAllowAnonymous]
        public ActionResult DownloadDocumento(SMCEncryptedLong Id)
        {
            var conteudo = ConsultaNotificacaoControllerService.BuscarArquivo(Id);

            if (conteudo.FileData == null)
            {
                Response.AppendHeader("Content-Disposition", "inline; filename=" + conteudo.Name + "_expurgo.pdf");
                return File(CONSTANTS.CONTEUDO_ARQUIVO_EXPURGO_PDF, "application/pdf");
            }

            Response.AppendHeader("Content-Disposition", "inline; filename=" + conteudo.Name);
            return File(conteudo.FileData, conteudo.Type);
        }
    }
}