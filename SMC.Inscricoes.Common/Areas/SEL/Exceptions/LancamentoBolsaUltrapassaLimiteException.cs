using SMC.Framework.Exceptions;
using SMC.Inscricoes.Common.Areas.SEL.Resources;

namespace SMC.Inscricoes.Common.Areas.SEL.Exceptions
{
    public class LancamentoBolsaUltrapassaLimiteException : SMCApplicationException
    {
        public LancamentoBolsaUltrapassaLimiteException()
            : base(ExceptionsResource.LancamentoBolsaUltrapassaLimiteException)
        { }
    }
}
