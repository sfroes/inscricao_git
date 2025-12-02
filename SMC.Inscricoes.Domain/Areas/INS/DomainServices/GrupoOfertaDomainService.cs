using SMC.Framework;
using SMC.Framework.Model;
using SMC.Framework.Specification;
using SMC.Framework.UnitOfWork;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using SMC.Inscricoes.Common.Areas.INS.Exceptions;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SMC.Inscricoes.Domain.Areas.INS.DomainServices
{
    public class GrupoOfertaDomainService : InscricaoContextDomain<GrupoOferta>
    {
        #region DomainServices

        private InscricaoOfertaDomainService InscricaoOfertaDomainService
        {
            get { return this.Create<InscricaoOfertaDomainService>(); }
        }

        private InscricaoDomainService InscricaoDomainService
        {
            get { return this.Create<InscricaoDomainService>(); }
        }

        private OfertaDomainService OfertaDomainService
        {
            get { return this.Create<OfertaDomainService>(); }
        }

        private EtapaProcessoDomainService EtapaProcessoDomainService
        {
            get { return this.Create<EtapaProcessoDomainService>(); }
        }

        #endregion DomainServices

        public IEnumerable<SMCDatasourceItem> BuscarGruposOfertaKeyValue(GrupoOfertaFilterSpecification filtro)
        {
            return this.SearchProjectionBySpecification(filtro, x => new SMCDatasourceItem { Seq = x.Seq, Descricao = x.Nome });
        }

        public long Salvar(GrupoOferta grupoOferta)
        {
            var etapaInscricao = EtapaProcessoDomainService.SearchBySpecification(new EtapaProcessoFilterSpecification(grupoOferta.SeqProcesso)
            {
                Token = TOKENS.ETAPA_INSCRICAO
            }).ToList();

            if (etapaInscricao != null && etapaInscricao.Count > 0 && etapaInscricao.First().SituacaoEtapa == SituacaoEtapa.Liberada)
            {
                throw new AlteracaoGrupoOfertaEtapaLiberadaException();
            }

            IEnumerable<long> ofertasDesassociadas = null;

            var ofertaSeqs = grupoOferta.Ofertas.Select(f => f.Seq);

            var ofertas = OfertaDomainService.SearchBySpecification(
                                new SMCContainsSpecification<Oferta, long>(x => x.Seq, ofertaSeqs.ToArray()),
                                IncludesOferta.InscricoesOferta).ToList();

            if (grupoOferta.Seq != 0)
            {
                //RN_INS_077
                var spec = new SMCSeqSpecification<GrupoOferta>(grupoOferta.Seq);
                var grupoOfertaBanco = this.SearchByKey(spec, IncludesGrupoOferta.ConfiguracoesEtapa |
                                                              IncludesGrupoOferta.ConfiguracoesEtapa_ConfiguracaoEtapa |
                                                              IncludesGrupoOferta.Ofertas |
                                                              IncludesGrupoOferta.Ofertas_InscricoesOferta |
                                                              IncludesGrupoOferta.Processo |
                                                              IncludesGrupoOferta.Processo_EtapasProcesso |
                                                              IncludesGrupoOferta.InscricoesGrupoOferta);

                foreach (var configuracaoEtapa in grupoOfertaBanco.ConfiguracoesEtapa)
                {
                    foreach (var oferta in ofertas)
                    {
                        if (oferta.DataInicio < configuracaoEtapa.ConfiguracaoEtapa.DataInicio ||
                            oferta.DataFim > configuracaoEtapa.ConfiguracaoEtapa.DataFim)
                        {
                            throw new AssociacaoOfertaGrupoException(oferta.Nome);
                        }
                    }
                }

                //RN_INS_081
                grupoOferta.Ofertas.SMCContainsList(grupoOfertaBanco.Ofertas, f => f.Seq, out ofertasDesassociadas);
            }

            ////RN_INS_078
            using (var unitOfWork = SMCUnitOfWork.Begin())
            {
                try
                {
                    this.SaveEntity(grupoOferta);

                    foreach (var oferta in ofertas)
                    {
                        //RN_INS_081
                        if (oferta.SeqGrupoOferta != grupoOferta.Seq)
                        {
                            if (oferta.InscricoesOferta != null && oferta.InscricoesOferta.Count > 0)
                            {
                                throw new DesassociacaoOfertaComInscricaoException(oferta.Nome);
                            }
                        }

                        //Modifica a FK das ofertas
                        oferta.SeqGrupoOferta = grupoOferta.Seq;
                        this.OfertaDomainService.SaveEntity(oferta);
                    }

                    if (ofertasDesassociadas != null)
                    {
                        //Desassocia todas as ofertas desmarcadas
                        /*var ofertasRemovidas = OfertaDomainService.SearchBySpecification(
                                    new SMCContainsSpecification<Oferta, long>(x => x.Seq, ofertasDesassociadas.ToArray()),
                                    IncludesOferta.InscricoesOferta).ToList();
*/
                        var ofertasRemovidas = OfertaDomainService.SearchProjectionBySpecification(new SMCContainsSpecification<Oferta, long>(x => x.Seq, ofertasDesassociadas.ToArray()), x => new
                        {
                            Seq = x.Seq,
                            TemInscricoes = x.InscricoesOferta.Any(),
                            Nome = x.Nome
                        }).ToList();

                        foreach (var oferta in ofertasRemovidas)
                        {
                            //RN_INS_081
                            if (oferta.TemInscricoes)
                                throw new DesassociacaoOfertaComInscricaoException(oferta.Nome);

                            OfertaDomainService.UpdateFields(new Oferta { Seq = oferta.Seq, SeqGrupoOferta = null }, x => x.SeqGrupoOferta);

                        }
                    }

                    unitOfWork.Commit();
                    return grupoOferta.Seq;
                }
                catch (Exception)
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Retorna o Guid do processo com base no seq do grupo de oferta
        /// </summary>
        /// <param name="seqGrupoOferta"></param>
        /// <returns></returns>
        public Guid BuscarSeqProcessoPorSeqGrupoOferta(long seqGrupoOferta)
        {
            var spec = new GrupoOfertaFilterSpecification()
            {
                Seq = seqGrupoOferta
            };
               var resultado = this.SearchProjectionByKey(spec, s => new { Seq = s.SeqProcesso, Uid = s.Processo.UidProcesso });

            return resultado.Uid;
        }

        public List<GrupoOferta> BuscarOfertaSeqsGrupoOferta(List<long> SeqGrupoOferta)
        {

           var spec = new SMCContainsSpecification<GrupoOferta, long>(x => x.Seq, SeqGrupoOferta.ToArray());


            var gruposOfertas =  this.SearchBySpecification(spec, IncludesGrupoOferta.Ofertas| IncludesGrupoOferta.Ofertas | IncludesGrupoOferta.Ofertas_Taxas| IncludesGrupoOferta.Ofertas_Taxas_Taxa).ToList();

            return gruposOfertas;

        }
    }
}