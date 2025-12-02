using SMC.Framework.Specification;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class InscritoAtividadeRelatorioFilterSpecification : SMCSpecification<InscricaoOferta>
    {
        public List<long> SeqsOfertas { get; set; }

        public long? SeqProcesso { get; set; }

        public string TokenHistoricoSituacao { get; set; }

        public bool? HistoricoInscricaoAtual { get; set; }


        public override Expression<Func<InscricaoOferta, bool>> SatisfiedBy()
        {            
            AddExpression(SeqsOfertas, inscOferta => SeqsOfertas.Contains(inscOferta.SeqOferta));
            if (HistoricoInscricaoAtual.HasValue && !string.IsNullOrEmpty(TokenHistoricoSituacao))
            {
                AddExpression(a => a.Inscricao.HistoricosSituacao.Where(w => w.Atual == HistoricoInscricaoAtual &&
                                                                                               w.TipoProcessoSituacao.Token == TokenHistoricoSituacao).Any());
            }
            else
            {
                AddExpression(HistoricoInscricaoAtual, a => a.Inscricao.HistoricosSituacao.Where(w => w.Atual == HistoricoInscricaoAtual).Any());
                AddExpression(TokenHistoricoSituacao, a => a.Inscricao.HistoricosSituacao.Where(w => w.TipoProcessoSituacao.Token == TokenHistoricoSituacao).Any());
            }
            return GetExpression();
        }

    }
}
