using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.SEL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.SEL.Specifications
{
    public class InscricaoOfertaHistoricoSituacaoFilterSpecification : SMCSpecification<InscricaoOfertaHistoricoSituacao>
    {
        public long SeqInscricaoOferta { get; set; }

        public long? SeqEtapaProcesso { get; set; }

        public bool? Atual { get; set; }

        public bool? AtualEtapa { get; set; }

        public long? SeqEtapaSGF { get; set; }

        public long? SeqInscricao { get; set; }

        public override Expression<Func<InscricaoOfertaHistoricoSituacao, bool>> SatisfiedBy()
        {
            return i => i.SeqInscricaoOferta == SeqInscricaoOferta
                && (!this.Atual.HasValue || i.Atual == this.Atual.Value)
                && (!this.AtualEtapa.HasValue || i.AtualEtapa == this.AtualEtapa.Value)
                && (!this.SeqEtapaProcesso.HasValue || i.SeqEtapaProcesso == this.SeqEtapaProcesso.Value)
                && (!this.SeqEtapaSGF.HasValue || i.EtapaProcesso.SeqEtapaSGF == this.SeqEtapaSGF.Value)
                && (!this.SeqInscricao.HasValue || i.InscricaoOferta.SeqInscricao == this.SeqInscricao.Value);
        }
    }
}
