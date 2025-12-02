using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class OfertaPeriodoTaxaFilterSpecification : SMCSpecification<OfertaPeriodoTaxa>
    {
        public long? SeqProcesso { get; set; }  
        public long? SeqOferta { get; set; }
        public bool? SelecaoInscricao { get; set; }
        public long? SeqTaxa { get; set; }
        public long? SeqGrupoOferta { get; set; }
        public List<long> SeqsTaxa { get; set; } = new List<long>();        

        public DateTime? DataReferencia { get; set; }

        public override Expression<Func<OfertaPeriodoTaxa, bool>> SatisfiedBy()
        {
            AddExpression(SelecaoInscricao, x => x.Taxa.SelecaoInscricao == SelecaoInscricao);
            AddExpression(SeqTaxa, x => x.SeqTaxa == SeqTaxa);
            AddExpression(SeqsTaxa, x => SeqsTaxa.Contains(x.SeqTaxa));            
            AddExpression(SeqGrupoOferta, x => x.Oferta.SeqGrupoOferta == SeqGrupoOferta);
            AddExpression(SeqOferta, x => x.SeqOferta == SeqOferta);
            AddExpression(SeqProcesso, x => x.Oferta.SeqProcesso == SeqProcesso);
            AddExpression(DataReferencia, x => x.DataInicio <= DataReferencia && x.DataFim >= DataReferencia);
            return GetExpression();
        }
    }
}