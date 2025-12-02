using SMC.Framework.Mapper;
using SMC.Inscricoes.ServiceContract.Areas.RES.Data.UnidadeResponsavel;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.RES.Data
{
    [DataContract]
    public class UnidadeResponsavelTipoProcessoData : ISMCMappable
    {
        [DataMember]
        public long Seq { get; set; }

        [DataMember]
        public long SeqUnidadeResponsavel { get; set; }

        [DataMember]
        public long? SeqTipoProcesso { get; set; }

        [DataMember]
        public bool Ativo { get; set; }

        [DataMember]
        public string CssIdentidadeVisual { get; set; }

        [DataMember]
        public string TokenCssAlternativoSas { get; set; }

        [DataMember]
        [SMCMapProperty("TiposHierarquiaOferta")]
        public List<UnidadeResponsavelDetalheTipoHierarquiaOfertaData> DetalhesTiposHierarquiaOferta { get; set; }

        public List<UnidadeResponsavelTipoProcessoIdentidadeVisualData> IdentidadesVisuais { get; set; }


    }
}
