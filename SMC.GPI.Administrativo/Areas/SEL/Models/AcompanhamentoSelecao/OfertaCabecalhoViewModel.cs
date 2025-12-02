using SMC.Framework;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.SEL.Models
{
    public class OfertaCabecalhoViewModel : SMCViewModelBase
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
        public string Situacao { get; set; }
    }
}