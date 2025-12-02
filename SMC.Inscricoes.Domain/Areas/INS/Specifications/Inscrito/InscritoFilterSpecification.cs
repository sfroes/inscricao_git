using SMC.Framework;
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
    public class InscritoFilterSpecification : SMCSpecification<Inscrito>
    {
        public InscritoFilterSpecification()
        {
            SetOrderBy(f => f.Nome);
        }

        /// <summary>
        /// Sequencial do usuário do SAS
        /// </summary>
        public long? SeqUsuarioSas { get; set; }

        public string Inscrito { get; set; }

        public string CPF { get; set; }

        public string Passaporte { get; set; }

        public long? Processo { get; set; }

        public long? Situacao { get; set; }
        public long? Seq { get; set; }
        public string Nome { get; set; }
        /// <summary>
        /// Realiza a pesquisa
        /// </summary>
        public override Expression<Func<Inscrito, bool>> SatisfiedBy()
        {
            var cpf = CPF.SMCRemoveNonDigits();
   


            return i => (!SeqUsuarioSas.HasValue || i.SeqUsuarioSas == this.SeqUsuarioSas)
                        && (string.IsNullOrEmpty(Inscrito) || i.Nome.ToLower().Contains(Inscrito.ToLower()) || i.NomeSocial.ToLower().Contains(Inscrito.ToLower()))
                        && (string.IsNullOrEmpty(CPF) || i.Cpf == cpf)
                        && (string.IsNullOrEmpty(Passaporte) || i.NumeroPassaporte == Passaporte)
                        && (!Processo.HasValue ||
                            i.Inscricoes.Any(f => f.Processo.Seq == Processo.Value
                                                  && !Situacao.HasValue || f.HistoricosSituacao.OrderByDescending(o => o.DataSituacao).FirstOrDefault().SeqTipoProcessoSituacao == Situacao))
                                                  && (!Seq.HasValue || i.Seq == this.Seq)
                                                  && (string.IsNullOrEmpty(Nome) || i.Nome == this.Nome);
        }
    }
}
