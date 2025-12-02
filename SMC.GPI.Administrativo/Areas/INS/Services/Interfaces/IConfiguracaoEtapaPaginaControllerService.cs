using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.GPI.Administrativo.Areas.INS.Models;
using System.Collections.Generic;

namespace SMC.GPI.Administrativo.Areas.INS.Services
{
    public interface IConfiguracaoEtapaPaginaControllerService
    {

        #region Montagem da árvore de páginas

        /// <summary>
        /// Buscar os itens da árvore da configuracao de pagina da etapa
        /// </summary>
        /// <param name="seqEtapa"></param>
        /// <param name="seqProcesso"></param>
        /// <returns></returns>
        List<ArvoreItemConfiguracaoPaginaEtapaViewModel> BuscarItensArvoreConfiguracaoPaginaEtapa(long seqConfiguracaoEtapa);

        #endregion

        #region CRUD de páginas

        /// <summary>
        /// Cria as páginas para uma configuração de etapa de acordo com o template do SGF 
        /// e os idiomas configurados para o processo
        /// </summary>        
        void CriarPaginasPadraoConfiguracao(long seqConfiguracaoEtapa);

        /// <summary>
        /// Verifica se a configuração possui páginas
        /// Retorna true se existirem página e falso caso contrário
        /// </summary>        
        bool VerificarConfiguracaoPossuiPaginas(long seqConfiguracaoEtapa);

        /// <summary>
        /// Salvar uma configuração de página da etapa
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        void SalvarConfiguracaoPaginaEtapa(ConfigurarPaginaEtapaViewModel modelo);

        /// <summary>
        /// Verifica se todas as inscrições estão canceladas como teste.
        /// </summary>        
        bool VerificaApenasInscricoesTeste(long seqConfiguracaoEtapaPaginaIdioma);

        /// <summary>
        /// Excluir uma configuração de página da etapa
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        void ExcluirConfiguracaoPaginaEtapa(long seqConfiguracaoEtapaPagina);

        /// <summary>
        /// Duplica uma página copiando os dados básicos do SGF
        /// </summary>
        void DuplicarConfiguracaoEtapaPagina(long seqConfiguracaoEtapaPagina);

        /// <summary>
        /// Retora a lista de páginas não criadas com o sequencial das páginas no SGF
        /// </summary>        
        List<SMCDatasourceItem> BuscarPaginasNaoCriadas(long seqConfiguracaoEtapa);

        /// <summary>
        /// Buscar a recuperacao de pagina de uma etapa
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        void SalvarRecuperacaoPaginasConfiguracaoPaginaEtapa(RecuperarPaginaEtapaViewModel modelo);

        /// <summary>
        /// Busca a configuação de página para edição
        /// </summary>
        ConfigurarPaginaEtapaViewModel BuscarConfiguracaoPagina(long seqConfiguracaoEtapaPagina);

        #endregion

        #region Configuração de idioma em páginas

        /// <summary>
        /// Buscar os idiomas de uma etapa
        /// </summary>
        /// <param name="seqEtapa"></param>
        /// <returns></returns>
        AlterarIdiomaEtapaViewModel BuscarIdiomasEtapa(long seqConfiguracaoEtapa);

        /// <summary>
        /// Salvar a alteração de idioma de uma etapa
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        void SalvarAlteraracaoIdiomaConfiguracaoPaginaEtapa(AlterarIdiomaEtapaViewModel modelo);


        /// <summary>
        /// Salvar a configuração da página no idioma
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        void SalvarConfiguracaoPaginaIdiomaEtapa(ConfigurarPaginaIdiomaEtapaViewModel modelo);

        /// <summary>
        /// Busca a configuração de página para um idioma
        /// </summary>        
        ConfigurarPaginaIdiomaEtapaViewModel BuscarConfiguracaoPaginaIdioma(long seqConfiguracaoEtapaPaginaIdioma);

        #endregion

        #region Configuração de seção de texto

        /// <summary>
        /// Busca o texto de uma seção pra edição
        /// </summary>        
        ConfigurarTextoSecaoViewModel BuscarTextoSecao(long seq);

        /// <summary>
        /// Salvar configuração do texto de uma seção
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        void SalvarConfiguracaoTextoSecao(ConfigurarTextoSecaoViewModel modelo);

        #endregion

        #region Configuração de seção de arquivos

        /// <summary>
        /// Salvar configuração de arquivo de uma seção
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        void SalvarConfiguracaoArquivoSecao(ConfigurarArquivoSecaoViewModel modelo);         

        /// <summary>
        /// Busca os arquivos de uma seção de uma página num determinada idioma para edição
        /// </summary>        
        List<ConfigurarArquivoSecaoDetalheViewModel> BuscarArquivosSecao(long seqConfiguracaoEtapaPaginaIdioma, long seqSecaoPaginaSGF);

        #endregion
      
        bool VerificaFormularioEmUso(long seqConfiguracaoEtapaPaginaIdioma);
    }
}
