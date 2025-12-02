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
    public class PermissaoInscricaoForaPrazoCoincidenteSpecification : SMCSpecification<PermissaoInscricaoForaPrazo>
    {
        public PermissaoInscricaoForaPrazoCoincidenteSpecification()
        {
            SetOrderBy(f => f.DataInicio);            
        }

        public long? Seq { get; set; }

        public long? SeqProcesso { get; set; }

        public string Inscrito { get; set; }        

        public DateTime? DataInicio { get; set; }

        public DateTime? DataFim { get; set; }

        public override Expression<Func<PermissaoInscricaoForaPrazo, bool>> SatisfiedBy()
        {
            return u => (!Seq.HasValue || u.Seq != Seq.Value)
                        && (!SeqProcesso.HasValue || u.SeqProcesso == SeqProcesso.Value)
                        && (string.IsNullOrEmpty(Inscrito) || u.Inscritos.Any(f => f.Inscrito.Nome.ToLower().Contains(Inscrito.ToLower()))
                                                           || u.Inscritos.Any(f => f.Inscrito.NomeSocial.ToLower().Contains(Inscrito.ToLower())))
                        && ((!DataInicio.HasValue || DataInicio.Value <= u.DataFim) && (!DataFim.HasValue || DataFim.Value >= u.DataInicio));
        }
    }
}
