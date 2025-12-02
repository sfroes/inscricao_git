using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class AlteracaoDocumentoRequeridoUploadObrigatorioException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de AlteracaoDocumentoRequeridoUploadObrigatorioException
        /// </summary>
        public AlteracaoDocumentoRequeridoUploadObrigatorioException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "AlteracaoDocumentoRequeridoUploadObrigatorioException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
