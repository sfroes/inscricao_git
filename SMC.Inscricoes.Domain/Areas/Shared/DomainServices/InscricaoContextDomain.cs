using SMC.Framework.Domain;
using SMC.Inscricoes.Common;

namespace SMC.Inscricoes.Domain
{
    public class InscricaoContextDomain<TEntity> : SMCDomainServiceBase<TEntity> where TEntity : class
    {
        public InscricaoContextDomain() : base(CONSTANTS.INSCRICAO_CONTEXT)
        {

        }
    }
}