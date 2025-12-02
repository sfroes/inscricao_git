using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class PessoaIntegracaoData : ISMCMappable
    {
        [DataMember]
        public long SeqInscricao { get; set; }

        [DataMember]
        public string TokenNivelEnsino { get; set; }

        [DataMember]
        public long SeqInscrito { get; set; }

        #region SGA: pessoa_dados_pessoais

        [DataMember]
        public long? SeqUsuarioSAS { get; set; }

        [DataMember]
        public string Nome { get; set; }

        [DataMember]
        public string NomeSocial { get; set; }

        [DataMember]
        public string NomePai { get; set; }

        [DataMember]
        public string NomeMae { get; set; }

        [DataMember]
        public Sexo Sexo { get; set; }

        [DataMember]
        public EstadoCivil? EstadoCivil { get; set; }

        [DataMember]
        public string RacaCor { get; set; }

        [DataMember]
        public string UfNaturalidade { get; set; }

        [DataMember]
        public int? CodigoCidadeNaturalidade { get; set; }

        [DataMember]
        public string DescricaoNaturalidadeEstrangeira { get; set; }

        [DataMember]
        public string NumeroIdentidade { get; set; }

        [DataMember]
        public string OrgaoEmissorIdentidade { get; set; }

        [DataMember]
        public string UfIdentidade { get; set; }

        // Dados que são buscados do SGF
        [DataMember]
        public string NumeroTituloEleitor { get; set; }

        [DataMember]
        public string NumeroZonaTituloEleitor { get; set; }

        [DataMember]
        public string NumeroSecaoTituloEleitor { get; set; }

        [DataMember]
        public string UfTituloEleitor { get; set; }

        [DataMember]
        public string TipoPisPasep { get; set; }

        [DataMember]
        public string NumeroPisPasep { get; set; }

        [DataMember]
        public string DataPisPasep { get; set; }

        [DataMember]
        public string NumeroDocumentoMilitar { get; set; }

        [DataMember]
        public string CsmDocumentoMilitar { get; set; }

        [DataMember]
        public string TipoDocumentoMilitar { get; set; }

        [DataMember]
        public string UfDocumentoMilitar { get; set; }

        [DataMember]
        public string NecessidadeEspecial { get; set; }

        [DataMember]
        public string TipoNecessidadeEspecial { get; set; }

        #endregion SGA: pessoa_dados_pessoais

        #region SGA: pessoa

        [DataMember]
        public DateTime DataNascimento { get; set; }

        [DataMember]
        public TipoNacionalidade TipoNacionalidade { get; set; }

        [DataMember]
        public int CodigoPaisNacionalidade { get; set; }

        [DataMember]
        public string Cpf { get; set; }

        [DataMember]
        public string NumeroPassaporte { get; set; }

        [DataMember]
        public DateTime? DataValidadePassaporte { get; set; }

        [DataMember]
        public int? CodigoPaisEmissaoPassaporte { get; set; }

        #endregion SGA: pessoa

        [DataMember]
        public List<OfertaIntegracaoData> Ofertas { get; set; }

        [DataMember]
        public List<EnderecoPessoaIntegracaoData> Enderecos { get; set; }

        [DataMember]
        public List<TelefonePessoaIntegracaoData> Telefones { get; set; }

        [DataMember]
        public List<EnderecoEletronicoPessoaIntegracaoData> EnderecosEletronicos { get; set; }

        [DataMember]
        public List<DocumentosIntegracaoData> Documentos { get; set; }
    }
}