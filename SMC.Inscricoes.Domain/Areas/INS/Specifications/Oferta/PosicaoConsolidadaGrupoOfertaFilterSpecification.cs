using SMC.Framework.Specification;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Inscricoes.Domain.Areas.INS;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    /// <summary>
    /// Specification para filtrar a posiçao consolidada por oferta
    /// Sempre filtra pelo Seq do Processo
    /// Se o filtro de SeqGrupoOferta for setado é utilizado
    /// Se o filtro de SeqOferta for setado é utilizado 
    /// </summary>
    public class PosicaoConsolidadaGrupoOfertaFilterSpecification : SMCSpecification<Oferta>
    {
        public long SeqProcesso { get; set; }

        public long? SeqGrupoOferta { get; set; }
        
        // Não é usado no SatisfiedBy mas é usado no DomainService posteriormente
        public long? SeqOferta { get; set; }

        // Não é usado no SatisfiedBy mas é usado no DomainService posteriormente
        public long? SeqItemHierarquiaOferta { get; set; }

        /// <summary>
        /// Realiza a pesquisa
        /// Sempre filtra pelo Seq do Processo
        /// Se o filtro de SeqGrupoOferta for setado é utilizado
        /// Se o filtro de SeqOferta for setado é utilizado 
        /// </summary>
        public override System.Linq.Expressions.Expression<Func<Oferta, bool>> SatisfiedBy()
        {
            return o => (o.SeqProcesso == SeqProcesso
                && (!SeqGrupoOferta.HasValue || (o.SeqGrupoOferta.HasValue && (o.SeqGrupoOferta.Value==SeqGrupoOferta.Value))));
        }
    }
}
