using SMC.Framework.Mapper;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class CopiaProcessoVO : ISMCMappable
    {
        public long SeqProcessoOrigem { get; set; }

        public long SeqProcessoGpi { get; set; }

        public string NovoProcessoDescricao { get; set; }

        public int NovoProcessoAnoReferencia { get; set; }

        public int NovoProcessoSemestreReferencia { get; set; }

        public bool CopiarItens { get; set; }

        public DateTime? DataInicioInscricao { get; set; }

        public DateTime? DataFimInscricao { get; set; }

        public bool CopiarNotificacoes { get; set; }

        public bool MontarHierarquiaOfertaGPI { get; set; }

        public List<ItemOfertaHierarquiaOfertaVO> ItensOfertasHierarquiasOfertas { get; set; }

        public List<CopiaEtapaProcessoVO> Etapas { get; set; }

        public DateTime? DataInicioFormulario { get; set; }

        public DateTime? DataFimFormulario { get; set; }

        public bool CopiarFormularioEvento { get; set; }
    }
}