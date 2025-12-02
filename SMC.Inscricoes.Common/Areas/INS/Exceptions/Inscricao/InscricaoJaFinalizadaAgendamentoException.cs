using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class InscricaoJaFinalizadaAgendamentoException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de InscricaoJaFinalizadaAgendamentoException
        /// </summary>
        public InscricaoJaFinalizadaAgendamentoException()
            : base(Resources.ExceptionsResource.InscricaoJaFinalizadaAgendamentoException)
        {

        }
    }
}
