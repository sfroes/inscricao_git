using SMC.Framework.Exceptions;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class AlteracaoDocumentoRequeridoValidacaoOutroSetorException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de AlteracaoDocumentoRequeridoValidacaoOutroSetorException
        /// </summary>
        public AlteracaoDocumentoRequeridoValidacaoOutroSetorException()
            : base(Resources.ExceptionsResource.ResourceManager.GetString(
            "AlteracaoDocumentoRequeridoValidacaoOutroSetorException", System.Threading.Thread.CurrentThread.CurrentCulture))
        { }
    }
}
