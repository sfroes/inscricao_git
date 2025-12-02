using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class DocumentoRequeridoExibirTermoResponsabilidadeEntregaException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de AlteracaoDocumentoRequeridoVersaoException
        /// </summary>
        public DocumentoRequeridoExibirTermoResponsabilidadeEntregaException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "DocumentoRequeridoExibirTermoResponsabilidadeEntregaException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}