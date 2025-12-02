using SMC.Framework.Exceptions;
using SMC.Inscricoes.Common.Areas.INS.Resources;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions.Inscricao
{
    public class OfertaImpedidaException : SMCApplicationException
    {
        public OfertaImpedidaException()
            : base(ExceptionsResource.Err_OfertaImpedidaException)
        { }

    }
}
