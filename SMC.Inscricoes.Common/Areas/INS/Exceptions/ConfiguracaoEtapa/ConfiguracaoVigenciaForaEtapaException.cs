using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class ConfiguracaoVigenciaForaEtapaException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de ConfiguracaoVigenciaForaEtapaException
        /// </summary>
        public ConfiguracaoVigenciaForaEtapaException()
            : base(
            SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "ConfiguracaoVigenciaForaEtapaException", Thread.CurrentThread.CurrentCulture))            
        {

        }
    }
}
