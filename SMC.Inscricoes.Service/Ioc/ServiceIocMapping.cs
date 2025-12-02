using SMC.AssinaturaDigital.Service.Areas.CAD.Services;
using SMC.AssinaturaDigital.ServiceContract.Areas.CAD.Interfaces;
using SMC.EstruturaOrganizacional.ServiceContract.Areas.ESO.Interfaces;
using SMC.Financeiro.Service.FIN;
using SMC.Financeiro.ServiceContract.BLT;
using SMC.Formularios.ServiceContract.Areas.FRM.Interfaces;
using SMC.Formularios.ServiceContract.Areas.TMP.Interfaces;
using SMC.Framework.DataFilters;
using SMC.Framework.Domain.DataFilters;
using SMC.Framework.Service;
using SMC.Inscricoes.Domain.DomainService;
using SMC.Inscricoes.EntityRepository.Ioc;
using SMC.Inscricoes.Service.Areas.FRM;
using SMC.Inscricoes.Service.Areas.INS.Services;
using SMC.Inscricoes.Service.Areas.NOT.Services;
using SMC.Inscricoes.Service.Areas.RES.Services;
using SMC.Inscricoes.Service.Areas.SEL.Services;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using SMC.Inscricoes.ServiceContract.Areas.NOT.Interfaces;
using SMC.Inscricoes.ServiceContract.Areas.RES.Interfaces;
using SMC.Inscricoes.ServiceContract.Areas.SEL.Interfaces;
using SMC.IntegracaoAcademico.ServiceContract.Areas.IAC.Interfaces;
using SMC.Localidades.ServiceContract.Areas.LOC.Interfaces;
using SMC.Notificacoes.ServiceContract.Areas.NTF.Interfaces;
using SMC.Pessoas.ServiceContract.Areas.PES.Interfaces;
using SMC.Seguranca.ServiceContract.Areas.APL.Interfaces;
using SMC.Seguranca.ServiceContract.Areas.USU.Interfaces;

