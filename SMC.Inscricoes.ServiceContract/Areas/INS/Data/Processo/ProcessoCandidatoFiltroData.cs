using SMC.Framework.Mapper;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    public class ProcessoCandidatoFiltroData : ISMCMappable
    {
        public long? SeqUnidadeResponsavel { get; set; }
        public long? SeqTipoProcesso { get; set; }
        public bool? GestaoEventos { get; set; }
    }
}
