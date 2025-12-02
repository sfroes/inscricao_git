using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class AlteracaoConfiguracaoEtapaLiberadaException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de ConfiguracaoEtapaInvalidaException
        /// </summary>
        public AlteracaoConfiguracaoEtapaLiberadaException()
            : base(
            SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "AlteracaoConfiguracaoEtapaLiberadaException",Thread.CurrentThread.CurrentCulture))            
        {

        }
    }
}
