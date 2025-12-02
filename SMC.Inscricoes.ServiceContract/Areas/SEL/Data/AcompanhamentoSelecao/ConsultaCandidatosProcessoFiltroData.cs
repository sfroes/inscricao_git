using SMC.Framework.Mapper;
using SMC.Framework.Model;

namespace SMC.Inscricoes.ServiceContract.Areas.SEL.Data
{
    public class ConsultaCandidatosProcessoFiltroData : SMCPagerFilterData, ISMCMappable
    {
        public long SeqProcesso { get; set; }

        public long? SeqTipoProcessoSituacao { get; set; }

        public long? SeqMotivo { get; set; }

        public long? SeqGrupoOferta { get; set; }

        public long? SeqItemHierarquiaOferta { get; set; }

        [SMCMapProperty("Oferta")]
        public long? SeqOferta { get; set; }

        public short? Opcao { get; set; }

        public long? SeqInscricao { get; set; }

        public string Candidato { get; set; }
    }
}