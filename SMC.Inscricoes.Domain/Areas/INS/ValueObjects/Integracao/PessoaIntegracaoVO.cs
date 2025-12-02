using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class PessoaIntegracaoVO : ISMCMappable
    {
        public long SeqInscricao { get; set; }
        
        public string TokenNivelEnsino { get; set; }
        
        public long SeqInscrito { get; set; }

        #region SGA: pessoa_dados_pessoais
        
        public long? SeqUsuarioSAS { get; set; }
        
        public string Nome { get; set; }

        public string NomeSocial { get; set; }
        
        public string NomePai { get; set; }
        
        public string NomeMae { get; set; }
        
        public string Email { get; set; }
        
        public Sexo? Sexo { get; set; }
        
        public EstadoCivil? EstadoCivil { get; set; }
        
        public string RacaCor { get; set; }
        
        public string UfNaturalidade { get; set; }
        
        public int? CodigoCidadeNaturalidade { get; set; }
        
        public string DescricaoNaturalidadeEstrangeira { get; set; }
        
        public string NumeroIdentidade { get; set; }
        
        public string OrgaoEmissorIdentidade { get; set; }
        
        public string UfIdentidade { get; set; }
        
        public string NumeroTituloEleitor { get; set; }
        
        public string NumeroZonaTituloEleitor { get; set; }
        
        public string NumeroSecaoTituloEleitor { get; set; }

        public string UfTituloEleitor { get; set; }
        
        public string TipoPisPasep { get; set; }
        
        public string NumeroPisPasep { get; set; }
        
        public string DataPisPasep { get; set; }
        
        public string NumeroDocumentoMilitar { get; set; }
        
        public string CsmDocumentoMilitar { get; set; }
        
        public string TipoDocumentoMilitar { get; set; }
        
        public string UfDocumentoMilitar { get; set; }
        
        public string NecessidadeEspecial { get; set; }
        
        public string TipoNecessidadeEspecial { get; set; }

        #endregion SGA: pessoa_dados_pessoais

        #region SGA: pessoa
        
        public DateTime DataNascimento { get; set; }
        
        public TipoNacionalidade TipoNacionalidade { get; set; }
        
        public int? CodigoPaisNacionalidade { get; set; }
        
        public string Cpf { get; set; }
        
        public string NumeroPassaporte { get; set; }
        
        public DateTime? DataValidadePassaporte { get; set; }

        public int? CodigoPaisEmissaoPassaporte { get; set; }

        #endregion SGA: pessoa

        public List<OfertaIntegracaoVO> Ofertas { get; set; }
        
        public List<EnderecoPessoaIntegracaoVO> Enderecos { get; set; }
        
        public List<TelefonePessoaIntegracaoVO> Telefones { get; set; }
        
        public List<EnderecoEletronicoPessoaIntegracaoVO> EnderecosEletronicos { get; set; }
        
        public List<DocumentosIntegracaoVO> Documentos { get; set; }
    }
}
