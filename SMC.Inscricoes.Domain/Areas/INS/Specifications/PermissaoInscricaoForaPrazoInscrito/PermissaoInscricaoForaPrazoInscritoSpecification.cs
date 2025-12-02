using SMC.Framework.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using SMC.Inscricoes.Domain.Areas.INS.Models;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class PermissaoInscricaoForaPrazoInscritoSpecification : SMCSpecification<PermissaoInscricaoForaPrazoInscrito>
    {
        public long? SeqProcesso { get; set; }

        public long? SeqInscrito { get; set; }

        public long? SeqPermissao { get; set; }

        public DateTime? DataAtual { get; set; }

        public override Expression<Func<PermissaoInscricaoForaPrazoInscrito, bool>> SatisfiedBy()
        {
            return o => (!SeqProcesso.HasValue || o.PermissaoInscricaoForaPrazo.SeqProcesso == SeqProcesso) &&
                        (!SeqInscrito.HasValue || o.SeqInscrito == SeqInscrito) &&
                        (!SeqPermissao.HasValue || o.SeqPermissaoInscricaoForaPrazo == SeqPermissao.Value) &&
                        (!DataAtual.HasValue || o.PermissaoInscricaoForaPrazo.DataInicio <= DataAtual.Value && DataAtual.Value <= o.PermissaoInscricaoForaPrazo.DataFim);
        }
    }
}
