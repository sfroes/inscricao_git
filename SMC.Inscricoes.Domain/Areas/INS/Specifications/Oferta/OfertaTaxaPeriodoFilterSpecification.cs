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
    public class OfertaTaxaPeriodoFilterSpecification : SMCSpecification<Oferta>
    {

        public OfertaTaxaPeriodoFilterSpecification()
        {
            SetOrderBy(x => x.GrupoOferta.Nome);
            SetOrderBy(x => x.Nome);
        }      

        public long SeqProcesso { get; set; }
        
        public long SeqTipoTaxa { get; set; }

        public bool PossuiTaxa { get; set; }

        public long? SeqGrupoOferta { get; set; }

        private string _periodo { get; set; }

        public string Periodo {
            get { return this._periodo; }
            set 
            {
                _periodo = value;
                var split = value.Split('-');
                DataInicio = DateTime.Parse(split[0]);
                DataFim = DateTime.Parse(split[1]);
            } 
        }

        public bool TemTaxa { get; set; }

        public DateTime DataInicio { get; set; }

        public DateTime DataFim { get; set; }
       
        public override Expression<Func<Oferta, bool>> SatisfiedBy()
        {
            return o => o.SeqProcesso == this.SeqProcesso
                 && (!this.SeqGrupoOferta.HasValue || SeqGrupoOferta==o.SeqGrupoOferta)
                 && ((this.PossuiTaxa && o.Taxas.Any(t => t.SeqTaxa == this.SeqTipoTaxa))
                  || (!this.PossuiTaxa && !o.Taxas.Any(t => t.SeqTaxa == this.SeqTipoTaxa)))
                 && o.DataInicio.Value.Year==this.DataInicio.Year
                 && o.DataInicio.Value.Month == this.DataInicio.Month
                 && o.DataInicio.Value.Day == this.DataInicio.Day
                 && o.DataFim.Value.Year == this.DataFim.Year
                 && o.DataFim.Value.Month == this.DataFim.Month
                 && o.DataFim.Value.Day == this.DataFim.Day;
        }
    }
}
