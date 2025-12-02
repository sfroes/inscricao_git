using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class GrupoDocumentoDocumentoSemUploadException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de GrupoDocumentoDocumentoSemUploadException
        /// </summary>
        public GrupoDocumentoDocumentoSemUploadException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "GrupoDocumentoDocumentoSemUploadException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
