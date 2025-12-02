using SMC.Framework.Exceptions;
using SMC.Inscricoes.Common.Areas.INS.Resources;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class UsuarioComDebidoFinanceiroException : SMCApplicationException
    {
        public UsuarioComDebidoFinanceiroException(string processo, string telefones)
            : base(string.Format(ExceptionsResource.UsuarioComDebidoFinanceiroException, processo, telefones))
        { }
    }
}
