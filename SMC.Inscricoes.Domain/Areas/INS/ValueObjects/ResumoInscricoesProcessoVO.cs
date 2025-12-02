using SMC.Framework.Mapper;
using SMC.Inscricoes.Common.Areas.INS;
using System;
using System.Collections.Generic;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class ResumoInscricoesProcessoVO : ISMCMappable
    {
        public string Descricao { get; set; }
        public int Total { get; set; }
        public int InscricoesIniciadas { get; set; }
        public int InscricoesFinalizadas { get; set; }
        public int InscricoesCanceladas { get; set; }
        public int InscricoesConfirmadas { get; set; }
        public int InscricoesDeferidas { get; set; }
        public int InscricoesIndeferidas { get; set; }

    }
}
