using SMC.Framework.Extensions;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc.Controllers.Service;
using SMC.GPI.Administrativo.Areas.INS.Models;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using SMC.Financeiro.ServiceContract.BLT;
using SMC.Financeiro.ServiceContract.TXA.Data;
using System.Collections.Generic;
using System.Linq;
using SMC.Financeiro.Service.FIN;

namespace SMC.GPI.Administrativo.Areas.INS.Services
{
    public class HierarquiaOfertaControllerService : SMCControllerServiceBase
    {
        #region Services
        private IOfertaService OfertaService
        {
            get
            {
                return this.Create<IOfertaService>();
            }
        }

        private IProcessoService ProcessoService
        {
            get
            {
                return this.Create<IProcessoService>();
            }
        }

        private ProcessoControllerService ProcessoControllerService
        {
            get
            {
                return this.Create<ProcessoControllerService>();
            }
        }

        private IIntegracaoFinanceiroService FinanceiroService
        {
            get { return this.Create<IIntegracaoFinanceiroService>(); }
        }

        #endregion

        public HierarquiaOfertaViewModel BuscarInformaceosHierarquiaOferta(long seqProcesso)
        {
            return new HierarquiaOfertaViewModel
            {
                SeqProcesso = seqProcesso,
                RaizPermiteItemFilho = OfertaService.VerificarRaizHierarquiaOfertaPermiteItem(seqProcesso),
                RaizPermiteOferta = OfertaService.VerificarRaizHierarquiaOfertaPermiteOferta(seqProcesso)
            };
        }

        public OfertaViewModel BuscarOfertaHierarquiaOferta(long seqOferta)
        {
            return this.OfertaService.BuscarOferta(seqOferta).Transform<OfertaViewModel>();
        }

        /// <summary>
        /// Busca um item da hierarquia de oferta através de um processo
        /// </summary>        
        public AssociarItemHierarquiaOfertaViewModel BuscarItemHierarquiaOferta(long seqItemHierarquiaOferta)
        {
            return this.OfertaService.BuscarHierarquiaOferta(seqItemHierarquiaOferta)
                .Transform<AssociarItemHierarquiaOfertaViewModel>();
        }

        // <summary>
        /// Salvar a associação de um item de hierarquia de oferta com o processo
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        public long SalvarHierarquiaOferta(AssociarItemHierarquiaOfertaViewModel modelo)
        {
            return this.OfertaService.SalvarHierarquiaOferta(
                modelo.Transform<HierarquiaOfertaData>());
        }

        /// <summary>
        /// Salvar a oferta para um item de hierarquia de oferta
        /// </summary>        
        public long SalvarOferta(OfertaViewModel modelo)
        {
            return this.OfertaService.SalvarOferta(
               modelo.Transform<OfertaData>());
        }


        /// <summary>
        /// Exclui uma oferta
        /// </summary>        
        public void ExcluirOferta(long seqOferta)
        {
            this.OfertaService.ExcluirOferta(seqOferta);
        }

        /// <summary>
        /// Exclui uma hierarquia de oferta (item da árvore)
        /// </summary>        
        public void ExcluirHierarquiaOferta(long seqHierarquiaOferta)
        {
            this.OfertaService.ExcluirHierarquiaOferta(seqHierarquiaOferta);
        }

        public List<SMCDatasourceItem> BuscarEventosTaxaSelect(long seqProcesso)
        {
            int? seqEvento = ProcessoService.BuscarEventoProcesso(seqProcesso);
            if (!seqEvento.HasValue) return new List<SMCDatasourceItem>();
            return FinanceiroService.BuscarEventosTaxa(new EventoTaxaFiltroData
            {
                SeqEvento = seqEvento
            }).Select(x => new SMCDatasourceItem
            {
                Seq = x.SeqEventoTaxa,
                Descricao = x.Descricao
            }).ToList();
        }

        public decimal? BuscarValorEvento(int seqEventoTaxa)
        {
            return FinanceiroService.BuscarEventosTaxa(new EventoTaxaFiltroData
            {
                SeqEventoTaxa = seqEventoTaxa
            }).FirstOrDefault().Valor;
        }

