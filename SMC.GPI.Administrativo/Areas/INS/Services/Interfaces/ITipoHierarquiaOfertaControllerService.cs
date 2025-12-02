using SMC.Framework.Model;
using SMC.GPI.Administrativo.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.GPI.Administrativo.Areas.INS.Services
{
    public interface ITipoHierarquiaOfertaControllerService
    {
        /// <summary>
        /// Buscar os Tipos de Hierarquia de Oferta
        /// </summary>
        /// <returns>Lista de Tipos de Hierarquia de Oferta</returns>
        SMCPagerModel<TipoHierarquiaOfertaListaViewModel> BuscarTiposHierarquiaOferta(TipoHierarquiaOfertaFiltroViewModel filtros);
        
        /// <summary>
        /// Buscar o Tipo de Hierarquia de Oferta desejado
        /// </summary>
        /// <param name="seqTipoHierarquiaOferta"></param>
        /// <returns></returns>
        TipoHierarquiaOfertaViewModel BuscarTipoHierarquiaOferta(long seqTipoHierarquiaOferta);

        /// <summary>
        /// Buscar os itens da arvore de tipo de hierarquia de oferta
        /// </summary>
        /// <returns>Lista de itens da árvore</returns>
        List<NoArvoreTipoHierarquiaOfertaViewModel> BuscarItensArvoreTipoHierarquiaOferta(long seqTipoHierarquiaOferta);

        /// <summary>
        /// Salva um Tipo de Hierarquia de Oferta
        /// </summary>
        /// <param name="modelo">Tipo de Hierarquia de Oferta a ser persistida</param>
        /// <returns>Sequencial gerado</returns>
        long SalvarTipoHierarquiaOferta(TipoHierarquiaOfertaViewModel modelo);

        /// <summary>
        /// Exclui um Tipo de Hierarquia de Oferta
        /// </summary>
        void ExcluirTipoHierarquiaOferta(long seqTipoHierarquiaOferta);
        
        /// <summary>
        /// Salva a associação de um tipo de hierarquia de oferta na hierarquia
        /// </summary>
        /// <param name="modelo">Tipo de Hierarquia a ser associado</param>
        void SalvarAssociacaoTipoHierarquiaOferta(ItemHierarquiaOfertaViewModel modelo);

        /// <summary>
        /// Exclui um item da hierarquia e todos os seus filhos
        /// </summary>        
        void ExcluirAssociaoTipoHierarquiaOferta(long seqItemHierarquiaOferta);

        /// <summary>
        /// Busca um item da hierarquia para edição ou exibição
        /// </summary>        
        ItemHierarquiaOfertaViewModel BuscarItemHierarquiaOferta(long seqItemHieraraquiOferta);

        /// <summary>
        /// Buscar tipos hierarquia de oferta para select
        /// </summary>        
        List<SMCDatasourceItem> BuscarTiposHierarquiaOfertaSelect();
        
    }
}
