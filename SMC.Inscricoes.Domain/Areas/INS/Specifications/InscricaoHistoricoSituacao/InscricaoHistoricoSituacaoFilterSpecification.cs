using SMC.Framework.Specification;
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
    public class InscricaoHistoricoSituacaoFilterSpecification : SMCSpecification<InscricaoHistoricoSituacao>
    {
        public long? SeqInscricao { get; set; }

        public long? SeqEtapaProcesso { get; set; }

        public bool? Atual { get; set; }

        public bool? AtualEtapa { get; set; }

        public long? SeqEtapaSGF { get; set; }

        public List<long> SeqsInscricoes { get; set; }

        public string TokenTipoProcessoSituacao { get; set; }

        public override Expression<Func<InscricaoHistoricoSituacao, bool>> SatisfiedBy()
        {
            AddExpression(SeqInscricao, i => i.SeqInscricao == SeqInscricao);
            AddExpression(Atual, i => i.Atual == Atual);
            AddExpression(AtualEtapa, i => i.AtualEtapa == AtualEtapa);
            AddExpression(SeqEtapaProcesso, i => i.SeqEtapaProcesso == SeqEtapaProcesso);
            AddExpression(SeqEtapaSGF, i => i.EtapaProcesso.SeqEtapaSGF == SeqEtapaSGF);
            AddExpression(SeqsInscricoes, i => SeqsInscricoes.Contains(i.SeqInscricao));
            AddExpression(TokenTipoProcessoSituacao, i => i.TipoProcessoSituacao.Token == TokenTipoProcessoSituacao);


            return GetExpression();
        }
    }
}
