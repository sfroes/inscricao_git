using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class PesquisaInscricaoInvalidaException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de PesquisaInscricaoInvalidaException
        /// </summary>
        public PesquisaInscricaoInvalidaException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "PesquisaInscricaoInvalidaException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
