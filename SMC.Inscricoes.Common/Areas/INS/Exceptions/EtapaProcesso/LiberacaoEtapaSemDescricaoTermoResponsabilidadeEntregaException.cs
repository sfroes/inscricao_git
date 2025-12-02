using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class LiberacaoEtapaSemDescricaoTermoResponsabilidadeEntregaException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de LiberacaoEtapaSemDescricaoTermoResponsabilidadeEntregaException
        /// </summary>
        public LiberacaoEtapaSemDescricaoTermoResponsabilidadeEntregaException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "LiberacaoEtapaSemDescricaoTermoResponsabilidadeEntregaException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
