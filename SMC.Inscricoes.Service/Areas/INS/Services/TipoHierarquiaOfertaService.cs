using SMC.Framework.Extensions;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.Service;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Inscricoes.Domain.Areas.INS;
using SMC.Inscricoes.Domain.Areas.INS.DomainServices;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using System.Collections.Generic;
using System.Linq;

namespace SMC.Inscricoes.Service.Areas.INS.Services
{
    public class TipoHierarquiaOfertaService : SMCServiceBase, ITipoHierarquiaOfertaService
    {
        #region DomainService
        
        private TipoHierarquiaOfertaDomainService TipoHierarquiaOfertaDomainService
        {
            get { return this.Create<TipoHierarquiaOfertaDomainService>(); }
        }

        private ItemHierarquiaOfertaDomainService ItemHierarquiaOfertaDomainService
        {
            get { return this.Create<ItemHierarquiaOfertaDomainService>(); }
        }

        #endregion

        #region Tipo Hierarquia Oferta

        public List<SMCDatasourceItem> BuscarTiposHierarquiaOfertaKeyValue()
        {
            return TipoHierarquiaOfertaDomainService.SearchProjectionAll(
                        x => new SMCDatasourceItem
                        {
                            Seq = x.Seq,
                            Descricao = x.Descricao
                        }, o => o.Descricao).ToList();
        }

        public SMCPagerData<TipoHierarquiaOfertaListaData> BuscarTiposHierarquiaOferta(TipoHierarquiaOfertaFiltroData filtro) 
        {
            var spec = SMCMapperHelper.Create<TipoHierarquiaOfertaFilterSpecification>(filtro);
            int total = 0;
            var itens = this.TipoHierarquiaOfertaDomainService.SearchProjectionBySpecification(spec,
                x => new TipoHierarquiaOfertaListaData
                {
                    Seq = x.Seq,
                    Descricao = x.Descricao
                },out total);
            return new SMCPagerData<TipoHierarquiaOfertaListaData>(itens, total);
        }

        public TipoHierarquiaOfertaData BuscarTipoHierarquiaOferta(long seqTipoHierarquiaOferta)
        {
            return this.TipoHierarquiaOfertaDomainService
                .SearchByKey<TipoHierarquiaOferta, TipoHierarquiaOfertaData>(seqTipoHierarquiaOferta);
        }

        public long SalvarTipoHierarquiaOferta(TipoHierarquiaOfertaData tipoHierarquiaOferta)
        {            
            return this.TipoHierarquiaOfertaDomainService.SaveEntity<TipoHierarquiaOferta>(tipoHierarquiaOferta);
        }

        public void ExcluirTipoHieraquiaOferta(long seqTipoHierarquiaOferta)
        {
            this.TipoHierarquiaOfertaDomainService.DeleteEntity(seqTipoHierarquiaOferta);
        }

        #endregion

        #region Item Hierarquia Oferta


        public List<ItemHierarquiaOfertaData> BuscarItemsHieraquiaOferta(long seqTipoHierarquiaOferta)
        {
            return this.ItemHierarquiaOfertaDomainService.SearchProjectionBySpecification(new ItemHierarquiaOfertaFilterSpecification
            {
                SeqTipoHierarquiaOferta = seqTipoHierarquiaOferta
            }, x => new ItemHierarquiaOfertaData
            {
                Seq = x.Seq,
                SeqPai = x.SeqPai,
                HabilitaCadastroOferta = x.HabilitaCadastroOferta,
                SeqTipoHierarquiaOferta = x.SeqTipoHierarquiaOferta,
                SeqTipoItemHierarquiaOferta = x.SeqTipoItemHierarquiaOferta,
                Descricao = x.TipoItemHierarquiaOferta.Descricao
            }).ToList();
            
        }

        public ItemHierarquiaOfertaData BuscarItemHieraquiaOferta(long seqItemHierarquiaOferta)
        {
            return this.ItemHierarquiaOfertaDomainService
                .SearchByKey<ItemHierarquiaOferta, ItemHierarquiaOfertaData>(
                    seqItemHierarquiaOferta, IncludesItemHierarquiaOferta.ItemHierarquiaOfertaPai|
                    IncludesItemHierarquiaOferta.ItemHierarquiaOfertaPai_TipoItemHierarquiaOferta);

        }

         public long SalvarItemHierarquiaOferta(ItemHierarquiaOfertaData item)
         {
             return ItemHierarquiaOfertaDomainService.SalvarItemHierarquiaOferta(
                 SMCMapperHelper.Create<ItemHierarquiaOferta>(item));
         }

         public void ExcluirItemHierarquiaOferta(long seqItemHierarquiaOferta) 
         {
             ItemHierarquiaOfertaDomainService.ExcluirItemHierarquiaOferta(seqItemHierarquiaOferta);
         }


        #endregion
    }
}
