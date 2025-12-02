using SMC.Framework.Service;
using SMC.Inscricoes.Service.Ioc;

namespace SMC.Inscricoes.ServiceHost.Ioc
{
    public class ServiceHostIocMapping : SMCServiceHostIocMapping
    {
        protected override void Configure()
        {
            this.Services
                .Map<ServiceIocMapping>();
        }
    }
}