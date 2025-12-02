using SMC.Financeiro.ServiceContract.Areas.TXA.Data;
using SMC.Financeiro.ServiceContract.BLT;
using SMC.Financeiro.ServiceContract.TXA.Data;
using SMC.Framework.Domain;
using SMC.Framework.Specification;
using SMC.Framework.UnitOfWork;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using SMC.Inscricoes.Common.Areas.INS.Exceptions;
using SMC.Inscricoes.Common.Areas.INS.Exceptions.OfertaPeriodoTaxa;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using SMC.Inscricoes.Domain.Areas.INS.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SMC.Inscricoes.Domain.Areas.INS.DomainServices
{
    public class OfertaPeriodoTaxaDomainService : InscricaoContextDomain<OfertaPeriodoTaxa>
    {
        #region Services

        private IIntegracaoFinanceiroService FinanceiroService
        {
            get { return this.Create<IIntegracaoFinanceiroService>(); }
        }

        private InscricaoBoletoTaxaDomainService InscricaoBoletoTaxaDomainService
        {
            get { return this.Create<InscricaoBoletoTaxaDomainService>(); }
        }

        private OfertaDomainService OfertaDomainService
        {
            get { return this.Create<OfertaDomainService>(); }
        }

        private EtapaProcessoDomainService EtapaProcessoDomainService
        {
            get { return this.Create<EtapaProcessoDomainService>(); }
        }

        private InscricaoDomainService InscricaoDomainService
        {
            get { return this.Create<InscricaoDomainService>(); }
        }

        #endregion Services

        /// <summary>
        /// Busca taxas de inscrição para uma oferta
        /// estas taxas tem q estar vigente e poder ser informada na inscrição
        /// </summary>
        /// <param name="seqOferta">Sequencial da oferta</param>
        /// <param name="seqInscricaoa">Sequencial da inscricao</param>
        /// <returns>Lista de taxas vigentes para a oferta</returns>
        public IEnumerable<InscricaoTaxaOfertaVO> BuscarTaxaInscricaoOfertaVigente(long seqOferta, long seqInscricao)
        {
            var spec = new OfertaPeriodoTaxaFilterSpecification()
            {
                SeqOferta = seqOferta,
                SelecaoInscricao = true
            };

            spec.SetOrderBy(t => t.Taxa.TipoTaxa.Descricao);

            var taxas = this.SearchProjectionBySpecification(spec & new OfertaPeriodoTaxaVigenteSpecification(),
                    x => new InscricaoTaxaOfertaVO
                    {
                        Descricao = x.Taxa.TipoTaxa.Descricao,
                        DescricaoComplementar = x.Taxa.DescricaoComplementar,
                        NumeroMaximo = x.NumeroMaximo,
                        NumeroMinimo = x.NumeroMinimo,
                        NumeroItens = x.NumeroMinimo,
                        SeqTaxa = x.SeqTaxa,
                        SeqEventoTaxa = x.SeqEventoTaxa,
                        TipoCobranca = x.Taxa.TipoCobranca,
                        CobrarPorOferta = x.Taxa.CobrarPorOferta,
                        SeqOferta = x.SeqOferta,
                        PossuiGrupoTaxas = x.Oferta.Processo.GruposTaxa.Any(),
                        GrupoTaxa = x.Oferta.Processo.GruposTaxa.Select(g => new GrupoTaxaVO
                        {
                            Seq = g.Seq,
                            Descricao = g.Descricao,
                            Itens = g.Itens.Select(s => new GrupoTaxaItemVO
                            {
                                Seq = s.Seq,
                                SeqTaxa = s.SeqTaxa,
                                SeqGrupoTaxa = s.SeqGrupoTaxa,
                            }).ToList()
                        }).FirstOrDefault()

                    }).ToList();

            /*DateTime? dataPagamentoBoleto = InscricaoDomainService.SearchProjectionByKey(seqInscricao,
                i => i.Boletos.SelectMany(b => b.Titulos).FirstOrDefault(t => t.DataCancelamento == null && t.DataPagamento.HasValue).DataPagamento);*/

            var dadosBoletoPago = InscricaoDomainService.SearchProjectionByKey(seqInscricao,
                i => i.Boletos.SelectMany(b => b.Titulos).Select(b => new
                {
                    b.DataCancelamento,
                    b.DataPagamento,
                    DataGeracao = (DateTime?)b.DataGeracao,
                    Taxas = b.InscricaoBoleto.Taxas.Select(t => new
                    {
                        t.SeqTaxa,
                        t.NumeroItens
                    }).ToList()
                }).FirstOrDefault(t => t.DataCancelamento == null && t.DataPagamento.HasValue));

            // Chama o serviço do financeiro para buscar o valor da taxa
            foreach (var item in taxas)
            {
                if (dadosBoletoPago != null && dadosBoletoPago.DataGeracao.HasValue)
                {
                    // Recupera qual o evento taxa na data de pagamento do boleto
                    var seqEventoTaxa = SearchProjectionByKey(new OfertaPeriodoTaxaFilterSpecification
                    {
                        SeqOferta = seqOferta,
                        DataReferencia = dadosBoletoPago.DataGeracao.Value.Date,
                        SeqTaxa = item.SeqTaxa
                    }, x => x.SeqEventoTaxa);

                    // Define na taxa qual o seq evento referente na data de pagamento
                    item.SeqEventoTaxa = seqEventoTaxa;
                    item.NumeroItens = dadosBoletoPago.Taxas?.FirstOrDefault(t => t.SeqTaxa == item.SeqTaxa)?.NumeroItens ?? item.NumeroItens;
                }

                var filtro = new EventoTaxaFiltroData() { SeqEventoTaxa = item.SeqEventoTaxa };

                var eventoTaxa = FinanceiroService.BuscarEventosTaxa(filtro).FirstOrDefault();
                item.ValorEventoTaxa = eventoTaxa != null && eventoTaxa.Valor.HasValue ? eventoTaxa.Valor.Value : 0;
            }

            return taxas;
        }

        public void ValidarBoletoPagoAlteracaoValor(long seqInscricao, long seqOfertaPrincipal)
        {
            if (InscricaoDomainService.PossuiBoletoPago(seqInscricao))
            {
                // Recupera qual o seq da oferta que foi pago.
                var seqOfertaPaga = InscricaoDomainService.SearchProjectionByKey(seqInscricao, x => x.Ofertas.FirstOrDefault(o => o.NumeroOpcao == 1).SeqOferta);

                var taxasAtuais = BuscarTaxaInscricaoOfertaVigente(seqOfertaPrincipal, seqInscricao);
                var taxasPagas = BuscarTaxaInscricaoOfertaVigente(seqOfertaPaga, seqInscricao);

                // Valida se os valores são iguais para cada taxa
                foreach (var taxaAtual in taxasAtuais)
                {
                    var taxaPaga = taxasPagas.FirstOrDefault(t => t.SeqTaxa == taxaAtual.SeqTaxa);
                    if (taxaPaga.ValorEventoTaxa != taxaAtual.ValorEventoTaxa && (taxaPaga.NumeroItens > 0 || taxaAtual.NumeroItens > 0))
                        throw new AlteracaoOfertaTaxaNaoPermitidaBoletoPagoValoresDiferentesException(taxaPaga.Descricao, taxaPaga.ValorEventoTaxa, taxaAtual.ValorEventoTaxa);
                }
            }
        }

        public void ExcluirTaxaOfertaEmLote(long seqTipoTaxa, List<long> seqOfertas)
        {
            // Verifica se a situação da etapa de inscrição permite a alteração de taxas. 
            var seqProcesso = this.OfertaDomainService.SearchProjectionByKey(
                new SMCSeqSpecification<Oferta>(seqOfertas[0]),
                x => x.SeqProcesso);
            var specEtapa = new EtapaProcessoFilterSpecification(seqProcesso)
            {
                Token = TOKENS.ETAPA_INSCRICAO
            };
            var situacaoEtapaInscricao = this.EtapaProcessoDomainService.SearchProjectionBySpecification(specEtapa, x => x.SituacaoEtapa);
            if (situacaoEtapaInscricao != null && situacaoEtapaInscricao.Count() > 0
                && situacaoEtapaInscricao.FirstOrDefault() == SituacaoEtapa.Liberada)
            {
                throw new ExclusaoPeriodoTaxaEtapaLiberadaExcetpion();
            }

            // Verifica se já foi gerado algum boleto com a taxa
            var spec = new OfertaPeriodoTaxaFilterSpecification
            {
                SeqTaxa = seqTipoTaxa
            };
            var containsSpec = new SMCContainsSpecification<OfertaPeriodoTaxa, long>(x => x.SeqOferta, seqOfertas.ToArray());
            var periodosTaxa = this.SearchBySpecification(containsSpec & spec, x => x.Taxa, x => x.Taxa.TipoTaxa, x => x.Oferta);
            foreach (var periodo in periodosTaxa)
            {
                var boletoSpec = new InscricaoBoletoTaxaFilterSpecification
                {
                    SeqTaxa = periodo.SeqTaxa,
                    SeqOferta = periodo.SeqOferta
                };
                if (InscricaoBoletoTaxaDomainService.Count(boletoSpec) > 0)
                {
                    //Para cada tipo de taxa, verificar se já existe boleto com a taxa
                    throw new ExclusaoTaxaComBoletoException(periodo.Taxa.TipoTaxa.Descricao);
                }
            }

            using (var unitOfWork = SMCUnitOfWork.Begin())
            {
                try
                {
                    // Exclui os períodos de taxa em lote
                    var queryTemplate = @"  delete  oferta_periodo_taxa
                                        where   seq_taxa = {0}
                                        and     seq_hierarquia_oferta in ({1})";
                    var query = string.Format(queryTemplate, seqTipoTaxa, string.Join(",", seqOfertas.ToArray()));
                    this.OfertaDomainService.RawQuery<string>(query);

                    // Atualiza o flag de pagamento por taxa da oferta
                    var queryTemplateOferta = @"update	o
                                                set		ind_exige_pagamento_taxa = case
									                                                when exists (	select	1 
													                                                from	oferta_periodo_taxa 
													                                                where	seq_hierarquia_oferta = o.seq_hierarquia_oferta) then 1
									                                                else 0
									                                                end
                                                from	oferta o
                                                where	o.seq_hierarquia_oferta in ({0})";
                    var queryOferta = string.Format(queryTemplateOferta, string.Join(",", seqOfertas.ToArray()));
                    this.OfertaDomainService.RawQuery<string>(queryOferta);

                    unitOfWork.Commit();
                }
                catch (Exception)
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }

        public List<OfertaPeriodoTaxaVO> SelecionaOfertasPeriodosTaxaCoincidentes(List<OfertaPeriodoTaxaVO> periodos, List<OfertaPeriodoTaxaVO> periodosComparacao)
        {
            var retorno = new List<OfertaPeriodoTaxaVO>();

            if (periodos.Any() && periodosComparacao.Any())
            {
                for (int i = 0; i < periodos.Count; i++)
                {
                    for (int j = 0; j < periodosComparacao.Count; j++)
                    {
                        if (periodos[i].SeqTaxa == periodosComparacao[j].SeqTaxa &&
                               periodos[i].Seq != periodosComparacao[j].Seq &&
                               periodos[i].DataInicio < periodosComparacao[j].DataFim &&
                               periodosComparacao[j].DataInicio < periodos[i].DataFim)
                        {
                            if (!retorno.Contains(periodos[i]))                            
                                retorno.Add(periodos[i]);

                            if (!retorno.Contains(periodosComparacao[j]))                            
                                retorno.Add(periodosComparacao[j]);
                        }
                    }
                }

            }
            return retorno;
        }
    }
}