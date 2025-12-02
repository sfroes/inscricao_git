using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class TotalInscricoesProcessoData : ISMCMappable
    {
        [DataMember]
        public long SeqProcesso { get; set; }

        [DataMember]
        public string Descricao { get; set; }

        [DataMember]
        public int TotalInscricoes { get; set; }

    }
}
