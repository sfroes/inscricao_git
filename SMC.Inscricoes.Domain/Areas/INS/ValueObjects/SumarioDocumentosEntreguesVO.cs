using SMC.Framework.Mapper;
using SMC.Framework.Model;
using System;
using System.Collections.Generic;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    /// <summary>
    /// Value Objec para representar uma inscrição em um processo com sua situação
    /// usado para listagem de situção de inscrição no processo
    /// </summary>
    public class SumarioDocumentosEntreguesVO : ISMCMappable
    {
        public long SeqInscricao { get; set; }

        //public SumarioDocumentosEntreguesVO()
        //{
        //    DocumentosObrigatorios = new List<DocumentoRequeridoVO>();
        //    GruposDocumentos = new List<GrupoDocumentoEntregueVO>();
        //    DocumentosOpcionais = new List<DocumentoRequeridoVO>();
        //}

        public bool InscricaoConfirmada { get; set; }

        public List<DocumentoRequeridoVO> DocumentosObrigatorios { get; set; }

        public List<GrupoDocumentoEntregueVO> GruposDocumentos { get; set; }

        public List<DocumentoRequeridoVO> DocumentosOpcionais { get; set; }

        public DateTime? DataPrazoNovaEntregaDocumentacao { get; set; }
    }
}

