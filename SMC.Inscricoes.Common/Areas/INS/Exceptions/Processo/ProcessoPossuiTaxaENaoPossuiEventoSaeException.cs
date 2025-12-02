using SMC.Framework.Exceptions;
using SMC.Inscricoes.Common.Areas.INS.Resources;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions.Processo
{
    public class ProcessoPossuiTaxaENaoPossuiEventoSaeException : SMCApplicationException
    {
        public ProcessoPossuiTaxaENaoPossuiEventoSaeException()
            : base(ExceptionsResource.ERR_ProcessoPossuiTaxaENaoPossuiEventoSaeException)
        {
        }
    }
}
