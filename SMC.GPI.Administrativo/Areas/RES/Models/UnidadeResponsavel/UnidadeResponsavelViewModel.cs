using Newtonsoft.Json;
using SMC.EstruturaOrganizacional.UI.Mvc.Areas.ESO.Lookups;
using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.UI.Mvc.Html;
using SMC.GPI.Administrativo.Models;
using SMC.Inscricoes.Common.Areas.RES.Enums;
using SMC.Localidades.Common.Areas.LOC.Enums;
using SMC.Localidades.UI.Mvc.DataAnnotation;
using SMC.Localidades.UI.Mvc.Models;
using System.Collections.Generic;

namespace SMC.GPI.Administrativo.Areas.RES.Models
{
    public class UnidadeResponsavelViewModel : SMCViewModelBase, ISMCMappable
    {
        #region DataSources

        [JsonIgnore]
        public List<SMCDatasourceItem> TiposTelefone { get; set; } = new List<SMCDatasourceItem>();

        public List<SMCDatasourceItem> UnidadesResponsaveisNotificacao { get; set; }

        [SMCDataSource]
        [JsonIgnore]
        public List<SMCDatasourceItem> UnidadesResponsaveisSGF { get; set; }

        #endregion

        [SMCReadOnly]
        [SMCSize(SMCSize.Grid3_24)]
        public long Seq { get; set; }

        [SMCRequired]
        [SMCSize(SMCSize.Grid8_24)]
        [SMCMaxLength(100)]
        public string Nome { get; set; }

        [SMCSize(SMCSize.Grid3_24)]
        [SMCMaxLength(15)]
        public string Sigla { get; set; }

        [SMCSize(SMCSize.Grid10_24)]
        [UnidadeLookup]
        [SMCRequired]
        public UnidadeLookupViewModel CodigoUnidade { get; set; }

        [SMCSize(SMCSize.Grid7_24)]
        [SMCSelect("UnidadesResponsaveisNotificacao")]
        [SMCRequired]
        public long? SeqUnidadeResponsavelNotificacao { get; set; }

        [SMCSize(SMCSize.Grid7_24)]
        [SMCSelect("UnidadesResponsaveisSGF")]
        public long? SeqUnidadeResponsavelSgf { get; set; }

        [SMCSize(SMCSize.Grid5_24)]
        [SMCSelect]
        [SMCRequired]
        public TipoUnidadeResponsavel TipoUnidadeResponsavel { get; set; }

        [SMCSize(SMCSize.Grid5_24)]
        [SMCMaxLength(100)]
        public string NomeContato { get; set; }

        [SMCSize(SMCSize.Grid24_24)]
        [SMCDetail(SMCDetailType.Tabular, min: 1)]
        [SMCMapForceFromTo]
        public SMCMasterDetailList<EnderecoEletronicoViewModel> EnderecosEletronicos { get; set; }

        [SMCSize(SMCSize.Grid24_24)]
        [Address(max: 1, min: 1, HideMasterDetailButtons = true, TiposEnderecos = new TipoEndereco[] { TipoEndereco.Comercial })]
        [SMCMapForceFromTo]
        public AddressList Enderecos { get; set; }

        [SMCSize(SMCSize.Grid24_24)]
        [Phone(min: 1)]
        [SMCMapForceFromTo]
        public PhoneList Telefones { get; set; }

        [SMCSize(SMCSize.Grid24_24)]
        [SMCDetail]
        [SMCMapForceFromTo]
        public SMCMasterDetailList<UnidadeResponsavelCentroCustoViewModel> CentrosCusto { get; set; }


        [SMCSize(SMCSize.Grid24_24)]
        [SMCSelect("SistemaOrigemGADSelect")]
        public string TokenSistemaOrigemGad { get; set; }
        public List<SMCDatasourceItem<string>> SistemaOrigemGADSelect { get; set; }


        public override void ConfigureNavigation(ref SMCNavigationGroup navigationGroup)
        {
            navigationGroup = new UnidadeResponsavelNavigationGroup(this);
        }
    }
}