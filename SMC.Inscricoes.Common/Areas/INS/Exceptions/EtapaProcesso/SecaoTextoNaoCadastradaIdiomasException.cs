using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class SecaoTextoNaoCadastradaIdiomasException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de EtapaProcessoEmManutencaoException
        /// </summary>
        public SecaoTextoNaoCadastradaIdiomasException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "SecaoTextoNaoCadastradaIdiomasException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
