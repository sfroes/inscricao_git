using SMC.DadosMestres.Common.Constants;
using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.GPI.Administrativo.Areas.RES.Views.UnidadeResponsavel.PartialsParametros.App_LocalResources;
using SMC.Inscricoes.ServiceContract.Areas.RES.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SMC.GPI.Administrativo.Areas.RES.Models.UnidadeResponsavel
{
    public class UnidadeResponsavelTipoProcessoIdentidadeVisualViewModel : SMCViewModelBase, ISMCMappable
    {
        public UnidadeResponsavelTipoProcessoIdentidadeVisualViewModel()
        {
            this.Ativo = true;
        }


        [Key]
        [SMCHidden]
        public long Seq { get; set; }

        [Required]
        [SMCHidden]
        public long SeqUnidadeResponsavelTipoProcesso { get; set; }

        [SMCRequired]
        [SMCSize(SMCSize.Grid8_24)]
        public string Descricao { get; set; }

        [SMCRequired]
        [SMCSize(SMCSize.Grid8_24)]
        [SMCMaxLength(255)]
        public string CssAplicacao { get; set; }

        [SMCRegularExpression(REGEX.TOKEN, FormatErrorResourceKey = nameof(MetadataResource.MSG_Token_Expression_Error))]
        [SMCSize(SMCSize.Grid8_24)]
        [SMCMaxLength(30)]
        public string TokenCssAlternativoSas { get; set; }

        [SMCRequired]
        [SMCSize(SMCSize.Grid12_24)]
        [SMCSelect("LayoutMensagemEmail")]
        public long SeqLayoutMensagemEmail { get; set; }

        [SMCRequired]
        [SMCSize(SMCSize.Grid6_24)]
        [SMCRadioButtonList]
        [SMCMapForceFromTo]
        public bool? Ativo { get; set; }
    }
}