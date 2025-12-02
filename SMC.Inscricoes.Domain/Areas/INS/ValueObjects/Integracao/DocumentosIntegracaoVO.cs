using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class DocumentosIntegracaoVO : ISMCMappable
    {
        public long? SeqArquivoAnexado { get; set; }
        
        public long SeqTipoDocumento { get; set; }
        
        public DateTime? DataEntrega { get; set; }

        public DateTime? DataPrazoEntrega { get; set; }

        public FormaEntregaDocumento? FormaEntregaDocumento { get; set; }
        
        public VersaoDocumento? VersaoDocumento { get; set; }
        
        public string Observacao { get; set; }
        
        public SituacaoEntregaDocumento SituacaoEntregaDocumento { get; set; }

        public string DescricaoTipoDocumento { get; set; }
    }
}
