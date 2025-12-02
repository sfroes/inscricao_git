using SMC.Framework.Model;
using SMC.Inscricoes.Common;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    /// <summary>
    /// Value Objec para representar uma inscrição em um processo com sua situação
    /// usado para listagem de situção de inscrição no processo
    /// </summary>
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class InscricaoDocumentoGrupoUploadData : InscricaoDocumentoData
    {
        public string DescricaoGrupoDocumentos { get; set; }

        public long SeqGrupoDocumentoRequerido { get; set; }

        public List<SMCDatasourceItem> DocumentosRequeridosGrupo { get; set; }

    }
}
