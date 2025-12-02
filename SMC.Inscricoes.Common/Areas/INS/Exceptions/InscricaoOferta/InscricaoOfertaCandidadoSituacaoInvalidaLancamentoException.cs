using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class InscricaoOfertaCandidadoSituacaoInvalidaLancamentoException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de InscricaoOfertaCandidadoSituacaoInvalidaLancamentoException
        /// </summary>
        public InscricaoOfertaCandidadoSituacaoInvalidaLancamentoException(string situacao)
            : base(string.Format(Resources.ExceptionsResource.ResourceManager.GetString(
                "InscricaoOfertaCandidadoSituacaoInvalidaLancamentoException", System.Threading.Thread.CurrentThread.CurrentCulture), situacao))
        { }
    }
}
