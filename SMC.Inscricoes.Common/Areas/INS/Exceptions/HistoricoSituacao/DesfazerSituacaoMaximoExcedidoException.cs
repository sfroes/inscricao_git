using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class DesfazerSituacaoMaximoExcedidoException: SMCApplicationException
    {
        /// <summary>
        /// Construtor de InscricaoComOfertaInvalidaException
        /// </summary>
        public DesfazerSituacaoMaximoExcedidoException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "DesfazerSituacaoMaximoExcedidoException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
