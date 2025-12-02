using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Enums;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.ServiceContract.Areas.INS.Data
{
    [DataContract(Namespace = NAMESPACES.MODEL, IsReference = true)]
    public class NoArvoreConfiguracaoEtapaData : ISMCMappable
    {        
        public long Seq { get; set; }

        public long SeqItem { get; set; }

        public long SeqConfiguracaoEtapa { get; set; }

        public long SeqEtapaProcesso { get; set; }

        public long SeqProcesso { get; set; }

        public long? SeqPai { get; set; }
                
        public string Descricao { get; set; }

        public TipoItemPaginaEtapa Tipo { get; set; }

        public string PaginaToken { get; set; }

        public bool PaginaPermiteExibicaoOutrasPaginas { get; set; }

        public bool PaginaObrigatoria { get; set; }

        public bool PaginaExibeFormulario { get; set; }

        public bool PaginaPermiteDuplicar { get; set; }
    }
}