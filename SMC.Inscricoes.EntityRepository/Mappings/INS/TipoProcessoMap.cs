using SMC.Inscricoes.Domain.Areas.INS.Models;
using System.Data.Entity.ModelConfiguration;

namespace SMC.Inscricoes.EntityRepository
{
    public class TipoProcessoMap : EntityTypeConfiguration<TipoProcesso>
    {
        public TipoProcessoMap()
        {
            // Primary Key
            this.HasKey(t => t.Seq);

            // Properties
            this.Property(t => t.Seq);

            this.Property(t => t.Descricao)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(t => t.DataInclusao);

            this.Property(t => t.UsuarioInclusao)
                .IsRequired()
                .HasMaxLength(60);

            this.Property(t => t.DataAlteracao);

            this.Property(t => t.UsuarioAlteracao)
                .HasMaxLength(60);

            this.Property(t => t.SeqTipoTemplateProcessoSGF);

            this.Property(t => t.ExigeCodigoOrigemOferta);

            this.Property(t => t.IntegraSGALegado);

            this.Property(t => t.IdsTagManager)
                .HasMaxLength(255);

            this.Property(t => t.IsencaoP1);

            this.Property(t => t.BolsaExAluno);

            this.Property(t => t.IntegraGPC);

            this.Property(t => t.OrientacaoAceiteConversaoPDF);

            this.Property(t => t.TermoAceiteConversaoPDF);

            this.Property(t => t.TermoConsentimentoLGPD);

            this.Property(t => t.PermiteRegerarTitulo);

            this.Property(t => t.HabilitaPercentualDesconto);

            this.Property(t => t.ValidaLimiteDesconto);

            this.Property(t => t.GestaoEventos);

            this.Property(t => t.SeqContextoBibliotecaGed);

            this.Property(t => t.SeqHierarquiaClassificacaoGed);

            this.Property(t => t.TokenResource)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(t => t.HabilitaGed);

            this.Property(t => t.Token)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(t => t.OrientacaoCadastroInscrito);

            // Table & Column Mappings

            this.ToTable("tipo_processo");
            this.Property(t => t.Seq).HasColumnName("seq_tipo_processo");
            this.Property(t => t.Descricao).HasColumnName("dsc_tipo_processo");
            this.Property(t => t.DataInclusao).HasColumnName("dat_inclusao");
            this.Property(t => t.UsuarioInclusao).HasColumnName("usu_inclusao");
            this.Property(t => t.DataAlteracao).HasColumnName("dat_alteracao");
            this.Property(t => t.UsuarioAlteracao).HasColumnName("usu_alteracao");
            this.Property(t => t.SeqTipoTemplateProcessoSGF).HasColumnName("seq_tipo_template_processo_sgf");
            this.Property(t => t.ExigeCodigoOrigemOferta).HasColumnName("ind_exige_cod_origem_oferta");
            this.Property(t => t.IntegraSGALegado).HasColumnName("ind_integra_sga_legado");
            this.Property(t => t.IdsTagManager).HasColumnName("dsc_ids_tag_manager");
            this.Property(t => t.IsencaoP1).HasColumnName("ind_isencao_p1");
            this.Property(t => t.BolsaExAluno).HasColumnName("ind_bolsa_ex_aluno");
            this.Property(t => t.IntegraGPC).HasColumnName("ind_integra_gpc");
            this.Property(t => t.OrientacaoAceiteConversaoPDF).HasColumnName("dsc_orientacao_aceite_conversao_arquivos_PDF");
            this.Property(t => t.TermoAceiteConversaoPDF).HasColumnName("dsc_termo_aceite_conversao_arquivos_PDF");
            this.Property(t => t.TermoConsentimentoLGPD).HasColumnName("dsc_termo_consentimento_lgpd");
            this.Property(t => t.PermiteRegerarTitulo).HasColumnName("ind_permite_regerar_titulo");
            this.Property(t => t.HabilitaPercentualDesconto).HasColumnName("ind_habilita_percentual_desconto");
            this.Property(t => t.ValidaLimiteDesconto).HasColumnName("ind_valida_limite_desconto");
            this.Property(t => t.GestaoEventos).HasColumnName("ind_gestao_eventos");
            this.Property(t => t.SeqContextoBibliotecaGed).HasColumnName("seq_contexto_biblioteca_ged");
            this.Property(t => t.SeqHierarquiaClassificacaoGed).HasColumnName("seq_hierarquia_classificacao_ged");
            this.Property(t => t.TokenResource).HasColumnName("dsc_token_resource");
            this.Property(t => t.HabilitaGed).HasColumnName("ind_habilita_ged");
            this.Property(t => t.Token).HasColumnName("dsc_token_tipo_processo");
            this.Property(t => t.OrientacaoCadastroInscrito).HasColumnName("dsc_orientacao_cadastro_inscrito");
        }
    }
}