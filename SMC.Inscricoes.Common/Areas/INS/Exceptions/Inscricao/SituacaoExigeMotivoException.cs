using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class SituacaoExigeMotivoException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de SituacaoExigeMotivoException
        /// </summary>
        public SituacaoExigeMotivoException()
            : base(Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "SituacaoExigeMotivoException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
