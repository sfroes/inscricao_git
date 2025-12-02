using SMC.Framework.Pdf;
using SMC.Framework.Service;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.Html;
using SMC.GPI.Inscricao.App_GlobalResources;
using SMC.Infraestrutura.ServiceContract.Areas.PDF.Interfaces;
using SMC.Inscricoes.Service.Areas.INS.Services;
using SMC.Inscricoes.Service.Ioc;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using SMC.Localidades.UI.Mvc;
using SMC.Localidades.UI.Mvc.Controls.Phone;

namespace SMC.GPI.Inscricao.Ioc
{
    public class MvcIocMapping : SMCMvcIocMapping
    {
        protected override void Configure()
        {
            this.Services.Map<ServiceIocMapping>();

            this.Resources
                .RegisterMetadata<MetadataResource>()
                .RegisterUI<UIResource>();

            //this.ControllerServices
            //    .Register<InscritoControllerService, InscritoControllerService>()
            //    .Register<ProcessoControllerService, ProcessoControllerService>()
            //    .Register<ILocalidadesControllerService, LocalidadesControllerService>()
            //    .Register<InscricaoControllerService, InscricaoControllerService>();

            Container.ConfigureMap<Framework.Security.Ioc.SMCIocMapping>();

            // Solução de PDF 
            this.Container.RegisterType<ISMCPdfBuilder, IPdfService>();
            this.Container.RegisterServiceClient<IPdfService>();
            //------------------------

            // Componentes externos
            this.Container.RegisterType<ISMCEditorFluentConfiguration, AddressFluentConfiguration>("SMCAddress");
            this.Container.RegisterType<ISMCEditorFluentConfiguration, PhoneFluentConfiguration>("SMCPhone");
            this.Container.RegisterType<ISMCEditorFluentConfiguration, StateCityConfiguration>("SMCStateCity");

            //Configuração de fontes externas ao SGF
            this.Container.RegisterType<IFonteExternaService, FonteExternaService>("LISTA_PROJETO_VINCULADO_INSCRICAO");
            this.Container.RegisterType<IFonteExternaService, FonteExternaService>("LISTA_UNIDADE_ACADEMICO");
            this.Container.RegisterType<IFonteExternaService, FonteExternaService>("LISTA_CURSO_ACADEMICO");


        }
    }
}

