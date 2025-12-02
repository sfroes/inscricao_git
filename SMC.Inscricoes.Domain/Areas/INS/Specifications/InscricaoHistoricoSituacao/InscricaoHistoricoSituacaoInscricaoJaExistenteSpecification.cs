using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class InscricaoHistoricoSituacaoInscricaoJaExistenteSpecification : SMCSpecification<InscricaoHistoricoSituacao>
    {
        public long SeqInscrito { get; set; }

        public long SeqProcesso { get; set; }

        public long SeqMotivoSituacaoSGF { get; set; }

        public override Expression<Func<InscricaoHistoricoSituacao, bool>> SatisfiedBy()
        {
            return x => x.SeqMotivoSituacaoSGF == SeqMotivoSituacaoSGF &&
                        x.Inscricao.SeqInscrito == SeqInscrito &&
                        x.Inscricao.SeqProcesso == SeqProcesso &&
                        x.Atual == true;
        }
    }
}
