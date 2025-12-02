using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using SMC.Inscricoes.Common.Areas.NOT.Enums;

namespace SMC.GPI.Administrativo.Areas.NOT.Models
{
    public class ParametrosNotificacaoItemViewModel : SMCViewModelBase
    {
        public ParametrosNotificacaoItemViewModel()
        {
            Ativo = true;
        }

        [SMCHidden]
        [SMCKey]
        public long Seq { get; set; }

        [SMCSize(SMCSize.Grid3_24)]
        [SMCMinValue(0)]
        [SMCMaxValue(10000)]
        [SMCMask("99999")]
        [SMCRequired]
        public short? QuantidadeDiasInicioEnvio { get; set; }

        [SMCSize(SMCSize.Grid2_24)]
        [SMCSelect]
        [SMCRequired]
        public Temporalidade Temporalidade { get; set; }

        [SMCSize(SMCSize.Grid4_24)]
        [SMCSelect("AtributosDisponiveis")]
        [SMCRequired]
        public AtributoAgendamento AtributoAgendamento { get; set; }

        [SMCSize(SMCSize.Grid4_24)]
        [SMCMinValue(0)]
        [SMCMaxValue(10000)]
        [SMCMask("99999")]
        public short? QuantidadeDiasRecorrencia { get; set; }

        [SMCSize(SMCSize.Grid6_24)]
        [SMCSelect]
        [SMCConditionalReadonly("QuantidadeDiasRecorrencia", "0", "")]
        [SMCConditionalRequired("QuantidadeDiasRecorrencia", SMCConditionalOperation.NotEqual, "0", "")]
        public bool? ReenviarNotificacaoInscrito { get; set; }

        [SMCSize(SMCSize.Grid3_24)]
        [SMCSelect]
        [SMCMapForceFromTo]
        [SMCRequired]
        public bool Ativo { get; set; }
    }
}