using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class DocumentoRequeridoOpcionalOuMultiploSpecification : SMCSpecification<DocumentoRequerido>
    {

        public DocumentoRequeridoOpcionalOuMultiploSpecification(long seqConfiguracaoEtapa)
        {
            this.SeqConfiguracaoEtapa = seqConfiguracaoEtapa;            
        }

        public Sexo? Sexo { get; set; }

        public long SeqConfiguracaoEtapa { get; set; }
     

        public override Expression<Func<DocumentoRequerido, bool>> SatisfiedBy()
        {
            return d => d.SeqConfiguracaoEtapa == this.SeqConfiguracaoEtapa
                && d.PermiteUploadArquivo
                && (!d.UploadObrigatorio || (d.UploadObrigatorio && d.PermiteVarios) 
                    || (this.Sexo.HasValue && d.UploadObrigatorio && d.Sexo.HasValue && d.Sexo.Value!=this.Sexo.Value))
                && (!d.Sexo.HasValue || ( this.Sexo.HasValue && this.Sexo.Value!=d.Sexo.Value));
        }
    }
}
