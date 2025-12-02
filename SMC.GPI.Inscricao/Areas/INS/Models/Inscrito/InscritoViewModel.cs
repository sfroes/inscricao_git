using Newtonsoft.Json;
using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.UI.Mvc.Html;
using SMC.GPI.Inscricao.Areas.INS.Controllers;
using SMC.GPI.Inscricao.Models;
using SMC.Inscricoes.Common;
using SMC.Localidades.UI.Mvc.DataAnnotation;
using SMC.Localidades.UI.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Web.Razor.Text;

namespace SMC.GPI.Inscricao.Areas.INS.Models
{
    public class InscritoViewModel : SMCWizardViewModel, ISMCStatefulView, ISMCMappable
    {
        // Regex para validação de Nome - Valida pelo menos um nome e um sobrenome separados por um espaço
        private const string NomeRegex = @"^[^\s]+( +[^\s]+)+";

        public InscritoViewModel()
        {
            this.Enderecos = new AddressList();
            this.EnderecosEletronicos = new SMCMasterDetailList<EnderecoEletronicoViewModel>();
            this.Telefones = new PhoneList();
            this.EstadoCidade = new EstadoCidadeViewModel();

            this.Paises = new List<SMCDatasourceItem>();
            this.TiposEnderecoEletronico = new List<SMCDatasourceItem>();
            this.EstadosIdentidade = new List<SMCSelectListItem>();

            // Por padrão o pais selecionado na nacionalidade é Brasil
            this.CodigoPaisNacionalidade = CONSTANTS.CODIGO_PAIS_BRASIL;
        }

        [SMCKey]
        [SMCHidden]
        public long Seq { get; set; }

        [SMCHidden]
        public long SeqUsuarioSas { get; set; }

        [SMCSize(SMCSize.Grid7_24, SMCSize.Grid12_24, SMCSize.Grid12_24)]
        [SMCMaxLength(100)]
        [SMCRequired]
        [SMCRegularExpression(NomeRegex, FormatErrorResourceKey = "InscritoViewModel_Nome_NomeESobrenome")]
        public string Nome { get; set; }

        [SMCSize(SMCSize.Grid7_24, SMCSize.Grid12_24, SMCSize.Grid12_24)]
        [SMCMaxLength(100)]
        [SMCReadOnly]
        public string NomeSocial { get; set; }

        [SMCRequired]
        [SMCSize(SMCSize.Grid4_24, SMCSize.Grid8_24, SMCSize.Grid8_24, SMCSize.Grid4_24)]
        [SMCDateTimeMode(SMCDateTimeMode.Date)]
        public DateTime DataNascimento { get; set; }

        [SMCRequired]
        [SMCSize(SMCSize.Grid3_24, SMCSize.Grid8_24, SMCSize.Grid8_24)]
        [SMCSelect]
        public Sexo Sexo { get; set; }

        [SMCSize(SMCSize.Grid3_24, SMCSize.Grid8_24, SMCSize.Grid8_24)]
        [SMCSelect]
        public EstadoCivil? EstadoCivil { get; set; }

        [SMCCpf]
        [SMCSize(SMCSize.Grid6_24)]
        [SMCConditionalRequired("Nacionalidade", new object[] { TipoNacionalidade.Brasileira, TipoNacionalidade.BrasileiraNaturalizado })]
        public string Cpf { get; set; }

        [SMCSize(SMCSize.Grid5_24)]
        [SMCMaxLength(30)]
        [SMCConditionalRequired("Nacionalidade", new object[] { TipoNacionalidade.Estrangeira }, RuleName = "R1")]
        [SMCConditionalRequired(nameof(CodigoPaisEmissaoPassaporte), SMCConditionalOperation.NotEqual, "", RuleName = "R2")]
        [SMCConditionalRequired(nameof(DataValidadePassaporte), SMCConditionalOperation.NotEqual, "", RuleName = "R3")]
        [SMCConditionalRule("R1 || R2 || R3")]
        public string NumeroPassaporte { get; set; }

        [SMCSize(SMCSize.Grid5_24)]
        [SMCConditionalRequired("Nacionalidade", new object[] { TipoNacionalidade.Estrangeira }, RuleName = "R1")]
        [SMCConditionalRequired(nameof(CodigoPaisEmissaoPassaporte), SMCConditionalOperation.NotEqual, "", RuleName = "R2")]
        [SMCConditionalRequired(nameof(NumeroPassaporte), SMCConditionalOperation.NotEqual, "", RuleName = "R3")]
        [SMCConditionalRule("R1 || R2 || R3")]
        public DateTime? DataValidadePassaporte { get; set; }

        [SMCSize(SMCSize.Grid8_24)]
        [SMCSelect("Paises")]
        [SMCConditionalRequired("Nacionalidade", new object[] { TipoNacionalidade.Estrangeira }, RuleName = "R1")]
        [SMCConditionalRequired(nameof(NumeroPassaporte), SMCConditionalOperation.NotEqual, "", RuleName = "R2")]
        [SMCConditionalRequired(nameof(DataValidadePassaporte), SMCConditionalOperation.NotEqual, "", RuleName = "R3")]
        [SMCConditionalRule("R1 || R2 || R3")]
        public int? CodigoPaisEmissaoPassaporte { get; set; }

        [SMCSize(SMCSize.Grid9_24)]
        [SMCMaxLength(30)]
        [SMCConditionalRequired("Nacionalidade", new object[] { TipoNacionalidade.Brasileira, TipoNacionalidade.BrasileiraNaturalizado })]
        public string NumeroIdentidade { get; set; }

        [SMCSize(SMCSize.Grid8_24)]
        [SMCMaxLength(100)]
        [SMCConditionalRequired("Nacionalidade", new object[] { TipoNacionalidade.Brasileira, TipoNacionalidade.BrasileiraNaturalizado })]
        public string OrgaoEmissorIdentidade { get; set; }

