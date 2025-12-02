using SMC.Framework.Domain;
using SMC.Framework.Specification;
using SMC.Inscricoes.Common.Areas.INS.Exceptions;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Linq;

namespace SMC.Inscricoes.Domain.Areas.INS.DomainServices
{
    public class CodigoAutorizacaoDomainService : InscricaoContextDomain<CodigoAutorizacao>
    {


        #region DomainServices

        private InscricaoDomainService InscricaoDomainService 
        {
            get { return this.Create<InscricaoDomainService>(); }
        }

        #endregion

        public long SalvarCodigoAutorizacao(CodigoAutorizacao codigoAutorizacao)
        {
            if (codigoAutorizacao.Seq == default(long))
            {
                return this.InsertEntity(codigoAutorizacao).Seq;
            }
            else
            {
                //Realiza validação da RN_INS_110 - Consistência da alteração do código de autorizaçãovar
                var seqSpec = new SMCSeqSpecification<CodigoAutorizacao>(codigoAutorizacao.Seq);
                var codigoOld = this.SearchByKey(seqSpec);
                if (codigoAutorizacao.Codigo != codigoOld.Codigo
                    || codigoOld.SeqCliente != codigoAutorizacao.SeqCliente
                    || codigoAutorizacao.SeqUnidadeResponsavel != codigoOld.SeqUnidadeResponsavel) 
                {
                    var spec = new CodigoAutorizacaoComInscricaoSpecification(codigoAutorizacao.Seq);
                    var numInscricoesComCodigo = this.InscricaoDomainService.Count(spec);
                    if (numInscricoesComCodigo > 0)
                    {
                        throw new InscricaoExistenteComCodigoAutorizacaoException(codigoOld.Codigo);
                    }
                }                
                UpdateEntity(codigoAutorizacao);
                return codigoAutorizacao.Seq;
            }
        }

    }

    public class CodigoAutorizacaoComInscricaoSpecification : SMCSpecification<Inscricao>
    {
        public long SeqCodigoAutorizacao { get; set; }

        public CodigoAutorizacaoComInscricaoSpecification(long seqCodigoAutorizacao)
        {
            this.SeqCodigoAutorizacao = seqCodigoAutorizacao;
        }

        public override System.Linq.Expressions.Expression<Func<Inscricao, bool>> SatisfiedBy()
        {
            return i => i.CodigosAutorizacao.Any(x => x.SeqCodigoAutorizacao == this.SeqCodigoAutorizacao);
        }
    }
}
