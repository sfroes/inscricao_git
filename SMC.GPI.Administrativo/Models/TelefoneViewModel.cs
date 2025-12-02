// Substituido pela classe de Telefone do Localidades

/*using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework;
using SMC.Framework.DataAnnotations;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc;
using SMC.Inscricoes.Common.Areas.RES;
using System;
using System.Collections.Generic;

namespace SMC.GPI.Administrativo.Models
{
    public class TelefoneViewModelX : SMCViewModelBase, ISMCMappable
    {
        public TelefoneViewModelX()
        {
            this.CodigoPais = 55;
        }

        [SMCHidden]
        public long? Seq { get; set; }

        // Deixar com o tipo de dados short, pois as opções do select são filtradas. Se alterar para o Enum, o filtro não funciona.
        [SMCSize(SMCSize.Grid4_24)]
        [SMCRequired]
        [SMCSelect("TiposTelefone")]
        public short TipoTelefone { get; set; }

        [SMCSize(SMCSize.Grid4_24)]
        [SMCRequired]
        [SMCMask("099")]
        [SMCMapForceFromTo]
        public int CodigoPais { get; set; }

        [SMCSize(SMCSize.Grid4_24)]
        [SMCRequired]
        [SMCMask("099")]
        public int? CodigoArea { get; set; }

        [SMCSize(SMCSize.Grid6_24)]
        [SMCRequired]
        [SMCMaxLength(10)]
        [SMCPhone]
        public string Numero { get; set; }

        [SMCHidden()]
        public TipoTelefone TipoTelefoneTipado { get { return (TipoTelefone)TipoTelefone; } }

    }
}*/