        [SMCSize(SMCSize.Grid7_24)]
        [SMCSelect(nameof(EstadosIdentidade))]
        [SMCConditionalRequired("Nacionalidade", new object[] { TipoNacionalidade.Brasileira, TipoNacionalidade.BrasileiraNaturalizado })]
        public string UfIdentidade { get; set; }

        [JsonIgnore]
        public List<SMCSelectListItem> EstadosIdentidade { get; set; }

        [SMCSize(SMCSize.Grid12_24)]
        [SMCSelect]
        [SMCRequired]
        public TipoNacionalidade Nacionalidade { get; set; }

        [SMCSelect("Paises")]
        [SMCSize(SMCSize.Grid12_24)]
        [SMCRequired]
        [SMCMapForceFromTo]
        [SMCConditionalReadonly(nameof(Nacionalidade), TipoNacionalidade.Brasileira, PersistentValue = true)]
        public int? CodigoPaisNacionalidade { get; set; }

        [SMCHidden]
        [SMCDependency(nameof(CodigoPaisNacionalidade), action: "HabilitarDesricaoNaturalidade", controller: "Inscrito", required: false)]
        public bool HabilitaDesricaoNaturalidade { get; set; }

        [JsonIgnore]
        public List<SMCDatasourceItem> Paises { get; set; }

        // Representa um seletor de estado/cidade
        [StateCity]
        [SMCSize(SMCSize.Grid24_24)]
        [SMCConditionalDisplay("HabilitaDesricaoNaturalidade", SMCConditionalOperation.Equals, false)]
        [SMCConditionalRequired("HabilitaDesricaoNaturalidade", SMCConditionalOperation.Equals, false)]
        [SMCMapForceFromTo]
        public EstadoCidadeViewModel EstadoCidade { get; set; }

        // Essa propriedade não está marcada como required, pois no caso do pais ser Brasil ela não é obrigatória
        // Na view é forçada a apresentação do * de required
        [SMCSize(SMCSize.Grid24_24)]
        [SMCMaxLength(100)]
        [SMCConditionalRequired("HabilitaDesricaoNaturalidade", SMCConditionalOperation.Equals, true)]
        [SMCConditionalDisplay("HabilitaDesricaoNaturalidade", SMCConditionalOperation.Equals, true)]
        public string DescricaoNaturalidadeEstrangeira { get; set; }

        [SMCSize(SMCSize.Grid12_24)]
        [SMCMaxLength(100)]
        [SMCRegularExpression(NomeRegex, FormatErrorResourceKey = "InscritoViewModel_NomeMae_NomeESobrenome")]
        public string NomeMae { get; set; }

        [SMCMaxLength(100)]
        [SMCSize(SMCSize.Grid12_24)]
        [SMCRegularExpression(NomeRegex, FormatErrorResourceKey = "InscritoViewModel_NomePai_NomeESobrenome")]
        public string NomePai { get; set; }

        [SMCEmail]
        [SMCRequired]
        [SMCMaxLength(100)]
        [SMCSize(SMCSize.Grid12_24)]
        public string Email { get; set; }

        [SMCRequired]
        [SMCEmail]
        [SMCMaxLength(100)]
        [SMCSize(SMCSize.Grid12_24)]
        public string EmailConfirmacao { get; set; }

        [Address(min: 1, Correspondence = true, AcceptForeignAddress = true)]
        [SMCMapForceFromTo]
        public AddressList Enderecos { get; set; }

        [Phone(min: 1)]
        [SMCMapForceFromTo]
        public PhoneList Telefones { get; set; }

        [SMCMapForceFromTo]
        public SMCMasterDetailList<EnderecoEletronicoViewModel> EnderecosEletronicos { get; set; }

        [JsonIgnore]
        public List<SMCDatasourceItem> TiposEnderecoEletronico { get; set; }

        public bool PossuiInscricao { get; set; }

        /// <summary>
        /// Armazena o ID da inscrição de onde o usuário veio.
        /// </summary>
        public long? Origem { get; set; }


        [JsonIgnore]
        public int Idade
        {
            get
            {
                int idade = DateTime.Now.Year - DataNascimento.Year;
                if (DataNascimento > DateTime.Now.AddYears(-idade))
                    idade--;

                return idade;
            }
        }

        public bool ConsentimentoLGPD { get; set; }

        public string TermoLGPD { get; set; }

        public Guid? UidProcesso { get; set; }

        public string BackUrl { get; set; }

        public string OrientacaoCadastroInscrito { get; set; }

        #region Campos que serão exibidos para inscrito

        public bool ExibirNome { get; set; }
        public bool ExibirNomeSocial { get; set; }
        public bool ExibirDataNascimento { get; set; }
        public bool ExibirSexo { get; set; }
        public bool ExibirEstadoCivil { get; set; }
        public bool ExibirNacionalidade { get; set; }
        public bool ExibirPaisOrigem { get; set; }
        public bool ExibirNaturalidade { get; set; }
        public bool ExibirCPF { get; set; }
        public bool ExibirPassaporte { get; set; }
        public bool ExibirNumeroIdentidade { get; set; }
        public bool ExibirOrgaoEmissorIdentidade { get; set; }
        public bool ExibirUfIdentidade { get; set; }
        public bool ExibirFiliacao { get; set; }
        public bool ExibirEndereco { get; set; }
        public bool ExibirTelefone { get; set; }
        public bool ExibirEmail { get; set; }
        public bool ExibirOutrosEnderecosEletronicos { get; set; }
        public bool EXibirOrientacaoInscrito { get; set; }
        #endregion
    }
}