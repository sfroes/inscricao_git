using SMC.Framework.Specification;
using SMC.Inscricoes.Common;
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
    public class InscricaoComOfertaSpecification : SMCSpecification<Inscricao>
    {
        public InscricaoComOfertaSpecification(long seqOferta)
        {
            this.SeqOferta = seqOferta;
        }

        public InscricaoComOfertaSpecification(long seqOferta, DateTime datainicio, DateTime datafim)
            : this(seqOferta)
        {            
            this.DataInicio = datainicio;
            this.DataFim = datafim;
        }
       
        public long SeqOferta { get; set; }

        public DateTime? DataInicio { get; set; }

        public DateTime? DataFim { get; set; }        

        public override Expression<Func<Inscricao, bool>> SatisfiedBy()
        {
            return i => i.Ofertas.Any(x => (x.SeqOferta == SeqOferta))
                        && (!DataInicio.HasValue || !DataFim.HasValue ||
                               i.DataInscricao >= DataInicio.Value && i.DataInscricao <= DataFim.Value); 
        }
    }
}
