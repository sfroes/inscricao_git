using SMC.DadosMestres.Common.Constants;
using SMC.Framework.Mapper;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class InscricaoTaxaVO : ISMCMappable
    {
        public long SeqTaxa { get; set; }

        public short NumeroItens { get; set; }

        public decimal ValorItem { get; set; }

        public long SeqOferta { get; set; }

        public TipoCobranca TipoCobranca { get; set; }

    }
}