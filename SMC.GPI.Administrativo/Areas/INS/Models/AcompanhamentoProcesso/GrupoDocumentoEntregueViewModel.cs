using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Framework.UI.Mvc;
using SMC.Inscricoes.Common.Areas.INS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.GPI.Administrativo.Areas.INS.Models
{
    /// <summary>
    /// Value Objec para representar uma inscrição em um processo com sua situação
    /// usado para listagem de situção de inscrição no processo
    /// </summary>
    public class GrupoDocumentoEntregueViewModel : SMCViewModelBase, ISMCMappable
    {
        public string Descricao { get; set; }

        public long MinimoObrigatorio { get; set; }

        public List<DocumentoRequeridoEntregueViewModel> DocumentosRequeridosGrupo { get; set; }
    }
}

