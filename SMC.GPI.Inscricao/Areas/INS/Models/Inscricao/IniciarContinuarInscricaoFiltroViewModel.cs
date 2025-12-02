using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Inscricao.Areas.INS.Models
{
    public class IniciarContinuarInscricaoFiltroViewModel : SMCViewModelBase, ISMCMappable
    {
        public long SeqInscrito { get; set; }

        public long SeqConfiguracaoEtapa { get; set; }

        public long SeqGrupoOferta { get; set; }

        public long? SeqInscricao { get; set; }

    }
}