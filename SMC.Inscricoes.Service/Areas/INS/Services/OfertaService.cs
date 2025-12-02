using SMC.Framework.Extensions;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.Service;
using SMC.Framework.Specification;
using SMC.Framework.UnitOfWork;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Inscricoes.Domain.Areas.INS.DomainServices;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using SMC.Inscricoes.Domain.Areas.INS.ValueObjects;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace SMC.Inscricoes.Service.Areas.INS.Services
{
    public class OfertaService : SMCServiceBase, IOfertaService
    {
        #region DomainService

        private OfertaDomainService OfertaDomainService
        {
            get { return this.Create<OfertaDomainService>(); }
        }

        private OfertaPeriodoTaxaDomainService OfertaPeriodoTaxaDomainService
        {
            get { return this.Create<OfertaPeriodoTaxaDomainService>(); }
        }

        private HierarquiaOfertaDomainService HierarquiaOfertaDomainService
        {
            get { return this.Create<HierarquiaOfertaDomainService>(); }
        }

        private ProcessoDomainService ProcessoDomainService
        {
            get { return this.Create<ProcessoDomainService>(); }
        }

        private TaxaDomainService TaxaDomainService
        {
            get { return Create<TaxaDomainService>(); }
        }

        #endregion

        public List<HierarquiaOfertaData> BuscarArvoreOfertaCompleta(long seqProcesso, long? seqPai, long[] expandedNodes)
        {
            var retorno = HierarquiaOfertaDomainService.BuscarArvoreHierarquiaOfertaProcesso(seqProcesso, seqPai, expandedNodes)
                .TransformList<HierarquiaOfertaData>();

            return retorno;
        }

        public List<HierarquiaOfertaData> BuscarArvoreHierarquiaOfertaProcesso(long seqProcesso, long? seqGrupoOferta)
        {
            return HierarquiaOfertaDomainService.BuscarArvoreHierarquiaOfertaGrupoOferta(seqProcesso, seqGrupoOferta)
                .TransformList<HierarquiaOfertaData>();
        }

        public List<HierarquiaOfertaData> BuscarArvoreHierarquiaOfertaProcessoComGrupo(long seqProcesso, long? seqGrupoOferta)
        {
            return HierarquiaOfertaDomainService.BuscarArvoreHierarquiaOfertaGrupoOferta(seqProcesso, seqGrupoOferta)
                .Where(x => !x.EOferta || (x.PossuiGrupo && x.GrupoEmConfiguracao))
                .TransformList<HierarquiaOfertaData>();
        }

        public HierarquiaOfertaData[] BuscarArvoreOfertas(LookupOfertaFiltroData filtro)
        {
            return OfertaDomainService.BuscarHierarquiaOfertasArvore(filtro.SeqGrupoOferta, filtro.SeqItemHierarquiaOferta, filtro.SeqProcesso)
                                        .TransformListToArray<HierarquiaOfertaData>();
        }

        public HierarquiaOfertaData[] BuscarArvoreOfertasInscricao(long seqGrupoOferta)
        {
            return OfertaDomainService.BuscarHierarquiaOfertasArvoreInscricao(seqGrupoOferta).TransformListToArray<HierarquiaOfertaData>();
        }
        public SelecaoOfertaInscricaoData[] BuscarArvoreSelecaoOfertasInscricao(long seqGrupoOferta)
        {
            var retorno = OfertaDomainService.BuscarHierarquiaOfertasArvoreInscricao(seqGrupoOferta).TransformListToArray<SelecaoOfertaInscricaoData>();
            return retorno;
        }
        
        public SelecaoOfertaInscricaoData[] BuscarHierarquiaCompletaAngular(long seqOferta)
        {
            var retorno = OfertaDomainService.BuscarHierarquiaCompletaAngular(seqOferta).TransformListToArray<SelecaoOfertaInscricaoData>();
            return retorno;
        }

        public List<HierarquiaOfertaData> BuscarHierarquiaOfertaComGrupo(HierarquiaOfertaComGrupoFiltroData filtro)
        {
            return HierarquiaOfertaDomainService.BuscarArvoreHierarquiaOfertaGrupoOferta(filtro.SeqGrupoOferta, filtro.SeqOfertas)
                .TransformList<HierarquiaOfertaData>();
        }

        public SMCDatasourceItem[] BuscarOfertaKeyValue(long seqOferta)
        {
            OfertaFilterSpecification spec = new OfertaFilterSpecification
            {
                SeqOferta = seqOferta
            };
            spec.SetOrderBy(x => x.Nome);
            return OfertaDomainService.BuscarOfertasKeyValue(spec);
        }
        public SMCDatasourceItem[] BuscarSelecaoOfertasInscricaoKeyValue(long seqOferta)
        {
            OfertaFilterSpecification spec = new OfertaFilterSpecification
            {
                SeqOferta = seqOferta
            };
            spec.SetOrderBy(x => x.Nome);
            return OfertaDomainService.BuscarSelecaoOfertasInscricaoKeyValue(spec);
        }

        public SMCDatasourceItem[] BuscarDescricaoSelecaoOfertasInscricaoSeqsOfertas(long[] seqsOfertas)
        {
            OfertaFilterSpecification spec = new OfertaFilterSpecification
            {
                SeqsOfertas = seqsOfertas
            };
            spec.SetOrderBy(x => x.Nome);
            return OfertaDomainService.BuscarSelecaoOfertasInscricaoKeyValue(spec);
        }

        /// <summary>
        /// Retorna os sequenciais das ofertas vigentes e ativas para um grupo de oferta
        /// </summary>        
        public List<long> BuscarSeqOfertasVigentesAtivas(long SeqGrupoOferta)
        {
            return this.OfertaDomainService.BuscarSeqOfertasVigentesAtivas(SeqGrupoOferta);
        }

        /// <summary>
        /// Verifica se uma das raizes da hierarquia de ofertas do processo é oferta
        /// </summary>
        /// <param name="seqProcesso"></param>
        /// <returns></returns>
        public bool VerificarRaizHierarquiaOfertaPermiteOferta(long seqProcesso)
        {
            return ProcessoDomainService.VerificarRaizHierarquiaOfertaPermiteOferta(seqProcesso);
        }

        /// <summary>
        /// Verifica se uma das raizes da hierarquia de ofertas do processo é oferta
        /// </summary>
        /// <param name="seqProcesso"></param>
        /// <returns></returns>
        public bool VerificarRaizHierarquiaOfertaPermiteItem(long seqProcesso)
        {
            return ProcessoDomainService.VerificarRaizHierarquiaOfertaPermiteItem(seqProcesso);
        }

        /// <summary>
        /// Busca uma oferta com os dados completos
        /// </summary>        
        public OfertaData BuscarOferta(long seqOferta)
        {
            var oferta = OfertaDomainService.SearchByKey(new SMCSeqSpecification<Oferta>(seqOferta),
                                                IncludesOferta.CodigosAutorizacao |
                                                IncludesOferta.Telefones |
                                                IncludesOferta.EnderecosEletronicos |
                                                IncludesOferta.Taxas_Taxa |
                                                IncludesOferta.HierarquiaOfertaPai);
            return oferta.Transform<OfertaData>();
        }

        /// <summary>
        /// Busca uma oferta com os dados completos
        /// </summary>        
        public HierarquiaOfertaData BuscarHierarquiaOferta(long seqHierarquiaOferta)
        {
            return this.HierarquiaOfertaDomainService
                .SearchByKey<HierarquiaOferta, HierarquiaOfertaData>(seqHierarquiaOferta, IncludesHierarquiaOferta.HierarquiaOfertaPai);
        }

        /// <summary>
        /// Salva uma hierarquia de oferta( item da árvore de hierarquia de ofertas) de um processo
        /// </summary>        
        public long SalvarHierarquiaOferta(HierarquiaOfertaData hierarquiaOferta)
        {
            return this.HierarquiaOfertaDomainService.SalvarHierarquiaOferta(
                hierarquiaOferta.Transform<HierarquiaOferta>());
        }

        /// <summary>
        /// Salva uma oferta( item da árvore de hierarquia de ofertas) de um processo
        /// </summary>        
        public long SalvarOferta(OfertaData oferta)
        {
            return this.OfertaDomainService.SalvarOferta(
                oferta.Transform<Oferta>());
        }

        public void ExcluirHierarquiaOferta(long seqHierarquiaOferta)
        {
            this.HierarquiaOfertaDomainService.VerificaPermissaoExclusaoHierarquia(seqHierarquiaOferta);
            this.HierarquiaOfertaDomainService.DeleteEntity(seqHierarquiaOferta);
        }

        public void ExcluirOferta(long seqOferta)
        {
            this.HierarquiaOfertaDomainService.VerificaPermissaoExclusaoHierarquia(seqOferta);
            this.OfertaDomainService.DeleteEntity(seqOferta);
        }

        public SMCPagerData<OfertaTaxaData> BuscarOfertasTaxas(OfertaTaxaFiltroData filtro)
        {
            int total;
            var dados = this.OfertaDomainService.BuscarTaxasOferta(
                filtro.Transform<OfertaTaxaFilterSpecification>(), out total);
            return new SMCPagerData<OfertaTaxaData>(dados.TransformList<OfertaTaxaData>(), total);
        }

        public List<SMCDatasourceItem<string>> BuscarPeriodosOfertas(long seqProcesso)
        {
            return this.OfertaDomainService.BuscarPeriodosOfertas(seqProcesso);
        }

        public IEnumerable<SMCDatasourceItem> BuscarOfertasPeriodoTaxaKeyValue(OfertaPeriodoTaxaFiltroData filtro)
        {
            var dados = this.OfertaDomainService.SearchProjectionBySpecification(
                filtro.Transform<OfertaTaxaPeriodoFilterSpecification>(),
                x => new SMCDatasourceItem
                {
                    Seq = x.Seq,

                }).ToList();
            foreach (var oferta in dados)
            {
                oferta.Descricao = this.OfertaDomainService.BuscarHierarquiaOfertaCompleta(oferta.Seq, false).DescricaoCompleta;
            }
            return dados;
        }

        public void ExcluirTaxaOfertaEmLote(long seqTipoTaxa, List<long> seqOfertas)
        {
            this.OfertaPeriodoTaxaDomainService.ExcluirTaxaOfertaEmLote(seqTipoTaxa, seqOfertas);
        }

        public SMCDatasourceItem[] BuscarOfertasKeyValue(List<long> seqOfertas)
        {
            return OfertaDomainService.BuscarOfertasKeyValue(seqOfertas);
        }

        public void IncluirTaxasLote(IncluirTaxaEmLoteData modelo)
        {
            this.OfertaDomainService.IncluirTaxasLote(modelo.Transform<IncluirTaxaEmLoteVO>());
        }

        /// <summary>
        /// Busca o sdetalhes da oferta e a quantidade de inscrições confirmadas para a oferta
        /// Inicialmente usado na tela de prorrogação de etapa do processo
        /// </summary>
        public IEnumerable<OfertaPeriodoInscricaoData> BuscarOfertasInscricoes(long[] seqOfertas)
        {
            return this.OfertaDomainService.BuscarOfertasInscrioes(seqOfertas)
                .TransformList<OfertaPeriodoInscricaoData>();
        }

        public List<OfertaPeriodoInscricaoData> BuscarContagemInscrioesDasOfertasPorSituacao(long[] seqOfertas, string[] tokens)
        {
            return OfertaDomainService.BuscarContagemInscrioesDasOfertasPorSituacao(new InscricaoOfertaPorSituacaoVO() { SeqOfertas = seqOfertas, Tokens = tokens })
                                            .TransformList<OfertaPeriodoInscricaoData>();
        }

        /// <summary>
        /// Busca o sdetalhes da oferta e a quantidade de inscrições confirmadas para a oferta
        /// Inicialmente usado na tela de prorrogação de etapa do processo
        /// </summary>
        public OfertaPeriodoInscricaoData BuscarOfertaInscricoes(long seqOferta)
        {
            return this.OfertaDomainService.BuscarOfertaInscricoes(seqOferta)
                .Transform<OfertaPeriodoInscricaoData>();
        }

        /// <summary>
        /// Verifica se a inclusão de itens de hierarquia de oferta é permitida
        /// </summary>        
        public void VerificarPermissaoCadastrarHierarquia(long seqProcesso)
        {
            this.HierarquiaOfertaDomainService.VerificarPermissaoCadastrarHierarquia(seqProcesso);
        }

        public string BuscarDescricaoTaxa(long seqTaxa)
        {
            return TaxaDomainService.SearchProjectionByKey(new SMCSeqSpecification<Taxa>(seqTaxa),
                                                                x => x.TipoTaxa.Descricao);
        }

        public OfertaData BuscarUltimaOfertaCadastrada(long seqProcesso)
        {
            var oferta = OfertaDomainService.SearchBySpecification(new UltimaOfertaCadastradaSpecification(seqProcesso)).FirstOrDefault();
            if (oferta != null)
                return oferta.Transform<OfertaData>();
            return null;
        }

        /// <summary>
        /// Buscar a lista de ofertas a serem consolidadas no checkin
        /// </summary>
        /// <param name="filtro">Parametros dos filtros</param>
        /// <returns>List de ofertas</returns>
        public SMCPagerData<AcompanhamentoInscritoCheckinListaData> BuscarPosicaoConsolidadaCheckin(AcompanhamentoCheckinFiltroData filtro)
        {
            var retorno = OfertaDomainService.BuscarPosicaoConsolidadaCheckin(filtro.Transform<PosicaoConsolidadaCheckinFilterSpecification>(), out int total);

            return new SMCPagerData<AcompanhamentoInscritoCheckinListaData>(retorno.TransformList<AcompanhamentoInscritoCheckinListaData>(), total);

        }
    }
}
