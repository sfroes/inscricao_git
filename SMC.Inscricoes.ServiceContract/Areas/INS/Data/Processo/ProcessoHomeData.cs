using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    /// <summary>
    /// Value Objec para representar a posição consolidade de um processo
    /// </summary>
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class ProcessoHomeData : ISMCMappable
    {
        [DataMember]
        public long SeqProcesso { get; set; }

        [DataMember]
        public string DescricaoProcesso { get; set; }

        [DataMember]
        public string DescricaoComplementarProcesso { get; set; }

        [DataMember]
        public List<SMCLanguage> IdiomasDisponiveis { get; set; }

        [DataMember]
        public string UrlInformacaoComplementar { get; set; }

        [DataMember]
        public SMCLanguage IdiomaAtual { get; set; }

        [DataMember]
        public bool ProcessoCancelado { get; set; }

        [DataMember]
        public SituacaoEtapa SituacaoEtapaInscricao { get; set; }

        [DataMember]
        public long SeqTipoProcesso { get; set; }
        
        [DataMember]
        public string TituloInscricoes { get; set; }

        [DataMember]
        public string UrlCss { get; set; }

        [DataMember]
        public bool TodasOfertasProcessoInativas { get; set; }

        [DataMember]
        public string TokenResource { get; set; }

        [DataMember]
        public bool ProcessoEncerrado { get; set; }

        [DataMember]
        public string TokenCssAlternativoSas { get; set; }

        [DataMember]
        public string OrientacaoCadastroInscrito { get; set; }

        [DataMember]
        public FormularioImpactoData FormularioImpacto { get; set; }
    }
}
