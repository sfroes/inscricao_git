using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class ConfiguracaoEtapaNaoVigenteException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de ConfiguracaoEtapaNaoVigenteException
        /// </summary>
        public ConfiguracaoEtapaNaoVigenteException(string tipoPorcesso, string nomeGrupo)
            : base(string.Format(
            SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "ConfiguracaoEtapaNaoVigenteException", System.Threading.Thread.CurrentThread.CurrentCulture),tipoPorcesso, nomeGrupo))
        {

        }
    }
}
