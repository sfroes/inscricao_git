using SMC.Formularios.ServiceContract.Areas.FRM.Data;
using SMC.Formularios.UI.Mvc.Models;
using SMC.Framework.Extensions;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.Html;
using SMC.Framework.UI.Mvc.Security;
using SMC.GPI.Administrativo.Areas.INS.Models;
using SMC.Inscricoes.Common.Areas.INS.Constants;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMC.GPI.Administrativo.Areas.INS.Controllers
{
    public class GestaoInscricaoController : SMCControllerBase
    {
        #region Services
        private IInscricaoService InscricaoService => Create<IInscricaoService>();
        #endregion
        // GET: INS/GestaoInscricao
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        [SMCAuthorize(UC_INS_003_01_13.ALTERAR_FORMULARIO_INSCRICAO)]
        public ActionResult EditarFormulario(EditarFormularioViewModel model)
        {
            var dado = model.Transform<EditarFormularioDadoViewModel>();

            dado.DadosCampos = InscricaoService.BuscarDadoFormulario(model.Seq).DadosCampos.TransformList<DadoCampoViewModel>();

            return PartialView("_EditarFormulario", dado);
        }

        [SMCAuthorize(UC_INS_003_01_13.ALTERAR_FORMULARIO_INSCRICAO)]
        [HttpPost]
        public ActionResult SalvarFormulario(EditarFormularioDadoViewModel model)
        {
            InscricaoService.AlterarFormularioInscricao(model.Transform<InscricaoDadoFormularioData>());

            SetSuccessMessage(Views.GestaoInscricao.App_LocalResources.UIResource.Mensagem_Alteracao, target: SMCMessagePlaceholders.Centro);
            return SMCRedirectToAction("ConsultaInscricao", "AcompanhamentoProcesso", new { seqInscricao = (SMCEncryptedLong)model.SeqInscricao, origem = model.Origem });
        }
    }
}