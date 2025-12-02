using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class ConfiguracaoEtapaPaginaIdiomaInvalidaException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de ConfiguracaoEtapaPaginaIdiomaException
        /// </summary>
        public ConfiguracaoEtapaPaginaIdiomaInvalidaException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "ConfiguracaoEtapaPaginaIdiomaInvalidaException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
