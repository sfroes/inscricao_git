using SMC.Framework.Exceptions;
using SMC.Inscricoes.Common.Areas.INS.Resources;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions.Processo
{
    public class IdVisualDesativadaException : SMCApplicationException
    {
        public IdVisualDesativadaException(string menssage)
            : base(string.Format(ExceptionsResource.ERR_IdVisualDesativadaException, menssage))
        {
        }
    }
}
