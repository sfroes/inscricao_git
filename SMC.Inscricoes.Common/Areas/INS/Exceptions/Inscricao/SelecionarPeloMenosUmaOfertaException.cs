using SMC.Framework.Exceptions;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions.Inscricao
{
    public class SelecionarPeloMenosUmaOfertaException : SMCApplicationException
    {
        public SelecionarPeloMenosUmaOfertaException() : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
             "SelecionarPeloMenosUmaOfertaException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
