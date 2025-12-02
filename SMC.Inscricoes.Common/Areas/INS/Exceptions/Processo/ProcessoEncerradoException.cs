using SMC.Framework.Exceptions;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class ProcessoEncerradoException : SMCApplicationException
    {
        public ProcessoEncerradoException()
            : base(Resources.ExceptionsResource.ERR_ProcessoEncerradoException)
        {
        }
    }
}