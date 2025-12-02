using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using SMC.Inscricoes.Service.Data;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data.TipoDocumento;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class TipoProcessoCampoInscritoData : ISMCMappable
    {
        public long Seq { get; set; }
        public long SeqTipoProcesso { get; set; }
        public CampoInscrito CampoInscrito { get; set; }        
    }
}
