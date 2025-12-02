using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.Extensions;
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
using SMC.Financeiro.Service.FIN;

namespace SMC.GPI.Administrativo.Areas.INS.Services
{
    public class ConfiguracaoEtapaPaginaControllerService : SMCControllerServiceBase
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

        private IInscricaoService InscricaoService
        {
            get { return Create<IInscricaoService>(); }
        }

        #endregion

        #region Montagem da árvore de páginas

        public List<ArvoreItemConfiguracaoPaginaEtapaViewModel> BuscarItensArvoreConfiguracaoPaginaEtapa(long seqConfiguracaoEtapa)
        {
            return this.ConfiguracaoEtapaService.BuscarArvoreConfiguracaoEtapa(seqConfiguracaoEtapa)
                .TransformList<ArvoreItemConfiguracaoPaginaEtapaViewModel>();
        }

        #endregion

        #region CRUD de páginas

        /// <summary>
        /// Cria as páginas para uma configuração de etapa de acordo com o template do SGF 
        /// e os idiomas configurados para o processo
        /// </summary>        
        public void CriarPaginasPadraoConfiguracao(long seqConfiguracaoEtapa)
        {
            this.ConfiguracaoEtapaService.CriarPaginasPadraoConfiguracao(seqConfiguracaoEtapa);
        }

        /// <summary>
        /// Verifica se a configuração possui páginas
        /// Retorna true se existirem página e falso caso contrário
        /// </summary>        
        public bool VerificarConfiguracaoPossuiPaginas(long seqConfiguracaoEtapa)
        {
            return this.ConfiguracaoEtapaService.VerificarConfiguracaoPossuiPaginas(seqConfiguracaoEtapa);
        }

        public void SalvarConfiguracaoPaginaEtapa(ConfigurarPaginaEtapaViewModel modelo)
        {
            this.ConfiguracaoEtapaService
                .SalvarConfiguracaoPagina(modelo.Transform<ConfiguracaoPaginaData>());
        }

        public bool VerificaApenasInscricoesTeste(long seqConfiguracaoEtapaPaginaIdioma)
        {
            var seqInscricoes = ConfiguracaoEtapaService.BuscarInscricoesConfiguracaoPaginaEtapaIdioma(seqConfiguracaoEtapaPaginaIdioma);

            return InscricaoService.VerificaApenasInscricoesTeste(seqInscricoes);
        }

        public void ExcluirConfiguracaoPaginaEtapa(long seqConfiguracaoEtapaPagina)
        {
            this.ConfiguracaoEtapaService.ExcluirConfiguracaoEtapaPagina(seqConfiguracaoEtapaPagina);
        }

        public void DuplicarConfiguracaoEtapaPagina(long seqConfiguracaoEtapaPagina)
        {
            this.ConfiguracaoEtapaService.DuplicarConfiguracaoEtapaPagina(seqConfiguracaoEtapaPagina);
        }

        /// <summary>
        /// Retora a lista de páginas não criadas com o sequencial das páginas no SGF
        /// </summary>        
        public List<SMCDatasourceItem> BuscarPaginasNaoCriadas(long seqConfiguracaoEtapa)
        {
            return this.ConfiguracaoEtapaService.BuscarPaginasNaoCriadas(seqConfiguracaoEtapa)
                .TransformList<SMCDatasourceItem>();
        }

        /// <summary>
        /// Adiciona as páginas segundo o modelo do SGF na configuracao etapa passada 
        /// (para todos os idiomas em uso na configuração)
        /// </summary>        
        public void SalvarRecuperacaoPaginasConfiguracaoPaginaEtapa(RecuperarPaginaEtapaViewModel modelo)
        {
            this.ConfiguracaoEtapaService.AdicionarPaginasConfiguracao(modelo.SeqConfiguracaoEtapa,
                modelo.SeqPaginas);
        }

