using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class AlteracaoGrupoOfertaEtapaLiberadaException : SMCApplicationException
    {
        public AlteracaoGrupoOfertaEtapaLiberadaException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "AlteracaoGrupoOfertaEtapaLiberadaException", System.Threading.Thread.CurrentThread.CurrentCulture))
        { }
    }
}
