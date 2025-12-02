using SMC.Framework.Specification;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class InscricaoOfertaFilterSpecification : SMCSpecification<InscricaoOferta>
    {
        public long? SeqInscricaoOferta { get; set; }

        public long? SeqInscricao { get; set; }

        public short? NumeroOpcao { get; set; }

        public long? SeqOferta { get; set; }

        public List<long> Seqs { get; set; }

        public long? Seq { get; set; }
        public Guid? Guid { get; set; }
        public string NomeInscrito { get; set; }
        public string Cpf { get; set; }
        public List<long> SeqsOfertas { get; set; }
        public bool? HistoricoInscricaoAtual { get; set; }
        public string TokenHistoricoSituacao { get; set; }
        public bool? VerificaPossuiCheckin { get; set; }
        public bool? CheckinRealizado { get; set; }

        public string Token { get; set; }

        public List<long> SeqsInscricao { get; set; }

        public long? SeqInscrito { get; set; }

        public bool? Atual { get; set; }

        public override Expression<Func<InscricaoOferta, bool>> SatisfiedBy()
        {
            AddExpression(this.Seq, x => x.Seq == Seq);
            AddExpression(this.SeqInscricaoOferta, x => x.Seq == SeqInscricaoOferta);
            AddExpression(this.Seqs, x => this.Seqs.Contains(x.Seq));
            AddExpression(this.SeqInscricao, x => this.SeqInscricao == x.SeqInscricao);
            AddExpression(this.NumeroOpcao, x => this.NumeroOpcao == x.NumeroOpcao);
            AddExpression(this.SeqOferta, x => this.SeqOferta == x.SeqOferta);
            AddExpression(Guid, a => a.UidInscricaoOferta == Guid);
            AddExpression(NomeInscrito, a => a.Inscricao.Inscrito.Nome.Contains(NomeInscrito));
            AddExpression(Cpf, a => a.Inscricao.Inscrito.Cpf == Cpf);
            AddExpression(SeqsOfertas, a => SeqsOfertas.Contains(a.SeqOferta));
            AddExpression(SeqsInscricao, a => SeqsInscricao.Contains(a.SeqInscricao));
            AddExpression(Token, x => x.Inscricao.HistoricosSituacao.Any(h => h.Atual == true && h.TipoProcessoSituacao.Token == Token));
            AddExpression(SeqInscrito, x => x.Inscricao.SeqInscrito == this.SeqInscrito);

            if (Atual.HasValue)
            {
                AddExpression(x => x.HistoricosSituacao.Any(f => f.Atual));

            }

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

            if (VerificaPossuiCheckin.HasValue && VerificaPossuiCheckin.Value)
            {
                AddExpression(a => a.DataCheckin != null || a.TipoCheckin != null && a.TipoCheckin != TipoCheckin.Nenhum);
            }


            if (CheckinRealizado.HasValue)
            {
                if (CheckinRealizado.Value)
                {

                    AddExpression(a => a.DataCheckin != null || a.TipoCheckin != null && a.TipoCheckin != TipoCheckin.Nenhum);

                }
                else
                {
                    AddExpression(a => a.DataCheckin == null || a.TipoCheckin != null && a.TipoCheckin == TipoCheckin.Nenhum);
                }

            }

            return GetExpression();
        }
    }
}
