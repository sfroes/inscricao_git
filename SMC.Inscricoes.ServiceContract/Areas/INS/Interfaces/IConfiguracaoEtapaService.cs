using SMC.Framework.Model;
using SMC.Framework.Service;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using System.Collections.Generic;
using System.ServiceModel;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces
{
    /// <summary>
    /// Inteface para o serviço que chama o DomainService de Inscrito
    /// </summary>
    [ServiceContract(Namespace = NAMESPACES.SERVICE)]
    public interface IConfiguracaoEtapaService : ISMCService
    {

        /// <summary>
        /// Busca a lista de etapas de um processo filtrada
        /// </summary>        
        SMCPagerData<ConfiguracaoEtapaListaData> BuscarConfiguracoesEtapa(ConfiguracaoEtapaFiltroData filtro);

        ConfiguracaoEtapaData BuscarConfiguracaoEtapa(long seqConfiguracaoEtapa);

        void ExcluirConfiguracaoEtapa(long seqConfiguracaoEtapa);

        long SalvarConfiguracaoEtapa(ConfiguracaoEtapaData configuracaoEtapa);

        /// <summary>
        /// Retorna o cabeçalho contendo informações do processo, da etapa e da configuração de etapa
        /// </summary>        
        CabecalhoProcessoEtapaConfiguracaoData BuscarCabecalhoEtapaProcessoConfiguracao(long seqConfiguracaoEtapa);

        /// <summary>
        /// Retorna a árvore de páginas > idiomas > seções para uma configuração de etapa
        /// </summary>        
        List<NoArvoreConfiguracaoEtapaData> BuscarArvoreConfiguracaoEtapa(long seqConfiguracaoEtapa);

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
        /// Exclui uma página de uma configuração de etapa e suas seções filhas para todos os idiomas
        /// </summary>        
        void ExcluirConfiguracaoEtapaPagina(long seqConfiguracaoEtapaPagina);

        /// <summary>
        /// Duplica uma página copiando os valores padrão do SGF se necessário
        /// </summary>        
        void DuplicarConfiguracaoEtapaPagina(long seqConfiguracaoEtapaPagina);

        /// <summary>
        /// Busca um VO com os dados de idioma do processo
        /// </summary>        
        IdiomasPaginasProcessoData BuscarIdiomasPaginasProcesso(long seqConfiguracaoEtapa);

        /// <summary>
        /// Adiciona e/ou remove idiomas de TODAS AS Páginas configuradas para uma configuração de etapa
        /// </summary>        
        void AlterarIdiomasPaginas(IdiomasPaginasProcessoData idiomasPagina);

        /// <summary>
        /// Retora a lista de páginas não criadas com o sequencial das páginas no SGF
        /// </summary>        
        List<SMCDatasourceItem> BuscarPaginasNaoCriadas(long seqConfiguracaoEtapa);

        /// <summary>
        /// Adiciona as páginas segundo o modelo do SGF na configuracao etapa passada 
        /// (para todos os idiomas em uso na configuração)
        /// </summary>        
        void AdicionarPaginasConfiguracao(long seqConfiguracaoEtapa, IEnumerable<long> seqPaginasSGF);

        long[] BuscarInscricoesConfiguracaoEtapa(long seqConfiguracaoEtapa);

        /// <summary>
        /// Busca as inscrições para uma determinada configuração etapa pagina idioma.
        /// </summary>
        long[] BuscarInscricoesConfiguracaoPaginaEtapaIdioma(long seqConfiguracaoEtapaPaginaIdioma);        

        /// <summary>
        /// Busca uma configuração etapa página para ser exibida em edição
        /// </summary>        
        ConfiguracaoPaginaData BuscarConfiguracaoEtapaPaginaEdicao(long seqConfiguracaoEtapaPagina);

        /// <summary>
        /// Salva as alterações numa configuração etapa página
        /// </summary>        
        void SalvarConfiguracaoPagina(ConfiguracaoPaginaData configuracaoPagina);

        /// <summary>
        /// Busca uma configuração de página por idioma para edição e/ou exibição
        /// </summary>        
        ConfiguracaoPaginaIdiomaData BuscarConfiguracaoEtapaPaginaIdioma(long seqConfiguracaoEtapaPaginaIdioma);

        /// <summary>
        /// Salva a configuração da página no idioma
        /// </summary>        
        void SalvarConfiguracaoEtapaPaginaIdioma(ConfiguracaoPaginaIdiomaData configuracaoPaginaIdioma);

        /// <summary>
        /// Busca uma seção de texto para edição
        /// </summary>
        /// <param name="seqSecaoTexto"></param>
        /// <returns></returns>
        SecaoPaginaTextoData BuscarSecaoTextoPagina(long seqSecaoTexto); 

        /// <summary>
        /// Salva a alteração no texto da seção de uma página em um idioma
        /// </summary>        
        void SalvarTextoSecao(SecaoPaginaTextoData textoSecao);

        /// <summary>
        /// Busca a lista de arquivos numa seção para edição
        /// </summary>        
        List<ArquivoSecaoData> BuscarArquivosSecaoPagina(ArquivoSecaoFiltroData filtro);

        /// <summary>
        /// Salvar os arquivos de uma seção de uma página em um idioma
        /// </summary>        
        void SalvarArquivosSecaoPagina(long seqConfiguracaoEtapaIdioma, long seqSecaoSGF, List<ArquivoSecaoData> secaoArquivos);

        /// <summary>
        /// Implementa a cópia de configurações de etapa de uma etapa de origem para uma etapa de um processo de destinho
        /// </summary>       
        void CopiarConfiguracoesEtapa(CopiarConfiguracoesEtapaData dadosCopia);


        /// <summary>
        /// Busca o nome de uma configuracação de etapa
        /// </summary>
        string BuscarDescricaoConfiguracaoEtapa(long seqConfiguracaoEtapa);

        IEnumerable<SMCDatasourceItem> BuscarConfiguracoesEtapaKeyValue(ConfiguracaoEtapaFiltroData filtro);
    }
}
