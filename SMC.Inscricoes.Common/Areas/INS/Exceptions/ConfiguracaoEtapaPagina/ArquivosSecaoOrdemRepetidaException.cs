using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class ArquivosSecaoOrdemRepetidaException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de ArquivosSecaoOrdemRepetidaException
        /// </summary>
        public ArquivosSecaoOrdemRepetidaException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "ArquivosSecaoOrdemRepetidaException", System.Threading.Thread.CurrentThread.CurrentCulture))
        { }
    }
}
