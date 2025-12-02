using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Financeiro.Common.HUB;
using SMC.Framework.Mapper;
using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Rest.Models;
using SMC.PDFCore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.ConstrainedExecution;
using System.Web.UI.WebControls.WebParts;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    /// <summary>
    /// Specification para buscar inscricoes de um processo
    /// </summary>
    public class SituacaoInscricaoProcessoFilterSpecification : SMCSpecification<Inscricao>
    {
        public long SeqProcesso { get; set; }

        public long? SeqSituacao { get; set; }

        public long? SeqGrupoOferta { get; set; }

        public long? SeqItemHierarquiaOferta { get; set; }

        public long? SeqOferta { get; set; }

        public string TokenEtapaSituacao { get; set; }

        public long? SeqInscricao { get; set; }

        public long? SeqMotivo { get; set; }

        public string NomeInscrito { get; set; }

        public long? SeqTipoProcessoSituacao { get; set; }

        [SMCMapProperty("FiltroSGF.Dados")]
        public List<KeyValuePair<long, string>> Dados { get; set; }

        public SituacaoDocumentacao? SituacaoDocumentacao { get; set; }

        public bool? RecebeuBolsa { get; set; }

        public long? SeqTipoTaxa { get; set; }

        public bool? CheckinRealizado { get; set; }


        public override Expression<Func<Inscricao, bool>> SatisfiedBy()
        {
            if (Dados != null && Dados.Any())
            {
                // tive que fazer essa solução aqui pois não da pra usar o próprio dic na specification
                var elementosFiltro = Dados?.Select(x => $"{x.Key}.{x.Value}");
                AddExpression(elementosFiltro, x => x.Formularios.GroupBy(f => f.SeqVisao).OrderByDescending(f => f.Key).FirstOrDefault().Any(f => elementosFiltro.All(e => f.DadosCampos.Any(d => e == (d.SeqElemento + "." + d.Valor)))));
            }

            AddExpression(x => x.SeqProcesso == SeqProcesso);
            AddExpression(TokenEtapaSituacao, i => i.HistoricosSituacao.Any(x => x.EtapaProcesso.Token.ToLower().StartsWith(TokenEtapaSituacao.ToLower())));
            AddExpression(SeqSituacao, i => i.HistoricosSituacao.Any(x => x.Atual && x.TipoProcessoSituacao.SeqSituacao == SeqSituacao.Value));
            AddExpression(SeqMotivo, i => i.HistoricosSituacao.Any(x => x.Atual && x.SeqMotivoSituacaoSGF == SeqMotivo.Value));
            AddExpression(SeqInscricao, i => i.Seq == SeqInscricao.Value);
            AddExpression(NomeInscrito, i => i.Inscrito.Nome.ToLower().Contains(NomeInscrito.ToLower()) || i.Inscrito.NomeSocial.ToLower().Contains(NomeInscrito.ToLower()));
            AddExpression(SeqGrupoOferta, i => i.SeqGrupoOferta == SeqGrupoOferta.Value);
            AddExpression(SeqOferta, i => i.Ofertas.Any(x => x.SeqOferta == SeqOferta.Value));
            // Filtra pela hierarquia. Contains irá trazer falsos resultados que deverão ser ajustados posteriormente em memória.
            AddExpression(SeqItemHierarquiaOferta, i => i.Ofertas.Any(x => x.Oferta.HierarquiaCompleta.Contains(SeqItemHierarquiaOferta.Value.ToString())));
            AddExpression(SeqTipoProcessoSituacao, i => i.HistoricosSituacao.Any(x => x.Atual && x.SeqTipoProcessoSituacao == SeqTipoProcessoSituacao));
            AddExpression(SituacaoDocumentacao, x => x.SituacaoDocumentacao == SituacaoDocumentacao);
            AddExpression(RecebeuBolsa, a => a.RecebeuBolsa == RecebeuBolsa);

            //UC_INS_003_01_03 - Consulta de Inscrições do Processo - NV 14 - Filtro:  "Tipo de taxa" 
            //Quando houver um valor selecionado para este filtro, devem ser retornados no result set as Inscrições que possuem o Tipo de taxa selecionado no Boleto de inscrição.
            AddExpression(SeqTipoTaxa, insc => insc.Boletos.Any(inscBoleto => inscBoleto.Taxas.Any(inscBoletoTaxa => inscBoletoTaxa.Taxa.Seq == SeqTipoTaxa)));

            //UC_INS_003_01_03 - Consulta de Inscrições do Processo - NV 15 - Filtro: "Check-in realizado?"
            //Quando o valor "Sim" estiver selecionado para este filtro, deverão ser retornados no result set as Inscrições que possuem pelo menos uma Oferta com Data de check-in preenchida.
            //Quando o valor "Não" estiver selecionado, deverão ser retornadas somente as Inscrições que não possuem nenhuma Oferta com Data de check-in preenchida. 
            //Quando o valor("Check-in realizado?") não estiver selecionado, os filtros relativos a este campo não deverão ser realizados.
            if (CheckinRealizado.HasValue)
            {
                if (CheckinRealizado.Value)
                    AddExpression(CheckinRealizado, x => x.Ofertas.Any(oferta => oferta.DataCheckin.HasValue));
                else
                    AddExpression(CheckinRealizado, x => x.Ofertas.All(oferta => !oferta.DataCheckin.HasValue));
            }

            return GetExpression();
        }
    }
}