using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class CopiaProcessoData : ISMCMappable
    {
        [DataMember]
        public long SeqProcessoOrigem { get; set; }

        [DataMember]
        public long SeqProcessoGpi { get; set; }


        [DataMember]
        public string DescricaoTipoProcesso { get; set; }

        [DataMember]
        public string Descricao { get; set; }

        [DataMember]
        public int AnoReferencia { get; set; }

        [DataMember]
        public int SemestreReferencia { get; set; }

        [DataMember]
        public string NovoProcessoDescricao { get; set; }

        [DataMember]
        public int? NovoProcessoAnoReferencia { get; set; }

        [DataMember]
        public int? NovoProcessoSemestreReferencia { get; set; }

        [DataMember]
        public bool CopiarItens { get; set; }

        [DataMember]
        public DateTime? DataInicioInscricao { get; set; }

        [DataMember]
        public DateTime? DataFimInscricao { get; set; }

        [DataMember]
        public bool CopiarNotificacoes { get; set; }

        [DataMember]
        public bool MontarHierarquiaOfertaGPI { get; set; }

        [DataMember]
        public List<ItemOfertaHierarquiaOfertaData> ItensOfertasHierarquiasOfertas { get; set; }

        [DataMember]
        public List<CopiaEtapaProcessoData> Etapas { get; set; }

        [DataMember]
        public bool TipoProcessoDesativado { get; set; }

        [DataMember]
        public bool TemplateProcessoDesativado { get; set; }

        [DataMember]
        public bool TipoHierarquiaOfertaDesativado { get; set; }

        [DataMember]
        public bool PossuiFormularioConfigurado { get; set; }

        [DataMember]
        public DateTime? DataInicioFormulario { get; set; }
        
        [DataMember]
        public DateTime? DataFimFormulario { get; set; }

        [DataMember]
        public bool CopiarFormularioEvento { get; set; }
    }
}