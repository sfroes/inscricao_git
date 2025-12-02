using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework.Util;
using SMC.Framework.Validation;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Models;
using SMC.Localidades.Common.Areas.LOC.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SMC.Inscricoes.Domain.Areas.INS.Validators
{
    public class InscritoValidatorStep2 : SMCValidator<Inscrito>
    {
        private static readonly string[] IgnoredProperties =
        new List<string>
        {
            nameof(Endereco.Seq),
            nameof(Endereco.UsuarioAlteracao),
            nameof(Endereco.UsuarioInclusao),
            nameof(Endereco.DataAlteracao),
            nameof(Endereco.DataInclusao),
            nameof(Endereco.Correspondencia)
        }.ToArray();

        /// <summary>
        /// Realiza a validação de inscrito
        /// </summary>
        /// <param name="item">Inscrito a ser validado</param>
        /// <param name="validationResults">Resultado da validação</param>
        protected override void DoValidate(Inscrito item, SMCValidationResults validationResults)
        {
            base.DoValidate(item, validationResults);

            // Valida campos obrigatórios
            ValidacaoCamposObrigatorios(item);

            if (item.Enderecos == null || !item.Enderecos.Any(f => f.TipoEndereco == TipoEndereco.Residencial))
            {
                AddPropertyError(x => x.Enderecos, Resources.MessagesResource.ResourceManager.GetString(
                    "EnderecoResidencialObrigatorio", System.Threading.Thread.CurrentThread.CurrentCulture));
            }

            var endCorrepondency = item.Enderecos.Count(f => f.Correspondencia.HasValue && f.Correspondencia.Value);
            if (endCorrepondency == 0)
            {
                AddPropertyError(x => x.Enderecos, Resources.MessagesResource.ResourceManager.GetString(
                    "EnderecoCorrespondenciaObrigatorio", System.Threading.Thread.CurrentThread.CurrentCulture));
            }
            else if (endCorrepondency > 1)
            {
                AddPropertyError(x => x.Enderecos, Resources.MessagesResource.ResourceManager.GetString(
                    "MultiploEnderecoCorrespondencia", System.Threading.Thread.CurrentThread.CurrentCulture));
            }

            var enderecoDuplicado = item.Enderecos.Any(a => EnderecoDuplicado(a, item.Enderecos));
            if (enderecoDuplicado)
            {
                AddPropertyError(x => x.Enderecos, Resources.MessagesResource.ResourceManager.GetString(
                    "EnderecoDuplicado", System.Threading.Thread.CurrentThread.CurrentCulture));
            }

            if (item.Telefones == null || item.Telefones.Count == 0)
            {
                AddPropertyError(x => x.Telefones,
                    Resources.MessagesResource.ResourceManager.GetString(
                    "TelefoneInvalido", System.Threading.Thread.CurrentThread.CurrentCulture));
			}
			else
			{
                //verifica se tem algum telefone inválido
                bool telefoneInvalido = item.Telefones.Any(a => (a.CodigoPais == 0 || a.CodigoArea == 0 || a.TipoTelefone == TipoTelefone.Nenhum || string.IsNullOrEmpty(a.Numero)));
                if (telefoneInvalido)
                {
                    AddPropertyError(x => x.Telefones,
                        Resources.MessagesResource.ResourceManager.GetString(
                        "TelefonePreenchidoMasInvalido", System.Threading.Thread.CurrentThread.CurrentCulture));
                }
            }

			if (string.IsNullOrEmpty(item.Email))
			{
                AddPropertyError(x => x.Email,
                        Resources.MessagesResource.ResourceManager.GetString(
                        "EmailObrigatorioInvalido", System.Threading.Thread.CurrentThread.CurrentCulture));
            }

            if(item.EnderecosEletronicos.Count() > 0)
			{
                bool enderecoEletronicoInvalido = item.EnderecosEletronicos.Any(e => (e.TipoEnderecoEletronico == TipoEnderecoEletronico.Nenhum || string.IsNullOrEmpty(e.Descricao)));
				if (enderecoEletronicoInvalido)
				{
                    AddPropertyError(x => x.EnderecosEletronicos,
                        Resources.MessagesResource.ResourceManager.GetString(
                        "EnderecoEletronicoObrigatorio", System.Threading.Thread.CurrentThread.CurrentCulture));
                }
			}
        }

        /// <summary>
        /// Validacao de Campos Obrigatorios
        /// </summary>
        /// <param name="inscrito">Dados do Inscrito</param>
        private void ValidacaoCamposObrigatorios(Inscrito inscrito)
        {
            ValidaCamposEnderecosObrigatorios(inscrito);

            foreach (var telefone in inscrito.Telefones)
            {
                if(telefone.CodigoArea == 0 ||
                    telefone.CodigoPais == 0 ||
                    string.IsNullOrEmpty(telefone.Numero) ||
                    telefone.TipoTelefone == TipoTelefone.Nenhum)
                {
                    this.AddPropertyError(p => p.Telefones, Resources.MessagesResource.ResourceManager.GetString(
                    "DadosTelefoneObrigatorio", System.Threading.Thread.CurrentThread.CurrentCulture));
                }
            }

            foreach (var enderecoEletronico in inscrito.EnderecosEletronicos)
            {
                if (String.IsNullOrWhiteSpace(enderecoEletronico.TipoEnderecoEletronico.ToString()) ||
                    String.IsNullOrWhiteSpace(enderecoEletronico.Descricao))
                {
                    this.AddPropertyError(p => p.Nacionalidade, Resources.MessagesResource.ResourceManager.GetString(
                    "EnderecoEletronicoObrigatorio", System.Threading.Thread.CurrentThread.CurrentCulture));
                }
            }
        }

        /// <summary>
        /// Validar regras para os endereços
        /// </summary>
        /// <param name="inscrito">Dados do inscrito</param>
        private void ValidaCamposEnderecosObrigatorios(Inscrito inscrito)
        {
            foreach (var item in inscrito.Enderecos)
            {
                if (String.IsNullOrWhiteSpace(item.CodigoPais.ToString()))
                {
                    this.AddPropertyError(p => p.Enderecos, Resources.MessagesResource.ResourceManager.GetString(
                        "PaisEnderecoObrigatorio", System.Threading.Thread.CurrentThread.CurrentCulture));
                }

                if (String.IsNullOrWhiteSpace(item.Logradouro))
                {
                    this.AddPropertyError(p => p.Enderecos, Resources.MessagesResource.ResourceManager.GetString(
                   "LogradouroEnderecoObrigatorio", System.Threading.Thread.CurrentThread.CurrentCulture));
                }

                if ((!String.IsNullOrWhiteSpace(item.Cep) && item.CodigoPais == CONSTANTS.CODIGO_PAIS_BRASIL) && String.IsNullOrWhiteSpace(item.Numero))
                {
                    this.AddPropertyError(p => p.Enderecos, Resources.MessagesResource.ResourceManager.GetString(
                   "NumeroEnderecoObrigatorio", System.Threading.Thread.CurrentThread.CurrentCulture));
                }

                if ((!String.IsNullOrWhiteSpace(item.Cep) && item.CodigoPais == CONSTANTS.CODIGO_PAIS_BRASIL) && String.IsNullOrWhiteSpace(item.Bairro))
                {
                    this.AddPropertyError(p => p.Enderecos, Resources.MessagesResource.ResourceManager.GetString(
                   "BairroEndereco", System.Threading.Thread.CurrentThread.CurrentCulture));
                }

                if (String.IsNullOrWhiteSpace(item.NomeCidade))
                {
                    this.AddPropertyError(p => p.Enderecos, Resources.MessagesResource.ResourceManager.GetString(
                   "CidadeEndereco", System.Threading.Thread.CurrentThread.CurrentCulture));
                }

                if ((!String.IsNullOrWhiteSpace(item.Cep) && item.CodigoPais == CONSTANTS.CODIGO_PAIS_BRASIL) && String.IsNullOrWhiteSpace(item.Uf))
                {
                    this.AddPropertyError(p => p.Enderecos, Resources.MessagesResource.ResourceManager.GetString(
                   "EstadoEndereco", System.Threading.Thread.CurrentThread.CurrentCulture));
                }

                if ((String.IsNullOrWhiteSpace(item.Cep) && item.CodigoPais == CONSTANTS.CODIGO_PAIS_BRASIL))
                {
                    this.AddPropertyError(p => p.Enderecos, Resources.MessagesResource.ResourceManager.GetString(
                   "CepEnderecoObrigatorio", System.Threading.Thread.CurrentThread.CurrentCulture));
                }
            }
        }


        /// <summary>
        /// Configurações para validação
        /// - Email é válido
        /// </summary>
        public override void Configure()
        {
            Property(p => p.Email).HasMaxLength(100).IsEmail();
            base.Configure();
        }

        private bool EnderecoDuplicado(Endereco enderecoBase, IEnumerable<Endereco> enderecos)
        {
            return enderecos
                .Count(c => SMCReflectionHelper
                    .CompareExistingPrimitivePropertyValues(enderecoBase, c, IgnoredProperties)) > 1;
        }
    }
}
