using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using SMC.Inscricoes.Common;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class InscritoConvocadoSGASpecification : SMCSpecification<Inscricao>
    {
        public IEnumerable<long> SeqOfertas { get; set; }

        public override Expression<Func<Inscricao, bool>> SatisfiedBy()
        {
            return x => x.Ofertas.Any(f => SeqOfertas.Contains(f.SeqOferta)
                                        && f.HistoricosSituacao.Any(y => y.Atual && y.TipoProcessoSituacao.Token == TOKENS.SITUACAO_CONVOCADO)
                                        && (!f.Exportado.HasValue || !f.Exportado.Value));
            //return x => x.Inscricoes.Any(
            //                    f => f.Ofertas.Any(
            //                        g => SeqOfertas.Contains(g.SeqOferta)
            //                            // Regra de convocação do SGA. Traz apenas os inscritos na situação CONVOCADO
            //                            && g.HistoricosSituacao.Any(y => y.Atual && y.TipoProcessoSituacao.Token == TOKENS.SITUACAO_CONVOCADO)));
        }
    }
}
