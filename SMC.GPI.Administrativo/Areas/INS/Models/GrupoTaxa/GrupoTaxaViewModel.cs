using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.UI.Mvc.Html;
using System.Collections.Generic;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class GrupoTaxaViewModel : SMCViewModelBase, ISMCMappable
    {
        public GrupoTaxaViewModel()
        {
            Taxas = new List<SMCDatasourceItem>();
            Itens = new SMCMasterDetailList<GrupoTaxaDetalheViewModel>();
        }

        public CabecalhoProcessoViewModel Cabecalho { get; set; }
                
        [SMCHidden]
        public long? Seq { get; set; }

        [SMCHidden]        
        public long SeqProcesso { get; set; }

        [SMCRequired]        
        [SMCSize(SMCSize.Grid10_24)]
        [SMCMaxLength(100)]
        public string Descricao { get; set; }

        [SMCRequired]
        [SMCSize(SMCSize.Grid7_24)]        
        public short? NumeroMinimoItens { get; set; }

        [SMCSize(SMCSize.Grid7_24)]
        [SMCMinValue(nameof(NumeroMinimoItens))]
        public short? NumeroMaximoItens { get; set; }

        //Taxas do Processo que fazem parte de um Grupo de taxas
        #region TaxasProcessoGrupoTaxas                
        public List<SMCDatasourceItem> Taxas { get; set; }

        //Taxas
        [SMCMapForceFromTo]
        [SMCDetail(SMCDetailType.Tabular)]
        public SMCMasterDetailList<GrupoTaxaDetalheViewModel> Itens { get; set; }

        #endregion
    }
}