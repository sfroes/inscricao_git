using SMC.Framework.Mapper;
using System;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class AcompanhamentoCheckinFiltroVO :  ISMCMappable
    {
        public long? SeqProcesso { get; set; }
        public long? SeqUnidadeResponsavel { get; set; }
        public long? SeqTipoProcesso { get; set; }
        public long? SemestreReferencia { get; set; }
        public int? AnoReferencia { get; set; }
        public long? SeqItemHierarquiaOferta { get; set; }
        public long? SeqOferta { get; set; }
        public DateTime? Data { get; set; }
    }
}
