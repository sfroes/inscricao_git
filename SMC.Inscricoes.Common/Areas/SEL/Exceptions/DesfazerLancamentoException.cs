using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.SEL.Exceptions
{
    public class DesfazerLancamentoException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de DesfazerLancamentoException
        /// </summary>
        public DesfazerLancamentoException(bool singleItem)
            : base(Resources.ExceptionsResource.ResourceManager.GetString(
            singleItem ? "DesfazerLancamentoException" : "DesfazerLancamentosException", System.Threading.Thread.CurrentThread.CurrentCulture))
        { }
    }
}
