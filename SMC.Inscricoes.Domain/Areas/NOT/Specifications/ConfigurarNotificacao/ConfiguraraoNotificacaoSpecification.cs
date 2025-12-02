using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.INS;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.NOT.Specifications
{
    public class ConfiguraraoNotificacaoSpecification : SMCSpecification<ProcessoConfiguracaoNotificacao>
    {
        public long SeqProcesso { get; set; }

        public override System.Linq.Expressions.Expression<Func<ProcessoConfiguracaoNotificacao, bool>> SatisfiedBy()
        {
            return x => x.SeqProcesso == SeqProcesso;
        }
    }
}
