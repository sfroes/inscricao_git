using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.SEL.Exceptions
{
    public class LancamentoNotaClassificacaoDuplicadaException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de InscricaoJaExistenteException
        /// </summary>
        public LancamentoNotaClassificacaoDuplicadaException(int NumeroClassificacaoDuplicado)
            : base(
            String.Format(
            Resources.ExceptionsResource.ResourceManager.GetString(
            "LancamentoNotaClassificacaoDuplicadaException", System.Threading.Thread.CurrentThread.CurrentCulture),
            NumeroClassificacaoDuplicado.ToString()))
        {

        }
    }
}
