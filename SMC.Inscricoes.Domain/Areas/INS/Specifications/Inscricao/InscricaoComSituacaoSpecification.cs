using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class InscricaoComSituacaoSpecification : SMCSpecification<Inscricao>
    {
        public InscricaoComSituacaoSpecification()
        {
        }

        public long? SeqConfiguracaoEtapa { get; set; }

        public string Token { get; set; }

        public bool ConsiderarGrupoOfertaDaConfiguracao { get; set; }

        public override Expression<Func<Inscricao, bool>> SatisfiedBy()
        {
            AddExpression(SeqConfiguracaoEtapa, x => x.SeqConfiguracaoEtapa == SeqConfiguracaoEtapa);
            AddExpression(() => ConsiderarGrupoOfertaDaConfiguracao && SeqConfiguracaoEtapa.HasValue, x => x.ConfiguracaoEtapa.GruposOferta.Any(i => i.SeqGrupoOferta == x.SeqGrupoOferta));
            AddExpression(Token, x => x.HistoricosSituacao.Any(h => h.TipoProcessoSituacao.Token == Token));

            return GetExpression();
        }
    }
}