using SMC.Framework.Exceptions;
using SMC.Inscricoes.Common.Areas.INS.Resources;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class InscricaoSeminarioInvalidaException : SMCApplicationException
    {
        public InscricaoSeminarioInvalidaException(string descricaoProcesso)
            : base(string.Format(ExceptionsResource.ERR_InscricaoSeminarioInvalidaException, descricaoProcesso))
        { }
    }
}
