using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class AlterarCobrancaPorOfertaException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de AlterarCobrancaPorOfertaException
        /// </summary>
        public AlterarCobrancaPorOfertaException()
            : base(Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "AlterarCobrancaPorOfertaException", System.Threading.Thread.CurrentThread.CurrentCulture))
        { }
    }
}
