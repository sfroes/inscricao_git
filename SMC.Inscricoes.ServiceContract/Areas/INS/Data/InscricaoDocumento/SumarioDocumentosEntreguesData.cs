using SMC.Framework.Mapper;
using SMC.Framework.Model;
using System;
using System.Collections.Generic;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    /// <summary>
    /// Value Objec para representar uma inscrição em um processo com sua situação
    /// usado para listagem de situção de inscrição no processo
    /// </summary>
    public class SumarioDocumentosEntreguesData : ISMCMappable
    {
        public long SeqInscricao { get; set; }

        public bool InscricaoConfirmada { get; set; }

        public List<DocumentoRequeridoData> DocumentosObrigatorios { get; set; }

        public List<GrupoDocumentoEntregueData> GruposDocumentos { get; set; }

        public List<DocumentoRequeridoData> DocumentosOpcionais { get; set; }

        public DateTime? DataPrazoNovaEntregaDocumentacao { get; set; }
    }
}

