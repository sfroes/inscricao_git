using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class ProrrogarEtapaValoresDistintosPeriodosCoincidentesException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de ProrrogarEtapaValoresDistintosPeriodosCoincidentesException
        /// </summary>
        public ProrrogarEtapaValoresDistintosPeriodosCoincidentesException(string taxa)
            : base(String.Format(Resources.ExceptionsResource.ResourceManager.GetString(
            "ProrrogarEtapaValoresDistintosPeriodosCoincidentesException", System.Threading.Thread.CurrentThread.CurrentCulture), taxa))
        { }
    }
}
