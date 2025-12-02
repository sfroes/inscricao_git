using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class DataEntregaDocumentacaoNotificacaoException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de DataEntregaDocumentacaoNotificacaoException
        /// </summary>
        public DataEntregaDocumentacaoNotificacaoException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "DataEntregaDocumentacaoNotificacaoException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
