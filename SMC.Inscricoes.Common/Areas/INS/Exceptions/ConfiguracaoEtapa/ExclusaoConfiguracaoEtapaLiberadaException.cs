using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class ExclusaoConfiguracaoEtapaLiberadaException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de ExclusaoConfiguracaoEtapaLiberadaException
        /// </summary>
        public ExclusaoConfiguracaoEtapaLiberadaException()
            : base(
            SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "ExclusaoConfiguracaoEtapaLiberadaException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
