using SMC.Framework.DataAnnotations;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using System;

namespace SMC.GPI.Administrativo.Areas.SEL.Models
{
    public class ConsultaCandidatosProcessoListaViewModel : SMCViewModelBase
    {
        public long SeqInscricaoOferta { get; set; }

        [SMCSortable]
        public long SeqInscricao { get; set; }

        public long SeqInscrito { get; set; }

        public long SeqProcesso { get; set; }

        public long SeqGrupoOferta { get; set; }

        public long SeqOferta { get; set; }

        public long? SeqOfertaOriginal { get; set; }

        [SMCSortable(true, true, "Inscricao.Inscrito.Nome")]
        public string Candidato { get; set; }

        [SMCSortable(true, false, "NumeroOpcao")]
        public string Opcao { get; set; }

        public string Oferta { get; set; }

        public bool PossuiJustificativa { get; set; }

        public long? SeqInscricaoHistoricoSituacao { get; set; }
        
        public string Situacao { get; set; }

        [SMCSortable(true, false, "ValorNota")]
        public decimal? Nota { get; set; }

        [SMCSortable(true, false, "ValorSegundaNota")]
        public decimal? SegundaNota { get; set; }

        [SMCSortable(true, false, "NumeroClassificacao")]
        public int? Classificacao { get; set; }

        [SMCDateTimeMode(Framework.SMCDateTimeMode.DateTime)]
        [SMCSortable(true, false, "Inscricao.DataInscricao")]
        public DateTime DataInscricao { get; set; }

        [SMCHidden]
        public string BackURL { get; set; }
    }
}