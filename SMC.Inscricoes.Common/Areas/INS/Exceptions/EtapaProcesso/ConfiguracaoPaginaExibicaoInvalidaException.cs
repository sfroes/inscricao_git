using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class ConfiguracaoPaginaExibicaoInvalidaException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de EtapaProcessoEmManutencaoException
        /// </summary>
        public ConfiguracaoPaginaExibicaoInvalidaException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "ConfiguracaoPaginaExibicaoInvalidaException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
