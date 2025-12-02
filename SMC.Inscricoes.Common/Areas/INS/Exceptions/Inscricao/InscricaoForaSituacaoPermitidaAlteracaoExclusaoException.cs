using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class InscricaoForaSituacaoPermitidaAlteracaoExclusaoException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de InscricaoForaSituacaoPermitidaAlteracaoExclusaoException
        /// </summary>
        public InscricaoForaSituacaoPermitidaAlteracaoExclusaoException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "InscricaoForaSituacaoPermitidaAlteracaoExclusaoException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
