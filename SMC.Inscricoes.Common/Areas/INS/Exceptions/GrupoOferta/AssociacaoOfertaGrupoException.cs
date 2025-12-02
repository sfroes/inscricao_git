using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class AssociacaoOfertaGrupoException : SMCApplicationException
    {
        public AssociacaoOfertaGrupoException(string oferta)
            : base(string.Format(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
                "AssociacaoOfertaGrupoException", System.Threading.Thread.CurrentThread.CurrentCulture)
            , oferta))
        { }
    }
}
