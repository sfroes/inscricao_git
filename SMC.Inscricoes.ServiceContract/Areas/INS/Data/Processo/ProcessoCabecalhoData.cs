using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    /// <summary>
    /// DTO usado para geração de cabeçalhos de processos
    /// </summary>
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class ProcessoCabecalhoData : ISMCMappable
    {
        [DataMember]
        public long Seq { get; set; }

        [DataMember]
        public string Descricao { get; set; }

        [SMCMapProperty("Cliente.Nome")]
        public string NomeCliente  { get; set; }

        [SMCMapProperty("TipoProcesso.Descricao")]
        public string DescricaoTipoProcesso { get; set; }

    }
}
