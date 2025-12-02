using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class EntregaDocumentoNotificacaoSemDataPrazoNovaEntregaDocumentacaoException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de EntregaDocumentoNotificacaoSemDataPrazoNovaEntregaDocumentacaoException
        /// </summary>
        public EntregaDocumentoNotificacaoSemDataPrazoNovaEntregaDocumentacaoException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "EntregaDocumentoNotificacaoSemDataPrazoNovaEntregaDocumentacaoException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
