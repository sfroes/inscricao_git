using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Notificacoes.ServiceContract.Areas.NTF.Data;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{

    public class ConfiguracaoNotificacaoIdiomaVO : ISMCMappable
    {

        public SMCLanguage Idioma { get; set; }
        
        public ConfiguracaoNotificacaoEmailData ConfiguracaoNotificacao { get; set; }
    }
}
