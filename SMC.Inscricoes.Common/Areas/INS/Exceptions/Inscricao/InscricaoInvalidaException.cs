using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class InscricaoInvalidaException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de InscricaoInvalidaException
        /// </summary>
        public InscricaoInvalidaException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "InscricaoInvalidaException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
