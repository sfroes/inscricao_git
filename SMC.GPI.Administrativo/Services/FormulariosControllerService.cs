using SMC.Formularios.UI.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMC.Framework.Mapper;
using SMC.Formularios.Service.FRM;
using SMC.Framework.UI.Mvc.Controllers.Service;

namespace SMC.GPI.Administrativo.Services
{
    public class FormulariosControllerService : SMCControllerServiceBase, IFormulariosControllerService
    {

        #region Services

        private IFormularioService FormularioService
        {
            get { return this.Create<IFormularioService>(); }
        }

        #endregion

        /// <summary>
        /// Busca as informações de um formulário
        /// </summary>
        /// <param name="seqFormulario">Sequencial do Formulário</param>
        /// <returns>Configuração do formulario</returns>
        public FormularioViewModel BuscarFormulario(long seqFormulario)
        {
            return SMCMapperHelper.Create<FormularioViewModel>(FormularioService.BuscarFormulario(seqFormulario));
        }
    }
}