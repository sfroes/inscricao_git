using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.Html;
using SMC.GPI.Administrativo.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.GPI.Administrativo.Areas.INS.Services
{
    public interface IProcessoControllerService
    {
        /// <summary>
        /// Buscar os Processos
        /// </summary>
        /// <returns>Lista de Processo</returns>
        SMCPagerModel<ProcessoListaViewModel> BuscarProcessos(ProcessoFiltroViewModel filtros);

        /// <summary>
        /// Buscar o Processo desejado
        /// </summary>
        /// <param name="seqProcesso"></param>
        /// <returns></returns>
        ProcessoViewModel BuscarProcesso(long seqProcesso);

        /// <summary>
        /// Busca o cabecalho com as informacoes do processo
        /// </summary>
        /// <param name="seqProcesso"></param>
        /// <returns></returns>
        CabecalhoProcessoViewModel BuscarCabecalhoProcesso(long seqProcesso);

        CopiaProcessoViewModel BuscarProcessoCopia(long seqProcesso);

        /// <summary>
        /// Salva um  Processo
        /// </summary>
        /// <param name="modelo">Processo a ser persistida</param>
        /// <returns>Sequencial gerado</returns>
        long SalvarProcesso(ProcessoViewModel modelo);

        /// <summary>
        /// Exclui um Processo
        /// </summary>
        void ExcluirProcesso(long seqProcesso);

        /// <summary>
        /// Copia um Processo
        /// </summary>
        void CopiarProcesso(CopiaProcessoViewModel modelo);

        List<SMCDatasourceItem> BuscarTiposItemHierarquiaOfertaSelect(long seqProcesso, long? seqPai, bool HabilitaCadastroOferta);

        /// <summary>
        /// Buscar as taxas para uma oferta
        /// </summary>
        /// <returns></returns>
        List<SMCDatasourceItem> BuscarTaxasOfertaSelect(long seqProcesso);


        List<SMCDatasourceItem> BuscarEventosGRASelect(long seqUnidadeResponsavel);

        List<SMCDatasourceItem> BuscarSituacoesProcessoSelect(long seqProcesso);

        long BuscarSeqTipoTemplateProcesso(long seqProcesso);

        /// <summary>
        /// Verifica se é permitido cadastrar período taxa em lote para um processo
        /// </summary>
        void VerificarConsistenciaCadastroPeriodoTaxaEmLote(long seqProcesso);


        /// <summary>
        /// Compara se dois processos possuem algum idioma em comum
        /// </summary>        
        /// <returns>
        /// false: se os processos não tiverem NENHUM idioma em comum
        /// true : se os processos tiverem AO MENOS UM idioma em comum
        /// </returns>
        bool CompararIdiomasProcesso(long seqProcessoDestino, long seqProcessoOrigem);
    }
}
