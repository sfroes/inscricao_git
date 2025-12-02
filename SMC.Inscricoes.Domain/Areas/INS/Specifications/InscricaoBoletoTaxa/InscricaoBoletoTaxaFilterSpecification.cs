using SMC.Framework.Specification;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class InscricaoBoletoTaxaFilterSpecification : SMCSpecification<InscricaoBoletoTaxa>
    {

        public long? SeqInscricaoBoleto { get; set; }

        public long? SeqTaxa { get; set; }

        public long? SeqOferta { get; set; }

        public long? SeqInscricao { get; set; }


        public override Expression<Func<InscricaoBoletoTaxa, bool>> SatisfiedBy()
        {
            AddExpression(SeqInscricaoBoleto, x => x.SeqInscricaoBoleto == SeqInscricaoBoleto);
            AddExpression(SeqInscricao, x => x.InscricaoBoleto.SeqInscricao == SeqInscricao);
            AddExpression(SeqTaxa, x => x.SeqTaxa == SeqTaxa);
            AddExpression(SeqOferta, x => x.InscricaoBoleto.Inscricao.Ofertas.Any(o => o.SeqOferta == SeqOferta.Value && o.NumeroOpcao == 1));
                
            return GetExpression();
        }
    }
}
