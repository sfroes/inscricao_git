using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using System.Collections.Generic;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class GrupoTaxaListaViewModel : SMCViewModelBase, ISMCMappable
    {
        public GrupoTaxaListaViewModel()
        {
            Itens = new List<string>();
        }

        [SMCHidden]
        public  long Seq { get; set; }

        [SMCHidden]
        public long SeqProcesso { get; set; }
        
        [SMCSize(SMCSize.Grid8_24)]
        [SMCMaxLength(100)]        
        [SMCReadOnly]
        public string Descricao { get; set; }

        [SMCSize(SMCSize.Grid2_24)]
        [SMCReadOnly]
        public short NumeroMinimoItens { get; set; }

        [SMCSize(SMCSize.Grid2_24)]
        [SMCReadOnly]       
        public short? NumeroMaximoItens { get; set; }

        //GrupoTaxaItem
        [SMCSize(SMCSize.Grid8_24)]
        [SMCMaxLength(100)]
        [SMCReadOnly]
        [SMCMapForceFromTo]
        public List<string> Itens { get; set; }

    }
}