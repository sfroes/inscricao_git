using SMC.Framework;
using SMC.Framework.Extensions;
using SMC.Framework.UI.Mvc;
using SMC.GPI.Inscricao.Areas.INS.Models;
using SMC.Inscricoes.Service.Areas.INS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.SessionState;

namespace SMC.GPI.Inscricao.Areas.INS.Controllers
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    [SMCAllowAnonymous]
    public class FrontController : SMCControllerBase
    {
        internal InscricaoService InscricaoService => Create<InscricaoService>();
        public ActionResult PartialHeader()
        {
            return PartialView("_PartialHeader");
        }
        public ActionResult PartialFooter()
        {
            return PartialView("_PartialFooter");
        }
        public JsonResult BuscarDescricaoProcesso(long seqInscricao)
        {
            string retorno = string.Empty;

            retorno = InscricaoService.BuscarDescricaoProcessoInscricao(seqInscricao);

            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PartialMenuLateral([FromBody] ModeloPaginaAngularViewModel model)
        {
            return PartialView("_PainelLateralProcessoAngular", model);
        }

        public ActionResult PartialNavegacao([FromBody] ModeloPaginaAngularViewModel model)
        {
            return PartialView("_BotoesNavegacaoAngular", model);
        }
    }

}