using SMC.Framework.Mapper;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class InscricaoOfertaVO : ISMCMappable
    {
        public long Seq { get; set; }
        public long SeqInscricao { get; set; }
        public long SeqOferta { get; set; }

        public int? NumeroClassificacao { get; set; }

        public Oferta Oferta { get; set; }

        public bool? ExibirPeriodoAtividadeOferta { get; set; }

        public TipoCheckin? TipoCheckin { get; set; }
        public string UsuarioCheckin { get; set; }
        public DateTime? DataCheckin { get; set; }
        public string DescricaoSituacaoInscricao { get; set; }
        public string SituacaoInscricao { get; set; }
        public DateTime? DataInicioAtividade { get; set; }
        public DateTime? DataFimAtividade { get; set; }
        public string DescricaoOferta { get; set; }
        public string Nome { get; set; }
        public string DescricaoProcesso { get; set; }

        public long SeqGrupoOferta { get; set; }

        public long SeqProcesso { get; set; }

        public Guid? UidInscricaoOferta { get; set; }
        public Guid UidProcesso { get; set; }
        public long SeqInscrito { get; set; }
        public string NomeInscrito { get; set; }
        public long? SeqUsuario { get; set; }
    }
}
