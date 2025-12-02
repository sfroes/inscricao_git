using SMC.Framework.Extensions;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc.Controllers.Service;
using SMC.GPI.Administrativo.Areas.INS.Models;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using System.Collections.Generic;

namespace SMC.GPI.Administrativo.Areas.INS.Services
{
    public class EtapaProcessoControllerService : SMCControllerServiceBase
    {

        #region Services

        private IEtapaProcessoService EtapaProcessoService 
        {
            get 
            {
                return this.Create<IEtapaProcessoService>();
            }
        }

        #endregion

        public SMCPagerModel<EtapaProcessoListaViewModel> BuscarEtapasProcesso(EtapaProcessoFiltroViewModel filtros)
        {
            var pagerData = this.EtapaProcessoService.BuscarEtapasProcesso(filtros.Transform<EtapaProcessoFiltroData>())
                .Transform<SMCPagerData<EtapaProcessoListaViewModel>>();
            var pagerModel = new SMCPagerModel<EtapaProcessoListaViewModel>(pagerData, filtros.PageSettings, filtros);
            return pagerModel;
        }

        public EtapaProcessoViewModel BuscarEtapaProcesso(long seqEtapaProcesso)
        {
            return this.EtapaProcessoService.BuscarEtapaProcesso(seqEtapaProcesso).Transform<EtapaProcessoViewModel>();
        }

        public long SalvarAssociacaoEtapa(EtapaProcessoViewModel modelo)
        {
            return this.EtapaProcessoService.SalvarEtapaProcesso(modelo.Transform<EtapaProcessoData>());
        }

        public void ExcluirAssociacaoEtapa(long seqEtapaProcesso)
        {
            this.EtapaProcessoService.ExcluirEtapaProcesso(seqEtapaProcesso);
        }

        public List<SMCDatasourceItem> BuscarEtapasSelect(long seqProcesso) 
        {
            return this.EtapaProcessoService.BuscarEtapasSGFKeyValue(seqProcesso)
                .TransformList<SMCDatasourceItem>();
        }

        public List<SMCDatasourceItem> BuscarSituacoesPermitidas(long seqEtapaProcesso) 
        {
            return this.EtapaProcessoService.BuscarSituacoesPermitidas(seqEtapaProcesso)
                .TransformList<SMCDatasourceItem>();
        }

        public CabecalhoProcessoEtapaViewModel BuscarCabecalhoProcessoEtapa(long seqEtapaProcesso)
        {
            return this.EtapaProcessoService.BuscarCabecalhoProcessoEtapa(seqEtapaProcesso)
                .Transform<CabecalhoProcessoEtapaViewModel>();
        }


        
        /// <summary>
        /// Verifica se a inclusão de etapas é permitida
        /// </summary>        
        public void VerificarPermissaoCadastrarEtapa(long seqProcesso) 
        {
            this.EtapaProcessoService.VerificarPermissaoCadastrarEtapa(seqProcesso);
        }

        public List<ConfiguracaoProrrogacaoViewModel> BuscarConfiguracoesProrrogacao(long seqEtapaProcesso
            , long[] seqOfertas)
        {
            return this.EtapaProcessoService.BuscarConfiguracoesProrrogacao(seqEtapaProcesso,
                seqOfertas).TransformList<ConfiguracaoProrrogacaoViewModel>();
        }

        /// <summary>
        /// Recupera o sumário de uma prorrogação de processo para ser exibido para o usuário
        /// </summary>
        public ProrrogacaoEtapaViewModel SumarioProrrogacao(ProrrogacaoEtapaViewModel etapaProrrogar)
        {
            return this.EtapaProcessoService.SumarioProrrogacao(
                etapaProrrogar.Transform<ProrrogacaoEtapaData>()).Transform<ProrrogacaoEtapaViewModel>();
        }

        /// <summary>
        /// Prorroga a etapa informada com os dados passados no DTO
        /// </summary>
        /// <param name="etapaProrrogar"></param>
        public void ProrrogarEtapa(ProrrogacaoEtapaViewModel etapaProrrogar)
        {
            this.EtapaProcessoService.ProrrogarEtapa(etapaProrrogar.Transform<ProrrogacaoEtapaData>());
        }

        /// <summary>
        /// Verifica se é possível prorrogar a etapa informada
        /// </summary>        
        public void VerificarPossibilidadeProrrogacao(long seqEtapaProcesso)
        {
            this.EtapaProcessoService.VerificarPossibilidadeProrrogacao(seqEtapaProcesso);
        }

    }
}