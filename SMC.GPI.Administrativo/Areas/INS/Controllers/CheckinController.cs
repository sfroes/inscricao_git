using SMC.Financeiro.ServiceContract.Areas.BNK.Data;
using SMC.Formularios.UI.Mvc;
using SMC.Framework;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.Security;
using SMC.GPI.Administrativo.Areas.INS.Models;
using SMC.GPI.Administrativo.Areas.INS.Services;
using SMC.Inscricoes.Common.Areas.INS.Constants;
using SMC.Inscricoes.Domain.Areas.INS.ValueObjects;
using SMC.Inscricoes.Service.Areas.INS.Services;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data.Checkin;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web.Mvc;

namespace SMC.GPI.Administrativo.Areas.INS.Controllers
{
    public class CheckinController : SMCControllerBase
    {
        #region Services
        private IProcessoService ProcessoService => this.Create<IProcessoService>();

        private IInscricaoOfertaService InscricaoOfertaService => this.Create<IInscricaoOfertaService>();
        #endregion

        [SMCAuthorize(UC_INS_005_01_02.REALIZAR_CHECKIN_POR_QRCODE)]
        public ActionResult Index(bool somenteAtendente = false)
        {
            var modelo = ProcessoService.BuscarProcessoHierarquiaLeituraQRCode();
            Session["__ProcessoHierarquia__"] = modelo;
            modelo.ForEach(f => f.Atendente = somenteAtendente);
            return View();
        }

        [SMCAuthorize(UC_INS_005_01_02.REALIZAR_CHECKIN_POR_QRCODE)]
        public JsonResult BuscarGrupoOfertas(long seqProcesso)
        {
            var modelo = Session["__ProcessoHierarquia__"] as List<ProcessoGestaoEventoQRCodeData>;
            var grupoOferta = modelo.Find(x => x.SeqProcesso == seqProcesso).GrupoOfertas;

            return Json(grupoOferta, JsonRequestBehavior.AllowGet);
        }

        [SMCAuthorize(UC_INS_005_01_02.REALIZAR_CHECKIN_POR_QRCODE)]
        public JsonResult BuscarHierarquiaOfertas(long seqProcesso, long seqGrupoOferta)
        {
            var modelo = Session["__ProcessoHierarquia__"] as List<ProcessoGestaoEventoQRCodeData>;
            var nivelsuperior = modelo.Find(x => x.SeqProcesso == seqProcesso)
                                      .Hierarquias.Where(x => x.SeqGrupoOferta == seqGrupoOferta).ToList();

            return Json(nivelsuperior, JsonRequestBehavior.AllowGet);
        }

        [SMCAuthorize(UC_INS_005_01_02.REALIZAR_CHECKIN_POR_QRCODE)]
        public JsonResult BuscarOfertas(long seqProcesso, long SeqHierarquia)
        {
            var modelo = Session["__ProcessoHierarquia__"] as List<ProcessoGestaoEventoQRCodeData>;
            var ofertas = modelo.Find(x => x.SeqProcesso == seqProcesso)
                                      .Ofertas.Where(x => x.SeqNivelSuperior == SeqHierarquia || x.SeqNivelSuperior == null).ToList();

            return Json(ofertas, JsonRequestBehavior.AllowGet);
        }

        [SMCAuthorize(UC_INS_005_01_02.REALIZAR_CHECKIN_POR_QRCODE)]
        public ActionResult Checkin(ConfiguracaoUsuarioData dados)
        {

            if (dados.SeqProcesso != null)
            {
                Session["__ConfiguracaoUsuario__"] = dados;
            }
            else
            {
                dados = Session["__ConfiguracaoUsuario__"] as ConfiguracaoUsuarioData;
            }

            var processoHierarquia = Session["__ProcessoHierarquia__"] as List<ProcessoGestaoEventoQRCodeData>;

            var processo = processoHierarquia.Find(x => x.SeqProcesso == dados.SeqProcesso);

            var modelo = new CheckinViewModel();

            modelo.SeqProcesso = processo.SeqProcesso;
            modelo.DescricaoProcesso = processo.DescricaoProcesso;
            modelo.Atendente = processo.Atendente;
            modelo.Ofertas = new List<string>();
            modelo.Ofertas.AddRange(processo.Ofertas.Where(w => dados.SeqsOferta.Contains(w.SeqOferta)).Select(x => x.DescricaoCompleta));

            return View(modelo);
        }

