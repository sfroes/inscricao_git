using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using SMC.Localidades.Common.Areas.LOC.Enums;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class EnderecoPessoaIntegracaoData : ISMCMappable
    {
        [DataMember]
        public TipoEndereco TipoEndereco { get; set; }

        [DataMember]
        public int CodigoPais { get; set; }

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
        public int? CodigoCidade { get; set; }

        [DataMember]
        public string NomeCidade { get; set; }

        [DataMember]
        public string SiglaUf { get; set; }

        [DataMember]
        public bool? Correspondencia { get; set; }
    }
}
