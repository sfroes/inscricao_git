using SMC.Formularios.ServiceContract.Areas.TMP.Interfaces;
using SMC.Framework.Extensions;
using SMC.Framework.Model;
using SMC.Framework.Service;
using SMC.Framework.Specification;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Inscricoes.Domain.Areas.INS.DomainServices;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using SMC.Inscricoes.Domain.Areas.INS.ValueObjects;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using System.Collections.Generic;
using System.Linq;

namespace SMC.Inscricoes.Service.Areas.INS.Services
{
    public class ConfiguracaoEtapaService : SMCServiceBase, IConfiguracaoEtapaService
    {
        #region DomainService

        private ConfiguracaoEtapaDomainService ConfiguracaoEtapaDomainService
        {
            get { return this.Create<ConfiguracaoEtapaDomainService>(); }
        }

        private ConfiguracaoEtapaPaginaDomainService ConfiguracaoEtapaPaginaDomainService
        {
            get { return this.Create<ConfiguracaoEtapaPaginaDomainService>(); }
        }

        private ConfiguracaoEtapaPaginaIdiomaDomainService ConfiguracaoEtapaPaginaIdiomaDomainService
        {
            get { return this.Create<ConfiguracaoEtapaPaginaIdiomaDomainService>(); }
        }

        private TextoSecaoPaginaDomainService TextoSecaoPaginaDomainService
        {
            get { return this.Create<TextoSecaoPaginaDomainService>(); }
        }

        private ArquivoSecaoPaginaDomainService ArquivoSecaoPaginaDomainService
        {
            get { return this.Create<ArquivoSecaoPaginaDomainService>(); }
        }

        private IEtapaService EtapaService
        {
            get { return this.Create<IEtapaService>(); }
        }

        #endregion

        public SMCPagerData<ConfiguracaoEtapaListaData> BuscarConfiguracoesEtapa(ConfiguracaoEtapaFiltroData filtro)
        {
            int total = 0;
            var dados = this.ConfiguracaoEtapaDomainService.SearchProjectionBySpecification(
                filtro.Transform<ConfiguracaoEtapaFilterSpecification>(),
                x => new ConfiguracaoEtapaListaData
                {
                    Seq = x.Seq,
                    SeqEtapaProcesso = x.SeqEtapaProcesso,
                    SeqProcesso = x.EtapaProcesso.SeqProcesso,
                    Nome = x.Nome,
                    DataFim = x.DataFim,
                    DataInicio = x.DataInicio
                }, out total);
            return new SMCPagerData<ConfiguracaoEtapaListaData>(dados, total);
        }

        public ConfiguracaoEtapaData BuscarConfiguracaoEtapa(long seqConfiguracaoEtapa)
        {
            return this.ConfiguracaoEtapaDomainService.SearchByKey<ConfiguracaoEtapa, ConfiguracaoEtapaData>(
                            seqConfiguracaoEtapa, IncludesConfiguracaoEtapa.ArquivoImagem |
                            IncludesConfiguracaoEtapa.EtapaProcesso |
                            IncludesConfiguracaoEtapa.GruposOferta
                            | IncludesConfiguracaoEtapa.GruposOferta_GrupoOferta);
        }

        public void ExcluirConfiguracaoEtapa(long seqConfiguracaoEtapa)
        {
            this.ConfiguracaoEtapaDomainService.ExcluirConfiguracaoEtapa(seqConfiguracaoEtapa);
        }

        public long SalvarConfiguracaoEtapa(ConfiguracaoEtapaData configuracaoEtapa)
        {
            return this.ConfiguracaoEtapaDomainService.SalvarConfiguracaoEtapa(
                configuracaoEtapa.Transform<ConfiguracaoEtapa>());
        }

        public CabecalhoProcessoEtapaConfiguracaoData BuscarCabecalhoEtapaProcessoConfiguracao(long seqConfiguracaoEtapa)
        {
            var cabecalho = this.ConfiguracaoEtapaDomainService.SearchProjectionByKey(
                new SMCSeqSpecification<ConfiguracaoEtapa>(seqConfiguracaoEtapa),
                x => new CabecalhoProcessoEtapaConfiguracaoData
                {
                    DescricaoConfiguracaoEtapa = x.Nome,
                    DescricaoProcesso = x.EtapaProcesso.Processo.Descricao,
                    DescricaoTipoProcesso = x.EtapaProcesso.Processo.TipoProcesso.Descricao,
                    SeqConfiguracaoEtapa = x.Seq,
                    SeqEtapaProcesso = x.SeqEtapaProcesso,
                    SeqProcesso = x.EtapaProcesso.SeqProcesso,
                    SeqEtapaProcessoSGF = x.EtapaProcesso.SeqEtapaSGF
                });
            cabecalho.DescricaoEtapaProcesso = this.EtapaService
                .BuscarEtapasKeyValue(new long[] { cabecalho.SeqEtapaProcessoSGF })[0].Descricao;
            return cabecalho;
        }