        public List<SMCDatasourceItem> BuscarDataVencimentoSelect(long seqProcesso)
        {
            int? seqEvento = ProcessoService.BuscarEventoProcesso(seqProcesso);
            if (!seqEvento.HasValue) return new List<SMCDatasourceItem>();
            return FinanceiroService.BuscarParametrosCREI(new ParametroCREIFiltroData
            {
                SeqEvento = seqEvento
            }).Select(x => new SMCDatasourceItem
            {
                Seq = x.SeqParametroCREI,
                Descricao = x.DataVencimentoTitulo.ToShortDateString() + " - CREI: " + x.SeqParametroCREI.ToString()
            }).ToList();
        }

        public SMCPagerModel<OfertaTaxaViewModel> BuscarTaxasOferta(OfertaTaxaFiltroViewModel filtro)
        {
            var datas = this.OfertaService.BuscarOfertasTaxas(
                filtro.Transform<OfertaTaxaFiltroData>());
            var pagerData = datas.Transform<SMCPagerData<OfertaTaxaViewModel>>();
            return new SMCPagerModel<OfertaTaxaViewModel>(pagerData, filtro.PageSettings, filtro);
        }

        public List<SMCDatasourceItem<string>> BuscarPeriodosOfertas(long seqProcesso)
        {
            return this.OfertaService.BuscarPeriodosOfertas(seqProcesso)
                .TransformList<SMCDatasourceItem<string>>();
        }

        public SMCPagerModel<SMCDatasourceItem> BuscarOfertasPeriodoTaxaParaInclusao(CadastroOfertaTaxaFiltroViewModel filtro)
        {
            filtro.PossuiTaxa = false;
            return new SMCPagerModel<SMCDatasourceItem>(this.OfertaService.BuscarOfertasPeriodoTaxaKeyValue(
                filtro.Transform<OfertaPeriodoTaxaFiltroData>()).OrderBy(o => o.Descricao), filtro.PageSettings, filtro);
        }

        public SMCPagerModel<SMCDatasourceItem> BuscarOfertasPeriodoTaxaParaExclusao(CadastroOfertaTaxaFiltroViewModel filtro)
        {
            filtro.PossuiTaxa = true;
            return new SMCPagerModel<SMCDatasourceItem>(this.OfertaService.BuscarOfertasPeriodoTaxaKeyValue(
                filtro.Transform<OfertaPeriodoTaxaFiltroData>()).OrderBy(o => o.Descricao), filtro.PageSettings, filtro);
        }

        public void ExcluirTaxaOfertaEmLote(long seqTipoTaxa, List<long> seqOfertas)
        {
            this.OfertaService.ExcluirTaxaOfertaEmLote(seqTipoTaxa, seqOfertas);
        }

        public List<SMCDatasourceItem> BuscarOfertasKeyValue(List<long> seqOfertas)
        {
            return this.OfertaService.BuscarOfertasKeyValue(seqOfertas).ToList();
        }

        public void IncluirTaxasLote(IncluirTaxaOfertaViewModel modelo)
        {
            this.OfertaService.IncluirTaxasLote(modelo.Transform<IncluirTaxaEmLoteData>());
        }

        /// <summary>
        /// Verifica se a inclusão de itens de hierarquia de oferta é permitida
        /// </summary>        
        public void VerificarPermissaoCadastrarHierarquia(long seqProcesso)
        {
            this.OfertaService.VerificarPermissaoCadastrarHierarquia(seqProcesso);
        }

        public string BuscarDescricaoTaxa(long seqTaxa)
        {
            return OfertaService.BuscarDescricaoTaxa(seqTaxa);
        }

        public OfertaViewModel BuscarUltimaOfertaCadastrada(long seqProcesso)
        {
            var oferta = OfertaService.BuscarUltimaOfertaCadastrada(seqProcesso);
            if (oferta != null)
                return oferta.Transform<OfertaViewModel>();
            return null;
        }

        public bool? VerificaTipoTaxaCobraPorQtdOferta(long seqTipoTaxa)
        {
            return ProcessoService.VerificaTipoTaxaCobraPorQtdOferta(seqTipoTaxa);
        }
    }
}