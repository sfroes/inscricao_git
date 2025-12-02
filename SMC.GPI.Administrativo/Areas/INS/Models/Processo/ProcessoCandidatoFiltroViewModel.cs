using SMC.Framework.Mapper;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class ProcessoCandidatoFiltroViewModel : ISMCMappable
    {
        public long? SeqUnidadeResponsavel { get; set; }
        public long? SeqTipoProcesso { get; set; }
    }
}