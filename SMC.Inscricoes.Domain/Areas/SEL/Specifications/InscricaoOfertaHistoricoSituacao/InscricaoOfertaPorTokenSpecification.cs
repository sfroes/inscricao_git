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
    public class InscricaoOfertaPorTokenSpecification : SMCSpecification<InscricaoOfertaHistoricoSituacao>
    {
        public List<long> Inscricoes { get; set; }

        public string Token { get; set; }

        public override Expression<Func<InscricaoOfertaHistoricoSituacao, bool>> SatisfiedBy()
        {
            return x => x.TipoProcessoSituacao.Token != Token &&
                        x.Atual == true &&
                        Inscricoes.Contains(x.InscricaoOferta.SeqInscricao);
        }
    }
}
