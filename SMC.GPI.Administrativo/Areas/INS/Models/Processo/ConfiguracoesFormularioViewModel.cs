using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class ConfiguracoesFormularioViewModel : SMCViewModelBase, ISMCMappable
    {
        [SMCHidden]
        public long Seq { get; set; }

        [SMCRequired]
        [SMCDependency("SeqUnidadeResponsavel", "BuscarTiposFormularioAssociadosSelect", "Processo", true)]
        [SMCSelect("TiposFormulario")]
        [SMCSize(SMCSize.Grid10_24)]
        public long SeqTipoFormularioSgf { get;set;}

        [SMCRequired]
        [SMCSelect("Formularios")]
        [SMCDependency("SeqTipoFormularioSgf", "BuscarFormulariosSelect", "Processo", true)]
        [SMCSize(SMCSize.Grid10_24)]
        public long SeqFormularioSgf { get; set; }

        [SMCRequired]
        [SMCSelect(("VisoesFormulario"), AutoSelectSingleItem = true)]
        [SMCDependency("SeqTipoFormularioSgf", "BuscarVisoesSelect", "Processo", true)]
        [SMCSize(SMCSize.Grid4_24)]
        public long SeqVisaoSgf { get; set; }

        [SMCRequired]
        [SMCSize(SMCSize.Grid10_24)]
        public string Descricao { get; set; }

        [SMCRadioButtonList]
        [SMCSize(SMCSize.Grid6_24)]
        [SMCRequired]
        public bool DisponivelApenasComCheckin { get; set; }

        [SMCRequired]
        [SMCSize(SMCSize.Grid4_24)]
        [SMCDateTimeMode(SMCDateTimeMode.Date)]
        public DateTime? DataInicioFormulario { get; set; }

        [SMCSize(SMCSize.Grid4_24)]
        [SMCMinDate(nameof(DataInicioFormulario))]
        [SMCDateTimeMode(SMCDateTimeMode.Date)]
        public DateTime? DataFimFormulario { get; set; }

        [SMCMultiline]
        [SMCRequired]
        [SMCSize(SMCSize.Grid24_24)]
        public string Mensagem { get; set; }
    }
}