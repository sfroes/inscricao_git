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
    public interface IConfiguracaoEtapaControllerService
    {
        /// <summary>
        /// Buscar as configurações de etapa vinculadas a um processo
        /// </summary>
        /// <param name="seqEtapa"></param>
        /// <param name="seqProcesso"></param>
        /// <returns></returns>
        SMCPagerModel<ConfiguracaoEtapaListaViewModel> BuscarConfiguracoesEtapa(ConfiguracaoEtapaFiltroViewModel filtros);

        /// <summary>
        /// Buscar uma configuração de etapa vinculada a um processo
        /// </summary>
        /// <param name="seqEtapa"></param>
        /// <param name="seqProcesso"></param>
        /// <returns></returns>
        ConfiguracaoEtapaViewModel BuscarConfiguracaoEtapa(long seqEtapaProcesso);

        /// <summary>
        /// Salvar uma configuração de etapa vinculada a um processo
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        long SalvarConfiguracaoEtapa(ConfiguracaoEtapaViewModel modelo);

        /// <summary>
        /// Excluir uma configuração de etapa de um processo
        /// </summary>
        /// <param name="seqEtapa"></param>
        /// <param name="seqProcesso"></param>
        void ExcluirConfiguracaoEtapa(long seqEtapaProcesso);      

        /// <summary>
        /// Buscar o cabecalho com informações do processo, etapa e configuração da etapa
        /// </summary>
        /// <param name="seqEtapa"></param>
        /// <param name="seqProcesso"></param>
        /// <param name="seqConfiguracaoEtapa"></param>
        /// <returns></returns>
        CabecalhoProcessoEtapaConfiguracaoViewModel BuscarCabecalhoProcessoEtapaConfiguracao(long seqConfiguracaoEtapa);

        string BuscarNomeConfiguracaoEtapa(long seqConfiguracaoEtapa);

        List<SMCDatasourceItem> BuscarConfiguracoesEtapaKeyValue(ConfiguracaoEtapaFiltroViewModel filtro);

        void CopiarConfiguracoesEtapa(CopiarConfiguracoesEtapaViewModel modelo);
    }
}
