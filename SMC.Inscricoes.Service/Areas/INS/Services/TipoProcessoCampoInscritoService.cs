using SMC.Formularios.ServiceContract.Areas.TMP.Interfaces;
using SMC.Framework.Extensions;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.Service;
using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.INS.DomainServices;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace SMC.Inscricoes.Service.Areas.INS.Services
{
    public class TipoProcessoCampoInscritoService : SMCServiceBase, ITipoProcessoCampoInscritoService
    {
        #region DomainService

        private TipoProcessoCampoInscritoDomainService TipoProcessoCampoInscritoDomainService => Create<TipoProcessoCampoInscritoDomainService>();

        #endregion DomainService

        /// <summary>
        /// Busca os tipos de processo campos inscrito por tipo de processo.
        /// </summary>
        /// <param name="seqTipoProcesso">O sequencial do tipo de processo.</param>
        /// <returns>Uma lista de tipos de processo campos inscrito.</returns>
        public List<TipoProcessoCampoInscritoData> BuscarTiposProcessoCamposInscritoPorTipoProcesso(long seqTipoProcesso)
        {
            return TipoProcessoCampoInscritoDomainService.BuscarTiposProcessoCamposInscritoPorTipoProcesso(seqTipoProcesso)
                                                         .TransformList<TipoProcessoCampoInscritoData>();
        }

       
    }
}