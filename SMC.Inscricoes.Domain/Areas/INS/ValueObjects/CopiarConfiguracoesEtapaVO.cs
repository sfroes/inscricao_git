using SMC.Framework;
using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class CopiarConfiguracoesEtapaVO :  ISMCMappable
    {
      
        public long SeqEtapaProcesso { get; set; }
      
        public long SeqProcessoOrigem { get; set; }
 
        public long  SeqProcessoDestino { get; set; }
     
        public bool CopiarPaginas { get; set; }

        public bool CopiarDocumentacao { get; set; }        

        public List<CopiarConfiguracoesEtapaDetalheVO> Configuracoes {get;set;}
                        
    }
}