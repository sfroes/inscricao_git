using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.UI.Mvc.Html;
using SMC.GPI.Administrativo.Models;
using SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class IncluirTaxaOfertaViewModel : SMCViewModelBase, ISMCMappable
    {
        #region Datasources
        public List<SMCDatasourceItem> TaxasSelect { get; set; }

        public List<SMCDatasourceItem> EventosTaxasSelect { get; set; }

        public List<SMCDatasourceItem> ParametrosCreiSelect { get; set; }
        #endregion

        public IncluirTaxaOfertaViewModel()
        {
            Ofertas = new List<OfertaKeyValue>();
            Taxas = new SMCMasterDetailList<TaxaOfertaViewModel>();
        }

        [SMCHidden]
        public string Periodo { get; set; }

        [SMCHidden]
        public long SeqTipoTaxa { get; set; }

        [SMCHidden]
        public long SeqProcesso { get; set; }

        [SMCHidden]
        public long? SeqGrupoOferta { get; set; }

        public List<OfertaKeyValue> Ofertas { get; set; }

        [SMCDetail(SMCDetailType.Block)]
        public SMCMasterDetailList<TaxaOfertaViewModel> Taxas { get; set; }        
    }

    public class OfertaKeyValue
    {
        [SMCKey]
        [SMCHidden]
        public long Seq { get; set; }

        [SMCDescription]
        public string Descricao { get; set; }
    }
}
