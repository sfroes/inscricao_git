using SMC.Framework.Mapper;
using SMC.Inscricoes.Common.Areas.INS;
using System;
using System.Collections.Generic;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class TotalInscricoesProcessoVO : ISMCMappable
    {

        public long SeqProcesso { get; set; }

        public string Descricao { get; set; }

        public int TotalInscricoes { get; set; }

    }
}
