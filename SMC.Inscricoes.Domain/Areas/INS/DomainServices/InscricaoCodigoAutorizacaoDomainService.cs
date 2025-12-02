using SMC.Framework.Mapper;
using SMC.Framework.Specification;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Inscricoes.Common.Areas.INS.Exceptions;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using SMC.Inscricoes.Domain.Areas.INS.ValueObjects;
using System.Collections.Generic;
using System.Linq;

namespace SMC.Inscricoes.Domain.Areas.INS.DomainServices
{
    public class InscricaoCodigoAutorizacaoDomainService : InscricaoContextDomain<InscricaoCodigoAutorizacao>
    {
        #region DomainServices

        private InscricaoDomainService InscricaoDomainService
        {
            get { return this.Create<InscricaoDomainService>(); }
        }

        private InscricaoOfertaDomainService InscricaoOfertaDomainService
        {
            get { return this.Create<InscricaoOfertaDomainService>(); }
        }

        #endregion DomainServices

        /// <summary>
        /// Salva a lista de códigos de uma inscrição
        /// </summary>
        /// <param name="seqInscricao">Sequencial da inscrição</param>
        /// <param name="param">Lista de códigos a serem salvos</param>
        public void SalvarListaInscricaoCodigoAutorizacao(long seqInscricao, List<InscricaoCodigoAutorizacaoVO> param)
        {
            // Verifica se todas as InscricaoCodigoAutorizacao recebidas como parametro são de uma mesma inscrição
            if (param.Count > 1 && param.Select(o => o.SeqInscricao).Distinct().Count() > 1)
                throw new InscricaoInvalidaException();

            // Se o sequencial da inscrição recebido como parametro for diferente da lista de códigos, erro
            if (param.Count > 1 && seqInscricao != param.Select(o => o.SeqInscricao).Distinct().SingleOrDefault())
                throw new InscricaoInvalidaException();

            // Busca as ofertas da inscrição de 1ª opção
            InscricaoOfertaFilterSpecification specOferta = new InscricaoOfertaFilterSpecification() { SeqInscricao = seqInscricao };
            IncludesInscricaoOferta includes = IncludesInscricaoOferta.Oferta |
                                                IncludesInscricaoOferta.Oferta_CodigosAutorizacao |
                                                IncludesInscricaoOferta.Oferta_CodigosAutorizacao_CodigoAutorizacao;

            var ofertas = InscricaoOfertaDomainService.SearchBySpecification(specOferta, includes).ToList();
            if (ofertas.Count <= 0)
                throw new InscricaoInvalidaException();

            // Verifica se alguma oferta de primeira opção exige o código
            bool exigeCodigo = ofertas.Any(o => o.NumeroOpcao == 1 && o.Oferta.InscricaoSoComCodigo == true);

            // Se a oferta exige codigo e o mesmo não foi informado, erro
            if (exigeCodigo && param.Count <= 0)
                throw new InscricaoExigeCodigoAutorizacaoException();

            // Verificando se caso alguma oferta exige código, se o código informado é dela.
            if (exigeCodigo && param.Count() > 0 && !ofertas.Any(x => x.Oferta.CodigosAutorizacao
                       .Any(c => param.Any(p => p.Codigo == c.CodigoAutorizacao.Codigo))))
            {
                throw new CodigoAutorizacaoInvalidoParaInscricaoException();
            }

            // Se nenhuma oferta possui código e o mesmo foi informado, erro
            if (!ofertas.Any(o => o.Oferta.CodigosAutorizacao.Count > 0) && param.Count > 0)
                throw new CodigoAutorizacaoInvalidoParaInscricaoException();

            // Verifica se os códigos informados são válidos para as ofertas
            foreach (var item in param)
            {
                if (!ofertas.Any(o => o.Oferta.CodigosAutorizacao.Any(x =>
                    x.CodigoAutorizacao.Codigo.Equals(item.Codigo))))
                {
                    throw new CodigoAutorizacaoInvalidoParaInscricaoException();
                }
                else
                {
                    var ofertaComCodigo = ofertas.FirstOrDefault(o => o.Oferta.CodigosAutorizacao.Any(x =>
                    x.CodigoAutorizacao.Codigo.Equals(item.Codigo)));
                    item.SeqCodigoAutorizacao = ofertaComCodigo.Oferta.CodigosAutorizacao.FirstOrDefault(x => x.CodigoAutorizacao.Codigo.Equals(item.Codigo)).SeqCodigoAutorizacao;
                }
            }

            // Busca a inscrição
            var includesInscricao = IncludesInscricao.CodigosAutorizacao |
                                    IncludesInscricao.ConfiguracaoEtapa |
                                    IncludesInscricao.ConfiguracaoEtapa_EtapaProcesso;
            SMCSeqSpecification<Inscricao> spec = new SMCSeqSpecification<Inscricao>(seqInscricao);
            Inscricao inscricao = InscricaoDomainService.SearchByKey(spec, includesInscricao);

            // Verifica as regras para avançar na inscrição
            InscricaoDomainService.VerificarRegrasAvancarInscricao(inscricao);

            // Percorre os códigos encontrados no banco atualizando os que foram recebidas como parâmetro
            // e excluindo os que estão apenas no banco
            foreach (var codigo in inscricao.CodigosAutorizacao)
            {
                // Verifica se o codigo do banco foi alterado ou excluído
                InscricaoCodigoAutorizacaoVO itemParam = param.Where(c => c.Seq == codigo.Seq && c.SeqCodigoAutorizacao != default(long)).SingleOrDefault();
                if (itemParam != null)
                {
                    codigo.SeqCodigoAutorizacao = itemParam.SeqCodigoAutorizacao;
                    this.UpdateEntity(codigo);
                    param.Remove(itemParam);
                }
                else
                {
                    this.DeleteEntity(codigo);
                }
            }

            // Se sobrou algum código no parametro que não foi atualizado, inclui em banco
            foreach (var codigo in param.Where(c => c.SeqCodigoAutorizacao != default(long)).ToList())
            {
                this.InsertEntity(SMCMapperHelper.Create<InscricaoCodigoAutorizacao>(codigo));
            }
        }
    }
}