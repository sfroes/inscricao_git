using SMC.Framework;
using SMC.Framework.Domain;
using SMC.Framework.Extensions;
using SMC.Framework.Specification;
using SMC.Framework.UnitOfWork;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using SMC.Inscricoes.Common.Areas.INS.Exceptions;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using SMC.Inscricoes.Domain.Areas.INS.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SMC.Inscricoes.Domain.Areas.INS.DomainServices
{
    public class HierarquiaOfertaDomainService : InscricaoContextDomain<HierarquiaOferta>
    {
        #region Domain Services

        private TipoHierarquiaOfertaDomainService TipoHierarquiaOfertaDomainService
        {
            get { return this.Create<TipoHierarquiaOfertaDomainService>(); }
        }

        private ProcessoDomainService ProcessoDomainService
        {
            get { return this.Create<ProcessoDomainService>(); }
        }

        private GrupoOfertaDomainService GrupoOfertaDomainService
        {
            get { return this.Create<GrupoOfertaDomainService>(); }
        }

        private OfertaDomainService OfertaDomainService
        {
            get { return this.Create<OfertaDomainService>(); }
        }

        private ItemHierarquiaOfertaDomainService ItemHierarquiaOfertaDomainService
        {
            get { return this.Create<ItemHierarquiaOfertaDomainService>(); }
        }

        #endregion Domain Services

        public List<HierarquiaOferta> BuscarHierarquiaOfertaCompleta(long seqHierarquiaOferta)
        {

            var lista = new List<HierarquiaOferta>();

            Recursao(seqHierarquiaOferta);

            void Recursao(long seqHierarquia)
            {
                var hierarquia = this.SearchByKey(seqHierarquia);
                lista.Add(hierarquia);

                if (hierarquia.SeqPai.HasValue)
                {
                    Recursao(hierarquia.SeqPai.Value);
                }
            }

            return lista;
        }

        /// <summary>
        /// Retorna a árove de hierarquia de ofertas de um processo para exibição
        /// </summary>
        public List<HierarquiaOfertaVO> BuscarArvoreHierarquiaOfertaProcesso(long seqProcesso, long? seqPai, long[] expandedNodes)
        {
            var processo = ProcessoDomainService.SearchProjectionByKey(new SMCSeqSpecification<Processo>(seqProcesso), p => new
            {
                p.ExibeArvoreFechada,
                ItemsTipoHierarquia = p.TipoHierarquiaOferta.Itens,
                ExibirPeriodoAtividadeOferta = p.ExibirPeriodoAtividadeOferta
            });

            var spec = new HierarquiaOfertaFilterSpecification { SeqProcesso = seqProcesso, SeqHierarquiaPai = processo.ExibeArvoreFechada ? seqPai : null };
            if (processo.ExibeArvoreFechada && !seqPai.HasValue)
                spec.ApenasNosPai = true;

            var listaHierarquias = this.SearchBySpecification(spec).ToList();

            // Carrega os itens que já vieram abertos.
            if (expandedNodes != null)
            {
                var expSpec = new SMCContainsSpecification<HierarquiaOferta, long?>(f => f.SeqPai, expandedNodes.Cast<long?>().ToArray());
                expSpec.SetOrderBy(o => o.Nome);
                var nodes = SearchBySpecification(expSpec);
                listaHierarquias.AddRange(nodes);
            }

            var result = new List<HierarquiaOfertaVO>();
            foreach (var item in listaHierarquias)
            {
                var vo = item.Transform<HierarquiaOfertaVO>();
                var itemHierarquiaOferta = processo.ItemsTipoHierarquia.FirstOrDefault(x => x.Seq == item.SeqItemHierarquiaOferta);
                if (processo.ItemsTipoHierarquia.Any(x => x.SeqPai.HasValue && x.SeqPai == itemHierarquiaOferta.Seq && x.HabilitaCadastroOferta))
                {
                    vo.PermiteCadastroOfertaFilha = true;
                }

                if (processo.ItemsTipoHierarquia.Any(x => x.SeqPai.HasValue && x.SeqPai == itemHierarquiaOferta.Seq && !x.HabilitaCadastroOferta))
                {
                    vo.PermiteCadastroItemFilho = true;
                }

                if (processo.ItemsTipoHierarquia.Any(x => x.SeqPai.HasValue && x.SeqPai.Value == itemHierarquiaOferta.Seq &&
                    processo.ItemsTipoHierarquia.Any(i => i.SeqPai.HasValue && i.SeqPai.Value == x.Seq)))
                {
                    vo.PermiteCadastroNetos = true;
                }

                if (item.EOferta)
                {
                    var oferta = item as Oferta;

                    OfertaDomainService.AdicionarDescricaoCompleta(oferta, processo.ExibirPeriodoAtividadeOferta);

                    //Sempre que exibir a árvore, tem que mostrar somente a descrição da oferta, no nó da oferta e não o caminho completo
                    if (!processo.ExibirPeriodoAtividadeOferta.HasValue || processo.ExibirPeriodoAtividadeOferta.HasValue && !processo.ExibirPeriodoAtividadeOferta.Value)
                    {
                        oferta.DescricaoCompleta = oferta.Nome;
                    }

                    vo.Descricao = oferta.DescricaoCompleta;
                    vo.Cancelada = oferta.Cancelada;
                    vo.Desativada = !oferta.Ativo;
                }

                // Se estiver buscando itens "filhos", limpa o seqPai para evitar erro ao formar os galhos da árvore.
                if (seqPai.HasValue)
                    vo.SeqPai = null;


                result.Add(vo);
            }
            return result;
        }

        /// <summary>
        /// Retorna a árove de hierarquia de ofertas de um processo para exibição, informando as ofertas que possuem grupos
        /// </summary>
        public List<HierarquiaOfertaVO> BuscarArvoreHierarquiaOfertaGrupoOferta(long seqProcesso, long? seqGrupoOferta)
        {
            /*var spec = new HierarquiaOfertaFilterSpecification { SeqProcesso = seqProcesso };
            var listaHierarquias = this.SearchBySpecification(spec);

            return BuscarArvoreHierarquiaOfertaGrupoOferta(seqGrupoOferta, listaHierarquias);*/

            var spec = new HierarquiaOfertaFilterSpecification { SeqProcesso = seqProcesso };
            var listaHierarquiasVO = this.SearchProjectionBySpecification(spec, x => new HierarquiaOfertaVO
            {
                Desativada = false,
                Descricao = x.Nome,
                DescricaoCompleta = x.DescricaoCompleta,
                PermiteCadastroItemFilho = false,//
                PermiteCadastroNetos = false,//
                PermiteCadastroOfertaFilha = false,//
                Seq = x.Seq,
                SeqItemHierarquiaOferta = x.SeqItemHierarquiaOferta,
                SeqPai = x.SeqPai,
                SeqProcesso = x.SeqProcesso,

                ProcessoExibirPeriodoAtividadeOferta = (x as Oferta).Processo.ExibirPeriodoAtividadeOferta,
                DataInicioAtividade = (x as Oferta).DataInicioAtividade,
                DataFimAtividade = (x as Oferta).DataFimAtividade,
                CargaHorariaAtividade = (x as Oferta).CargaHorariaAtividade,
                EOferta = (x is Oferta),
                GrupoEmConfiguracao = (x as Oferta).GrupoOferta.ConfiguracoesEtapa.Any(),//
                NomeGrupoOferta = (x as Oferta).GrupoOferta.Nome,
                SeqGrupoOferta = (x as Oferta).SeqGrupoOferta,
                Cancelada = (x as Oferta).DataCancelamento.HasValue && (x as Oferta).DataCancelamento.Value <= DateTime.Now,//
            }).ToList();

            foreach (var oferta in listaHierarquiasVO.Where(h => h.EOferta))
            {
                var of = new Oferta()
                {
                    DataInicioAtividade = oferta.DataInicioAtividade,
                    DataFimAtividade = oferta.DataFimAtividade,
                    DescricaoCompleta = oferta.Descricao,
                    Nome = oferta.Descricao,
                    CargaHorariaAtividade = oferta.CargaHorariaAtividade

                };

                OfertaDomainService.AdicionarDescricaoCompleta(of, oferta.ProcessoExibirPeriodoAtividadeOferta);

                if (!string.IsNullOrEmpty(of.DescricaoCompleta))
                    oferta.Descricao = of.DescricaoCompleta;

                oferta.PossuiGrupo = (!seqGrupoOferta.HasValue && oferta.SeqGrupoOferta.HasValue) || (oferta.SeqGrupoOferta.HasValue && seqGrupoOferta.HasValue && seqGrupoOferta.Value != oferta.SeqGrupoOferta.Value);
            }

            return listaHierarquiasVO;
        }

        public List<HierarquiaOfertaVO> BuscarHieraquiaLeituraQRCode()
        {
            /*var spec = new HierarquiaOfertaFilterSpecification { SeqProcesso = seqProcesso };
            var listaHierarquias = this.SearchBySpecification(spec);

            return BuscarArvoreHierarquiaOfertaGrupoOferta(seqGrupoOferta, listaHierarquias);*/

            var spec = new HierarquiaOfertaFilterSpecification { ProcessoGestaoEvento = true };
            var listaHierarquiasVO = this.SearchProjectionBySpecification(spec, x => new HierarquiaOfertaVO
            {
                //Desativada = false,
                Descricao = x.Nome,
                DescricaoCompleta = x.DescricaoCompleta,
                //PermiteCadastroItemFilho = false,//
                //PermiteCadastroNetos = false,//
                //PermiteCadastroOfertaFilha = false,//
                Seq = x.Seq,
                SeqItemHierarquiaOferta = x.SeqItemHierarquiaOferta,
                SeqPai = x.SeqPai,
                SeqProcesso = x.SeqProcesso,

                //ProcessoExibirPeriodoAtividadeOferta = (x as Oferta).Processo.ExibirPeriodoAtividadeOferta,
                DataInicioAtividade = (x as Oferta).DataInicioAtividade,
                DataFimAtividade = (x as Oferta).DataFimAtividade,
                CargaHorariaAtividade = (x as Oferta).CargaHorariaAtividade,
                EOferta = (x is Oferta),
                HoraAberturaCheckin = x.Processo.HoraAberturaCheckin
                //GrupoEmConfiguracao = (x as Oferta).GrupoOferta.ConfiguracoesEtapa.Any(),//
                //NomeGrupoOferta = (x as Oferta).GrupoOferta.Nome,
                //SeqGrupoOferta = (x as Oferta).SeqGrupoOferta,
                //Cancelada = (x as Oferta).DataCancelamento.HasValue && (x as Oferta).DataCancelamento.Value <= DateTime.Now,//

            }).ToList();

            var retorno = new List<HierarquiaOfertaVO>();
            var dataEventoAtual = new DateTime(2024, 3, 18, 13, 0, 0);

            foreach (var oferta in listaHierarquiasVO.Where(h => h.EOferta))
            {
                if (!oferta.DataInicioAtividade.HasValue || !oferta.DataFimAtividade.HasValue)
                {
                    continue;
                }

                if (oferta.DataInicioAtividade.Value.AddHours(-oferta.HoraAberturaCheckin.Value.TotalHours) <= dataEventoAtual && oferta.DataFimAtividade >= dataEventoAtual)
                {
                    var of = new Oferta()
                    {
                        DataInicioAtividade = oferta.DataInicioAtividade,
                        DataFimAtividade = oferta.DataFimAtividade,
                        DescricaoCompleta = oferta.DescricaoCompleta,
                        Nome = oferta.Descricao,
                        CargaHorariaAtividade = oferta.CargaHorariaAtividade

                    };

                    OfertaDomainService.AdicionarDescricaoCompleta(of, oferta.ProcessoExibirPeriodoAtividadeOferta);

                    if (!string.IsNullOrEmpty(of.DescricaoCompleta))
                        oferta.Descricao = of.DescricaoCompleta;

                    //oferta.PossuiGrupo = (!seqGrupoOferta.HasValue && oferta.SeqGrupoOferta.HasValue) || (oferta.SeqGrupoOferta.HasValue && seqGrupoOferta.HasValue && seqGrupoOferta.Value != oferta.SeqGrupoOferta.Value);
                    retorno.Add(oferta);
                }
            }

            return retorno;
        }

        /// <summary>
        /// Retorna a árove de hierarquia de ofertas de um processo para exibição, informando as ofertas que possuem grupos
        /// </summary>
        public List<HierarquiaOfertaVO> BuscarArvoreHierarquiaOfertaGrupoOferta(long? seqGrupoOferta, long[] seqOfertas)
        {
            var spec = new SMCContainsSpecification<HierarquiaOferta, long>(x => x.Seq, seqOfertas);
            var listaHierarquiasVO = this.SearchProjectionByDepth(spec, x => new HierarquiaOfertaVO
            {
                Desativada = false,
                Descricao = x.Nome,
                DescricaoCompleta = x.DescricaoCompleta,
                PermiteCadastroItemFilho = false,//
                PermiteCadastroNetos = false,//
                PermiteCadastroOfertaFilha = false,//
                Seq = x.Seq,
                SeqItemHierarquiaOferta = x.SeqItemHierarquiaOferta,
                SeqPai = x.SeqPai,
                SeqProcesso = x.SeqProcesso,

                EOferta = (x is Oferta),
                GrupoEmConfiguracao = (x as Oferta).GrupoOferta.ConfiguracoesEtapa.Any(),//
                NomeGrupoOferta = (x as Oferta).GrupoOferta.Nome,
                SeqGrupoOferta = (x as Oferta).SeqGrupoOferta,
                Cancelada = (x as Oferta).DataCancelamento.HasValue && (x as Oferta).DataCancelamento.Value <= DateTime.Now,//
            }, 10, false, x => x.HierarquiaOfertaPai).ToList();

            foreach (var oferta in listaHierarquiasVO.Where(h => h.EOferta))
                oferta.PossuiGrupo = (!seqGrupoOferta.HasValue && oferta.SeqGrupoOferta.HasValue) || (oferta.SeqGrupoOferta.HasValue && seqGrupoOferta.HasValue && seqGrupoOferta.Value != oferta.SeqGrupoOferta.Value);

            return listaHierarquiasVO;

            //return BuscarArvoreHierarquiaOfertaGrupoOferta(seqGrupoOferta, listaHierarquias);
        }

        /* private List<HierarquiaOfertaVO> BuscarArvoreHierarquiaOfertaGrupoOferta(long? seqGrupoOferta, IEnumerable<HierarquiaOferta> listaHierarquias)
         {
             var listahierarquiasVO = new List<HierarquiaOfertaVO>();

             foreach (var item in listaHierarquias)
             {
                 var itemVO = item.Transform<HierarquiaOfertaVO>();

                 //Se for uma oferta
                 if (item.EOferta)
                 {
                     var oferta = item as Oferta;

                     listahierarquiasVO.Add(itemVO);

                     if ((!seqGrupoOferta.HasValue && oferta.SeqGrupoOferta.HasValue)
                         || (oferta.SeqGrupoOferta.HasValue && seqGrupoOferta.HasValue && seqGrupoOferta.Value != oferta.SeqGrupoOferta.Value))
                     {
                         itemVO.PossuiGrupo = true;
                     }

                     if (oferta.SeqGrupoOferta.HasValue)
                     {
                         itemVO.GrupoEmConfiguracao = this.GrupoOfertaDomainService.SearchProjectionByKey(
                             new SMCSeqSpecification<GrupoOferta>(oferta.SeqGrupoOferta.Value), x => x.ConfiguracoesEtapa.Any());
                         itemVO.NomeGrupoOferta = GrupoOfertaDomainService.SearchProjectionByKey(
                             new SMCSeqSpecification<GrupoOferta>(oferta.SeqGrupoOferta.Value), x => x.Nome);
                     }
                 }
                 else
                 {
                     listahierarquiasVO.Add(itemVO);
                 }
             }
             return listahierarquiasVO;
         }*/

        /// <summary>
        /// Salva uma hierarquia recursivamente de oferta e realiza as validações necessárias
        /// </summary>
        public long SalvarHierarquiaOferta(HierarquiaOferta hierarquiaOferta)
        {
            var itemOld = new HierarquiaOferta();
            if (hierarquiaOferta.Seq != 0)
            {
                //Implementação da RN_INS_041
                itemOld = this.SearchByKey(new SMCSeqSpecification<HierarquiaOferta>(hierarquiaOferta.Seq), x => x.HierarquiasOfertaFilhas);
                if (itemOld.SeqItemHierarquiaOferta != hierarquiaOferta.SeqItemHierarquiaOferta
                    && itemOld.HierarquiasOfertaFilhas != null && itemOld.HierarquiasOfertaFilhas.Count > 0)
                {
                    throw new HierquiaOfertaAlteradaInvalidaException();
                }
            }

            // Ajusta o valor da descrição completa para a hierarquia
            GerarCamposHierarquia(hierarquiaOferta);

            // Início da transação
            using (var unitOfWork = SMCUnitOfWork.Begin())
            {
                this.SaveEntity(hierarquiaOferta);
                //Método recursivo
                AtualizarCamposHierarquiasOfertasFilhas(itemOld, hierarquiaOferta);
                unitOfWork.Commit();
            }

            return hierarquiaOferta.Seq;
        }

        /// <summary>
        /// Atualiza a descrição completa, recursivamente, das ofertas filhas, conforme descrição da HierarquiaOferta
        /// </summary>
        /// <param name="hierarquiaOfertaBase">Hierarquia pai com sua hierarquia de filhas</param>
        /// <param name="hierarquiaOferta"></param>
        private void AtualizarCamposHierarquiasOfertasFilhas(HierarquiaOferta hierarquiaOfertaBase, HierarquiaOferta hierarquiaOferta)
        {
            if (hierarquiaOfertaBase.Seq > 0 && hierarquiaOfertaBase.HierarquiasOfertaFilhas.SMCAny())
            {
                foreach (var ofertaFilha in hierarquiaOfertaBase.HierarquiasOfertaFilhas)
                {
                    ofertaFilha.DescricaoCompleta = $"{hierarquiaOferta.DescricaoCompleta} => {ofertaFilha.Nome}";
                    this.SaveEntity(ofertaFilha);
                    // Propaga as alterações para as filhas das filhas e assim sucessivamente.
                    SalvarHierarquiaOferta(ofertaFilha);
                }
            }
        }

        /// <summary>
        /// Monta a string para a descrição completa baseada na hierarquia.
        /// </summary>
        public void GerarCamposHierarquia(HierarquiaOferta hierarquiaOferta)
        {
            // hierarquiaOferta.Nome = hierarquiaOferta.Nome.SMCToPascalCaseName();

            if (!hierarquiaOferta.SeqPai.HasValue)
            {
                hierarquiaOferta.DescricaoCompleta = hierarquiaOferta.Nome;
                hierarquiaOferta.HierarquiaCompleta = null;
            }
            else
            {
                var descricaoCompleta = new List<string>();
                var seqhierarquiaCompleta = new List<long>();

                //recursividade para buscar a arvore de hierarquia no banco, retornando uma lista de objetos da hierarquia
                var listaHierarquia = this.BuscarHierarquiaOfertaCompleta(hierarquiaOferta.SeqPai.Value);

                #region[Adiciona a descrição completa]
                listaHierarquia.Reverse();

                descricaoCompleta.AddRange(listaHierarquia.Select(s => s.Nome));

                descricaoCompleta.Add(hierarquiaOferta.Nome);

                hierarquiaOferta.DescricaoCompleta = string.Join(" => ", descricaoCompleta);

                hierarquiaOferta.DescricaoCompleta = hierarquiaOferta.DescricaoCompleta.SMCToPascalCaseName();
                #endregion

                #region[Adiciona os Seqs Hierarquia Completa]
                seqhierarquiaCompleta.Reverse();
             
                seqhierarquiaCompleta.AddRange(listaHierarquia.Select(s => s.Seq).ToList());

                hierarquiaOferta.HierarquiaCompleta = string.Join(",", seqhierarquiaCompleta);
                #endregion

            }
        }

        /// <summary>
        /// Verifica se o item da hierarquia pode ser excluido.
        /// </summary>
        /// <param name="seqHierarquiaOferta"></param>
        public void VerificaPermissaoExclusaoHierarquia(long seqHierarquiaOferta)
        {
            var spec = new SMCSeqSpecification<HierarquiaOferta>(seqHierarquiaOferta);
            var hierarquia = this.SearchProjectionBySpecification(spec,
                    x => new
                    {
                        EtapaInscricaoLiberada = x.Processo.EtapasProcesso.Where(s => s.Token == TOKENS.ETAPA_INSCRICAO && s.SituacaoEtapa == SituacaoEtapa.Liberada).Any()
                    }).FirstOrDefault();

            if (hierarquia != null && hierarquia.EtapaInscricaoLiberada)
            {
                throw new HierarquiaOfertaEtapaLiberadaException();
            }
        }

        /// <summary>
        /// Verifica se a inclusão de itens de hierarquia de oferta é permitida
        /// </summary>
        public void VerificarPermissaoCadastrarHierarquia(long seqProcesso)
        {
            var specProcesso = new SMCSeqSpecification<Processo>(seqProcesso);
            var dadosProcesso = this.ProcessoDomainService.SearchProjectionByKey(specProcesso,
                x => new
                {
                    TipoProcessoDesativado = x.UnidadeResponsavel.TiposProcesso
                        .Any(t => t.TipoProcesso.Seq == x.SeqTipoProcesso && !t.Ativo),
                    TipoHierarquiaOfertaDesativado = x.UnidadeResponsavel.TiposProcesso
                        .Any(t => t.TipoProcesso.Seq == x.SeqTipoProcesso && t.TiposHierarquiaOferta.Any(
                            h => h.SeqTipoHierarquiaOferta == x.SeqTipoHierarquiaOferta && !h.Ativo)),
                    PossuiHieraquiaCadastrada = x.HierarquiasOferta.Any()
                });
            if (!dadosProcesso.PossuiHieraquiaCadastrada)
            {
                if (dadosProcesso.TipoProcessoDesativado)
                {
                    //O tipo de processo informado para o processo foi desativado. Informe um tipo de processo ativo pra o processo antes de criar sua hierarquia de ofertas.
                    throw new HierarquiaOfertaTipoProcessoDesativadoException();
                }

                if (dadosProcesso.TipoHierarquiaOfertaDesativado)
                {
                    //O tipo de hierarquia de oferta informado para o processo foi desativado. Informe um tipo de hierarquia de oferta ativo pra o processo antes de criar sua hierarquia de ofertas.
                    throw new HierarquiaOfertaTipoHierarquiaOfertaDesativadoException();
                }
            }
        }

        public Dictionary<long, long?> AdicionarItemHierarquiaOferta(long seqProcesso, List<ItemOfertaHierarquiaOfertaVO> itensOfertasHierarquiasOfertas)
        {
            var hierarquiaOfertaMapping = new Dictionary<long, long?>();

            //Busca todos os itens da hierarquia configurados para o processo
            var ItensHierarquiaOfertaArvore = ItemHierarquiaOfertaDomainService.BuscarItensHierarquiaOfertaArvore(seqProcesso);

            //Specification para filtro da hierarquia de ofertas do processo
            var specHierarquiaProcesso = new HierarquiaOfertaFilterSpecification() { SeqProcesso = seqProcesso };

            //Seta a ordem dos itens pelo seq
            specHierarquiaProcesso.SetOrderBy(x => x.Seq);

            //Busca as hieraquias de oferta que o processo ja possui
            var hierarquiaOfertasProcesso = SearchBySpecification(specHierarquiaProcesso).ToList();

            //PERcorre os itens de hierarquia em grupo, e cria os novos itens hierarquia oferta de cada grupo em separado
            foreach (var grupoHierarquiaOfertaOrigem in itensOfertasHierarquiasOfertas.GroupBy(i => i.SeqHierarquiaOfertaOrigem).ToList())
            {
                //Variável para armazenar o sequencial do item de hierarquia pai
                long? seqPai = null;

                //Para cada item de oferta recebido
                foreach (var itemOfertaHierarquiaOferta in grupoHierarquiaOfertaOrigem)
                {
                    //Recupera o item de hierarquia pelo token
                    var itemHierarquiaOfertaArvore = ItensHierarquiaOfertaArvore.FirstOrDefault(i => i.Token == itemOfertaHierarquiaOferta.TokenTipoItemHierarquiaOferta);

                    //Prepara o novo item de hierarquia pra ser gravado
                    var novoItemHierarquiaOferta = new HierarquiaOferta()
                    {
                        SeqProcesso = seqProcesso,
                        Nome = itemOfertaHierarquiaOferta.Descricao,
                        SeqItemHierarquiaOferta = itemHierarquiaOfertaArvore.Seq
                    };

                    //Verifica se o processo ja possui o item de hierarquia
                    var itensHierarquiaOfertaProcesso = hierarquiaOfertasProcesso.Where(h => h.SeqItemHierarquiaOferta == itemHierarquiaOfertaArvore.Seq).ToList();

                    //Caso encontre algum item de hierarquia vinculado ao processo
                    if (itensHierarquiaOfertaProcesso.Count > 0)
                    {
                        //Racupera o item pela descricao e pelo SepPai se estiver preenchido (se for filho de alguem)
                        var itemHierarquiaOfertaProcessoDescricao = seqPai.HasValue ?
                               itensHierarquiaOfertaProcesso.FirstOrDefault(i => i.Nome.SMCToPascalCaseName() == itemOfertaHierarquiaOferta.Descricao.SMCToPascalCaseName() && i.SeqPai == seqPai.Value) :
                               itensHierarquiaOfertaProcesso.FirstOrDefault(i => i.Nome.SMCToPascalCaseName() == itemOfertaHierarquiaOferta.Descricao.SMCToPascalCaseName());

                        //Caso já exista o item, não é necessário criar novamente
                        if (itemHierarquiaOfertaProcessoDescricao != null)
                        {
                            seqPai = itemHierarquiaOfertaProcessoDescricao.Seq;
                            continue;
                        }
                    }

                    //Seta o sequencial do pai
                    novoItemHierarquiaOferta.SeqPai = seqPai;

                    //Salva o novo item de hierarquia de oferta
                    var novoSeq = SalvarHierarquiaOferta(novoItemHierarquiaOferta);

                    //insere ou atualiza o sequanciao no mapeamento de Seqs origem -> destino
                    if (hierarquiaOfertaMapping.ContainsKey(itemOfertaHierarquiaOferta.SeqHierarquiaOfertaOrigem))
                        hierarquiaOfertaMapping[itemOfertaHierarquiaOferta.SeqHierarquiaOfertaOrigem] = novoSeq;
                    else
                        hierarquiaOfertaMapping.Add(itemOfertaHierarquiaOferta.SeqHierarquiaOfertaOrigem, novoSeq);

                    //Atualiza o seq do pai na variável auxiliar
                    seqPai = novoSeq;

                    //Atualiza a lista de itens de hierarquia com o novo registro
                    hierarquiaOfertasProcesso.Add(novoItemHierarquiaOferta);
                }

            }

            return hierarquiaOfertaMapping;
        }
        /// <summary>
        /// Monta uma hierarquia de ofertas conforme os itens de hierarquia enviados
        /// </summary>
        /// <param name="seqProcesso">Sequencial do processo</param>
        /// <param name="itensOfertaHierarquiasOfertas">Itens das ofertas para a hierarquia</param>
        /// <param name="dataInicioInscricao">Data inicio da inscrição</param>
        /// <param name="dataFimInscricao">Data fim da inscricao</param>
        public Dictionary<long, long?> MontarHiererquiaOferta(long seqProcesso, List<ItemOfertaHierarquiaOfertaVO> itensOfertasHierarquiasOfertas, Dictionary<long, long> grupoOfertaMapping, Dictionary<long, long?> hierarquiaOfertaMapping, DateTime? dataInicioInscricao, DateTime? dataFimInscricao, ISMCUnitOfWork unitOfWork)
        {
            //Separa os sequenciais das hierarquias de oferta selecionadas pelo usuário
            var seqsHierarquiasOfertasGPI = itensOfertasHierarquiasOfertas.Select(i => i.SeqHierarquiaOfertaGPI).Distinct();

            //Para cada sequencial
            foreach (var seqHierarquiaOfertaGPI in seqsHierarquiasOfertasGPI)
            {
                //Cria uma lista que receberá a hierarquia de oferta atual
                var lista = new List<HierarquiaOfertaArvoreVO>();

                //Gera a hierarquia de oferta, baseado no sequencial do item folha da hierarquia de oferta
                var itensHierarquiaOfertaAtual = GerarHierarquiaOfertaItemFolha(seqHierarquiaOfertaGPI.Value, lista);

                //Separa as hierarquias de oferta selecionadas pelo usuário para pegar a descrição da hierarquia posteriormente
                var ofertasSelecionadas = itensOfertasHierarquiasOfertas.Where(i => i.SeqHierarquiaOfertaGPI == seqHierarquiaOfertaGPI).ToList();

                //Percorre a lista de hierarquias de oferta atual para gerar os novos registros
                foreach (var itemHierarquiaOfertaAtual in itensHierarquiaOfertaAtual)
                {
                    //Caso ja tenha criado o item pai, não é necessário criá-lo novamente, ou seja
                    //Se na lista de-para ja existir o sequencial do item antigo, com o sequencial do item novo, 
                    //o item novo ja foi criado e não é necessário criá-lo novamente.
                    //Ocorre quando temos dois filhos para o mesmo pai. O pai deve ser criado apenas uma vez.
                    if (hierarquiaOfertaMapping.ContainsKey(itemHierarquiaOfertaAtual.Seq))
                        continue;

                    //Recupera a hierarquia de oferta com todas as propriedades para servir de base para o novo item
                    var hierarquiaOferta = this.SearchByKey(new SMCSeqSpecification<HierarquiaOferta>(itemHierarquiaOfertaAtual.Seq));

                    //Caso seja filho de algum item, seta o sequencial do seu pai
                    if (itemHierarquiaOfertaAtual.SeqPai.HasValue)
                    {
                        hierarquiaOferta.SeqPai = hierarquiaOfertaMapping[itemHierarquiaOfertaAtual.SeqPai.Value];
                    }

                    //Prepara a hierarquia de oferta para gerar um novo registro
                    var seq = hierarquiaOferta.Seq;
                    hierarquiaOferta.Seq = 0;
                    hierarquiaOferta.SeqProcesso = seqProcesso;

                    if (hierarquiaOferta.EOferta)
                    {
                        hierarquiaOferta.Nome = ofertasSelecionadas.FirstOrDefault(i => i.SeqHierarquiaOfertaGPI == itemHierarquiaOfertaAtual.Seq && i.TokenTipoItemHierarquiaOferta == itemHierarquiaOfertaAtual.Token).Descricao;

                        ((Oferta)hierarquiaOferta).Ativo = true;
                        ((Oferta)hierarquiaOferta).DataCancelamento = null;
                        ((Oferta)hierarquiaOferta).MotivoCancelamento = null;
                        ((Oferta)hierarquiaOferta).DataInicio = dataInicioInscricao;
                        ((Oferta)hierarquiaOferta).DataFim = dataFimInscricao;
                        ((Oferta)hierarquiaOferta).ExigePagamentoTaxa = false;
                        ((Oferta)hierarquiaOferta).DataInicioAtividade = null;
                        ((Oferta)hierarquiaOferta).DataFimAtividade = null;
                        ((Oferta)hierarquiaOferta).CargaHorariaAtividade = null;
                        ((Oferta)hierarquiaOferta).Taxas = new List<OfertaPeriodoTaxa>();

                        var specOferta = new SMCSeqSpecification<Oferta>(seq);
                        var codigos = this.OfertaDomainService.SearchProjectionByKey(specOferta, x => x.CodigosAutorizacao);
                        foreach (var codigo in codigos)
                        {
                            codigo.Seq = 0;
                            codigo.SeqOferta = 0;
                        }
                        ((Oferta)hierarquiaOferta).CodigosAutorizacao = codigos;

                        var oferta = (hierarquiaOferta as Oferta);
                        if (oferta.SeqGrupoOferta.HasValue && oferta.SeqGrupoOferta.Value != 0)
                        {
                            oferta.SeqGrupoOferta = grupoOfertaMapping[oferta.SeqGrupoOferta.Value];
                        }
                    }

                    //Salva a nova hierarquia de oferta
                    long? novoSeq = 0;
                    if (!hierarquiaOferta.EOferta)
                    {
                        novoSeq = SalvarHierarquiaOferta(hierarquiaOferta);
                    }
                    else
                    {
                        novoSeq = OfertaDomainService.SalvarOferta((Oferta)hierarquiaOferta);
                    }

                    //Armazena o sequencial origem e destino
                    hierarquiaOfertaMapping.Add(seq, novoSeq);
                }
            }
            return hierarquiaOfertaMapping;
        }

        /// <summary>
        /// RN_CAM_077 Exclusão da oferta do GPI
        /// Excluir a oferta do GPI e seu respectivo item de hierarquia, referenciado na oferta do processo
        /// seletivo do SGA, da hierarquia de ofertas do processo do GPI, executando os serviços de exclusão
        /// de oferta e de hierarquia da oferta do GPI.
        /// Caso não reste na hierarquia de ofertas do processo do GPI mais nenhum filho associado ao pai do
        /// item excluído, remover o seu pai. Se não restar mais nenhum item associado ao pai do pai do item
        /// excluído, remover o pai do seu pai e assim sucessivamente, até excluir a raiz do galho da árvore
        /// do item excluído, se for o caso.
        /// </summary>
        /// <param name="seqProcesso"></param>
        /// <param name="seqHierarquiaOferta"></param>
        public void ExcluirHierarquiaOferta(long seqHierarquiaOferta)
        {
            VerificaPermissaoExclusaoHierarquia(seqHierarquiaOferta);

            var itemHierarquiaOferta = this.SearchByKey(new SMCSeqSpecification<HierarquiaOferta>(seqHierarquiaOferta));
            var hierarquiaOferta = this.SearchProjectionByKey(new SMCSeqSpecification<HierarquiaOferta>(seqHierarquiaOferta), x => x.HierarquiasOfertaFilhas.FirstOrDefault());

            if (itemHierarquiaOferta == null) { return; }

            // Início da transação
            using (var unitOfWork = SMCUnitOfWork.Begin())
            {
                if (hierarquiaOferta != null)
                {
                    /// Excluir a oferta do GPI
                    OfertaDomainService.DeleteEntity(hierarquiaOferta as Oferta);
                }

                //FIX: Tive que armazenar o seq pai em uma variável, porque ao excluir a entidade o mesmo estava ficando null
                var seqPai = itemHierarquiaOferta.SeqPai;

                /// e seu respectivo item de hierarquia,
                this.DeleteEntity(itemHierarquiaOferta);

                /// Caso não reste na hierarquia de ofertas do processo do GPI mais nenhum filho associado ao pai do
                /// item excluído, remover o seu pai.
                if (seqPai.HasValue)
                {
                    ExcluirHierarquiaOfertaPaiSemFilho(seqPai.Value);
                }

                unitOfWork.Commit();
            }
        }

        /// <summary>
        /// RN_CAM_077 Exclusão da oferta do GPI - Parte 2
        /// Caso não reste na hierarquia de ofertas do processo do GPI mais nenhum filho associado ao pai do
        /// item excluído, remover o seu pai. Se não restar mais nenhum item associado ao pai do pai do item
        /// excluído, remover o pai do seu pai e assim sucessivamente, até excluir a raiz do galho da árvore
        /// do item excluído, se for o caso.
        /// </summary>
        /// <param name="seqPaiHierarquiaOferta"></param>
        private void ExcluirHierarquiaOfertaPaiSemFilho(long seqPaiHierarquiaOferta)
        {
            var itemHierarquiaOferta = this.SearchByKey(new SMCSeqSpecification<HierarquiaOferta>(seqPaiHierarquiaOferta));
            var hierarquiaOfertaFilhas = this.SearchProjectionByKey(new SMCSeqSpecification<HierarquiaOferta>(seqPaiHierarquiaOferta), x => x.HierarquiasOfertaFilhas).ToList();

            // Verifico se não possui filhas
            if (!hierarquiaOfertaFilhas.SMCAny())
            {
                //FIX: Tive que armazenar o seq pai em uma variável, porque ao excluir a entidade o mesmo estava ficando null
                var seqPai = itemHierarquiaOferta.SeqPai;

                // se não possuir filhos eu excluo a oferta de hierarquia
                this.DeleteEntity(itemHierarquiaOferta);
                // se possuir um pai
                if (seqPai.HasValue)
                {
                    // Faço a recursividade para verificar se o pai possuir filhas.
                    // Remover o pai do seu pai e assim sucessivamente,
                    ExcluirHierarquiaOfertaPaiSemFilho(seqPai.Value);
                }
            }
        }

        private List<HierarquiaOfertaArvoreVO> GerarHierarquiaOfertaItemFolha(long seqHierarquiaOferta, List<HierarquiaOfertaArvoreVO> lista)
        {
            lista = lista ?? new List<HierarquiaOfertaArvoreVO>();

            var item = this.SearchProjectionByKey(new SMCSeqSpecification<HierarquiaOferta>(seqHierarquiaOferta), h => new HierarquiaOfertaArvoreVO()
            {
                Seq = h.Seq,
                SeqPai = h.SeqPai,
                Token = h.ItemHierarquiaOferta.TipoItemHierarquiaOferta.Token
            });

            lista.Add(item);

            if (item.SeqPai.HasValue)
                GerarHierarquiaOfertaItemFolha(item.SeqPai.Value, lista);

            return lista.OrderBy(o => o.SeqPai).ToList();
        }

    }
}