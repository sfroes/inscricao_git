using SMC.Framework.Model;
using SMC.Framework.UI.Mvc.Controllers.Service;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;

namespace SMC.GPI.Administrativo.Services
{
    public class ArquivoControllerService : SMCControllerServiceBase
    {
        private IArquivoAnexadoService ArquivoAnexadoService
        {
            get { return this.Create<IArquivoAnexadoService>(); }
        }

        public SMCUploadFile BuscarArquivo(long seq)
        {
            return ArquivoAnexadoService.BuscarArquivoAnexado(seq);
        }
    }
}