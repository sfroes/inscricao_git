using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class NumeroMinimoTaxasExcedidoException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de InscricaoComOfertaInvalidaException
        /// </summary>
        public NumeroMinimoTaxasExcedidoException(string descricaoTaxa, int numero)
            : base(String.Format(
            SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "NumeroMinimoTaxasExcedidoException", System.Threading.Thread.CurrentThread.CurrentCulture), descricaoTaxa, numero))
        {

        }
    }
}
