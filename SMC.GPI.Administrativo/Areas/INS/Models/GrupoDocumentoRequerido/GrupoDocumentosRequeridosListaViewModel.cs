using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class GrupoDocumentosRequeridosListaViewModel : SMCViewModelBase, ISMCMappable
    {
        public GrupoDocumentosRequeridosListaViewModel()
        {
            Itens = new List<string>();
        }
                
        public long Seq { get; set; }  
        
        public long SeqConfiguracaoEtapa { get; set; }
        
        public string Descricao { get; set; }

        public int MinimoObrigatorio { get; set; }

        public bool UploadObrigatorio { get; set; }

        [SMCMapForceFromTo]
        public List<string> Itens { get; set; }
    }
}