using SMC.Formularios.ServiceContract.Areas.FRM.Data;
using SMC.Formularios.ServiceContract.Areas.FRM.Interfaces;
using SMC.Formularios.ServiceContract.Areas.TMP.Interfaces;
using SMC.Framework.Extensions;
using SMC.Framework.Model;
using SMC.Framework.Service;
using SMC.Inscricoes.Domain.Areas.INS.DomainServices;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using System.Linq;

namespace SMC.Inscricoes.Service.Areas.FRM
{
    public class ValorElementoService : SMCServiceBase, IValorElementoService
    {
        private ISituacaoService SituacaoService => this.Create<ISituacaoService>();

        private InscricaoDadoFormularioCampoDomainService InscricaoDadoFormularioCampoDomainService => Create<InscricaoDadoFormularioCampoDomainService>();

        public DadoCampoLookupData BuscarDadoCampo(string valor)
        {
            valor = valor.Replace("%%", ";");
            return new DadoCampoLookupData { Valor = valor, Descricao = valor?.Split('|').LastOrDefault() };
        }

        public SMCPagerData<DadoCampoLookupData> BuscarDadosCampo(DadoFormularioCampoFiltroData filtro)
        {
            // Busca qual o sequencial do motivo de token INSCRICAO_CANCELADA_TESTE
            var seqsMotivos = SituacaoService.BuscarSeqMotivosSituacaoPorToken("INSCRICAO_CANCELADA_TESTE");

            var spec = filtro.Transform<InscricaoDadoFormularioCampoFilterSpecification>();
            spec.SeqsMotivosDiferente = seqsMotivos;

            return InscricaoDadoFormularioCampoDomainService.BuscarValoresElemento(spec).Transform<SMCPagerData<DadoCampoLookupData>>();
        }
    }
}