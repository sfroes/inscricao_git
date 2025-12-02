using SMC.Framework.Mapper;
using System.Collections.Generic;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class NovaEntregaDocumentacaoCabecalhoVO : ISMCMappable
    {
        public string Usuario { get; set; }
        public string Processo { get; set; }
        public string GrupoOferta { get; set; }
        public List<string> Oferta { get; set; }
    }
}