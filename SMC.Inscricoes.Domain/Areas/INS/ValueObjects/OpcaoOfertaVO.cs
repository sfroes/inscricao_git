using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    /// <summary>
    /// Value Objec uma opção de oferta de um processo
    /// </summary>
    public class OpcaoOfertaVO : ISMCMappable
    {
        public long SeqOferta { get; set; }
        public int NumeroOpcao { get; set; }
        public string NumeroOpcaoFormatado { get; set; }

        public string DescricaoOferta { get; set; }
        public string Descricao { get; set; }
        public string DescricaoOfertaCompleta { get; set; }



        public string Justificativa { get; set; }

        public string HierarquiaCompleta { get; set; }

        public string SituacaoOferta { get; set; }
        public long SeqProcesso { get; set; }
        public List<long> InscricaoOferta { get; set; }
        public long SeqInscricao { get; set; }
        public bool ExibirOpcoes { get; set; }
        public long SeqInscricaoOferta { get; set; }
        public long SeqInscrito { get; set; }
        public string NomeInscrito { get; set; }

        public DateTime? DataInicioAtividade { get; set; }

        public DateTime? DataFimAtividade { get; set; }

        public int? CargaHorariaAtividade { get; set; }

        public bool? ExibirPeriodoAtividadeOferta { get; set; }

        #region[Oferta Original]
        public string DescricaoOfertaOriginal { get; set; }
        public string DescricaoOfertaCompletaOriginal { get; set; }

        public DateTime? DataInicioAtividadeOfertaOriginal { get; set; }

        public DateTime? DataFimAtividadeOfertaOriginal { get; set; }

        public int? CargaHorariaAtividadeOfertaOriginal { get; set; }

        public bool? ExibirPeriodoAtividadeOfertaOfertaOriginal { get; set; }
        #endregion

    }
}
