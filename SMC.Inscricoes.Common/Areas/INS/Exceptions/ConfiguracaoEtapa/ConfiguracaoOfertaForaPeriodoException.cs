using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class ConfiguracaoOfertaForaPeriodoException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de ConfiguracaoEtapaInvalidaException
        /// </summary>
        public ConfiguracaoOfertaForaPeriodoException(string nomeGrupo)
            : base(String.Format(
            SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "ConfiguracaoOfertaForaPeriodoException", Thread.CurrentThread.CurrentCulture),nomeGrupo))           
        {

        }
    }
}
