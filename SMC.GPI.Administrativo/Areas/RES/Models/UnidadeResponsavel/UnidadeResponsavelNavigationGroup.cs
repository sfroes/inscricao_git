using SMC.Framework.UI.Mvc;
using SMC.Inscricoes.Common.Areas.RES.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.RES.Models
{
    public class UnidadeResponsavelNavigationGroup : SMCNavigationGroup
    {
        public UnidadeResponsavelNavigationGroup(SMCViewModelBase model) 
            : base(model)
        {
            AddItem("GRUPO_ConfigurarUnidadeResponsavel",
                    "Configurar",
                    "UnidadeResponsavel",
                    new string[] { UC_RES_001_01_03.PESQUISAR_CONFIGURACAO_UNIDADE_RESPONSAVEL },
                    parameters: new SMCNavigationParameter("seq", "Seq"))
                    .HideForModel<UnidadeResponsavelViewModel>(Framework.SMCViewMode.Insert);
        }
    }
}