using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class OfertaCabecalhoVO : ISMCMappable
    {
        /// <summary>
        /// Número de inscrição
        /// </summary>
        public string NumeroInscricao { get; set; }

        /// <summary>
        /// Candidato: nome do inscrito.
        /// </summary>
        public string Candidato { get; set; }

        /// <summary>
        /// Oferta original: caminho completo da oferta original do candidato.
        /// </summary>
        public string OfertaOriginal { get; set; }

        /// <summary>
        ///  Exibir o número da opção seguido do caracter "ª".
        /// </summary>
        public string Opcao { get; set; }

        /// <summary>
        /// Justificativa da alteração: justificativa da alteração da oferta.
        /// </summary>
        public string Nome { get; set; }
        public string Situacao { get; set; }

        public DateTime? DataInicioAtividade { get; set; }

        public DateTime? DataFimAtividade { get; set; }

        public int? CargaHorariaAtividade { get; set; }

        public bool? ExibirPeriodoAtividadeOferta { get; set; }
    }
}