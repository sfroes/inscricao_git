using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class InscricaoExistenteComCodigoAutorizacaoException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de CodigoAutorizacaoInvalidoException
        /// </summary>
        public InscricaoExistenteComCodigoAutorizacaoException(string codigo)
            : base(String.Format(
            SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "InscricaoExistenteComCodigoAutorizacaoException",Thread.CurrentThread.CurrentCulture),codigo))
        {

        }
    }
}
