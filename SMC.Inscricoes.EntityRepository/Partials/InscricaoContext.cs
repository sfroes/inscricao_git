using SMC.Framework.Entity;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.RES.Models;
using SMC.Inscricoes.Domain.Areas.NOT.Models;
using SMC.Inscricoes.Domain.Models;
using SMC.Inscricoes.Domain.Areas.INS.CustomFilters;
using SMC.Inscricoes.Domain.Areas.SEL.Models;

namespace SMC.Inscricoes.EntityRepository
{
    /// <summary>
    /// Classe de contexto do EF
    /// </summary>
    public partial class InscricaoContext
    {
        /// <summary>
        /// Configuração do modelo
        /// </summary>
        /// <param name="config"></param>
        protected override void Configure(SMCDbContextConfiguration config)
        {
            #region Filtros

            config.Filter<UnidadeResponsavel>(FILTERS.UNIDADE_RESPONSAVEL, true)
                         .Associated<CodigoAutorizacao, long>(i => i.SeqUnidadeResponsavel)
                         .Associated<Processo, long>(i => i.SeqUnidadeResponsavel)
                         .Associated<TipoProcesso, TipoProcessoCustomFilter>();

            #endregion

            #region INS

            // Inscrito
            config.Register<Inscrito>()
                  .OwnedCollection<Telefone>(i => i.Telefones)
                  .OwnedCollection<Endereco>(i => i.Enderecos)
                  .OwnedCollection<EnderecoEletronico>(i => i.EnderecosEletronicos);

            // Inscricao
            config.Register<Inscricao>()
                  .OwnedCollection(i => i.Documentos)
                  .OwnedCollection(i => i.EnvioNotificacoes)
                  .OwnedCollection(i => i.HistoricosSituacao)
                  .OwnedCollection(i => i.Ofertas)
                  .OwnedCollection(i => i.Boletos)
                  .OwnedCollection(i => i.Formularios)
                  .OwnedCollection(i => i.CodigosAutorizacao)
                  .OwnedCollection(i => i.HistoricosPagina)
                  .OwnedEntity<ArquivoAnexado>(i => i.ArquivoComprovante)
                  .AssociatedEntity(i => i.Processo);

            // InscricaoDadoFormulario
            config.Register<InscricaoDadoFormulario>()
                .OwnedCollection<InscricaoDadoFormularioCampo>(i => i.DadosCampos);

            // InscricaoOferta
            config.Register<InscricaoOferta>()
                .OwnedCollection(i => i.HistoricosSituacao)
                .AssociatedEntity(i => i.Oferta);

            // InscricaoDocumento
            config.Register<InscricaoDocumento>()
                  .OwnedEntity<ArquivoAnexado>(i => i.ArquivoAnexado);

            config.Register<InscricaoBoleto>()
                .OwnedCollection<InscricaoBoletoTaxa>(x => x.Taxas)
                .OwnedCollection<InscricaoBoletoTitulo>(x => x.Titulos);

            config.Register<TipoHierarquiaOferta>()
                .OwnedCollection<ItemHierarquiaOferta>(x => x.Itens);

            config.Register<Processo>()
                .OwnedCollection<Telefone>(x => x.Telefones)
                .OwnedCollection<EnderecoEletronico>(x => x.EnderecosEletronicos)
                .OwnedCollection<Taxa>(x => x.Taxas)
                .OwnedCollection<ProcessoIdioma>(x => x.Idiomas)
                .OwnedCollection(x => x.GruposOferta)
                .OwnedCollection(x => x.GruposTaxa)
                .OwnedCollection(x => x.HierarquiasOferta)
                .OwnedCollection(x => x.ConfiguracoesFormulario)
                .OwnedCollection(x => x.ConfiguracoesModeloDocumento)
                .OwnedCollection(x => x.CamposInscrito);

            config.Register<GrupoTaxa>()
                  .OwnedCollection<GrupoTaxaItem>(x => x.Itens);

            config.Register<TipoProcesso>()
                .OwnedCollection<TipoProcessoSituacao>(x => x.Situacoes)
                .OwnedCollection<TipoProcessoTemplate>(x => x.Templates)
                .OwnedCollection<TipoProcessoTipoTaxa>(x => x.TiposTaxa)
                .OwnedCollection(x => x.Consistencias)
                .OwnedCollection(x => x.Documentos)
                .OwnedCollection(x => x.CamposInscrito);

            config.Register<Oferta>()
                .OwnedCollection<OfertaPeriodoTaxa>(x => x.Taxas)
                .OwnedCollection<Telefone>(x => x.Telefones)
                .OwnedCollection<EnderecoEletronico>(x => x.EnderecosEletronicos)
                .OwnedCollection<OfertaCodigoAutorizacao>(x => x.CodigosAutorizacao);

            config.Register<HierarquiaOferta>()
                .OwnedCollection<HierarquiaOferta>(x => x.HierarquiasOfertaFilhas, SchemeMapping.Delete);

            config.Register<ConfiguracaoEtapa>()
                .OwnedCollection<GrupoOfertaConfiguracaoEtapa>(x => x.GruposOferta)
                .OwnedEntity<ArquivoAnexado>(x => x.ArquivoImagem);

            config.Register<ConfiguracaoEtapaPagina>();

            config.Register<ConfiguracaoEtapaPaginaIdioma>()
                .OwnedCollection<ArquivoSecaoPagina>(x => x.Arquivos);

            config.Register<GrupoDocumentoRequerido>()
                .OwnedCollection<GrupoDocumentoRequeridoItem>(x => x.Itens);

            config.Register<GrupoTaxa>()
                .OwnedCollection<GrupoTaxaItem>(x => x.Itens);

            config.Register<ArquivoSecaoPagina>()
                .OwnedEntity<ArquivoAnexado>(x => x.Arquivo);

            config.Register<PermissaoInscricaoForaPrazo>()
                .OwnedCollection(x => x.Inscritos)
                .OwnedCollection(x => x.OfertaPeriodoTaxas);

            config.Register<InscricaoHistoricoSituacao>()
                .AssociatedEntity(i => i.TipoProcessoSituacao);
            #endregion

            #region RES

            config.Register<UnidadeResponsavel>()
                 .OwnedCollection<Telefone>(u => u.Telefones)
                 .OwnedCollection<Endereco>(u => u.Enderecos)
                 .OwnedCollection<EnderecoEletronico>(u => u.EnderecosEletronicos)
                 .OwnedCollection<UnidadeResponsavelTipoProcesso>(u => u.TiposProcesso)
                 .OwnedCollection<UnidadeResponsavelTipoFormulario>(u => u.TiposFormulario)
                 .OwnedCollection<UnidadeResponsavelCentroCusto>(u => u.CentrosCusto);

            config.Register<Cliente>()
                .OwnedCollection<Telefone>(u => u.Telefones)
                .OwnedCollection<Endereco>(u => u.Enderecos)
                .OwnedCollection<EnderecoEletronico>(u => u.EnderecosEletronicos);

            config.Register<UnidadeResponsavelTipoProcesso>()
                .OwnedCollection<UnidadeResponsavelTipoHierarquiaOferta>(u => u.TiposHierarquiaOferta)
                .OwnedCollection<UnidadeResponsavelTipoProcessoIdVisual>(u => u.IdentidadesVisuais);

            #endregion

            #region NOT
            config.Register<TipoNotificacao>()
                .OwnedCollection<TipoNotificacaoAtributoAgendamento>(u => u.AtributosAgendamento);

            config.Register<ProcessoConfiguracaoNotificacao>()
                .OwnedCollection<ParametroEnvioNotificacao>(u => u.ParametrosEnvioNotificacao)
                .OwnedCollection<ProcessoConfiguracaoNotificacaoIdioma>(u => u.ConfiguracoesIdioma);
            #endregion
        }

    }
}
