using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.NOT.Exceptions
{
    public class TipoNotificacaoJaExistenteException : SMCApplicationException
    {
        public TipoNotificacaoJaExistenteException()
            : base(SMC.Inscricoes.Common.Areas.NOT.Resources.ExceptionsResource.ResourceManager.GetString(
            "TipoNotificacaoJaExistenteException", System.Threading.Thread.CurrentThread.CurrentCulture))
        { }
    }
}
