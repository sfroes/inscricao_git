using SMC.Formularios.ServiceContract.Areas.TMP.Interfaces;
using SMC.Framework;
using SMC.Framework.Domain;
using SMC.Framework.Model;
using SMC.Framework.Specification;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using SMC.Inscricoes.Common.Areas.INS.Exceptions;
using SMC.Inscricoes.Common.Areas.INS.Exceptions.TipoProcesso;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SMC.Inscricoes.Domain.Areas.INS.DomainServices
{
    public class TipoProcessoDomainService : InscricaoContextDomain<TipoProcesso>
    {
        #region Services

        private ITipoTemplateProcessoService TipoTemplateProcessoService
        {
            get { return this.Create<ITipoTemplateProcessoService>(); }
        }

        private ProcessoDomainService ProcessoDomainService
        {
            get { return this.Create<ProcessoDomainService>(); }
        }

        private TaxaDomainService TaxaDomainService
        {
            get { return this.Create<TaxaDomainService>(); }
        }

        #endregion Services

        /// <summary>
        /// Retorna todos os tipos de processo com apenas seq  e descricao preenchidos
        /// </summary>
        public List<SMCDatasourceItem> BuscarTiposProcessoKeyValue(long? seqUnidadeResponsavel = null)
        {
            if (seqUnidadeResponsavel.HasValue)
            {

                var lista = this.SearchProjectionBySpecification(new TipoProcessoUnidadeResponsavelSpecification() { SeqUnidadeResponsavel = seqUnidadeResponsavel.Value },
                                        x => new SMCDatasourceItem() { Seq = x.Seq, Descricao = x.Descricao }).ToList();
                if (lista.Count == 1)
                    lista[0].Selected = true;

                return lista;
            }

            return this.SearchProjectionAll(x => new SMCDatasourceItem { Seq = x.Seq, Descricao = x.Descricao }, x => x.Descricao).ToList();
        }

        public long Salvar(TipoProcesso tipoProcesso)
        {
            ValidarCamposInscrito(tipoProcesso.CamposInscrito.ToList());

            //Ao alterar a biblioteca, verficar se algum processo do tipo de processo em questão
            //possui inscrição com UID GED do processo preenchido.
            if (tipoProcesso.Seq != 0)
            {
                var spec = new TipoProcessoFilterSpecification() { Seq = tipoProcesso.Seq };
                var bibliotecaAntiga = this.SearchProjectionByKey(spec, s => s.SeqContextoBibliotecaGed);

                if (bibliotecaAntiga != tipoProcesso.SeqContextoBibliotecaGed)
                {
                    if (ProcessoDomainService.ValidaTipoProcessoComUIDGed(tipoProcesso.Seq))
                    {
                        throw new DossieCriadoGedException();
                    }

                }
            }

            var novosTemplates = tipoProcesso.Templates.Select(f => f.SeqTemplateProcessoSGF).ToArray();
            var templates = TipoTemplateProcessoService.BuscarTemplateProcesso(novosTemplates);

            foreach (var template in templates)
            {
                if (!template.Ativo)
                {
                    throw new AlteracaoTemplateProcessoDesativadoException(template.Descricao);
                }
            }

            if (!ValidaTokenCss(tipoProcesso.TokenResource))
            {
                throw new FormatoDeTokenInvalidoException();
            }

            if (tipoProcesso.Seq != 0)
            {
                var spec = new SMCSeqSpecification<TipoProcesso>(tipoProcesso.Seq);
                var tipoProcessoBanco = this.SearchByKey(spec, IncludesTipoProcesso.Templates | IncludesTipoProcesso.TiposTaxa);

                if (!tipoProcesso.Templates.SMCContainsList(tipoProcessoBanco.Templates, f => f.SeqTemplateProcessoSGF, out IEnumerable<long> missingTemplates))
                {
                    var processoSpec = new ProcessoContainsTemplateSGFSpecification(missingTemplates.ToArray());
                    var tipoProcessoSpec = new ProcessoFilterSpecification { SeqTipoProcesso = tipoProcesso.Seq };
                    var processos = ProcessoDomainService.SearchProjectionBySpecification(tipoProcessoSpec & processoSpec,
                        x => new
                        {
                            SeqSGF = x.SeqTemplateProcessoSGF,
                            DescricaoTipo = x.TipoProcesso.Descricao,
                        });

                    var templatesMissingSGF = TipoTemplateProcessoService.BuscarTemplateProcesso(missingTemplates.ToArray());

                    if (processos.Any())
                    {
                        throw new TemplateProcessoAssociadoProcessoException(
                                        templatesMissingSGF.Where(f => f.Seq == processos.ElementAt(0).SeqSGF).First().Descricao,
                                        processos.First().DescricaoTipo);
                    }
                }

                if (!tipoProcesso.TiposTaxa.SMCContainsList(tipoProcessoBanco.TiposTaxa, f => f.SeqTipoTaxa, out IEnumerable<long> missingTaxas))
                {
                    var taxasSpec = new SMCContainsSpecification<Taxa, long>(f => f.SeqTipoTaxa, missingTaxas.ToArray());
                    var taxas = TaxaDomainService.SearchProjectionBySpecification(taxasSpec,
                                        x => new
                                        {
                                            x.Processo.SeqTipoProcesso,
                                            TipoProcesso = x.Processo.TipoProcesso.Descricao,
                                            TipoTaxa = x.TipoTaxa.Descricao
                                        });
                    if (taxas.Any(x => x.SeqTipoProcesso == tipoProcesso.Seq))
                    {
                        throw new TipoTaxaAssociadaProcessoException(
                                        taxas.First().TipoTaxa,
                                        taxas.First().TipoProcesso);
                    }
                }
                VerificaCampoObrigatorio(tipoProcesso);
            }

            VerificaCampoObrigatorio(tipoProcesso);

            this.SaveEntity(tipoProcesso);
            return tipoProcesso.Seq;
        }
        private bool ValidaTokenCss(string tokenCss)
        {
            return !string.IsNullOrEmpty(tokenCss) && tokenCss.Length >= 3 && Regex.IsMatch(tokenCss, @"^(?:[A-Z0-9_]+|)$");
        }
        private void VerificaCampoObrigatorio(TipoProcesso tipoProcesso)
        {
            if (!string.IsNullOrEmpty(tipoProcesso.OrientacaoAceiteConversaoPDF) && string.IsNullOrEmpty(tipoProcesso.TermoAceiteConversaoPDF))
                throw new TipoProcessoTermoObrigatoriaException();

            if (!string.IsNullOrEmpty(tipoProcesso.TermoAceiteConversaoPDF) && string.IsNullOrEmpty(tipoProcesso.OrientacaoAceiteConversaoPDF))
                throw new TipoProcessoOrientacaoObrigatoriaException();
        }
        private void ValidarCamposInscrito(List<TipoProcessoCampoInscrito> camposInscrito)
        {
            if (camposInscrito.Count(c => c.CampoInscrito == CampoInscrito.Nome) == 0)
            {
                throw new TipoProcessoCampoInscritoNomeException();
            }

            if (camposInscrito.Count(c => c.CampoInscrito == CampoInscrito.CPF || c.CampoInscrito == CampoInscrito.Passaporte) == 0)
            {
                throw new TipoProcessoCampoInscritoCPFPassaporteException();
            }

            if ((camposInscrito.Any(c => c.CampoInscrito == CampoInscrito.OrgaoEmissorIdentidade) || 
                 camposInscrito.Any(c => c.CampoInscrito == CampoInscrito.UfIdentidade)) && 
                 !camposInscrito.Any(c => c.CampoInscrito == CampoInscrito.NumeroIdentidade))
            {
                throw new TipoProcessoCampoInscritoRGException();
            }
           
            if(camposInscrito.Count(c => c.CampoInscrito == CampoInscrito.Email) == 0)
            {
                throw new TipoProcessoCampoInscritoEmailException();
            }

            if((camposInscrito.Any(a => a.CampoInscrito == CampoInscrito.Naturalidade) && 
                !camposInscrito.Any(a => a.CampoInscrito == CampoInscrito.PaisOrigem)))
            {
                throw new TipoProcessoCampoInscritoNaturalidadeException();
            }
        }

        public bool PossuiConsistencia(TipoProcesso tipoProcesso, TipoConsistencia tipoConsistencia)
        {
            if (tipoProcesso.Consistencias == null)
            {
                tipoProcesso.Consistencias = SearchProjectionByKey(new SMCSeqSpecification<TipoProcesso>(tipoProcesso.Seq), x => x.Consistencias);
            }

            return tipoProcesso.Consistencias.Any(f => f.TipoConsistencia == tipoConsistencia);
        }

        public bool PossuiConsistencia(long seqTipoProcesso, TipoConsistencia tipoConsistencia)
        {
            return this.SearchProjectionByKey(seqTipoProcesso, x => x.Consistencias.Any(c => c.TipoConsistencia == tipoConsistencia));
        }

        /// <summary>
        /// Buscar tipo de processo de um processo
        /// </summary>
        /// <param name="seqProcesso">Sequencial Processo</param>
        /// <returns>Tipo de processo</returns>
        public TipoProcesso BuscarTipoProcessoPorProcesso(long seqProcesso)
        {
            var spec = new TipoProcessoFilterSpecification() { SeqProcesso = seqProcesso };
            var tipoProcesso = this.SearchBySpecification(spec).FirstOrDefault();

            return tipoProcesso;
        }

        /// <summary>
        /// Busca tipo de processo de uma inscrição
        /// </summary>
        /// <param name="seqInscricao">Sequencial da inscrição</param>
        /// <returns>Tipo de processo</returns>
        public TipoProcesso BuscarTipoProcessoPorInscricao(long seqIncricao)
        {
            var spec = new TipoProcessoFilterSpecification() { SeqInscricao = seqIncricao };
            var tipoProcesso = this.SearchBySpecification(spec).FirstOrDefault();

            return tipoProcesso;
        }

        public bool VerificaIntegraGPC(long seqProcesso)
        {
            var spec = new TipoProcessoFilterSpecification() { SeqProcesso = seqProcesso };
            var tipoProcesso = this.SearchByKey(spec);

            return tipoProcesso.IntegraGPC;
        }

        public bool ConferirHabilitaDatasEvento(long seqTipoProcesso)
        {
            var spec = new TipoProcessoFilterSpecification() { Seq = seqTipoProcesso };
            var tipoProcesso = this.SearchByKey(spec);
            return tipoProcesso.GestaoEventos;
        }

        /// <summary>
        /// Valida se o tipo de documento está ativo para ser usado no processo
        /// </summary>
        public bool BuscarSituacaoTipoDocumentoDoProcesso(long seqTipoProcesso, long seqTipoDocumento)
        {
            var tipoProcesso = this.SearchProjectionByKey(new SMCSeqSpecification<TipoProcesso>(seqTipoProcesso), x => x.Documentos);
            var tipoDocumento = tipoProcesso.Where(x => x.SeqTipoDocumento == seqTipoDocumento);
            if (tipoDocumento.Any(y => y.Ativo == false))
            {
                return true;
            }
            return false;
        }
        
        /// <summary>
        /// Verifica se o processo é de um tipo cujo flag "Habilita gestão de eventos" está ativa.
        /// </summary>
        public bool ConferirHabilitaGestaoEvento(long seqTipoProcesso)
        {
            var spec = new TipoProcessoFilterSpecification() { Seq = seqTipoProcesso };
            var tipoProcesso = this.SearchByKey(spec);
            return tipoProcesso.GestaoEventos;            
        } 

  
    }
}