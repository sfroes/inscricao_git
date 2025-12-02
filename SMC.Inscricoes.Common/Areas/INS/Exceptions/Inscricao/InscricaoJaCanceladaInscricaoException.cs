using SMC.Framework.Exceptions;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions.Inscricao
{

    public class InscricaoJaCanceladaInscricaoException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de InscricaoJaFinalizadaInscricaoException
        /// </summary>
        public InscricaoJaCanceladaInscricaoException()
            : base(Resources.ExceptionsResource.InscricaoJaCanceladaInscricaoException)
        {

        }
    }
}
