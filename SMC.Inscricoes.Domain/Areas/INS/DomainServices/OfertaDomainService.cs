using SautinSoft.Document;
using SMC.Financeiro.ServiceContract.Areas.BNK.Data;
using SMC.Financeiro.ServiceContract.Areas.TXA.Data;
using SMC.Financeiro.ServiceContract.BLT;
using SMC.Financeiro.ServiceContract.TXA.Data;
using SMC.Formularios.Service.Areas.FRM.Services;
using SMC.Formularios.ServiceContract.Areas.TMP.Interfaces;
using SMC.Framework;
using SMC.Framework.Domain;
using SMC.Framework.Extensions;
using SMC.Framework.Model;
using SMC.Framework.Repository;
using SMC.Framework.Security;
using SMC.Framework.Specification;
using SMC.Framework.UnitOfWork;
using SMC.Framework.Util;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using SMC.Inscricoes.Common.Areas.INS.Exceptions;
using SMC.Inscricoes.Common.Areas.INS.Exceptions.HierarquiaOferta;
using SMC.Inscricoes.Common.Areas.SEL.Exceptions;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using SMC.Inscricoes.Domain.Areas.INS.Validators;
using SMC.Inscricoes.Domain.Areas.INS.ValueObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls.WebParts;

namespace SMC.Inscricoes.Domain.Areas.INS.DomainServices
{
    public class OfertaDomainService : InscricaoContextDomain<Oferta>
    {
        #region DomainServices
        private ProcessoDomainService ProcessoDomainService
        {
            get { return this.Create<ProcessoDomainService>(); }
        }

        private InscricaoCodigoAutorizacaoDomainService InscricaoCodigoAutorizacaoDomainService
        {
            get { return this.Create<InscricaoCodigoAutorizacaoDomainService>(); }
        }

        private InscricaoDomainService InscricaoDomainService
        {
            get { return this.Create<InscricaoDomainService>(); }
        }

        private OfertaPeriodoTaxaDomainService OfertaPeriodoTaxaDomainService
        {
            get { return this.Create<OfertaPeriodoTaxaDomainService>(); }
        }

        private TaxaDomainService TaxaDomainService
        {
            get { return this.Create<TaxaDomainService>(); }
        }

        private GrupoOfertaDomainService GrupoOfertaDomainService
        {
            get { return this.Create<GrupoOfertaDomainService>(); }
        }

        private EtapaProcessoDomainService EtapaProcessoDomainService
        {
            get { return this.Create<EtapaProcessoDomainService>(); }
        }

        private HierarquiaOfertaDomainService HierarquiaOfertaDomainService
        {
            get { return Create<HierarquiaOfertaDomainService>(); }
        }

        private InscritoDomainService InscritoDomainService
        {
            get { return this.Create<InscritoDomainService>(); }
        }

        private InscricaoBoletoTituloDomainService InscricaoBoletoTituloDomainService
        {
            get { return this.Create<InscricaoBoletoTituloDomainService>(); }
        }

        private PermissaoInscricaoForaPrazoDomainService PermissaoInscricaoForaPrazoDomainService
        {
            get { return Create<PermissaoInscricaoForaPrazoDomainService>(); }
        }

        private InscricaoBoletoTaxaDomainService InscricaoBoletoTaxaDomainService
        {
            get { return Create<InscricaoBoletoTaxaDomainService>(); }
        }
        private TipoProcessoDomainService TipoProcessoDomainService => Create<TipoProcessoDomainService>();

        private ConfiguracaoEtapaDomainService ConfiguracaoEtapaDomainService => Create<ConfiguracaoEtapaDomainService>();

        private GrupoTaxaDomainService GrupoTaxaDomainService => Create<GrupoTaxaDomainService>();

        #endregion DomainServices

        #region Services

        private IIntegracaoFinanceiroService FinanceiroService
        {
            get { return this.Create<IIntegracaoFinanceiroService>(); }
        }

        private ISituacaoService SituacaoService
        {
            get { return this.Create<ISituacaoService>(); }
        }

        private IEtapaService EtapaService
        {
            get { return Create<IEtapaService>(); }
        }
        private ITemplateProcessoService TemplateProcessoService => Create<ITemplateProcessoService>();

        #endregion Services

        private string _buscarArvoreOfertas = @"WITH OFERTAS AS
                        (
	                        SELECT	ho.seq_hierarquia_oferta as Seq,
			                        ho.seq_hierarquia_oferta_pai as SeqPai,
			                        ho.nom_hierarquia_oferta as Descricao,
			                        IsLeaf = CONVERT(bit, 1)
	                        FROM	oferta o
	                        JOIN	hierarquia_oferta ho
			                        ON o.seq_hierarquia_oferta = ho.seq_hierarquia_oferta
	                        JOIN	processo p
			                        ON ho.seq_processo = p.seq_processo
	                        WHERE	(@seq_grupo_oferta = 0 OR seq_grupo_oferta = @seq_grupo_oferta)
	                        AND		(@seq_processo = 0 OR p.seq_processo = @seq_processo)
	                        AND		o.ind_ativo = 1
	                        UNION ALL
	                        SELECT	hot.seq_hierarquia_oferta as Seq,
			                        hot.seq_hierarquia_oferta_pai as SeqPai,
			                        hot.nom_hierarquia_oferta as Descricao,
			                        IsLeaf = CONVERT(bit, 0)
	                        FROM	OFERTAS
	                        JOIN	hierarquia_oferta hot
			                        ON hot.seq_hierarquia_oferta = OFERTAS.SeqPai
                        )

                        SELECT DISTINCT *
                        FROM	OFERTAS";

        public Oferta BuscarHierarquiaOfertaCompleta(long seqOferta, bool ExibirDescricaoPorNome = true)
        {
            var oferta = this.SearchByDepth(new SMCSeqSpecification<Oferta>(seqOferta), 10, x => x.HierarquiaOfertaPai, x => x.Processo).FirstOrDefault();

            AdicionarDescricaoCompleta(oferta, oferta.Processo.ExibirPeriodoAtividadeOferta, ExibirDescricaoPorNome);
            return oferta;
        }
        public Oferta BuscarOfertaComDescicaoCompleta(long seqOferta, bool ExibirDescricaoPorNome = true)
        {
            var oferta = this.SearchByKey(new SMCSeqSpecification<Oferta>(seqOferta), IncludesOferta.Processo);
            AdicionarDescricaoCompleta(oferta, oferta.Processo.ExibirPeriodoAtividadeOferta, ExibirDescricaoPorNome);

            return oferta;
        }

        /// <summary>
        /// Recve uma lista de seqs e  retorna uma lista de ofertas
        /// </summary>
        /// <param name="seqsOferta"></param>
        /// <returns>Lista de ofertas</returns>
        public List<Oferta> BuscarListaHierarquiaOfertaCompleta(List<long> seqsOferta)
        {
            var spec = new OfertaFilterSpecification() { SeqsOfertas = seqsOferta.ToArray() };
            spec.SetOrderBy(x => x.Nome);
            return this.SearchByDepth(spec, 10, x => x.HierarquiaOfertaPai).ToList();
        }

        /// <summary>
        /// Consulta a posição consolidada por processo para listagem
        /// </summary>
        public List<PosicaoConsolidadaGrupoOfertaVO> BuscarPosicaoConsolidadaProcesso(PosicaoConsolidadaGrupoOfertaFilterSpecification filter, out int total)
        {

            if (filter.SeqGrupoOferta.HasValue && filter.SeqGrupoOferta.Value == default(long)) filter.SeqGrupoOferta = null;

            if (filter.SeqOferta.HasValue && filter.SeqOferta.Value == default(long)) filter.SeqOferta = null;

            var numeroPagina = filter.PageNumber;
            var tamanhoPagina = filter.MaxResults;

            var spec = new ProcessoFilterSpecification()
            {
                SeqProcesso = filter.SeqProcesso,
                TokenEtapa = new string[] { TOKENS.ETAPA_INSCRICAO }
            };

            var result = ProcessoDomainService.ResultadoPosicaoConsolidada(spec, out total).FirstOrDefault();

            result.GruposOfertas.OrderBy(o => o.NomeGrupo)
                .ThenBy(tb => tb.Ofertas.OrderBy(ob => ob.DescricaoOferta));

            var seqsGrupos = result.GruposOfertas.Select(s => s.SeqGrupoOferta).ToList();

            if (filter.SeqGrupoOferta.HasValue)
            {
                seqsGrupos = seqsGrupos.FindAll(f => f == filter.SeqGrupoOferta.Value);
            }

            var listaGrupos = new List<PosicaoConsolidadaGrupoOfertaVO>();


            foreach (var grupo in result.GruposOfertas)
            {
                var inscricoesGrupoOferta = result.Inscricoes.Where(i => i.SeqGrupoOferta == grupo.SeqGrupoOferta).ToList();

                if (!inscricoesGrupoOferta.SMCAny())
                {
                    continue;
                }

                var listaGrupoPosicaoConsolidada = new List<ResultadoPosicaoConsolidadaVO>();

                listaGrupoPosicaoConsolidada.Add(new ResultadoPosicaoConsolidadaVO()
                {
                    Descricao = grupo.NomeGrupo,
                    Inscricoes = inscricoesGrupoOferta
                });

                var totalGrupo = ProcessoDomainService.MontarContabilizadorPosicaoConsoliada(listaGrupoPosicaoConsolidada, grupo.SeqGrupoOferta).FirstOrDefault();

                var posicaoGrupo = new PosicaoConsolidadaGrupoOfertaVO()
                {

                    Total = totalGrupo.Total,
                    Confirmadas = totalGrupo.Confirmadas,
                    Indeferidos = totalGrupo.Indeferidos,
                    NaoConfirmadas = totalGrupo.NaoConfirmadas,
                    Deferidos = totalGrupo.Deferidos,
                    DocumentacoesEntregues = totalGrupo.DocumentacoesEntregues,
                    Canceladas = totalGrupo.Canceladas,
                    Finalizadas = totalGrupo.Finalizadas,
                    Iniciadas = totalGrupo.Iniciadas,
                    Pagas = totalGrupo.Pagas,
                    Descricao = totalGrupo.Descricao,
                    Seq = totalGrupo.Seq,
                    SeqProcesso = filter.SeqProcesso,
                    OfertasNaoSelecionadas = totalGrupo.OfertasNaoSelecionadas

                };

                //   var seqsOfertas = inscricoesGrupoOferta.SelectMany(x => x.Ofertas.Select(a => a.SeqOferta)).Distinct().ToList();
                var ofertas = result.GruposOfertas.Where(w => w.SeqGrupoOferta == grupo.SeqGrupoOferta).SelectMany(sm => sm.Ofertas).ToList();

                if (filter.SeqItemHierarquiaOferta.HasValue)
                {
                    ofertas = ofertas.Where(w => w.HierarquiaCompleta.Contains(filter.SeqItemHierarquiaOferta.Value.ToString())).ToList(); //.Select(a => a.SeqOferta).ToList();

                }

                if (filter.SeqOferta.HasValue)
                {
                    ofertas = ofertas.FindAll(f => f.SeqOferta == filter.SeqOferta.Value);
                }

                foreach (var oferta in ofertas)
                {
                    var listaOfertaPosicaoConsolidada = new List<ResultadoPosicaoConsolidadaVO>();

                    var inscricaoOferta = inscricoesGrupoOferta.Where(x => x.Ofertas.Any(a => a.SeqOferta == oferta.SeqOferta)).ToList();

                    listaOfertaPosicaoConsolidada.Add(new ResultadoPosicaoConsolidadaVO()
                    {
                        Descricao = oferta.DescricaoOferta,
                        Inscricoes = inscricaoOferta
                    });

                    var totalOferta = ProcessoDomainService.MontarContabilizadorPosicaoConsoliada(listaOfertaPosicaoConsolidada).FirstOrDefault();

                    var posicaoOferta = new PosicaoConsolidadaOfertaVO()
                    {
                        Total = totalOferta.Total,
                        Confirmadas = totalOferta.Confirmadas,
                        Indeferidos = totalOferta.Indeferidos,
                        NaoConfirmadas = totalOferta.NaoConfirmadas,
                        Deferidos = totalOferta.Deferidos,
                        DocumentacoesEntregues = totalOferta.DocumentacoesEntregues,
                        Canceladas = totalOferta.Canceladas,
                        Finalizadas = totalOferta.Finalizadas,
                        Iniciadas = totalOferta.Iniciadas,
                        Pagas = totalOferta.Pagas,
                        Descricao = totalOferta.Descricao,
                        Seq = totalOferta.Seq,
                        NomeGrupo = grupo.NomeGrupo,
                        SeqProcesso = filter.SeqProcesso
                    };

                    posicaoGrupo.PosicoesConsolidadasOfertas.Add(posicaoOferta);
                }

                listaGrupos.Add(posicaoGrupo);
            }

            total = listaGrupos.Count();

            //Essa condição é para não trazer grupos vazios
            return listaGrupos.Where(a => a.PosicoesConsolidadasOfertas.Any()).ToList();
        }

