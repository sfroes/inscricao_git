using SMC.DadosMestres.Common.Constants;
using SMC.Framework.Mapper;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class InscricaoTaxaData : ISMCMappable
    {

        [DataMember]
        public long SeqTaxa { get; set; }

        [DataMember]
        public short NumeroItens { get; set; }

        [DataMember]
        public decimal ValorItem { get; set; }

        [DataMember]
        public long SeqOferta { get; set; }

        [DataMember]
        public TipoCobranca TipoCobranca { get; set; }


    }
}