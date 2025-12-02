using System.Collections.Generic;
using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.GPI.Administrativo.Models;
using SMC.Localidades.UI.Mvc.Models;
using SMC.Framework.Model;
using Newtonsoft.Json;
using SMC.Framework.UI.Mvc.Html;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Localidades.UI.Mvc.DataAnnotation;
using SMC.Localidades.Common.Areas.LOC.Enums;
using System;

namespace SMC.GPI.Administrativo.Areas.RES.Models
{
    public class ClienteViewModel : SMCViewModelBase, ISMCMappable
    {
        public ClienteViewModel()
        {
            this.Enderecos = new AddressList();
            this.Telefones = new PhoneList();
            this.TiposTelefone = new List<SMCDatasourceItem>();
            this.EnderecosEletronicos = new SMCMasterDetailList<EnderecoEletronicoViewModel>();
        }

        [SMCFilter] 
        [SMCOrder(0)]
        [SMCReadOnly] 
        [SMCSortable(true)]
        [SMCSize(SMCSize.Grid4_24)] 
        public long Seq { get; set; }

        [SMCSize(SMCSize.Grid16_24)]
        [SMCMaxLength(100)]
        [SMCRequired]
        public string Nome { get; set; }

        [SMCFilter]
        [SMCSize(SMCSize.Grid4_24)]
        [SMCMaxLength(15)]
        public string Sigla { get; set; }

        [SMCSize(SMCSize.Grid10_24)]
        [SMCMaxLength(100)]
        public string NomeFantasia { get; set; }

        [SMCSize(SMCSize.Grid10_24)]
        [SMCMaxLength(100)]
        [SMCRequired]
        public string RazaoSocial { get; set; }

        [SMCCnpj]
        [SMCRequired]
        [SMCSize(SMCSize.Grid4_24)]
        public string Cnpj { get; set; }

        [Address(max: 1, TiposEnderecos = new TipoEndereco[] { TipoEndereco.Comercial })]
        [SMCMapForceFromTo]
        public AddressList Enderecos { get; set; }

        [Phone]
        [SMCMapForceFromTo]
        public PhoneList Telefones { get; set; }

        [JsonIgnore]
        public List<SMCDatasourceItem> TiposTelefone { get; set; }

        [SMCDetail]
        [SMCMapForceFromTo]
        public SMCMasterDetailList<EnderecoEletronicoViewModel> EnderecosEletronicos { get; set; }
    }
}