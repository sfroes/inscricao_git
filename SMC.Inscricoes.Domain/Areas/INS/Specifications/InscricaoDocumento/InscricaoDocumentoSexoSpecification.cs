using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework.Specification;
using SMC.Inscricoes.Common.Areas.INS;
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
    public class InscricaoDocumentoSexoSpecification : SMCSpecification<InscricaoDocumento>
    {
        public InscricaoDocumentoSexoSpecification(long seqDocumentoRequerido,Sexo sexo,bool documentacaoEntregue)
        {
            SeqDocumentoRequerido = seqDocumentoRequerido;
            Sexo = sexo;
            DocumentacaoEntregue = DocumentacaoEntregue;
        }

        public long SeqDocumentoRequerido { get; set; }

        public Sexo Sexo { get; set; }

        public bool DocumentacaoEntregue { get; set; }


        public override Expression<Func<InscricaoDocumento, bool>> SatisfiedBy()
        {
            return i => i.SeqDocumentoRequerido == SeqDocumentoRequerido
                && i.Inscricao.Inscrito.Sexo == Sexo
                && i.Inscricao.DocumentacaoEntregue == DocumentacaoEntregue;
        }
    }
}
