using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class InclusaoDocumentoRequeridoDocumentacaoEntregueException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de InclusaoDocumentoRequeridoDocumentacaoEntregueException
        /// </summary>
        public InclusaoDocumentoRequeridoDocumentacaoEntregueException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "InclusaoDocumentoRequeridoDocumentacaoEntregueException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
