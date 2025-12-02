using SMC.Framework.Exceptions;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class BoletoInscricaoCanceladaException : SMCApplicationException
    {
        public BoletoInscricaoCanceladaException(string motivo)
            : base(string.Format(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "BoletoInscricaoCanceladaException", System.Threading.Thread.CurrentThread.CurrentCulture), motivo))
        {
        }
    }
}