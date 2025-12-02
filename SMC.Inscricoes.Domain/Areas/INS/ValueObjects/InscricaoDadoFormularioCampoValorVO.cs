using SMC.Framework.Mapper;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class InscricaoDadoFormularioCampoValorVO : ISMCMappable
    {
        public string Valor { get; set; }

        public string Descricao { get; set; }
    }
}