using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class RemocaoGrupoComOfertaException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de RemocaoGrupoComOfertaException
        /// </summary>
        public RemocaoGrupoComOfertaException(string nomeGrupo)
            : base(String.Format(
            SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "RemocaoGrupoComOfertaException", Thread.CurrentThread.CurrentCulture),nomeGrupo))            
        {

        }
    }
}
