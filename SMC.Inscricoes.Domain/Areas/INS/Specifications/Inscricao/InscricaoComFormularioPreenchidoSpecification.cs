using SMC.Framework.Specification;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Areas.INS.Exceptions;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class InscricaoComFormularioPreenchidoSpecification : SMCSpecification<Inscricao>
    {
        public InscricaoComFormularioPreenchidoSpecification(long seqConfiguracaoEtapa, long seqFormulario)
        {
            this.SeqConfiguracaoEtapa = seqConfiguracaoEtapa;
            this.SeqFormulario = seqFormulario;
        }

        public long SeqConfiguracaoEtapa { get; set; }

        public long SeqFormulario { get; set; }

        public override Expression<Func<Inscricao, bool>> SatisfiedBy()
        {
            return x => x.SeqConfiguracaoEtapa == SeqConfiguracaoEtapa
                        && x.Formularios.Any(f => f.SeqFormulario == SeqFormulario);
        }
    }
}
