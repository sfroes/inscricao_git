using SMC.Framework.Extensions;
using SMC.Framework.Service;
using SMC.Inscricoes.Domain.Areas.INS.DomainServices;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Service.Areas.INS.Services
{
    public class InscritoAtividadeRelatorioService : SMCServiceBase, IInscritoAtividadeRelatorioService
    {

        #region DomainService

        private InscritoAtividadeRelatorioDomainService InscritoAtividadeRelatorioDomainService => Create<InscritoAtividadeRelatorioDomainService>();

        #endregion

        public List<InscritoAtividadeRelatorioListaData> BuscarInscritosAtividades(InscritoAtividadeRelatorioFiltroData filtro)
        {
            var specFilter = filtro.Transform<InscritoAtividadeRelatorioFilterSpecification>();
            var inscritos = InscritoAtividadeRelatorioDomainService.BuscarInscritosAtividades(specFilter);
            return inscritos.TransformList<InscritoAtividadeRelatorioListaData>();           

        }
    }
}
