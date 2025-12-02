using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.INS;
using SMC.Inscricoes.Domain.Areas.NOT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.NOT.Specifications
{
    public class ViewInscricaoEnvioNotificacaoSpecification : SMCSpecification<ViewInscricaoEnvioNotificacao>
    {

        public long? SeqProcesso { get; set; }

        public long? SeqGrupoOferta { get; set; }

        public long? SeqOferta { get; set; }

        public long? SeqTipoNotificacao { get; set; }

        public long? SeqInscricao { get; set; }

        public string Inscrito { get; set; }

        public string Assunto { get; set; }

        public DateTime? DataInicio { get; set; }

        public DateTime? DataFim { get; set; }

        public List<long> SeqUnidadeResponsavel { get; set; }

        public override System.Linq.Expressions.Expression<Func<ViewInscricaoEnvioNotificacao, bool>> SatisfiedBy()
        {
            if (DataFim.HasValue)
            {
                DataFim = DataFim.Value.Add(new TimeSpan(23, 59, 59));
            }

            if (SeqUnidadeResponsavel == null)
                SeqUnidadeResponsavel = new List<long>();

            //return x => (!SeqProcesso.HasValue || x.Inscricao.SeqProcesso == SeqProcesso.Value)
            //            && (!SeqGrupoOferta.HasValue || x.Inscricao.SeqGrupoOferta == SeqGrupoOferta.Value)
            //            && (!SeqOferta.HasValue || x.Inscricao.Ofertas.Any(f => f.SeqOferta == SeqOferta))
            //            && (!SeqTipoNotificacao.HasValue || x.SeqTipoNotificacao == SeqTipoNotificacao.Value)
            //            && (!SeqInscricao.HasValue || x.SeqInscricao == SeqInscricao.Value)
            //            && (string.IsNullOrEmpty(Inscrito) || x.Inscricao.Inscrito.Nome.ToLower().StartsWith(Inscrito.ToLower()) || x.Inscricao.Inscrito.NomeSocial.ToLower().StartsWith(Inscrito.ToLower()))
            //            && (string.IsNullOrEmpty(Assunto) || x.AssuntoNotificacao.Contains(Assunto))
            //            && (!DataInicio.HasValue || !DataFim.HasValue || (DataInicio < x.DataEnvio && x.DataEnvio < DataFim))
            //            && (SeqUnidadeResponsavel.Count == 0 || 
            //                    SeqUnidadeResponsavel.Contains(x.Inscricao.Processo.SeqUnidadeResponsavel));




            AddExpression(SeqProcesso, x => !SeqProcesso.HasValue || x.Inscricao.SeqProcesso == SeqProcesso.Value);
            AddExpression(SeqGrupoOferta, x => !SeqGrupoOferta.HasValue || x.Inscricao.SeqGrupoOferta == SeqGrupoOferta.Value);
            AddExpression(SeqTipoNotificacao, x => !SeqTipoNotificacao.HasValue || x.SeqTipoNotificacao == SeqTipoNotificacao.Value);
            AddExpression(SeqInscricao, x => !SeqInscricao.HasValue || x.SeqInscricao == SeqInscricao.Value);
            AddExpression(Inscrito, x => string.IsNullOrEmpty(Inscrito) || x.Inscricao.Inscrito.Nome.ToLower().StartsWith(Inscrito.ToLower()) || x.Inscricao.Inscrito.NomeSocial.ToLower().StartsWith(Inscrito.ToLower()));
            AddExpression(Assunto, x => string.IsNullOrEmpty(Assunto) || x.AssuntoNotificacao.Contains(Assunto));
            AddExpression(DataInicio, x => !DataInicio.HasValue || !DataFim.HasValue || (DataInicio < x.DataEnvio && x.DataEnvio < DataFim));
            AddExpression(SeqUnidadeResponsavel, x => SeqUnidadeResponsavel.Count == 0 || SeqUnidadeResponsavel.Contains(x.Inscricao.Processo.SeqUnidadeResponsavel));

            return GetExpression();
        }
    }
}
