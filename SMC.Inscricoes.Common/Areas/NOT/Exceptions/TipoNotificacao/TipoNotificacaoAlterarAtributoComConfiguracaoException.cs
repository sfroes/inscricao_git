using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.NOT.Exceptions
{
    public class TipoNotificacaoAlterarAtributoComConfiguracaoException : SMCApplicationException
    {
        public TipoNotificacaoAlterarAtributoComConfiguracaoException()
            : base(SMC.Inscricoes.Common.Areas.NOT.Resources.ExceptionsResource.ResourceManager.GetString(
            "TipoNotificacaoAlterarAtributoComConfiguracaoException", System.Threading.Thread.CurrentThread.CurrentCulture))
        { }
    }
}
