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
    public class InscricaoOfertaHistoricoPorTokensSpecification : SMCSpecification<InscricaoOfertaHistoricoSituacao>
    {
        public long SeqOferta { get; set; }

        public string[] Tokens { get; set; }

        public override Expression<Func<InscricaoOfertaHistoricoSituacao, bool>> SatisfiedBy()
        {
            return x => x.InscricaoOferta.SeqOferta == SeqOferta &&
                        Tokens.Contains(x.TipoProcessoSituacao.Token) &&
                        x.Atual;
        }
    }
}
