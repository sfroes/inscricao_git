using SMC.Framework.Exceptions;
using SMC.Inscricoes.Common.Areas.INS.Resources;


namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class PeriodoEventoAtividadeException : SMCApplicationException
    {   
        public PeriodoEventoAtividadeException() 
            : base(ExceptionsResource.ResourceManager.GetString(
                "PeriodoEventoAtividadeException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {
        }

    }
}
