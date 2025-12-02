using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class InscricaoBolsaSocialSpecification : SMCSpecification<Inscricao>
    {
        public long? SeqInscrito { get; set; }

        public long? SeqTipoProcesso { get; set; }

        public List<long> SeqsTiposProcesso { get; set; }

        public string TipoProcessoSituacaoToken { get; set; }

        public List<string> TiposProcessosSituacoesToken { get; set; }

        public string DescricaoGrupoOferta { get; set; }

        public int? AnoReferencia { get; set; }

        public int? SemestreReferencia { get; set; }

        public override Expression<Func<Inscricao, bool>> SatisfiedBy()
        {
            AddExpression(SeqInscrito, x => x.SeqInscrito == SeqInscrito);
            AddExpression(SeqsTiposProcesso, x => SeqsTiposProcesso.Contains(x.Processo.SeqTipoProcesso));
            AddExpression(SeqTipoProcesso, x => x.Processo.SeqTipoProcesso == SeqTipoProcesso);
            AddExpression(TipoProcessoSituacaoToken, x => x.HistoricosSituacao.Any(f => f.Atual && f.TipoProcessoSituacao.Token == TipoProcessoSituacaoToken));
            AddExpression(TiposProcessosSituacoesToken, x => x.HistoricosSituacao.Any(f => f.Atual && TiposProcessosSituacoesToken.Contains(f.TipoProcessoSituacao.Token)));
            AddExpression(AnoReferencia, x => x.Processo.AnoReferencia == AnoReferencia);
            AddExpression(SemestreReferencia, x => x.Processo.SemestreReferencia == SemestreReferencia);

            AddExpression(x => DescricaoGrupoOferta.ToLower().Contains(x.Ofertas.FirstOrDefault().Oferta.HierarquiaOfertaPai.DescricaoCompleta.ToLower()));

            return GetExpression();
        }
    }
}
