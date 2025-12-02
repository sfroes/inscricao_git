using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc.Controllers.Service;
using SMC.GPI.Administrativo.Areas.INS.Models;
using SMC.Inscricoes.Service.Areas.INS.Services;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;

namespace SMC.GPI.Administrativo.Areas.INS.Services
{
    public class ScanQrCodeControllerService : SMCControllerServiceBase
    {
        private IInscricaoService InscricaoService => Create<IInscricaoService>();


        #region Listar Inscritos

        public string BuscarNomeInscritosSeqInscricao(long seqInscricao)
        {
            return InscricaoService.BuscarNomeInscritosSeqInscricao(seqInscricao);
        }
        #endregion
    }
}