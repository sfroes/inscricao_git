using SMC.Framework.Exceptions;
using SMC.Inscricoes.Common.Areas.INS.Resources;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class ExisteInscricaoRegistradaParaEssaInscricaoException  : SMCApplicationException
    {
        public ExisteInscricaoRegistradaParaEssaInscricaoException()
            : base(ExceptionsResource.ERR_ExisteInscricaoRegistradaParaEssaInscricaoException)
        { }
    }
}
