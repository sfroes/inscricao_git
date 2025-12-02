using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.SEL.Exceptions
{
    public class SituacaoNaoPermiteDesfazerLancamentoException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de SituacaoNaoPermiteDesfazerLancamentoException
        /// </summary>
        public SituacaoNaoPermiteDesfazerLancamentoException(string situacao)
            : base(string.Format(Resources.ExceptionsResource.ResourceManager.GetString(
                "SituacaoNaoPermiteDesfazerLancamentoException", System.Threading.Thread.CurrentThread.CurrentCulture), situacao))
        { }
    }
}
