using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework;
using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace SMC.Inscricoes.Domain.Areas.INS.Specifications
{
    public class InscricaoFilterSpecification : SMCSpecification<Inscricao>
    {
        public InscricaoFilterSpecification()
        {
           // this.SetOrderByDescending(x => x.DataInscricao);
        }

        public long? SeqInscricao { get; set; }

        public long? SeqInscrito { get; set; }

        public long? SeqOferta { get; set; }

        public long? SeqProcesso { get; set; }

        public long? SeqConfiguracaoEtapa { get; set; }

        public long? SeqGrupoOferta { get; set; }

        public bool? DocumentacaoEntregue { get; set; }

        public Sexo? SexoInscrito { get; set; }

        public long? SeqTipoDocumento { get; set; }

        public SituacaoEntregaDocumento? SituacaoEntregaDocumento { get; set; }

        public string NomeInscrito { get; set; }

        public string DescricaoProcesso { get; set; }

        public string Cpf { get; set; }

        public string NumeroPassaporte { get; set; }

        public int? AnoReferencia { get; set; }

        public int? SemestreReferencia { get; set; }

        public long? SeqUnidadeResponsavel { get; set; }
        public long? SeqTipoProcesso { get; set; }
        public string TokenElemento { get; set; }
        public string ValorElemento { get; set; }

        public long? SeqFormulario { get; set; }
        public bool? SituacaoFormulario { get; set; }

        public bool? FormularioRespondido { get; set; }

        public long? SeqTaxa { get; set; }

        public override Expression<Func<Inscricao, bool>> SatisfiedBy()
        {
            AddExpression(SeqInscricao, a => a.Seq == this.SeqInscricao.Value);
            AddExpression(SeqInscrito, a => a.SeqInscrito == this.SeqInscrito.Value);
            AddExpression(SeqOferta, a => a.Ofertas.Any(o => o.NumeroOpcao == 1 && o.SeqOferta == this.SeqOferta.Value));
            AddExpression(SeqProcesso, a => a.SeqProcesso == this.SeqProcesso.Value);
            AddExpression(SeqTipoProcesso, a => a.Processo.SeqTipoProcesso == this.SeqTipoProcesso.Value);
            AddExpression(SeqConfiguracaoEtapa, a => a.SeqConfiguracaoEtapa == this.SeqConfiguracaoEtapa.Value);
            AddExpression(SeqGrupoOferta, a => a.SeqGrupoOferta == this.SeqGrupoOferta.Value);
            AddExpression(DocumentacaoEntregue, a => a.DocumentacaoEntregue == this.DocumentacaoEntregue.Value);
            AddExpression(SexoInscrito, a => a.Inscrito.Sexo == this.SexoInscrito.Value);
            AddExpression(SeqTipoDocumento, a => a.Documentos.Any(d => d.DocumentoRequerido.SeqTipoDocumento == this.SeqTipoDocumento.Value));
            AddExpression(SituacaoEntregaDocumento, a => a.Documentos.Any(d => d.SituacaoEntregaDocumento == this.SituacaoEntregaDocumento.Value));
            AddExpression(NomeInscrito, a => a.Inscrito.Nome.Contains(this.NomeInscrito));
            AddExpression(DescricaoProcesso, a => a.Processo.Descricao.Contains(this.DescricaoProcesso));
            if (!string.IsNullOrEmpty(this.Cpf))
            {
                var cpf = Cpf.SMCRemoveNonDigits();
                AddExpression(a => a.Inscrito.Cpf == cpf);
            }
            AddExpression(NumeroPassaporte, a => a.Inscrito.NumeroPassaporte == this.NumeroPassaporte);
            AddExpression(AnoReferencia, a => a.Processo.AnoReferencia == this.AnoReferencia.Value);
            AddExpression(SemestreReferencia, a => a.Processo.SemestreReferencia == this.SemestreReferencia.Value);
            AddExpression(SeqUnidadeResponsavel, a => a.Processo.SeqUnidadeResponsavel == this.SeqUnidadeResponsavel.Value);
            AddExpression(TokenElemento, a => a.Formularios.Any(af => af.DadosCampos.Any(ad => ad.Token == TokenElemento)));
            AddExpression(ValorElemento, a => a.Formularios.Any(af => af.DadosCampos.Any(ad => ad.Valor.Contains(ValorElemento))));

            AddExpression(SeqFormulario, a => a.Formularios.Any(x=>x.SeqFormulario == this.SeqFormulario.Value));
            AddExpression(SituacaoFormulario, a => a.HistoricosSituacao.Any(s=>s.Atual == true));

            if(FormularioRespondido.HasValue && FormularioRespondido.Value && SeqFormulario.HasValue)
            {
            AddExpression( a => a.Formularios.Any(n=> n.SeqFormulario == this.SeqFormulario.Value && n.DadosCampos.Any()));

            }

            AddExpression(this.SeqTaxa, a => a.Boletos.Any(an => an.Taxas.Any(any => any.SeqTaxa == this.SeqTaxa)));
            return GetExpression();

        }
    }
}