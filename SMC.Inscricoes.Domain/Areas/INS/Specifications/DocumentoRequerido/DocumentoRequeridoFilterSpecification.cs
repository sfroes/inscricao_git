using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class DocumentoRequeridoFilterSpecification : SMCSpecification<DocumentoRequerido>
    {
        public DocumentoRequeridoFilterSpecification()
        {
            this.SetOrderBy(x => x.TipoDocumento.Descricao);
        }

        public long? SeqConfiguracaoEtapa { get; set; }

        public bool? PermiteUploadArquivo { get; set; }

        public bool? UploadObrigatorio { get; set; }

        public bool? Obrigatorio { get; set; }

        public bool? PermiteVarios { get; set; }

        public List<long> Seqs { get; set; }

        public long? SeqTipoDocumento { get; set; }

        public SituacaoEntregaDocumento? SituacaoEntregaDocumento { get; set; }

        public override Expression<Func<DocumentoRequerido, bool>> SatisfiedBy()
        {
            AddExpression(SeqConfiguracaoEtapa, d => d.SeqConfiguracaoEtapa == this.SeqConfiguracaoEtapa.Value);
            AddExpression(PermiteUploadArquivo, d => d.PermiteUploadArquivo == this.PermiteUploadArquivo);
            AddExpression(UploadObrigatorio, d => d.UploadObrigatorio == this.UploadObrigatorio.Value);
            AddExpression(Obrigatorio, d => d.Obrigatorio == Obrigatorio.Value);
            AddExpression(PermiteVarios, d => d.PermiteVarios == this.PermiteVarios.Value);
            AddExpression(Seqs, x => Seqs.Contains(x.Seq));
            AddExpression(SeqTipoDocumento, x => x.SeqTipoDocumento == this.SeqTipoDocumento.Value);
            AddExpression(SituacaoEntregaDocumento, x => x.InscricaoDocumentos.Any(id=>id.SituacaoEntregaDocumento == this.SituacaoEntregaDocumento.Value));
            return GetExpression();
        }
    }
}