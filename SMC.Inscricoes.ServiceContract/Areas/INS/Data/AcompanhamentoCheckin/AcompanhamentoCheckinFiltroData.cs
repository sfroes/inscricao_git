using SMC.Framework.Mapper;
using SMC.Framework.Model;
using System;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    public class AcompanhamentoCheckinFiltroData : SMCPagerFilterData, ISMCMappable
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