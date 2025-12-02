using SMC.Framework.Exceptions;
using SMC.Inscricoes.Common.Areas.INS.Resources;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class InscricaoHistoricoSituacaoSemHistoricoAnteriorException : SMCApplicationException
    {
        public InscricaoHistoricoSituacaoSemHistoricoAnteriorException()
            : base(ExceptionsResource.InscricaoHistoricoSituacaoSemHistoricoAnteriorException)
        { }
    }
}
