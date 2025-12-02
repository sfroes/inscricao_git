using SMC.Framework.Service;
using SMC.Inscricoes.Domain.Areas.INS.DomainServices;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using System.Collections.Generic;
using SMC.Framework.Extensions;
using System.ServiceModel;

namespace SMC.Inscricoes.Service.Areas.INS.Services
{
    public class ItemHierarquiaOfertaService : SMCServiceBase, IItemHierarquiaOfertaService
    {
        #region Domain Services

        private ItemHierarquiaOfertaDomainService ItemHierarquiaOfertaDomainService
        {
            get { return Create<ItemHierarquiaOfertaDomainService>(); }
        }

        #endregion

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        public List<ItemHierarquiaOfertaArvoreData> BuscarItensHierarquiaOfertaArvore(long seqProcesso)
        {
            return ItemHierarquiaOfertaDomainService.BuscarItensHierarquiaOfertaArvore(seqProcesso).TransformList<ItemHierarquiaOfertaArvoreData>();
        }
    }
}
