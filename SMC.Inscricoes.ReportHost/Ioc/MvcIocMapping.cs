using SMC.Formularios.ServiceContract.Areas.FRM.Interfaces;
using SMC.Formularios.ServiceContract.Areas.TMP.Interfaces;
using SMC.Framework;
using SMC.Framework.Pdf;
using SMC.Framework.Rest;
using SMC.Framework.UI.Mvc;
using SMC.Infraestrutura.ServiceContract.Areas.PDF.Interfaces;
using SMC.Inscricoes.ReportHost.App_GlobalResources;
using SMC.Inscricoes.Service.Areas.FRM;
using SMC.Inscricoes.Service.Areas.INS.Services;
using SMC.Inscricoes.Service.Ioc;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;

namespace SMC.Inscricoes.ReportHost.Ioc
{
    public class MvcIocMapping : SMCMvcIocMapping
    {
        protected override void Configure()
        {
            Resources.RegisterMetadata<MetadataResource>().RegisterUI<UIResource>();

            this.Services.Map<ServiceIocMapping>();
            this.MapMock = false;
            this.MapPdfBuilder = false;

            Container.RegisterType<ISMCPdfBuilder, IPdfService>();
            Container.RegisterType<ISMCApiClientFactory, SMCApiClientFactory>();

        }
    }
}