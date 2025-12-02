using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class EnvioNotificacaoException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de EnvioNotificacaoException
        /// </summary>
        public EnvioNotificacaoException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "EnvioNotificacaoException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
