using SMC.Formularios.UI.Mvc.Attributes;
using SMC.Formularios.UI.Mvc.Controls;
using SMC.Framework.Pdf;
using SMC.Framework.Service;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.Html;
using SMC.GPI.Administrativo.App_GlobalResources;
using SMC.Infraestrutura.ServiceContract.Areas.PDF.Interfaces;
using SMC.Inscricoes.Service.Ioc;
using SMC.Localidades.UI.Mvc;
using SMC.Localidades.UI.Mvc.Controls.Phone;

namespace SMC.GPI.Administrativo.Ioc
{
    public class MvcIocMapping : SMCMvcIocMapping
    {
        protected override void Configure()
        {
            MapMock = false;

            this.Services.Map<ServiceIocMapping>();

            this.Resources
                .RegisterMetadata<MetadataResource>()
                .RegisterUI<UIResource>();

            //this.ControllerServices
            //    .Register<TipoProcessoControllerService, TipoProcessoControllerService>()
            //    .Register<IClienteControllerService, ClienteControllerService>()
            //    .Register<UnidadeResponsavelControllerService, UnidadeResponsavelControllerService>()
            //    .Register<GrupoOfertaControllerService, GrupoOfertaControllerService>()
            //    .Register<IAcompanhamentoProcessoControllerService, AcompanhamentoProcessoControllerService>()
            //    .Register<TipoHierarquiaOfertaControllerService, TipoHierarquiaOfertaControllerService>()
            //    .Register<TipoDocumentoControllerService, TipoDocumentoControllerService>()
            //    .Register<HierarquiaOfertaControllerService, HierarquiaOfertaControllerService>()
            //    .Register<EtapaProcessoControllerService, EtapaProcessoControllerService>()
            //    .Register<IConfiguracaoEtapaControllerService, ConfiguracaoEtapaControllerService>()
            //    .Register<InscritoControllerService, InscritoControllerService>()
            //    .Register<ProcessoControllerService, ProcessoControllerService>()
            //    .Register<IConfiguracaoEtapaPaginaControllerService, ConfiguracaoEtapaPaginaControllerService>()
            //    .Register<DocumentoRequeridoControllerService, DocumentoRequeridoControllerService>()
            //    .Register<GrupoDocumentoRequeridoControllerService, GrupoDocumentoRequeridoControllerService>()
            //    .Register<ConsultaNotificacaoControllerService, ConsultaNotificacaoControllerService>()
            //    .Register<TipoNotificacaoControllerService, TipoNotificacaoControllerService>()
            //    .Register<ConfigurarNotificacaoControllerService, ConfigurarNotificacaoControllerService>()
            //    .Register<IArquivoControllerService, ArquivoControllerService>()
            //    .Register<InscricaoForaPrazoControllerService, InscricaoForaPrazoControllerService>();

            // Solução de PDF
            this.Container.RegisterServiceClient<IPdfService>();
            this.Container.RegisterType<ISMCPdfBuilder, IPdfService>();

            // Excel - OpenXMLService
            this.Container.RegisterServiceClient<SMC.Framework.OpenXml.ISMCWorkbookService>();

            //------------------------

            // Componentes externos
            this.Container.RegisterType<ISMCEditorFluentConfiguration, AddressFluentConfiguration>("SMCAddress");
            this.Container.RegisterType<ISMCEditorFluentConfiguration, PhoneFluentConfiguration>("SMCPhone");
            this.Container.RegisterType<ISMCEditorFluentConfiguration, StateCityConfiguration>("SMCStateCity");

            this.Container.RegisterType<ISMCEditorFluentConfiguration, SGFFluentConfiguration>(SMCSGFAttribute.KEY_SGF_IOC);
            this.Container.RegisterType<ISMCEditorFluentConfiguration, SGFFilterFluentConfiguration>(SMCSGFFilterAttribute.KEY_SGF_IOC_EDITORFOR_FILTER);
        }
    }
}