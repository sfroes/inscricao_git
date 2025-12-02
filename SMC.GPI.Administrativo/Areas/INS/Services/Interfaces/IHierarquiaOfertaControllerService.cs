using SMC.Framework.Model;
using SMC.Framework.UI.Mvc.Html;
using SMC.GPI.Administrativo.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMC.Framework.UI.Mvc;

namespace SMC.GPI.Administrativo.Areas.INS.Services
{
    public interface IHierarquiaOfertaControllerService
    {

        /// <summary>
        /// Busca a itens da árvore de hierarquia de oferta associada ao processo
        /// </summary>  
        List<ArvoreItemHierarquiaOfertaViewModel> BuscarItensArvoreHierarquiaOferta(long seqProcesso);

        HierarquiaOfertaViewModel BuscarInformaceosHierarquiaOferta(long seqProcesso);

        /// <summary>
        /// Busca uma oferta para edição
        /// </summary>        
        OfertaViewModel BuscarOfertaHierarquiaOferta(long seqOferta);

        /// <summary>
        /// Busca um item da hierarquia de oferta através de um processo
        /// </summary>        
        AssociarItemHierarquiaOfertaViewModel BuscarItemHierarquiaOferta(long seqItemHierarquiaOferta);


        // <summary>
        /// Salvar a associação de um item de hierarquia de oferta com o processo
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        long SalvarHierarquiaOferta(AssociarItemHierarquiaOfertaViewModel modelo);

        /// <summary>
        /// Salvar a oferta para um item de hierarquia de oferta
        /// </summary>        
        long SalvarOferta(OfertaViewModel modelo);

        /// <summary>
        /// Exclui uma oferta
        /// </summary>        
        void ExcluirOferta(long seqOferta);

        /// <summary>
        /// Exclui uma hierarquia de oferta (item da árvore)
        /// </summary>        
        void ExcluirHierarquiaOferta(long seqHierarquiaOferta);


        List<SMCDatasourceItem> BuscarEventosTaxaSelect(long seqProcesso);

        List<SMCDatasourceItem> BuscarDataVencimentoSelect(long seqProcesso);

        decimal? BuscarValorEvento(int seqEventoTaxa);

        SMCPagerModel<OfertaTaxaViewModel> BuscarTaxasOferta(OfertaTaxaFiltroViewModel filtro);

        List<SMCDatasourceItem<string>> BuscarPeriodosOfertas(long seqProcesso);

        SMCPagerModel<SMCDatasourceItem> BuscarOfertasPeriodoTaxaParaInclusao(CadastroOfertaTaxaFiltroViewModel filtro);

        SMCPagerModel<SMCDatasourceItem> BuscarOfertasPeriodoTaxaParaExclusao(CadastroOfertaTaxaFiltroViewModel filtro);

        void ExcluirTaxaOfertaEmLote(long seqTipoTaxa, List<long> seqOfertas);

        List<SMCDatasourceItem> BuscarOfertasKeyValue(List<long> seqOfertas);

        void IncluirTaxasLote(IncluirTaxaOfertaViewModel modelo);

        /// <summary>
        /// Verifica se a inclusão de itens de hierarquia de oferta é permitida
        /// </summary>        
        void VerificarPermissaoCadastrarHierarquia(long seqProcesso);

        string BuscarDescricaoTaxa(long seqTaxa);

        OfertaViewModel BuscarUltimaOfertaCadastrada(long seqProcesso);

        bool VerificaTipoTaxaCobraPorOferta(long seqTipoTaxa);
    }
}
