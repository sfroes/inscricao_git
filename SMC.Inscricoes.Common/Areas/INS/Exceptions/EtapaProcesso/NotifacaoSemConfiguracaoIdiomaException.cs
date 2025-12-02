using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class NotifacaoSemConfiguracaoIdiomaException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de NotifacaoSemConfiguracaoIdiomaException
        /// </summary>
        public NotifacaoSemConfiguracaoIdiomaException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "NotifacaoSemConfiguracaoIdiomaException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
