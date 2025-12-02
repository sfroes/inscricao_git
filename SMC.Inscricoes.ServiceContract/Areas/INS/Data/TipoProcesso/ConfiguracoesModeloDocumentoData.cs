using SMC.Framework.Mapper;
using SMC.Framework.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    public class ConfiguracoesModeloDocumentoData : ISMCMappable
    {
        public long SeqTipoDocumento { get; set; }

        public long SeqArquivoModelo { get; set; }

        public SMCUploadFile ArquivoModelo { get; set; }

        public string TokenConfiguracaoDocumentoGad { get; set; }

        public bool ExibeDocumentoHome { get; set; }

        public DateTime? DataInicioDocumentoHome { get; set; }

        public DateTime? DataFimDocumentoHome { get; set; }

        public bool AssinaturaEletronica { get; set; }

        public bool RequerCheckin { get; set; }
    }
}