        public List<PosicaoConsolidadaGrupoOfertaVO> MontarContabilizadorPosicaoConsoliada(List<ResultadoPosicaoConsolidadaVO> listaPosicao)
        {
            var retorno = new List<PosicaoConsolidadaGrupoOfertaVO>();

            var motivosCanceladoTeste = SituacaoService.BuscarSeqMotivosSituacaoPorToken(TOKENS.MOTIVO_INSCRICAO_CANCELADA_TESTE);



            foreach (var posicao in listaPosicao)
            {
                PosicaoConsolidadaGrupoOfertaVO posicaoConsolidada = new PosicaoConsolidadaGrupoOfertaVO();

                posicaoConsolidada.Descricao = posicao.Descricao;
                posicaoConsolidada.Seq = posicao.Seq;

                //Quantidade de inscrições: total de inscrições no processo, exceto as canceladas cujo motivo é CANCELADA_TESTE.
                posicaoConsolidada.Total = posicao.Inscricoes.Count();

                //Deferidas: o total de inscrições cuja situação atual da etapa de inscrição é INSCRICAO_DEFERIDA.
                posicaoConsolidada.Deferidos = posicao.Inscricoes.Count(i => i.HistoricosSituacao.Any(s => s.AtualEtapa &&
                                                                        s.Token == TOKENS.SITUACAO_INSCRICAO_DEFERIDA));

                //Indeferidas: o total de inscrições cuja situação atual é INSCRICAO_INDEFERIDA
                posicaoConsolidada.Indeferidos = posicao.Inscricoes.Count(i => i.HistoricosSituacao.Any(s => s.Atual &&
                                                                            s.Token == TOKENS.SITUACAO_INSCRICAO_INDEFERIDA));

                //Iniciadas: o total de inscrições com a situação atual igual à INSCRICAO_INICIADA e cuja oferta foi selecionada.
                posicaoConsolidada.Iniciadas = posicao.Inscricoes.Count(i => i.HistoricosSituacao.Any(s => s.Atual &&
                                                                        s.Token == TOKENS.SITUACAO_INSCRICAO_INICIADA && i.Ofertas.Any()));

                //Oferta não Selecionada: o total de inscrições cuja oferta não foi selecionada.
                posicaoConsolidada.OfertasNaoSelecionadas = posicao.Inscricoes.Count(i => i.HistoricosSituacao.Any(s => s.Atual &&
                                                                                      s.Token == TOKENS.SITUACAO_INSCRICAO_INICIADA && !i.Ofertas.Any()));

                //Finalizadas: o total de inscrições que possuem no “histórico de situação” a situação INSCRICAO_FINALIZADA e que a situação atual não é INSCRICAO_CANCELADA e tampouco INSCRICAO_INICIADA.
                posicaoConsolidada.Finalizadas = posicao.Inscricoes.Count(i => i.HistoricosSituacao.Any(s => s.Atual &&
                                                                                       s.Token != TOKENS.SITUACAO_INSCRICAO_CANCELADA &&
                                                                                       s.Token != TOKENS.SITUACAO_INSCRICAO_INICIADA)
                                                                                       && i.HistoricosSituacao.Any(s => s.Token == TOKENS.SITUACAO_INSCRICAO_FINALIZADA));

                //Pagas: o total de inscrições com o campo “inscrições pagas” igual a “Sim”, cuja situação atual não é INSCRICAO_CANCELADA.
                posicaoConsolidada.Pagas = posicao.Inscricoes.Count(i => i.TituloPago && !i.HistoricosSituacao
                                   .Any(s => s.Atual && s.Token == TOKENS.SITUACAO_INSCRICAO_CANCELADA));

                //Documentação Entregue: o total de inscrições com o campo “documentação entregue” igual a “Sim”, cuja situação atual não é INSCRICAO_CANCELADA.
                posicaoConsolidada.DocumentacoesEntregues = posicao.Inscricoes.Count(i => i.DocumentacaoEntregue &&
                                                                                     !i.HistoricosSituacao.Any(s => s.Atual && s.Token == TOKENS.SITUACAO_INSCRICAO_CANCELADA));

                //Confirmadas: o total de inscrições que possuem no "histórico de situação" cuja a situação atual da inscrição é INSCRICAO_CONFIRMADA, INSCRICAO_DEFERIDA e INSCRICAO_INDEFERIDA.
                posicaoConsolidada.Confirmadas = posicao.Inscricoes.Count(i => i.HistoricosSituacao.Any(s => s.Atual &&
                                                                          (s.Token == TOKENS.SITUACAO_INSCRICAO_CONFIRMADA ||
                                                                           s.Token == TOKENS.SITUACAO_INSCRICAO_INDEFERIDA ||
                                                                           s.Token == TOKENS.SITUACAO_INSCRICAO_DEFERIDA)));

                //Não Confirmadas: o total de inscrições cuja situação atual é INSCRICAO_FINALIZADA.
                posicaoConsolidada.NaoConfirmadas = posicao.Inscricoes.Count(i => i.HistoricosSituacao.Any(s => s.Atual &&
                                                                             s.Token == TOKENS.SITUACAO_INSCRICAO_FINALIZADA));

                //Canceladas: o total de inscrições cuja situação atual é INSCRICAO_CANCELADA e o motivo, quando existir, não é CANCELADA_TESTE.
                posicaoConsolidada.Canceladas = posicao.Inscricoes.Count(i => i.HistoricosSituacao.Any(s => s.Atual
                                                                          && s.Token == TOKENS.SITUACAO_INSCRICAO_CANCELADA)
                                                                          && !i.HistoricosSituacao.Any(f => f.Atual && f.SeqMotivoSituacaoSGF.HasValue
                                                                          && motivosCanceladoTeste.Contains(f.SeqMotivoSituacaoSGF.Value)));
                retorno.Add(posicaoConsolidada);
            }

            return retorno;
        }

        public List<PosicaoConsolidadaPorGrupoOfertaListaVO> ConsultaPosicaoConsolidadaSelecaoGrupoOferta(PosicaoConsolidadaGrupoOfertaFilterSpecification filter, out int total)
        {
            var seqsEtapasSituacaoDeferida = EtapaService.BuscarSeqEtapasPorTokenSituacao(TOKENS.SITUACAO_INSCRICAO_DEFERIDA);

            if (filter.SeqGrupoOferta.HasValue && filter.SeqGrupoOferta.Value == default(long)) filter.SeqGrupoOferta = null;
            if (filter.SeqOferta.HasValue && filter.SeqOferta.Value == default(long)) filter.SeqOferta = null;
            var numeroPagina = filter.PageNumber;
            var tamanhoPagina = filter.MaxResults;
            filter.PageNumber = 1;
            filter.MaxResults = Int32.MaxValue;
            List<PosicaoConsolidadaPorOfertaListaVO> vos = this.SearchProjectionBySpecification(filter, x => new PosicaoConsolidadaPorOfertaListaVO
            {
                Seq = x.Seq,
                SeqProcesso = x.SeqProcesso,
                SeqGrupoOferta = x.SeqGrupoOferta,
                Descricao = x.DescricaoCompleta,
                NomeGrupo = x.GrupoOferta.Nome,
                HierarquiaCompleta = x.HierarquiaCompleta,

                CandidatosConfirmados = x.GrupoOferta.InscricoesGrupoOferta.Where(i => i.HistoricosSituacao.Any(
                            g => g.Atual &&
                            // Verifica se o processo possui INSCRICAO_DEFERIDA em seu template. Se existir, utiliza essa situação para candidado confirmado,
                            // caso contrário, utiliza INSCRICAO_CONFIRMADA
                            g.TipoProcessoSituacao.Token == ((i.Processo.EtapasProcesso.Select(e => e.SeqEtapaSGF).Intersect(seqsEtapasSituacaoDeferida).Any())
                                                                ? TOKENS.SITUACAO_INSCRICAO_DEFERIDA : TOKENS.SITUACAO_INSCRICAO_CONFIRMADA)

                        )
                    ).SelectMany(i => i.Ofertas.SelectMany(f => f.HistoricosSituacao.Where(g => g.Atual && f.SeqOferta == x.Seq))).Count(),

                CandidatosDesistentes = x.InscricoesOferta.Count(
                        f => f.HistoricosSituacao.Any(g => g.Atual && g.TipoProcessoSituacao.Token == TOKENS.SITUACAO_CANDIDATO_DESISTENTE)),

                CandidatosReprovados = x.InscricoesOferta.Count(
                        f => f.HistoricosSituacao.Any(g => g.Atual && g.TipoProcessoSituacao.Token == TOKENS.SITUACAO_CANDIDATO_REPROVADO)),

                CandidatosSelecionados = x.InscricoesOferta.Count(
                        f => f.HistoricosSituacao.Any(g => g.Atual && g.TipoProcessoSituacao.Token == TOKENS.SITUACAO_CANDIDATO_SELECIONADO)),

                CandidatosExcedentes = x.InscricoesOferta.Count(
                        f => f.HistoricosSituacao.Any(g => g.Atual && g.TipoProcessoSituacao.Token == TOKENS.SITUACAO_CANDIDATO_EXCEDENTE)),

                Convocados = x.InscricoesOferta.Count(
                        f => f.HistoricosSituacao.Any(g => g.Atual && g.EtapaProcesso.Token == TOKENS.ETAPA_CONVOCACAO)),

                ConvocadosDesistentes = x.InscricoesOferta.Count(
                        f => f.HistoricosSituacao.Any(g => g.Atual && g.TipoProcessoSituacao.Token == TOKENS.SITUACAO_CONVOCADO_DESISTENTE)),

                ConvocadosConfirmados = x.InscricoesOferta.Count(
                        f => f.HistoricosSituacao.Any(g => g.Atual && g.TipoProcessoSituacao.Token == TOKENS.SITUACAO_CONVOCADO_CONFIRMADO))
            }).ToList();

            // Filtra por um galho da hierarquia
            if (filter.SeqItemHierarquiaOferta.HasValue)
            {
                var tmpList = new List<PosicaoConsolidadaPorOfertaListaVO>();
                foreach (var item in vos)
                {
                    var oferta = new Oferta() { HierarquiaCompleta = item.HierarquiaCompleta };
                    if (oferta.VerificarHierarquia(filter.SeqItemHierarquiaOferta.Value))
                        tmpList.Add(item);
                }
                vos = tmpList;
            }

            List<PosicaoConsolidadaPorGrupoOfertaListaVO> listaGrupos = new List<PosicaoConsolidadaPorGrupoOfertaListaVO>();
            foreach (long? SeqGrupo in vos.Where(x => !filter.SeqGrupoOferta.HasValue ||
                 (filter.SeqGrupoOferta.Value == x.SeqGrupoOferta)).Select(x => x.SeqGrupoOferta).Distinct())
            {
                var grupoOferta = vos.Where(x => x.SeqGrupoOferta == SeqGrupo);
                PosicaoConsolidadaPorGrupoOfertaListaVO posicaoGrupo = new PosicaoConsolidadaPorGrupoOfertaListaVO
                {
                    CandidatosConfirmados = grupoOferta.Sum(x => x.CandidatosConfirmados),
                    CandidatosDesistentes = grupoOferta.Sum(x => x.CandidatosDesistentes),
                    CandidatosReprovados = grupoOferta.Sum(x => x.CandidatosReprovados),
                    CandidatosSelecionados = grupoOferta.Sum(x => x.CandidatosSelecionados),
                    CandidatosExcedentes = grupoOferta.Sum(x => x.CandidatosExcedentes),
                    Convocados = grupoOferta.Sum(x => x.Convocados),
                    ConvocadosDesistentes = grupoOferta.Sum(x => x.ConvocadosDesistentes),
                    ConvocadosConfirmados = grupoOferta.Sum(x => x.ConvocadosConfirmados),

                    Descricao = grupoOferta.First().NomeGrupo,
                    Seq = SeqGrupo.GetValueOrDefault(),
                    SeqProcesso = filter.SeqProcesso
                };

                posicaoGrupo.PosicoesConsolidadasOfertas =
                    grupoOferta.Where(x =>
                        (!filter.SeqOferta.HasValue || (filter.SeqOferta.Value == x.Seq))).OrderBy(x => x.Descricao).ToList();
                listaGrupos.Add(posicaoGrupo);
            }
            total = listaGrupos.Count;
            return listaGrupos.OrderBy(x => x.Descricao).Skip((numeroPagina - 1) * tamanhoPagina).Take(tamanhoPagina).ToList();
        }

