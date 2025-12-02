using SMC.Framework.Exceptions;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions.OfertaPeriodoTaxa
{
    public class AlteracaoOfertaTaxaNaoPermitidaBoletoPagoValoresDiferentesException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de CadastroOfertaPeriodoTaxaNaoPermitidoException
        /// </summary>
        public AlteracaoOfertaTaxaNaoPermitidaBoletoPagoValoresDiferentesException(string nomeTaxa, decimal valorAntigo, decimal valorNovo)
            : base(string.Format(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "AlteracaoOfertaTaxaNaoPermitidaBoletoPagoValoresDiferentesException", System.Threading.Thread.CurrentThread.CurrentCulture), nomeTaxa, valorAntigo.ToString("#,##0.00"), valorNovo.ToString("#,##0.00")))
        {
        }
    }
}