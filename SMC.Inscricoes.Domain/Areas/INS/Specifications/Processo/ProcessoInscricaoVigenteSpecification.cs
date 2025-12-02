using SMC.Framework.Specification;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class ProcessoInscricaoVigenteSpecification : SMCSpecification<Processo>
    {

        public DateTime DataTolerancia { get; set; }

        public ProcessoInscricaoVigenteSpecification(int diasTolerancia = 30)
        {
            DataTolerancia = DateTime.Now.AddDays((-1) * diasTolerancia);
        }

        public override Expression<Func<Processo, bool>> SatisfiedBy()
        {
            return p => p.EtapasProcesso.Any(x => x.Token == TOKENS.ETAPA_INSCRICAO &&
                                                  DateTime.Now >= x.DataInicioEtapa &&
                                                  (DateTime.Now <= x.DataFimEtapa || x.DataFimEtapa >= DataTolerancia));
        }

    }
}
