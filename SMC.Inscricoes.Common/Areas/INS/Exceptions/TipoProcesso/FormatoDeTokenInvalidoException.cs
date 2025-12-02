using SMC.Framework.Exceptions;
using SMC.Inscricoes.Common.Areas.INS.Resources;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class FormatoDeTokenInvalidoException : SMCApplicationException
    {
        public FormatoDeTokenInvalidoException()
            : base(ExceptionsResource.ERR_FormatoDeTokenInvalidoException)
        {
        }
    }
}
