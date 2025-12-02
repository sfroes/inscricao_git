using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class DuplicacaoPaginaEtapaLiberadaException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de ConfiguracaoEtapaPaginaIdiomaException
        /// </summary>
        public DuplicacaoPaginaEtapaLiberadaException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "DuplicacaoPaginaEtapaLiberadaException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
