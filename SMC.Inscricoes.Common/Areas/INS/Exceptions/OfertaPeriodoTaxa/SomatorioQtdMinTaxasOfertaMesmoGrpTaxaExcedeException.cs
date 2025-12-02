using SMC.Framework.Exceptions;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class SomatorioQtdMinTaxasOfertaMesmoGrpTaxaExcedeException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de CadastroOfertaPeriodoTaxaNaoPermitidoException
        /// </summary>       
        public SomatorioQtdMinTaxasOfertaMesmoGrpTaxaExcedeException(string descHierarquiaCompleta, string descGrupoTaxa)
        : base(string.Format(
            SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "SomatorioQtdMinTaxasOfertaMesmoGrpTaxaExcedeException", System.Threading.Thread.CurrentThread.CurrentCulture),
            descHierarquiaCompleta, descGrupoTaxa))
        { }
    }
}