using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class AlterarTaxaOfertaEventoTaxaException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de AlterarTaxaOfertaEventoTaxaException
        /// </summary>
        public AlterarTaxaOfertaEventoTaxaException()
            : base(Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
                "AlterarTaxaOfertaEventoTaxaException", System.Threading.Thread.CurrentThread.CurrentCulture))
        { }
    }
}
