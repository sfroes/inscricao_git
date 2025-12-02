using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.Framework.UI.Mvc.DataAnnotations;
using System;
using System.Collections.Generic;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    /// <summary>
    /// Value Objec para representar uma inscrição em um processo com sua situação
    /// usado para listagem de situção de inscrição no processo
    /// </summary>
    public class SumarioDocumentosEntreguesViewModel : SMCViewModelBase, ISMCMappable
    {
        [SMCHidden]
        public bool InscricaoConfirmada { get; set; }

        [SMCHidden]
        public long SeqInscricao { get; set; }

        public List<DocumentoRequeridoEntregueViewModel> DocumentosObrigatorios { get; set; }

        public List<GrupoDocumentoEntregueViewModel> GruposDocumentos { get; set; }

        public List<DocumentoRequeridoEntregueViewModel> DocumentosOpcionais { get; set; }

        public DateTime? DataPrazoNovaEntregaDocumentacao { get; set; }

        [SMCHidden]
        public bool Sair { get; set; }

        [SMCHidden]
        public string BackURL { get; set; }

        [SMCHidden]
        public string Origem { get; set; }
    }
}

