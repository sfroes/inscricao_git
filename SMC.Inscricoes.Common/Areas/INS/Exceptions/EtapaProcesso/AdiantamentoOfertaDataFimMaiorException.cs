using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class AdiantamentoOfertaDataFimMaiorException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de AdiantamentoOfertaDataFimMaiorException
        /// </summary>
        public AdiantamentoOfertaDataFimMaiorException(string descricao)
            : base(String.Format(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "AdiantamentoOfertaDataFimMaiorException", System.Threading.Thread.CurrentThread.CurrentCulture),descricao))
        {

        }
    }
}
