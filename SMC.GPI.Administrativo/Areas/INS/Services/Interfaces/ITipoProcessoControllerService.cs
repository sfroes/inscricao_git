using SMC.Framework.Model;
using SMC.Framework.UI.Mvc.Html;
using SMC.GPI.Administrativo.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.GPI.Administrativo.Areas.INS.Services
{
    public interface ITipoProcessoControllerService
    {
        /// <summary>
        /// Buscar os Tipos de Processo
        /// </summary>
        /// <returns>Lista de Tipos de Processo</returns>
        SMCPagerModel<TipoProcessoListaViewModel> BuscarTiposProcesso(TipoProcessoFiltroViewModel filtros);
        
        /// <summary>
        /// Buscar o Tipo de Processo desejado
        /// </summary>
        /// <param name="seqTipoProcesso"></param>
        /// <returns></returns>
        TipoProcessoViewModel BuscarTipoProcesso(long seqTipoProcesso);

        /// <summary>
        /// Buscar os Tipos de Processo
        /// </summary>
        /// <returns>Lista de Tipos de Processo</returns>
        SMCMasterDetailList<TipoProcessoSituacaoViewModel> BuscarSituacoesTiposProcesso(long seqTipoTemplateProcesso);
        
        /// <summary>
        /// Salva um Tipo de Processo
        /// </summary>
        /// <param name="modelo">Tipo de Processo a ser persistida</param>
        /// <returns>Sequencial gerado</returns>
        long SalvarTipoProcesso(TipoProcessoViewModel modelo);

        /// <summary>
        /// Exclui um Tipo de Processo
        /// </summary>
        void ExcluirTipoProcesso(long seqTipoProcesso);

        /// <summary>
        /// Busca os tipos de processos para select
        /// </summary>
        /// <returns>Lista de tipos de processos encontrados</returns>
        List<SMCSelectListItem> BuscarTiposProcessosSelect(long? seqUnidadeResponsavel = null);
        
        /// <summary>
        /// Busca os Templates de Processos para select
        /// </summary>
        /// <returns>Lista dos Templates de processo encontrados</returns>
        List<SMCSelectListItem> BuscarTemplatesTiposProcessoSelect(long seqTipoTemplateProcesso);

        /// <summary>
        /// Busca os tipos de templates de processo para select
        /// </summary>
        /// <returns></returns>
        List<SMCDatasourceItem> BuscarTiposTemplateProcessoSelect();

        /// <summary>
        /// Retorna a lista de situações de destino a partir da situação de origem
        /// </summary>        
        List<SMCDatasourceItem> BuscarTipoProcessoSitucaoDestinoSelect(long seqTipoProcessoSituacaoOrigem, long? seqProcesso= null);


        List<SMCDatasourceItem> BuscarTemplatesAssociados(long seqTipoProcesso);

        List<SMCDatasourceItem> BuscarTiposTaxaAssociados(long seqTipoProcesso);
    }
}
