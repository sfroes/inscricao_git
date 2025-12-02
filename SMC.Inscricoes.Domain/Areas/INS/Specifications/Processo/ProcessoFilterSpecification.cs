using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class ProcessoFilterSpecification : SMCSpecification<Processo>
    {
        public ProcessoFilterSpecification()
        {
            SetOrderByDescending(x => x.DataInclusao);
        }

        public Guid? UidProcesso { get; set; }

        public long? SeqProcesso { get; set; }

        public long[] SeqsProcessos { get; set; }

        public string DescricaoProcesso { get; set; }

        public long? SeqTipoProcesso { get; set; }

        public long? SeqUnidadeResponsavel { get; set; }

        public long? SeqCliente { get; set; }

        public long? SeqTipoHierarquiaOferta { get; set; }

        public List<long?> SeqsTiposHierarquiaOfertas { get; set; }

        public int? AnoReferencia { get; set; }

        public int? SemestreReferencia { get; set; }

        public DateTime? DataInicio { get; set; }

        public DateTime? DataFim { get; set; }

        public string[] TokenEtapa { get; set; }

        public long? TipoProcesso { get; set; }

        public bool? IntegraGPC { get; set; }

        public bool? UidGedPreenchido { get; set; }

        public bool? GestaoEventos { get; set; }

        public DateTime? DataProcessoEventoCorrente { get; set; }

        public long? SeqInscrito { get; set; }

        public override Expression<Func<Processo, bool>> SatisfiedBy()
        {
            AddExpression(UidProcesso, x => x.UidProcesso == UidProcesso);
            AddExpression(SeqProcesso, x => x.Seq == SeqProcesso);
            AddExpression(SeqsProcessos, x => SeqsProcessos.Contains(x.Seq));

            AddExpression(DescricaoProcesso, x => x.Descricao.Contains(DescricaoProcesso));
            AddExpression(SeqTipoProcesso, x => x.SeqTipoProcesso == SeqTipoProcesso);
            AddExpression(SeqUnidadeResponsavel, x => x.SeqUnidadeResponsavel == SeqUnidadeResponsavel);
            AddExpression(SeqCliente, x => x.SeqCliente == SeqCliente);
            AddExpression(SeqTipoHierarquiaOferta, x => SeqTipoHierarquiaOferta == SeqTipoHierarquiaOferta);
            AddExpression(AnoReferencia, x => x.AnoReferencia == AnoReferencia);
            AddExpression(SemestreReferencia, x => x.SemestreReferencia == SemestreReferencia);
            AddExpression(TipoProcesso, x => x.SeqTipoProcesso == TipoProcesso);
            AddExpression(IntegraGPC, x => x.TipoProcesso.IntegraGPC == IntegraGPC);
            AddExpression(SeqsTiposHierarquiaOfertas, x => SeqsTiposHierarquiaOfertas.Contains(x.SeqTipoHierarquiaOferta));

            if (DataInicio.HasValue && DataFim.HasValue)
            {
                DateTime? datafim = DataFim.Value.Add(new TimeSpan(23, 59, 59));
                string tokensEtapa = TokenEtapa != null ? string.Join(" ", TokenEtapa) : null;

                AddExpression(x => x.EtapasProcesso.Any(f => f.DataInicioEtapa <= datafim.Value && f.DataFimEtapa >= DataInicio.Value &&
                                                                (tokensEtapa == null || tokensEtapa.Contains(f.Token))));
            }

            if (UidGedPreenchido.HasValue && UidGedPreenchido.Value)
            {
                AddExpression(x => x.Inscricoes.Where(w => w.UidProcessoGed != null).Any());
            }

            AddExpression(GestaoEventos, x => x.TipoProcesso.GestaoEventos);
            AddExpression(DataProcessoEventoCorrente, x => x.DataInicioEvento <= DataProcessoEventoCorrente && x.DataFimEvento >= DataProcessoEventoCorrente); 

            return GetExpression();
        }
    }
}