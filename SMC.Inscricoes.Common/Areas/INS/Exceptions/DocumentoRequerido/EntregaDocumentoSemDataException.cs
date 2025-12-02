using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class EntregaDocumentoSemDataException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de EntregaDocumentoSemDataException
        /// </summary>
        public EntregaDocumentoSemDataException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "EntregaDocumentoSemDataException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
