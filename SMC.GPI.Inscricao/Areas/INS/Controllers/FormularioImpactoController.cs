using SMC.DadosMestres.Common.Helper;
using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.GPI.Inscricao.Areas.INS.Models;
using SMC.GPI.Inscricao.Areas.INS.Services;
using SMC.GPI.Inscricao.Models;
using SMC.Inscricoes.Service.Areas.INS.Services;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using System;
using System.Security.Principal;
using System.Threading;
using System.Web.Mvc;

namespace SMC.GPI.Inscricao.Areas.INS.Controllers
{
    public class FormularioImpactoController : SMCControllerBase
    {
        #region Services

        private ProcessoControllerService ProcessoControllerService => this.Create<ProcessoControllerService>();
        private InscricaoOfertaService InscricaoOfertaService => this.Create<InscricaoOfertaService>();
        private InscricaoService InscricaoService => this.Create<InscricaoService>();

        #endregion

        [HttpGet]
        [AllowAnonymous]
        [SMCAllowAnonymous]
        public ActionResult Index(Guid? uidInscricaoOferta)
        {
            ProcessoHomeViewModel modelo = new ProcessoHomeViewModel();
            

            if (uidInscricaoOferta != null)
            {

                var dadosInscricaoOferta = InscricaoOfertaService.BuscarInscricaoOfertaPorGuid(uidInscricaoOferta.Value);

                if (dadosInscricaoOferta == null)
                {
                    return View("FomularioIndisponivel");
                }

                SMCLanguage idioma = this.GetCurrentLanguage().HasValue ? this.GetCurrentLanguage().Value : SMCLanguage.Portuguese;
                modelo = ProcessoControllerService.BuscarProcessoHome(dadosInscricaoOferta.UidProcesso, idioma, dadosInscricaoOferta.SeqInscrito);
                Session["__CSSLayout__"] = modelo.UrlCss;
                modelo.UidProcesso = dadosInscricaoOferta.UidProcesso.ToString();
                Session["__GuidProcesso__"] = dadosInscricaoOferta.UidProcesso.ToString();
                Session["__dadosInscricaoOferta__"] = dadosInscricaoOferta;                
            }
            else
            {
                @ViewBag.Mensagem = "Inscrição não encontrada";
                return View("FomularioIndisponivel");
            }


            if (!modelo.FormularioImpacto.ExibirFormularioImpacto)
            {
                @ViewBag.Mensagem = modelo.FormularioImpacto.MensagemFormularioImpactoIndisponivel;
                @ViewBag.TokenCss = modelo.TokenCssAlternativoSas;
                return View("FomularioIndisponivel");
            }
            modelo.UidInscricaoOferta = uidInscricaoOferta;
            return View(modelo);
        }

        [AllowAnonymous]
        [SMCAllowAnonymous]
        public ActionResult SalvarFormularioImpacto(InscricaoDadoFormularioViewModel modelo)
        {
            var dados = SMCMapperHelper.Create<InscricaoDadoFormularioData>(modelo);

            if((Session["__dadosInscricaoOferta__"] as InscricaoOfertaData) == null)
            {
                return RedirectToAction("Index", new {uidInscricaoOferta = modelo.UidInscricaoOferta });
            }

            SimularLogin(Session["__dadosInscricaoOferta__"] as InscricaoOfertaData);

            InscricaoService.SalvarFormularioImpacto(dados);

            @ViewBag.TokenCss = modelo.TokenCssAlternativoSas;

            return View("FormularioRespondido"); ;
        }

        [NonAction]
        private void SimularLogin(InscricaoOfertaData dados)
        {
            SecurityHelper.SetupSatUser("Fomulário via email.");

      

            var splitNome = dados.NomeInscrito.ToUpper().Split(' ');

            var identificacao = $@"Fomulário via email\{splitNome[0]} {splitNome[splitNome.Length -1]}";
            var generic = new GenericIdentity(identificacao, "Ânonimo");
            generic.AddClaim(new System.Security.Claims.Claim("SeqUsuario", dados.SeqUsuario.GetValueOrDefault().ToString()));
            generic.AddClaim(new System.Security.Claims.Claim("Nome", dados.NomeInscrito));
            generic.AddClaim(new System.Security.Claims.Claim("ApplicationId", "GPI"));

            Thread.CurrentPrincipal = new GenericPrincipal(generic, null);


        }
    }
}