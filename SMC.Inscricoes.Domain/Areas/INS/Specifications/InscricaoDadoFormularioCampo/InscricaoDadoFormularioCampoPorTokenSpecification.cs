using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Linq.Expressions;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class InscricaoDadoFormularioCampoPorTokenSpecification : SMCSpecification<InscricaoDadoFormularioCampo>
    {
        public InscricaoDadoFormularioCampoPorTokenSpecification(long seqInscricao, string token)
        {
            SeqInscricao = seqInscricao;
            Token = token;
        }

        public long SeqInscricao { get; private set; }

        public string Token { get; private set; }

        public override Expression<Func<InscricaoDadoFormularioCampo, bool>> SatisfiedBy()
        {
            AddExpression(x => x.InscricaoDadoFormulario.SeqInscricao == SeqInscricao);
            AddExpression(x => x.Token == Token);
            return GetExpression();
        }
    }
}