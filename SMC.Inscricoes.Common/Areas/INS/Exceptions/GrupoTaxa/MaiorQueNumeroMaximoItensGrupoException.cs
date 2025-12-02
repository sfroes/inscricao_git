using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class MaiorQueNumeroMaximoItensGrupoException : SMCApplicationException
    {
        public MaiorQueNumeroMaximoItensGrupoException(string tipoTaxa, string hierarquiaCompletaOferta)
            : base(string.Format(
                SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
                "MaiorQueNumeroMaximoItensGrupoException", System.Threading.Thread.CurrentThread.CurrentCulture),
                tipoTaxa, hierarquiaCompletaOferta))
        { }

    }
}
