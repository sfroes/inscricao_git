using SMC.Formularios.UI.Mvc.Controls;
using SMC.Formularios.UI.Mvc.Model;
using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups;
using System.Collections.Generic;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class ExcluirConsultaInscricaoProcessoFiltroViewModel : SMCPagerViewModel, ISMCMappable
    {
        [SMCHidden]
        public long SeqProcesso { get; set; }

        [SMCHidden]
        public long? SeqInscricao { get; set; }

        [SMCHidden]
        public string NomeInscrito { get; set; }
        [SMCHidden]
        public string BackUrl { get; set; }
    }
}