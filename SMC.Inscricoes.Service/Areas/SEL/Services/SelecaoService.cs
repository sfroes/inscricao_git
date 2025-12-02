using SMC.Framework.Extensions;
using SMC.Framework.Model;
using SMC.Framework.Service;
using SMC.Framework.Specification;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Domain.Areas.INS.DomainServices;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using SMC.Inscricoes.Domain.Areas.INS.ValueObjects;
using SMC.Inscricoes.Domain.Areas.SEL.DomainServices;
using SMC.Inscricoes.Domain.Areas.SEL.ValueObjects;
using SMC.Inscricoes.ServiceContract.Areas.SEL.Data;
using SMC.Inscricoes.ServiceContract.Areas.SEL.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace SMC.Inscricoes.Service.Areas.SEL.Services
{
    public class SelecaoService : SMCServiceBase, ISelecaoService
    {
        #region Domain Services

        private ProcessoDomainService ProcessoDomainService
        {
            get { return Create<ProcessoDomainService>(); }
        }

        private OfertaDomainService OfertaDomainService
        {
            get { return Create<OfertaDomainService>(); }
        }

        private InscricaoOfertaDomainService InscricaoOfertaDomainService
        {
            get { return Create<InscricaoOfertaDomainService>(); }
        }

        private InscricaoOfertaHistoricoSituacaoDomainService InscricaoOfertaHistoricoSituacaoDomainService
        {
            get { return Create<InscricaoOfertaHistoricoSituacaoDomainService>(); }
        }

        #endregion Domain Services

        #region Acompanhamento Selecao

        public SMCPagerData<AcompanhamentoSelecaoData> ConsultaPosicaoConsolidada(AcompanhamentoSelecaoFiltroData filtro)
        {
            var spec = filtro.Transform<ProcessoFilterSpecification>();
            spec.TokenEtapa = new string[] { TOKENS.ETAPA_SELECAO, TOKENS.ETAPA_CONVOCACAO };
            var lista = ProcessoDomainService.ConsultaPosicaoConsolidadaSelecao(spec, out int total).TransformList<AcompanhamentoSelecaoData>();
            return new SMCPagerData<AcompanhamentoSelecaoData>(lista, total);
        }

        public SMCPagerData<PosicaoConsolidadaPorGrupoOfertaListaData> ConsultaPosicaoConsolidadaGrupoOferta(PosicaoConsolidadaPorGrupoOfertaFiltroData filtro)
        {
            var lista = OfertaDomainService.ConsultaPosicaoConsolidadaSelecaoGrupoOferta(
                                filtro.Transform<PosicaoConsolidadaGrupoOfertaFilterSpecification>(), out int total).TransformList<PosicaoConsolidadaPorGrupoOfertaListaData>();
            return new SMCPagerData<PosicaoConsolidadaPorGrupoOfertaListaData>(lista, total);
        }

        public SMCPagerData<ConsultaCandidatosProcessoListaData> BuscarCandidatosProcesso(ConsultaCandidatosProcessoFiltroData filtro)
        {
            var lista = InscricaoOfertaDomainService.BuscarCandidatosSelecaoProcesso(filtro.Transform<CandidatosSelecaoProcessoSpecification>(), out int total)
                                                .TransformList<ConsultaCandidatosProcessoListaData>();
            return new SMCPagerData<ConsultaCandidatosProcessoListaData>(lista, total);
        }

        public CabecalhoSelecaoData BuscarCabecalhoSelecaoProcesso(long seqProcesso)
        {
            return ProcessoDomainService.SearchProjectionByKey(new SMCSeqSpecification<Processo>(seqProcesso),
                                                x => new CabecalhoSelecaoData
                                                {
                                                    TipoProcesso = x.TipoProcesso.Descricao,
                                                    Descricao = x.Descricao
                                                });
        }

        public CabecalhoSelecaoData BuscarCabecalhoSelecaoOferta(long seqOferta)
        {
            var spec = new OfertaFilterSpecification() { SeqOferta = seqOferta };

            spec.SetOrderBy(x => x.Nome);
            var cabecalhoSelecao = OfertaDomainService.SearchProjectionByKey(spec,
                                            x => new CabecalhoSelecaoData
                                            {
                                                TipoProcesso = x.Processo.TipoProcesso.Descricao,
                                                Descricao = x.Processo.Descricao,
                                                GrupoOferta = x.GrupoOferta.Nome,
                                                Oferta = x.Nome
                                            });

            cabecalhoSelecao.Oferta = OfertaDomainService.BuscarHierarquiaOfertaCompleta(seqOferta, false).DescricaoCompleta;

            return cabecalhoSelecao;
        }

        #endregion Acompanhamento Selecao

        #region Analise Seleção

        public List<LancamentoResultadoItemData> BuscarInscricoesOfertaParaSelecao(long seqProcesso, List<long> inscricoesOfertas)
        {
            return InscricaoOfertaDomainService.BuscarInscricoesOferta(seqProcesso, inscricoesOfertas)
                                                    .TransformList<LancamentoResultadoItemData>();
        }

        public void SalvarLancamentos(List<LancamentoResultadoItemData> list)
        {
            InscricaoOfertaDomainService.SalvarLancamentos(
                        list.TransformList<LancamentoResultadoItemVO>());
        }

        public bool VerificaDisponibilidadeVagas(long seqOferta, List<LancamentoResultadoItemData> lancamentos)
        {
            return InscricaoOfertaDomainService.VerificaDisponibilidadeVagas(seqOferta, lancamentos.TransformList<LancamentoResultadoItemVO>());
        }

        public void DesfazerLancamentoResultado(List<long> seqsInscricaoOferta)
        {
            InscricaoOfertaDomainService.DesfazerLancamentoResultado(seqsInscricaoOferta);
        }

        public CabecalhoSelecaoData BuscarCabecalhoInscricaoOferta(long seqInscricaoOferta)
        {
            var cabecalhoSelecaoOferta = InscricaoOfertaDomainService.SearchProjectionByKey(new SMCSeqSpecification<InscricaoOferta>(seqInscricaoOferta),
                                                x => new CabecalhoSelecaoData
                                                {
                                                    TipoProcesso = x.Inscricao.Processo.TipoProcesso.Descricao,
                                                    SeqProcesso = x.Inscricao.SeqProcesso,
                                                    Descricao = x.Inscricao.Processo.Descricao,
                                                    SeqInscricao = x.SeqInscricao,
                                                    Candidato = (x.Inscricao.Inscrito.NomeSocial != null) ?
                                                                    x.Inscricao.Inscrito.NomeSocial + " (" + x.Inscricao.Inscrito.Nome + ")" :
                                                                    x.Inscricao.Inscrito.Nome,
                                                    GrupoOferta = x.Oferta.GrupoOferta.Nome,
                                                    Opcao = x.NumeroOpcao + "ª",
                                                    SeqOferta = x.SeqOferta,
                                                    Oferta = x.Oferta.Nome
                                                });
            cabecalhoSelecaoOferta.Oferta = OfertaDomainService.BuscarHierarquiaOfertaCompleta(cabecalhoSelecaoOferta.SeqOferta, false).DescricaoCompleta;

            return cabecalhoSelecaoOferta;
        }

        public List<HistoricoSituacaoData> BuscarHistoricosSituacao(long seqInscricaoOferta)
        {
            return InscricaoOfertaHistoricoSituacaoDomainService.BuscarHistoricosSituacao(seqInscricaoOferta)
                                                                    .TransformList<HistoricoSituacaoData>();
        }

        public HistoricoSituacaoData BuscarHistoricoSituacao(long seqHistoricoSituacao)
        {
            return InscricaoOfertaHistoricoSituacaoDomainService.BuscarHistoricoSituacao(seqHistoricoSituacao)
                                                                    .Transform<HistoricoSituacaoData>();
        }

        public long SalvarAlteracaoSituacao(HistoricoSituacaoData data)
        {
            return InscricaoOfertaHistoricoSituacaoDomainService.SalvarAlteracaoSituacao(data.Transform<HistoricoSituacaoVO>());
        }

        public List<OpcoesInscricaoData> BuscarOpcoesInscricao(long seqInscricao)
        {
            var spec = new InscricaoOfertaFilterSpecification() { SeqInscricao = seqInscricao };
            spec.SetOrderBy(o => o.NumeroOpcao);
            var opcoesInscricao = InscricaoOfertaDomainService.SearchProjectionBySpecification(spec,
                                            x => new OpcoesInscricaoData
                                            {
                                                Opcao = x.NumeroOpcao + "ª",
                                                Oferta = x.Oferta.Nome,
                                                SeqOferta = x.SeqOferta,
                                                Situacao = x.HistoricosSituacao.FirstOrDefault(f => f.Atual).TipoProcessoSituacao.Descricao
                                            }).ToList();

            foreach (var item in opcoesInscricao)
            {
                item.Oferta = OfertaDomainService.BuscarHierarquiaOfertaCompleta(item.SeqOferta, false).DescricaoCompleta;
            }

            return opcoesInscricao;
        }

        public List<ConvocadoData> BuscarInscricoesOfertaParaConvocacao(long seqProcesso, List<long> seqsInscricaoOferta)
        {
            return InscricaoOfertaDomainService.BuscarInscricoesOfertaParaConvocacao(seqProcesso, seqsInscricaoOferta)
                                                                    .TransformList<ConvocadoData>();
        }

        #endregion Analise Seleção 

        #region Alteração Oferta

        public OfertaCabecalhoData BuscarOfertaCabecalho(OfertaFiltroData filtro)
        {
            return InscricaoOfertaDomainService.BuscarOfertaCabecalho(filtro.Transform<InscricaoOfertaFilterSpecification>())
                                                                            .Transform<OfertaCabecalhoData>();
        }
        public OfertaAlteracaoData BuscarOferta(OfertaFiltroData filtro)
        {
            return InscricaoOfertaDomainService.BuscarOferta(filtro.Transform<InscricaoOfertaFilterSpecification>())
                                                                   .Transform<OfertaAlteracaoData>();
        }

        public void SalvarAlteracaoOferta(OfertaAlteracaoData oferta)
        {
            InscricaoOfertaDomainService.SalvarAlteracaoOferta(oferta.Transform<OfertaVO>());
        }

        #endregion Alteração Oferta

        public OfertaAlteracaoData BuscarDadosOferta(OfertaFiltroData filtro)
        {
            return InscricaoOfertaDomainService.BuscarDadosOferta(filtro.Transform<InscricaoOfertaFilterSpecification>()).Transform<OfertaAlteracaoData>();
        }
    }
}