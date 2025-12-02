using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class InscricaoComBoletoTaxaSpecification : SMCSpecification<InscricaoBoletoTaxa>
    {
        public long SeqOferta { get; set; }

        public DateTime DataInicio { get; set; }

        public DateTime DataFim { get; set; }

        public override Expression<Func<InscricaoBoletoTaxa, bool>> SatisfiedBy()
        {
            return i => i.InscricaoBoleto.Inscricao.Ofertas.Any(f => f.SeqOferta == SeqOferta) &&
                        (
                            (i.DataAlteracao.HasValue && i.DataAlteracao >= DataInicio && i.DataAlteracao <= DataFim) ||
                            (i.DataInclusao >= DataInicio && i.DataInclusao <= DataFim)
                        );
        }

        public InscricaoComBoletoTaxaSpecification(long seqOferta, DateTime datainicio, DateTime datafim)
        {
            SeqOferta = seqOferta;
            DataInicio = datainicio;
            // Adiciona um dia à pesquisa, pois no banco a data possui hora. A expression foi modificada de i.DataGeracao <= DataFim para i.DataGeracao < DataFim.
            DataFim = datafim.AddDays(1);
        }
    }
}
