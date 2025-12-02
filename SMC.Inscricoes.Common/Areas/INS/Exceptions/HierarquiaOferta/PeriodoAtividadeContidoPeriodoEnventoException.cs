using SMC.Framework.Exceptions;
using SMC.Inscricoes.Common.Areas.INS.Resources;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions.HierarquiaOferta
{
    public class PeriodoAtividadeContidoPeriodoEnventoException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de HierarquiaOfertaTipoHierarquiaOfertaDesativadoException
        /// </summary>
        public PeriodoAtividadeContidoPeriodoEnventoException()
           : base(ExceptionsResource.Err_PeriodoAtividadeContidoPeriodoEnventoException)
        {
        }
    }

}