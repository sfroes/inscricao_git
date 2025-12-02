using SMC.Framework.Exceptions;

namespace SMC.Inscricoes.Common.Areas.RES.Exceptions.UnidadeResponsavelTipoProcesso
{
    public class NaoExisteIdentidadeVisualAtivaException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de UsuarioInvalidoException
        /// </summary>
        public NaoExisteIdentidadeVisualAtivaException()
            : base(Resources.ExceptionsResource.ERR_NaoExisteIdentidadeVisualAtivaException)
        {
        }

    }
}
