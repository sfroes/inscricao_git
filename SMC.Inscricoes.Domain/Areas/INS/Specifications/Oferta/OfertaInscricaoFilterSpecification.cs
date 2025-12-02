using SMC.Framework.Specification;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class OfertaInscricaoFilterSpecification : SMCSpecification<Oferta>
    {
        public OfertaInscricaoFilterSpecification(DateTime data, long seqUsuario)
        {
            SetOrderBy(x => x.Nome);

            DataOferta = data;

            SeqUsuario = seqUsuario;
        }

        public long SeqUsuario { get; set; }

        public long? SeqGrupoOferta { get; set; }

        public DateTime DataOferta { get; set; }

        public bool? ControlaVagaInscricao { get; set; }

        /// <summary>
        /// Realiza a pesquisa
        /// </summary>
        public override Expression<Func<Oferta, bool>> SatisfiedBy()
        {
            AddExpression(SeqGrupoOferta, o => o.SeqGrupoOferta.HasValue && o.SeqGrupoOferta == SeqGrupoOferta);
            AddExpression(o => (DataOferta >= o.DataInicio && DataOferta <= o.DataFim) ||
                            // Verifica por inscrições fora do prazo
                            o.Processo.PermissoesInscricaoForaPrazo.Any(f => f.Inscritos.Any(x => x.SeqInscrito == SeqUsuario) &&
                                                                        f.DataFim >= DateTime.Now && f.DataInicio <= DateTime.Now));
            AddExpression(o => !o.DataCancelamento.HasValue || o.DataCancelamento.Value > DataOferta);
            AddExpression(o => o.Ativo);

            if (ControlaVagaInscricao.GetValueOrDefault())
            {
                AddExpression(o => o.NumeroVagas > o.InscricoesOferta.Count(io => 
                    io.Inscricao.HistoricosSituacao.Any(f => f.Atual && 
                                                        (f.TipoProcessoSituacao.Token == TOKENS.SITUACAO_INSCRICAO_CONFIRMADA || 
                                                         f.TipoProcessoSituacao.Token == TOKENS.SITUACAO_INSCRICAO_DEFERIDA ||
                                                         f.TipoProcessoSituacao.Token == TOKENS.SITUACAO_INSCRICAO_FINALIZADA))));
            }

            return GetExpression();
        }
    }
}
