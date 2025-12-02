using SMC.Framework.Extensions;
using SMC.Framework.Model;
using SMC.Framework.Specification;
using SMC.Inscricoes.Common.Areas.RES;
using SMC.Inscricoes.Common.Areas.RES.Exceptions;
using SMC.Inscricoes.Common.Areas.RES.Exceptions.UnidadeResponsavelTipoProcesso;
using SMC.Inscricoes.Domain.Areas.INS.DomainServices;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using SMC.Inscricoes.Domain.Areas.RES.Models;
using SMC.Inscricoes.Domain.Areas.RES.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SMC.Inscricoes.Domain.Areas.RES.DomainServices
{
    public class UnidadeResponsavelTipoProcessoDomainService : InscricaoContextDomain<UnidadeResponsavelTipoProcesso>
    {
        #region DomainServices
        private ProcessoDomainService ProcessoDomainService
        {
            get { return this.Create<ProcessoDomainService>(); }
        }
        
        private UnidadeResponsavelTipoProcessoIdVisualDomainService UnidadeResponsavelTipoProcessoIdVisualDomainService
        {
            get { return this.Create<UnidadeResponsavelTipoProcessoIdVisualDomainService>(); }
        }

        #endregion

        public long Salvar(UnidadeResponsavelTipoProcesso unidadeResponsavelTipoProcesso)
        {
          

            if (unidadeResponsavelTipoProcesso.Seq != default(long))
            {
                ValidaAssociacoes(unidadeResponsavelTipoProcesso);                
            }
            //Deve existir ao menos uma Identidade visual por unidade responsável e tipo de processo Ativa.  
            if (!unidadeResponsavelTipoProcesso.IdentidadesVisuais.Any(a=>a.Ativo))
            {
                throw new NaoExisteIdentidadeVisualAtivaException();
            }

            foreach (var identidadeVisual in unidadeResponsavelTipoProcesso.IdentidadesVisuais)
            {
                //Se informou o token do CSS alternativo do SAS, valida o formato
                if (!string.IsNullOrEmpty(identidadeVisual.TokenCssAlternativoSas) &&
                  !ValidaTokenCss(identidadeVisual.TokenCssAlternativoSas))
                {
                    throw new FormatoDeTokenInvalidoException(identidadeVisual.TokenCssAlternativoSas);
                }
            }

            this.SaveEntity(unidadeResponsavelTipoProcesso);
            
            return unidadeResponsavelTipoProcesso.Seq;
        }

        private bool ValidaTokenCss(string tokenCss) => tokenCss.Length >= 3 && Regex.IsMatch(tokenCss, @"^(?:[A-Z0-9_]+|)$");

        private void ValidaAssociacoes(UnidadeResponsavelTipoProcesso unidadeResponsavelTipoProcesso)
        {
            var spec = new SMCSeqSpecification<UnidadeResponsavelTipoProcesso>(unidadeResponsavelTipoProcesso.Seq);
            var tipoProcessoEntidade = this.SearchByKey(spec, IncludesUnidadeResponsavelTipoProcesso.TipoProcesso |
                                                              IncludesUnidadeResponsavelTipoProcesso.UnidadeResponsavel |
                                                              IncludesUnidadeResponsavelTipoProcesso.TiposHierarquiaOferta_TipoHierarquiaOferta |
                                                              IncludesUnidadeResponsavelTipoProcesso.TiposHierarquiaOferta);

            //Se o usuário estiver tentando modificar o formulário selecionado, verifica se existe algum processo já cadastrado.
            if (unidadeResponsavelTipoProcesso.SeqTipoProcesso != tipoProcessoEntidade.SeqTipoProcesso)
            {
                var spec2 = new ProcessoFilterSpecification()
                {
                    SeqTipoProcesso = tipoProcessoEntidade.SeqTipoProcesso,
                    SeqUnidadeResponsavel = tipoProcessoEntidade.SeqUnidadeResponsavel
                };

                if (ProcessoDomainService.Count(spec2) > 0)
                {
                    throw new UnidadeResponsavelTipoProcessoAlterException(
                                tipoProcessoEntidade.TipoProcesso.Descricao,
                                tipoProcessoEntidade.UnidadeResponsavel.Nome);
                }
            }

            // Filtrar os tipos de hierarquia que estão sendo modificados
            var itensHierarquiaModificados = tipoProcessoEntidade.TiposHierarquiaOferta
                   .Where(t => !unidadeResponsavelTipoProcesso.TiposHierarquiaOferta
                   .Select(u => u.SeqTipoHierarquiaOferta)
                   .Contains(t.SeqTipoHierarquiaOferta))
                   .Select(x => (long?)x.SeqTipoHierarquiaOferta)
                   .ToList();

            //Verifica se todos os itens da lista de hierarquia são iguais. Não é permitido modificar se já existir um processo associado.
            // Se nao houver processo sendo modificado, nem entra na condição
            if (tipoProcessoEntidade.TiposHierarquiaOferta != null && itensHierarquiaModificados.Count() > 0)
            {

                var specProcesso = new ProcessoFilterSpecification()
                {
                    SeqUnidadeResponsavel = unidadeResponsavelTipoProcesso.SeqUnidadeResponsavel,
                    SeqTipoProcesso = unidadeResponsavelTipoProcesso.SeqTipoProcesso,
                    SeqsTiposHierarquiaOfertas = itensHierarquiaModificados
                };

                // Dados de processo
                var processosComHierarquiaModificada = ProcessoDomainService.SearchProjectionBySpecification(specProcesso, x => new
                {
                    SeqProcesso = x.Seq,
                    SeqUnidadeResponsavel = x.SeqUnidadeResponsavel,
                    SeqTipoHierarquiaOferta = x.SeqTipoHierarquiaOferta,
                    SeqTipoProcesso = x.SeqTipoProcesso,
                    TipoDeProcesso = x.TipoProcesso,
                    TipoHierarquia = x.TipoHierarquiaOferta
                }).ToList();

                // Se foi encontrado a hierarquia a ser modificada vinculada a algum processo, lançar a exception
                if (processosComHierarquiaModificada.Count > 0)
                {
                    throw new UnidadeResponsavelTipoHierarquiaExcludeException(
                                        tipoProcessoEntidade.TiposHierarquiaOferta.Where(f => f.SeqTipoHierarquiaOferta == itensHierarquiaModificados.FirstOrDefault()).First().TipoHierarquiaOferta.Descricao,
                                        tipoProcessoEntidade.UnidadeResponsavel.Nome,
                                        tipoProcessoEntidade.TipoProcesso.Descricao);
                }
            }
        }

        public void Excluir(long seqConfiguracaoTipoProcessoTipoHierarquiaOferta)
        {
            var spec = new SMCSeqSpecification<UnidadeResponsavelTipoProcesso>(seqConfiguracaoTipoProcessoTipoHierarquiaOferta);
            var tipoProcessoEntidade = this.SearchByKey(spec, IncludesUnidadeResponsavelTipoProcesso.TipoProcesso |
                                                              IncludesUnidadeResponsavelTipoProcesso.UnidadeResponsavel |
                                                              IncludesUnidadeResponsavelTipoProcesso.TiposHierarquiaOferta);

            var spec2 = new ProcessoFilterSpecification()
            {
                SeqTipoProcesso = tipoProcessoEntidade.SeqTipoProcesso,
                SeqUnidadeResponsavel = tipoProcessoEntidade.SeqUnidadeResponsavel
            };

            if (ProcessoDomainService.Count(spec2) > 0)
            {
                throw new UnidadeResponsavelTipoProcessoExcludeException(
                            tipoProcessoEntidade.TipoProcesso.Descricao,
                            tipoProcessoEntidade.UnidadeResponsavel.Nome);
            }

            this.DeleteEntity<UnidadeResponsavelTipoProcesso>(seqConfiguracaoTipoProcessoTipoHierarquiaOferta);

        }

        public string BuscaTokenCssAlternativo(long seqUnidadeResponsavelTipoProcessoIdVisual)
        {
            var spec = new SMCSeqSpecification<UnidadeResponsavelTipoProcessoIdVisual>(seqUnidadeResponsavelTipoProcessoIdVisual);
            var tokenCss = this.UnidadeResponsavelTipoProcessoIdVisualDomainService.SearchByKey(spec).TokenCssAlternativoSas;
            return tokenCss;
        }
        public UnidadeResponsavelTipoProcessoIdVisual BuscarIdentidadeVisual(long seqUnidadeResponsavelTipoProcessoIdVisual)
        {
            var spec = new SMCSeqSpecification<UnidadeResponsavelTipoProcessoIdVisual>(seqUnidadeResponsavelTipoProcessoIdVisual);
            var identidade = this.UnidadeResponsavelTipoProcessoIdVisualDomainService.SearchByKey(spec);
            
            return identidade;
        }




        public List<SMCDatasourceItem> BuscarIdentidadesVisuais(long seqUnidadeResponsavel, long seqTipoProcesso)
        {
            var spec = new UnidadeResponsavelTipoProcessoFilterSpecification() { SeqUnidadeResponsavel = seqUnidadeResponsavel, SeqTipoProcesso= seqTipoProcesso };

            var teste = this.SearchProjectionByKey(spec, s => s); 
            
            var identidadesVisuais = this.SearchProjectionByKey(spec, s => s.IdentidadesVisuais.Select(se=>  new SMCDatasourceItem
            {
                Seq = se.Seq,
                Descricao = se.Descricao
            })).ToList();

            return identidadesVisuais;
        }

    }
}
