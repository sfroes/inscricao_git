using SMC.Framework.Exceptions;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class AlteracaoDocumentoRequeridoVariosArquivosException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de AlteracaoDocumentoRequeridoVariosArquivosException
        /// </summary>
        public AlteracaoDocumentoRequeridoVariosArquivosException()
            : base(Resources.ExceptionsResource.ResourceManager.GetString(
            "AlteracaoDocumentoRequeridoVariosArquivosException", System.Threading.Thread.CurrentThread.CurrentCulture))
        { }
    }
}
