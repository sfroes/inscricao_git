using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions.Processo
{
    public class InscricoesValidasFormularioPreenchidoException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de InscricoesValidasFormularioPreenchidoException
        /// </summary>
        public InscricoesValidasFormularioPreenchidoException()
            : base(Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "InscricoesValidasFormularioPreenchidoException", System.Threading.Thread.CurrentThread.CurrentCulture))
        { }
    }
}
