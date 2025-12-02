using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class LiberacaoEtapaDataLimiteEntregaMenorDataFimEtapaException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de LiberacaoEtapaDataLimiteEntregaMenorDataFimEtapaException
        /// </summary>
        public LiberacaoEtapaDataLimiteEntregaMenorDataFimEtapaException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "LiberacaoEtapaDataLimiteEntregaMenorDataFimEtapaException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