        public OfertaVO BuscarOfertaPorSeq(long seqOferta)
        {

            var spec = new OfertaFilterSpecification() { SeqOferta = seqOferta };
            var Oferta = this.SearchByKey(spec);

            return Oferta.Transform<OfertaVO>();
        }

        public SMCDatasourceItem[] BuscarOfertasKeyValue(OfertaFilterSpecification filtro)
        {
            List<SMCDatasourceItem> vos = new List<SMCDatasourceItem>();
            var seqOfertas = this.SearchProjectionBySpecification(filtro, x => x.Seq);
            foreach (var seq in seqOfertas)
            {
                vos.Add(new SMCDatasourceItem
                {
                    Seq = seq,
                    Descricao = this.BuscarHierarquiaOfertaCompleta(seq, false).DescricaoCompleta
                });
            }
            return vos.ToArray();
        }

        public SMCDatasourceItem[] BuscarSelecaoOfertasInscricaoKeyValue(OfertaFilterSpecification filtro)
        {
            List<SMCDatasourceItem> vos = new List<SMCDatasourceItem>();
            var ofertas = this.SearchProjectionBySpecification(filtro, x => x.Seq).ToList();
            foreach (var item in ofertas)
            {

                var oferta = this.BuscarOfertaComDescicaoCompleta(item, false);

                vos.Add(new SMCDatasourceItem
                {
                    Seq = item,

                    Descricao = oferta.DescricaoCompleta

                });

            }
            return vos.ToArray();
        }

        public SMCDatasourceItem[] BuscarOfertasKeyValue(List<long> seqOfertas)
        {
            List<SMCDatasourceItem> vos = new List<SMCDatasourceItem>();
            foreach (var seq in seqOfertas)
            {
                vos.Add(new SMCDatasourceItem
                {
                    Seq = seq,
                    Descricao = this.BuscarHierarquiaOfertaCompleta(seq, false).DescricaoCompleta
                });
            }
            return vos.ToArray();
        }

        /// <summary>
        /// Retorna a lista de níveis de ensino associadaos a uma instituição de ensino
        /// </summary>
        public List<HierarquiaOfertaArvoreVO> BuscarHierarquiaOfertasArvore(long? seqGrupoOferta, long? seqHierarquiaOferta, long? seqProcesso)
        {
            DisableFilter(FILTERS.UNIDADE_RESPONSAVEL);

            if (seqHierarquiaOferta.HasValue && seqProcesso == null)
            {
                seqProcesso = HierarquiaOfertaDomainService.SearchProjectionByKey(new SMCSeqSpecification<HierarquiaOferta>(seqHierarquiaOferta.Value),
                                                                        x => x.SeqProcesso);
            }

            var parameters = new SMCFuncParameter[]
            {
                new SMCFuncParameter("seq_grupo_oferta", seqGrupoOferta.GetValueOrDefault(), typeof(long)),
                new SMCFuncParameter("seq_processo", seqProcesso.GetValueOrDefault(), typeof(long))
            };

            var ofertas = RawQuery<HierarquiaOfertaArvoreVO>(_buscarArvoreOfertas, parameters);

            // Se a busca for pela hieraquia e não pelo grupo, filtra o resultado
            if (seqHierarquiaOferta.HasValue)
            {
                List<HierarquiaOfertaArvoreVO> ofertasHierarquia = FiltrarHierarquias(seqHierarquiaOferta, ofertas);
                ofertas = ofertasHierarquia;
            }

            EnableFilter(FILTERS.UNIDADE_RESPONSAVEL);

            return ofertas;
        }

        private List<HierarquiaOfertaArvoreVO> FiltrarHierarquias(long? seqHierarquiaOferta, List<HierarquiaOfertaArvoreVO> ofertas)
        {
            // Adiciona o node
            var node = ofertas.SingleOrDefault(f => f.Seq == seqHierarquiaOferta);

            // Se não foi encontrado nenhum item, retorna uma lista vazia
            if (node == null)
                return new List<HierarquiaOfertaArvoreVO>();

            // Cria uma nova lista temporária, para adicionar os pais e filhos do item encontrado
            var ofertasHierarquia = new List<HierarquiaOfertaArvoreVO>()
            {
                node
            };

            // Busca pelos pais
            var tmpList = new List<HierarquiaOfertaArvoreVO>();
            var seqPai = node.SeqPai;
            while (seqPai.HasValue)
            {
                var ofertaPai = ofertas.Single(f => f.Seq == seqPai);
                tmpList.Add(ofertaPai);
                seqPai = ofertaPai.SeqPai;
            }
            ofertasHierarquia.AddRange(tmpList);

            // Busca pelos filhos
            BuscaHierarquiasFilhas(node.Seq, ofertas, ofertasHierarquia);

            return ofertasHierarquia;
        }

        private void BuscaHierarquiasFilhas(long? seqPai, List<HierarquiaOfertaArvoreVO> ofertas, List<HierarquiaOfertaArvoreVO> tmpList)
        {
            if (!seqPai.HasValue)
                return;

            var filhas = ofertas.Where(f => f.SeqPai == seqPai);
            if (filhas.Any())
            {
                foreach (var filha in filhas)
                {
                    tmpList.Add(filha);
                    BuscaHierarquiasFilhas(filha.Seq, ofertas, tmpList);
                }
            }
        }

        public List<HierarquiaOferta> BuscarHierarquiaOfertasArvoreInscricao(long seqGrupoOferta)
        {
            // Busca os dados do usuario logado
            var seqUsuario = SMCContext.User.SMCGetSequencialUsuario();
            if (!seqUsuario.HasValue)
                throw new UsuarioInvalidoException();

            var seqInscrito = InscritoDomainService.BuscarSeqInscrito(seqUsuario.Value);

            var controlaVagaInscricao = GrupoOfertaDomainService.SearchProjectionByKey(seqGrupoOferta, p => p.Processo.ControlaVagaInscricao);

            var spec = new OfertaInscricaoFilterSpecification(DateTime.Now, seqInscrito.Value)
            {
                SeqGrupoOferta = seqGrupoOferta,
                ControlaVagaInscricao = controlaVagaInscricao
            };

            return BuscarHierarquiasOfertas(spec);
        }

        public List<HierarquiaOferta> BuscarHierarquiaCompletaAngular(long seqOferta)
        {
            List<HierarquiaOferta> lista = new List<HierarquiaOferta>();

            var ofertaHierarquiaCompleta = this.BuscarHierarquiaOfertaCompleta(seqOferta);

            //Sempre que exibir a árvore, tem que mostrar somente a descrição da oferta, no nó da oferta e não o caminho completo
            if (!ofertaHierarquiaCompleta.Processo.ExibirPeriodoAtividadeOferta.HasValue || ofertaHierarquiaCompleta.Processo.ExibirPeriodoAtividadeOferta.HasValue && ofertaHierarquiaCompleta.Processo.ExibirPeriodoAtividadeOferta.Value)
            {
                ofertaHierarquiaCompleta.DescricaoCompleta = ofertaHierarquiaCompleta.Nome;
            }

            ofertaHierarquiaCompleta.IsLeaf = true;
            lista.Add(ofertaHierarquiaCompleta);

            //Remover duplicadas
            List<HierarquiaOferta> listaRetorno = new List<HierarquiaOferta>();
            foreach (var hierarquia in lista)
            {
                AdicionarHierarquiaPai(listaRetorno, hierarquia);
            }

            return listaRetorno;

        }

        private List<HierarquiaOferta> BuscarHierarquiasOfertas(SMCSpecification<Oferta> spec)
        {
            //FIX: Aguardando o refactor no search by projection para permitir includes

            var ofertas = this.SearchProjectionBySpecification(spec, x => new { Seq = x.Seq, ExibirPeriodoAtividadeOferta = x.GrupoOferta.Processo.ExibirPeriodoAtividadeOferta });
            List<HierarquiaOferta> lista = new List<HierarquiaOferta>();
            foreach (var item in ofertas)
            {
                var oferta = this.BuscarHierarquiaOfertaCompleta(item.Seq);

                //Sempre que exibir a árvore, tem que mostrar somente a descrição da oferta, no nó da oferta e não o caminho completo
                if (!oferta.Processo.ExibirPeriodoAtividadeOferta.HasValue || oferta.Processo.ExibirPeriodoAtividadeOferta.HasValue && !oferta.Processo.ExibirPeriodoAtividadeOferta.Value && !oferta.EOferta)
                {
                    oferta.DescricaoCompleta = oferta.Nome;
                }


                oferta.IsLeaf = true;
                lista.Add(oferta);

            }

            //Remover duplicadas
            List<HierarquiaOferta> listaRetorno = new List<HierarquiaOferta>();
            foreach (var hierarquia in lista)
            {
                AdicionarHierarquiaPai(listaRetorno, hierarquia);
            }
            return listaRetorno;


        }

        //IMPORTANTE: Toda regra relacionada a formatação da descrição da oferta, deve ser aplicada nesse metodo
        public void AdicionarDescricaoCompleta(Oferta oferta, bool? exibirPeriodoAtividadeOferta, bool ExibirDescricaoPorNome = true)
        {

            //Exibe a descrição formatada somente para os processos que permitem essa exibição
            if (exibirPeriodoAtividadeOferta.HasValue && exibirPeriodoAtividadeOferta.Value)
            {
                // se nao for o elemento pai da lista, adiciona a descrição
                if (oferta.SeqPai != 0 && oferta.DataFimAtividade.HasValue && oferta.DataInicioAtividade.HasValue)
                {

                    //Se o dia de início da atividade for igual ao dia do fim da atividade
                    if (oferta.DataInicioAtividade.Value.Date == oferta.DataFimAtividade.Value.Date)
                    {
                        oferta.DescricaoCompleta = string.Format(" {0} - {1} - {2} às {3} - CH: {4} hora(s)",
                                                            (ExibirDescricaoPorNome ? oferta.Nome : oferta.DescricaoCompleta),
                                                            oferta.DataInicioAtividade.Value.ToString("dd/MM/yyyy"),
                                                            oferta.DataInicioAtividade.Value.ToString("HH:mm"),
                                                            oferta.DataFimAtividade.Value.ToString("HH:mm"),
                                                            oferta.CargaHorariaAtividade);
                    }
                    else
                    {
                        // Senão, se o dia de início da atividade for diferente do dia do fim da atividade, 
                        // o nome da oferta deve ser concatenado com sua data/hora de início e data/hora fim
                        // da atividade, junto à carga horária

                        oferta.DescricaoCompleta = string.Format(" {0} - {1} a {2} - CH: {3} hora(s)",
                                                            (ExibirDescricaoPorNome ? oferta.Nome : oferta.DescricaoCompleta),
                                                            oferta.DataInicioAtividade.Value.ToString("dd/MM/yyyy HH:mm"),
                                                            oferta.DataFimAtividade.Value.ToString("dd/MM/yyyy HH:mm"),
                                                            oferta.CargaHorariaAtividade);
                    }
                }
            }
            else
            {
                oferta.DescricaoCompleta = ExibirDescricaoPorNome ? oferta.Nome : oferta.DescricaoCompleta;
            }
        }

