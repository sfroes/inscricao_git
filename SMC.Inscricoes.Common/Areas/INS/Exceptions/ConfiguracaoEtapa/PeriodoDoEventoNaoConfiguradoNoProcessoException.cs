using SMC.Framework.Exceptions;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions.ConfiguracaoEtapa
{
    public class PeriodoDoEventoNaoConfiguradoNoProcessoException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de EtapaProcessoEmManutencaoException
        /// </summary>
        public PeriodoDoEventoNaoConfiguradoNoProcessoException()
            : base(Resources.ExceptionsResource.Err_PeriodoDoEventoNaoConfiguradoNoProcessoException)
        {
        }
    }
}
