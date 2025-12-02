using SMC.Framework.Mapper;
using System;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class CopiaEtapaProcessoVO : ISMCMappable
    {
        public long SeqEtapa { get; set; }

        public long SeqEtapaSGF { get; set; }

        public bool Copiar { get; set; }

        public string Etapa { get; set; }

        public DateTime DataInicio { get; set; }

        public DateTime DataFim { get; set; }

        public bool CopiarConfiguracoes { get; set; }
    }
}