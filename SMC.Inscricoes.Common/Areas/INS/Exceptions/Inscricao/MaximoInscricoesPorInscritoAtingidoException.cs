using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class MaximoInscricoesPorInscritoAtingidoException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de MaximoInscricoesPorInscritoAtingidoException
        /// </summary>
        public MaximoInscricoesPorInscritoAtingidoException(string resource, Exception innerException)
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            $"MaximoInscricoesPorInscritoAtingido{resource}Exception", System.Threading.Thread.CurrentThread.CurrentCulture), innerException)
        {

        }
    }
}