        /// <summary>
        /// Busca a configuação de página para edição
        /// </summary>
        public ConfigurarPaginaEtapaViewModel BuscarConfiguracaoPagina(long seqConfiguracaoEtapaPagina)
        {
            return this.ConfiguracaoEtapaService.BuscarConfiguracaoEtapaPaginaEdicao(seqConfiguracaoEtapaPagina)
                .Transform<ConfigurarPaginaEtapaViewModel>();
        }

        #endregion

        #region Configurações de idioma nas páginas

        public AlterarIdiomaEtapaViewModel BuscarIdiomasEtapa(long seqConfiguracaoEtapa)
        {
            return this.ConfiguracaoEtapaService.BuscarIdiomasPaginasProcesso(seqConfiguracaoEtapa)
                .Transform<AlterarIdiomaEtapaViewModel>();
        }

        public void SalvarAlteraracaoIdiomaConfiguracaoPaginaEtapa(AlterarIdiomaEtapaViewModel modelo)
        {
            this.ConfiguracaoEtapaService.AlterarIdiomasPaginas(
                modelo.Transform<IdiomasPaginasProcessoData>());
        }

        /// <summary>
        /// Busca a configuração de página para um idioma
        /// </summary>        
        public ConfigurarPaginaIdiomaEtapaViewModel BuscarConfiguracaoPaginaIdioma(long seqConfiguracaoEtapaPaginaIdioma) 
        {
            return this.ConfiguracaoEtapaService
                .BuscarConfiguracaoEtapaPaginaIdioma(seqConfiguracaoEtapaPaginaIdioma)
                .Transform<ConfigurarPaginaIdiomaEtapaViewModel>();
        }

        public void SalvarConfiguracaoPaginaIdiomaEtapa(ConfigurarPaginaIdiomaEtapaViewModel modelo)
        {
            this.ConfiguracaoEtapaService.SalvarConfiguracaoEtapaPaginaIdioma(
                modelo.Transform<ConfiguracaoPaginaIdiomaData>());
        }

        #endregion

        #region Configuração de seção de texto na página

        public ConfigurarTextoSecaoViewModel BuscarTextoSecao(long seq) 
        {
           return this.ConfiguracaoEtapaService.BuscarSecaoTextoPagina(seq)
               .Transform<ConfigurarTextoSecaoViewModel>();
        }

        public void SalvarConfiguracaoTextoSecao(ConfigurarTextoSecaoViewModel modelo)
        {
            this.ConfiguracaoEtapaService.SalvarTextoSecao(
                modelo.Transform<SecaoPaginaTextoData>());
        }

        #endregion

        #region Configuração de seções de arquivo

        public List<ConfigurarArquivoSecaoDetalheViewModel> BuscarArquivosSecao(long seqConfiguracaoEtapaPaginaIdioma,
            long seqSecaoPaginaSGF) 
        {
            return this.ConfiguracaoEtapaService.BuscarArquivosSecaoPagina(
                new ArquivoSecaoFiltroData
                {
                    SeqConfiguracaoEtapaPaginaIdioma = seqConfiguracaoEtapaPaginaIdioma,
                    SeqSecaoPaginaSGF = seqSecaoPaginaSGF
                }).TransformList<ConfigurarArquivoSecaoDetalheViewModel>();
        }

        public void SalvarConfiguracaoArquivoSecao(ConfigurarArquivoSecaoViewModel modelo)
        {
            this.ConfiguracaoEtapaService.SalvarArquivosSecaoPagina(modelo.SeqConfiguracaoEtapaPaginaIdioma, modelo.SeqSecaoPaginaSGF,
                modelo.Arquivos.TransformList<ArquivoSecaoData>());
        }

        #endregion     

        public bool VerificaFormularioEmUso(long seqConfiguracaoEtapaPaginaIdioma)
        {
            return InscricaoService.VerificaFormularioEmUso(seqConfiguracaoEtapaPaginaIdioma);
        }
    }

}
