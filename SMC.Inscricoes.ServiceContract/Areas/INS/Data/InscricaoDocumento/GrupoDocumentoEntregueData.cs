using SMC.Framework.Mapper;
using System.Collections.Generic;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    /// <summary>
    /// Value Objec para representar uma inscrição em um processo com sua situação
    /// usado para listagem de situção de inscrição no processo
    /// </summary>
    public class GrupoDocumentoEntregueData : ISMCMappable
    {
        public string Descricao { get; set; }

        public long MinimoObrigatorio { get; set; }

        public List<DocumentoRequeridoData> DocumentosRequeridosGrupo { get; set; }
    }
}

