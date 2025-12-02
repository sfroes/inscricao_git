using SMC.Framework.Extensions;
using SMC.Framework.Model;
using SMC.Framework.Service;
using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.INS.DomainServices;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using SMC.Inscricoes.Domain.Areas.INS.ValueObjects;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Service.Areas.INS.Services
{
    public class HierarquiaOfertaService : SMCServiceBase, IHierarquiaOfertaService
    {
        #region Domain Services
        private HierarquiaOfertaDomainService HierarquiaOfertaDomainService
        {
            get { return Create<HierarquiaOfertaDomainService>(); }
        }
        #endregion

        public List<SMCDatasourceItem> BuscarHierarquiaOfertasPorTipoKeyValue(long seqProcesso, long seqTipoItem)
        {
            return HierarquiaOfertaDomainService.SearchProjectionBySpecification(new HierarquiaOfertaTipoItemFilterSpecification() { SeqProcesso = seqProcesso, SeqTipoItem = seqTipoItem },
                                                        x => new SMCDatasourceItem
                                                        {
                                                            Seq = x.Seq,
                                                            Descricao = x.Nome
                                                        }).ToList();
        }

        public SMCPagerData<LookupHierarquiaOfertaData> LookupBuscarHierarquiaOfertas(LookupHierarquiaOfertaFiltroData filtro)
        {
            int total;
            var lista = HierarquiaOfertaDomainService.SearchProjectionBySpecification(filtro.Transform<HierarquiaOfertaTipoItemFilterSpecification>(),
                                                    x => new LookupHierarquiaOfertaData
                                                    {
                                                        Seq = x.Seq,
                                                        HierarquiaOferta = x.DescricaoCompleta,
                                                        TipoItemHierarquiaOferta = x.ItemHierarquiaOferta.TipoItemHierarquiaOferta.Descricao
                                                    }, out total).ToList();

            return new SMCPagerData<LookupHierarquiaOfertaData>(lista, total);
        }

        public LookupHierarquiaOfertaData LookupBuscarHierarquiaOferta(long seq)
        {
            return HierarquiaOfertaDomainService.SearchProjectionByKey(new SMCSeqSpecification<HierarquiaOferta>(seq),
                                                    x => new LookupHierarquiaOfertaData
                                                    {
                                                        Seq = x.Seq,
                                                        HierarquiaOferta = x.Nome,
                                                        TipoItemHierarquiaOferta = x.ItemHierarquiaOferta.TipoItemHierarquiaOferta.Descricao
                                                    });
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        public void ExcluirHierarquiaOferta(long seqHierarquiaOferta)
        {
            HierarquiaOfertaDomainService.ExcluirHierarquiaOferta(seqHierarquiaOferta);
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        public Dictionary<long,long?> AdicionarItemHierarquiaOferta (long seqProcesso, List<ItemOfertaHierarquiaOfertaData> itensOfertasHierarquiasOfertas)
        {
            return HierarquiaOfertaDomainService.AdicionarItemHierarquiaOferta(seqProcesso, itensOfertasHierarquiasOfertas.TransformList<ItemOfertaHierarquiaOfertaVO>());
        }
        
    }
}
