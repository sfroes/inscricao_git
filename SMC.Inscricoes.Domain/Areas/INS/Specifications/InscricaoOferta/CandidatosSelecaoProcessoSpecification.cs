using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class CandidatosSelecaoProcessoSpecification : SMCSpecification<InscricaoOferta>
    {
        public long SeqProcesso { get; set; }

        public long? SeqSituacao { get; set; }

        public long? SeqMotivo { get; set; }

        public long? SeqGrupoOferta { get; set; }

        public long? SeqItemHierarquiaOferta { get; set; }

        public long? SeqOferta { get; set; }

        public short? Opcao { get; set; }

        public long? SeqInscricao { get; set; }

        public string Candidato { get; set; }

        public long? SeqTipoProcessoSituacao { get; set; }

        public override Expression<Func<InscricaoOferta, bool>> SatisfiedBy()
        {
            AddExpression(x => x.Inscricao.SeqProcesso == SeqProcesso);
            if (SeqSituacao.HasValue || SeqMotivo.HasValue || SeqTipoProcessoSituacao.HasValue)
            {
                AddExpression(x => x.HistoricosSituacao.Any(f => f.Atual
                                                              && (!SeqSituacao.HasValue || f.TipoProcessoSituacao.SeqSituacao == SeqSituacao.Value)
                                                              && (!SeqMotivo.HasValue || f.SeqMotivoSituacao == SeqMotivo.Value)
                                                              && (!SeqTipoProcessoSituacao.HasValue || f.SeqTipoProcessoSituacao == SeqTipoProcessoSituacao)));
            }
            else
            {
                AddExpression(x => x.HistoricosSituacao.Any(f => f.Atual));
            }
            AddExpression(SeqGrupoOferta, x => x.Inscricao.SeqGrupoOferta == SeqGrupoOferta);
            AddExpression(SeqOferta, x => x.SeqOferta == SeqOferta);
            AddExpression(Opcao, x => x.NumeroOpcao == Opcao);
            AddExpression(SeqInscricao, x => x.SeqInscricao == SeqInscricao);
            AddExpression(Candidato, x => x.Inscricao.Inscrito.Nome.ToLower().Contains(Candidato.ToLower()) || x.Inscricao.Inscrito.NomeSocial.ToLower().Contains(Candidato.ToLower()));

            return GetExpression();
        }
    }
}