using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class InclusaoPeriodoTaxaEtapaLiberadaExcetpion : SMCApplicationException
    {
        /// <summary>
        /// Construtor de InclusaoPeriodoTaxaEtapaLiberadaExcetpion
        /// </summary>
        public InclusaoPeriodoTaxaEtapaLiberadaExcetpion()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "InclusaoPeriodoTaxaEtapaLiberadaExcetpion", System.Threading.Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
