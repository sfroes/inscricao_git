using SMC.Framework.Exceptions;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class ProcessoCanceladoException : SMCApplicationException
    {
        public ProcessoCanceladoException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "ProcessoCanceladoException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {
        }
    }
}