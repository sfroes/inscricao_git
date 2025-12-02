using SMC.AssinaturaDigital.ServiceContract.Areas.CAD.Interfaces;
using SMC.Framework;
using SMC.Framework.Domain;
using SMC.Framework.Extensions;
using SMC.Framework.Model;
using SMC.Framework.Specification;
using SMC.Inscricoes.Common.Areas.RES.Exceptions;
using SMC.Inscricoes.Domain.Areas.INS.DomainServices;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using SMC.Inscricoes.Domain.Areas.RES.Models;
using SMC.Inscricoes.Domain.Areas.RES.Specifications;
using SMC.Inscricoes.Domain.Areas.RES.Validators;
using SMC.Inscricoes.ServiceContract.Areas.RES.Data;
using System.Collections.Generic;
using System.Linq;

namespace SMC.Inscricoes.Domain.Areas.RES.DomainServices
{
    public class UnidadeResponsavelDomainService : InscricaoContextDomain<UnidadeResponsavel>
    {
        #region Domain Services

        private ISistemaOrigemService SistemaOrigemService { get => Create<ISistemaOrigemService>(); }
        private ConfiguracaoModeloDocumentoDomainService ConfiguracaoModeloDocumentoDomainService { get => Create<ConfiguracaoModeloDocumentoDomainService>(); }

        #endregion

        /// <summary>
        /// Retorna todos os tipos de processo com apenas seq  e descricao preenchidos
        /// </summary>        
        public IEnumerable<SMCDatasourceItem> BuscarUnidadesResponsaveisKeyValue()
        {
            return this.SearchProjectionAll(x => new SMCDatasourceItem { Seq = x.Seq, Descricao = x.Nome }
                , x => x.Nome);
        }

        /// <summary>
        /// Salva Unidade Responsável e faz validações necessárias
        /// </summary>   
        public long SalvarUnidadeResponsavel(UnidadeResponsavelData unidadeResponsavel)
        {
            //Validar se existe alguma configuração modelo documento relacionada ao(s) processo(s) relacionados a Unidade responsável, não permitir edição caso exista.
            if (unidadeResponsavel.Seq > 0)
            {
                var spec = new ConfiguracaoModeloDocumentoFilterSpecification() { SeqUnidadeResponsavel = unidadeResponsavel.Seq };

                if (ValidaAlteracaoCampoGAD(unidadeResponsavel))
                {
                    if (ConfiguracaoModeloDocumentoDomainService.SearchBySpecification(spec).Any())
                    {
                        throw new SistemaOrigemJaAssociadoException(unidadeResponsavel.Nome);
                    }
                }
            }

            foreach (var endereco in unidadeResponsavel.Enderecos)
            {
                endereco.Cep = endereco.Cep.SMCRemoveNonDigits();
            }

            unidadeResponsavel.Transform<UnidadeResponsavel>();
            return this.SaveEntity<UnidadeResponsavel>(unidadeResponsavel, new UnidadeResponsavelValidator());
        }

        private bool ValidaAlteracaoCampoGAD(UnidadeResponsavelData unidadeResponsavel)
        {
            bool retorno = false;

            var spec = new UnidadeResponsavelFilterSpecification() { Seq = unidadeResponsavel.Seq };

            string tokenGAD = this.SearchProjectionByKey(spec, s => s.TokenSistemaOrigemGad);

            if (tokenGAD == null || unidadeResponsavel.TokenSistemaOrigemGad == null)
            {
                return retorno;
            }

            if (unidadeResponsavel.TokenSistemaOrigemGad.Equals(tokenGAD))
                retorno = true;

            return retorno;
        }


        /// <summary>
        /// Busca Sistemas origem do GAD pela sigla "GPI"
        /// </summary>   
        public List<SMCDatasourceItem<string>> BuscarSistemaOrigemGADSelect(string sigla)
        {
            return SistemaOrigemService.BuscaDadosSelectSistemaOrigemPorSigla(sigla);
        }

        /// <summary>
        /// Busca as configurações de documento disponíveis no GAD para o Sistema/Origem configurado na unidade responsável do processo
        /// </summary> 
        public List<SMCDatasourceItem<string>> BuscarConfiguracoesAssinaturaGadSelect(long seqUnidadeResponsavel)
        {
            var unidadeResponsavel = this.SearchByKey(seqUnidadeResponsavel);
            var configuracoesDocumento = SistemaOrigemService.BuscarConfiguracaoDocumentoPorSistemaOrigemSelect(unidadeResponsavel.TokenSistemaOrigemGad);
            return configuracoesDocumento;
        }

        /// <summary>
        /// Valida se o tipo de formulário está ativo para ser usado no processo
        /// </summary> 
        public bool BuscarSituacaoTipoFormularioDaUnidadeResponsavel(long seqUnidadeResponsavel, long seqTipoFormulario)
        {
            var tipoFormularioUnidadeResponsavel = this.SearchProjectionByKey(new SMCSeqSpecification<UnidadeResponsavel>(seqUnidadeResponsavel), x => x.TiposFormulario);
            var tipoFormulario = tipoFormularioUnidadeResponsavel.Where(x => x.SeqTipoFormularioSGF == seqTipoFormulario);
            if (tipoFormulario.Any(y => y.Ativo == false))
            {
                return true;
            }
            return false;
        }

        public int BuscarCodigoUnidadePromotora(long seqUnidadeResponsavel)
        {
            return this.SearchProjectionByKey(seqUnidadeResponsavel, x => x.CodigoUnidade).Value;
        }
    }

}
