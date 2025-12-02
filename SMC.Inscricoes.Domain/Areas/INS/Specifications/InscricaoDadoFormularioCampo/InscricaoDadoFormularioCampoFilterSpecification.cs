using SMC.Framework.Mapper;
using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class InscricaoDadoFormularioCampoFilterSpecification : SMCSpecification<InscricaoDadoFormularioCampo>
    {
        public long? SeqElemento { get; set; }

        public long? Seq { get; set; }

        public string Valor { get; set; }

        [SMCMapProperty("SeqFilter")]
        public long? SeqProcesso { get; set; }

        public List<long> SeqsMotivosDiferente { get; set; }

        public InscricaoDadoFormularioCampoFilterSpecification()
        {
        }

        public override Expression<Func<InscricaoDadoFormularioCampo, bool>> SatisfiedBy()
        {
            AddExpression(Seq, x => x.Seq == Seq);
            AddExpression(Valor, x => x.Valor.Contains(Valor));
            AddExpression(SeqElemento, x => x.SeqElemento == SeqElemento);
            AddExpression(SeqProcesso, x => x.InscricaoDadoFormulario.Inscricao.SeqProcesso == SeqProcesso);
            AddExpression(SeqsMotivosDiferente, x => !SeqsMotivosDiferente.Contains(x.InscricaoDadoFormulario.Inscricao.HistoricosSituacao.FirstOrDefault(h => h.Atual).SeqMotivoSituacaoSGF.Value));

            return GetExpression();
        }
    }
}