using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class DescricaoDocumentacaoNotificacaoException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de DescricaoDocumentacaoNotificacaoException
        /// </summary>
        public DescricaoDocumentacaoNotificacaoException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "DescricaoDocumentacaoNotificacaoException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
