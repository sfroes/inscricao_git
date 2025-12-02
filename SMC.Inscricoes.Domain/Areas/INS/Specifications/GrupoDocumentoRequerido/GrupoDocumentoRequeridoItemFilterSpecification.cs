using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class GrupoDocumentoRequeridoItemFilterSpecification : SMCSpecification<GrupoDocumentoRequeridoItem>
    {
        public long? Seq { get; set; }

        public List<long> Seqs { get; set; }

        public long? SeqGrupoDocumentoRequerido { get; set; }

        public long? SeqDocumentoRequerido { get; set; }

        public long? SeqConfiguracaoEtapa { get; set; }

        public override Expression<Func<GrupoDocumentoRequeridoItem, bool>> SatisfiedBy()
        {
            AddExpression(Seq, x => x.Seq == Seq);
            AddExpression(Seqs, x => Seqs.Contains(x.Seq));
            AddExpression(SeqGrupoDocumentoRequerido, x => x.SeqGrupoDocumentoRequerido == SeqGrupoDocumentoRequerido);
            AddExpression(SeqDocumentoRequerido, x => x.SeqDocumentoRequerido == SeqDocumentoRequerido);
            AddExpression(SeqConfiguracaoEtapa, x => x.GrupoDocumentoRequerido.SeqConfiguracaoEtapa == SeqConfiguracaoEtapa);

            return GetExpression();
        }
    }
}