using SMC.Framework.Mapper;
using System;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class InscricaoDadoFormularioCampoVO : ISMCMappable
    {

        public long Seq { get; set; }
        public long SeqInscricaoDadoFormulario { get; set; }
        public long SeqElemento { get; set; }
        public Guid? UidCorrelacao { get; set; }
        public string Valor { get; set; }
        public string Token { get; set; }
    }
}
