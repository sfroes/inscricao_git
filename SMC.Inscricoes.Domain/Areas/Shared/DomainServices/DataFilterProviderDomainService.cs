using SMC.Framework.Domain.DataFilters;
using SMC.Inscricoes.Common;

namespace SMC.Inscricoes.Domain.DomainService
{
    public class DataFilterProviderDomainService : SMCDataFilterProviderDomainService
    {
        public DataFilterProviderDomainService() : base(CONSTANTS.INSCRICAO_CONTEXT) { }
    }
}
