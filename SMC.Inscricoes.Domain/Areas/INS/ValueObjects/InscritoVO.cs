using SMC.Framework;
using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Inscricoes.Service.Data;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    /// <summary>
    /// Value Objec para representar uma inscrição em um processo (usado em InscricoesProcessoVO)
    /// </summary>
    public class InscritoVO : ISMCMappable
    { 
        public long Seq { get; set; }
        public long SeqUsuarioSas { get; set; }
        public string Nome { get; set; }
        public string NomeSocial { get; set; }
        public DateTime DataNascimento { get; set; }
        public Sexo Sexo { get; set; }
        public EstadoCivil? EstadoCivil { get; set; }
        public string Cpf { get; set; }
        public string NumeroPassaporte { get; set; }
        public DateTime? DataValidadePassaporte { get; set; }
        public int? CodigoPaisEmissaoPassaporte { get; set; }
        public string NumeroIdentidade { get; set; }
        public string OrgaoEmissorIdentidade { get; set; }
        public string UfIdentidade { get; set; }
        public TipoNacionalidade Nacionalidade { get; set; }
        public int CodigoPaisNacionalidade { get; set; }
        public string UfNaturalidade { get; set; }
        public int? CodigoCidadeNaturalidade { get; set; }
        public string DescricaoNaturalidadeEstrangeira { get; set; }
        public string NomePai { get; set; }
        public string NomeMae { get; set; }
        public string Email { get; set; }
        public List<EnderecoData> Enderecos { get; set; }
        public List<EnderecoEletronicoData> EnderecosEletronicos { get; set; }
        public List<TelefoneData> Telefones { get; set; }
        public bool ConsentimentoLGPD { get; set; }
        public Guid? UidProcesso { get; set; }
    }
}
