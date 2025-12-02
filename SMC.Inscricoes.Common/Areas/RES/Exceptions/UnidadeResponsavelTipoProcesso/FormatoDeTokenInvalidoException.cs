using SMC.Framework.Exceptions;
using SMC.Inscricoes.Common.Areas.RES.Resources;

namespace SMC.Inscricoes.Common.Areas.RES.Exceptions.UnidadeResponsavelTipoProcesso
{
    public class FormatoDeTokenInvalidoException : SMCApplicationException
    {
        public FormatoDeTokenInvalidoException(string token)
            : base(string.Format(ExceptionsResource.ERR_FormatoDeTokenInvalidoException, token))
        {
        }
    }
}
