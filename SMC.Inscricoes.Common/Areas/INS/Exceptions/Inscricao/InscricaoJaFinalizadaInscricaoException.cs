using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class InscricaoJaFinalizadaInscricaoException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de InscricaoJaFinalizadaInscricaoException
        /// </summary>
        public InscricaoJaFinalizadaInscricaoException()
            : base(Resources.ExceptionsResource.InscricaoJaFinalizadaInscricaoException)
        {

        }
    }
}
