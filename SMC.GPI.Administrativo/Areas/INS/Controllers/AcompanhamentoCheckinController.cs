using SMC.Formularios.UI.Mvc;
using SMC.Framework;
using SMC.Framework.Exceptions;
using SMC.Framework.Extensions;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.Rest;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.Html;
using SMC.Framework.UI.Mvc.Security;
using SMC.GPI.Administrativo.App_GlobalResources;
using SMC.GPI.Administrativo.Areas.INS.Models;
using SMC.GPI.Administrativo.Areas.INS.Services;
using SMC.Inscricoes.Common.Areas.INS.Constants;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data.AcompanhamentoCheckin;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data.Checkin;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using SMC.Inscricoes.ServiceContract.Areas.RES.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace SMC.GPI.Administrativo.Areas.INS.Controllers
{
    public class AcompanhamentoCheckinController : SGFController
    {
        #region injeção de dependencia

        private ITipoProcessoService TipoProcessoService => this.Create<ITipoProcessoService>();
        private IProcessoService ProcessoService => this.Create<IProcessoService>();
        private IUnidadeResponsavelService UnidadeResponsavelService => this.Create<IUnidadeResponsavelService>();
        private AcompanhamentoInscritoControllerService AcompanhamentoInscritoControllerService => this.Create<AcompanhamentoInscritoControllerService>();
        private IOfertaService OfertaService => this.Create<IOfertaService>();
        private IInscricaoOfertaService InscricaoOfertaService => this.Create<IInscricaoOfertaService>();
        private SMCApiClient ReportClient => SMCApiClient.Create("Reports");
        #endregion

        [SMCAuthorize(UC_INS_005_01_04.CONSULTA_POSICAO_CONSOLIDADA_CHECKIN)]
        public ActionResult Index(AcompanhamentoCheckinFiltroViewModel filtros)
        {
            //filtros.Unidades = UnidadeResponsavelService.BuscarUnidadesResponsaveisKeyValue().TransformList<SMCSelectListItem>();
            filtros.Processos = ProcessoService.BuscarProcessoSelect(filtros.Transform<ProcessoCandidatoFiltroData>()).TransformList<SMCSelectListItem>();
            //filtros.TiposProcessos = TipoProcessoService.BuscarTiposProcessoKeyValue(filtros.SeqUnidadeResponsavel).TransformList<SMCSelectListItem>();

            return View(filtros);
        }

        [SMCAuthorize(UC_INS_005_01_04.CONSULTA_POSICAO_CONSOLIDADA_CHECKIN)]
        public ActionResult Listar(AcompanhamentoCheckinFiltroViewModel filtro)
        {
            // Caso tenha sido clicado no botão limpar do filtro, retorna a mensagem padrão de selecione o filtro
            if (CheckPostClearSubmit())
                return DisplayFilterMessage();

            // filtro.PageSettings.PageSize = int.MaxValue;

            SMCPagerData<AcompanhamentoInscritoCheckinListaViewModel> model = SMCMapperHelper.Create<SMCPagerData<AcompanhamentoInscritoCheckinListaViewModel>>
                                                                               (OfertaService.BuscarPosicaoConsolidadaCheckin(filtro.Transform<AcompanhamentoCheckinFiltroData>()));

            Session["seqProcesso"] = filtro.SeqProcesso;

            var retorno = new SMCPagerModel<AcompanhamentoInscritoCheckinListaViewModel>(model, filtro.PageSettings, filtro);

            return PartialView("Listar", retorno);
        }

        [SMCAuthorize(UC_INS_005_01_06.LISTAGEM_INSCRITOS_ATIVIDADE)]
        public ActionResult ListagemInscritosAtividade(List<long> gridConsolidacaoCheckin)
        {
            var filtroData = new InscritoAtividadeRelatorioFiltroData() { SeqsOfertas = gridConsolidacaoCheckin, SeqProcesso = (long)Session["seqProcesso"] };
            // Session["seqProcesso"] = null;

            var dadosReport = ReportClient.Execute<byte[]>("InscritoAtividadeRelatorio", filtroData, Method.POST);

            return new FileContentResult(dadosReport, "application/pdf");
        }

        [SMCAuthorize(UC_INS_005_01_06.LISTAGEM_INSCRITOS_ATIVIDADE)]
        public ActionResult ListarInscritosChekinLote(CheckinLoteViewModel filtro)
        {
            // Caso tenha sido clicado no botão limpar do filtro, retorna a mensagem padrão de selecione o filtro
            if (CheckPostClearSubmit())
                return DisplayFilterMessage();

            filtro.SeqProcesso = (long)Session["seqProcesso"];
            var model = InscricaoOfertaService.BuscarInscritosCheckinLote(filtro.Transform<CheckinLoteFiltroData>());



            return PartialView("ListarCheckinLote", new SMCPagerModel<ListarCheckinLoteViewModel>(model));
        }

        [SMCAuthorize(UC_INS_005_01_05.REALIZAR_CHECKIN_EM_LOTE)]
        public ActionResult CheckinLote(SMCEncryptedLong seqOferta, SMCEncryptedLong seqProcesso)
        {
            try
            {

                var retorno = InscricaoOfertaService.BuscarCabecalhoCheckinLote(seqOferta.Value, seqProcesso.Value).Transform<CheckinLoteViewModel>();
                Session["seqOferta"] = seqOferta.Value;
                retorno.SeqOferta = seqOferta.Value;

                return View("CheckinLote", retorno);
            }
            catch (Exception e)
            {
                SetErrorMessage(e.Message, MessagesResource.Titulo_Erro, SMCMessagePlaceholders.Centro);
                return BackToAction();
            }
        }


        [SMCAuthorize(UC_INS_005_01_05.REALIZAR_CHECKIN_EM_LOTE)]
        [HttpPost]
        public ActionResult RealizarCheckinLote(List<long> gridCheckinLote)
        {
            if ( gridCheckinLote == null || !gridCheckinLote.Any())
            {
                throw new SMCApplicationException("Nenhum inscrito foi selecionado.");
            }
            var result = InscricaoOfertaService.EfetuarCheckinLote(new CheckinData() { SeqsInscricao = gridCheckinLote, SeqOferta = (long)Session["seqOferta"] });

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {

                SetSuccessMessage("Checkin realizado com sucesso");

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                throw new SMCApplicationException(result.Mensagem);
            }



        }

        [SMCAuthorize(UC_INS_005_01_05.REALIZAR_CHECKIN_EM_LOTE)]
        [HttpPost]
        public ActionResult DesfazerCheckinLote(List<long> gridCheckinLote)
        {
            if (gridCheckinLote == null || !gridCheckinLote.Any())
            {
                throw new SMCApplicationException("Nenhum inscrito foi selecionado.");
            }

            var result = InscricaoOfertaService.DesfazerCheckinLote(new CheckinData() { SeqsInscricao = gridCheckinLote, SeqOferta = (long)Session["seqOferta"] });

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {

                SetSuccessMessage("Desfazer checkin realizado com sucesso");

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {

                throw new SMCApplicationException(result.Mensagem);
            }
        }

    }
}