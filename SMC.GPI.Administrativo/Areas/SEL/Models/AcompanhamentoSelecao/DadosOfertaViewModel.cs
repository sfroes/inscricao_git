using SMC.Framework;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups;
using System;
using SMC.Framework.DataAnnotations;
using System.Collections.Generic;

namespace SMC.GPI.Administrativo.Areas.SEL.Models
{
    public class DadosOfertaViewModel : SMCViewModelBase
    {
        [SMCHidden]
        public long? SeqInscricaoOferta { get; set; }

        [SMCHidden]
        public long? SeqInscricao { get; set; }

        [SMCHidden]
        public long? SeqOfertaOriginal { get; set; }

        [SMCHidden]
        public long? SeqGrupoOferta { get; set; }

        [SMCHidden]
        public long? SeqItemHierarquiaOferta { get; set; }

        [SMCReadOnly]
        [SMCSize(SMCSize.Grid24_24)]
        [SMCValueEmpty("-")]
        public string Oferta { get; set; }

        [SMCReadOnly]
        [SMCSize(SMCSize.Grid24_24)]
        [SMCValueEmpty("-")]
        public string OfertaAtual { get; set; }

        /// <summary>
        /// Candidato: nome do inscrito.
        /// </summary>
        [SMCSize(SMCSize.Grid12_24)]
        [SMCReadOnly]
        [SMCValueEmpty("-")]
        public string Candidato { get; set; }

        /// <summary>
        /// Justificativa da inscrição: este campo deverá ser exibido somente se o inscrito tiver 
        /// informado uma justificativa para a inscrição na oferta.
        /// </summary>
        [SMCSize(SMCSize.Grid24_24)]
        [SMCReadOnly]
        [SMCMaxLength(500)]
        [SMCMultiline]
        [SMCValueEmpty("-")]
        public string JustificativaInscricao { get; set; }

        /// <summary>
        /// Oferta original: caminho completo da oferta original do candidato.
        /// </summary>
        [SMCSize(SMCSize.Grid24_24)]
        [SMCReadOnly]
        [SMCValueEmpty("-")]
        public string OfertaOriginal { get; set; }

        /// <summary>
        /// Justificativa da alteração: justificativa da alteração da oferta.
        /// </summary>
        [SMCSize(SMCSize.Grid24_24)]
        [SMCMaxLength(500)]
        [SMCMultiline]
        [SMCReadOnly]
        [SMCValueEmpty("-")]
        public string JustificativaAlteracaoOferta { get; set; }

        /// <summary>
        /// Responsável: nome do usuário responsável pela alteração.
        /// </summary>
        [SMCSize(SMCSize.Grid18_24)]
        [SMCReadOnly]
        [SMCValueEmpty("-")]
        public string UsuarioAlteracaoOferta { get; set; }

        /// <summary>
        /// Data: data e hora da alteração.
        /// </summary>
        [SMCSize(SMCSize.Grid6_24)]
        [SMCReadOnly]
        [SMCValueEmpty("-")]
        [SMCDateTimeMode(SMCDateTimeMode.DateTime)]
        public DateTime? DataAlteracaoOferta { get; set; }

        [SMCHidden]
        public bool OfertasIguais { get; set; }

    }
}