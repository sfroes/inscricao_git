using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Inscricoes.Common.Areas.INS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    /// <summary>
    /// Value Objec para representar uma inscrição em um processo com sua situação
    /// usado para listagem de situção de inscrição no processo
    /// </summary>
    public class GrupoDocumentoEntregueVO : ISMCMappable
    {
        //public GrupoDocumentoEntregueVO()
        //{
        //    DocumentosRequeridosGrupo = new List<DocumentoRequeridoVO>();
        //}

        public string Descricao { get; set; }

        public long MinimoObrigatorio { get; set; }

        public List<DocumentoRequeridoVO> DocumentosRequeridosGrupo { get; set; }
    }
}

