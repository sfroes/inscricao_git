using SMC.Framework.Extensions;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc.Controllers.Service;
using SMC.GPI.Administrativo.Areas.INS.Models;
using SMC.Inscricoes.Service.Areas.INS.Services;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using SMC.Financeiro.ServiceContract.BLT;
using SMC.Financeiro.ServiceContract.TXA.Data;
using SMC.Inscricoes.ServiceContract.Areas.RES.Interfaces;
using SMC.Framework;
using SMC.Framework.Util;
using SMC.Inscricoes.Common;
using SMC.Financeiro.Service.FIN;
using SMC.Inscricoes.Domain.Areas.INS.Models;

namespace SMC.GPI.Administrativo.Areas.INS.Services
{
    public class ProcessoControllerService : SMCControllerServiceBase
    {

        #region Services

        private IProcessoService ProcessoService
        {
            get { return this.Create<IProcessoService>(); }
        }

        private IIntegracaoFinanceiroService FinanceiroService
        {
            get { return this.Create<IIntegracaoFinanceiroService>(); }
        }

        private IUnidadeResponsavelService UnidadeResponsavelService
        {
            get { return this.Create<IUnidadeResponsavelService>(); }
        }

        #endregion

        public SMCPagerModel<ProcessoListaViewModel> BuscarProcessos(ProcessoFiltroViewModel filtros)
        {
            var pagerData = this.ProcessoService.BuscarProcessos(
                SMCMapperHelper.Create<ProcessoFiltroData>(filtros));
            SMCPagerData<ProcessoListaViewModel> model = SMCMapperHelper.Create<SMCPagerData<ProcessoListaViewModel>>(pagerData);
            return new SMCPagerModel<ProcessoListaViewModel>(model, filtros.PageSettings, filtros);
        }

        public List<SMCDatasourceItem> BuscarEventosGRASelect(long seqUnidadeResponsavel)
        {
            var seqsCentroCusto = this.UnidadeResponsavelService.BuscarUnidadeResponsavel(seqUnidadeResponsavel)
                .CentrosCusto.Select(x => new Nullable<int>(x.CentroCusto)).ToList();
            var filtro = new EventoFiltroData
            {
                ListaCodigoCentroCusto = seqsCentroCusto
            };
            var datas = this.FinanceiroService.BuscarEventos(filtro);
            if (datas != null)
            {
                return datas.TransformList<SMCDatasourceItem>();
            }
            else return new List<SMCDatasourceItem>();
        }

        public ProcessoViewModel BuscarProcesso(long seqProcesso)
        {
            var processoData = this.ProcessoService.BuscarProcesso(seqProcesso);
            
            var retorno = SMCMapperHelper.Create<ProcessoViewModel>(processoData);
            retorno.HabilitaGestaoEvento = processoData.TipoProcesso.GestaoEventos;

            return retorno;
        }

        public CabecalhoProcessoViewModel BuscarCabecalhoProcesso(long seqProcesso)
        {
            return SMCMapperHelper.Create<CabecalhoProcessoViewModel>
                (this.ProcessoService.BuscarCabecalhoProcesso(seqProcesso));
        }

        public CopiaProcessoViewModel BuscarProcessoCopia(long seqProcesso)
        {
            var modelo = this.ProcessoService.BuscarProcessoCopia(seqProcesso).Transform<CopiaProcessoViewModel>();
            modelo.NovoProcessoDescricao = modelo.Descricao;
            return modelo;
        }

        public long SalvarProcesso(ProcessoViewModel modelo)
        {
            return this.ProcessoService.SalvarProcesso(SMCMapperHelper.Create<ProcessoData>(modelo));
        }

        public void ExcluirProcesso(long seqProcesso)
        {
            this.ProcessoService.ExcluirProcesso(seqProcesso);
        }

        public void CopiarProcesso(CopiaProcessoViewModel modelo)
        {
            this.ProcessoService.CopiarProcesso(modelo.Transform<CopiaProcessoData>());
        }

