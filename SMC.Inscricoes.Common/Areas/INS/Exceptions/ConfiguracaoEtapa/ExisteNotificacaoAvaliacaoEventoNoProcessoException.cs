using SMC.Framework.Exceptions;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions.ConfiguracaoEtapa
{
    public class ExisteNotificacaoAvaliacaoEventoNoProcessoException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de EtapaProcessoEmManutencaoException
        /// </summary>
        public ExisteNotificacaoAvaliacaoEventoNoProcessoException()
            : base(Resources.ExceptionsResource.Err_ExisteNotificacaoAvaliacaoEventoNoProcessoException)
        {

        }
    }
}
