using SMC.Framework.Specification;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Inscricoes.Domain.Areas.INS;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    /// <summary>
    /// Specification para filtrar a posiçao consolidada checkin
    /// Sempre filtra pelo Seq do Processo
    /// </summary>
    public class PosicaoConsolidadaCheckinFilterSpecification : SMCSpecification<Oferta>
    {
        public long? SeqProcesso { get; set; }
        public long? SeqUnidadeResponsavel { get; set; }
        public long? SeqTipoProcesso { get; set; }
        public long? SemestreReferencia { get; set; }
        public int? AnoReferencia { get; set; }
        public long? SeqItemHierarquiaOferta { get; set; }
        public long? SeqOferta { get; set; }
        public DateTime? Data { get; set; }
        public string Token { get; set; }

        /// <summary>
        /// Realiza a pesquisa
        /// Sempre filtra pelo Seq do Processo
        /// Se o filtro de SeqGrupoOferta for setado é utilizado
        /// Se o filtro de SeqOferta for setado é utilizado 
        /// </summary>
        public override System.Linq.Expressions.Expression<Func<Oferta, bool>> SatisfiedBy()
        {
            AddExpression(SeqProcesso, x => x.SeqProcesso == SeqProcesso);
            AddExpression(SeqUnidadeResponsavel, x => x.InscricoesOferta.Where(w => w.Inscricao.Processo.SeqUnidadeResponsavel == SeqUnidadeResponsavel).Any());
            AddExpression(SeqTipoProcesso, x => x.Processo.SeqTipoProcesso == SeqTipoProcesso);
            AddExpression(SemestreReferencia, x => x.Processo.SemestreReferencia == SemestreReferencia);
            AddExpression(AnoReferencia, x => x.Processo.AnoReferencia == AnoReferencia);
            AddExpression(SeqOferta, a => a.Seq == SeqOferta);
            //DateTime.Date não pode ser convertido para SQL, por isso a comparação é feita separadamente
            //r => r.DataInicioAtividade.Value.Date <= filtro.Data.Value.Date && r.DataFimAtividade.Value.Date >= filtro.Data.Value.Date
            AddExpression(Data, a => a.DataInicioAtividade.Value.Day <= Data.Value.Day && a.DataInicioAtividade.Value.Month == Data.Value.Month && a.DataInicioAtividade.Value.Year == Data.Value.Year &&
                                                    a.DataFimAtividade.Value.Day >= Data.Value.Day && a.DataFimAtividade.Value.Month == Data.Value.Month && a.DataFimAtividade.Value.Year == Data.Value.Year);  

            AddExpression(SeqItemHierarquiaOferta, x => x.SeqItemHierarquiaOferta == SeqItemHierarquiaOferta);
            AddExpression(Token, x => x.InscricoesOferta.Where(w => w.Inscricao.HistoricosSituacao.Any(h => h.Atual == true && h.TipoProcessoSituacao.Token == Token)).Any());

            return GetExpression();
        }
    }
}
