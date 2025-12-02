using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class EtapaProcessoEmManutencaoException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de EtapaProcessoEmManutencaoException
        /// </summary>
        public EtapaProcessoEmManutencaoException(string tipoProcesso)
            : base(string.Format(Resources.ExceptionsResource.EtapaProcessoEmManutencaoException, tipoProcesso))
        {

        }
    }
}