        public List<NoArvoreConfiguracaoEtapaData> BuscarArvoreConfiguracaoEtapa(long seqConfiguracaoEtapa)
        {
            return ConfiguracaoEtapaDomainService.BuscarArvoreConfiguracaoEtapa(seqConfiguracaoEtapa)
                .TransformList<NoArvoreConfiguracaoEtapaData>();
        }

        /// <summary>
        /// Cria as páginas para uma configuração de etapa de acordo com o template do SGF 
        /// e os idiomas configurados para o processo
        /// </summary>        
        public void CriarPaginasPadraoConfiguracao(long seqConfiguracaoEtapa)
        {
            this.ConfiguracaoEtapaDomainService.CriarPaginasPadraoConfiguracao(seqConfiguracaoEtapa);
        }

        /// <summary>
        /// Verifica se a configuração possui páginas
        /// Retorna true se existirem página e falso caso contrário
        /// </summary>        
        public bool VerificarConfiguracaoPossuiPaginas(long seqConfiguracaoEtapa)
        {
            return this.ConfiguracaoEtapaDomainService.VerificarConfiguracaoPossuiPaginas(seqConfiguracaoEtapa);
        }

        /// <summary>
        /// Exclui uma página de uma configuração de etapa e suas seções filhas para todos os idiomas
        /// </summary>        
        public void ExcluirConfiguracaoEtapaPagina(long seqConfiguracaoEtapaPagina)
        {
            this.ConfiguracaoEtapaPaginaDomainService.ExcluirConfiguracaoEtapaPagina(seqConfiguracaoEtapaPagina);
        }

        /// <summary>
        /// Duplica uma página copiando os valores padrão do SGF se necessário
        /// </summary>        
        public void DuplicarConfiguracaoEtapaPagina(long seqConfiguracaoEtapaPagina)
        {
            this.ConfiguracaoEtapaPaginaDomainService.DuplicarConfiguracaoEtapaPagina(seqConfiguracaoEtapaPagina);
        }

        /// <summary>
        /// Busca um VO com os dados de idioma do processo
        /// </summary>        
        public IdiomasPaginasProcessoData BuscarIdiomasPaginasProcesso(long seqConfiguracaoEtapa)
        {
            return this.ConfiguracaoEtapaDomainService.BuscarIdiomasPaginasProcesso(seqConfiguracaoEtapa)
                .Transform<IdiomasPaginasProcessoData>();
        }

        /// <summary>
        /// Adiciona e/ou remove idiomas de TODAS AS Páginas configuradas para uma configuração de etapa
        /// </summary>        
        public void AlterarIdiomasPaginas(IdiomasPaginasProcessoData idiomasPagina)
        {
            this.ConfiguracaoEtapaDomainService
                .AlterarIdiomasPaginas(idiomasPagina.Transform<IdiomasPaginasProcessoVO>());
        }

        /// <summary>
        /// Retora a lista de páginas não criadas com o sequencial das páginas no SGF
        /// </summary>        
        public List<SMCDatasourceItem> BuscarPaginasNaoCriadas(long seqConfiguracaoEtapa)
        {
            return ConfiguracaoEtapaDomainService.BuscarPaginasNaoCriadas(seqConfiguracaoEtapa);
        }

        /// <summary>
        /// Adiciona as páginas segundo o modelo do SGF na configuracao etapa passada 
        /// (para todos os idiomas em uso na configuração)
        /// </summary>        
        public void AdicionarPaginasConfiguracao(long seqConfiguracaoEtapa, IEnumerable<long> seqPaginasSGF)
        {
            this.ConfiguracaoEtapaDomainService.AdicionarPaginasConfiguracao(seqConfiguracaoEtapa, seqPaginasSGF);
        }

        /// <summary>
        /// Busca uma configuração etapa página para ser exibida em edição
        /// </summary>        
        public ConfiguracaoPaginaData BuscarConfiguracaoEtapaPaginaEdicao(long seqConfiguracaoEtapaPagina)
        {
            return this.ConfiguracaoEtapaPaginaDomainService.
                BuscarConfiguracaoEtapaPaginaEdicao(seqConfiguracaoEtapaPagina)
                .Transform<ConfiguracaoPaginaData>();
        }

        /// <summary>
        /// Salva as alterações numa configuração etapa página
        /// </summary>        
        public void SalvarConfiguracaoPagina(ConfiguracaoPaginaData configuracaoPagina)
        {
            this.ConfiguracaoEtapaPaginaDomainService.SalvarConfiguracaoPagina(
                configuracaoPagina.Transform<ConfiguracaoPaginaVO>());
        }

