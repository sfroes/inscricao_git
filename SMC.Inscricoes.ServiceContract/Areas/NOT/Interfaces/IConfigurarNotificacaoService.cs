using SMC.Framework.Model;
using SMC.Framework.Service;
using SMC.Inscricoes.ServiceContract.Areas.NOT.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.NOT.Interfaces
{
    public interface IConfigurarNotificacaoService : ISMCService
    {
        SMCPagerData<ConfigurarNotificacaoListaData> BuscarConfiguracoesNotificacao(ConfigurarNotificacaoFiltroData filtro);

        ConfigurarNotificacaoData BuscarConfiguracaoNotificacao(long seqConfiguracaoNotificacao);

        long SalvarConfiguracaoNotificacao(ConfigurarNotificacaoData configurarNotificacaoData);

        void VerificaPermissaoAlteracao(long seqProcesso);

        void ValidarTipoNotificacao(ConfigurarNotificacaoData configurarNotificacaoData);

        void Excluir(long seqConfiguracaoNotificacao);

        ProcessoConfiguracaoNotificacaoData BuscarParametrosNotificacao(long seqConfiguracaoNotificacao);

        void SalvarParametrosNotificacao(ProcessoConfiguracaoNotificacaoData processoConfiguracaoNotificacaoData);
    }
}
