using SMC.Framework.Exceptions;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class ComprovanteInscricaoCanceladaException : SMCApplicationException
    {
        public ComprovanteInscricaoCanceladaException(string motivo)
            : base(string.Format(Resources.ExceptionsResource.ResourceManager.GetString(
            "ERR_ComprovanteInscricaoCanceladaException", System.Threading.Thread.CurrentThread.CurrentCulture), motivo))
        {
        }
    }
}