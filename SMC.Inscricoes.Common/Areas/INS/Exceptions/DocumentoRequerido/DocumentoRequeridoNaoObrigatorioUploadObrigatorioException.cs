using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class DocumentoRequeridoNaoObrigatorioUploadObrigatorioException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de DocumentoRequeridoNaoObrigatorioUploadObrigatorioException
        /// </summary>
        public DocumentoRequeridoNaoObrigatorioUploadObrigatorioException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "DocumentoRequeridoNaoObrigatorioUploadObrigatorioException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
