using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class CodigoAutorizacaoInvalidoParaInscricaoException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de CodigoAutorizacaoInvalidoException
        /// </summary>
        public CodigoAutorizacaoInvalidoParaInscricaoException()
            : base(
            SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "CodigoAutorizacaoInvalidoParaInscricaoException",Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
