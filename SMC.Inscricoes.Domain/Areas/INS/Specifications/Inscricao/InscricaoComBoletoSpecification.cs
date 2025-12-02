using SMC.Framework.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using SMC.Inscricoes.Domain.Areas.INS.Models;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class InscricaoComBoletoSpecification : SMCSpecification<InscricaoBoletoTitulo>
    {
        public long SeqOferta { get; set; }

        public DateTime DataInicio { get; set; }

        public DateTime DataFim { get; set; }

        public override Expression<Func<InscricaoBoletoTitulo, bool>> SatisfiedBy()
        {
            return i => i.InscricaoBoleto.Inscricao.Ofertas.Any(f => f.SeqOferta == SeqOferta) &&
                        i.DataGeracao >= DataInicio && i.DataGeracao < DataFim;
        }

        public InscricaoComBoletoSpecification(long seqOferta, DateTime datainicio, DateTime datafim)
        {
            SeqOferta = seqOferta;
            DataInicio = datainicio;
            // Adiciona um dia à pesquisa, pois no banco a data possui hora. A expression foi modificada de i.DataGeracao <= DataFim para i.DataGeracao < DataFim.
            DataFim = datafim.AddDays(1);
        }
    }
}
