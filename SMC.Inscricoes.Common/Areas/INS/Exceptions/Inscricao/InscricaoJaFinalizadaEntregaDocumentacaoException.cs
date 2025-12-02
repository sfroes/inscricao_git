using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class InscricaoJaFinalizadaEntregaDocumentacaoException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de InscricaoJaFinalizadaEntregaDocumentacaoException
        /// </summary>
        public InscricaoJaFinalizadaEntregaDocumentacaoException()
            : base(Resources.ExceptionsResource.InscricaoJaFinalizadaEntregaDocumentacaoException)
        {

        }
    }
}
