using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class OfertaPeriodoTaxaFechadaException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de InscricaoComOfertaInvalidaException
        /// </summary>
        public OfertaPeriodoTaxaFechadaException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "OfertaPeriodoTaxaFechadaException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
