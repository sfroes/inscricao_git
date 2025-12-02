using SMC.Framework.Mapper;
using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class InscricaoOfertasPorProcessoSpecification : SMCSpecification<InscricaoOferta>, ISMCMappable
    {
        public long[] SeqProcessos { get; set; }

        public string TokenEtapa { get; set; }

        public bool? Exportado { get; set; }

        public override Expression<Func<InscricaoOferta, bool>> SatisfiedBy()
        {
            if (SeqProcessos == null)
                SeqProcessos = new long[] { };

            AddExpression(x => SeqProcessos.Contains(x.Inscricao.SeqProcesso));
            AddExpression(TokenEtapa, x => x.HistoricosSituacao.Any(f => f.Atual && f.EtapaProcesso.Token == TokenEtapa));

            if (Exportado.HasValue)
            {
                AddExpression(x => (Exportado == true && x.Exportado == true) || (Exportado == false && (x.Exportado == false || x.Exportado == null)));
            }

            return GetExpression();
        }
    }
}