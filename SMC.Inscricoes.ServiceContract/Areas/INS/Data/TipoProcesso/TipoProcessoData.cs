using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Service.Data;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data.TipoDocumento;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class TipoProcessoData : ISMCMappable
    {
        [DataMember]
        public long Seq { get; set; }

        [DataMember]
        public string Descricao { get; set; }

        [DataMember]
        public long SeqTipoTemplateProcessoSGF { get; set; }

        [DataMember]
        public bool ExibeTermoConsentimentoLGPD { get; set; }

        [DataMember]
        public bool ExigeCodigoOrigemOferta { get; set; }

        [DataMember]
        public bool IntegraSGALegado { get; set; }

        [DataMember]
        public string IdsTagManager { get; set; }

        [DataMember]
        public bool BolsaExAluno { get; set; }

        [DataMember]
        public bool IsencaoP1 { get; set; }

        public bool IntegraGPC { get; set; }

        public bool GestaoEventos { get; set; }

        [DataMember]
        public bool PermiteRegerarTitulo { get; set; }

        [DataMember]
        public List<TipoProcessoTipoTaxaData> TiposTaxa { get; set; }

        [DataMember]
        public List<TipoProcessoSituacaoData> Situacoes { get; set; }

        [DataMember]
        public List<TipoProcessoTemplateData> Templates { get; set; }

        [DataMember]
        public List<TipoProcessoDocumentoData> Documentos { get; set; }

        public List<ConsistenciaTipoProcessoData> Consistencias { get; set; }

        [DataMember]
        public string OrientacaoAceiteConversaoPDF { get; set; }

        [DataMember]
        public string TermoAceiteConversaoPDF { get; set; }

        [DataMember]
        public string TermoConsentimentoLGPD { get; set; }

        [DataMember]
        public bool HabilitaPercentualDesconto { get; set; }

        [DataMember]
        public  bool ValidaLimiteDesconto { get; set; }

        [DataMember]
        public long? SeqContextoBibliotecaGed { get; set; }

        [DataMember]
        public long? SeqHierarquiaClassificacaoGed { get; set; }

        [DataMember]
        public  string TokenResource { get; set; }

        [DataMember]
        public bool HabilitaGed { get; set; }

        [DataMember]
        public List<long> CamposInscrito { get; set; }

        [DataMember]
        public string Token { get; set; }
    }
}