        //IMPORTANTE: Toda regra relacionada a formatação da descrição da oferta, deve ser aplicada nesse metodo
        public void AdicionarPeriodo(Oferta oferta, bool? exibirPeriodoAtividadeOferta)
        {
            //Exibe a descrição formatada somente para os processos que permitem essa exibição
            if (exibirPeriodoAtividadeOferta.HasValue && exibirPeriodoAtividadeOferta.Value)
            {
                // se nao for o elemento pai da lista, adiciona a descrição
                if (oferta.SeqPai != 0 && oferta.DataFimAtividade.HasValue && oferta.DataInicioAtividade.HasValue)
                {

                    //Se o dia de início da atividade for igual ao dia do fim da atividade
                    if (oferta.DataInicioAtividade.Value.Date == oferta.DataFimAtividade.Value.Date)
                    {
                        oferta.DescricaoCompleta = string.Format(" {0} - {1} - {2} às {3} - CH: {4} hora(s)",
                                                            oferta.DescricaoCompleta,
                                                            oferta.DataInicioAtividade.Value.ToString("dd/MM/yyyy"),
                                                            oferta.DataInicioAtividade.Value.ToString("HH:mm"),
                                                            oferta.DataFimAtividade.Value.ToString("HH:mm"),
                                                            oferta.CargaHorariaAtividade);
                    }
                    else
                    {
                        // Senão, se o dia de início da atividade for diferente do dia do fim da atividade, 
                        // o nome da oferta deve ser concatenado com sua data/hora de início e data/hora fim
                        // da atividade, junto à carga horária

                        oferta.DescricaoCompleta = string.Format(" {0} - {1} a {2} - CH: {3} hora(s)",
                                                            oferta.DescricaoCompleta,
                                                            oferta.DataInicioAtividade.Value.ToString("dd/MM/yyyy HH:mm"),
                                                            oferta.DataFimAtividade.Value.ToString("dd/MM/yyyy HH:mm"),
                                                            oferta.CargaHorariaAtividade);
                    }
                }
            }

        }


        private void AdicionarHierarquiaPai(List<HierarquiaOferta> lista, HierarquiaOferta oferta)
        {
            if (oferta.SeqPai.HasValue)
            {
                if (!lista.Any(x => x.Seq == oferta.SeqPai.Value))
                {
                    AdicionarHierarquiaPai(lista, oferta.HierarquiaOfertaPai);
                }
            }
            if (!lista.Any(x => x.Seq == oferta.Seq))
            {
                if (oferta.Processo != null)
                {
                    oferta.DescricaoCompleta = oferta.Processo.ExibirPeriodoAtividadeOferta.HasValue && oferta.Processo.ExibirPeriodoAtividadeOferta.Value && oferta.EOferta ? oferta.DescricaoCompleta : oferta.Nome;
                }
                else
                {
                    oferta.DescricaoCompleta = oferta.Nome;
                }
                lista.Add(oferta);
            }
        }

        /// <summary>
        /// Retorna os sequenciais das ofertas vigentes e ativas para um grupo de oferta
        /// </summary>
        public List<long> BuscarSeqOfertasVigentesAtivas(long SeqGrupoOferta)
        {
            OfertaFilterSpecification spec = new OfertaFilterSpecification
            {
                SeqGrupoOferta = SeqGrupoOferta,
                Ativo = true,
                Vigente = true,
            };
            spec.SetOrderBy(x => x.Nome);
            return this.SearchProjectionBySpecification(spec, x => x.Seq).ToList();
        }

