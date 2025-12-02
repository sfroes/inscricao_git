using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class OfertaNaoDisponivelException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de OfertaNaoDisponivelException
        /// </summary>
        public OfertaNaoDisponivelException(string artigo, string tipoProcesso, string oferta)
            : base(string.Format(
                Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
                    "OfertaNaoDisponivelException", System.Threading.Thread.CurrentThread.CurrentCulture),artigo, tipoProcesso, oferta))
        { }
    }
}
