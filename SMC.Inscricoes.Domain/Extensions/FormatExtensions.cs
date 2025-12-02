using SMC.Inscricoes.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Extensions
{
    public static class FormatExtensions
    {

        /// <summary>
        /// Formata um endereço para impressão
        /// Formato:
        /// Logradouro, Numero [ - Complemento] - Bairro - Cidade/UF - CEP: Cep
        /// </summary>
        /// <param name="endereco">Endereço a ser formatado</param>
        /// <returns>Endereço formatado para impressão</returns>
        public static string FormatarParaImpressao(this Endereco endereco)
        {
            string endFormatado = string.Format("{0}, {1}", endereco.Logradouro.Trim(), endereco.Numero.Trim());
            if (!string.IsNullOrEmpty(endereco.Complemento))
                endFormatado += string.Format(" - {0}", endereco.Complemento.Trim());
            endFormatado += string.Format(" - {0} - {1}/{2} - CEP: {3}", endereco.Bairro.Trim(), endereco.NomeCidade.Trim(), endereco.Uf, endereco.Cep);
            return endFormatado;
        }

        /// <summary>
        /// Formata um telefone para impressão
        /// formato:
        /// +CodigoPais CodigoArea Número
        /// </summary>
        /// <param name="telefone">Telefone a ser formatado</param>
        /// <returns>Telefone formatado</returns>
        public static string FormatarParaImpressao(this Telefone telefone)
        {
            return string.Format("+{0} {1} {2}", telefone.CodigoPais.ToString(), telefone.CodigoArea.ToString(), telefone.Numero.Trim());
        }
    }
}
