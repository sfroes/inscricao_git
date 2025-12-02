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
    public class GrupoOfertaFilterSpecification : SMCSpecification<GrupoOferta>
    {

        public GrupoOfertaFilterSpecification()
        {
            SetOrderBy(x => x.Nome);
        }

        public long? Seq { get; set; }

        /// <summary>
        /// Sequencial do usuário do SAS
        /// </summary>
        public long? SeqProcesso { get; set; }

        public string Nome { get; set; }

        public long? SeqConfiguracaoEtapa { get; set; }

        public long? SeqEtapaProcesso { get; set; }

        /// <summary>
        /// Realiza a pesquisa
        /// </summary>
        public override Expression<Func<GrupoOferta, bool>> SatisfiedBy()
        {

            return g => (!Seq.HasValue || g.Seq == Seq)
                        && (!SeqProcesso.HasValue || g.SeqProcesso == SeqProcesso)
                        && (!SeqConfiguracaoEtapa.HasValue || !SeqEtapaProcesso.HasValue ||
                                    g.ConfiguracoesEtapa.Any(x => x.ConfiguracaoEtapa.SeqEtapaProcesso==SeqEtapaProcesso.Value
                                    && x.SeqConfiguracaoEtapa == SeqConfiguracaoEtapa.Value))
                        && (string.IsNullOrEmpty(Nome) || g.Nome.ToLower().Contains(Nome.ToLower()));
        }
    }
}
