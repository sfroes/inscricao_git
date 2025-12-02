using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Service.Data;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    /// <summary>
    /// Dados do processo resumidos
    /// </summary>
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class ProcessoData : ISMCMappable
    {
        [DataMember]
        public long Seq { get; set; }

        [DataMember]
        public long SeqTipoProcesso { get; set; }

        [DataMember]
        public string Descricao { get; set; }

        [DataMember]
        public long? SeqCliente { get; set; }

        [DataMember]
        public long SeqUnidadeResponsavel { get; set; }

        [DataMember]
        public long SeqTemplateProcessoSGF { get; set; }

        [DataMember]
        public long SeqTipoHierarquiaOferta { get; set; }

        [DataMember]
        public string NomeContato { get; set; }

        [DataMember]
        public short? MaximoInscricoesPorInscrito { get; set; }

        [DataMember]
        public bool ExibeRelacaoGeral { get; set; }

        [DataMember]
        public DateTime? DataCancelamento { get; set; }

        [DataMember]
        public DateTime? DataEncerramento { get; set; }

        [DataMember]
        public int SemestreReferencia { get; set; }

        [DataMember]
        public int AnoReferencia { get; set; }

        [DataMember]
        public Guid UidProcesso { get; set; }

        [DataMember]
        public int? SeqEvento { get; set; }

        [DataMember]
        public bool ControlaVagaInscricao { get; set; }

        [DataMember]
        public bool ExibeArvoreFechada { get; set; }

        [DataMember]
        [SMCMapProperty("TipoProcesso.IdsTagManager")]
        public string IdsTagManager { get; set; }

        [DataMember]
        public DateTime? DataInicioEvento { get; set; }

        [DataMember]
        public DateTime? DataFimEvento { get; set; }

        [DataMember]
        public DateTime? DataInicioAtividade { get; set; }

        [DataMember]
        public bool? VerificaCoincidenciaHorario { get; set; }

        [DataMember]
        public IList<ProcessoIdiomaData> Idiomas { get; set; }

        [DataMember]
        public IList<ConfiguracoesModeloDocumentoData> ConfiguracoesModeloDocumento { get; set; }

        [DataMember]
        public IList<ConfiguracoesFormularioData> ConfiguracoesFormulario { get; set; }

        [DataMember]
        public IList<EnderecoEletronicoData> EnderecosEletronicos { get; set; }

        [DataMember]
        [SMCMapForceFromTo]
        public IList<TelefoneData> Telefones { get; set; }

        [DataMember]
        public IList<TaxaData> Taxas { get; set; }

        [DataMember]
        public bool PossuiIntegracao { get; set; }

        [DataMember]
        public List<long> CamposInscrito { get; set; }

        [DataMember]
        public bool? ExibirPeriodoAtividadeOferta { get; set; }

        [DataMember]
        public  TipoProcessoData TipoProcesso { get; set; }

        [DataMember]
        public TimeSpan? HoraAberturaCheckin { get; set; }

        [DataMember]
        public int? CodigoEventoSAE { get; set; }

        [DataMember]
        public long SeqUnidadeResponsavelTipoProcessoIdVisual { get; set; }


    }
}
