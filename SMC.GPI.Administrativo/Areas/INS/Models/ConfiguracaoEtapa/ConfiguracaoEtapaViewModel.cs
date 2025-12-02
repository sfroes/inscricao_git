using Newtonsoft.Json;
using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.UI.Mvc.Html;
using SMC.GPI.Administrativo.Models;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using SMC.Inscricoes.UI.Mvc.Areas.INS.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class  ConfiguracaoEtapaViewModel : SMCViewModelBase, ISMCMappable
    {
        public ConfiguracaoEtapaViewModel()
        {
            GruposOfertaSelect = new List<SMCDatasourceItem>();
            GruposOferta = new SMCMasterDetailList<GrupoOfertaDetalheViewModel>();
            ExigeJustificativaOferta = false;
            PermiteNovaEntregaDocumentacao = false;
        }

        [SMCKey]
        [SMCHidden]
        public long Seq { get; set; }
        
        [SMCHidden]
        [SMCSize(SMCSize.Grid4_24)]
        public long SeqEtapaProcesso { get; set; }

        [SMCHidden]
        [SMCSize(SMCSize.Grid4_24)]
        public long SeqProcesso { get; set; }
        
        [SMCSize(SMCSize.Grid24_24)]
        [SMCMaxLength(255)]
        [SMCRequired]
        public string Nome { get; set; }        

        [SMCRequired]
        [SMCDateTimeMode(SMCDateTimeMode.DateTime)]
        [SMCSize(SMCSize.Grid6_24)]
        public DateTime DataInicio { get; set; }

        [SMCRequired]
        [SMCDateTimeMode(SMCDateTimeMode.DateTime)]
        [SMCMinDate("DataInicio")]
        [SMCSize(SMCSize.Grid6_24)]
        public DateTime DataFim { get; set; }

        [SMCSize(SMCSize.Grid6_24)]
        [SMCMaxLength(255)]
        public string NumeroEdital { get; set; }

        [SMCDateTimeMode(SMCDateTimeMode.DateTime)]
        [SMCSize(SMCSize.Grid4_24)]
        public DateTime? DataLimiteEntregaDocumentacao { get; set; }

        [SMCSize(SMCSize.Grid24_24)]
        [SMCHtml]
        public string DescricaoEntregaDocumentacao { get; set; }

        [SMCSize(SMCSize.Grid24_24)]
        [SMCHtml]
        public string DescricaoTermoEntregaDocumentacao { get; set; }

        [SMCSize(SMCSize.Grid12_24)]
        [SMCRadioButtonList]
        public bool ExisteNumeroMaximoPorOferta
        {
            get
            {
                if (Seq > 0)
                    return NumeroMaximoOfertaPorInscricao.HasValue;
                else
                    return true;
            }
            set { }
        }

        [SMCSize(SMCSize.Grid12_24)]
        [SMCConditionalReadonly("ExisteNumeroMaximoPorOferta", false)]
        [SMCConditionalRequired("ExisteNumeroMaximoPorOferta", true)]
        [SMCMinValue(1)]
        [SMCMaxValue(99)]
        public short? NumeroMaximoOfertaPorInscricao { get; set; }

        [SMCSize(SMCSize.Grid12_24)]
        [SMCRadioButtonList]
        [SMCRequired]
        [SMCMapForceFromTo]
        public bool? ExigeJustificativaOferta { get; set; }

        [SMCSize(SMCSize.Grid12_24)]
        [SMCRadioButtonList]
        [SMCRequired]
        [SMCMapForceFromTo]
        public bool? PermiteNovaEntregaDocumentacao { get; set; }

        [SMCSize(SMCSize.Grid12_24)]
        [SMCRadioButtonList]
        [SMCConditionalReadonly("NumeroMaximoOfertaPorInscricao", 1)]
        [SMCConditionalRequired("NumeroMaximoOfertaPorInscricao", SMCConditionalOperation.NotEqual, 1)]
        public bool? DeveExigirInformarNumeroMaximoConvocacao
        {
            get
            {
                if (Seq > 0)
                    return NumeroMaximoConvocacaoPorInscricao.HasValue;
                return null;
            }
            set { }
        }

        [SMCSize(SMCSize.Grid12_24)]
        [SMCMinValue(2)]
        [SMCMaxValue("NumeroMaximoOfertaPorInscricao")]
        [SMCMask("99")]
        [SMCConditionalReadonly("DeveExigirInformarNumeroMaximoConvocacao", SMCConditionalOperation.NotEqual, "true")]
        [SMCConditionalRequired("DeveExigirInformarNumeroMaximoConvocacao", "true")]                
        public short? NumeroMaximoConvocacaoPorInscricao { get; set; }        
        
        [SMCMapForceFromTo]
        [SMCDetail(SMCDetailType.Tabular)]
        public SMCMasterDetailList<GrupoOfertaDetalheViewModel> GruposOferta { get; set; }

        #region Datasources

        public List<SMCDatasourceItem> GruposOfertaSelect { get; set; }

        #endregion
    }
}