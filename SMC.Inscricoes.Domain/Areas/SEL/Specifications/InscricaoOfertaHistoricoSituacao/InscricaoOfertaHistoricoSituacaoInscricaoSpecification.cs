using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.SEL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace SMC.Inscricoes.Domain.Areas.SEL.Specifications
{
    public class InscricaoOfertaHistoricoSituacaoInscricaoSpecification : SMCSpecification<InscricaoOfertaHistoricoSituacao>
    {
        public InscricaoOfertaHistoricoSituacaoInscricaoSpecification(long seqInscricao)
        {
            SeqInscricao = seqInscricao;
        }
        public long SeqInscricao { get; set; }

        public override Expression<Func<InscricaoOfertaHistoricoSituacao, bool>> SatisfiedBy()
        {
            return x => x.InscricaoOferta.SeqInscricao == SeqInscricao;
        }
    }
}
