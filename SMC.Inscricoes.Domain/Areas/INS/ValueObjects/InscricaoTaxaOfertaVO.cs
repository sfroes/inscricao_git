using SMC.Framework.Mapper;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using System.Collections.Generic;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class InscricaoTaxaOfertaVO : ISMCMappable
    {
        public long? SeqInscricaoBoleto { get; set; }

        public long SeqTaxa { get; set; }

        public string Descricao { get; set; }

        public string DescricaoComplementar { get; set; }

        public short? NumeroItens { get; set; }

        public short? NumeroMinimo { get; set; }

        public short? NumeroMaximo { get; set; }

        public int SeqEventoTaxa { get; set; }

        public decimal ValorEventoTaxa { get; set; }

        public decimal? ValorTitulo { get; set; }

        public bool? CobrarPorOferta { get; set; }

        public  TipoCobranca TipoCobranca { get; set; }
        public long? SeqOferta { get; set; }
        public bool PossuiGrupoTaxas { get; set; }

        public GrupoTaxaVO GrupoTaxa { get; set; }

    }
}