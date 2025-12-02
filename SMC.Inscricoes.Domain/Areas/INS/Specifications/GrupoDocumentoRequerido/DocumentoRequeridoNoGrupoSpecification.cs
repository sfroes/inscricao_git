using SMC.Framework.Specification;
using SMC.Inscricoes.Common.Areas.INS.Exceptions;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class DocumentoRequeridoNoGrupoSpecification : SMCSpecification<GrupoDocumentoRequerido>
    {

        public DocumentoRequeridoNoGrupoSpecification(long seqDocumentoRequerido, long seqConfiguracaoEtapa)
        {
            this.SeqConfiguracaoEtapa = seqConfiguracaoEtapa;
            this.SeqDocumentoRequerido = seqDocumentoRequerido;
        }

        public long SeqConfiguracaoEtapa { get; set; }

        public long SeqDocumentoRequerido { get; set; }



        public override Expression<Func<GrupoDocumentoRequerido, bool>> SatisfiedBy()
        {
            return g => (g.SeqConfiguracaoEtapa ==SeqConfiguracaoEtapa 
                && g.Itens.Any(i=>i.SeqDocumentoRequerido==SeqDocumentoRequerido));
        }
    }
}
