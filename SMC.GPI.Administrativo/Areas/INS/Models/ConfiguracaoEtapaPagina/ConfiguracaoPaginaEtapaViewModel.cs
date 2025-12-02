using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.UI.Mvc.Html;
using SMC.GPI.Administrativo.Areas.RES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class ConfiguracaoPaginaEtapaViewModel : SMCViewModelBase, ISMCMappable
    {
        public ConfiguracaoPaginaEtapaViewModel()
        {
            ItensConfiguracaoPagina = new List<SMCTreeViewNode<ArvoreItemConfiguracaoPaginaEtapaViewModel>>();
            Cabecalho = new CabecalhoProcessoEtapaViewModel();
        }
        
        public CabecalhoProcessoEtapaViewModel Cabecalho { get; set; }

        [SMCHidden]
        public long SeqProcesso { get; set; }

        [SMCHidden]
        public long SeqEtapa { get; set; }

        [SMCHidden]
        public long SeqConfiguracaoEtapa { get; set; }

        public bool PossuiPaginas { get; set; }

        public List<SMCTreeViewNode<ArvoreItemConfiguracaoPaginaEtapaViewModel>> ItensConfiguracaoPagina { get; set; }
        
    }
}