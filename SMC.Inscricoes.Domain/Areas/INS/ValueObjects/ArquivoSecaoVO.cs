using SMC.Framework.Mapper;
using SMC.Framework.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class ArquivoSecaoVO : ISMCMappable
    {
        public long SeqArquivo { get; set; }

        public string NomeLink { get; set; }

        public string Descricao { get; set; }

        public SMCUploadFile Arquivo { get; set; }
    }
}
