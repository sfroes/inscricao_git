using SMC.Framework.Exceptions;
using SMC.Inscricoes.Common.Areas.INS.Resources;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class ExtensaoDocumentoNovaDocumentacaoException : SMCException
    {
        public ExtensaoDocumentoNovaDocumentacaoException(string nomeArquivo, string extensoes )
            : base(string.Format(ExceptionsResource.ExtensaoDocumentoNovaDocumentacaoException, nomeArquivo, extensoes ))
        { }
    }
}
