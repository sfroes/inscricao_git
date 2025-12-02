using SMC.Framework.Exceptions;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class AlteracaoDocumentoRequeridoPermiteEntregaPosteriorException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de AlteracaoDocumentoRequeridoPermiteEntregaPosteriorException
        /// </summary>
        public AlteracaoDocumentoRequeridoPermiteEntregaPosteriorException()
            : base(Resources.ExceptionsResource.ResourceManager.GetString(
            "AlteracaoDocumentoRequeridoPermiteEntregaPosteriorException", System.Threading.Thread.CurrentThread.CurrentCulture))
        { }
    }
}
