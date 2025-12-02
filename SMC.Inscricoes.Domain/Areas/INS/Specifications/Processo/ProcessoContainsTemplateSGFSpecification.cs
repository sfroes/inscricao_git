using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class ProcessoContainsTemplateSGFSpecification : SMCContainsSpecification<Processo, long>
    {        

        public ProcessoContainsTemplateSGFSpecification(long[] templatesSGF)
            : base(p => p.SeqTemplateProcessoSGF, templatesSGF)
        { }
    }
}
