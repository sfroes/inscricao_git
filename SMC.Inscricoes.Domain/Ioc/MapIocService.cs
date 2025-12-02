using SMC.Framework.Ioc;


namespace SMC.Inscricoes.Ioc
{
    /// <summary>
    /// Classe para mapeamento de serviços de outros dominios
    /// </summary>
    public class MapIocService : SMCIocExtension
    {
        /// <summary>
        /// Configurações de ioc
        /// </summary>
        public override void Configure(ISMCIocContainer container)
        {
            // Serviço de segurança para o Authorize
            //container.ConfigureMap<SMC.Framework.Security.Ioc.SMCIocMapping>();
            //container.RegisterFactory<ISMCAuthorizationService, SMCServiceClientFactory<ISMCAuthorizationService>>();

            // Exemplo de inclusão de ioc para serviço de outro domínio.
            //container.RegisterType<IEmailService, EmailService>();

            container.ConfigureMap<SMC.Framework.Transaction.Ioc.SMCIocMapping>();
        }
    }


}
