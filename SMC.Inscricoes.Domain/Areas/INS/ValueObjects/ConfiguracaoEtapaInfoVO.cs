using SMC.Framework.Mapper;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class ConfiguracaoEtapaInfoVO : ISMCMappable
    {
        // Arquivo em base64 do processo
        public string ImagemCabecalho { get; set; }

        public long SeqProcesso { get; set; }

        public string DescricaoProcesso { get; set; }

        public List<FluxoPaginaVO> FluxoPaginas { get; set; }

        public string LabelCodigoAutorizacao { get; set; }

        public string LabelGrupoOferta { get; set; }

        public string LabelOferta { get; set; }

        public bool ExigeJustificativaOferta { get; set; }

        public short? NumeroMaximoOfertaPorInscricao { get; set; }

        public short? NumeroMaximoConvocacaoPorInscricao { get; set; }

        public Guid UidProcesso { get; set; }
    }
}
