using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class SecaoArquivoNaoCadastradaIdiomasException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de EtapaProcessoEmManutencaoException
        /// </summary>
        public SecaoArquivoNaoCadastradaIdiomasException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "SecaoArquivoNaoCadastradaIdiomasException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
