using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class CopiaProcessoRetornoVO : ISMCMappable
    {
        public long SeqProcessoOrigem { get; set; }

        public Dictionary<long, long?> ProcessosGpi { get; set; }

        public Dictionary<long, long?> ItensOfertasHierarquiasOfertas { get; set; }

    }
}