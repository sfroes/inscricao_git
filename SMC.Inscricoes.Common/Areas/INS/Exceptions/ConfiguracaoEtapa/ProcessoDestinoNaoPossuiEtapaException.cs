using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class ProcessoDestinoNaoPossuiEtapaException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de ConfiguracaoEtapaInvalidaException
        /// </summary>
        public ProcessoDestinoNaoPossuiEtapaException(string processo, string etapa)
            : base(String.Format(
            SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "ProcessoDestinoNaoPossuiEtapaException", Thread.CurrentThread.CurrentCulture), processo, etapa))           
        {

        }
    }
}
