using SMC.Framework.Specification;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Linq.Expressions;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class InscricaoBoletoTituloFilterSpecification : SMCSpecification<InscricaoBoletoTitulo>
    {
        public long? SeqInscricaoBoleto { get; set; }

        public long? SeqInscricao { get; set; }

        public TipoBoleto? TipoBoleto { get; set; }

        public long? SeqTitulo { get; set; }

        public override Expression<Func<InscricaoBoletoTitulo, bool>> SatisfiedBy()
        {
            return p => (!this.SeqInscricaoBoleto.HasValue || p.SeqInscricaoBoleto == this.SeqInscricaoBoleto) &&
                        (!this.SeqInscricao.HasValue || p.InscricaoBoleto.SeqInscricao == this.SeqInscricao) &&
                        (!this.TipoBoleto.HasValue || p.InscricaoBoleto.TipoBoleto == this.TipoBoleto) &&
                        (!this.SeqTitulo.HasValue || p.SeqTitulo == this.SeqTitulo);
        }
    }
}
