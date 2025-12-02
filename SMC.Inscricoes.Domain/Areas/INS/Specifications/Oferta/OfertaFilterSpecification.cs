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
    public class OfertaFilterSpecification : SMCSpecification<Oferta>
    {

        public long? SeqGrupoOferta { get; set; }

        public long? SeqOferta { get; set; }

        public long[] SeqsOfertas { get; set; }

        public bool? Ativo { get; set; }

        public bool? Vigente { get; set; }

        public long? SeqProcesso { get; set; }

        public bool SemCodigoOrigem { get; set; }

        public override Expression<Func<Oferta, bool>> SatisfiedBy()
        {
            AddExpression(SeqGrupoOferta, o => o.SeqGrupoOferta == this.SeqGrupoOferta);
            AddExpression(SeqOferta, o => o.Seq == this.SeqOferta);
            AddExpression(Ativo, o=> o.Ativo == this.Ativo);
            AddExpression(SeqProcesso, o => o.SeqProcesso == this.SeqProcesso.Value);
            AddExpression(Vigente, o => (this.Vigente.Value && (o.DataInicio <= DateTime.Now && DateTime.Now <= o.DataFim && !o.DataCancelamento.HasValue)));
            AddExpression(SeqsOfertas, o => SeqsOfertas.Contains(o.Seq));
            AddExpression(() => SemCodigoOrigem, x => !x.CodigoOrigem.HasValue);

            return GetExpression();
            /*
            return o => (!this.SeqGrupoOferta.HasValue || o.SeqGrupoOferta == this.SeqGrupoOferta)
                     && (!this.SeqOferta.HasValue || o.Seq == this.SeqOferta)
                     && (!this.Ativo.HasValue || o.Ativo == this.Ativo)
                     && (!this.SeqProcesso.HasValue || o.SeqProcesso == this.SeqProcesso.Value)
                     && (!this.Vigente.HasValue || (this.Vigente.Value &&
                                                    (o.DataInicio <= DateTime.Now && DateTime.Now <= o.DataFim && !o.DataCancelamento.HasValue)));*/
        }
    }
}
