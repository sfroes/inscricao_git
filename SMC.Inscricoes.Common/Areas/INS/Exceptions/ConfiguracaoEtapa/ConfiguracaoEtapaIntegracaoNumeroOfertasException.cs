using SMC.Framework.Exceptions;
using System.Threading;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class ConfiguracaoEtapaIntegracaoNumeroOfertasException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de ConfiguracaoEtapaIntegracaoNumeroOfertasException
        /// </summary>
        public ConfiguracaoEtapaIntegracaoNumeroOfertasException()
            : base(
            SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "ConfiguracaoEtapaIntegracaoNumeroOfertasException", Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
