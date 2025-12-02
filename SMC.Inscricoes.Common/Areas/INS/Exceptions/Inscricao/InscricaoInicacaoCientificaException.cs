using SMC.Framework.Exceptions;
using SMC.Inscricoes.Common.Areas.INS.Resources;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class InscricaoInicacaoCientificaException : SMCApplicationException
    {
        public InscricaoInicacaoCientificaException()
            : base(ExceptionsResource.ERR_IncricaoIniciacaoCientificaException)
        { }
    }
}
