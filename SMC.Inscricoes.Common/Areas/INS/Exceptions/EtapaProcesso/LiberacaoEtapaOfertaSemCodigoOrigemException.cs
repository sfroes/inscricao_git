using SMC.Framework.Exceptions;
using SMC.Framework.Util;
using SMC.Inscricoes.Common.Areas.INS.Resources;
using System.Collections.Generic;
using System.Threading;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class LiberacaoEtapaOfertaSemCodigoOrigemException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de LiberacaoEtapaCodigoSemPaginaException
        /// </summary>
        public LiberacaoEtapaOfertaSemCodigoOrigemException(List<string> ofertasSemCodigo)
            : base(string.Format(ExceptionsResource.ResourceManager.GetString("LiberacaoEtapaOfertaSemCodigoOrigemException", Thread.CurrentThread.CurrentCulture), SMCStringHelper.JoinWithLastSeparatorIgnoringNullOrEmpty(", ", " e ", ofertasSemCodigo)))
        {
        }
    }
}