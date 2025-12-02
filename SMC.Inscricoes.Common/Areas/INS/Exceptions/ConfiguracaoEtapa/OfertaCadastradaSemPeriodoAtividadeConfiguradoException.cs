using SMC.Framework.Exceptions;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions.ConfiguracaoEtapa
{
    public class OfertaCadastradaSemPeriodoAtividadeConfiguradoException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de EtapaProcessoEmManutencaoException
        /// </summary>
        public OfertaCadastradaSemPeriodoAtividadeConfiguradoException()
            : base(Resources.ExceptionsResource.Err_OfertaCadastradaSemPeriodoAtividadeConfiguradoException)
        {

        }
    }
}
