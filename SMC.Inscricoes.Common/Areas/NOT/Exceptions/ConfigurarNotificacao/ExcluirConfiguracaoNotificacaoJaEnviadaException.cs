using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.NOT.Exceptions
{
    public class ExcluirConfiguracaoNotificacaoJaEnviadaException : SMCApplicationException
    {
        public ExcluirConfiguracaoNotificacaoJaEnviadaException()
            : base(SMC.Inscricoes.Common.Areas.NOT.Resources.ExceptionsResource.ResourceManager.GetString(
            "ExcluirConfiguracaoNotificacaoJaEnviadaException", System.Threading.Thread.CurrentThread.CurrentCulture))
        { }
    }
}
