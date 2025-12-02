using SMC.Framework.Model;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using SMC.Inscricoes.Domain.Areas.INS.ValueObjects;
using System.Linq;

namespace SMC.Inscricoes.Domain.Areas.INS.DomainServices
{
    public class InscritoAtividadeRelatorioDomainService : InscricaoContextDomain<InscricaoOferta>
    {
        private ProcessoDomainService ProcessoDomainService
        {
            get { return this.Create<ProcessoDomainService>(); }
        }
        private OfertaDomainService OfertaDomainService
        {
            get { return this.Create<OfertaDomainService>(); }
        }

        /// <summary>
        /// Busca Inscritos nas Atividades
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns></returns>
        public SMCPagerData<InscritoAtividadeRelatorioVO> BuscarInscritosAtividades(InscritoAtividadeRelatorioFilterSpecification filtro)
        {

            var situacoesTemplateProcesso = ProcessoDomainService.BuscarSituacoesProcesso(filtro.SeqProcesso.Value);
            var situacaoInscricao = situacoesTemplateProcesso.Contains(TOKENS.SITUACAO_INSCRICAO_DEFERIDA) ? TOKENS.SITUACAO_INSCRICAO_DEFERIDA : TOKENS.SITUACAO_INSCRICAO_CONFIRMADA;
            var total = 0;

            filtro.TokenHistoricoSituacao = situacaoInscricao;
            filtro.HistoricoInscricaoAtual = true;

            var inscritos = SearchProjectionBySpecification(filtro, insc => new InscritoAtividadeRelatorioVO()
            {
                SeqOferta = insc.SeqOferta,
                Oferta = insc.Oferta,
                ExibirPeriodoOferta = insc.Oferta.Processo.ExibirPeriodoAtividadeOferta,
                SeqProcesso = insc.Inscricao.SeqProcesso,
                DescricaoProcesso = insc.Inscricao.Processo.Descricao,
                NumeroInscricao = insc.Inscricao.Seq.ToString(),
                NomeInscrito = insc.Inscricao.Inscrito.Nome,

            }, out total).ToList();

            foreach (var item in inscritos)
            {
                OfertaDomainService.AdicionarDescricaoCompleta(item.Oferta, item.ExibirPeriodoOferta, false);
                item.DescricaoOferta = item.Oferta.DescricaoCompleta;
            }

            inscritos.OrderBy(x => x.DescricaoOferta).ThenBy(x => x.NomeInscrito);

            return new SMCPagerData<InscritoAtividadeRelatorioVO>(inscritos, total);
        }

    }
}
