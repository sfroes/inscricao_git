using SMC.Framework.Mapper;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class CopiarConfiguracoesEtapaDetalheVO : ISMCMappable
    {
        
        public long SeqConfiguracaoEtapa { get; set; }

        public string Descricao { get; set; }

        public string NumeroEdital { get; set; }

        public DateTime DataInicio { get; set; }

        public DateTime DataFim { get; set; }

        public DateTime? DataLimiteDocumentacao { get; set; }

        public virtual short? NumeroMaximoOfertaPorInscricao { get; set; }

        public virtual bool ExigeJustificativaOferta { get; set; }

        public short? NumeroMaximoConvocacaoPorInscricao { get; set; }
        public bool PermiteNovaEntregaDocumentacao { get; set; }

    }
}