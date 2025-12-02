using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.NOT.Exceptions
{
    public class ParametroComNotificacaoJaEnviadaException : SMCApplicationException
    {
        public ParametroComNotificacaoJaEnviadaException()
            : base(SMC.Inscricoes.Common.Areas.NOT.Resources.ExceptionsResource.ResourceManager.GetString(
            "ParametroComNotificacaoJaEnviadaException", System.Threading.Thread.CurrentThread.CurrentCulture))
        { }
    }
}
