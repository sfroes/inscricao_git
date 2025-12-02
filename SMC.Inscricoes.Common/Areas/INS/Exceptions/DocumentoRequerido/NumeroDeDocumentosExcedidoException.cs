using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class NumeroDeDocumentosExcedidoException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de NumeroDeDocumentosExcedidoException
        /// </summary>
        public NumeroDeDocumentosExcedidoException(string descricaoTipo)
            : base(string.Format(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "NumeroDeDocumentosExcedidoException", System.Threading.Thread.CurrentThread.CurrentCulture), descricaoTipo))
        {

        }
    }
}
