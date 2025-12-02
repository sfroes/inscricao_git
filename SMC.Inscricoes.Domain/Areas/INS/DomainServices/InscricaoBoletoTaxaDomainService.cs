using SMC.Framework.Domain;
using SMC.Inscricoes.Domain.Areas.INS.Models;

namespace SMC.Inscricoes.Domain.Areas.INS.DomainServices
{
    public class InscricaoBoletoTaxaDomainService : InscricaoContextDomain<InscricaoBoletoTaxa>
    {
        #region DomainServices

        private InscricaoBoletoDomainService InscricaoBoletoDomainService 
        {
            get { return this.Create<InscricaoBoletoDomainService>(); }
        }   

        #endregion   
  
    }
     
}
