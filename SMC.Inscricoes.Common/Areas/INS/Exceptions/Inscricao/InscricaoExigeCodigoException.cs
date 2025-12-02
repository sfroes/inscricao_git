using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class InscricaoExigeCodigoAutorizacaoException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de InscricaoExigeCodigoAutorizacaoException
        /// </summary>
        public InscricaoExigeCodigoAutorizacaoException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "InscricaoExigeCodigoAutorizacaoException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
