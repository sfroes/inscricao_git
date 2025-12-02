using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework.Validation;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.Validators
{
    public class InscritoValidatorStep1 : SMCValidator<Inscrito>
    {
        // Regex para validação de Nome - Valida pelo menos um nome e um sobrenome separados por um espaço
        private const string NomeRegex = @"^[^\s]+( +[^\s]+)+$";

        /// <summary>
        /// Realiza a validação de inscrito
        /// Regras:
        /// 1) Deve informar o nome do pai ou da mãe
        /// 2) Se a nacionalidade for "Brasileira", o CPF e a Identidade são obrigatórios
        /// 3) Se a nacionalidade não for "Brasileira", o Passaporte é obrigatório
        /// 4) Se o pais for "Brasil", verifica se informou a UF/Cidade de naturalidade
        /// 5) Se o pais não for "Brasil", verifica se informou a descrição da nacionalidade
        /// </summary>
        /// <param name="item">Inscrito a ser validado</param>
        /// <param name="validationResults">Resultado da validação</param>
        protected override void DoValidate(Inscrito item, SMCValidationResults validationResults)
        {
            base.DoValidate(item, validationResults);

            // Valida campos obrigatórios
            ValidacaoCamposObrigatorios(item);

            // Verifica se a data de nascimento é válida
            if (item.DataNascimento.Year <= 1500)
            {
                this.AddPropertyError(p => p.NumeroIdentidade, Resources.MessagesResource.ResourceManager.GetString(
                    "DataNascimentoInvalidaException", System.Threading.Thread.CurrentThread.CurrentCulture));
            }
            // Verifica se o a nacionalidade é estrangeira e o pais não é o brasil
            if(item.Nacionalidade == TipoNacionalidade.Estrangeira && item.CodigoPaisNacionalidade == CONSTANTS.CODIGO_PAIS_BRASIL)
			{
                this.AddPropertyError(p => p.Cpf, Resources.MessagesResource.ResourceManager.GetString(
                    "PaisInvalidoParaNacionalidadeEstrangeira", System.Threading.Thread.CurrentThread.CurrentCulture));
            }

            if (item.Nacionalidade == TipoNacionalidade.Brasileira)
            {
                // Verifica se CPF foi informado
                if (String.IsNullOrWhiteSpace(item.Cpf))
                {
                    this.AddPropertyError(p => p.Cpf, Resources.MessagesResource.ResourceManager.GetString(
                    "InscritoCpfObrigatorio", System.Threading.Thread.CurrentThread.CurrentCulture));
                }

                // Verifica se a Identidade foi informada
                if (string.IsNullOrWhiteSpace(item.NumeroIdentidade) ||
                    string.IsNullOrWhiteSpace(item.OrgaoEmissorIdentidade) ||
                    string.IsNullOrWhiteSpace(item.UfIdentidade))
                {
                    this.AddPropertyError(p => p.NumeroIdentidade, Resources.MessagesResource.ResourceManager.GetString(
                    "IdentidadeObrigatoria", System.Threading.Thread.CurrentThread.CurrentCulture));
                }
            }
            else
            {
                // Verifica se o passaporte foi informado
                if (item.Nacionalidade == TipoNacionalidade.Estrangeira && (String.IsNullOrWhiteSpace(item.NumeroPassaporte)
                                                                            || String.IsNullOrWhiteSpace(item.DataValidadePassaporte.ToString())
                                                                            || String.IsNullOrWhiteSpace(item.CodigoPaisEmissaoPassaporte.ToString())))
                {
                    this.AddPropertyError(p => p.Cpf, Resources.MessagesResource.ResourceManager.GetString(
                    "InscritoPassaporteObrigatorio", System.Threading.Thread.CurrentThread.CurrentCulture));
                }

                if (item.Nacionalidade == TipoNacionalidade.BrasileiraNaturalizado && String.IsNullOrWhiteSpace(item.Cpf))
                {
                    this.AddPropertyError(p => p.Cpf, Resources.MessagesResource.ResourceManager.GetString(
                    "InscritoCpfObrigatorio", System.Threading.Thread.CurrentThread.CurrentCulture));
                }

                if (item.Nacionalidade == TipoNacionalidade.BrasileiraNaturalizado &&
                    (string.IsNullOrWhiteSpace(item.NumeroIdentidade) ||
                    string.IsNullOrWhiteSpace(item.OrgaoEmissorIdentidade) ||
                    string.IsNullOrWhiteSpace(item.UfIdentidade)))
                {
                    this.AddPropertyError(p => p.NumeroIdentidade, Resources.MessagesResource.ResourceManager.GetString(
                    "IdentidadeObrigatoria", System.Threading.Thread.CurrentThread.CurrentCulture));
                }
            }

            // Se o pais for Brasil, verifica se informou a UF e Cidade da Naturalidade
            if (item.CodigoPaisNacionalidade.Equals(CONSTANTS.CODIGO_PAIS_BRASIL))
            {
                if (string.IsNullOrEmpty(item.UfNaturalidade) || !item.CodigoCidadeNaturalidade.HasValue)
                    this.AddPropertyError(p => p.UfNaturalidade, Resources.MessagesResource.ResourceManager.GetString(
                    "NaturalidadeBrasileiraInvalida", System.Threading.Thread.CurrentThread.CurrentCulture));
                else
                    item.DescricaoNaturalidadeEstrangeira = null;
            }
            else // Se pais diferente de Brasil, verifica se informou a descrição da naturalidade
            {
                if (string.IsNullOrEmpty(item.DescricaoNaturalidadeEstrangeira))
                    this.AddPropertyError(p => p.DescricaoNaturalidadeEstrangeira,
                        Resources.MessagesResource.ResourceManager.GetString(
                    "NaturalidadeEstrangeiraInvalida", System.Threading.Thread.CurrentThread.CurrentCulture));
                else
                {
                    item.UfNaturalidade = null;
                    item.CodigoCidadeNaturalidade = null;
                }
            }

            // O preenchimento do campo "Nome da mãe" ou "Nome do pai" é obrigatório
            if (string.IsNullOrEmpty(item.NomeMae) && string.IsNullOrEmpty(item.NomePai))
            {
                this.AddPropertyError(p => p.NomeMae, Resources.MessagesResource.ResourceManager.GetString(
                    "InscritoSemNomeMaeOuNomePai", System.Threading.Thread.CurrentThread.CurrentCulture));
            }
            else if (item.NomeMae?.ToLower() == item.NomePai?.ToLower())
            {
                this.AddPropertyError(p => p.NomeMae, Resources.MessagesResource.InscritoNomeMaeNomePaiIdenticos);
            }

            //Se preencher um campo do passaporte os outros devem ser preenchidos
            if (!ValidacaoPassaporte(item))
            {
                this.AddPropertyError(p => p.NumeroPassaporte, Resources.MessagesResource.ResourceManager.GetString(
                    "DadosPassaporte", System.Threading.Thread.CurrentThread.CurrentCulture));
            }
        }

        /// <summary>
        /// Configurações para validação
        /// - Tamanho máximo do Nome = 100
        /// - Tamanho máximo do Nome Social = 100
        /// - Data de nascimento deve ser anterior a data de hoje
        /// - CPF é válido
        /// - Tamanho máximo do Nome da mãe = 100
        /// - Tamanho máximo do Nome do pai = 100
        /// - Tamanho máximo do email = 100
        /// - Nome, Nome Social, Nome do Pai e Nome da mãe devem ter pelo menos um sobrenome
        /// </summary>
        public override void Configure()
        {
            this.Property(p => p.Nome).HasMaxLength(100);
            this.Property(p => p.Nome).HasRegularExpression(NomeRegex);

            this.Property(p => p.NomeSocial).HasMaxLength(100);
            //this.Property(p => p.NomeSocial).HasRegularExpression(NomeRegex);

            this.Property(p => p.DataNascimento).EarlierThanNow();

            this.Property(p => p.Cpf).IsCpf();

            this.Property(p => p.NomeMae).HasMaxLength(100);
            this.Property(p => p.NomeMae).HasRegularExpression(NomeRegex);

            this.Property(p => p.NomePai).HasMaxLength(100);
            this.Property(p => p.NomePai).HasRegularExpression(NomeRegex);

            base.Configure();
        }

        /// <summary>
        /// Valida se algum campo do passaporte veio vazio
        /// </summary>
        /// <param name="inscrito">Dados do Inscrito</param>
        private bool ValidacaoPassaporte(Inscrito inscrito)
        {
            bool dadosPassaporteValido = true;

            //Se preencheu somente o numero do passaporte e burlou os outros
            if (!String.IsNullOrWhiteSpace(inscrito.NumeroPassaporte) &&
                (String.IsNullOrWhiteSpace(inscrito.CodigoPaisEmissaoPassaporte.ToString()) ||
                 String.IsNullOrWhiteSpace(inscrito.DataValidadePassaporte.ToString())))
            {
                dadosPassaporteValido = false;
            }

            //Se preencheu somente o data de validade e burlou os outros
            if (!String.IsNullOrWhiteSpace(inscrito.DataValidadePassaporte.ToString()) &&
                (String.IsNullOrWhiteSpace(inscrito.CodigoPaisEmissaoPassaporte.ToString()) ||
                 String.IsNullOrWhiteSpace(inscrito.NumeroPassaporte)))
            {
                dadosPassaporteValido = false;
            }

            //Se preencheu somente o codigo do pais e burlou os outros
            if (!String.IsNullOrWhiteSpace(inscrito.CodigoPaisEmissaoPassaporte.ToString()) &&
                (String.IsNullOrWhiteSpace(inscrito.DataValidadePassaporte.ToString()) ||
                 String.IsNullOrWhiteSpace(inscrito.NumeroPassaporte)))
            {
                dadosPassaporteValido = false;
            }

            return dadosPassaporteValido;

        }

        /// <summary>
        /// Validacao de Campos Obrigatorios
        /// </summary>
        /// <param name="inscrito">Dados do Inscrito</param>
        private void ValidacaoCamposObrigatorios(Inscrito inscrito)
        {
            if (String.IsNullOrWhiteSpace(inscrito.Nome))
            {
                this.AddPropertyError(p => p.Nome, Resources.MessagesResource.ResourceManager.GetString(
                     "NomeObrigatorio", System.Threading.Thread.CurrentThread.CurrentCulture));
            }

            if (String.IsNullOrWhiteSpace(inscrito.DataNascimento.ToString()))
            {
                this.AddPropertyError(p => p.DataNascimento, Resources.MessagesResource.ResourceManager.GetString(
                    "DataNacismentoObrigatorio", System.Threading.Thread.CurrentThread.CurrentCulture));
            }

            if (inscrito.Sexo == Sexo.Nenhum)
            {
                this.AddPropertyError(p => p.Sexo, Resources.MessagesResource.ResourceManager.GetString(
                    "SexoObrigatorio", System.Threading.Thread.CurrentThread.CurrentCulture));
            }

            if (inscrito.Nacionalidade == TipoNacionalidade.Nenhum)
            {
                this.AddPropertyError(p => p.Nacionalidade, Resources.MessagesResource.ResourceManager.GetString(
                    "NacionalidadeObrigatorio", System.Threading.Thread.CurrentThread.CurrentCulture));
            }
        }
    }
}
