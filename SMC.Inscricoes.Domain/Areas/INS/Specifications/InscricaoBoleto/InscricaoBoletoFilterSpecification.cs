using SMC.Framework.Specification;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class InscricaoBoletoFilterSpecification : SMCSpecification<InscricaoBoleto>
    {

        public long SeqInscricao { get; set; }

        public TipoBoleto? TipoBoleto { get; set; }

        public override Expression<Func<InscricaoBoleto, bool>> SatisfiedBy()
        {
            return p => p.SeqInscricao == SeqInscricao
                && (!TipoBoleto.HasValue || TipoBoleto.Value==p.TipoBoleto);
        }
    }
}
