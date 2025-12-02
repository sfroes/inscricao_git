using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class ProrrogacaoEtapaDiferenteInscricaoException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de ProrrogacaoEtapaDiferenteInscricaoException
        /// </summary>
        public ProrrogacaoEtapaDiferenteInscricaoException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "ProrrogacaoEtapaDiferenteInscricaoException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
