using SMC.Framework.Exceptions;
using SMC.Inscricoes.Common.Areas.INS.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class ProcessoTagInvalidaNotificacaoException : SMCApplicationException
    {
        public ProcessoTagInvalidaNotificacaoException(string assunto, string descTipo) 
            : base(string.Format(ExceptionsResource.ProcessoTagInvalidaNotificacaoException, assunto, descTipo))
        { }
    }
}
