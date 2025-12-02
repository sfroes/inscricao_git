using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class OfertaPeriodoTaxaCobradaPorOfertaException : SMCApplicationException
    {

        /// <summary>
        /// Construtor de OfertaPeriodoTaxaCobradaPorOfertaException
        /// </summary>
        /// <param name="tipoTaxa">Descrição do Tipo de taxa.</param>
        /// <param name="tipoCobranca">Descrição do Tipo de cobrança da Taxa em caixa baixa</param>
        public OfertaPeriodoTaxaCobradaPorOfertaException(string tipoTaxa, string tipoCobranca)
            : base(string.Format(
                Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
                "OfertaPeriodoTaxaCobradaPorOfertaException", System.Threading.Thread.CurrentThread.CurrentCulture), tipoTaxa, tipoCobranca))
        { }
    }
}
