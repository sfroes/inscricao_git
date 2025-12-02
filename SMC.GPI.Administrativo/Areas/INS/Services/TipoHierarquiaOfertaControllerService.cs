using SMC.Framework.Extensions;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc.Controllers.Service;
using SMC.GPI.Administrativo.Areas.INS.Models;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using System.Collections.Generic;

namespace SMC.GPI.Administrativo.Areas.INS.Services
{
    public class TipoHierarquiaOfertaControllerService : SMCControllerServiceBase
    {

        #region 

        private ITipoHierarquiaOfertaService TipoHierarquiaOfertaService 
        {
            get { return this.Create<ITipoHierarquiaOfertaService>(); }
        }

        #endregion

        /// <summary>
        /// Buscar os Tipos de Hierarquia de Oferta
        /// </summary>
        /// <returns>Lista de Tipos de Hierarquia de Oferta</returns>
        public SMCPagerModel<TipoHierarquiaOfertaListaViewModel> BuscarTiposHierarquiaOferta(TipoHierarquiaOfertaFiltroViewModel filtros) 
        {
            var datas = this.TipoHierarquiaOfertaService.BuscarTiposHierarquiaOferta(SMCMapperHelper.Create<TipoHierarquiaOfertaFiltroData>(filtros));
            SMCPagerData<TipoHierarquiaOfertaListaViewModel> model = SMCMapperHelper.Create<SMCPagerData<TipoHierarquiaOfertaListaViewModel>>(datas);
            return new SMCPagerModel<TipoHierarquiaOfertaListaViewModel>(model, filtros.PageSettings, filtros);
        }
        
        /// <summary>
        /// Buscar o Tipo de Hierarquia de Oferta desejado
        /// </summary>
        /// <param name="seqTipoHierarquiaOferta"></param>
        /// <returns></returns>
        public TipoHierarquiaOfertaViewModel BuscarTipoHierarquiaOferta(long seqTipoHierarquiaOferta)
        {
            return SMCMapperHelper.Create<TipoHierarquiaOfertaViewModel>(
                this.TipoHierarquiaOfertaService.BuscarTipoHierarquiaOferta(seqTipoHierarquiaOferta));
        }

        /// <summary>
        /// Buscar os itens da arvore de tipo de hierarquia de oferta
        /// </summary>
        /// <returns>Lista de itens da árvore</returns>
        public List<NoArvoreTipoHierarquiaOfertaViewModel> BuscarItensArvoreTipoHierarquiaOferta(long SeqTipoHierarquiaOferta)
        {
            return this.TipoHierarquiaOfertaService.BuscarItemsHieraquiaOferta(SeqTipoHierarquiaOferta)
                .TransformList<NoArvoreTipoHierarquiaOfertaViewModel>();
        }

        /// <summary>
        /// Salva um Tipo de Hierarquia de Oferta
        /// </summary>
        /// <param name="modelo">Tipo de Hierarquia de Oferta a ser persistida</param>
        /// <returns>Sequencial gerado</returns>
        public long SalvarTipoHierarquiaOferta(TipoHierarquiaOfertaViewModel modelo)
        {
            return this.TipoHierarquiaOfertaService.SalvarTipoHierarquiaOferta(
                SMCMapperHelper.Create<TipoHierarquiaOfertaData>(modelo));
        }

        /// <summary>
        /// Exclui um Tipo de Hierarquia de Oferta
        /// </summary>
        public void ExcluirTipoHierarquiaOferta(long seqTipoHierarquiaOferta)
        {
            this.TipoHierarquiaOfertaService.ExcluirTipoHieraquiaOferta(seqTipoHierarquiaOferta);
        }
        
        /// <summary>
        /// Salva a associação de um tipo de hierarquia de oferta na hierarquia
        /// </summary>
        /// <param name="modelo">Tipo de Hierarquia a ser associado</param>
        public void SalvarAssociacaoTipoHierarquiaOferta(ItemHierarquiaOfertaViewModel modelo)
        {
            this.TipoHierarquiaOfertaService.SalvarItemHierarquiaOferta(
                SMCMapperHelper.Create<ItemHierarquiaOfertaData>(modelo));
        }

        public void ExcluirAssociaoTipoHierarquiaOferta(long seqItemHierarquiaOferta) 
        {
            this.TipoHierarquiaOfertaService.ExcluirItemHierarquiaOferta(seqItemHierarquiaOferta);
        }


        /// <summary>
        /// Busca um item da hierarquia para edição ou exibição
        /// </summary>        
        public ItemHierarquiaOfertaViewModel BuscarItemHierarquiaOferta(long seqItemHieraraquiOferta) 
        {
            return SMCMapperHelper.Create<ItemHierarquiaOfertaViewModel>(
            this.TipoHierarquiaOfertaService.BuscarItemHieraquiaOferta(seqItemHieraraquiOferta));
        }

        /// <summary>
        /// Buscar tipos hierarquia de oferta para select
        /// </summary>        
        public List<SMCDatasourceItem> BuscarTiposHierarquiaOfertaSelect() 
        {
            return this.TipoHierarquiaOfertaService.BuscarTiposHierarquiaOfertaKeyValue()
                .TransformList<SMCDatasourceItem>();
        }
        
        
    }
}
