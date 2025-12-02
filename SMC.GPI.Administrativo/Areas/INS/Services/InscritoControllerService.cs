using SMC.Framework.Extensions;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc.Controllers.Service;
using SMC.GPI.Administrativo.Areas.INS.Models;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMC.DadosMestres.ServiceContract.Areas.PES.Interfaces;
using SMC.DadosMestres.ServiceContract.Areas.PES.Data;
using SMC.Inscricoes.Service.Areas.INS.Services;

namespace SMC.GPI.Administrativo.Areas.INS.Services
{
    public class InscritoControllerService : SMCControllerServiceBase
    {
        #region Services
        public IInscritoService InscritoService
        {
            get { return this.Create<IInscritoService>(); }
        }
        
        public IIntegracaoDadoMestreService IntegracaoDadoMestreService
        {
            get { return this.Create<IIntegracaoDadoMestreService>(); }
        }

        private IProcessoService ProcessoService

        {
            get { return this.Create<IProcessoService>(); }
        }
        
        #endregion

        public AlterarDadosInscritoViewModel BuscarNomesDadosInscrito(long seqInscrito)
        {
            InscritoData data = InscritoService.BuscarInscrito(seqInscrito);
            var dados = data.Transform<AlterarDadosInscritoViewModel>();
            dados.SeqUsuarioSas = data.SeqUsuarioSas;
            dados.EmailConfirmacao = dados.Email;
            return dados;
        }

        public void AlterarNomeSocial(AlterarDadosInscritoViewModel modelo)
        {
            InscritoService.AlterarNomeSocial(modelo.Seq, modelo.NomeSocial);
        }

        public void AlterarInscrito(AlterarDadosInscritoViewModel modelo)
        {
            var dados = modelo.Transform<InscritoData>();

            if (modelo.Origem.HasValue)  
            {
                dados.UidProcesso = ProcessoService.BuscarProcesso(modelo.Origem.Value).UidProcesso;
            }

            InscritoService.AlterarInscrito(dados);
        }

        public void AlterarInscritoSincronizarGDM(AlterarDadosInscritoViewModel modelo)
        {
            var dados = modelo.Transform<InscritoData>();

            if (modelo.Origem.HasValue)
            {
                dados.UidProcesso = ProcessoService.BuscarProcesso(modelo.Origem.Value).UidProcesso;
            }
            
            InscritoService.AlterarInscrito(dados, true);
        }
    }
}