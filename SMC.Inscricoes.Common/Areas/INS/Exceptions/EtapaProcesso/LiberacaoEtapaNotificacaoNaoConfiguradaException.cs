using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class LiberacaoEtapaNotificacaoNaoConfiguradaException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de LiberacaoEtapaNotificacaoNaoConfiguradaException
        /// </summary>
        public LiberacaoEtapaNotificacaoNaoConfiguradaException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "LiberacaoEtapaNotificacaoNaoConfiguradaException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
