using SMC.Financeiro.Service.FIN;
using SMC.Framework.Extensions;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc.Controllers.Service;
using SMC.GPI.Administrativo.Areas.INS.Models;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using SMC.Inscricoes.ServiceContract.Areas.RES.Interfaces;
using System.Collections.Generic;

namespace SMC.GPI.Administrativo.Areas.INS.Services
{
    public class ConfiguracaoEtapaControllerService : SMCControllerServiceBase
    {

        #region Services

        private IProcessoService ProcessoService 
        {
            get { return this.Create<IProcessoService>(); }
        }

        private IFinanceiroService FinanceiroService
        {
            get { return this.Create<IFinanceiroService>(); }
        }

        private IUnidadeResponsavelService UnidadeResponsavelService
        {
            get { return this.Create<IUnidadeResponsavelService>(); }
        }

        private IConfiguracaoEtapaService ConfiguracaoEtapaService
        {
            get { return this.Create<IConfiguracaoEtapaService>(); }
        }


        #endregion



        public SMCPagerModel<ConfiguracaoEtapaListaViewModel> BuscarConfiguracoesEtapa(ConfiguracaoEtapaFiltroViewModel filtros)
        {
            var datas  = 
                this.ConfiguracaoEtapaService.BuscarConfiguracoesEtapa(filtros.Transform<ConfiguracaoEtapaFiltroData>());
            var pagerData = datas.Transform<SMCPagerData<ConfiguracaoEtapaListaViewModel>>();
            return new SMCPagerModel<ConfiguracaoEtapaListaViewModel>(pagerData, filtros.PageSettings, filtros);
        }

        public ConfiguracaoEtapaViewModel BuscarConfiguracaoEtapa(long seqConfiguracaoEtapa)
        {
            return this.ConfiguracaoEtapaService
                .BuscarConfiguracaoEtapa(seqConfiguracaoEtapa).Transform<ConfiguracaoEtapaViewModel>();
        }

        public long SalvarConfiguracaoEtapa(ConfiguracaoEtapaViewModel modelo)
        {
            return this.ConfiguracaoEtapaService
                .SalvarConfiguracaoEtapa(modelo.Transform<ConfiguracaoEtapaData>());
        }


        public void ExcluirConfiguracaoEtapa(long seqConfiguracaoEtapa)
        {
            this.ConfiguracaoEtapaService.ExcluirConfiguracaoEtapa(seqConfiguracaoEtapa);
        }


        public CabecalhoProcessoEtapaConfiguracaoViewModel BuscarCabecalhoProcessoEtapaConfiguracao(long seqConfiguracaoEtapa)
        {
            return this.ConfiguracaoEtapaService.BuscarCabecalhoEtapaProcessoConfiguracao(seqConfiguracaoEtapa)
                .Transform<CabecalhoProcessoEtapaConfiguracaoViewModel>();
        }

        public string BuscarNomeConfiguracaoEtapa(long seqConfiguracaoEtapa) 
        {
            return this.ConfiguracaoEtapaService.BuscarDescricaoConfiguracaoEtapa(seqConfiguracaoEtapa);
        }
        
        public List<SMCDatasourceItem> BuscarConfiguracoesEtapaKeyValue(ConfiguracaoEtapaFiltroViewModel filtro) 
        {
            return this.ConfiguracaoEtapaService.BuscarConfiguracoesEtapaKeyValue(
                filtro.Transform<ConfiguracaoEtapaFiltroData>())
                .TransformList<SMCDatasourceItem>();
        }

        public void CopiarConfiguracoesEtapa(CopiarConfiguracoesEtapaViewModel modelo) 
        {
            this.ConfiguracaoEtapaService.CopiarConfiguracoesEtapa(
                modelo.Transform<CopiarConfiguracoesEtapaData>());
        }

    }
}