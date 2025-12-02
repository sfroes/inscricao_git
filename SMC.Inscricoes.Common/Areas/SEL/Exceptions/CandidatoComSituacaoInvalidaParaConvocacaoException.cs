using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.SEL.Exceptions
{
    public class CandidatoComSituacaoInvalidaParaConvocacaoException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de CandidatoComSituacaoInvalidaParaConvocacaoException
        /// </summary>
        public CandidatoComSituacaoInvalidaParaConvocacaoException()
            : base(Resources.ExceptionsResource.ResourceManager.GetString(
            "CandidatoComSituacaoInvalidaParaConvocacaoException", System.Threading.Thread.CurrentThread.CurrentCulture))
        { }
    }
}
