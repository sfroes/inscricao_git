using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class AdicionarPaginaEtapaLiberadaException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de AdicionarPaginaEtapaLiberadaException
        /// </summary>
        public AdicionarPaginaEtapaLiberadaException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "AdicionarPaginaEtapaLiberadaException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
