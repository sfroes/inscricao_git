using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class OfertaCoincideComPermissaoForaPrazoException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de OfertaVencimentoInvalidoException
        /// </summary>
        public OfertaCoincideComPermissaoForaPrazoException()
            : base(
            SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
                "OfertaCoincideComPermissaoForaPrazoException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {
        }
    }
}
