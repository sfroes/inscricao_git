using SMC.Framework.Mapper;
using SMC.Inscricoes.Common;
using System;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    /// <summary>
    /// Value Objec para representar a posição consolidade de um processo
    /// </summary>    
    public class HierarquiaOfertaVO : ISMCMappable
    {
        [SMCMapProperty("Nome")]        
        public string Descricao { get; set; }

        public string DescricaoCompleta { get; set; }
        
        public long Seq { get; set; }
        
        public long? SeqPai { get; set; }

        public long SeqItemHierarquiaOferta { get; set; }

        public long SeqProcesso { get; set; }

        public bool EOferta { get; set; }

        public bool PossuiGrupo { get; set; }

        public bool GrupoEmConfiguracao { get; set; }

        public string NomeGrupoOferta { get; set; }

        public long? SeqGrupoOferta { get; set; }

        public bool PermiteCadastroOfertaFilha { get; set; }

        public bool PermiteCadastroItemFilho { get; set; }

        public bool PermiteCadastroNetos { get; set; }

        public bool Cancelada { get; set; }

        public bool Desativada { get; set; }

        public DateTime? DataInicioAtividade { get; set; }

        public DateTime? DataFimAtividade { get; set; }

        public bool? ProcessoExibirPeriodoAtividadeOferta { get; set; }

        public int? CargaHorariaAtividade { get; set; }

        public TimeSpan? HoraAberturaCheckin { get; set; }

    }
}
