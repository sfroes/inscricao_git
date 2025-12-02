using SMC.Framework.Specification;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Inscricoes.Common.Areas.INS.Enums;
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
    /// Specification para filtrar ConfiguracaoEtapa
    /// </summary>
    public class ConfiguracaoEtapaInscricaoEmAbertoSpecification : SMCSpecification<ConfiguracaoEtapa>
    {
        private DateTime DataAtual;

        public long? SeqProcesso { get; set; }

        public long SeqInscrito { get; set; }

        public string DescricaoProcesso { get; set; }

        public ConfiguracaoEtapaInscricaoEmAbertoSpecification()
        {
            DataAtual = DateTime.Now;
        }

        /// <summary>
        /// Realiza a pesquisa
        /// </summary>
        public override System.Linq.Expressions.Expression<Func<ConfiguracaoEtapa, bool>> SatisfiedBy()
        {
            return c =>
                // Exibe na relação Geral ou é um processo especifico
                (c.EtapaProcesso.Processo.ExibeRelacaoGeral == true || this.SeqProcesso.HasValue)
                // Não estar cancelado
                && (!c.EtapaProcesso.Processo.DataCancelamento.HasValue || c.EtapaProcesso.Processo.DataCancelamento > this.DataAtual)
                // Não estar encerrado
                && (!c.EtapaProcesso.Processo.DataEncerramento.HasValue || c.EtapaProcesso.Processo.DataEncerramento > this.DataAtual)
                // A situação da etapa de inscrição está liberada
                && c.EtapaProcesso.Token.Equals(TOKENS.ETAPA_INSCRICAO)
                && c.EtapaProcesso.SituacaoEtapa == SituacaoEtapa.Liberada
                // A etapa de inscrição está vigente
                //&& c.EtapaProcesso.DataInicioEtapa < this.DataAtual
                //&& c.EtapaProcesso.DataFimEtapa > this.DataAtual
                // A configuração da inscrição está vigente ou possui inscrição fora do prazo
                && ((c.DataInicio < this.DataAtual
                    && c.DataFim > this.DataAtual)
                    || c.EtapaProcesso.Processo.PermissoesInscricaoForaPrazo.Any(f => f.Inscritos.Any(o => o.SeqInscrito == SeqInscrito) &&
                                                                                    f.DataFim >= DateTime.Now && f.DataInicio <= DateTime.Now))
                // Possui um grupo de oferta com oferta ativa, vigente e não cancelada
                && c.GruposOferta.Any(g => g.GrupoOferta.Ofertas.Any(o => o.Ativo 
                                                                 && (!o.DataCancelamento.HasValue || o.DataCancelamento > this.DataAtual)
                                                                 && (o.DataInicio < this.DataAtual && o.DataFim > this.DataAtual)
                                                                       || c.EtapaProcesso.Processo.PermissoesInscricaoForaPrazo.Any(f => f.Inscritos.Any(y => y.SeqInscrito == SeqInscrito) &&
                                                                                    f.DataFim >= DateTime.Now && f.DataInicio <= DateTime.Now) ))
                // Filtro pelo nome do processo (informado pelo usuario)
                && (string.IsNullOrEmpty(this.DescricaoProcesso) || c.EtapaProcesso.Processo.Descricao.ToLower().Contains(this.DescricaoProcesso.ToLower()))
                && (!this.SeqProcesso.HasValue || c.EtapaProcesso.SeqProcesso == this.SeqProcesso.Value); 
        }
    }
}
