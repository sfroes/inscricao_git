using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class InscricaoComEtapaInativaException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de InscricaoForaSituacaoPermitidaAlteracaoExclusaoException
        /// </summary>
        public InscricaoComEtapaInativaException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "InscricaoComEtapaInativaException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
