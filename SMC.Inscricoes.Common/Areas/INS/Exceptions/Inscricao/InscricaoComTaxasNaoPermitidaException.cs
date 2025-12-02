using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class InscricaoComTaxasNaoPermitidaException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de InscricaoComTaxasNaoPermitidaException
        /// </summary>
        public InscricaoComTaxasNaoPermitidaException(string tipoTaxa, string paginaSelecao)
            : base(string.Format(Resources.ExceptionsResource.ERR_InscricaoComTaxasNaoPermitidaException, tipoTaxa, paginaSelecao))
        {

        }
    }
}
