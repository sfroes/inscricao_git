using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Service.Data;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class OfertaData : HierarquiaOfertaData
    {
        [DataMember]
        public bool Ativo { get; set; }

        [DataMember]
        public DateTime? DataInicio { get; set; }

        [DataMember]
        public DateTime? DataFim { get; set; }

        [DataMember]
        public int? NumeroVagas { get; set; }

        [DataMember]
        public long? SeqGrupoOferta { get; set; }

        [DataMember]
        public bool ExigePagamentoTaxa { get; set; }

        [DataMember]
        public bool InscricaoSoPorCliente { get; set; }

        [DataMember]
        public int? CodigoOrigem { get; set; }

        [DataMember]
        public decimal? LimitePercentualDesconto { get; set; }

        [DataMember]
        public int? NumeroVagasBolsa { get; set; }

        [DataMember]
        public bool InscricaoSoComCodigo { get; set; }

        [DataMember]
        public bool PermiteVariosCodigos { get; set; }

        [DataMember]
        public bool HabilitaCheckin { get; set; }

        public DateTime? DataInicioAtividade { get; set; }

        public DateTime? DataFimAtividade { get; set; }

        public int? CargaHorariaAtividade { get; set; }

        [DataMember]
        public List<OfertaCodigoAutorizacaoData> CodigosAutorizacao { get; set; }

        [DataMember]
        public string NomeResponsavel { get; set; }

        [DataMember]
        [SMCMapForceFromTo]
        public List<TelefoneData> Telefones { get; set; }

        [DataMember]
        public List<EnderecoEletronicoData> EnderecosEletronicos { get; set; }

        [DataMember]
        public DateTime? DataCancelamento { get; set; }

        [DataMember]
        public string MotivoCancelamento { get; set; }

        [DataMember]
        public List<OfertaPeriodoTaxaData> Taxas { get; set; }
    }
}