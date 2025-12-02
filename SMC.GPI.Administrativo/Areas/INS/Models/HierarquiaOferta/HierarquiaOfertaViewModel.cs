using Newtonsoft.Json;
using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.Html;
using SMC.GPI.Administrativo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class HierarquiaOfertaViewModel : SMCViewModelBase, ISMCMappable
    {
        public long SeqProcesso { get; set; }

        public bool ExibeArvoreFechada { get; set; }

        public bool RaizPermiteOferta { get; set; }

        public bool RaizPermiteItemFilho { get; set; }   
        
    }
}