using SMC.Framework.Exceptions;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions.Inscricao
{
    public class PermissaoEmitirDocumentacaoException : SMCApplicationException
    {
        public PermissaoEmitirDocumentacaoException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "Err_PermissaoEmitirDocumentacaoException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
