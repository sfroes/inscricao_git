using SMC.Framework.Mapper;
using System;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class TaxaVO : ISMCMappable
    {
        public long Seq { get; set; }

        public long SeqProcesso { get; set; }

        public DateTime DataInclusao { get; set; }

        public virtual long SeqTipoTaxa { get; set; }

        public virtual string DescricaoComplementar { get; set; }
    }

}
