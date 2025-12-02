using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class ProrrogacaoEtapaDataOfertaMaiorConfiguracaoException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de ProrrogacaoEtapaDataOfertaMaiorConfiguracaoException
        /// </summary>
        public ProrrogacaoEtapaDataOfertaMaiorConfiguracaoException(string nome)
            : base(String.Format(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "ProrrogacaoEtapaDataOfertaMaiorConfiguracaoException", System.Threading.Thread.CurrentThread.CurrentCulture),nome))
        {

        }
    }
}
