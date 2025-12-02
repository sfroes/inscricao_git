using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Areas.INS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    /// <summary>
    /// Value Objec para representar uma inscrição em um processo com sua situação
    /// usado para listagem de situção de inscrição no processo
    /// </summary>
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class InscricaoDocumentoUploadData : ISMCMappable
    {
        [DataMember]
        public long Seq { get; set; }  

        [DataMember]
        public long SeqDocumentoRequerido { get; set; }

        [DataMember]
        public long SeqTipoDocumento { get; set; }

        [DataMember]
        public string DescricaoTipoDocumento { get; set; }

    }
}
