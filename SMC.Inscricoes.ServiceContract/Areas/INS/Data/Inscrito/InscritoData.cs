using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Service.Data;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class InscritoData : ISMCMappable
    {
        [DataMember]
        public long Seq { get; set; }

        [DataMember]
        public long SeqUsuarioSas { get; set; }

        [DataMember]
        public string Nome { get; set; }

        [DataMember]
        public string NomeSocial { get; set; }

        [DataMember]
        public DateTime DataNascimento { get; set; }

        [DataMember]
        public Sexo Sexo { get; set; }

        [DataMember]
        public EstadoCivil? EstadoCivil { get; set; }

        [DataMember]
        public string Cpf { get; set; }

        [DataMember]
        public string NumeroPassaporte { get; set; }

        [DataMember]
        public DateTime? DataValidadePassaporte { get; set; }

        [DataMember]
        public int? CodigoPaisEmissaoPassaporte { get; set; }

        [DataMember]
        public string NumeroIdentidade { get; set; }

        [DataMember]
        public string OrgaoEmissorIdentidade { get; set; }

        [DataMember]
        public string UfIdentidade { get; set; }

        [DataMember]
        public TipoNacionalidade Nacionalidade { get; set; }

        [DataMember]
        public int CodigoPaisNacionalidade { get; set; }

        [DataMember]
        [SMCMapProperty("EstadoCidade.Estado")]
        [SMCMapForceFromTo]
        public string UfNaturalidade { get; set; }

        [DataMember]
        [SMCMapProperty("EstadoCidade.SeqCidade")]
        [SMCMapForceFromTo]
        public int? CodigoCidadeNaturalidade { get; set; }

        [DataMember]
        public string DescricaoNaturalidadeEstrangeira { get; set; }

        [DataMember]
        public string NomePai { get; set; }

        [DataMember]
        public string NomeMae { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public List<EnderecoData> Enderecos { get; set; }

        [DataMember]
        public List<EnderecoEletronicoData> EnderecosEletronicos { get; set; }

        [DataMember]
        [SMCMapForceFromTo]
        public List<TelefoneData> Telefones { get; set; }

        [DataMember]
        public bool ConsentimentoLGPD { get; set; }

        [DataMember]
        public Guid? UidProcesso { get; set; }

    }
}
