using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework.Specification;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class InscricaoDocumentoEmitidoFilterSpecification : SMCSpecification<InscricaoDocumentoEmitido>
    {
        public long? Seq { get; set; }
        public long? SeqInscricao { get; set; }
        public List<long> SeqInscricoes { get; set; }

        public override Expression<Func<InscricaoDocumentoEmitido, bool>> SatisfiedBy()
        {
            AddExpression(Seq, w => w.Seq == this.Seq.Value);
            AddExpression(SeqInscricao, w => w.SeqInscricao == SeqInscricao);
            AddExpression(SeqInscricoes, w => SeqInscricoes.Contains(w.SeqInscricao));

            return GetExpression();
        }
    }
}
