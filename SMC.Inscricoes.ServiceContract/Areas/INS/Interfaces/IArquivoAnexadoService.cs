using SMC.DadosMestres.ServiceContract.Areas.SHA.Data;
using SMC.Framework.Model;
using SMC.Framework.Service;
using SMC.Inscricoes.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces
{
    [ServiceContract(Namespace = NAMESPACES.SERVICE)]
    public interface IArquivoAnexadoService : ISMCService
    {
        /// <summary>
        /// Busca um arquivo anexado por seu sequencial.
        /// </summary>
        /// <param name="seq">O sequencial do arquivo.</param>
        /// <returns></returns>
        SMCUploadFileGED BuscarArquivoAnexado(long seq);
    }
}
