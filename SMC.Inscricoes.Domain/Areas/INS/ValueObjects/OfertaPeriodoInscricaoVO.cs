using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    /// <summary>
    /// VO contendo os dados de uma oferta e o número de inscrições confirmadas para a mesma
    /// </summary>
    public class OfertaPeriodoInscricaoVO : ISMCMappable
    {
        public long SeqOferta { get; set; }

        public long SeqProcesso { get; set; }

        public string Descricao { get; set; }      
      
        public DateTime? DataInicio { get; set; }

        public DateTime? DataFim { get; set; }

        public bool ExigePagamentoTaxa { get; set; }

        public bool ExigeEntregaDocumentaaco { get; set; }

        public int Vagas { get; set; }

        public int InscricoesConfirmadas { get; set; }

        public int InscricoesConfirmadasReceberamBolsa { get; set; }

    }
}
