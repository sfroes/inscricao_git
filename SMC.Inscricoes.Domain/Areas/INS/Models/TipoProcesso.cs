using SMC.Framework;
using SMC.Framework.Audit;
using SMC.Framework.Mapper;
using SMC.Inscricoes.Domain.Areas.RES.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SMC.Inscricoes.Domain.Areas.INS.Models
{
    public partial class TipoProcesso : ISMCSeq, ISMCAuditData, ISMCMappable
    {
        #region Primitive Properties

        [Key]
        [Required]
        public virtual long Seq { get; set; }

        [Required]
        [StringLength(255)]
        public virtual string Descricao { get; set; }

        [Required]
        public virtual System.DateTime DataInclusao { get; set; }

        [Required]
        [StringLength(60)]
        public virtual string UsuarioInclusao { get; set; }

        public virtual Nullable<System.DateTime> DataAlteracao { get; set; }

        [StringLength(60)]
        public virtual string UsuarioAlteracao { get; set; }

        [Required]
        public virtual long SeqTipoTemplateProcessoSGF { get; set; }

        [Required]
        public virtual bool ExigeCodigoOrigemOferta { get; set; }

        [Required]
        public virtual bool IntegraSGALegado { get; set; }

        [StringLength(255)]
        public virtual string IdsTagManager { get; set; }

        [Required]
        public virtual bool IsencaoP1 { get; set; }

        [Required]
        public virtual bool BolsaExAluno { get; set; }

        [Required]
        public virtual bool IntegraGPC { get; set; }

        public virtual string OrientacaoAceiteConversaoPDF { get; set; }
        public virtual string TermoAceiteConversaoPDF { get; set; }
        public virtual string TermoConsentimentoLGPD { get; set; }

        [Required]
        public virtual bool PermiteRegerarTitulo { get; set; }

        [Required]
        public virtual bool HabilitaPercentualDesconto { get; set; }

        [Required]
        public virtual bool ValidaLimiteDesconto { get; set; }

        [Required]
        public virtual bool GestaoEventos { get; set; }

        public virtual Nullable<long> SeqContextoBibliotecaGed { get; set; }
        public virtual Nullable<long> SeqHierarquiaClassificacaoGed { get; set; }

        [Required]
        [StringLength(255)]
        public virtual string TokenResource { get; set; }

        [Required]
        public virtual bool HabilitaGed { get; set; }

        [Required]
        [StringLength(255)]
        public virtual string Token { get; set; }

        public virtual string OrientacaoCadastroInscrito { get; set; }

        #endregion Primitive Properties

        #region Navigation Properties

        public virtual IList<Processo> Processos { get; set; }

        public virtual IList<TipoProcessoSituacao> Situacoes { get; set; }

        public virtual IList<TipoProcessoTemplate> Templates { get; set; }

        public virtual IList<TipoProcessoTipoTaxa> TiposTaxa { get; set; }

        public virtual IList<UnidadeResponsavelTipoProcesso> UnidadeResponsavelTipoProcesso { get; set; }

        public virtual IList<TipoProcessoConsistencia> Consistencias { get; set; }

        public virtual IList<TipoProcessoDocumento> Documentos { get; set; }

        public virtual IList<TipoProcessoCampoInscrito> CamposInscrito { get; set; }

        #endregion Navigation Properties
    }
}