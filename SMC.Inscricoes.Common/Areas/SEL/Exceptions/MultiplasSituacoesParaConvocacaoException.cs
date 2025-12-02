using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.SEL.Exceptions
{
    public class MultiplasSituacoesParaConvocacaoException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de MultiplasSituacoesParaConvocacaoException
        /// </summary>
        public MultiplasSituacoesParaConvocacaoException()
            : base(Resources.ExceptionsResource.ResourceManager.GetString(
            "MultiplasSituacoesParaConvocacaoException", System.Threading.Thread.CurrentThread.CurrentCulture))
        { }
    }
}
