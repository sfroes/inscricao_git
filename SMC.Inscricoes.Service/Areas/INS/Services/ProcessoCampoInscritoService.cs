using SMC.Framework.Extensions;
using SMC.Framework.Service;
using SMC.Inscricoes.Domain.Areas.INS.DomainServices;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using System;
using System.Collections.Generic;

namespace SMC.Inscricoes.Service.Areas.INS.Services
{
    public class ProcessoCampoInscritoService : SMCServiceBase, IProcessoCampoInscritoService
    {
        #region DomainService

        private ProcessoCampoInscritoDomainService ProcessoCampoInscritoDomainService => Create<ProcessoCampoInscritoDomainService>();

        #endregion DomainService

        /// Buscar campos inscritos por processo.
        /// </summary>
        /// <param name="seqProcesso">O sequencial do processo.</param>
        /// <returns>Uma lista de campos inscritos.</returns>
        public List<ProcessoCampoInscritoData> BuscarCamposIncritosPorProcesso(long seqProcesso)
        {
            return ProcessoCampoInscritoDomainService.BuscarCamposIncritosPorProcesso(seqProcesso).TransformList<ProcessoCampoInscritoData>();
        }

        /// <summary>
        /// Buscar campos inscritos por UIID do processo.
        /// </summary>
        /// <param name="guid">O UIID do processo.</param>
        /// <returns>Uma lista de campos inscritos.</returns>
        public List<ProcessoCampoInscritoData> BuscarCamposInscritosPorUIIDProcesso(Guid guid)
        {
            return ProcessoCampoInscritoDomainService.BuscarCamposInscritosPorUIIDProcesso(guid).TransformList<ProcessoCampoInscritoData>();
        }
    }
}