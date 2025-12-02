using SMC.Framework.Exceptions;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions.Inscricao
{
    public class ExigeJustificativaException : SMCApplicationException
    {

        public ExigeJustificativaException()
            : base(Resources.ExceptionsResource.ERR_ExigeJustificativaException)
        {

        }
    }
}
