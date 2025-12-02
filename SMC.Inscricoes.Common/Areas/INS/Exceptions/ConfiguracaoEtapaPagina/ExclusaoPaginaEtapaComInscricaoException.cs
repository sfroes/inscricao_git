using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class ExclusaoPaginaEtapaComInscricaoException : SMCApplicationException
    {
        public ExclusaoPaginaEtapaComInscricaoException()
            : base(Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "ExclusaoPaginaEtapaComInscricaoException", System.Threading.Thread.CurrentThread.CurrentCulture))
        { }
    }
}
