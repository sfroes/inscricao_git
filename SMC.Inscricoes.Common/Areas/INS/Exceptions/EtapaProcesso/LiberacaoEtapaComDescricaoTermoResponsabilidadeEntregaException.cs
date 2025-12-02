using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class LiberacaoEtapaComDescricaoTermoResponsabilidadeEntregaException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de LiberacaoEtapaComDescricaoTermoResponsabilidadeEntregaException
        /// </summary>
        public LiberacaoEtapaComDescricaoTermoResponsabilidadeEntregaException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "LiberacaoEtapaComDescricaoTermoResponsabilidadeEntregaException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
