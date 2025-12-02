
using SMC.Framework.Exceptions;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class DocumentoRequeridoAdicionalObrigatorioPosteriormenteException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de AlteracaoDocumentoRequeridoVersaoException
        /// </summary>
        public DocumentoRequeridoAdicionalObrigatorioPosteriormenteException(string descricaoTipo)
            : base(string.Format(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "DocumentoRequeridoAdicionalObrigatorioPosteriormenteException", System.Threading.Thread.CurrentThread.CurrentCulture), descricaoTipo))
        { 

        }
    }
}