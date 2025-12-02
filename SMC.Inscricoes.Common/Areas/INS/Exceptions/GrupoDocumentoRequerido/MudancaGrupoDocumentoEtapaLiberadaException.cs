using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class MudancaGrupoDocumentoEtapaLiberadaException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de MudancaGrupoDocumentoEtapaLiberadaException
        /// </summary>
        public MudancaGrupoDocumentoEtapaLiberadaException(string operacao)
            : base(String.Format(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "MudancaGrupoDocumentoEtapaLiberadaException", System.Threading.Thread.CurrentThread.CurrentCulture),operacao))
        {

        }
    }
}
