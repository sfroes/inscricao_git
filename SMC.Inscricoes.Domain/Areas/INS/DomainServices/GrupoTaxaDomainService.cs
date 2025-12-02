using SMC.Framework.Extensions;
using SMC.Framework.Model;
using SMC.Framework.Specification;
using SMC.Framework.UnitOfWork;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Inscricoes.Common.Areas.INS.Exceptions;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using SMC.Inscricoes.Domain.Areas.INS.ValueObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SMC.Inscricoes.Domain.Areas.INS.DomainServices
{
    public class GrupoTaxaDomainService : InscricaoContextDomain<GrupoTaxa>
    {

        #region DomainServices

        private ProcessoDomainService ProcessoDomainService
        {
            get { return this.Create<ProcessoDomainService>(); }
        }

        private OfertaPeriodoTaxaDomainService OfertaPeriodoTaxaDomainService
        {
            get { return this.Create<OfertaPeriodoTaxaDomainService>(); }
        }

        private HierarquiaOfertaDomainService HierarquiaOfertaDomainService
        {
            get { return this.Create<HierarquiaOfertaDomainService>(); }
        }

        #endregion DomainServices

        #region [Métodos Auxiliares]

        private void ValidarRestricoesDeQuantidadeEntreTaxasDoGrupoETaxasDoProcesso(GrupoTaxa grupoTaxa, long seqProcesso)
        {
            //4.1.Se existir Período de taxa da oferta {obs: NO PROCESSO} para alguma das Taxas do grupo, cujo Número máximo ou
            //o Número mínimo é maior que o Número máximo de itens do grupo,
            //abortar a operação e emitir a mensagem de erro:

            var seqsTaxas = grupoTaxa.Itens.Select(gti => gti.SeqTaxa).ToList();

            var taxasDoGrupoNoProcesso = OfertaPeriodoTaxaDomainService.SearchProjectionBySpecification(
                new OfertaPeriodoTaxaFilterSpecification
                {
                    SeqProcesso = seqProcesso,
                    SeqsTaxa = seqsTaxas
                }, x => new
                {
                    x.NumeroMinimo,
                    x.NumeroMaximo,
                    DescricaoTipoTaxa = x.Taxa.TipoTaxa.Descricao,
                    NomHierarquiaComplOferta = x.Oferta.DescricaoCompleta

                }).Where(opt => opt.NumeroMinimo > grupoTaxa.NumeroMaximoItens || opt.NumeroMaximo > grupoTaxa.NumeroMaximoItens)
                  .Select(opt => opt).FirstOrDefault();

            if (taxasDoGrupoNoProcesso != null)
                throw new MaiorQueNumeroMaximoItensGrupoException(taxasDoGrupoNoProcesso.DescricaoTipoTaxa, taxasDoGrupoNoProcesso.NomHierarquiaComplOferta);

        }

        /// <summary>        
        /// Verifica se o somatório das quantidades mínimas configuradas nos períodos coincidentes das taxas da oferta 
        /// ultrapassam o número máximo de itens permitidos no Grupo de Taxas
        /// </summary>
        /// <param name="grupoTaxa">O grupo de taxa com a restrição do número máximo de itens</param>
        /// <param name="seqProcesso">Sequêncial do processo</param>
        /// <returns>True se somatório excede quantidade Maxíma itens grupo, False caso contrário</returns>
        private bool QuantidadeTaxaExcedidaNasOfertasDoProcesso(GrupoTaxa grupoTaxa, long seqProcesso)
        {
            //ValidarRestricoesDeSomatorioQtdEntreTaxasDoGrupoETaxasDoProcesso
            //Last Rule
            //4.2.Para cada uma das Ofertas do Processo, verificar se existem Períodos de taxa da oferta para as Taxas do Grupo de taxas,
            //com períodos coincidentes, cujo somatório da Qtd. mínima é mai​or que o Número máximo de itens do Grupo de taxas.
            //Se existir, abortar a operação e emitir a mensagem de erro:

            //"O somatório da quantidade mínima configurada nos períodos de taxa das ofertas não pode ultrapassar
            //o número máximo de itens do grupo".

            //Observação: ao verificar os Períodos de taxa coincidentes, caso exista mais de um período para a mesma Taxa,
            //considerar somente o que tiver a maior Qtd.mínima.


            // Verificar se o grupo possui número máximo definido
            if (!grupoTaxa.NumeroMaximoItens.HasValue)
                return false;

            // Obter todas as sequências de Taxas que pertencem ao grupo
            var seqTaxasDoGrupo = grupoTaxa.Itens.Select(gti => gti.SeqTaxa).ToList();

            var taxasDoGrupoNoProcesso = OfertaPeriodoTaxaDomainService
                                        .SearchBySpecification(new OfertaPeriodoTaxaFilterSpecification
                                        {
                                            SeqProcesso = seqProcesso,
                                            SeqsTaxa = seqTaxasDoGrupo
                                        })
                                        .Select(ofertaPeriodoTaxa => new OfertaPeriodoTaxaVO
                                        {
                                            Seq = ofertaPeriodoTaxa.Seq,
                                            SeqTaxa = ofertaPeriodoTaxa.SeqTaxa,
                                            SeqOferta = ofertaPeriodoTaxa.SeqOferta,
                                            DataInicio = ofertaPeriodoTaxa.DataInicio,
                                            DataFim = ofertaPeriodoTaxa.DataFim,
                                            NumeroMinimo = ofertaPeriodoTaxa.NumeroMinimo,
                                            NumeroMaximo = ofertaPeriodoTaxa.NumeroMaximo
                                        })
                                        .ToList();


            if (taxasDoGrupoNoProcesso.Any())
            {
                var taxasAgrupadasPorOferta = taxasDoGrupoNoProcesso
                    .GroupBy(t => t.SeqOferta)
                    .Select(grupo => new
                    {
                        SeqOferta = grupo.Key,
                        Taxas = grupo.ToList()
                    })
                    .ToList();

                foreach (var taxasDaOferta in taxasAgrupadasPorOferta)
                {
                    if (taxasDaOferta.Taxas.Any() && SomatorioQtdMinEmPeriodosCoincidentesExcedeQtdMaxItensGrupoTaxa(grupoTaxa, taxasDaOferta.Taxas) > 0)
                        return true;
                }
            }
            return false;
        }

        /// <summary>        
        /// Verifica se o somatório das quantidades mínimas configuradas nos períodos coincidentes das taxas da oferta
        /// ultrapassam o número máximo de itens permitidos no Grupo de Taxas
        /// </summary>
        /// <param name="grupoTaxa">O grupo de taxa com a restrição do número máximo de itens</param>
        /// <param name="taxasDaOferta">Lista de períodos de taxas da oferta a serem validados</param>
        /// <returns>SeqOferta(long) do periodo taxa que exceder o limite, 0 caso contrário</returns>
        public long SomatorioQtdMinEmPeriodosCoincidentesExcedeQtdMaxItensGrupoTaxa(GrupoTaxa grupoTaxa, List<OfertaPeriodoTaxaVO> taxasDaOferta)
        {
            // Verificação de parâmetros
            if (grupoTaxa == null || !grupoTaxa.NumeroMaximoItens.HasValue)
                return 0;

            if (taxasDaOferta == null || !taxasDaOferta.Any())
                return 0;

            // Otimização: Se não existirem períodos sobrepostos, é mais eficiente verificar apenas os dias de início
            // e mudanças, em vez de cada dia do intervalo completo
            var datasImportantes = new HashSet<DateTime>();

            foreach (var taxa in taxasDaOferta)
            {
                datasImportantes.Add(taxa.DataInicio);
                datasImportantes.Add(taxa.DataFim.AddDays(1)); // O dia após o término também é importante
            }

            var datasOrdenadas = datasImportantes.Where(d => d >= taxasDaOferta.Min(p => p.DataInicio) &&
                                                            d <= taxasDaOferta.Max(p => p.DataFim).AddDays(1))
                                                 .OrderBy(d => d)
                                                 .ToList();

            // Para cada ponto de mudança (início ou fim de um período)
            for (int i = 0; i < datasOrdenadas.Count - 1; i++)
            {
                var dataAtual = datasOrdenadas[i];

                // Não precisamos verificar o dia após o término, apenas o período
                if (dataAtual > taxasDaOferta.Max(p => p.DataFim))
                    break;

                // Encontra todos os períodos ativos nesta data e
                // calcula a soma considerando apenas o período com maior NumeroMinimo para cada taxa
                var periodosAtivos = taxasDaOferta
                    .Where(p => p.DataInicio <= dataAtual && p.DataFim >= dataAtual)
                    .GroupBy(p => p.SeqTaxa)
                    .Select(g => g.OrderByDescending(p => p.NumeroMinimo ?? 0).First())
                    .ToList();

                var somaNumeroMinimo = periodosAtivos.Sum(p => p.NumeroMinimo ?? 0);


                if (somaNumeroMinimo > (int)grupoTaxa.NumeroMaximoItens)
                    return periodosAtivos.FirstOrDefault(x => x.SeqOferta != 0)?.SeqOferta ?? 0;
            }

            return 0;
        }

        private List<Taxa> BuscarTaxasGrupoTaxas(GrupoTaxa grupoTaxa, Processo processo)
        {
            var taxas = processo.Taxas
            .Where(taxa => grupoTaxa.Itens.Select(i => i.SeqTaxa).Contains(taxa.Seq))
            .ToList();

            return taxas;
        }

        #endregion

        public IEnumerable<SMCDatasourceItem> BuscarGrupoTaxa(GrupoTaxaFilterSpecification filtro)
        {
            return this.SearchProjectionBySpecification(filtro, x => new SMCDatasourceItem { Seq = x.Seq, Descricao = x.Descricao });
        }

        public long SalvarGrupoTaxa(GrupoTaxa grupoTaxa)
        {
            var specProcesso = new ProcessoFilterSpecification() { SeqProcesso = grupoTaxa.SeqProcesso };
            var processo = ProcessoDomainService.SearchByKey(specProcesso, IncludesProcesso.Taxas | IncludesProcesso.Taxas_TipoTaxa | IncludesProcesso.GruposOferta_Ofertas | IncludesProcesso.GruposOferta_Ofertas_Taxas);
            var gruposTaxaProcesso = this.SearchBySpecification(new GrupoTaxaFilterSpecification() { SeqProcesso = grupoTaxa.SeqProcesso }, IncludesGrupoTaxa.Itens | IncludesGrupoTaxa.Itens_Taxa | IncludesGrupoTaxa.Itens_Taxa_TipoTaxa).ToList();

            #region Validações

            //RN_INS_221 - Consistência do grupo de taxas
            if (grupoTaxa.Itens.Count() < 2)
            {
                throw new MinimoItensTaxaException();
            }

            ////2.Se alguma das Taxas informadas já estiver associada a outro Grupo de taxas, abortar a operação e emitir a mensagem de erro:
            ////“A taxa<Descrição do Tipo de taxa da Taxa que já foi associada a outro Grupo de taxa > está associada ao grupo
            ////< Descrição do Grupo de taxa onde a Taxa em questão foi encontrada >.Não é permitido associar uma mesma taxa a mais de um grupo de taxas.”

            if (gruposTaxaProcesso.Any())
            {
                var tiposTaxaInformados = grupoTaxa.Itens.Select(i => i.SeqTaxa).ToList();

                var taxaCadastrada = gruposTaxaProcesso
                    .SelectMany(gt => gt.Itens, (gt, item) => new { GrupoTaxa = gt, Item = item })
                    .FirstOrDefault(x => tiposTaxaInformados.Contains(x.Item.SeqTaxa) && x.GrupoTaxa.Seq != grupoTaxa.Seq);

                if (taxaCadastrada != null)
                {
                    throw new AssociacaoDuplicadaTaxaException(taxaCadastrada.Item.Taxa.TipoTaxa.Descricao, taxaCadastrada.GrupoTaxa.Descricao);
                }
            }

            //3.Se alguma das Taxas associadas ao Grupo de taxas tiver um Tipo de cobrança distinto das demais, abortar a operação e emitir a mensagem de erro:

            if (grupoTaxa.Itens.Count > 1)
            {
                var taxasGrupoTaxa = BuscarTaxasGrupoTaxas(grupoTaxa, processo);


                if (taxasGrupoTaxa.Select(t => t.TipoCobranca).Distinct().Count() > 1)
                {
                    throw new TipoCobrancaTaxaDiferenteException();
                }
            }
            // 4.Se o Número máximo de itens do Grupo de taxas estiver preenchido:

            if (grupoTaxa.NumeroMaximoItens.HasValue)
            {
                ValidarRestricoesDeQuantidadeEntreTaxasDoGrupoETaxasDoProcesso(grupoTaxa, processo.Seq);

                if (QuantidadeTaxaExcedidaNasOfertasDoProcesso(grupoTaxa, processo.Seq))
                    throw new QuantidadeTaxaExcedidaNasOfertasDoProcessoException();
            }

            //5.Se a mesma Taxa tiver sido selecionada mais de uma vez, abortar a operação e emitir a mensagem de erro:
            //"Operação não permitida. Uma taxa não pode ser selecionada mais de uma vez em um mesmo grupo de taxas.

            if (grupoTaxa.Itens.GroupBy(t => t.SeqTaxa).Any(g => g.Count() > 1))
                throw new TaxaDuplicadaException();

            #endregion

            using (var unitOfWork = SMCUnitOfWork.Begin())
            {
                try
                {
                    this.SaveEntity(grupoTaxa);
                    unitOfWork.Commit();
                }
                catch (Exception)
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }

            return grupoTaxa.Seq;
        }

        public void ExcluirGrupoTaxa(long seqGrupoTaxa)
        {
            using (var unitOfWork = SMCUnitOfWork.Begin())
            {
                try
                {
                    var grupoTaxa = this.SearchByKey(new SMCSeqSpecification<GrupoTaxa>(seqGrupoTaxa));

                    this.DeleteEntity(grupoTaxa);
                    unitOfWork.Commit();
                }
                catch (Exception)
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }

        public List<GrupoTaxa> BuscarGruposTaxaPorSeqProcesso(long seqProcesso)
        {
            var spec = new GrupoTaxaFilterSpecification()
            {
                SeqProcesso = seqProcesso
            };

            return this.SearchBySpecification(spec, IncludesGrupoTaxa.Itens | IncludesGrupoTaxa.Itens_Taxa | IncludesGrupoTaxa.Itens_Taxa_Ofertas).ToList();
        }

        /// <summary>
        /// Faz a cópia dos grupos de taxa do processo de origem para o processo de cópia.
        /// </summary>
        /// <param name="seqProcessoOrigem"></param>
        /// <param name="seqProcessoCopia"></param>
        public void CopiarGruposTaxa(CopiaGrupoTaxaVO copiaGrupoTaxaVO)
        {
            var gruposTaxaProcessoOrigem = this.BuscarGruposTaxaPorSeqProcesso(copiaGrupoTaxaVO.SeqProcessoOrigem);

            if (gruposTaxaProcessoOrigem.Any())
            {
                foreach (var grupoTaxaOrigem in gruposTaxaProcessoOrigem)
                {
                    var copiaGrupoTaxa = new GrupoTaxa();
                    copiaGrupoTaxa.Seq = 0; 
                    
                    if (copiaGrupoTaxa.Itens == null)
                        copiaGrupoTaxa.Itens = new List<GrupoTaxaItem>();

                    foreach (var itemOrigem in grupoTaxaOrigem.Itens)
                    {
                        var taxaCopia = copiaGrupoTaxaVO.TaxasProcessoCopia
                            .FirstOrDefault(t => t.SeqTipoTaxa == itemOrigem.Taxa.SeqTipoTaxa);
                        
                        if (taxaCopia.Seq < 1)
                            continue;

                        copiaGrupoTaxa.Itens.Add(new GrupoTaxaItem()
                        {
                            Seq = 0, 
                            SeqGrupoTaxa = 0, 
                            SeqTaxa = taxaCopia.Seq,
                            DataInclusao = DateTime.MinValue,
                            Taxa = taxaCopia.Transform<Taxa>()
                        });

                    }
                    copiaGrupoTaxa.Descricao = grupoTaxaOrigem.Descricao;
                    copiaGrupoTaxa.SeqProcesso = copiaGrupoTaxaVO.SeqProcessoCopia;
                    copiaGrupoTaxa.DataInclusao = DateTime.MinValue;
                    copiaGrupoTaxa.NumeroMinimoItens = grupoTaxaOrigem.NumeroMinimoItens;
                    copiaGrupoTaxa.NumeroMaximoItens = grupoTaxaOrigem.NumeroMaximoItens;

                    this.SalvarGrupoTaxa(copiaGrupoTaxa);
                }
            }
        }
    }
}