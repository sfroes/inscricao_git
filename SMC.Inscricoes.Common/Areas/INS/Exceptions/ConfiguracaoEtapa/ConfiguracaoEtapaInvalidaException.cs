using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class ConfiguracaoEtapaInvalidaException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de ConfiguracaoEtapaInvalidaException
        /// </summary>
        public ConfiguracaoEtapaInvalidaException()
            : base(
            SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "ConfiguracaoEtapaInvalidaException",Thread.CurrentThread.CurrentCulture))            
        {

        }
    }
}