        /// <summary>
        /// Busca uma configuração de página por idioma para edição e/ou exibição
        /// </summary>        
        public ConfiguracaoPaginaIdiomaData BuscarConfiguracaoEtapaPaginaIdioma(long seqConfiguracaoEtapaPaginaIdioma)
        {
            return this.ConfiguracaoEtapaPaginaIdiomaDomainService
                 .BuscarConfiguracaoEtapaPaginaIdioma(seqConfiguracaoEtapaPaginaIdioma)
                 .Transform<ConfiguracaoPaginaIdiomaData>();
        }

        /// <summary>
        /// Salva a configuração da página no idioma
        /// </summary>        
        public void SalvarConfiguracaoEtapaPaginaIdioma(ConfiguracaoPaginaIdiomaData configuracaoPaginaIdioma)
        {
            this.ConfiguracaoEtapaPaginaIdiomaDomainService.SalvarConfiguracaoEtapaPaginaIdioma(
                configuracaoPaginaIdioma.Transform<ConfiguracaoPaginaIdiomaVO>());
        }

        public SecaoPaginaTextoData BuscarSecaoTextoPagina(long seqSecaoTexto) 
        {
            return this.TextoSecaoPaginaDomainService
                .SearchByKey<TextoSecaoPagina, SecaoPaginaTextoData>(seqSecaoTexto);
        }

        /// <summary>
        /// Salva a alteração no texto da seção de uma página em um idioma
        /// </summary>        
        public void SalvarTextoSecao(SecaoPaginaTextoData textoSecao) 
        {
            this.TextoSecaoPaginaDomainService.SalvarTextoSecao(textoSecao.Transform<TextoSecaoPagina>());
        }

        /// <summary>
        /// Busca a lista de arquivos numa seção para edição
        /// </summary>        
        public List<ArquivoSecaoData> BuscarArquivosSecaoPagina(ArquivoSecaoFiltroData filtro) 
        {
            return this.ArquivoSecaoPaginaDomainService.
                SearchBySpecification(filtro.Transform<ArquivoSecaoPaginaFilterSpecification>(),
                x => x.Arquivo).TransformList<ArquivoSecaoData>();
        }

        /// <summary>
        /// Salvar os arquivos de uma seção de uma página em um idioma
        /// </summary>        
        public void SalvarArquivosSecaoPagina(long seqConfiguracaoEtapaIdioma, long seqSecaoSGF, List<ArquivoSecaoData> secaoArquivos) 
        {
            this.ArquivoSecaoPaginaDomainService.SalvarArquivosSecaoPagina(seqConfiguracaoEtapaIdioma, seqSecaoSGF,
                secaoArquivos.TransformList<ArquivoSecaoPagina>());
        }

        /// <summary>
        /// Implementa a cópia de configurações de etapa de uma etapa de origem para uma etapa de um processo de destinho
        /// </summary>       
        public void CopiarConfiguracoesEtapa(CopiarConfiguracoesEtapaData dadosCopia)
        {
            this.ConfiguracaoEtapaDomainService.CopiarConfiguracoesEtapa(
                dadosCopia.Transform<CopiarConfiguracoesEtapaVO>());
        }


        /// <summary>
        /// Busca o nome de uma configuracação de etapa
        /// </summary>
        public string BuscarDescricaoConfiguracaoEtapa(long seqConfiguracaoEtapa) 
        {
            return this.ConfiguracaoEtapaDomainService.SearchProjectionByKey(
                new SMCSeqSpecification<ConfiguracaoEtapa>(seqConfiguracaoEtapa),
                x => x.Nome);
        }
        
        public IEnumerable<SMCDatasourceItem> BuscarConfiguracoesEtapaKeyValue(ConfiguracaoEtapaFiltroData filtro) 
        {
            return this.ConfiguracaoEtapaDomainService.SearchProjectionBySpecification(
                filtro.Transform<ConfiguracaoEtapaFilterSpecification>(),
                x => new SMCDatasourceItem
                {
                    Seq = x.Seq,
                    Descricao = x.Nome
                });
        }

        public long[] BuscarInscricoesConfiguracaoEtapa(long seqConfiguracaoEtapa)
        {
            return ConfiguracaoEtapaDomainService.SearchProjectionByKey(new SMCSeqSpecification<ConfiguracaoEtapa>(seqConfiguracaoEtapa),
                                x => x.Inscricoes.Select(f => f.Seq)).ToArray();
        }

        public long[] BuscarInscricoesConfiguracaoPaginaEtapaIdioma(long seqConfiguracaoEtapaPaginaIdioma)
        {
            return this.ConfiguracaoEtapaPaginaIdiomaDomainService.SearchProjectionByKey(
                        new SMCSeqSpecification<ConfiguracaoEtapaPaginaIdioma>(seqConfiguracaoEtapaPaginaIdioma),
                                x => x.ConfiguracaoEtapaPagina.ConfiguracaoEtapa.Inscricoes.Select(f => f.Seq)).ToArray();
        }
    }
}
