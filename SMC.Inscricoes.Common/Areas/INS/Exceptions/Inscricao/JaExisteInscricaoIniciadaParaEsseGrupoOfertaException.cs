using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class JaExisteInscricaoIniciadaParaEsseGrupoOfertaException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de JaExisteInscricaoIniciadaParaEsseGrupoOfertaException
        /// </summary>
        public JaExisteInscricaoIniciadaParaEsseGrupoOfertaException(string resource, Exception innerException)
            : base(string.Format(
            SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            $"JaExisteInscricaoIniciadaParaEsseGrupoOferta{resource}Exception", System.Threading.Thread.CurrentThread.CurrentCulture)), innerException)
        {

        }
    }
}