        public List<SMCDatasourceItem> BuscarTiposItemHierarquiaOfertaSelect(long seqProcesso, long? seqPai, bool HabilitaCadastroOferta)
        {
            return this.ProcessoService
                .BuscarTiposItemHierarquiaOfertaKeyValue(seqProcesso, seqPai, HabilitaCadastroOferta)
                .TransformList<SMCDatasourceItem>();
        }

        public List<SMCDatasourceItem> BuscarTaxasOfertaSelect(long seqProcesso)
        {
            return ProcessoService.BuscarTaxasKeyValue(seqProcesso).TransformList<SMCDatasourceItem>();
        }


        public List<SMCDatasourceItem> BuscarSituacoesProcessoSelect(long seqProcesso)
        {
            return this.ProcessoService.BuscarSituacoesProcessoPorEtapaKeyValue(seqProcesso, TOKENS.ETAPA_INSCRICAO).
                TransformList<SMCDatasourceItem>();
        }

        public long BuscarSeqTipoTemplateProcesso(long seqProcesso)
        {
            return this.ProcessoService.BuscarTipoTemplateProcesso(seqProcesso);
        }

        /// <summary>
        /// Verifica se é permitido cadastrar período taxa em lote para um processo
        /// </summary>
        public void VerificarConsistenciaCadastroPeriodoTaxaEmLote(long seqProcesso)
        {
            ProcessoService.VerificarConsistenciaCadastroPeriodoTaxaEmLote(seqProcesso);
        }

        /// <summary>
        /// Compara se dois processos possuem algum idioma em comum
        /// </summary>        
        /// <returns>
        /// false: se os processos não tiverem NENHUM idioma em comum
        /// true : se os processos tiverem AO MENOS UM idioma em comum
        /// </returns>
        public bool CompararIdiomasProcesso(long seqProcessoDestino, long seqProcessoOrigem)
        {
            return this.ProcessoService.CompararIdiomasProcesso(seqProcessoDestino, seqProcessoOrigem);
        }

        /// <summary>
        /// Busca quais os sequenciais dos formulários usados em um determinado processo
        /// </summary>
        /// <param name="seqProcesso">Sequencial do processo</param>
        /// <returns>Lista de sequenciais dos formulários do SGF que são utilizados</returns>
        public List<long> BuscarSeqsFormulariosDoProcesso(long? seqOferta, long? seqGrupoOferta, long seqProcesso)
        {
            return this.ProcessoService.BuscarSeqsFormulariosDoProcesso(seqOferta, seqGrupoOferta, seqProcesso);
        }

        public List<SMCDatasourceItem> BuscarProcessosSelect(ProcessoCandidatoFiltroViewModel filtro)
        {
            return this.ProcessoService.BuscarProcessoSelect(SMCMapperHelper.Create<ProcessoCandidatoFiltroData>(filtro)).
                TransformList<SMCDatasourceItem>();
        }

        public List<SMCDatasourceItem<string>> BuscarConfiguracoesAssinaturaGadSelect(long seqUnidadeResponsavel)
        {
            return ProcessoService.BuscarConfiguracoesAssinaturaGadSelect(seqUnidadeResponsavel).TransformList<SMCDatasourceItem<string>>();
        }

        public bool ValidarFormulariosAssert(ProcessoViewModel processo)
        {
            return ProcessoService.ValidarFormulariosAssert(processo.Transform<ProcessoData>());
        }

        public bool ValidarAssertDocumentoEmitido(ProcessoViewModel processo)
        {
            return ProcessoService.ValidarAssertDocumentoEmitido(processo.Transform<ProcessoData>());
        }

        public string BuscaTokenCssAlternativo(long seqUnidadeResponsavelTipoProcessoIdVisual)
        {
            return UnidadeResponsavelService.BuscaTokenCssAlternativo(seqUnidadeResponsavelTipoProcessoIdVisual);
        }
    }
}