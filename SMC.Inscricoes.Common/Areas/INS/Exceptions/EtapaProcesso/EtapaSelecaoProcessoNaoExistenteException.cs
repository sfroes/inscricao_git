using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class EtapaSelecaoProcessoNaoExistenteException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de EtapaProcessoNaoExistenteException
        /// </summary>
        public EtapaSelecaoProcessoNaoExistenteException()
            : base(Resources.ExceptionsResource.ResourceManager.GetString(
            "EtapaSelecaoProcessoNaoExistenteException", System.Threading.Thread.CurrentThread.CurrentCulture))
        { }
    }
}
