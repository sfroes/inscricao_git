using Newtonsoft.Json;
using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.UI.Mvc.Html;
using SMC.GPI.Administrativo.Areas.INS.Models.Shared;
using SMC.GPI.Administrativo.Models;
using SMC.Inscricoes.Common;
using SMC.Localidades.UI.Mvc.DataAnnotation;
using SMC.Localidades.UI.Mvc.Models;
using System;
using System.Collections.Generic;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class AlterarDadosInscritoViewModel : SMCViewModelBase, ISMCStatefulView, ISMCMappable
    {
    /***************************************************************************************************************
       Esta pagina contem JS(LimparEstadosCidadesLocalidade) do administrativo
       que manipula o estado e cidade do inscrito, trabalhando com os required e display dos campos.
    ***************************************************************************************************************/

        // Regex para validação de Nome - Valida pelo menos um nome e um sobrenome separados por um espaço
        private const string NomeRegex = @"^[^\s]+( +[^\s]+)+";

        #region Campos que serão exibidos para inscrito

        [SMCHidden]
        public bool ExibirNome { get; set; }
        [SMCHidden]
        public bool ExibirNomeSocial { get; set; }
        [SMCHidden]
        public bool ExibirDataNascimento { get; set; }
        [SMCHidden]
        public bool ExibirSexo { get; set; }
        [SMCHidden]
        public bool ExibirEstadoCivil { get; set; }
        [SMCHidden]
        public bool ExibirNacionalidade { get; set; }
        [SMCHidden]
        public bool ExibirPaisOrigem { get; set; }
        [SMCHidden]
        public bool ExibirNaturalidade { get; set; }
        [SMCHidden]
        public bool ExibirCPF { get; set; }
        [SMCHidden]
        public bool ExibirPassaporte { get; set; }
        [SMCHidden]
        public bool ExibirNumeroIdentidade { get; set; }
        [SMCHidden]
        public bool ExibirOrgaoEmissorIdentidade { get; set; }
        [SMCHidden]
        public bool ExibirUfIdentidade { get; set; }
        [SMCHidden]
        public bool ExibirFiliacao { get; set; }
        [SMCHidden]
        public bool ExibirEndereco { get; set; }
        [SMCHidden]
        public bool ExibirTelefone { get; set; }
        [SMCHidden]
        public bool ExibirEmail { get; set; }
        [SMCHidden]
        public bool ExibirOutrosEnderecosEletronicos { get; set; }

        #endregion

        public AlterarDadosInscritoViewModel()
        {
            this.Enderecos = new AddressList();
            this.Telefones = new PhoneList();
            this.EnderecosEletronicos = new SMCMasterDetailList<EnderecoEletronicoViewModel>();
            this.EstadoCidade = new EstadoCidadeViewModel();

            this.Paises = new List<SMCDatasourceItem>();
            this.EstadosIdentidade = new List<SMCSelectListItem>();

            // Por padrão o pais selecionado na nacionalidade é Brasil
            //this.CodigoPaisNacionalidade = CONSTANTS.CODIGO_PAIS_BRASIL;
        }

        [SMCHidden]
        [SMCSize(Framework.SMCSize.Grid8_24)]
        public long Seq { get; set; }

        [SMCSize(SMCSize.Grid7_24, SMCSize.Grid12_24, SMCSize.Grid12_24)]
        [SMCMaxLength(100)]
        [SMCConditionalRequired(nameof(ExibirNome), SMCConditionalOperation.Equals, true)]
        [SMCRegularExpression(NomeRegex, FormatErrorResourceKey = "InscritoViewModel_Nome_NomeESobrenome")]
        public string Nome { get; set; }

        [SMCSize(SMCSize.Grid7_24, SMCSize.Grid12_24, SMCSize.Grid12_24)]
        [SMCMaxLength(100)]
        [SMCRegularExpression(NomeRegex, FormatErrorResourceKey = "InscritoViewModel_Nome_NomeESobrenome")]
        public string NomeSocial { get; set; }

        [SMCConditionalRequired(nameof(ExibirDataNascimento), SMCConditionalOperation.Equals, true)]
        [SMCSize(SMCSize.Grid4_24, SMCSize.Grid8_24, SMCSize.Grid8_24, SMCSize.Grid4_24)]
        [SMCDateTimeMode(SMCDateTimeMode.Date)]
        public DateTime DataNascimento { get; set; }

        [SMCConditionalRequired(nameof(ExibirSexo), SMCConditionalOperation.Equals, true)]
        [SMCSize(SMCSize.Grid3_24, SMCSize.Grid8_24, SMCSize.Grid8_24)]
        [SMCSelect]
        public Sexo Sexo { get; set; }

        [SMCSize(SMCSize.Grid3_24, SMCSize.Grid8_24, SMCSize.Grid8_24)]
        [SMCSelect]
        public EstadoCivil EstadoCivil { get; set; }

        [SMCCpf]
        [SMCSize(SMCSize.Grid6_24)]
        [SMCConditionalRequired("Nacionalidade", new object[] { TipoNacionalidade.Brasileira, TipoNacionalidade.BrasileiraNaturalizado }, RuleName = "R1")]
        [SMCConditionalRequired(nameof(ExibirCPF), SMCConditionalOperation.Equals, true, RuleName = "R2")]
        [SMCConditionalRequired(nameof(ExibirNacionalidade), SMCConditionalOperation.Equals, true, RuleName = "R3")]
        [SMCConditionalRule("R1 && R2 && R3")]
        public string Cpf { get; set; }

        [SMCSize(SMCSize.Grid5_24)]
        [SMCMaxLength(30)]
        [SMCConditionalRequired("Nacionalidade", new object[] { TipoNacionalidade.Estrangeira }, RuleName = "R1")]
        [SMCConditionalRequired(nameof(CodigoPaisEmissaoPassaporte), SMCConditionalOperation.NotEqual, "", RuleName = "R2")]
        [SMCConditionalRequired(nameof(DataValidadePassaporte), SMCConditionalOperation.NotEqual, "", RuleName = "R3")]
        [SMCConditionalRequired(nameof(ExibirPassaporte), SMCConditionalOperation.Equals, true, RuleName = "R4")]
        [SMCConditionalRequired(nameof(ExibirNacionalidade), SMCConditionalOperation.Equals, true, RuleName = "R5")]
        [SMCConditionalRule("(R1 || R2 || R3) && R4 && R5")]
        public string NumeroPassaporte { get; set; }

        [SMCSize(SMCSize.Grid5_24)]
        [SMCConditionalRequired("Nacionalidade", new object[] { TipoNacionalidade.Estrangeira }, RuleName = "R1")]
        [SMCConditionalRequired(nameof(CodigoPaisEmissaoPassaporte), SMCConditionalOperation.NotEqual, "", RuleName = "R2")]
        [SMCConditionalRequired(nameof(NumeroPassaporte), SMCConditionalOperation.NotEqual, "", RuleName = "R3")]
        [SMCConditionalRequired(nameof(ExibirPassaporte), SMCConditionalOperation.Equals, true, RuleName = "R4")]
        [SMCConditionalRequired(nameof(ExibirNacionalidade), SMCConditionalOperation.Equals, true, RuleName = "R5")]
        [SMCConditionalRule("(R1 || R2 || R3) && R4 && R5")]
        public DateTime? DataValidadePassaporte { get; set; }

        [SMCSize(SMCSize.Grid8_24)]
        [SMCSelect("Paises")]
        [SMCConditionalRequired("Nacionalidade", new object[] { TipoNacionalidade.Estrangeira }, RuleName = "R1")]
        [SMCConditionalRequired(nameof(NumeroPassaporte), SMCConditionalOperation.NotEqual, "", RuleName = "R2")]
        [SMCConditionalRequired(nameof(DataValidadePassaporte), SMCConditionalOperation.NotEqual, "", RuleName = "R3")]
        [SMCConditionalRequired(nameof(ExibirPassaporte), SMCConditionalOperation.Equals, true, RuleName = "R4")]
        [SMCConditionalRequired(nameof(ExibirNacionalidade), SMCConditionalOperation.Equals, true, RuleName = "R5")]
        [SMCConditionalRule("(R1 || R2 || R3) && R4 && R5")]
        public int? CodigoPaisEmissaoPassaporte { get; set; }

        [SMCSize(SMCSize.Grid9_24)]
        [SMCMaxLength(30)]
        [SMCConditionalRequired("Nacionalidade", new object[] { TipoNacionalidade.Brasileira, TipoNacionalidade.BrasileiraNaturalizado }, RuleName = "R1")]
        [SMCConditionalRequired(nameof(ExibirNumeroIdentidade), SMCConditionalOperation.Equals, true, RuleName = "R2")]
        [SMCConditionalRequired(nameof(ExibirNacionalidade), SMCConditionalOperation.Equals, true, RuleName = "R3")]
        [SMCConditionalRule("R1 && R2 && R3")]
        public string NumeroIdentidade { get; set; }

        [SMCSize(SMCSize.Grid8_24)]
        [SMCMaxLength(100)]
        [SMCConditionalRequired("Nacionalidade", new object[] { TipoNacionalidade.Brasileira, TipoNacionalidade.BrasileiraNaturalizado }, RuleName = "R1")]
        [SMCConditionalRequired(nameof(ExibirOrgaoEmissorIdentidade), SMCConditionalOperation.Equals, true, RuleName = "R2")]
        [SMCConditionalRequired(nameof(ExibirNacionalidade), SMCConditionalOperation.Equals, true, RuleName = "R3")]
        [SMCConditionalRule("R1 && R2 && R3")]
        public string OrgaoEmissorIdentidade { get; set; }

        [SMCSize(SMCSize.Grid7_24)]
        [SMCSelect(nameof(EstadosIdentidade))]
        [SMCConditionalRequired("Nacionalidade", new object[] { TipoNacionalidade.Brasileira, TipoNacionalidade.BrasileiraNaturalizado }, RuleName = "R1")]
        [SMCConditionalRequired(nameof(ExibirUfIdentidade), SMCConditionalOperation.Equals, true, RuleName = "R2")]
        [SMCConditionalRequired(nameof(ExibirNacionalidade), SMCConditionalOperation.Equals, true, RuleName = "R3")]
        [SMCConditionalRule("R1 && R2 && R3")]
        public string UfIdentidade { get; set; }

        [JsonIgnore]
        public List<SMCSelectListItem> EstadosIdentidade { get; set; }

        [SMCSize(SMCSize.Grid12_24)]
        [SMCSelect]
        [SMCConditionalRequired(nameof(ExibirNacionalidade), SMCConditionalOperation.Equals, true)]
        public TipoNacionalidade Nacionalidade { get; set; }

        [SMCSelect("Paises")]
        [SMCSize(SMCSize.Grid12_24)]
        [SMCConditionalRequired(nameof(ExibirPaisOrigem), SMCConditionalOperation.Equals, true)]
        [SMCMapForceFromTo]
        [SMCConditionalReadonly(nameof(Nacionalidade), TipoNacionalidade.Brasileira, PersistentValue = true)]
        public int CodigoPaisNacionalidade { get; set; }

        [JsonIgnore]
        public List<SMCDatasourceItem> Paises { get; set; }

        // Representa um seletor de estado/cidade
        [StateCity]
        [SMCSize(SMCSize.Grid24_24)]
        [SMCConditionalDisplay("CodigoPaisNacionalidade", SMCConditionalOperation.Equals, CONSTANTS.CODIGO_PAIS_BRASIL)]
        [SMCConditionalRequired("CodigoPaisNacionalidade", SMCConditionalOperation.Equals, CONSTANTS.CODIGO_PAIS_BRASIL, RuleName = "R1")]
        [SMCConditionalRequired(nameof(ExibirNaturalidade), SMCConditionalOperation.Equals, true, RuleName = "R2")]
        [SMCConditionalRule("R1 && R2")]
        [SMCMapForceFromTo]
        public EstadoCidadeViewModel EstadoCidade { get; set; }

        // Essa propriedade não está marcada como required, pois no caso do pais ser Brasil ela não é obrigatória
        // Na view é forçada a apresentação do * de required
        [SMCSize(SMCSize.Grid24_24)]
        [SMCMaxLength(100)]
        [SMCConditionalDisplay("CodigoPaisNacionalidade", SMCConditionalOperation.NotEqual, CONSTANTS.CODIGO_PAIS_BRASIL)]
        [SMCConditionalRequired("CodigoPaisNacionalidade", SMCConditionalOperation.NotEqual, CONSTANTS.CODIGO_PAIS_BRASIL, RuleName = "R1")]
        [SMCConditionalRequired(nameof(ExibirNaturalidade), SMCConditionalOperation.Equals, true, RuleName = "R2")]
        [SMCConditionalRule("R1 && R2")]
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
        [SMCConditionalRequired(nameof(ExibirEmail), SMCConditionalOperation.Equals, true)]
        [SMCMaxLength(100)]
        [SMCSize(SMCSize.Grid12_24)]
        public string Email { get; set; }

        [SMCEmail]
        [SMCConditionalRequired(nameof(ExibirEmail), SMCConditionalOperation.Equals, true)]
        [SMCMaxLength(100)]
        [SMCSize(SMCSize.Grid12_24)]
        [SMCTextBox(MatchField = "Email")]
        public string EmailConfirmacao { get; set; }

        [Address(Correspondence = true, AcceptForeignAddress = true)]
        [SMCMapForceFromTo]
        public AddressList Enderecos { get; set; }

        [Phone()]
        [SMCMapForceFromTo]
        public PhoneList Telefones { get; set; }

        [SMCMapForceFromTo]
        public SMCMasterDetailList<EnderecoEletronicoViewModel> EnderecosEletronicos { get; set; }

        [JsonIgnore]
        public List<SMCDatasourceItem> TiposEnderecoEletronico { get; set; }

        [SMCHidden]
        public long SeqUsuarioSas { get; set; }

        [SMCHidden]
        public long? Origem { get; set; }

        [SMCHidden]
        public string BackUrl { get; set; }
    }
}