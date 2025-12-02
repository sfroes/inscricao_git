using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using System;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class EtapaProcessoListaViewModel : SMCViewModelBase, ISMCMappable
    {
        [SMCHidden]
        public long Seq { get; set; }

        [SMCHidden]
        public long SeqProcesso { get; set; }

        [SMCSortable]
        public string DescricaoEtapaSGF { get; set; }
                
        [SMCSortable]
        [SMCDateTimeMode(SMCDateTimeMode.DateTime)]
        public DateTime DataInicioEtapa { get; set; }

        [SMCSortable]
        [SMCDateTimeMode(SMCDateTimeMode.DateTime)]
        public DateTime DataFimEtapa { get; set; }

        [SMCSortable]
        public SituacaoEtapa SituacaoEtapa { get; set; }
    }
}