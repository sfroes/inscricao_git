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

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{

    public class DocumentoSimplificadoVO : ISMCMappable
    {
        
        public long Seq { get; set; }  
        
        public long SeqDocumentoRequerido { get; set; }
        
        public long SeqTipoDocumento { get; set; }

        public string DescricaoTipoDocumento { get; set; }

    }
}
