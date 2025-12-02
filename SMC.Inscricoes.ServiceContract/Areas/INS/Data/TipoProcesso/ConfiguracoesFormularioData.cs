using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    public class ConfiguracoesFormularioData : ISMCMappable
    {
        public long Seq { get; set; }
        public long SeqTipoFormularioSgf { get; set; }

        public long SeqFormularioSgf { get; set; }

        public long SeqVisaoSgf { get; set; }

        public string Descricao { get; set; }

        public DateTime DataInicioFormulario { get; set; }

        public DateTime? DataFimFormulario { get; set; }

        public string Mensagem { get; set; }

        public  bool DisponivelApenasComCheckin { get; set; }
    }
}
