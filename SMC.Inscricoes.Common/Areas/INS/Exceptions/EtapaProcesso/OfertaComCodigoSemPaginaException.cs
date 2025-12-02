using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class OfertaComCodigoSemPaginaException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de OfertaComCodigoSemPaginaException
        /// </summary>
        public OfertaComCodigoSemPaginaException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "OfertaComCodigoSemPaginaException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