namespace SMC.Inscricoes.Service.Ioc
{
    /// <summary>
    /// Classe para mapeamento de serviços de outros dominios
    /// </summary>
    public class ServiceIocMapping : SMCServiceIocMapping
    {
        /// <summary>
        /// Configurações de ioc
        /// </summary>
        protected override void Configure()
        {
            // Serviços de Segurança
            this.Services
                .Register<IUsuarioService>()
                .Register<IAplicacaoService>()
                .Register<IFuncionalidadeService>()
                .Register<ISMCDataFilterService>()
                .Register<IFinanceiroService>()
                .Register<IIntegracaoFinanceiroService>()
                .Register<INotificacaoService>();

            this.Container.RegisterType<ISMCDataFilterProviderDomainService, DataFilterProviderDomainService>();

            // Serviços de Localidade
            this.Services
                .Register<ILocalidadeService>()
                .Register<ILocalidadeService>("BUSCAR_ESTADOS_BRASIL")
                .Register<ILocalidadeService>("BUSCAR_CIDADES_ESTADO");

            // Serviços de Formulário
            this.Services
                .Register<IElementoService>()
                .Register<IValorElementoService, ValorElementoService>()
                .Register<IFormularioService>()
                .Register<Formularios.ServiceContract.Areas.FRM.Interfaces.IUnidadeResponsavelService>()
                .Register<ISituacaoService>()
                .Register<ITipoTemplateProcessoService>()
                .Register<IEtapaService>()
                .Register<IPaginaService>()
                .Register<ITemplateProcessoService>();

            // Serviços do Dados mestres
            this.Services
                .Register<DadosMestres.ServiceContract.Areas.GED.Interfaces.ITipoDocumentoService>()
                .Register<DadosMestres.ServiceContract.Areas.GED.Interfaces.IContextoBibliotecaService>()
                .Register<DadosMestres.ServiceContract.Areas.GED.Interfaces.IHierarquiaClassificacaoService>()
                .Register<DadosMestres.ServiceContract.Areas.PES.Interfaces.IIntegracaoDadoMestreService>();

            // Serviços do estrutura organizacional
            this.Services
                .Register<IEstruturaOrganizacionalService>();

            // Serviços do integração acadêmico
            this.Services
                .Register<IIntegracaoAcademicoService>();

            // Serviços do pessoa
            this.Services
                .Register<IPessoaService>();

            // Serviços do assinatura digital
            this.Services
                .Register<ISistemaOrigemService>();

            // Serviços do Inscricao
            this.Services
                 .Register<IInscritoService, InscritoService>()
                 .Register<IProcessoService, ProcessoService>()
                 .Register<IGrupoOfertaService, GrupoOfertaService>()
                 .Register<IGrupoTaxaService, GrupoTaxaService>()
                 .Register<IOfertaService, OfertaService>()
                 .Register<ITipoProcessoService, TipoProcessoService>()
                 .Register<ITipoHierarquiaOfertaService, TipoHierarquiaOfertaService>()
                 .Register<IInscricaoService, InscricaoService>()
                 .Register<ServiceContract.Areas.RES.Interfaces.IUnidadeResponsavelService, UnidadeResponsavelService>()
                 .Register<IClienteService, ClienteService>()
                 .Register<IDocumentoRequeridoService, DocumentoRequeridoService>()
                 .Register<ITipoHierarquiaOfertaService, TipoHierarquiaOfertaService>()
                 .Register<IConfiguracaoEtapaService, ConfiguracaoEtapaService>()
                 .Register<IEtapaProcessoService, EtapaProcessoService>()
                 .Register<SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces.ITipoDocumentoService, SMC.Inscricoes.Service.Areas.INS.Services.TipoDocumentoService>()
                 .Register<IArquivoAnexadoService, ArquivoAnexadoService>()
                 .Register<IConsultaNotificacaoService, ConsultaNotificacaoService>()
                 .Register<ServiceContract.Areas.NOT.Interfaces.ITipoNotificacaoService, TipoNotificacaoService>()
                 .Register<IConfigurarNotificacaoService, ConfigurarNotificacaoService>()
                 .Register<IInscricaoForaPrazoService, InscricaoForaPrazoService>()
                 .Register<ITipoTaxaService, TipoTaxaService>()
                 .Register<ITipoItemHierarquiaOfertaService, TipoItemHierarquiaOfertaService>()
                 .Register<ICodigoAutorizacaoService, CodigoAutorizacaoService>()
                 .Register<IINSDynamicService, INSDynamicService>()
                 .Register<ISELDynamicService, SELDynamicService>()
                 .Register<ISelecaoService, SelecaoService>()
                 .Register<IHierarquiaOfertaService, HierarquiaOfertaService>()
                 .Register<IItemHierarquiaOfertaService, ItemHierarquiaOfertaService>()
                 .Register<IInscricaoOfertaHistoricoSituacaoService, InscricaoOfertaHistoricoSituacaoService>()
                 .Register<IInscricaoDocumentoService, InscricaoDocumentoService>()
                 .Register<IFonteExternaService, FonteExternaService>()
                 .Register<IAcompanhamentoInscritoService, AcompanhamentoInscritoService>()
                 .Register<IProcessoCampoInscritoService, ProcessoCampoInscritoService>()
                 .Register<ITipoProcessoCampoInscritoService, TipoProcessoCampoInscritoService>()
                 .Register<IInscricaoOfertaService, InscricaoOfertaService>()
                 .Register<IViewEventoSaeService, ViewEventoSaeService>();

            // Serviços de Relatórios
            this.Services
                .Register<IInscritoAtividadeRelatorioService, InscritoAtividadeRelatorioService>();

            this.Repositories
                .Map<EntityIocMapping>();
        }
    }
}

