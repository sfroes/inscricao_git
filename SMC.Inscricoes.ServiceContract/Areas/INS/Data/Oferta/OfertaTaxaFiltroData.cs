using System;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using SMC.Framework;
using SMC.Framework.Audit;
using SMC.Framework.Collections;
using SMC.Framework.Mapper;
using System.Collections.Generic;
using SMC.Inscricoes.Common;
using SMC.Framework.Model;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    /// <summary>
    /// Value Objec para representar a posição consolidade de um processo
    /// </summary>
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class OfertaTaxaFiltroData : SMCPagerFilterData, ISMCMappable
    {      
        public long? SeqTipoTaxa { get; set; }

        public long? SeqGrupoOferta { get; set; }

        [SMCMapProperty("Oferta.Seq")]
        public long? SeqOferta { get; set; }

        public bool? Ativo { get; set; }

        public bool? Vigente { get; set; }

        public long? SeqProcesso { get; set; }
    }
}
