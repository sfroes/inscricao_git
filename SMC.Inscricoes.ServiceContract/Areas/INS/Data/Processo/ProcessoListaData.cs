using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Inscricoes.Common;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    /// <summary>
    /// Dados do processo resumidos
    /// </summary>
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class ProcessoListaData : ISMCMappable
    {
        [DataMember]
        public long Seq { get; set; }

        [DataMember]
        public string Descricao { get; set; }

        [DataMember]
        public string DescricaoTipoProcesso { get; set; }

        [DataMember]
        public int AnoReferencia { get; set; }

        [DataMember]
        public int SemestreReferencia { get; set; }
        
    }
}
