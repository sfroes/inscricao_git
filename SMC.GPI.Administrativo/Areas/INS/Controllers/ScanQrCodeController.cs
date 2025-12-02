using SMC.Formularios.UI.Mvc;
using SMC.Framework;
using SMC.GPI.Administrativo.Areas.INS.Services;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using System;
using System.Web.Mvc;

namespace SMC.GPI.Administrativo.Areas.INS.Controllers
{
    public class ScanQrCodeController : SGFController
    {
        private ScanQrCodeControllerService ScanQrCodeControllerService
        {
            get { return this.Create<ScanQrCodeControllerService>(); }
        }

        private IOfertaService OfertaService
        {
            get { return this.Create<IOfertaService>(); }
        }

        private IProcessoService ProcessoService
        {
            get { return this.Create<IProcessoService>(); }
        }

        [SMCAllowAnonymous]
        public ActionResult Index()
        {
            ProcessoService.BuscarProcessoHierarquiaLeituraQRCode();
            return View();
        }

        [SMCAllowAnonymous]
        public ActionResult Cam(string leitura)
        {
            string retorno = string.Empty;
            try
            {
                var qrValido = long.TryParse(leitura, out var seqInscricao);

                if (qrValido)
                {
                    retorno = ScanQrCodeControllerService.BuscarNomeInscritosSeqInscricao(seqInscricao);

                    if (string.IsNullOrEmpty(retorno))
                    {
                        SetErrorMessage("QrCode possui matricula inválida", closeTimer: 2000);
                    }
                    else
                    {
                        SetSuccessMessage("Leitura efetuada com sucesso!", closeTimer: 1000);
                    }
                }
                else
                {
                    SetErrorMessage("O QrCode informado não pertence ao evento ou não é válido, resultado da leitura: \n" + leitura, closeTimer: 3000);
                }

                return Json(retorno, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                SetErrorMessage("Falha ao ler o QrCode Erro : " + e.Message, closeTimer: 3000);
                return Json(retorno, JsonRequestBehavior.AllowGet);
            }

        }
    }
}