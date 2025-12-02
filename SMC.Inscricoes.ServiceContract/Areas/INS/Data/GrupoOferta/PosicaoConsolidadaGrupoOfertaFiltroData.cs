using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Inscricoes.Common;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    /// <summary>
    /// Specification para filtrar a posiçao consolidada por oferta
    /// Sempre filtra pelo Seq do Processo
    /// Se o filtro de SeqGrupoOferta for setado é utilizado
    /// Se o filtro de SeqOferta for setado é utilizado 
    /// </summary>
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class PosicaoConsolidadaGrupoOfertaFiltroData : SMCPagerFilterData
    {
        [DataMember]
        public long SeqProcesso { get; set; }

        [DataMember]
        public long? SeqGrupoOferta { get; set; }

        [DataMember]
        [SMCMapProperty("SeqItemHierarquiaOferta.Seq")]
        public long? SeqItemHierarquiaOferta { get; set; }

        [DataMember]
        [SMCMapProperty("OpcaoLookupOfertas.Seq")]
        public long? SeqOferta { get; set; }
    }
}
