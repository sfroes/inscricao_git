using SMC.Framework.Exceptions;
using SMC.Inscricoes.Common.Areas.SEL.Resources;

namespace SMC.Inscricoes.Common.Areas.SEL.Exceptions
{
    public class LancamentoBolsaUltrapassaLimiteComExcecaoException : SMCApplicationException
    {
        public LancamentoBolsaUltrapassaLimiteComExcecaoException()
            : base(ExceptionsResource.LancamentoBolsaUltrapassaLimiteComExcecaoException)
        { }
    }
}
