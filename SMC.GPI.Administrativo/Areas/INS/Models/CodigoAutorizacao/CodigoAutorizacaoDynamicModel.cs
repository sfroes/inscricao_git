using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Framework.UI.Mvc.Dynamic;
using SMC.Inscricoes.Common.Areas.INS.Constants;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using SMC.Inscricoes.ServiceContract.Areas.RES.Interfaces;
using System.Collections.Generic;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    public class CodigoAutorizacaoDynamicModel : SMCDynamicViewModel
    {
        #region [ DataSources ]
        
        [SMCDataSource("UnidadeResponsavel", valueProperty: "Seq", displayProperty:"Descricao")]
        [SMCIgnoreProp]
        [SMCServiceReference(typeof(IUnidadeResponsavelService), nameof(IUnidadeResponsavelService.BuscarUnidadesResponsaveisKeyValue))]
        public List<SMCDatasourceItem> UnidadeResponsavel { get; set; }

        [SMCDataSource("Cliente", valueProperty: "Seq", displayProperty: "Descricao")]
        [SMCIgnoreProp]
        [SMCServiceReference(typeof(IClienteService), nameof(IClienteService.BuscarClientesKeyValue))]
        public List<SMCDatasourceItem> Clientes { get; set; }

        #endregion [ DataSources ]

        [SMCFilter(true)]
        [SMCKey]
        [SMCOrder(0)]
        [SMCRequired]
        [SMCReadOnly(SMCViewMode.Edit | SMCViewMode.Insert)]
        [SMCSortable]
        [SMCSize(SMCSize.Grid2_24)]
        [SMCMaxValue(long.MaxValue)]
        public override long Seq { get; set; }

        [SMCFilter(true)]
        [SMCRequired]
        [SMCMaxLength(100)]
        [SMCSize(SMCSize.Grid12_24)]
        [SMCDescription]
        [SMCSortable(true, true)]
        public string Descricao { get; set; }

        [SMCFilter(true)]
        [SMCRequired]
        [SMCMaxLength(15)]
        [SMCSortable(true)]
        [SMCSize(SMCSize.Grid10_24)]
        public string Codigo { get; set; }        

        [SMCFilter(true)]
        [SMCSize(SMCSize.Grid8_24)]        
        [SMCSelect(nameof(Clientes), NameDescriptionField = nameof(SeqClienteNameDescriptionField))]        
        public long? SeqCliente { get; set; }

        [SMCHidden]
        [SMCMapProperty("Cliente.Nome")]
        [SMCInclude("Cliente")]
        public string SeqClienteNameDescriptionField { get; set; }

        [SMCFilter(true)]
        [SMCRequired]
        [SMCSize(SMCSize.Grid8_24)]        
        [SMCSelect(nameof(UnidadeResponsavel), NameDescriptionField = nameof(NomeUnidadeResponsavel))]
        public long SeqUnidadeResponsavel { get; set; }

        [SMCHidden]
        [SMCMapProperty("UnidadeResponsavel.Nome")]
        [SMCInclude("UnidadeResponsavel")]
        public string NomeUnidadeResponsavel { get; set; }

        #region [ Configuração ]

        public override void ConfigureDynamic(ref SMCDynamicOptions options)
        {
            options.Tokens(tokenInsert: UC_INS_001_07_01.MANTER_CODIGO_AUTORIZACAO,
                           tokenEdit: UC_INS_001_07_01.MANTER_CODIGO_AUTORIZACAO,
                           tokenRemove: UC_INS_001_07_01.MANTER_CODIGO_AUTORIZACAO,
                           tokenList: UC_INS_001_07_01.MANTER_CODIGO_AUTORIZACAO)
                           .Service<ICodigoAutorizacaoService>(save: nameof(ICodigoAutorizacaoService.SalvarCodigoAutorizacao));
        }

        #endregion [ Configuração ]
    }
}