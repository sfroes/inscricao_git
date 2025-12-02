using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class GrupoDocumentoRequeridoFilterSpecification : SMCSpecification<GrupoDocumentoRequerido>
    {
        public GrupoDocumentoRequeridoFilterSpecification()
        {
            this.SetOrderBy(x => x.Descricao);
        }

        public long? SeqConfiguracaoEtapa { get; set; }

        public long? SeqDocumentoRequerido { get; set; }

        public long? SeqTipoDocumento { get; set; }

        public bool? UploadObrigatorio { get; set; }

        public int? MinimoObrigatorio { get; set; }

        public override Expression<Func<GrupoDocumentoRequerido, bool>> SatisfiedBy()
        {
            AddExpression(SeqConfiguracaoEtapa, g => g.SeqConfiguracaoEtapa == SeqConfiguracaoEtapa);
            AddExpression(UploadObrigatorio, g => g.UploadObrigatorio == UploadObrigatorio);
            AddExpression(MinimoObrigatorio, g => g.MinimoObrigatorio >= MinimoObrigatorio);
            AddExpression(SeqDocumentoRequerido, g => g.Itens.Any(i=>i.SeqDocumentoRequerido == SeqDocumentoRequerido));
            AddExpression(SeqTipoDocumento, g => g.Itens.Any(i => i.DocumentoRequerido.SeqTipoDocumento == SeqTipoDocumento));

            return GetExpression();
        }
    }
}