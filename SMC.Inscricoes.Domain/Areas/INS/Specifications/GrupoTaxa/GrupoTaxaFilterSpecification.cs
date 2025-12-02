using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Linq.Expressions;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class GrupoTaxaFilterSpecification : SMCSpecification<GrupoTaxa>
    {
        public GrupoTaxaFilterSpecification()
        {
            SetOrderBy(x => x.Descricao);
        }

        public long? Seq { get; set; }
        
        public long? SeqProcesso { get; set; }

        public string Descricao { get; set; }
                
        public override Expression<Func<GrupoTaxa, bool>> SatisfiedBy()
        {
            AddExpression(Seq, gt => gt.Seq == this.Seq.Value);            
            AddExpression(SeqProcesso, gt => gt.SeqProcesso == this.SeqProcesso.Value);            
            AddExpression(Descricao, gt => gt.Processo.Descricao.Contains(this.Descricao));

            return GetExpression();
        }
    }
}
