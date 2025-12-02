using SMC.Framework.Specification;
using SMC.Inscricoes.Common.Areas.INS.Exceptions;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class InscricaoTaxaComNumeroMinimoSpecification : SMCSpecification<Inscricao>
    {
        public InscricaoTaxaComNumeroMinimoSpecification(long seqOferta, long seqTaxa, int numeroMinimo, DateTime datainicio, DateTime datafim)
        {
            SeqOferta = seqOferta;
            NumeroMinimo = numeroMinimo;
            SeqTaxa = seqTaxa;
            DataInicio = datainicio;
            DataFim = datafim;
        }

        public long SeqOferta { get; set; }

        public int NumeroMinimo { get; set; }

        public long SeqTaxa { get; set; }

        public DateTime? DataInicio { get; set; }

        public DateTime? DataFim { get; set; }

        public override Expression<Func<Inscricao, bool>> SatisfiedBy()
        {
            return i => i.Boletos.Any(b => b.Taxas.Any(t => t.SeqTaxa == SeqTaxa && t.NumeroItens < NumeroMinimo
                                                            && i.Ofertas.Any(x => x.NumeroOpcao == 1 && x.SeqOferta == SeqOferta)
                                                            && (!DataInicio.HasValue || !DataFim.HasValue || 
                                                                (
                                                                    (i.DataAlteracao.HasValue && i.DataAlteracao >= DataInicio && i.DataAlteracao <= DataFim)
                                                                    || (i.DataInclusao >= DataInicio && i.DataInclusao <= DataFim))
                                                                )));
        }
    }

    public class InscricaoTaxaComNumeroMaximoSpecification : SMCSpecification<Inscricao>
    {
        public InscricaoTaxaComNumeroMaximoSpecification(long seqOferta, long seqTaxa, int numeroMaximo, DateTime datainicio, DateTime datafim)
        {
            SeqOferta = seqOferta;
            NumeroMaximo = numeroMaximo;
            SeqTaxa = seqTaxa;
            DataInicio = datainicio;
            DataFim = datafim;
        }

        public long SeqOferta { get; set; }

        public int NumeroMaximo { get; set; }

        public long SeqTaxa { get; set; }

        public DateTime? DataInicio { get; set; }

        public DateTime? DataFim { get; set; }

        public override Expression<Func<Inscricao, bool>> SatisfiedBy()
        {
            return i => i.Boletos.Any(b => b.Taxas.Any(t => t.SeqTaxa == SeqTaxa && t.NumeroItens > NumeroMaximo
                                                            && i.Ofertas.Any(x => x.NumeroOpcao == 1 && x.SeqOferta == SeqOferta)
                                                            && (!DataInicio.HasValue || !DataFim.HasValue ||
                                                                (
                                                                    (i.DataAlteracao.HasValue && i.DataAlteracao >= DataInicio && i.DataAlteracao <= DataFim)
                                                                    || (i.DataInclusao >= DataInicio && i.DataInclusao <= DataFim))
                                                                )));
        }
    }
}
