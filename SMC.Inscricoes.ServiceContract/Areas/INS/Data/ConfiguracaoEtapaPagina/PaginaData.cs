using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class PaginaData : ISMCMappable
    {
        public PaginaData()
        {
            Secoes = new List<SecaoPaginaData>();
            FluxoPaginas = new List<FluxoPaginaData>();
            DescricaoOfertas = new List<string>();
        }

        [DataMember]
        public long SeqConfiguracaoEtapaPagina { get; set; }

        [DataMember]
        public string Titulo { get; set; }

        [SMCMapForceFromTo]
        [DataMember]
        public List<SecaoPaginaData> Secoes { get; set; }

        [DataMember]
        public long SeqConfiguracaoEtapa { get; set; }

        [DataMember]
        public long SeqGrupoOferta { get; set; }

        [DataMember]
        public long? SeqInscricao { get; set; }

        [DataMember]
        public SMCLanguage Idioma { get; set; }

        [DataMember]
        public long SeqProcesso { get; set; }

        [DataMember]
        public string DescricaoProcesso { get; set; }

        [DataMember]
        public string DescricaoGrupoOferta { get; set; }

        [DataMember]
        public List<string> DescricaoOfertas { get; set; }

        [DataMember]
        public string TokenSituacaoAtual { get; set; }

        [DataMember]
        public string DescricaoSituacaoAtual { get; set; }

        [DataMember]
        public string ImagemCabecalho { get; set; }

        [SMCMapForceFromTo]
        [DataMember]
        public List<FluxoPaginaData> FluxoPaginas { get; set; }

        [DataMember]
        public int Ordem { get; set; }

        [DataMember]
        public string LabelCodigoAutorizacao { get; set; }

        [DataMember]
        public string LabelGrupoOferta { get; set; }

        [DataMember]
        public string LabelOferta { get; set; }

        [DataMember]
        public bool ExigeJustificativaOferta { get; set; }

        [DataMember]
        public short? NumeroMaximoOfertaPorInscricao { get; set; }

        [DataMember]
        public short? NumeroMaximoConvocacaoPorInscricao { get; set; }

        [DataMember]
        public string TokenResource { get; set; }

        [DataMember]
        public Guid UidProcesso { get; set; }

        [DataMember]
        public string UrlCss { get; set; }

        [DataMember]
        public bool HabilitaCheckin { get; set; }
        [DataMember]
        public bool GestaoEventos { get; set; }

    }
}
