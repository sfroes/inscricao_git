using SMC.Framework.Exceptions;
using SMC.Inscricoes.Common.Areas.INS.Resources;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class DevePossuirPeriodoTaxaIguaisException : SMCApplicationException
    {
        public DevePossuirPeriodoTaxaIguaisException()
            : base(ExceptionsResource.ERR_DevePossuirPeriodoTaxaIguaisException)
        {
        }
    }
}

