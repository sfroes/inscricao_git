using SMC.Framework.Mapper;
using System.Runtime.Serialization;
using SMC.Inscricoes.Common;
using SMC.Localidades.Common.Areas.LOC.Enums;

namespace SMC.Inscricoes.Service.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class EnderecoData : ISMCMappable
    {

        [DataMember]
        [SMCMapProperty("SeqEndereco")]
        public long Seq { get; set; }

        [DataMember]
        public string Cep { get; set; }

        [DataMember]
        public string Logradouro { get; set; }

        [DataMember]
        public string Numero { get; set; }

        [DataMember]
        public string Complemento { get; set; }

        [DataMember]
        public string Bairro { get; set; }

        [DataMember]
        [SMCMapProperty("SeqCidade")]
        public int? CodigoCidade { get; set; }

        [DataMember]        
        public string NomeCidade { get; set; }

        [DataMember]
        [SMCMapProperty("SiglaUf")]
        public string Uf { get; set; }

        [DataMember]
        [SMCMapProperty("SeqPais")]
        public int CodigoPais { get; set; }

        [DataMember]
        public TipoEndereco TipoEndereco { get; set; }

        [DataMember]
        public bool? Correspondencia { get; set; }
    }
}
