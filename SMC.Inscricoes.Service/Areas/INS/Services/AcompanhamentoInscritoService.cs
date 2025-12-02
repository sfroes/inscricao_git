using SMC.Framework.Extensions;
using SMC.Framework.Model;
using SMC.Framework.Service;
using SMC.Inscricoes.Domain.Areas.INS.DomainServices;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;

namespace SMC.Inscricoes.Service.Areas.INS.Services
{
    public class AcompanhamentoInscritoService : SMCServiceBase, IAcompanhamentoInscritoService
    {
        #region injeção de dependencia 

        private InscricaoDomainService InscricaoDomainService
        {
            get { return this.Create<InscricaoDomainService>(); }
        }
        private InscritoDomainService InscritoDomainService
        {
            get { return this.Create<InscritoDomainService>(); }
        }

        #endregion

        public SMCPagerData<AcompanhamentoInscritoListaData> BuscarInscrito(AcompanhamentoInscritoFiltroData filtro)
        {
            var spec = filtro.Transform<InscricaoFilterSpecification>();
            var Inscricao = InscricaoDomainService.BuscarInscrito(spec, out int total);

            return new SMCPagerData<AcompanhamentoInscritoListaData>(
             Inscricao.TransformList<AcompanhamentoInscritoListaData>(), total);
        }

    }
}
