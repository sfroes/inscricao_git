using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.UI.Mvc.Html;
using SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class GrupoOfertaViewModel : SMCViewModelBase, ISMCMappable
    {
        public GrupoOfertaViewModel()
        {
            Ofertas = new List<ItemGrupoOfertaViewModel>();
        }

        public CabecalhoProcessoViewModel Cabecalho { get; set; }

        [SMCKey]
        [SMCReadOnly]
        [SMCSize(SMCSize.Grid4_24)]
        public long? Seq { get; set; }

        [SMCHidden]
        [SMCSize(SMCSize.Grid4_24)]
        public long SeqProcesso { get; set; }

        [SMCRequired]
        [SMCSize(SMCSize.Grid10_24)]
        public string Descricao { get; set; }

        //Dependency para o filtro do lookup
        [SMCHidden]
        public long? SeqGrupoOferta
        {
            get { return Seq; }
        }

        [SMCMapForceFromTo]
        [LookupSelecaoItemGrupoOferta]
        [SMCDependency("SeqGrupoOferta")]
        [SMCDependency("SeqProcesso")]
        [SMCSize(SMCSize.Grid24_24)]
        public List<ItemGrupoOfertaViewModel> Ofertas { get; set; }
    }
}