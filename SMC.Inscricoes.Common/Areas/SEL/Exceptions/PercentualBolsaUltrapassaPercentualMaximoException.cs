using SMC.Framework.Exceptions;
using SMC.Inscricoes.Common.Areas.SEL.Resources;

namespace SMC.Inscricoes.Common.Areas.SEL.Exceptions
{
    public class PercentualBolsaUltrapassaPercentualMaximoException : SMCApplicationException
    {
        public PercentualBolsaUltrapassaPercentualMaximoException()
            : base(ExceptionsResource.PercentualBolsaUltrapassaPercentualMaximoException)
        { }
    }
}