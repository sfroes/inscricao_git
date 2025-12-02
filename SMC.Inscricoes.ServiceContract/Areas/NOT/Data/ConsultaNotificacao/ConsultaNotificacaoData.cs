using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Inscricoes.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.ServiceContract.Areas.NOT.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class ConsultaNotificacaoData : ISMCMappable
    {
        [DataMember]
        public long Seq { get; set; }

        [DataMember]
        public string Processo { get; set; }

        [DataMember]
        public string GrupoOferta { get; set; }

        [DataMember]
        public List<string> Oferta { get; set; }

        [DataMember]
        public string Inscrito { get; set; }

        [DataMember]
        public string DescricaoTipoNotificacao { get; set; }

        [DataMember]
        public string NomeRementente { get; set; }

        [DataMember]
        public string EmailRemetente { get; set; }

        [DataMember]
        public string EmailResposta { get; set; }

        [DataMember]
        public string AssuntoNotificacao { get; set; }

        [DataMember]
        public DateTime? DataPrevistaEnvio { get; set; }

        [DataMember]
        public DateTime? DataEnvio { get; set; }

        [DataMember]
        public bool? SucessoEnvio { get; set; }

        [DataMember]
        public string EmailDestinatario { get; set; }

        [DataMember]
        public string EmailComCopia { get; set; }

        [DataMember]
        public string EmailComCopiaOculta { get; set; }

        [DataMember]
        public string Mensagem { get; set; }

        [DataMember]
        public string DescricaoErroEnvio { get; set; }

        [DataMember]
        public List<SMCDatasourceItem> Arquivos { get; set; }

        public long SeqNotificacaoEmailDestinatario { get; set; }
    }
}
