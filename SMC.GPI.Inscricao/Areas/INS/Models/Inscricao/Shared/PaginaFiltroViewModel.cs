using SMC.Framework;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Inscricao.Areas.INS.Models
{
    public class PaginaFiltroViewModel : SMCViewModelBase, ISMCMappable
    {
        [SMCHidden]
        public long SeqConfiguracaoEtapaPagina { get; set; }

        [SMCHidden]
        public long SeqConfiguracaoEtapa { get; set; }

        [SMCHidden]
        public long SeqGrupoOferta { get; set; }

        [SMCHidden]
        public long SeqInscricao { get; set; }

        [SMCHidden]
        public SMCLanguage Idioma { get; set; }

        [SMCHidden]
        public Guid UidProcesso { get; set; }

        [SMCHidden]
        public string TokenAngular { get; set; }

    }
}