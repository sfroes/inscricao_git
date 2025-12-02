using SMC.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common.Areas.INS.Exceptions
{
    public class LiberacaoEtapaSemIdiomasConfiguradosException : SMCApplicationException
    {
        /// <summary>
        /// Construtor de EtapaProcessoEmManutencaoException
        /// </summary>
        public LiberacaoEtapaSemIdiomasConfiguradosException()
            : base(SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString(
            "LiberacaoEtapaSemIdiomasConfiguradosException", System.Threading.Thread.CurrentThread.CurrentCulture))
        {

        }
    }
}
