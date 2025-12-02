using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class UploadPermitidoSemPaginaException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de UploadPermitidoSemPaginaException
        /// </summary>
        public UploadPermitidoSemPaginaException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "UploadPermitidoSemPaginaException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
