using SMC.Formularios.UI.Mvc;
using SMC.Framework.Extensions;
using SMC.Framework.UI.Mvc.Dynamic;
using SMC.Framework.UI.Mvc.Html;
using SMC.Framework.UI.Mvc.Security;
using SMC.GPI.Administrativo.Areas.INS.Models;
using SMC.GPI.Administrativo.Areas.INS.Services;
using SMC.Inscricoes.Common.Areas.INS.Constants;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using SMC.Inscricoes.ServiceContract.Areas.RES.Interfaces;
using System.Web.Mvc;

namespace SMC.GPI.Administrativo.Areas.INS.Controllers
{
    public class AcompanhamentoInscritoController : SGFController
    {
        #region injeção de dependencia

        private ITipoProcessoService TipoProcessoService => this.Create<ITipoProcessoService>();
        private IProcessoService ProcessoService => this.Create<IProcessoService>();   
        private IUnidadeResponsavelService UnidadeResponsavelService => this.Create<IUnidadeResponsavelService>();
        private AcompanhamentoInscritoControllerService AcompanhamentoInscritoControllerService
        {
            get { return this.Create<AcompanhamentoInscritoControllerService>(); }
        }
        #endregion

        [SMCAuthorize(UC_INS_004_01_01.PESQUISAR_INSCRITO)]
        public ActionResult Index(AcompanhamentoInscritoFiltroViewModel filtros = null)
        {
            filtros.Unidades = UnidadeResponsavelService.BuscarUnidadesResponsaveisKeyValue().TransformList<SMCSelectListItem>();
            filtros.Processos = ProcessoService.BuscarProcessoSelect(filtros.Transform<ProcessoCandidatoFiltroData>()).TransformList<SMCSelectListItem>();
            filtros.TiposProcessos = TipoProcessoService.BuscarTiposProcessoKeyValue(filtros.SeqUnidadeResponsavel).TransformList<SMCSelectListItem>();

            return View(filtros);
        }

        [SMCAuthorize(UC_INS_004_01_01.PESQUISAR_INSCRITO)]
        public ActionResult VerificaUnidadeResponsavel(long? seqUnidadeResponsavel)
        {
            if (seqUnidadeResponsavel > 0 || seqUnidadeResponsavel != null)
            {
                return Json(TipoProcessoService.BuscarTiposProcessoKeyValue(seqUnidadeResponsavel));
            }

            return Json(TipoProcessoService.BuscarTiposProcessoKeyValue());
        }

        [SMCAuthorize(UC_INS_004_01_01.PESQUISAR_INSCRITO)]
        public ActionResult FiltroProcessos(long? seqUnidadeResponsavel, long? seqTipoProcesso)
        {
            var filtro = new ProcessoCandidatoFiltroViewModel();
            filtro.SeqTipoProcesso = seqTipoProcesso;
            filtro.SeqUnidadeResponsavel = seqUnidadeResponsavel;
            return Json(ProcessoService.BuscarProcessoSelect(filtro.Transform<ProcessoCandidatoFiltroData>()));
        }

        [SMCAuthorize(UC_INS_004_01_01.PESQUISAR_INSCRITO)]
        public ActionResult ListarInscritos(AcompanhamentoInscritoFiltroViewModel filtro)
        {
            // Caso tenha sido clicado no botão limpar do filtro, retorna a mensagem padrão de selecione o filtro
            if (CheckPostClearSubmit())
                return DisplayFilterMessage();

            var pager = AcompanhamentoInscritoControllerService.BuscarInscritos(filtro);
            
            return PartialView("ListarDetailList", pager);
        }

    }
}