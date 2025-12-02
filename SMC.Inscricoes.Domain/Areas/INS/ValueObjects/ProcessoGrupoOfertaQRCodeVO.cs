using SMC.Framework;
using SMC.Framework.Mapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class ProcessoGrupoOfertaQRCodeVO : ISMCMappable, IEquatable<ProcessoGrupoOfertaQRCodeVO>
    {
        public long SeqGrupoOferta { get; set; }
        public long SeqProcesso { get; set; }
        public string DescricaoGrupoOferta { get; set; }

        public bool Equals(ProcessoGrupoOfertaQRCodeVO other)
        {
            return this.SeqGrupoOferta == other.SeqGrupoOferta && this.SeqProcesso == other.SeqProcesso;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
