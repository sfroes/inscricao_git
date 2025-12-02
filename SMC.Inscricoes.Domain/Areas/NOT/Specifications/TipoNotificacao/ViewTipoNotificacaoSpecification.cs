using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.NOT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.NOT.Specifications
{
    public class ViewTipoNotificacaoSpecification : SMCSpecification<ViewTipoNotificacao>
    {
        public string Descricao { get; set; }

        public bool? PermiteAgendamento { get; set; }

        public string Token { get; set; }

        public override Expression<Func<ViewTipoNotificacao, bool>> SatisfiedBy()
        {
            return x => (string.IsNullOrEmpty(Descricao) || x.Descricao.Contains(Descricao))
                         && (!PermiteAgendamento.HasValue || x.PermiteAgendamento == PermiteAgendamento.Value)
                         && (string.IsNullOrEmpty(Token) || x.Token == Token);
        }
    }
}
