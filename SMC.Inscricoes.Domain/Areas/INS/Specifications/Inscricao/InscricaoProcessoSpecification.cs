using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.INS;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class InscricaoProcessoSpecification : SMCSpecification<InscricaoBoletoTitulo>
    {
        public long SeqProcesso { get; set; }

        public DateTime DataInicio { get; set; }

        public DateTime DataFim { get; set; }

        public override Expression<Func<InscricaoBoletoTitulo, bool>> SatisfiedBy()
        {
            return i => i.InscricaoBoleto.Inscricao.SeqProcesso == SeqProcesso &&
                        i.DataGeracao >= DataInicio && i.DataGeracao < DataFim;
        }

        public InscricaoProcessoSpecification(long seqProcesso, DateTime datainicio, DateTime datafim)
        {
            SeqProcesso = seqProcesso;
            DataInicio = datainicio;
            // Adiciona um dia à pesquisa, pois no banco a data possui hora. A expression foi modificada de i.DataGeracao <= DataFim para i.DataGeracao < DataFim.
            DataFim = datafim.AddDays(1);
        }
    }
}
