using SMC.Framework.Exceptions;
using SMC.Inscricoes.Common.Areas.INS.Resources;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class InscricaoExportadaAlterarDeferimentoException : SMCApplicationException
    {
        public InscricaoExportadaAlterarDeferimentoException()
            : base(ExceptionsResource.InscricaoExportadaAlterarDeferimentoException)
        { }
    }
}