using SMC.Framework.Domain;
using SMC.Framework.UnitOfWork;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Notificacoes.ServiceContract.Areas.NTF.Interfaces;
using System;

namespace SMC.Inscricoes.Domain.Areas.INS.DomainServices
{
    public class ProcessoConfiguracaoNotificacaoIdiomaDomainService : InscricaoContextDomain<ProcessoConfiguracaoNotificacaoIdioma>
    {
        #region Services

        private INotificacaoService NotificacaoService 
        {
            get { return this.Create<INotificacaoService>(); }
        }

        #endregion

        public void ExcluirConfiguracaoNotificacaoIdioma(ProcessoConfiguracaoNotificacaoIdioma processoConfiguracaoNotificacaoIdioma) 
        {
            using (var unitOfWork = SMCUnitOfWork.Begin())
            {
                try
                {
                    NotificacaoService.ExcluirConfiguracaoTipoNotificacao(processoConfiguracaoNotificacaoIdioma.SeqConfiguracaoTipoNotificacao);
                    this.DeleteEntity(processoConfiguracaoNotificacaoIdioma);
                    unitOfWork.Commit();
                }
                catch(Exception) {
                    unitOfWork.Rollback();
                    throw; 

                }

            }
        }
    }
}
