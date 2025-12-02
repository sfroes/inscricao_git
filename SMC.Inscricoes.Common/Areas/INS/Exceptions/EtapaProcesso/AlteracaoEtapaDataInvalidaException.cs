using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class AlteracaoEtapaDataInvalidaException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de EtapaProcessoEmManutencaoException
        /// </summary>
        public AlteracaoEtapaDataInvalidaException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "AlteracaoEtapaDataInvalidaException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