        [SMCAuthorize(UC_INS_005_01_02.REALIZAR_CHECKIN_POR_QRCODE)]
        public JsonResult EfetuarCheckin(CheckinData dadosCheckin)
        {
            RespostaCheckinData retorno = new RespostaCheckinData(); // Inicializa o objeto de retorno

            try
            {
                var dadosUsuario = Session["__ConfiguracaoUsuario__"] as ConfiguracaoUsuarioData;

                if (dadosUsuario == null)
                {
                    retorno.Mensagem = "Sessão de usuário não encontrada. Por favor, faça login novamente.";
                    retorno.StatusCode = HttpStatusCode.Unauthorized;
                    Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return Json(retorno); // Retorna imediatamente se a sessão for nula
                }

                dadosCheckin.SeqProcesso = dadosUsuario.SeqProcesso;
                dadosCheckin.SeqGrupoOferta = dadosUsuario.SeqGrupoOferta;
                dadosCheckin.SeqHierarquia = dadosUsuario.SeqHierarquia;
                dadosCheckin.SeqsOferta = dadosUsuario.SeqsOferta;
                dadosCheckin.TokenHistoricoSituacao = dadosUsuario.TokenHistoricoSituacao;

                retorno = InscricaoOfertaService.EfetuarCheckin(dadosCheckin);

                Response.StatusCode = (int)retorno.StatusCode;
            }
            catch (Exception ex)
            {

                retorno.Mensagem = "Ocorreu um erro interno inesperado: " + ex.Message;
                retorno.StatusCode = HttpStatusCode.InternalServerError;
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            return Json(retorno);
        }

        [SMCAuthorize(UC_INS_005_01_02.REALIZAR_CHECKIN_POR_QRCODE)]
        public JsonResult EfetuarCheckin_(CheckinData dadosCheckin)
        {
            var dadosUsuario = Session["__ConfiguracaoUsuario__"] as ConfiguracaoUsuarioData;

            dadosCheckin.SeqProcesso = dadosUsuario.SeqProcesso;
            dadosCheckin.SeqGrupoOferta = dadosUsuario.SeqGrupoOferta;
            dadosCheckin.SeqHierarquia = dadosUsuario.SeqHierarquia;
            dadosCheckin.SeqsOferta = dadosUsuario.SeqsOferta;
            dadosCheckin.TokenHistoricoSituacao = dadosUsuario.TokenHistoricoSituacao;


            var retorno = InscricaoOfertaService.EfetuarCheckin(dadosCheckin);

            if (retorno.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Response.StatusCode = 200;
            }
            else
            {
                Response.StatusCode = 400;
            }


            return Json(retorno);
        }

        [SMCAuthorize(UC_INS_005_01_02.REALIZAR_CHECKIN_POR_QRCODE)]
        public ActionResult CheckinManual(ConfiguracaoUsuarioData dados)
        {
            if (dados.SeqProcesso != null)
            {
                Session["__ConfiguracaoUsuario__"] = dados;
            }
            else
            {
                dados = Session["__ConfiguracaoUsuario__"] as ConfiguracaoUsuarioData;
            }

            //var dados = Session["__ConfiguracaoUsuario__"] as ConfiguracaoUsuarioData;

            var processoHierarquia = Session["__ProcessoHierarquia__"] as List<ProcessoGestaoEventoQRCodeData>;

            var processo = processoHierarquia.Find(x => x.SeqProcesso == dados.SeqProcesso);

            var modelo = new CheckinViewModel();

            modelo.SeqProcesso = processo.SeqProcesso;
            modelo.DescricaoProcesso = processo.DescricaoProcesso;
            modelo.Atendente = processo.Atendente;
            modelo.Ofertas = new List<string>();
            modelo.Ofertas.AddRange(processo.Ofertas.Where(w => dados.SeqsOferta.Contains(w.SeqOferta)).Select(x => x.DescricaoCompleta));

            return View(modelo);
        }

        [SMCAuthorize(UC_INS_005_01_02.REALIZAR_CHECKIN_POR_QRCODE)]
        public JsonResult EfetuarCheckinManual(CheckinData dadosCheckin)
        {
            var dadosUsuario = Session["__ConfiguracaoUsuario__"] as ConfiguracaoUsuarioData;

            dadosCheckin.SeqProcesso = dadosUsuario.SeqProcesso;
            dadosCheckin.SeqGrupoOferta = dadosUsuario.SeqGrupoOferta;
            dadosCheckin.SeqHierarquia = dadosUsuario.SeqHierarquia;
            dadosCheckin.SeqsOferta = dadosUsuario.SeqsOferta;
            dadosCheckin.TokenHistoricoSituacao = dadosUsuario.TokenHistoricoSituacao;

            var retorno = InscricaoOfertaService.EfetuarCheckin(dadosCheckin);

            if (retorno.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Response.StatusCode = 200;
            }
            else
            {
                Response.StatusCode = 400;
            }


            return Json(retorno);
        }

        [SMCAuthorize(UC_INS_005_01_02.REALIZAR_CHECKIN_POR_QRCODE)]
        public JsonResult PesquiarOfertaCheckinManual(FiltroCheckinData dados)
        {
            var dadosUsuario = Session["__ConfiguracaoUsuario__"] as ConfiguracaoUsuarioData;
            var processoHierarquia = Session["__ProcessoHierarquia__"] as List<ProcessoGestaoEventoQRCodeData>;

            dados.SeqsOferta = dadosUsuario.SeqsOferta;
            dados.TokenHistoricoSituacao = dadosUsuario.TokenHistoricoSituacao;

            var retorno = InscricaoOfertaService.PesquisaOfertaCheckinManual(dados);

            //Tratamento da descrição ccompleta para utilizar o que já existe na memoria
            retorno.ForEach(f => f.DescricaoOferta = processoHierarquia.Find(x => x.SeqProcesso == dadosUsuario.SeqProcesso)
                                                                                    .Ofertas.Find(y => y.SeqOferta == f.SeqOferta).DescricaoCompleta);

            retorno = retorno.OrderBy(x => x.DescricaoOferta).ToList();

            Response.StatusCode = 200;

            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        [SMCAuthorize(UC_INS_005_01_02.REALIZAR_CHECKIN_POR_QRCODE)]
        public JsonResult PesquiarNomeCheckinManual(FiltroCheckinData dados)
        {
            var dadosUsuario = Session["__ConfiguracaoUsuario__"] as ConfiguracaoUsuarioData;

            dados.SeqsOferta = dadosUsuario.SeqsOferta;
            dados.TokenHistoricoSituacao = dadosUsuario.TokenHistoricoSituacao;

            var retorno = InscricaoOfertaService.PesquisaNomeCheckinManual(dados);

            if (retorno.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Response.StatusCode = 200;
            }
            else
            {
                Response.StatusCode = 400;
            }

            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        [SMCAuthorize(UC_INS_005_01_02.REALIZAR_CHECKIN_POR_QRCODE)]
        public JsonResult CheckoutInscricao(string guid)
        {
            var retorno = InscricaoOfertaService.EfetuarCheckout(guid);

            if (retorno.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Response.StatusCode = 200;
            }
            else
            {
                Response.StatusCode = 400;
            }

            return Json(retorno, JsonRequestBehavior.AllowGet);
        }
    }
}