using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class ConfiguracaoPaginaIdiomaTipoFormularioDesativadoException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de ConfiguracaoPaginaIdiomaTipoFormularioDesativadoException
        /// </summary>
        public ConfiguracaoPaginaIdiomaTipoFormularioDesativadoException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "ConfiguracaoPaginaIdiomaTipoFormularioDesativadoException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
