using SMC.Framework;
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
    public class ProcessoIdiomaFilterSpecification : SMCSpecification<ProcessoIdioma>
    {

        public ProcessoIdiomaFilterSpecification(long seqProcesso, SMCLanguage? idioma = null)
        {
            SeqProcesso = seqProcesso;
            Idioma = idioma;
        }

        public long SeqProcesso { get; set; }

        public SMCLanguage? Idioma { get; set; }

        public override Expression<Func<ProcessoIdioma, bool>> SatisfiedBy()
        {
            return p => (p.SeqProcesso == SeqProcesso 
                && ((!Idioma.HasValue) || (Idioma.HasValue && p.Idioma == Idioma) || (Idioma.HasValue && p.Idioma != Idioma && p.Padrao)));
        }
    }
}