        /// <summary>
        /// Salva uma oferta e realiza todas as validações necessárias
        /// </summary>
        /// <param name="oferta"></param>
        /// <returns></returns>
        public long SalvarOferta(Oferta oferta)
        {
            IEnumerable<long> missingTaxas = null;
            if (oferta.Seq == 0)
            {
                if (oferta.CodigosAutorizacao.Any())
                {
                    var etapasProcesso = ProcessoDomainService.SearchByKey(
                        new SMCSeqSpecification<Processo>(oferta.SeqProcesso), IncludesProcesso.EtapasProcesso
                    );

                    if (etapasProcesso.EtapasProcesso.Any(x => x.Token == TOKENS.ETAPA_INSCRICAO && x.SituacaoEtapa == SituacaoEtapa.Liberada))
                    {
                        throw new OfertaComCodigoAutorizacaoEtapaNaoLiberadaException();
                    }
                }
            }
            else
            {
                missingTaxas = ValidaAlteracoesTaxas(oferta);
            }

            ValidarVigenciaOferta(oferta);
            ValidarTaxas(oferta);
            ValidarPermissoesForaPrazo(oferta);
            ValidarBolsaExAluno(oferta);
            ValidarPeriodoAtividade(oferta);

            // Implementação da RN_INS_118
            oferta.ExigePagamentoTaxa = oferta.Taxas.Any();

            using (var unitOfWork = SMCUnitOfWork.Begin())
            {
                try
                {
                    if (missingTaxas != null)
                    {
                        foreach (var taxa in missingTaxas)
                        {
                            this.OfertaPeriodoTaxaDomainService.DeleteEntity(taxa);
                        }
                    }

                    foreach (var item in oferta.Taxas)
                    {
                        if (item.SeqPermissaoInscricaoForaPrazo.HasValue)
                        {
                            var permissao = PermissaoInscricaoForaPrazoDomainService.SearchByKey(
                                new SMCSeqSpecification<PermissaoInscricaoForaPrazo>(item.SeqPermissaoInscricaoForaPrazo.Value)
                            );
                            item.PermissaoInscricaoForaPrazo = permissao;
                        }
                    }

                    // Ajusta o valor da descrição completa para a hierarquia
                    HierarquiaOfertaDomainService.GerarCamposHierarquia(oferta);
                    oferta.DescricaoComplementar = WebUtility.HtmlDecode(oferta.DescricaoComplementar);
                    this.SaveEntity(oferta, new OfertaValidator());
                    unitOfWork.Commit();

                    return oferta.Seq;
                }
                catch (Exception)
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }

        private IEnumerable<long> ValidaAlteracoesTaxas(Oferta oferta)
        {
            IEnumerable<long> missingTaxas;
            var itemOld = this.SearchByKey(new SMCSeqSpecification<Oferta>(oferta.Seq),
                              x => x.Processo, x => x.Processo.EtapasProcesso, x => x.Taxas, x => x.CodigosAutorizacao
                              , x => x.GrupoOferta, x => x.InscricoesOferta[0].Inscricao.Boletos[0].Taxas);
            var etapaInscricao =
                itemOld.Processo.EtapasProcesso.FirstOrDefault(x => x.Token == TOKENS.ETAPA_INSCRICAO);
            //Teste de alteração da oferta em etapa liberada
            if (etapaInscricao != null && etapaInscricao.SituacaoEtapa == SituacaoEtapa.Liberada
                 && (
                    itemOld.SeqGrupoOferta != oferta.SeqGrupoOferta
                    //|| itemOld.Nome != oferta.Nome
                    //|| itemOld.SeqItemHierarquiaOferta != oferta.SeqItemHierarquiaOferta
                    || itemOld.Ativo != oferta.Ativo
                    //|| itemOld.DataInicio != oferta.DataInicio
                    //|| itemOld.DataFim != oferta.DataFim
                    || itemOld.InscricaoSoComCodigo != oferta.InscricaoSoComCodigo
                    || itemOld.PermiteVariosCodigos != oferta.PermiteVariosCodigos
                    || itemOld.DataCancelamento != oferta.DataCancelamento
                    || itemOld.MotivoCancelamento != oferta.MotivoCancelamento
                    || itemOld.InscricaoSoPorCliente != oferta.InscricaoSoPorCliente
                    || itemOld.LimitePercentualDesconto != oferta.LimitePercentualDesconto
                    || CompararDadosTaxasCodigosOferta(itemOld, oferta))
                )
            {
                throw new AlteracaoOfertaEtapaLiberadaException();
            }

            //Teste de alteração de item de hierarquia de oferta com isncrição
            if (itemOld.SeqItemHierarquiaOferta != oferta.SeqItemHierarquiaOferta)
            {
                var inscricoes =
                    this.InscricaoDomainService.Count(new SituacaoInscricaoProcessoFilterSpecification
                    {
                        SeqProcesso = oferta.SeqProcesso,
                        SeqOferta = oferta.Seq
                    });
                if (inscricoes > 0)
                {
                    throw new AlteracaoOfertaComInscricaoException();
                }
            }

            //Teste de alteração de grupo de oferta após existência de inscrição no grupo de ofera
            //anterior
            if (itemOld.SeqGrupoOferta != null && itemOld.SeqGrupoOferta != oferta.SeqGrupoOferta)
            {
                var inscricoes =
                   this.InscricaoDomainService.Count(new SituacaoInscricaoProcessoFilterSpecification
                   {
                       SeqProcesso = oferta.SeqProcesso,
                       SeqOferta = oferta.Seq,
                       SeqGrupoOferta = itemOld.SeqGrupoOferta
                   });
                if (inscricoes > 0)
                {
                    throw new AlteracaoOfertaGrupoOfertaComInscricaoException(itemOld.GrupoOferta.Nome);
                }
            }

            //Teste de remoção de códigos de autorização já usados em inscrições
            IEnumerable<long> missingCodigosAutorizacao;
            oferta.CodigosAutorizacao.SMCContainsList(itemOld.CodigosAutorizacao, x => x.SeqCodigoAutorizacao, out missingCodigosAutorizacao);
            if (missingCodigosAutorizacao.Any())
            {
                var containsSpec = new SMCContainsSpecification<InscricaoCodigoAutorizacao, long>(
                            x => x.SeqCodigoAutorizacao, missingCodigosAutorizacao.ToArray());
                var ofertaSpec = new InscricaoCodigoAutorizacaoSpecification() { SeqOferta = oferta.Seq };
                var codigosComInscricao = InscricaoCodigoAutorizacaoDomainService.SearchProjectionBySpecification(
                                new SMCAndSpecification<InscricaoCodigoAutorizacao>(containsSpec, ofertaSpec), x => x.CodigoAutorizacao.Codigo);
                if (codigosComInscricao.Any())
                {
                    throw new AlteracaoOfertaCodigoAutorizacaoComInscricaoException(
                        codigosComInscricao.First());
                }
            }

            //Verifica a regra que diz que não pode haver inscrição finalizada sem código para
            //a oferta caso ela seja modificada para exigir código de autorização
            if (itemOld.InscricaoSoComCodigo == false && oferta.InscricaoSoComCodigo == true)
            {
                var inscricoesSemCodigo = InscricaoDomainService
                    .Count(new InscricaoComOfertaSpecification(oferta.Seq));
                if (inscricoesSemCodigo > 0)
                {
                    throw new AlteracaoOfertaSomenteComCodigoException();
                }
            }

            //Verifica a regra para cancelamento de oferta
            if ((oferta.DataCancelamento.HasValue && string.IsNullOrEmpty(oferta.MotivoCancelamento))
                || (!string.IsNullOrEmpty(oferta.MotivoCancelamento) && !oferta.DataCancelamento.HasValue))
            {
                throw new CancelamentoOfertaInvalidoException();
            }

            oferta.Taxas.SMCContainsList(itemOld.Taxas, f => f.Seq, out missingTaxas);

            //Verificação de mudança para taxas
            foreach (var taxa in oferta.Taxas)
            {
                ValidarTaxa(oferta, itemOld, taxa);
            }

            //Verificar se as taxas excluidas já não foram utilizadas  RN_INS_130
            var taxasExcluidas = itemOld.Taxas.Where(f => missingTaxas.Contains(f.Seq));
            foreach (var taxa in taxasExcluidas)
            {
                var totalInscricoesTaxaExcluida = InscricaoBoletoTaxaDomainService.Count(
                            new InscricaoComBoletoTaxaSpecification(oferta.Seq, taxa.DataInicio, taxa.DataFim));
                if (totalInscricoesTaxaExcluida > 0)
                {
                    var descricaoTaxa = this.OfertaPeriodoTaxaDomainService.SearchProjectionByKey(new SMCSeqSpecification<OfertaPeriodoTaxa>(taxa.Seq),
                                            x => x.Taxa.TipoTaxa.Descricao);
                    throw new ExclusaoTipoTaxaJaUtilizadaException(descricaoTaxa);
                }
            }

            return missingTaxas;
        }

        private void ValidarTaxa(Oferta oferta, Oferta itemOld, OfertaPeriodoTaxa taxa)
        {
            var taxaOld = itemOld.Taxas.FirstOrDefault(x => x.Seq == taxa.Seq);
            if (taxaOld != null)
            {
                if (taxaOld.NumeroMinimo < taxa.NumeroMinimo)
                {
                    var inscricoesInvalidas = this.InscricaoDomainService.Count(
                        new InscricaoTaxaComNumeroMinimoSpecification(oferta.Seq, taxa.SeqTaxa, taxa.NumeroMinimo.HasValue ? taxa.NumeroMinimo.Value : 0, taxa.DataInicio, taxa.DataFim));
                    if (inscricoesInvalidas > 0)
                    {
                        var descricaoTaxa = TaxaDomainService.SearchProjectionByKey(
                            new SMCSeqSpecification<Taxa>(taxa.SeqTaxa)
                            , x => x.TipoTaxa.Descricao);
                        throw new InscricaoQuantidadeTaxasInferiorMininoException(descricaoTaxa, taxa.NumeroMinimo.HasValue ? taxa.NumeroMinimo.Value : 0);
                    }
                }

                if (taxaOld.NumeroMaximo > taxa.NumeroMaximo)
                {
                    var inscricoesInvalidas = this.InscricaoDomainService.Count(
                        new InscricaoTaxaComNumeroMaximoSpecification(oferta.Seq, taxa.SeqTaxa, taxa.NumeroMaximo.HasValue ? taxa.NumeroMaximo.Value : 0, taxa.DataInicio, taxa.DataFim));
                    if (inscricoesInvalidas > 0)
                    {
                        var descricaoTaxa = TaxaDomainService.SearchProjectionByKey(
                            new SMCSeqSpecification<Taxa>(taxa.SeqTaxa)
                            , x => x.TipoTaxa.Descricao);
                        throw new InscricaoQuantidadeTaxasSuperiorMaximoException(descricaoTaxa, taxa.NumeroMaximo.HasValue ? taxa.NumeroMaximo.Value : 0);
                    }
                }

                if (taxaOld.NumeroMinimo == 0 && taxa.NumeroMinimo > 0)
                {
                    var totalInscricoes = InscricaoDomainService.Count(
                       new InscricaoComOfertaSpecification(oferta.Seq, taxa.DataInicio, taxa.DataFim));
                    if (totalInscricoes > 0)
                    {
                        throw new InscricaoNaOfertaSemTaxaException();
                    }
                }

                // Novas validações (Planilha onenote 6ª GPI administrativo 29/11/2016)
                int? totalInscricoesPeriodoCompleto = null; //Armazena o total de inscrições para esta taxa, para não precisar buscar mais de uma vez para as validações abaixo
                if (taxa.DataInicio.Date != taxaOld.DataInicio.Date)
                {
                    if (!totalInscricoesPeriodoCompleto.HasValue)
                    {
                        // Faz a busca no banco ordenando pela data maior e menor
                        if (taxa.DataInicio.Date < taxaOld.DataInicio.Date)
                            totalInscricoesPeriodoCompleto = InscricaoBoletoTituloDomainService.Count(
                                new InscricaoComBoletoSpecification(oferta.Seq, taxa.DataInicio.Date, taxaOld.DataInicio.Date));
                        else
                            totalInscricoesPeriodoCompleto = InscricaoBoletoTituloDomainService.Count(
                                new InscricaoComBoletoSpecification(oferta.Seq, taxaOld.DataInicio.Date, taxa.DataInicio.Date));
                    }
                    if (totalInscricoesPeriodoCompleto.GetValueOrDefault() > 0)
                    {
                        throw new AlteracaoTaxaOfertaNaoCobreBoletosGeradosException();
                    }
                }

                if (taxa.DataFim.Date != taxaOld.DataFim.Date)
                {
                    if (!totalInscricoesPeriodoCompleto.HasValue)
                    {
                        if (taxa.DataFim.Date < taxaOld.DataFim.Date)
                            totalInscricoesPeriodoCompleto = InscricaoBoletoTituloDomainService.Count(
                                new InscricaoComBoletoSpecification(oferta.Seq, taxa.DataFim.Date, taxaOld.DataFim.Date));
                        else
                            totalInscricoesPeriodoCompleto = InscricaoBoletoTituloDomainService.Count(
                                new InscricaoComBoletoSpecification(oferta.Seq, taxaOld.DataFim.Date, taxa.DataFim.Date));
                    }
                    if (totalInscricoesPeriodoCompleto.GetValueOrDefault() > 0)
                    {
                        throw new AlteracaoTaxaOfertaNaoCobreBoletosGeradosException();
                    }
                }

                if (taxa.SeqEventoTaxa != taxaOld.SeqEventoTaxa)
                {
                    if (!totalInscricoesPeriodoCompleto.HasValue)
                    {
                        totalInscricoesPeriodoCompleto = InscricaoBoletoTituloDomainService.Count(
                            new InscricaoComBoletoSpecification(oferta.Seq, taxaOld.DataInicio, taxaOld.DataFim));
                    }
                    if (totalInscricoesPeriodoCompleto.GetValueOrDefault() > 0)
                    {
                        throw new AlterarTaxaOfertaEventoTaxaException();
                    }
                }

                if (taxa.SeqParametroCrei != taxaOld.SeqParametroCrei)
                {
                    if (!totalInscricoesPeriodoCompleto.HasValue)
                    {
                        totalInscricoesPeriodoCompleto = InscricaoBoletoTituloDomainService.Count(
                            new InscricaoComBoletoSpecification(oferta.Seq, taxaOld.DataInicio, taxaOld.DataFim));
                    }
                    if (totalInscricoesPeriodoCompleto.GetValueOrDefault() > 0)
                    {
                        throw new AlterarOfertaTaxaParametroCreiException();
                    }
                }
                // Fim (Planilha onenote 6ª GPI administrativo 29/11/2016)
            }
            else
            {
                //nova Taxa
                if (taxa.NumeroMinimo > 0)
                {
                    var totalInscricoes = InscricaoDomainService.Count(
                        new InscricaoComOfertaSpecification(oferta.Seq, taxa.DataInicio, taxa.DataFim));
                    if (totalInscricoes > 0)
                    {
                        throw new InscricaoNaOfertaSemTaxaException();
                    }
                }
            }
        }

        private void VerificarPeriodoTaxas(Oferta oferta, long seqTaxa, IList<OfertaPeriodoTaxa> taxasLista)
        {
            bool periodoCompleto = true;
            if (taxasLista.Count > 0 && taxasLista[0].DataInicio.Date != oferta.DataInicio.Value.Date)
            {
                periodoCompleto = false;
            }
            // Lista de taxas está ordenada por data de inicio
            for (int i = 0; i < taxasLista.Count; i++)
            {
                if (i < taxasLista.Count - 1)
                {
                    if ((taxasLista[i + 1].DataInicio <= taxasLista[i].DataFim
                            && taxasLista[i + 1].DataInicio >= taxasLista[i].DataInicio)
                    || (taxasLista[i + 1].DataFim >= taxasLista[i].DataInicio
                            && taxasLista[i + 1].DataFim <= taxasLista[i].DataFim))
                    {
                        throw new MesmaTaxaLancamentosConcorrentesException();
                    }
                    if ((taxasLista[i + 1].DataInicio.Date - taxasLista[i].DataFim.Date).Days != 1)
                    {
                        periodoCompleto = false;
                    }
                }
                else
                {
                    if (taxasLista[i].DataFim.Date != oferta.DataFim.Value.Date)
                    {
                        periodoCompleto = false;
                    }
                }
            }
            if (!periodoCompleto)
            {
                var descricaoTaxa = this.TaxaDomainService.SearchProjectionByKey(
                    new SMCSeqSpecification<Taxa>(seqTaxa), x => x.TipoTaxa.Descricao);
                throw new PeriodoIncompletoTaxaOfertaException(descricaoTaxa);
            }
        }

        private void ValidarVigenciaOferta(Oferta oferta)
        {
            if (oferta.SeqGrupoOferta.HasValue && oferta.SeqGrupoOferta.Value != default(long))
            {
                var existeConfiguracao = GrupoOfertaDomainService.SearchProjectionByKey(
                    new SMCSeqSpecification<GrupoOferta>(oferta.SeqGrupoOferta.Value),
                    x => x.ConfiguracoesEtapa.Any(c => c.ConfiguracaoEtapa.EtapaProcesso.Token == TOKENS.ETAPA_INSCRICAO)
                );

                if (existeConfiguracao)
                {
                    var config = GrupoOfertaDomainService.SearchProjectionByKey(
                        new SMCSeqSpecification<GrupoOferta>(oferta.SeqGrupoOferta.Value),
                        x => x.ConfiguracoesEtapa.FirstOrDefault(c => c.ConfiguracaoEtapa.EtapaProcesso.Token == TOKENS.ETAPA_INSCRICAO).ConfiguracaoEtapa
                    );

                    if (config != null && (config.DataInicio > oferta.DataInicio || config.DataFim < oferta.DataFim))
                    {
                        throw new PeriodoVigenciaOfertaInvalidoException();
                    }
                }
            }
        }

        private void ValidarTaxas(Oferta oferta)
        {
            if (oferta.Taxas != null)
            {
                var taxasDistintas = oferta.Taxas.Select(x => x.SeqTaxa).Distinct().ToList();
                foreach (var seqTaxa in taxasDistintas)
                {
                    // Verifica apenas as taxas que não são de permissões de inscrição fora do prazo
                    var taxasLista = oferta.Taxas
                        .Where(x => x.SeqTaxa == seqTaxa && !x.SeqPermissaoInscricaoForaPrazo.HasValue)
                        .OrderBy(x => x.DataInicio)
                        .ToList();
                    VerificarPeriodoTaxas(oferta, seqTaxa, taxasLista);
                }


                foreach (var ofertaPeriodoTaxa in oferta.Taxas)
                {
                    if (!ofertaPeriodoTaxa.SeqPermissaoInscricaoForaPrazo.HasValue &&
                        ofertaPeriodoTaxa.DataFim.Date > FinanceiroService.BuscarParametrosCREI(
                            new ParametroCREIFiltroData { SeqParametroCREI = ofertaPeriodoTaxa.SeqParametroCrei }).First().DataVencimentoTitulo.Date)
                    {
                        throw new OfertaVencimentoInvalidoException();
                    }
                }

                ValidaPeriodoTaxaOferta(oferta);

            }
        }

        private void ValidaPeriodoTaxaGrupoTaxa(List<OfertaPeriodoTaxaVO> taxasBase, long seqProcesso)
        {
            //1- Se a Taxa do Período de taxa da oferta pertencer a um Grupo de taxa do Processo 
            //e a Qtd.mínima ou a Qtd. máxima for maior que o Número máximo de itens do Grupo de taxa,
            //se cadastrado, abortar a operação e emitir a mensagem de erro:

            //“A Qtd. mínima ou a Qtd. máxima da taxa < Descrição do Tipo de taxa cuja Qtd. mínima ou a Qtd. máxima 
            // ultrapassa o Número máximo de itens do Grupo de taxa > não pode ultrapassar o número máximo de itens do grupo de taxa
            // < Descrição do Grupo de taxa >.

            var gruposTaxaProcesso = GrupoTaxaDomainService.BuscarGruposTaxaPorSeqProcesso(seqProcesso).ToList();

            if (gruposTaxaProcesso.Any())
            {

                var taxaInvalida = (from grupoTaxa in gruposTaxaProcesso
                                    from taxaProc in grupoTaxa.Itens
                                    from taxaBase in taxasBase
                                    where taxaProc.SeqTaxa == taxaBase.SeqTaxa
                                       && (taxaBase.NumeroMinimo > grupoTaxa.NumeroMaximoItens || taxaBase.NumeroMaximo > grupoTaxa.NumeroMaximoItens)
                                    select (new { descTipoTaxa = taxaBase.Taxa.TipoTaxa.Descricao, descGrupoTaxa = grupoTaxa.Descricao })
                           ).FirstOrDefault();

                if (taxaInvalida != null)
                {

                    throw new QtdMinOuMaxTaxasOfertaExcedeMaxItensGrpTaxaException(taxaInvalida.descTipoTaxa, taxaInvalida.descGrupoTaxa);

                }

                //2- Se o somatório da Qtd. mínima dos Períodos de taxa da oferta das Taxas que pertencem a um
                //mesmo Grupo de taxa for maior que o Número máximo de itens deste grupo, abortar a operação e exibir a mensagem de erro.

                //“Operação não permitida.O somatório da Qtd. mínima dos Períodos de taxa da oferta
                //<Nome da hierarquia completa da Oferta encontrada> cujas taxas pertencem ao grupo
                //<Descrição do Grupo de taxas encontrado> ultrapassou o número máximo de itens deste grupo.

                //Observação: ao verificar os Períodos de taxa coincidentes, caso exista mais de um Período para a mesma Taxa,
                //considerar somente o que tiver a maior Qtd.mínima.

                foreach (var grupoTaxa in gruposTaxaProcesso)
                {

                    var taxasDoGrupoNaOferta = (from taxaGrupo in grupoTaxa.Itens
                                                from taxaBase in taxasBase
                                                where taxaGrupo.SeqTaxa == taxaBase.SeqTaxa
                                                select new OfertaPeriodoTaxaVO
                                                {
                                                    Seq = taxaBase.Seq,
                                                    SeqTaxa = taxaBase.SeqTaxa,
                                                    SeqOferta = taxaBase.SeqOferta,
                                                    Oferta = taxaBase.Oferta,
                                                    NumeroMinimo = taxaBase.NumeroMinimo,
                                                    NumeroMaximo = taxaBase.NumeroMaximo,
                                                    DataInicio = taxaBase.DataInicio,
                                                    DataFim = taxaBase.DataFim,
                                                }).ToList();

                    if (taxasDoGrupoNaOferta.Any())
                    {
                        var seqOfertaInvalidada = GrupoTaxaDomainService.SomatorioQtdMinEmPeriodosCoincidentesExcedeQtdMaxItensGrupoTaxa(grupoTaxa, taxasDoGrupoNaOferta);

                        if (seqOfertaInvalidada > 0)
                        {
                            var descGrupoTaxa = grupoTaxa.Descricao;
                            var descOferta = this.SearchProjectionBySpecification(
                                                    new SMCSeqSpecification<Oferta>(seqOfertaInvalidada),
                                                    oferta => oferta.DescricaoCompleta).FirstOrDefault();

                            throw new SomatorioQtdMinTaxasOfertaMesmoGrpTaxaExcedeException(descOferta, descGrupoTaxa);
                        }
                    }

                }
            }
        }


        private void ValidaPeriodoTaxaOferta(Oferta ofertaBase)
        {
            if (ofertaBase.Seq > 0)
            {

                // 1- O Tipo de cobrança da Taxa do Processo for diferente de “Por oferta” 

                // Busca as taxas com seus respectivos tipos na base de dados
                var taxas = TaxaDomainService.SearchBySpecification(
                    new SMCContainsSpecification<Taxa, long>(p => p.Seq, ofertaBase.Taxas.Select(f => f.SeqTaxa).ToArray()),
                    IncludesTaxa.TipoTaxa
                ).ToList();

                var taxasOfertaBase = new List<OfertaPeriodoTaxaVO>();

                foreach (var ofpt in ofertaBase.Taxas)
                {
                    taxasOfertaBase.Add(new OfertaPeriodoTaxaVO
                    {
                        Seq = ofpt.Seq,
                        DataAlteracao = ofpt.DataAlteracao,
                        DataInicio = ofpt.DataInicio,
                        DataFim = ofpt.DataFim,
                        DataInclusao = ofpt.DataInclusao,
                        NumeroMinimo = ofpt.NumeroMinimo,
                        NumeroMaximo = ofpt.NumeroMaximo,
                        SeqOferta = ofpt.SeqOferta,
                        SeqEventoTaxa = ofpt.SeqEventoTaxa,
                        SeqParametroCrei = ofpt.SeqParametroCrei,
                        SeqTaxa = ofpt.SeqTaxa,
                        Taxa = taxas.Where(x => x.Seq == ofpt.SeqTaxa).Select(x => x).FirstOrDefault(),
                        Oferta = ofpt.Oferta
                    });
                }

                ValidaPeriodoTaxaGrupoTaxa(taxasOfertaBase, ofertaBase.SeqProcesso);

                var ofertasPeriodosTaxasOfertaBase = taxasOfertaBase.Where(opt => opt.Taxa.TipoCobranca != TipoCobranca.PorOferta).Select(opt => opt).ToList();

                if (ofertasPeriodosTaxasOfertaBase.Any())
                {

                    //2- A Configuração da etapa associada ao Grupo de ofertas informado possuir um Número máximo de ofertas
                    //   por inscrição diferente de "1"(nulo é diferente de "1")
                    //   

                    var seqsOfertas = new long[] { ofertaBase.Seq };

                    var specConfigEtapa = new ConfiguracaoEtapaPorOfertasSpecification(seqsOfertas);

                    var configEtapa = ConfiguracaoEtapaDomainService.SearchBySpecification(specConfigEtapa).FirstOrDefault();


                    if (configEtapa != null && configEtapa.NumeroMaximoOfertaPorInscricao != 1)
                    {
                        //3- Existir Período de taxa da oferta com o mesmo Tipo de taxa cadastrado em outra Oferta
                        //do(s) Grupo(s) de oferta da Configuração da etapa do Grupo de oferta da Oferta em questão,               

                        //Seleciona todas as ofertasPeridosTaxa das Ofertas dos Grupos de Ofertas com exceção das cadastradas na ofertaBase 
                        var ofertasPeriodoTaxaConfigEtapa = ConfiguracaoEtapaDomainService
                                         .SearchProjectionBySpecification(specConfigEtapa,
                                                 configEtp => configEtp.GruposOferta
                                                   .SelectMany(grpConfEtp => grpConfEtp.GrupoOferta.Ofertas)
                                                   .Where(oferta => oferta.Seq != ofertaBase.Seq)
                                                   .SelectMany(oferta => oferta.Taxas)
                                                   .Select(ofpt =>
                                                             new OfertaPeriodoTaxaVO
                                                             {
                                                                 Seq = ofpt.Seq,
                                                                 SeqTaxa = ofpt.SeqTaxa,
                                                                 SeqOferta = ofpt.SeqOferta,
                                                                 SeqEventoTaxa = ofpt.SeqEventoTaxa,
                                                                 SeqParametroCrei = ofpt.SeqParametroCrei,
                                                                 DataInicio = ofpt.DataInicio,
                                                                 DataFim = ofpt.DataFim,
                                                                 NumeroMinimo = ofpt.NumeroMinimo,
                                                                 NumeroMaximo = ofpt.NumeroMaximo,
                                                                 Taxa = ofpt.Taxa

                                                             }
                                                   )
                                         ).ToList();
                        //Período de taxa da oferta com o mesmo TipoTaxa do PeriodoBase cadastrado em outras Ofertas da ConfigEtapa 
                        var ofertasPeriodoTaxaTipoTaxaIgual = (from ofertaPeriodoTaxaConfigEtapa in ofertasPeriodoTaxaConfigEtapa.SelectMany(o => o).ToList()
                                      from ofertaPeridoTaxaOfertaBase in ofertasPeriodosTaxasOfertaBase
                                      where ofertaPeridoTaxaOfertaBase.Taxa.SeqTipoTaxa == ofertaPeriodoTaxaConfigEtapa.Taxa.SeqTipoTaxa
                                      select ofertaPeriodoTaxaConfigEtapa).Distinct().ToList();




                        if (ofertasPeriodoTaxaTipoTaxaIgual.Any())
                        {
                            //Monta uma lista com ofertasPeriodoTaxaCoincidentes entre os PeriodosOfertasBase e os PeriodosOfertas da ConfigEtapa
                            var ofertasPeriodoTaxaCoincidentes = OfertaPeriodoTaxaDomainService
                                                                 .SelecionaOfertasPeriodosTaxaCoincidentes(ofertasPeriodosTaxasOfertaBase,
                                                                  ofertasPeriodoTaxaTipoTaxaIgual);


                            if (ofertasPeriodoTaxaCoincidentes.Any())
                            {
                                //5- e com o Valor da taxa da Taxa do evento do GRA, ou o Número mínimo, ou o Número máximo,
                                //ou a Data de vencimento do título do Parâmetro CREI do GRA distinto(considerar que em Ofertas que não possuem o mesmo
                                //Tipo de taxa em período coincidente essas propriedades serão distintas das que possuem),
                                //abortar a operação e emitir a mensagem de erro:


                                //Busca dados do evento GRA
                                var seqEvento = EtapaProcessoDomainService
                                .SearchProjectionByKey(configEtapa.SeqEtapaProcesso, etapaProcesso => etapaProcesso.Processo.SeqEvento);
                                // Busca os eventos de taxa e parâmetros CREI para o evento informado
                                var eventoTaxaParametros = this.FinanceiroService.BuscarEnvetoTaxaEParamentroCREIPorSeqEvento(seqEvento.Value);

                                var periodoInvalido = BuscarOfertaPeriodoTaxaComDadosInvalidos(ofertasPeriodoTaxaCoincidentes, eventoTaxaParametros);

                                if (periodoInvalido != null)
                                    throw new OfertaPeriodoTaxaCobradaPorOfertaException(periodoInvalido.Taxa.TipoTaxa.Descricao, periodoInvalido.Taxa.TipoCobranca.SMCGetDescription().ToLower());

                            }
                        }
                    }
                }
            }
        }

        private OfertaPeriodoTaxaVO BuscarOfertaPeriodoTaxaComDadosInvalidos(List<OfertaPeriodoTaxaVO> lista, EventoComTaxaEParametrosCREIData dadosEvento)
        {
            if (dadosEvento == null || !lista.Any())
                return null;

            //var dadosEvento = FinanceiroService.BuscarEnvetoTaxaEParamentroCREIPorSeqEvento(seqEvento.Value);

            // Agrupa por tipo de taxa, para analisar cada grupo isoladamente
            var gruposPorTipoTaxa = lista.GroupBy(x => x.Taxa.SeqTipoTaxa);

            foreach (var grupo in gruposPorTipoTaxa)
            {
                var ofertasDoGrupo = grupo.ToList();

                // Validação 1: períodos consecutivos e coincidentes entre as ofertas
                if (!HaPeriodosConsecutivosCoincidentes(ofertasDoGrupo, dadosEvento))
                {
                    return ofertasDoGrupo.First(); // Retorna um qualquer do grupo inválido
                }

                // Validação 2: divergência direta entre elementos da mesma oferta
                var periodoTaxaInvalido = ofertasDoGrupo
                    .Where(ofertaPeriodoTaxa =>
                        ofertasDoGrupo.Any(outra =>
                            ofertaPeriodoTaxa.SeqOferta == outra.SeqOferta &&
                            !ReferenceEquals(ofertaPeriodoTaxa, outra) &&
                            TemDadosDivergentes(ofertaPeriodoTaxa, outra, dadosEvento)
                        )
                    ).FirstOrDefault();

                if (periodoTaxaInvalido != null)
                    return periodoTaxaInvalido;
            }

            return null;
        }

        private bool TemDadosDivergentes(OfertaPeriodoTaxaVO ofertaPeriodoTaxa1, OfertaPeriodoTaxaVO ofertaPeriodoTaxa2, EventoComTaxaEParametrosCREIData dadosEvento)
        {
            bool saoDivergentes = false;

            if (!PeriodosConsecutivos(ofertaPeriodoTaxa1, ofertaPeriodoTaxa2))
            {
                saoDivergentes = ofertaPeriodoTaxa1.SeqEventoTaxa != ofertaPeriodoTaxa2.SeqEventoTaxa ||
                                     ofertaPeriodoTaxa1.NumeroMinimo != ofertaPeriodoTaxa2.NumeroMinimo ||
                                     ofertaPeriodoTaxa1.NumeroMaximo != ofertaPeriodoTaxa2.NumeroMaximo ||
                                     !Nullable.Equals(ObterDataVencTituloParamCREI_GRA(dadosEvento, ofertaPeriodoTaxa1.SeqParametroCrei), ObterDataVencTituloParamCREI_GRA(dadosEvento, ofertaPeriodoTaxa2.SeqParametroCrei)) ||
                                     !Nullable.Equals(ObterValorTaxaEventoGRA(dadosEvento, ofertaPeriodoTaxa1.SeqEventoTaxa), ObterValorTaxaEventoGRA(dadosEvento, ofertaPeriodoTaxa2.SeqEventoTaxa));

            }
            else
            {
                // Se os períodos são consecutivos, não considerar divergência
                saoDivergentes = false;
            }

            return saoDivergentes;
        }

        
        private bool HaPeriodosConsecutivosCoincidentes(List<OfertaPeriodoTaxaVO> todasOfertasPeriodoTaxa, EventoComTaxaEParametrosCREIData dadosEvento)
        {
            // Agrupa por código da oferta
            var ofertasAgrupadas = todasOfertasPeriodoTaxa
                .GroupBy(x => x.SeqOferta)
                .Select(g => g.OrderBy(p => p.DataInicio).ToList())
                .ToList();

            if (ofertasAgrupadas.Count < 2)
                return true; // Nada para comparar ou apenas uma oferta, que se valida por si só se tiver períodos consecutivos

            // Primeiro, garantir que CADA oferta individualmente tenha seus períodos consecutivos.
            foreach (var ofertaAgrupada in ofertasAgrupadas)
            {
                if (ofertaAgrupada.Count > 1) // Só faz sentido verificar consecutividade se houver mais de um período
                {
                    for (int i = 0; i < ofertaAgrupada.Count - 1; i++)
                    {
                        if ((ofertaAgrupada[i].DataFim.AddDays(1) != ofertaAgrupada[i + 1].DataInicio) &&
                            (ofertaAgrupada[i].SeqTaxa == ofertaAgrupada[i + 1].SeqTaxa))
                        {
                            return false; // Períodos não consecutivos dentro da mesma oferta
                        }
                    }
                }
            }

            // Agora, para a comparação entre as ofertas.
            // A referência ainda é útil para ter um ponto de partida para a comparação de dados.
            var referencia = ofertasAgrupadas[0];

            foreach (var oferta in ofertasAgrupadas.Skip(1))
            {
                
                // Percorre os períodos até o menor número de períodos entre as duas ofertas para comparação de dados.
                // Isso garante que o loop de comparação for (int i = 0; i < limit; i++) só compare os períodos que realmente
                // têm um correspondente em ambas as listas.
                
                int limit = Math.Min(referencia.Count, oferta.Count);

                for (int i = 0; i < limit; i++)
                {
                    var periodoRef = referencia[i];
                    var periodoAtual = oferta[i];

                    // Verifica se os dados do período coincidem para os períodos que se "sobrepõem"
                    if (TemDadosDivergentes(periodoRef, periodoAtual, dadosEvento))
                        return false;
                }
                //Os períodos que existirem em uma lista, mas não na outra(se uma for mais longa), já terão tido sua
                //consecutividade interna verificada no primeiro loop. A ausência de um correspondente não implica em
                //divergência de dados para esta regra.
            }

            return true;
        }

        private bool PeriodosConsecutivos(OfertaPeriodoTaxaVO periodo, OfertaPeriodoTaxaVO periodoComparado) 
        {
            if ((periodo.DataFim < periodoComparado.DataInicio &&
                periodo.DataFim.AddDays(1) == periodoComparado.DataInicio) ||
                    (periodoComparado.DataFim < periodo.DataInicio &&
                periodoComparado.DataFim.AddDays(1) == periodo.DataInicio))
            {
                return true;
            }

            return false;
        }

       
        private DateTime? ObterDataVencTituloParamCREI_GRA(EventoComTaxaEParametrosCREIData dadosEvento, int seqParametroCrei)
        {
            return (dadosEvento.ParametrosCREI
                .FirstOrDefault(p => p.SeqParametroCREI == seqParametroCrei)?.DataVencimentoTitulo);
        }

        private decimal? ObterValorTaxaEventoGRA(EventoComTaxaEParametrosCREIData dadosEvento, int seqEventoTaxa)
        {
            return (dadosEvento.EventosTaxa
                .FirstOrDefault(e => e.SeqEventoTaxa == seqEventoTaxa)?.ValorTaxa);
        }

        private bool ValidarRegrasDevePossuirPeriodoTaxaIguais(OfertaPeriodoTaxa periodoTaxa, OfertaPeriodoTaxa taxaComparada, EventoComTaxaEParametrosCREIData eventoTaxaParametros)
        {

            var valorTaxa = eventoTaxaParametros.EventosTaxa
                .FirstOrDefault(x => x.SeqEventoTaxa == taxaComparada.SeqEventoTaxa);

            var dataVencimento = eventoTaxaParametros.ParametrosCREI
                .FirstOrDefault(f => f.SeqParametroCREI == taxaComparada.SeqParametroCrei);


            if (periodoTaxa.Taxa.Seq != taxaComparada.Taxa.Seq)
                return true;

            if (valorTaxa == null || dataVencimento == null)
                return true; // Se dados estiverem ausentes, não considerar erro

            return valorTaxa.ValorTaxa == eventoTaxaParametros.EventosTaxa.FirstOrDefault(x => x.SeqEventoTaxa == periodoTaxa.SeqEventoTaxa)?.ValorTaxa &&
                   taxaComparada.NumeroMinimo == periodoTaxa.NumeroMinimo &&
                   taxaComparada.NumeroMaximo == periodoTaxa.NumeroMaximo &&
                   dataVencimento.DataVencimentoTitulo == eventoTaxaParametros.ParametrosCREI.FirstOrDefault(f => f.SeqParametroCREI == periodoTaxa.SeqParametroCrei)?.DataVencimentoTitulo;
        }

        private void ValidarPermissoesForaPrazo(Oferta oferta)
        {
            // Verifica se o período da oferta coincide com o período de alguma permissão.
            if (PermissaoInscricaoForaPrazoDomainService.Count(new PermissaoInscricaoForaPrazoCoincidenteSpecification
            {
                SeqProcesso = oferta.SeqProcesso,
                DataInicio = oferta.DataInicio,
                DataFim = oferta.DataFim
            }) > 0)
            {
                throw new OfertaCoincideComPermissaoForaPrazoException();
            }
        }

        /// <summary>
        ///  Ao salvar, se o tipo de processo do processo em questão estiver configurado para aplicar a bolsa ex-aluno:
        ///  
        /// Se o campo “% Desconto” estiver preenchido e o campo “Nº vagas bolsa” não, abortar a operação e emitir
        /// a mensagem de erro: “Informe o número de vagas de bolsa.”
        /// 
        /// Se o campo “Nº Vagas bolsa” estiver preenchido e o campo “% Desconto” não, abortar a operação e emitir
        /// a mensagem de erro: “Informe o percentual de desconto”. 
        /// 
        /// Se o campo “Nº Vagas bolsa” for maior que "0" e o campo “% Desconto” for "0", abortar a operação e emitir
        /// a mensagem de erro: “O percentual de desconto deve ser maior que zero."
        /// 
        /// Se o “Nº Vagas bolsa” for maior que o “Nº Vagas Oferta”, abortar a operação e emitir a mensagem de erro:
        /// “O número de vagas de bolsa não pode ultrapassar o número de vagas da oferta.”
        /// 
        /// Se o "Nº vagas bolsa" for menor que a quantidade de inscrições que selecionaram a oferta em questão e que
        /// receberam bolsa, abortar a operação e emitir a mensagem de erro:
        /// "O número de vagas não pode ser menor que a quantidade de inscrições que receberam bolsa."
        /// </summary>
        /// <param name="oferta"></param>
        private void ValidarBolsaExAluno(Oferta oferta)
        {
            bool bolsaExAluno = TipoProcessoDomainService.BuscarTipoProcessoPorProcesso(oferta.SeqProcesso).BolsaExAluno;

            if (bolsaExAluno)
            {
                if (oferta.LimitePercentualDesconto.HasValue && !oferta.NumeroVagasBolsa.HasValue)
                {
                    throw new BonusExAlunoNumeroVagasNaoInformadaException();
                }

                if (oferta.NumeroVagasBolsa.HasValue && !oferta.LimitePercentualDesconto.HasValue)
                {
                    throw new BonusExAlunoPercentualDescontoNaoInformadoException();
                }

                if (oferta.NumeroVagasBolsa > 0 && oferta.LimitePercentualDesconto == 0)
                {
                    throw new BonusExAlunoDescontoZeroException();
                }

                // Como a cópia agora passará a ser feita sem copiar o número de vagas de bolsa, não há como ultrapassar o número de vagas da oferta.
                if (oferta.NumeroVagasBolsa > oferta.NumeroVagas)
                {
                    throw new BonusExAlunoNumeroVagasInvalidoException();
                }

                // Só realiza esta regra se for alteração de registro. Na inclusão não faz sentido!
                if (oferta.Seq > 0 &&
                    oferta.NumeroVagasBolsa < BuscarOfertaInscricoes(oferta.Seq).InscricoesConfirmadasReceberamBolsa)
                {
                    throw new BonusExAlunoNumeroVagasMenorBolsaSelecionadas();
                }
            }
        }

        private void ValidarPeriodoAtividade(Oferta oferta)
        {
            //Se o período da atividade não estiver contido no período do evento configurado no processo.
            var processo = ProcessoDomainService.SearchByKey(new SMCSeqSpecification<Processo>(oferta.SeqProcesso));

            if (oferta.DataInicioAtividade.HasValue &&
                oferta.DataFimAtividade.HasValue &&
                processo.DataInicioEvento.HasValue &&
                processo.DataFimEvento.HasValue)
            {
                if (!(oferta.DataInicioAtividade.Value.Date >= processo.DataInicioEvento.Value.Date &&
                      oferta.DataFimAtividade.Value.Date <= processo.DataFimEvento.Value.Date))
                {
                    throw new PeriodoAtividadeContidoPeriodoEnventoException();
                }
            }
        }

        /// <summary>
        /// Compara os códigos de autorização e as ofertas período taxa configuradas para duas ofertas
        /// Se forem exatamente iguais retorna false, caso contrário retorna true
        /// </summary>
        private bool CompararDadosTaxasCodigosOferta(Oferta ofertaBanco, Oferta oferta)
        {
            if (oferta.Taxas.Count != ofertaBanco.Taxas.Count) return true;
            if (oferta.CodigosAutorizacao.Count != ofertaBanco.CodigosAutorizacao.Count) return true;

            foreach (var taxa in ofertaBanco.Taxas)
            {
                var taxaNova = oferta.Taxas.FirstOrDefault(x => x.Seq == taxa.Seq);
                if (taxaNova == null) return true;
                if (taxaNova.SeqEventoTaxa != taxa.SeqEventoTaxa
                    || taxaNova.SeqParametroCrei != taxa.SeqParametroCrei
                    || taxaNova.SeqTaxa != taxa.SeqTaxa
                    || taxaNova.NumeroMinimo != taxa.NumeroMinimo
                    || taxaNova.NumeroMaximo != taxa.NumeroMaximo
                    || taxaNova.DataFim != taxa.DataFim
                    || taxaNova.DataInicio != taxa.DataInicio)
                {
                    return true;
                }
            }

            foreach (var codigo in ofertaBanco.CodigosAutorizacao)
            {
                var codigoNovo = oferta.CodigosAutorizacao.FirstOrDefault(x => x.Seq == codigo.Seq);
                if (codigoNovo == null) return true;
                if (codigoNovo.SeqCodigoAutorizacao != codigo.SeqCodigoAutorizacao
                    || codigoNovo.MaximoInscricoes != codigo.MaximoInscricoes)
                {
                    return true;
                }
            }

            return false;
        }

        public IEnumerable<OfertaTaxaVO> BuscarTaxasOferta(OfertaTaxaFilterSpecification filtro, out int total)
        {
            filtro.SetOrderBy(x => x.DescricaoCompleta);
            var ret = this.SearchProjectionBySpecification(filtro,
                x => new OfertaTaxaVO
                {
                    SeqOferta = x.Seq,
                    DataInicio = x.DataInicio,
                    DataFim = x.DataFim,
                    Taxas = x.Taxas
                        .Where(t => !filtro.SeqTipoTaxa.HasValue || filtro.SeqTipoTaxa.Value == t.Taxa.Seq)
                        .Select(
                            t => new TaxaPeriodoOfertaVO
                            {
                                SeqEventoTaxa = t.SeqEventoTaxa,
                                SeqParametroCrei = t.SeqParametroCrei,
                                TipoTaxa = t.Taxa.TipoTaxa.Descricao,
                                NumeroMinimo = t.NumeroMinimo,
                                NumeroMaximo = t.NumeroMaximo,
                                DataInicio = t.DataInicio,
                                DataFim = t.DataFim
                            }).OrderBy(i => i.TipoTaxa).ThenBy(o => o.DataFim).ToList()
                }, out total).ToList();
            foreach (var oferta in ret)
            {
                oferta.Descricao =
                    this.BuscarHierarquiaOfertaCompleta(oferta.SeqOferta, false).DescricaoCompleta;
                foreach (var taxa in oferta.Taxas)
                {
                    var eventoGRA = FinanceiroService.BuscarEventosTaxa(
                        new EventoTaxaFiltroData
                        {
                            SeqEventoTaxa = (int)taxa.SeqEventoTaxa
                        }).FirstOrDefault();
                    taxa.EventoTaxa = eventoGRA.Descricao;
                    taxa.Valor = eventoGRA.Valor;
                    var parametroCrei = FinanceiroService.BuscarParametrosCREI(
                        new ParametroCREIFiltroData
                        {
                            SeqParametroCREI = (int)taxa.SeqParametroCrei
                        }).FirstOrDefault();
                    taxa.VencimentoTitulo = parametroCrei.DataVencimentoTitulo.ToShortDateString() + " - " +
                        parametroCrei.SeqParametroCREI.ToString();
                }
            }
            return ret;
        }

        public List<SMCDatasourceItem<string>> BuscarPeriodosOfertas(long seqProcesso)
        {
            var spec = new OfertaFilterSpecification { SeqProcesso = seqProcesso };

            var dados = this.SearchProjectionBySpecification(spec,
                x => new
                {
                    DataInicio = x.DataInicio,
                    DataFim = x.DataFim
                }, true).OrderBy(o => o.DataInicio).ThenBy(o => o.DataFim);

            List<SMCDatasourceItem<string>> ret = new List<SMCDatasourceItem<string>>();
            foreach (var item in dados)
            {
                if (item.DataInicio.HasValue && item.DataFim.HasValue)
                {
                    var datainicio = item.DataInicio.Value.Day.ToString("00") + "/" + item.DataInicio.Value.Month.ToString("00") + "/" + item.DataInicio.Value.Year;
                    var datafim = item.DataFim.Value.Day.ToString("00") + "/" + item.DataFim.Value.Month.ToString("00") + "/" + item.DataFim.Value.Year;

                    ret.Add(new SMCDatasourceItem<string>
                    {
                        Seq = string.Format("{0}-{1}", datainicio, datafim),
                        Descricao = string.Format("{0} a {1}", datainicio, datafim)
                    });
                }
            }
            return ret;
        }

        public void IncluirTaxasLote(IncluirTaxaEmLoteVO modelo)
        {
            // Verifica se a situação da etapa de inscrição permite a alteração de taxas. 
            var seqProcesso = this.SearchProjectionByKey(
                new SMCSeqSpecification<Oferta>(modelo.Ofertas[0].Seq),
                x => x.SeqProcesso
            );
            var specEtapa = new EtapaProcessoFilterSpecification(seqProcesso)
            {
                Token = TOKENS.ETAPA_INSCRICAO
            };
            var situacaoEtapaInscricao = this.EtapaProcessoDomainService.SearchProjectionBySpecification(
                specEtapa, x => x.SituacaoEtapa
            );
            if (situacaoEtapaInscricao != null &&
                situacaoEtapaInscricao.Any() &&
                situacaoEtapaInscricao.FirstOrDefault() == SituacaoEtapa.Liberada)
            {
                throw new InclusaoPeriodoTaxaEtapaLiberadaExcetpion();
            }

            using (var unitOfWork = SMCUnitOfWork.Begin())
            {
                try
                {
                    foreach (var seqOferta in modelo.Ofertas.Select(x => x.Seq).ToArray())
                    {
                        // Busca a oferta no banco
                        var specOf = new OfertaFilterSpecification() { SeqOferta = seqOferta };
                        var oferta = this.SearchByKey(specOf, x => x.Telefones, x => x.EnderecosEletronicos, x => x.Taxas);

                        // Inclui a(s) nova(s) taxa(s)
                        foreach (var taxa in modelo.Taxas)
                        {
                            var novaTaxa = taxa.SMCClone();
                            novaTaxa.SeqOferta = oferta.Seq;
                            oferta.Taxas.Add(novaTaxa);
                        }

                        // Salva a oferta
                        SalvarOferta(oferta);
                    }

                    unitOfWork.Commit();
                }
                catch (Exception)
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }


        /// <summary>
        /// Busca o sdetalhes da oferta e a quantidade de inscrições confirmadas para a oferta
        /// Inicialmente usado na tela de prorrogação de etapa do processo
        /// </summary>
        public IEnumerable<OfertaPeriodoInscricaoVO> BuscarOfertasInscrioes(long[] seqOfertas)
        {
            var spec = new SMCContainsSpecification<Oferta, long>(x => x.Seq, seqOfertas);
            return SearchProjectionBySpecification(spec,
                oferta => new OfertaPeriodoInscricaoVO
                {
                    SeqProcesso = oferta.SeqProcesso,
                    SeqOferta = oferta.Seq,
                    ExigeEntregaDocumentaaco = oferta.GrupoOferta.ConfiguracoesEtapa
                        .Count(x => x.ConfiguracaoEtapa.DocumentosRequeridos
                            .Any(d => d.Obrigatorio) || x.ConfiguracaoEtapa.GruposDocumentoRequerido.Any()) > 0,
                    ExigePagamentoTaxa = oferta.ExigePagamentoTaxa,
                    DataFim = oferta.DataFim,
                    DataInicio = oferta.DataInicio,
                    Vagas = oferta.NumeroVagas,
                    Descricao = oferta.DescricaoCompleta,
                    InscricoesConfirmadas = oferta.InscricoesOferta.Count(i => i.NumeroOpcao == 1
                     && i.Inscricao.HistoricosSituacao.Any(s =>
                         s.TipoProcessoSituacao.Token == TOKENS.SITUACAO_INSCRICAO_CONFIRMADA
                            && !s.Inscricao.HistoricosSituacao.Any(t => t.Atual
                                && t.TipoProcessoSituacao.Token == TOKENS.SITUACAO_INSCRICAO_CANCELADA)))
                }
            ).ToList();
        }

        public List<OfertaPeriodoInscricaoVO> BuscarContagemInscrioesDasOfertasPorSituacao(InscricaoOfertaPorSituacaoVO vo)
        {
            var spec = new SMCContainsSpecification<Oferta, long>(x => x.Seq, vo.SeqOfertas);
            return SearchProjectionBySpecification(spec,
                oferta => new OfertaPeriodoInscricaoVO
                {
                    Vagas = oferta.NumeroVagas,
                    Descricao = oferta.DescricaoCompleta,
                    InscricoesConfirmadas = oferta.InscricoesOferta.Count(i => i.HistoricosSituacao.Any(f => f.Atual && vo.Tokens.Contains(f.TipoProcessoSituacao.Token)))
                }
            ).ToList();
        }

        /// <summary>
        /// Busca o sdetalhes da oferta e a quantidade de inscrições confirmadas para a oferta
        /// Inicialmente usado na tela de prorrogação de etapa do processo
        /// </summary>
        public OfertaPeriodoInscricaoVO BuscarOfertaInscricoes(long seqOferta)
        {
            var spec = new SMCSeqSpecification<Oferta>(seqOferta);
            var oferta = this.SearchByDepth(spec, 10, x => x.HierarquiaOfertaPai).First();
            List<OfertaPeriodoInscricaoVO> ret = new List<OfertaPeriodoInscricaoVO>();

            var vo = new OfertaPeriodoInscricaoVO
            {
                SeqProcesso = oferta.SeqProcesso,
                SeqOferta = oferta.Seq,
                Descricao = oferta.DescricaoCompleta,
                ExigeEntregaDocumentaaco = this.SearchProjectionByKey(spec, o => o.GrupoOferta.ConfiguracoesEtapa
                    .Count(x => x.ConfiguracaoEtapa.DocumentosRequeridos
                        .Any(d => d.Obrigatorio) || x.ConfiguracaoEtapa.GruposDocumentoRequerido.Any()) > 0),
                ExigePagamentoTaxa = oferta.ExigePagamentoTaxa,
                DataFim = oferta.DataFim,
                DataInicio = oferta.DataInicio,
                Vagas = oferta.NumeroVagas,
                InscricoesConfirmadas = this.SearchProjectionByKey(spec, o => o.InscricoesOferta.Count(i => i.NumeroOpcao == 1
                 && i.Inscricao.HistoricosSituacao.Any(s =>
                     s.TipoProcessoSituacao.Token == TOKENS.SITUACAO_INSCRICAO_CONFIRMADA
                        && !s.Inscricao.HistoricosSituacao.Any(t => t.Atual
                            && t.TipoProcessoSituacao.Token == TOKENS.SITUACAO_INSCRICAO_CANCELADA)))),
                InscricoesConfirmadasReceberamBolsa = this.SearchProjectionByKey(spec, o => o.InscricoesOferta.Count(i => i.NumeroOpcao == 1
                  && i.Inscricao.RecebeuBolsa && i.Inscricao.HistoricosSituacao.Any(s =>
                      s.TipoProcessoSituacao.Token == TOKENS.SITUACAO_INSCRICAO_CONFIRMADA
                      && !s.Inscricao.HistoricosSituacao.Any(t => t.Atual
                          && t.TipoProcessoSituacao.Token == TOKENS.SITUACAO_INSCRICAO_CANCELADA))))
            };

            return vo;
        }

        /// <summary>
        /// Buscar a lista de ofertas a serem consolidadas no checkin
        /// </summary>
        /// <param name="filtro">Parametros dos filtros</param>
        /// <returns>List de ofertas</returns>
        public List<AcompanhamentoInscritoCheckinListaVO> BuscarPosicaoConsolidadaCheckin(PosicaoConsolidadaCheckinFilterSpecification filtro, out int total)
        {
            List<AcompanhamentoInscritoCheckinListaVO> retorno = new List<AcompanhamentoInscritoCheckinListaVO>();

            var processo = this.ProcessoDomainService.SearchProjectionByKey(filtro.SeqProcesso.Value, p => new { p.SeqTemplateProcessoSGF, p.Descricao });
            var situacoesTemplateProcesso = TemplateProcessoService.BuscarSituacoesPorTemplateProcesso(processo.SeqTemplateProcessoSGF);
            var tokenSituacao = situacoesTemplateProcesso.Contains(TOKENS.SITUACAO_INSCRICAO_DEFERIDA) ? TOKENS.SITUACAO_INSCRICAO_DEFERIDA : TOKENS.SITUACAO_INSCRICAO_CONFIRMADA;

            filtro.Token = tokenSituacao;

            filtro.SetOrderBy(sort => sort.DescricaoCompleta);

            var result = SearchBySpecification(filtro, out total, IncludesOferta.InscricoesOferta_Inscricao_HistoricosSituacao_TipoProcessoSituacao).ToList();

            foreach (var item in result)
            {

                retorno.Add(new AcompanhamentoInscritoCheckinListaVO()
                {
                    Seq = item.Seq,
                    DescricaoProcesso = processo.Descricao,
                    SeqProcesso = item.SeqProcesso,
                    NumeroInscrito = item.InscricoesOferta.Count(c => c.Inscricao.HistoricosSituacao.Any(a => a.Atual && a.TipoProcessoSituacao.Token == tokenSituacao)),
                    DescricaoOferta = this.BuscarHierarquiaOfertaCompleta(item.Seq, false).DescricaoCompleta,
                    NumeroChecinRealizado = item.InscricoesOferta.Count(c => c.Inscricao.HistoricosSituacao.Any(a => a.Atual && a.TipoProcessoSituacao.Token == tokenSituacao && c.DataCheckin.HasValue)),
                    NumeroVagasOferta = item.NumeroVagas

                });

            }

            retorno.ForEach(f => f.RestanteVagas = f.NumeroInscrito - f.NumeroChecinRealizado);

            // total = retorno.Count;

            return retorno;
        }

        public List<Oferta> BuscarOfertasProcesso(long seqProcesso)
        {
            var spec = new OfertaFilterSpecification()
            {
                SeqProcesso = seqProcesso
            };

            var lista = this.SearchProjectionBySpecification(spec, x => x).ToList();

            return lista;
        }
    }
}