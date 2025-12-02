using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class NumeroEditalNotificacaoException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de NumeroEditalNotificacaoException
        /// </summary>
        public NumeroEditalNotificacaoException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "NumeroEditalNotificacaoException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
