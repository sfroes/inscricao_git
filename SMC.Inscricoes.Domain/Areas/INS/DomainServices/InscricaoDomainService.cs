using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Reflection;
using Newtonsoft.Json;
using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Financeiro.ServiceContract.BLT;
using SMC.Formularios.Service.Areas.TMP.Services;
using SMC.Formularios.ServiceContract.Areas.TMP.Interfaces;
using SMC.Framework;
using SMC.Framework.Caching;
using SMC.Framework.Domain;
using SMC.Framework.Exceptions;
using SMC.Framework.Extensions;
using SMC.Framework.Logging;
using SMC.Framework.Repository;
using SMC.Framework.Security;
using SMC.Framework.Specification;
using SMC.Framework.UnitOfWork;
using SMC.Framework.Util;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using SMC.Inscricoes.Common.Areas.INS.Exceptions;
using SMC.Inscricoes.Common.Areas.INS.Exceptions.Inscricao;
using SMC.Inscricoes.Common.Constants;
using SMC.Inscricoes.Common.Enums;
using SMC.Inscricoes.Common.Shared.QrCode;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using SMC.Inscricoes.Domain.Areas.INS.ValueObjects;
using SMC.Inscricoes.Domain.Areas.SEL.DomainServices;
using SMC.Inscricoes.Domain.Areas.Shared.SharePointWebApi.Portfolio;
using SMC.Inscricoes.Domain.Areas.Shared.SharePointWebApi.ValueObject;
using SMC.Inscricoes.Domain.Models;
using SMC.Inscricoes.Rest.Helper;
using SMC.IntegracaoAcademico.ServiceContract.Areas.IAC.Data;
using SMC.IntegracaoAcademico.ServiceContract.Areas.IAC.Interfaces;
using SMC.Notificacoes.ServiceContract.Areas.NTF.Interfaces;
using SMC.Pessoas.ServiceContract.Areas.PES.Interfaces;
using SMC.Seguranca.ServiceContract.Areas.USU.Interfaces;

namespace SMC.Inscricoes.Domain.Areas.INS.DomainServices
{
    public class InscricaoDomainService : InscricaoContextDomain<Inscricao>
    {
        #region Query

        private string _query_aqruivos_pendentes_indeferidos = @"select    i.seq_inscricao as SeqInscricao
                    , DocumentosIndeferidos =
                                convert(varchar(500), STUFF((
                                select    ', ' + td.dsc_tipo_documento as [data()]
                                from inscricao_documento id
                                join documento_requerido dr
                                        on dr.seq_documento_requerido = id.seq_documento_requerido
                                join DADOSMESTRES..tipo_documento td
                                        on td.seq_tipo_documento = dr.seq_tipo_documento
                                where id.seq_inscricao = i.seq_inscricao
                                and        id.idt_dom_situacao_entrega_documento = 4-- Indeferido
                                and(
                                            dr.ind_obrigatorio = 1
                                            or
                                            exists
                                            (
                                                select    1
                                                from grupo_documento_requerido_item dri
                                                where dri.seq_documento_requerido = dr.seq_documento_requerido
                                            )
                                        )
                                order by td.dsc_tipo_documento
                                FOR xml path('')), 1, 1, ''))
                    , DocumentosPendentes =
                                convert(varchar(500), STUFF((
                                select    ', ' + td.dsc_tipo_documento as [data()]
                                from inscricao_documento id
                                join documento_requerido dr
                                        on dr.seq_documento_requerido = id.seq_documento_requerido
                                join DADOSMESTRES..tipo_documento td
                                        on td.seq_tipo_documento = dr.seq_tipo_documento
                                where id.seq_inscricao = i.seq_inscricao
                                and        id.idt_dom_situacao_entrega_documento = 6-- Pendente
                                and(
                                            dr.ind_obrigatorio = 1
                                            or
                                            exists
                                            (
                                                select    1
                                                from grupo_documento_requerido_item dri
                                                where dri.seq_documento_requerido = dr.seq_documento_requerido
                                            )
                                        )
                                order by td.dsc_tipo_documento
                                FOR xml path('')), 1, 1, ''))
                    from inscricao i
                    where   i.seq_processo = {0}";

        private string _query_area_conhecimento = @"select	distinct 
		--ca.dsc_calendario_acao 'Calendario',
		--a.seq_acao 'CodigoProjeto',
		--a.num_acao 'NumAcao',
		--a.dsc_titulo 'Titulo',
		--areaconhecimento.val_campo 'CodigoAreaConhecimento',
		ac.dsc_area 'AreaConhecimento'	
		
        from	acao..acao	a
        join	acao..calendario_acao	ca
        on		a.seq_calendario_acao = ca.seq_calendario_acao
        join	acao..etapa_calendario ec
        on		ca.seq_calendario_acao = ec.seq_calendario_acao
        and		ec.dsc_token_etapa like 'CADASTRO_SUBMISSAO'
        join	acao..acao_etapa ae
        on		ae.seq_acao = a.seq_acao
        join	acao..configuracao_etapa ce
        on		ae.seq_configuracao_etapa = ce.seq_configuracao_etapa
        and		ce.seq_etapa_calendario = ec.seq_etapa_calendario
        and		ce.dsc_token_visao like 'CADASTRO_ACAO'
        join	ACAO..dado_acao_formulario daf
        on		ae.seq_acao_etapa = daf.seq_acao_etapa
        join	acao..dado_acao_campo areaconhecimento
        on		daf.seq_dado_acao_formulario = areaconhecimento.seq_dado_acao_formulario
        and		areaconhecimento.dsc_token like 'AREA_CONHECIMENTO'
        join	sga..area_conhecimento ac
        on		ac.cod_area = areaconhecimento.val_campo

        where	ca.seq_processo_inscricao in ({0})
        and     a. seq_acao = {1}
        and		a.idt_dom_status in (90,120)
        --order by ca.dsc_calendario_acao, a.seq_acao";

        private string _query_orientador = @"
--2) Orientador e E-mail Orientador  - Projetos PIBIC FAPEMIG --Para os projetos FAPEMIG, retornar os dados do orientador

        select	distinct 
		      --  ca.dsc_calendario_acao 'Calendário',
		       -- a.seq_acao 'Código Projeto',
		       -- a.num_acao,
		       -- a.dsc_titulo 'Título',
		       -- oa.cod_pessoa 'Código de Pessoa',
		        oa.nom_pessoa 'NomeOrientador',
		        oa.dsc_email	'EmailOrientador'
		
        from	acao..acao	a
        join	acao..calendario_acao	ca
        on		a.seq_calendario_acao = ca.seq_calendario_acao
        join	acao..orientador_acao oa
        on		a.seq_acao = oa.seq_acao

        where	ca.seq_processo_inscricao in ({0})
        and     a. seq_acao = {1}
        and		a.idt_dom_status in (90,120)
        and		a.seq_tipo_acao_entidade in (7)
        --order by ca.dsc_calendario_acao, a.seq_acao";

        private string _query_alunos = @"
            SELECT DISTINCT	
            --ca.dsc_calendario_acao, 
            --a.seq_acao AS SEQ, CONCAT(a.num_acao, ' - ', a.dsc_titulo) AS descricao,
            u.nom_usuario 'Nome'
            --ue.dsc_email,
            --ra.seq_usuario_sas, 
            --ra.cod_aluno

            FROM	acao..acao a
            join	acao..calendario_acao	ca
            on		a.seq_calendario_acao = ca.seq_calendario_acao
            join	acao..responsavel_acao ra
            on		a.seq_acao = ra.seq_acao
            join	SAS..usuario u
            on		ra.seq_usuario_sas = u.seq_usuario
            left join SAS..usuario_email ue
            on		ue.seq_usuario = ra.seq_usuario_sas
            where	ca.seq_processo_inscricao = {0}
            and a.seq_acao = {1}
            and		a.idt_dom_status in (90, 120) -- APROVADO_PARA_EXECUCAO, CONCLUIDO
            and		a.seq_tipo_acao_entidade in (7)

            UNION

            SELECT DISTINCT 
            --ca.dsc_calendario_acao, 
            --a.seq_acao AS SEQ, CONCAT(a.num_acao, ' - ', a.dsc_titulo) AS descricao,
            u.nom_usuario 'Nome'
            --ue.dsc_email,
            --u.seq_usuario,
            --c.seq_aluno, 
            --al.cod_aluno
            FROM	SGE2..projeto	p
            join	acao..acao	a
            on		a.seq_acao = p.seq_projeto_gpc
            join	acao..calendario_acao	ca
            on		ca.seq_calendario_acao = a.seq_calendario_acao
            join	SGE2..projeto_contrato	pc
            on		pc.seq_projeto = p.seq_projeto
            JOIN	SGE2..contrato	c
            on		c.seq_contrato = pc.seq_contrato
            join	SGA..aluno al
            on		c.seq_aluno = '1' + cast (al.cod_aluno as varchar)
            join	SAS..usuario u
            on		al.cod_pessoa = u.cod_pessoa
            left join SAS..usuario_email ue
            on		 ue.seq_usuario = u.seq_usuario
            where	ca.seq_processo_inscricao = {0}
            and a.seq_acao = {1}
            and		c.seq_contrato in (select	seq_contrato
									            FROM SGE2..historico_status_contrato
									            JOIN SGE2..status ON historico_status_contrato.seq_status = status.seq_status
									            WHERE dsc_status = 'DEFERIDO')

            ORDER BY Nome";

        private string _query_coordenador = @"--3) Coordenador Projeto --Para os projetos PIBIC/PIBIT, FIP / Projeto de Pesquisa, IC Voluntário, retornar os dados do coordenador
        select	distinct 
		        --ca.dsc_calendario_acao 'Calendário',
		        --a.seq_acao 'Código Projeto',
		        --a.num_acao,
		        --a.dsc_titulo 'Título',
		        ra.cod_pessoa 'CodigoPessoa',
		        ra.nom_pessoa 'NomeOrientador',
		        ra.dsc_email	'EmailOrientador'
		        --a.seq_tipo_acao_entidade
		
        from	acao..acao	a
        join	acao..calendario_acao	ca
        on		a.seq_calendario_acao = ca.seq_calendario_acao
        join	acao..responsavel_acao ra
        on		a.seq_acao = ra.seq_acao

        where	ca.seq_processo_inscricao in ({0})
        and     a.seq_acao = {1}
        and		a.idt_dom_status in (90,120)
        and		a.seq_tipo_acao_entidade not in (7)
        --order by ca.dsc_calendario_acao, a.seq_acao";

        public string _query_validar_projeto_seminario = @"
            select    
            --i.seq_inscricao
            --tps.dsc_situacao
            idfc.val_campo
            from    inscricao i
            join    inscricao_historico_situacao ihs
                    on i.seq_inscricao = ihs.seq_inscricao
                    and ihs.ind_atual = 1
            join    tipo_processo_situacao tps
                    on ihs.seq_tipo_processo_situacao = tps.seq_tipo_processo_situacao
                    and tps.dsc_token_situacao in ('INSCRICAO_FINALIZADA', 'INSCRICAO_CONFIRMADA')
            join    inscricao_dado_formulario idf
                    on i.seq_inscricao = idf.seq_inscricao
            join    inscricao_dado_formulario_campo idfc
                    on idf.seq_inscricao_dado_formulario = idfc.seq_inscricao_dado_formulario
                    and idfc.dsc_token_elemento = 'PROJETO'
                    and idfc.val_campo like '%{0}%'
            where    i.seq_processo = {1}
            and      i.seq_inscricao <> {2}
            ";

        public string _query_formulario_gpc_campo_projeo = @"select  idfc.val_campo
        from    inscricao i
        join    inscricao_dado_formulario idf
                on i.seq_inscricao = idf.seq_inscricao
        join    inscricao_dado_formulario_campo idfc
                on idf.seq_inscricao_dado_formulario = idfc.seq_inscricao_dado_formulario
                and idfc.dsc_token_elemento = 'PROJETO'
        where   i.seq_inscricao = {0}";

        //public string _query_emitir_documentacao_incricao = @"select distinct
        //   i.seq_inscricao as SeqInscricao
        //,   p.dsc_processo as Processo
        //,   case when nom_social_inscrito is not null then nom_social_inscrito else nom_inscrito end as NomeInscrito
        //,   p.dat_inicio_evento as DataInicioEvento
        //,   p.dat_fim_evento as DataFimEvento
        //,   cmd.seq_tipo_documento as DocumentoTipoDocumento
        //,   td.dsc_token as DocumentoTokenTipoDocumento
        //,   cmd.ind_assinatura_eletronica as DocumentoAssinaturaEletronica
        //,   cmd.dsc_token_configuracao_documento_gad as DocumentoTokenConfiguracao_GAD
        //,   cmd.ind_requer_checkin as DocumentoRequerCheckin
        //,   cmd.ind_exibe_documento_home as DocumentoExibeHome
        //,   cmd.dat_inicio_documento_home as DocumentoDatInicioHome
        //,   cmd.dat_fim_documento_home as DocumentoDataFimHome
        //,   cmd.seq_configuracao_modelo_documento as SeqConfiguracaoModeloDocumento

        //,   (case when exists(select 1 from  
        //        SGF..etapa e     
        //        join SGF..situacao_etapa se
        //        on   se.seq_etapa = e.seq_etapa
        //        join SGF..situacao s
        //        on   s.seq_situacao = se.seq_situacao
        //        where  s.dsc_token like 'INSCRICAO_DEFERIDA' and e.seq_template_processo = p.seq_template_processo_sgf) then 1 else 0 end) as Processo_Possui_Deferimento
        //from  inscrito icto

        //join  inscricao i
        //on   i.seq_inscrito = icto.seq_inscrito

        //join  processo p
        //on   p.seq_processo = i.seq_processo

        //join  configuracao_modelo_documento cmd
        //on   cmd.seq_processo = i.seq_processo

        //join  tipo_documento tdgpi
        //on   tdgpi.seq_tipo_documento = cmd.seq_tipo_documento

        //join  DadosMestres..tipo_documento td
        //on   td.seq_tipo_documento = tdgpi.seq_tipo_documento

        //where  i.seq_inscricao = {0}
        //   and td.seq_tipo_documento = {1}";

        public string _query_emitir_documentacao_incricao = @"select distinct
            i.seq_inscricao as SeqInscricao
        ,   p.seq_processo as SeqProcesso
        ,   p.dsc_processo as Processo
        ,   p.seq_template_processo_sgf as SeqTemplateProcessoSGF
        ,   case when nom_social_inscrito is not null then nom_social_inscrito else nom_inscrito end as NomeInscrito
        ,   p.dat_inicio_evento as DataInicioEvento
        ,   p.dat_fim_evento as DataFimEvento
        ,   cmd.seq_tipo_documento as DocumentoTipoDocumento
        ,   cmd.ind_assinatura_eletronica as DocumentoAssinaturaEletronica
        ,   cmd.dsc_token_configuracao_documento_gad as DocumentoTokenConfiguracao_GAD
        ,   cmd.ind_requer_checkin as DocumentoRequerCheckin
        ,   cmd.ind_exibe_documento_home as DocumentoExibeHome
        ,   cmd.dat_inicio_documento_home as DocumentoDatInicioHome
        ,   cmd.dat_fim_documento_home as DocumentoDataFimHome
        ,   cmd.seq_configuracao_modelo_documento as SeqConfiguracaoModeloDocumento

        from  inscrito icto

        join  inscricao i
        on   i.seq_inscrito = icto.seq_inscrito

        join  processo p
        on   p.seq_processo = i.seq_processo

        join  configuracao_modelo_documento cmd
        on   cmd.seq_processo = i.seq_processo

        join  tipo_documento tdgpi
        on   tdgpi.seq_tipo_documento = cmd.seq_tipo_documento

        where  i.seq_inscricao = {0}
           and cmd.seq_tipo_documento = {1}";

        #endregion Query

        #region DomainService

        private InscricaoBoletoTituloDomainService InscricaoBoletoTituloDomainService => Create<InscricaoBoletoTituloDomainService>();
        private InscricaoBoletoDomainService InscricaoBoletoDomainService => Create<InscricaoBoletoDomainService>();
        private ConfiguracaoEtapaDomainService ConfiguracaoEtapaDomainService
        {
            get { return this.Create<ConfiguracaoEtapaDomainService>(); }
        }
        private OfertaDomainService OfertaDomainService
        {
            get { return this.Create<OfertaDomainService>(); }
        }
        private InscricaoHistoricoSituacaoDomainService InscricaoHistoricoSituacaoDomainService
        {
            get { return this.Create<InscricaoHistoricoSituacaoDomainService>(); }
        }
        private InscricaoOfertaDomainService InscricaoOfertaDomainService
        {
            get { return this.Create<InscricaoOfertaDomainService>(); }
        }
        private TipoProcessoSituacaoDomainService TipoProcessoSituacaoDomainService
        {
            get { return this.Create<TipoProcessoSituacaoDomainService>(); }
        }
        private DocumentoRequeridoDomainService DocumentoRequeridoDomainService
        {
            get { return this.Create<DocumentoRequeridoDomainService>(); }
        }
        private ProcessoIdiomaDomainService ProcessoIdiomaDomainService
        {
            get { return this.Create<ProcessoIdiomaDomainService>(); }
        }
        private InscricaoEnvioNotificacaoDomainService InscricaoEnvioNotificacaoDomainService
        {
            get { return this.Create<InscricaoEnvioNotificacaoDomainService>(); }
        }
        private GrupoOfertaDomainService GrupoOfertaDomainService
        {
            get { return this.Create<GrupoOfertaDomainService>(); }
        }
        private InscritoDomainService InscritoDomainService
        {
            get { return this.Create<InscritoDomainService>(); }
        }
        private PermissaoInscricaoForaPrazoInscritoDomainService PermissaoInscricaoForaPrazoInscritoDomainService
        {
            get { return Create<PermissaoInscricaoForaPrazoInscritoDomainService>(); }
        }
        private ProcessoDomainService ProcessoDomainService
        {
            get { return Create<ProcessoDomainService>(); }
        }
        private TipoProcessoDomainService TipoProcessoDomainService => Create<TipoProcessoDomainService>();
        private InscricaoHistoricoPaginaDomainService InscricaoHistoricoPaginaDomainService => Create<InscricaoHistoricoPaginaDomainService>();
        private InscricaoDadoFormularioDomainService InscricaoDadoFormularioDomainService => Create<InscricaoDadoFormularioDomainService>();
        private InscricaoDocumentoDomainService InscricaoDocumentoDomainService => Create<InscricaoDocumentoDomainService>();
        private InscricaoOfertaHistoricoSituacaoDomainService InscricaoOfertaHistoricoSituacaoDomainService => Create<InscricaoOfertaHistoricoSituacaoDomainService>();
        private ArquivoAnexadoDomainService ArquivoAnexadoDomainService => Create<ArquivoAnexadoDomainService>();
        InscricaoDadoFormularioCampoDomainService InscricaoDadoFormularioCampoDomainService => Create<InscricaoDadoFormularioCampoDomainService>();
        OfertaPeriodoTaxaDomainService OfertaPeriodoTaxaDomainService => Create<OfertaPeriodoTaxaDomainService>();
        private PortfolioApiDoaminService PortfolioApiDoaminService => Create<PortfolioApiDoaminService>();
        private ProcessoApiDoaminService ProcessoApiDoaminService => Create<ProcessoApiDoaminService>();
        private ArquivoApiDoaminService ArquivoApiDoaminService => Create<ArquivoApiDoaminService>();
        private ConfiguracaoModeloDocumentoDomainService ConfiguracaoModeloDocumentoDomainService => Create<ConfiguracaoModeloDocumentoDomainService>();

        #endregion DomainService

        #region Services

        private ISituacaoService SituacaoService
        {
            get { return this.Create<ISituacaoService>(); }
        }

        private IIntegracaoFinanceiroService IntegracaoFinanceiroService
        {
            get { return Create<IIntegracaoFinanceiroService>(); }
        }

        private IUsuarioService UsuarioService { get => Create<IUsuarioService>(); }

        private IPessoaService PessoaService => Create<IPessoaService>();

        private INotificacaoService NotificacaoService => Create<INotificacaoService>();

        private IIntegracaoAcademicoService IntegracaoAcademicoService => Create<IIntegracaoAcademicoService>();

        private ITemplateProcessoService TemplateProcessoService => Create<ITemplateProcessoService>();

        #endregion Services

        #region Variaveis do contexto
        private List<string> arquivosDeletadosGED = new List<string>();
        #endregion

        #region Regras de negócio

        /// <summary>
        /// Verificar regras para iniciar ou continuar nova inscrição
        /// REGRAS:
        /// 1. Um inscrito não poderá iniciar uma nova inscrição ou continuar uma inscrição já iniciada se ele já
        /// tiver atingido o limite permitido de inscrições para o processo em questão. Para o cálculo da quantidade
        /// de inscrições de um inscrito para um processo, não contabilizar inscrições com situação cancelada ou
        /// iniciada.
        /// 2. Um inscrito não poderá iniciar uma nova inscrição para um grupo de um processo se ele já possui uma
        /// inscrição não finalizada para o mesmo. Neste caso o inscrito deverá dar continuidade à inscrição iniciada,
        /// podendo alterá-la, se desejar.
        /// </summary>
        /// <param name="seqInscrito">Sequencial do inscrito</param>
        /// <param name="seqConfiguracaoEtapa">Sequencial da configuração etapa</param>
        /// <param name="seqGrupoOferta">Sequencial do grupo de oferta</param>
        /// <param name="seqInscricao">Sequencial da inscrição, caso esteja continuando</param>
        /// <returns>TRUE caso tenha permissão para iniciar ou continuar nova inscrição, FALSE caso contrário</returns>
        public bool VerificarPermissaoIniciarContinuarInscricao(long seqInscrito, long seqConfiguracaoEtapa, long seqGrupoOferta, long? seqInscricao)
        {
            // Busca as informações da configuração
            IncludesConfiguracaoEtapa includesConfig = IncludesConfiguracaoEtapa.EtapaProcesso |
                                                       IncludesConfiguracaoEtapa.EtapaProcesso_Processo |
                                                       IncludesConfiguracaoEtapa.EtapaProcesso_Processo_TipoProcesso;
            var spec = new SMCSeqSpecification<ConfiguracaoEtapa>(seqConfiguracaoEtapa);
            ConfiguracaoEtapa config = ConfiguracaoEtapaDomainService.SearchByKey(spec, includesConfig);
            if (config == null)
                throw new ConfiguracaoEtapaInvalidaException();

            // Busca as inscrições deste inscrito no processo da configuração
            IncludesInscricao includesInsc = IncludesInscricao.HistoricosSituacao |
                                             IncludesInscricao.HistoricosSituacao_TipoProcessoSituacao |
                                             IncludesInscricao.GrupoOferta;
            InscricaoFilterSpecification specInsc = new InscricaoFilterSpecification()
            {
                SeqInscrito = seqInscrito,
                SeqProcesso = config.EtapaProcesso.SeqProcesso
            };
            specInsc.SetOrderByDescending(x => x.DataInscricao);
            IEnumerable<Inscricao> inscricoes = this.SearchBySpecification(specInsc, includesInsc);

            // Conta quantas inscrições não iniciadas ou canceladas e Verifica regra nº 2
            int numInscricoes = 0;
            bool existeInscricaoGrupo = false;
            string tokenSituacaoInscricao = null;
            foreach (var inscricao in inscricoes)
            {
                InscricaoHistoricoSituacao situacao = inscricao.HistoricosSituacao.Where(h => h.Atual).First();
                if (situacao != null &&
                    !situacao.TipoProcessoSituacao.Token.Equals(TOKENS.SITUACAO_INSCRICAO_INICIADA) &&
                    !situacao.TipoProcessoSituacao.Token.Equals(TOKENS.SITUACAO_INSCRICAO_CANCELADA))
                {
                    numInscricoes++;
                }


                // Verificar regra nº 2
                if (situacao != null &&
                    (!seqInscricao.HasValue || seqInscricao.Value == 0) &&
                    situacao.TipoProcessoSituacao.Token.Equals(TOKENS.SITUACAO_INSCRICAO_INICIADA) &&
                    inscricao.SeqGrupoOferta == seqGrupoOferta)
                {
                    existeInscricaoGrupo = true;
                }
            }

            // Verifica regra nº 1
            if (config.EtapaProcesso.Processo.MaximoInscricoesPorInscrito != 0 && numInscricoes >= config.EtapaProcesso.Processo.MaximoInscricoesPorInscrito)
            {

                throw new MaximoInscricoesPorInscritoAtingidoException(config.EtapaProcesso.Processo.TipoProcesso.TokenResource, new Exception($"GuidProcesso:{config.EtapaProcesso.Processo.UidProcesso}"));
            }
            else if (config.EtapaProcesso.Processo.DataCancelamento.HasValue && config.EtapaProcesso.Processo.DataCancelamento <= DateTime.Now.Date)
            {
                new ProcessoCanceladoException();
            }
            else if (config.EtapaProcesso.SituacaoEtapa == SituacaoEtapa.EmManutencao)
            {
                switch (config.EtapaProcesso.Processo.TipoProcesso.TokenResource)
                {
                    case TOKENS.TOKEN_RESOURCE_AGENDAMENTO:
                        throw new EtapaProcessoEmManutencaoException("agendamento");
                    case TOKENS.TOKEN_RESOURCE_ENTREGA_DOCUMENTACAO:
                        throw new EtapaProcessoEmManutencaoException("entrega de documentação");
                    default:
                        throw new EtapaProcessoEmManutencaoException("inscrição");
                }
            }
            else if (existeInscricaoGrupo)
            {


                throw new JaExisteInscricaoIniciadaParaEsseGrupoOfertaException(config.EtapaProcesso.Processo.TipoProcesso.TokenResource, new Exception($"GuidProcesso:{config.EtapaProcesso.Processo.UidProcesso}"));
            }

            return true;
        }

        public (string mensagem, bool habilitarBotao) VerificarPermissaoIniciarContinuarInscricaoBotoes(long seqInscrito, long seqConfiguracaoEtapa, long seqGrupoOferta, long? seqInscricao)
        {
            // Busca as informações da configuração
            IncludesConfiguracaoEtapa includesConfig = IncludesConfiguracaoEtapa.EtapaProcesso |
                                                       IncludesConfiguracaoEtapa.EtapaProcesso_Processo |
                                                       IncludesConfiguracaoEtapa.EtapaProcesso_Processo_TipoProcesso;
            var spec = new SMCSeqSpecification<ConfiguracaoEtapa>(seqConfiguracaoEtapa);
            ConfiguracaoEtapa config = ConfiguracaoEtapaDomainService.SearchByKey(spec, includesConfig);
            if (config == null)
                throw new ConfiguracaoEtapaInvalidaException();

            // Busca as inscrições deste inscrito no processo da configuração
            IncludesInscricao includesInsc = IncludesInscricao.HistoricosSituacao |
                                             IncludesInscricao.HistoricosSituacao_TipoProcessoSituacao |
                                             IncludesInscricao.GrupoOferta;
            InscricaoFilterSpecification specInsc = new InscricaoFilterSpecification()
            {
                SeqInscrito = seqInscrito,
                SeqProcesso = config.EtapaProcesso.SeqProcesso
            };
            specInsc.SetOrderByDescending(x => x.DataInscricao);
            IEnumerable<Inscricao> inscricoes = this.SearchBySpecification(specInsc, includesInsc);

            // Conta quantas inscrições não iniciadas ou canceladas e Verifica regra nº 2
            int numInscricoes = 0;
            bool existeInscricaoGrupo = false;
            foreach (var inscricao in inscricoes)
            {
                InscricaoHistoricoSituacao situacao = inscricao.HistoricosSituacao.Where(h => h.Atual).First();
                if (situacao != null &&
                    !situacao.TipoProcessoSituacao.Token.Equals(TOKENS.SITUACAO_INSCRICAO_INICIADA) &&
                    !situacao.TipoProcessoSituacao.Token.Equals(TOKENS.SITUACAO_INSCRICAO_CANCELADA))
                {
                    numInscricoes++;
                }

                // Verificar regra nº 2
                if (situacao != null &&
                    (!seqInscricao.HasValue || seqInscricao.Value == 0) &&
                    situacao.TipoProcessoSituacao.Token.Equals(TOKENS.SITUACAO_INSCRICAO_INICIADA) &&
                    inscricao.SeqGrupoOferta == seqGrupoOferta)
                {
                    existeInscricaoGrupo = true;
                }
            }

            // Verifica regra nº 1
            if (config.EtapaProcesso.Processo.MaximoInscricoesPorInscrito != 0 && numInscricoes >= config.EtapaProcesso.Processo.MaximoInscricoesPorInscrito)
            {
                var mensagem = SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString($"MaximoInscricoesPorInscritoAtingido{config.EtapaProcesso.Processo.TipoProcesso.TokenResource}Exception");
                return (mensagem, false);
            }
            else if (existeInscricaoGrupo)
            {
                var mensagem = SMC.Inscricoes.Common.Areas.INS.Resources.ExceptionsResource.ResourceManager.GetString($"JaExisteInscricaoIniciadaParaEsseGrupoOferta{config.EtapaProcesso.Processo.TipoProcesso.TokenResource}Exception");
                return (mensagem, false);
            }
            return ("", true);
        }

        /// <summary>
        /// Verifica se é permitido registrar entrega de documentos para inscrição
        /// Se a situação da inscrição for iniciada não é permitido e a exceção com a mensagem
        /// "A inscrição ainda não foi finalizada. Não é possivel registrar a documentação entregue"
        /// é lançada
        /// </summary>
        public bool VerificarSituacaoRegistrarEntregaDocumentos(long seqInscricao)
        {
            var ehIniciada = this.SearchProjectionByKey(new SMCSeqSpecification<Inscricao>(seqInscricao), x =>
                 x.HistoricosSituacao.Any(s => s.Atual && s.TipoProcessoSituacao.Token == TOKENS.SITUACAO_INSCRICAO_INICIADA));
            if (ehIniciada) throw new RegistroDocumentacaoSituacaoInscricaoInvalidaException();
            return ehIniciada;
        }

        /// <summary>
        /// Verifica regras para avaçar com uma inscrição
        /// REGRAS:
        /// 0. Verifica se a inscrição já está confirmada ou finalizada
        /// 1. Se a etapa de inscrição do processo selecionado pelo inscrito encontra-se "Em manutenção", erro
        /// 2. Se o período de inscrição da configuração etapa da inscrição terminou, erro
        /// </summary>
        /// <param name="seqInscricao">Inscrição a ser verificada</param>
        public void VerificarRegrasAvancarInscricao(long seqInscricao, long seqConfiguracaoEtapa, long seqGrupoOferta, long seqInscrito)
        {
            //Revalida Lista de ofertas
            InscricaoOfertaDomainService.ValidaListaOfertas(seqInscricao);

            // Verifica regra 0
            if (seqInscricao > 0)
                this.VerificarInscricaoJaFinalizadaConfirmada(seqInscricao);

            if (seqConfiguracaoEtapa <= 0)
                throw new ConfiguracaoEtapaInvalidaException();

            // Busca as informações de configuração etapa, caso não informada
            var configuracaoEtapa = ConfiguracaoEtapaDomainService.SearchProjectionByKey(seqConfiguracaoEtapa, x => new
            {
                SituacaoEtapaProcesso = x.EtapaProcesso.SituacaoEtapa,
                Vigente = x.DataInicio <= DateTime.Now && DateTime.Now <= x.DataFim,
                x.DataInicio,
                x.DataFim,
                x.EtapaProcesso.Processo.TipoProcesso.TokenResource
            });

            // Busca os dados do Grupo de Oferta
            var grupoOferta = GrupoOfertaDomainService.SearchProjectionByKey(seqGrupoOferta, x => new
            {
                SeqProcesso = x.SeqProcesso,
                SeqTipoProcesso = x.Processo.SeqTipoProcesso,
                Nome = x.Nome,
                DataEncerramentoProcessso = x.Processo.DataEncerramento,
                DataCancelamentoProcesso = x.Processo.DataCancelamento
            });

            // Verifica regra 1
            if (configuracaoEtapa.SituacaoEtapaProcesso == SituacaoEtapa.EmManutencao)
            {
                switch (configuracaoEtapa.TokenResource)
                {
                    case TOKENS.TOKEN_RESOURCE_AGENDAMENTO:
                        throw new EtapaProcessoEmManutencaoException("agendamento");
                    case TOKENS.TOKEN_RESOURCE_ENTREGA_DOCUMENTACAO:
                        throw new EtapaProcessoEmManutencaoException("entrega de documentação");
                    default:
                        throw new EtapaProcessoEmManutencaoException("inscrição");
                }
            }

            // Verifica a regra 2
            if (!configuracaoEtapa.Vigente && !InscritoDomainService.VerificaPermissaoInscricaoForaPrazo(grupoOferta.SeqProcesso))
            {
                switch (configuracaoEtapa.TokenResource)
                {
                    case TOKENS.TOKEN_RESOURCE_AGENDAMENTO:
                        throw new ConfiguracaoEtapaNaoVigenteException("agendamento", grupoOferta.Nome);
                    case TOKENS.TOKEN_RESOURCE_ENTREGA_DOCUMENTACAO:
                        throw new ConfiguracaoEtapaNaoVigenteException("entrega de documentação", grupoOferta.Nome);
                    default:
                        throw new ConfiguracaoEtapaNaoVigenteException("inscrição", grupoOferta.Nome);
                }
            }

            if (grupoOferta.DataCancelamentoProcesso.HasValue && grupoOferta.DataCancelamentoProcesso <= DateTime.Now.Date)
            {
                throw new ProcessoCanceladoException();
            }

            if (grupoOferta.DataEncerramentoProcessso.HasValue && grupoOferta.DataEncerramentoProcessso <= DateTime.Now.Date)
            {
                throw new ProcessoEncerradoException();
            }

            if (seqInscricao != 0)
            {
                var ofertaIndisponivel = OfertaDomainService.SearchProjectionByKey(new OfertaInscricaoIndisponivelSpecification() { SeqInscricao = seqInscricao }, x => new
                {
                    x.DescricaoCompleta
                });
                if (ofertaIndisponivel != null)
                {
                    switch (configuracaoEtapa.TokenResource)
                    {
                        case TOKENS.TOKEN_RESOURCE_AGENDAMENTO:
                            throw new OfertaNaoDisponivelException("O", "agendamento", ofertaIndisponivel.DescricaoCompleta);
                        case TOKENS.TOKEN_RESOURCE_ENTREGA_DOCUMENTACAO:
                            throw new OfertaNaoDisponivelException("A", "entrega de documentação", ofertaIndisponivel.DescricaoCompleta);
                        default:
                            throw new OfertaNaoDisponivelException("A", "inscrição", ofertaIndisponivel.DescricaoCompleta);
                    }
                }
            }

            var validationCacheKey = $"GPIValidationCache{seqInscrito}";

            // Verifica se as consistências foram conferidas. Podem ser executadas apenas uma vez durante o fluxo de páginas, para reduzir processamento.
            if (SMCCacheManager.Get(validationCacheKey) == null)
            {
                var inscrito = InscritoDomainService.SearchProjectionByKey(seqInscrito, x => new
                {
                    x.Cpf,
                    x.NumeroPassaporte
                });

                if (TipoProcessoDomainService.PossuiConsistencia(grupoOferta.SeqTipoProcesso, TipoConsistencia.Desligamento))
                {
                    var filtro = new Pessoas.ServiceContract.Areas.PES.Data.PessoaDesligadaFiltroData()
                    {
                        Cpf = inscrito.Cpf,
                        Data = DateTime.Now
                    };
                    // (BUG 59343) Só realiza a pesquisa em pessoas desligadas por passaporte se o cpf não estiver preenchido
                    if (string.IsNullOrEmpty(inscrito.Cpf))
                        filtro.Passaporte = inscrito.NumeroPassaporte;
                    if (PessoaService.VerificarDadosPessoa(filtro))
                    {
                        switch (configuracaoEtapa.TokenResource)
                        {
                            case TOKENS.TOKEN_RESOURCE_AGENDAMENTO:
                                throw new InscricaoNaoPermitidaException("Agendamento", "o");
                            case TOKENS.TOKEN_RESOURCE_ENTREGA_DOCUMENTACAO:
                                throw new InscricaoNaoPermitidaException("Entrega de documentação", "a");
                            default:
                                throw new InscricaoNaoPermitidaException("Inscrição", "a");
                        }
                    }
                }

                if (TipoProcessoDomainService.PossuiConsistencia(grupoOferta.SeqTipoProcesso, TipoConsistencia.DesligamentoAcademico))
                {
                    var filtro = new Pessoas.ServiceContract.Areas.PES.Data.PessoaDesligadaFiltroData()
                    {
                        Cpf = inscrito.Cpf,
                        Data = DateTime.Now,
                        TipoDesligamento = 1 //Restrição aluno
                    };
                    // (BUG 59343) Só realiza a pesquisa em pessoas desligadas por passaporte se o cpf não estiver preenchido
                    if (string.IsNullOrEmpty(inscrito.Cpf))
                        filtro.Passaporte = inscrito.NumeroPassaporte;
                    if (PessoaService.VerificarDadosPessoa(filtro))
                    {
                        switch (configuracaoEtapa.TokenResource)
                        {
                            case TOKENS.TOKEN_RESOURCE_AGENDAMENTO:
                                throw new InscricaoNaoPermitidaException("Agendamento", "o");
                            case TOKENS.TOKEN_RESOURCE_ENTREGA_DOCUMENTACAO:
                                throw new InscricaoNaoPermitidaException("Entrega de documentação", "a");
                            default:
                                throw new InscricaoNaoPermitidaException("Inscrição", "a");
                        }
                    }
                }
                SMCCacheManager.Add(validationCacheKey, true, new TimeSpan(0, 20, 0));
            }
        }

        /// <summary>
        /// Verifica regras para avaçar com uma inscrição
        /// REGRAS:
        /// 0. Verifica se a inscrição já está confirmada ou finalizada
        /// 1. Se a etapa de inscrição do processo selecionado pelo inscrito encontra-se "Em manutenção", erro
        /// 2. Se o período de inscrição da configuração etapa da inscrição terminou, erro
        /// </summary>
        /// <param name="seqInscricao">Inscrição a ser verificada</param>
        public void VerificarRegrasAvancarInscricao(Inscricao inscricao)
        {
            // Verifica regra 0
            if (inscricao.Seq > 0)
                this.VerificarInscricaoJaFinalizadaConfirmada(inscricao.Seq);



            if (inscricao.SeqConfiguracaoEtapa <= 0)
                throw new ConfiguracaoEtapaInvalidaException();

            // Busca as informações de configuração etapa, caso não informada
            if (inscricao.ConfiguracaoEtapa == null ||
                (inscricao.ConfiguracaoEtapa != null && inscricao.ConfiguracaoEtapa.EtapaProcesso == null))
            {
                IncludesConfiguracaoEtapa includes = IncludesConfiguracaoEtapa.EtapaProcesso;
                SMCSeqSpecification<ConfiguracaoEtapa> spec = new SMCSeqSpecification<ConfiguracaoEtapa>(inscricao.SeqConfiguracaoEtapa);
                inscricao.ConfiguracaoEtapa = ConfiguracaoEtapaDomainService.SearchByKey(spec, includes);
            }

            // Busca as informações do grupo de oferta, caso não tenha informado
            if (inscricao.GrupoOferta == null)
            {
                inscricao.GrupoOferta = GrupoOfertaDomainService.SearchByKey(new SMCSeqSpecification<GrupoOferta>(inscricao.SeqGrupoOferta));
            }

            var processo = this.GrupoOfertaDomainService.SearchProjectionByKey(new SMCSeqSpecification<GrupoOferta>(inscricao.SeqGrupoOferta),
                                                            f => new
                                                            {
                                                                Seq = f.SeqProcesso,
                                                                f.Processo,
                                                                f.Processo.TipoProcesso,
                                                                f.Processo.TipoProcesso.TokenResource
                                                            });
            // Verifica regra 1
            if (processo.Processo.DataCancelamento.HasValue && processo.Processo.DataCancelamento <= DateTime.Now.Date)
            {
                throw new ProcessoCanceladoException();
            }

            if (inscricao.ConfiguracaoEtapa.EtapaProcesso.SituacaoEtapa == SituacaoEtapa.EmManutencao)
            {
                switch (processo.TokenResource)
                {
                    case TOKENS.TOKEN_RESOURCE_AGENDAMENTO:
                        throw new EtapaProcessoEmManutencaoException("agendamento");
                    case TOKENS.TOKEN_RESOURCE_ENTREGA_DOCUMENTACAO:
                        throw new EtapaProcessoEmManutencaoException("entrega de documentação");
                    default:
                        throw new EtapaProcessoEmManutencaoException("inscrição");
                }
            }

            // Verifica a regra 2
            if (!inscricao.ConfiguracaoEtapa.Vigente && !InscritoDomainService.VerificaPermissaoInscricaoForaPrazo(processo.Seq))
            {
                switch (processo.TokenResource)
                {
                    case TOKENS.TOKEN_RESOURCE_AGENDAMENTO:
                        throw new ConfiguracaoEtapaNaoVigenteException("agendamento", inscricao.GrupoOferta.Nome);
                    case TOKENS.TOKEN_RESOURCE_ENTREGA_DOCUMENTACAO:
                        throw new ConfiguracaoEtapaNaoVigenteException("entrega de documentação", inscricao.GrupoOferta.Nome);
                    default:
                        throw new ConfiguracaoEtapaNaoVigenteException("inscrição", inscricao.GrupoOferta.Nome);
                }
            }

            if (inscricao.Seq != 0)
            {
                var ofertasIndisponiveis = OfertaDomainService.SearchBySpecification(
                                    new OfertaInscricaoIndisponivelSpecification() { SeqInscricao = inscricao.Seq });
                var pagina = InscricaoHistoricoPaginaDomainService.BuscarUltimaPaginaInscricao(inscricao.Seq);
                if (ofertasIndisponiveis.Any() && pagina.ConfiguracaoEtapaPagina.Token == TOKENS.PAGINA_COMPROVANTE_INSCRICAO)
                {
                    switch (processo.TokenResource)
                    {
                        case TOKENS.TOKEN_RESOURCE_AGENDAMENTO:
                            throw new OfertaNaoDisponivelException("O", "agendamento", ofertasIndisponiveis.First().DescricaoCompleta);
                        case TOKENS.TOKEN_RESOURCE_ENTREGA_DOCUMENTACAO:
                            throw new OfertaNaoDisponivelException("A", "entrega de documentação", ofertasIndisponiveis.First().DescricaoCompleta);
                        default:
                            throw new OfertaNaoDisponivelException("A", "inscrição", ofertasIndisponiveis.First().DescricaoCompleta);
                    }
                }
            }

            var validationCacheKey = $"GPIValidationCache{inscricao.SeqInscrito}";
            // Verifica se as consistências foram conferidas. Podem ser executadas apenas uma vez durante o fluxo de páginas, para reduzir processamento.
            if (SMCCacheManager.Get(validationCacheKey) == null)
            {
                var inscrito = InscritoDomainService.SearchByKey(new SMCSeqSpecification<Inscrito>(inscricao.SeqInscrito));
                if (TipoProcessoDomainService.PossuiConsistencia(processo.TipoProcesso, TipoConsistencia.Desligamento))
                {
                    var filtro = new Pessoas.ServiceContract.Areas.PES.Data.PessoaDesligadaFiltroData()
                    {
                        Cpf = inscrito.Cpf,
                        Data = DateTime.Now
                    };
                    // (BUG 59343) Só realiza a pesquisa em pessoas desligadas por passaporte se o cpf não estiver preenchido
                    if (string.IsNullOrEmpty(inscrito.Cpf))
                        filtro.Passaporte = inscrito.NumeroPassaporte;
                    if (PessoaService.VerificarDadosPessoa(filtro))
                    {
                        switch (processo.TokenResource)
                        {
                            case TOKENS.TOKEN_RESOURCE_AGENDAMENTO:
                                throw new InscricaoNaoPermitidaException("Agendamento", "o");
                            case TOKENS.TOKEN_RESOURCE_ENTREGA_DOCUMENTACAO:
                                throw new InscricaoNaoPermitidaException("Entrega de documentação", "a");
                            default:
                                throw new InscricaoNaoPermitidaException("Inscrição", "a");
                        }
                    }
                }

                if (TipoProcessoDomainService.PossuiConsistencia(processo.TipoProcesso, TipoConsistencia.DesligamentoAcademico))
                {
                    var filtro = new Pessoas.ServiceContract.Areas.PES.Data.PessoaDesligadaFiltroData()
                    {
                        Cpf = inscrito.Cpf,
                        Data = DateTime.Now,
                        TipoDesligamento = 1 //Restrição aluno
                    };
                    // (BUG 59343) Só realiza a pesquisa em pessoas desligadas por passaporte se o cpf não estiver preenchido
                    if (string.IsNullOrEmpty(inscrito.Cpf))
                        filtro.Passaporte = inscrito.NumeroPassaporte;
                    if (PessoaService.VerificarDadosPessoa(filtro))
                    {
                        switch (processo.TokenResource)
                        {
                            case TOKENS.TOKEN_RESOURCE_AGENDAMENTO:
                                throw new InscricaoNaoPermitidaException("Agendamento", "o");
                            case TOKENS.TOKEN_RESOURCE_ENTREGA_DOCUMENTACAO:
                                throw new InscricaoNaoPermitidaException("Entrega de documentação", "a");
                            default:
                                throw new InscricaoNaoPermitidaException("Inscrição", "a");
                        }
                    }
                }
                SMCCacheManager.Add(validationCacheKey, true, new TimeSpan(0, 20, 0));
            }
        }

        /// <summary>
        /// Verifica se uma inscrição pode ser alterada.
        /// Regra:
        /// - Uma inscrição só pode ser alterada se não estiver finalizada
        /// </summary>
        /// <param name="seqInscricao">Sequencial da inscrição a ser consistida</param>
        public void VerificarInscricaoJaFinalizadaConfirmada(long seqInscricao)
        {
            var dadosInscricao = this.SearchProjectionByKey(seqInscricao, x => new
            {
                SeqTipoProcesso = x.Processo.SeqTipoProcesso,
                x.SeqProcesso,
                x.Processo.TipoProcesso.IntegraGPC,
                HistoricosSituacao = x.HistoricosSituacao.Select(h => new
                {
                    h.Seq,
                    h.Atual,
                    h.SeqTipoProcessoSituacao
                }).ToList(),
                x.Processo.TipoProcesso.TokenResource
            });

            // Caso não ache dados da inscrição
            if (dadosInscricao == null)
                throw new InscricaoInvalidaException();

            // Encontra a situação com token TOKENS.SITUACAO_INSCRICAO_FINALIZADA ou TOKENS.SITUACAO_INSCRICAO_CONFIRMADA
            var spec = new TipoProcessoSituacaoFilterSpecification()
            {
                SeqTipoProcesso = dadosInscricao.SeqTipoProcesso,
                Tokens = new string[] { TOKENS.SITUACAO_INSCRICAO_FINALIZADA, TOKENS.SITUACAO_INSCRICAO_CONFIRMADA }
            };

            var tiposProcessoSituacao = TipoProcessoSituacaoDomainService.SearchProjectionBySpecification(spec, w => new { w.Seq, w.Token }).ToList();
            if (tiposProcessoSituacao.Count == 0)
                throw new SituacaoTokenNaoIdentificadaException(string.Join(" ou ", new string[] { TOKENS.SITUACAO_INSCRICAO_FINALIZADA, TOKENS.SITUACAO_INSCRICAO_CONFIRMADA }));

            /*
             Ocorreu uma situação em que quando o processo está configurado para ser finalizado automaticamente,
             o sistema estava validando novamente de a inscrição encontrava-se finalizada.
             Foi necessário acrescentar uma validação para esta caso, contemplando também inscrições confirnadas
             */
            // Verifica se inscrição já está na situação TOKENS.SITUACAO_INSCRICAO_FINALIZADA ou TOKENS.SITUACAO_INSCRICAO_CONFIRMADA
            if (dadosInscricao.HistoricosSituacao.Any(h => tiposProcessoSituacao.Select(t => t.Seq).ToList().Contains(h.SeqTipoProcessoSituacao) && h.Atual))
            {
                switch (dadosInscricao.TokenResource)
                {
                    case TOKENS.TOKEN_RESOURCE_AGENDAMENTO:
                        throw new InscricaoJaFinalizadaAgendamentoException();
                    case TOKENS.TOKEN_RESOURCE_ENTREGA_DOCUMENTACAO:
                        throw new InscricaoJaFinalizadaEntregaDocumentacaoException();
                    default:
                        throw new InscricaoJaFinalizadaInscricaoException();
                }
            }
        }

        public bool VerificaApenasInscricoesTeste(long[] seqInscricoes)
        {
            // Se não existirem inscrição, então não existem apenas inscrições de teste.
            if (seqInscricoes.Length == 0)
                return false;

            var motivosTeste = SituacaoService.BuscarSeqMotivosSituacaoPorToken(TOKENS.MOTIVO_INSCRICAO_CANCELADA_TESTE);
            return this.Count(new InscricaoTesteSpecification(seqInscricoes, motivosTeste)) == seqInscricoes.Length;
        }

        public bool VerificaBoletoInscricaoAlteracaoTaxa(long seqInscricao, List<InscricaoTaxaVO> taxas)
        {
            // Caso a inscrição já tenha boleto
            var inscricao = this.SearchProjectionByKey(seqInscricao, i => new
            {
                InscricaoPossuiTituloBoleto = i.Boletos.Any(b => b.Titulos.Any(x => !x.DataCancelamento.HasValue || x.DataCancelamento.Value >= DateTime.Now)),
            });
            if (inscricao.InscricaoPossuiTituloBoleto)
            {
                // Pega os dados que já estão salvos na tela
                var dadosOriginaisTela = InscricaoBoletoDomainService.BuscarTaxasSalvasNoBoletoInscricao(seqInscricao);

                if (taxas != null && taxas.Count > 0)
                {
                    var taxasPorOferta = taxas.Where(w => w.NumeroItens > 0 && w.TipoCobranca == TipoCobranca.PorOferta).ToList();

                    var taxasDiferentePorOferta = taxas.Where(w => w.NumeroItens > 0 && w.TipoCobranca != TipoCobranca.PorOferta).SMCDistinct(s => s.SeqTaxa).ToList();

                    var quantidadeTaxas = taxasPorOferta.Count() + taxasDiferentePorOferta.Count();

                    if ((dadosOriginaisTela?.Count() ?? 0) != quantidadeTaxas)
                        return true;
                }


                // Verifica se houve alguma mudança tanto na quantidade de itens quanto no valor
                foreach (var itemBanco in dadosOriginaisTela)
                {
                    InscricaoTaxaVO itemPost = new InscricaoTaxaVO();

                    itemPost = taxas.FirstOrDefault(t => t.SeqTaxa == itemBanco.SeqTaxa);

                    if (itemPost.TipoCobranca == TipoCobranca.PorOferta)
                    {
                        itemPost = taxas.FirstOrDefault(t => t.SeqTaxa == itemBanco.SeqTaxa && t.SeqOferta == itemBanco.SeqOferta);
                    }

                    if (itemPost == null)
                        return true;

                    if (itemBanco.NumeroItens != itemPost.NumeroItens)
                        return true;

                    if (itemBanco.NumeroItens > 0 && itemBanco.ValorEventoTaxa != itemPost.ValorItem)
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Valida se o inscrito está apto a receber a bolsta ex-aluno
        /// </summary>
        /// <param name="seqInscricao">Sequencial da inscrição</param>
        /// <param name="seqOferta">Sequencial da oferta a ser validada</param>
        /// <returns>True caso esteja apto</returns>
        public bool ValidarAptoBolsaNovoTitulo(long seqInscricao, long seqOferta)
        {
            if (!TipoProcessoDomainService.BuscarTipoProcessoPorInscricao(seqInscricao).BolsaExAluno)
            {
                return false;
            }
            var vagasBolsaOferta = OfertaDomainService.SearchProjectionByKey(seqOferta, p => new
            {
                Vagas = p.NumeroVagasBolsa,
                Inscricoes = p.InscricoesOferta.Count(c => c.Inscricao.RecebeuBolsa)
            });
            if (vagasBolsaOferta.Inscricoes >= vagasBolsaOferta.Vagas.GetValueOrDefault())
            {
                return false;
            }
            var dadosCandidato = this.SearchProjectionByKey(seqInscricao, p => new CadidatoAptoBolsaObtencaoNovoTituloData()
            {
                Cpf = p.Inscrito.Cpf,
                Ano = p.Processo.AnoReferencia,
                Semestre = p.Processo.SemestreReferencia,
                SeqTipoProcesso = p.Processo.SeqTipoProcesso
            });
            return IntegracaoAcademicoService.ConsultaCadidatoAptoBolsaObtencaoNovoTitulo(dadosCandidato);
        }

        public void CancelarInscricao(long seqInscricao, long seqProcesso, string tokenMotivo)
        {
            // Busca o id do motivo situacao no SGF.
            var seqSituacao = ProcessoDomainService.SearchProjectionByKey(new SMCSeqSpecification<Processo>(seqProcesso),
                                                      x => x.TipoProcesso.Situacoes.FirstOrDefault(f => f.Token == TOKENS.SITUACAO_INSCRICAO_CANCELADA).SeqSituacao);

            var motivo = SituacaoService.BuscarSeqMotivoSituacaoPorToken(seqSituacao, tokenMotivo);
            if (!motivo.HasValue)
                throw new SMCArgumentException(tokenMotivo);

            CancelarInscricao(seqInscricao, seqProcesso, motivo.Value);
        }

        public void CancelarInscricao(long seqInscricao, long seqProcesso, long seqTokenMotivo)
        {
            // Busca a situacao cancelada
            var tipoProcessoSituacao = ProcessoDomainService.SearchProjectionByKey(new SMCSeqSpecification<Processo>(seqProcesso),
                                                                x => x.TipoProcesso.Situacoes.FirstOrDefault(f => f.Token == TOKENS.SITUACAO_INSCRICAO_CANCELADA));

            InscricaoHistoricoSituacaoDomainService.AlterarSituacaoInscricoes(new AlteracaoSituacaoVO()
            {
                SeqInscricoes = new List<long>() { seqInscricao },
                SeqMotivoSGF = seqTokenMotivo,
                SeqTipoProcessoSituacaoDestino = tipoProcessoSituacao.Seq
            });
        }

        public void AlterarAptoBolsa(long seqInscricao, bool apto)
        {
            this.UpdateFields(new Inscricao() { Seq = seqInscricao, AptoBolsa = apto }, i => i.AptoBolsa);
        }
        #endregion Regras de negócio

        #region Inclusão

        /// <summary>
        /// RN_INS_016 - Gravação da inscrição
        /// Inclui uma inscrição
        /// </summary>
        /// <param name="inscricao">Inscrição a ser incluida</param>
        /// <param name="seqGrupoOferta">Grupo de oferta da inscrição recebido por parâmetro do link. Utilizado para a validação da BOLSA SOCIAL.</param>
        /// <param name="consentimentoLGPD">Consentimento ou não com a LGPD</param>
        /// <returns>Sequencial da inscrição criada</returns>
        public long IncluirInscricao(Inscricao inscricao, long seqGrupoOferta, bool consentimentoLGPD)
        {
            ValidarConsistenciaInscritoSeminario(inscricao.SeqConfiguracaoEtapa, inscricao.SeqInscrito);

            // Verifica as regras para iniciar uma inscrição
            VerificarPermissaoIniciarContinuarInscricao(inscricao.SeqInscrito, inscricao.SeqConfiguracaoEtapa, inscricao.SeqGrupoOferta, inscricao.Seq);

            // Verifica as regras para avançar com a inscrição
            VerificarRegrasAvancarInscricao(inscricao);

            // Verificar regra consistência professor da casa
            VerificarConsistenciaProfessorDaCasa(inscricao);

            if (inscricao.Seq > 0)
            {
                //AtualizarGED(inscricao);
                return inscricao.Seq;
            }

            // Busca a data corrente
            DateTime agora = DateTime.Now;

            // Informa o sequencial do processo
            IncludesConfiguracaoEtapa includes = IncludesConfiguracaoEtapa.EtapaProcesso |
                                                 IncludesConfiguracaoEtapa.EtapaProcesso_Processo_TipoProcesso;
            ConfiguracaoEtapa config = ConfiguracaoEtapaDomainService.SearchByKey(new SMCSeqSpecification<ConfiguracaoEtapa>(inscricao.SeqConfiguracaoEtapa), includes);
            if (config == null)
                throw new ConfiguracaoEtapaInvalidaException();
            inscricao.SeqProcesso = config.EtapaProcesso.SeqProcesso;
            inscricao.HabilitaGed = config.EtapaProcesso.Processo.TipoProcesso.HabilitaGed;

            ///////
            // Regras para consistência das duas inscrições da BOLSA SOCIAL. (Lembrar de apagar a spec, a exception e o recurso quando for removido).
            ///////

            ValidarBolsaSocial(config, inscricao.SeqInscrito, seqGrupoOferta);

            ///////
            // Fim das consistências
            ///////

            // Informa a data da inscrição
            inscricao.DataInscricao = agora;

            var possuiDocumentoRequerido = PossuiDocumentoRequerido(inscricao.SeqConfiguracaoEtapa);
            inscricao.SituacaoDocumentacao = possuiDocumentoRequerido ? SituacaoDocumentacao.AguardandoEntrega : SituacaoDocumentacao.NaoRequerida;

            //TASK 41895 Compromisso entrega documentação = Não
            inscricao.CompromissoEntregaDocumentacao = false;

            //TASK 45722 Recebeu bolsta = Não
            inscricao.RecebeuBolsa = false;

            // Busca a situação com TOKENS.SITUACAO_INSCRICAO_INICIADA
            TipoProcessoSituacaoFilterSpecification spec = new TipoProcessoSituacaoFilterSpecification()
            {
                SeqTipoProcesso = config.EtapaProcesso.Processo.SeqTipoProcesso,
                Token = TOKENS.SITUACAO_INSCRICAO_INICIADA
            };
            TipoProcessoSituacao sitInicial = TipoProcessoSituacaoDomainService.SearchByKey(spec);
            if (sitInicial == null)
                throw new SituacaoTokenNaoIdentificadaException(TOKENS.SITUACAO_INSCRICAO_INICIADA);

            IncluirDocumentosInscricao(inscricao);

            RegistraInscriçãoForaPrazo(inscricao.SeqPermissaoInscricaoForaPrazoInscrito, inscricao.SeqInscrito, inscricao.SeqProcesso, inscricao.ConfiguracaoEtapa.Vigente);

            using (var unitOfWork = SMCUnitOfWork.Begin())
            {
                // Salva a inscrição
                this.InsertEntity(inscricao);

                // Inclui a situação inicial da inscrição
                InscricaoHistoricoSituacao situacao = new InscricaoHistoricoSituacao()
                {
                    SeqInscricao = inscricao.Seq,
                    SeqTipoProcessoSituacao = sitInicial.Seq,
                    SeqEtapaProcesso = config.SeqEtapaProcesso,
                    DataSituacao = agora,
                    AtualEtapa = true,
                    Atual = true
                };

                InscricaoHistoricoSituacaoDomainService.InsertEntity(situacao);

                if (TipoProcessoDomainService.PossuiConsistencia(config.EtapaProcesso.Processo.TipoProcesso, TipoConsistencia.DebitoFinanceiro))
                {
                    VerificarDebitoFinanceiro(inscricao, config.EtapaProcesso.Processo.SeqTipoProcesso, unitOfWork);
                }

                unitOfWork.Commit();
            }

            AtualizarGED(inscricao);

            return inscricao.Seq;
        }

        /// <summary>
        /// Atualiza o Ged validar se o portfolio foi criado se não estiver sido cria
        /// Valida se o processo estiver sido criado se não cria e atualiza a inscrição
        /// </summary>
        /// <param name="inscricao">Dados da inscrição</param>
        public void AtualizarGED(Inscricao inscricao)
        {
            var dadosComplementaresInscricao = this.SearchProjectionByKey(inscricao.Seq, p => new { p.SeqProcesso, p.UidProcessoGed });

            CriarPortfolioVO criarPortfolioVO = new CriarPortfolioVO();
            criarPortfolioVO.SeqInscrito = inscricao.SeqInscrito;
            criarPortfolioVO.SeqProcesso = dadosComplementaresInscricao.SeqProcesso;
            var portifolio = PortfolioApiDoaminService.CriarPortfolio(criarPortfolioVO);
            if (portifolio.ExistePortfolio && dadosComplementaresInscricao.UidProcessoGed == null)
            {
                CriarProcessoVO criarProcessoVO = new CriarProcessoVO();
                criarProcessoVO.GuidBiblioteca = portifolio.GuidBiblioteca;
                criarProcessoVO.IdGedPortfolio = portifolio.IdGedPortfolio;
                criarProcessoVO.SeqInscricao = inscricao.Seq;
                var processo = ProcessoApiDoaminService.CriarProcesso(criarProcessoVO);

                if (processo.StatusCode == HttpStatusCode.OK)
                {
                    inscricao.UidProcessoGed = Guid.Parse(processo.IdGedProcesso);
                    UpdateFields(inscricao, u => u.UidProcessoGed);
                }
            }
        }

        public void VerificarPermissaoEmitirComprovante(long seqInscricao)
        {
            // Busca a inscrição para saber se está ou não cancelada
            var inscricaoDados = this.SearchProjectionByKey(
                                        new SMCSeqSpecification<Inscricao>(seqInscricao),
                                        x => new
                                        {
                                            ProcessoCancelado = (x.Processo.DataCancelamento.HasValue && x.Processo.DataCancelamento.Value <= DateTime.Now),
                                            InscricaoCancelada = x.HistoricosSituacao.Any(y => y.Atual && y.TipoProcessoSituacao.Token == TOKENS.SITUACAO_INSCRICAO_CANCELADA),
                                            SituacaoAtual = x.HistoricosSituacao.FirstOrDefault(y => y.Atual).TipoProcessoSituacao.Descricao,
                                            SeqArquivoComprovante = x.SeqArquivoComprovante
                                        });

            if (inscricaoDados == null)
                return;

            // Não permite emissao de comprovante se não tem arquivo
            if (!inscricaoDados.SeqArquivoComprovante.HasValue)
                throw new SMCApplicationException("Não existe comprovante para emissão.");

            // Não permite emissao de comprovante caso o processo de inscrição esteja cancelado
            if (inscricaoDados.ProcessoCancelado)
                throw new ComprovanteInscricaoCanceladaException("Processo cancelado.");
            if (inscricaoDados.InscricaoCancelada)
                throw new ComprovanteInscricaoCanceladaException(inscricaoDados.SituacaoAtual);
        }

        private void VerificarConsistenciaProfessorDaCasa(Inscricao inscricao)
        {
            // Busca os dados do processo da inscrição
            var spec = new SMCSeqSpecification<GrupoOferta>(inscricao.SeqGrupoOferta);
            var processo = this.GrupoOfertaDomainService.SearchProjectionByKey(spec, p => new
            {
                p.Processo.TipoProcesso.Consistencias,
                p.Processo.Telefones
            });

            // Verifica se o processo que a pessoa deseja se inscrever possui a consistência de professor da casa
            if (processo.Consistencias.Any(c => c.TipoConsistencia == TipoConsistencia.ProfessorDaCasa))
            {
                // Se possui a consistência...
                // Busca os dados do inscrito (cpf e passaporte)
                var specInscrito = new SMCSeqSpecification<Inscrito>(inscricao.SeqInscrito);
                var inscrito = this.InscritoDomainService.SearchProjectionByKey(specInscrito, i => new
                {
                    i.Cpf,
                    i.NumeroPassaporte
                });

                // Chama a procedure para ver se o inscrito é professor da casa
                var listaContrato = IntegracaoAcademicoService.BuscaContratoValidoParaGPI(inscrito.Cpf, inscrito.NumeroPassaporte, null, null);
                if (listaContrato.Count <= 0)
                {
                    string telefones = "";
                    if (processo.Telefones.Count > 0)
                    {
                        foreach (var tel in processo.Telefones)
                            telefones = telefones + (!string.IsNullOrEmpty(telefones) ? ", " : " ") + tel.TelefoneFormatado;
                    }
                    else
                        telefones = "Não informado!";
                    throw new NaoEhProfessorDaCasaException(telefones);
                }
            }
        }

        private void AtualizarDadosLGPD(long seqInscrito, bool consentimentoLGPD)
        {
            var dadosLGPD = InscritoDomainService.SearchProjectionByKey(seqInscrito, x => new { x.ConsentimentoLGPD, x.DataConsentimentoLGPD });
            if (dadosLGPD.ConsentimentoLGPD != consentimentoLGPD)
            {
                InscritoDomainService.UpdateFields(new Inscrito
                {
                    Seq = seqInscrito,
                    ConsentimentoLGPD = consentimentoLGPD,
                    DataConsentimentoLGPD = DateTime.Now
                }, x => x.ConsentimentoLGPD, x => x.DataConsentimentoLGPD);
            }
        }

        public List<long> ObterSeqsDocumentosRequeridos(long seqConfiguracaoEtapa)
        {
            var query = @"  use INSCRICAO
                            select dr.seq_documento_requerido
	                            from documento_requerido dr
	                            left join grupo_documento_requerido_item gdri
		                            on dr.seq_documento_requerido = gdri.seq_documento_requerido
	                            left join grupo_documento_requerido gdr
		                            on gdr.seq_grupo_documento_requerido = gdri.seq_grupo_documento_requerido

                            where
	                            dr.seq_configuracao_etapa = @seqConfiguracaoEtapa and
	                            (dr.ind_obrigatorio = 1 or  gdri.seq_grupo_documento_requerido is not null)
                            ";

            return this.RawQuery<long>(query, new SMCFuncParameter("@seqConfiguracaoEtapa", seqConfiguracaoEtapa)).ToList();
        }

        public bool PossuiDocumentoRequerido(long seqConfiguracaoEtapa)
        {
            var query = @"  use INSCRICAO
                            select count(dr.seq_documento_requerido)
	                            from documento_requerido dr
	                            left join grupo_documento_requerido_item gdri
		                            on dr.seq_documento_requerido = gdri.seq_documento_requerido
	                            left join grupo_documento_requerido gdr
		                            on gdr.seq_grupo_documento_requerido = gdri.seq_grupo_documento_requerido

                            where
	                            dr.seq_configuracao_etapa = @seqConfiguracaoEtapa and
	                            (dr.ind_obrigatorio = 1 or  gdri.seq_grupo_documento_requerido is not null)
                            ";

            return this.RawQuery<long>(query, new SMCFuncParameter("@seqConfiguracaoEtapa", seqConfiguracaoEtapa)).FirstOrDefault() > 0;
        }

        public bool PossuiDocumentoRequeridoPorSeqInscricao(long seqConfiguracaoEtapa)
        {
            var query = @"  use INSCRICAO
                            select count(dr.seq_documento_requerido)
	                            from documento_requerido dr
	                            left join grupo_documento_requerido_item gdri
		                            on dr.seq_documento_requerido = gdri.seq_documento_requerido
	                            left join grupo_documento_requerido gdr
		                            on gdr.seq_grupo_documento_requerido = gdri.seq_grupo_documento_requerido

                            where
	                            dr.seq_configuracao_etapa = @seqConfiguracaoEtapa and
	                            (dr.ind_obrigatorio = 1 or  gdri.seq_grupo_documento_requerido is not null)
                            ";

            return this.RawQuery<long>(query, new SMCFuncParameter("@seqConfiguracaoEtapa", seqConfiguracaoEtapa)).FirstOrDefault() > 0;
        }

        /// <summary>
        /// Incluir um registro de documento da inscrição para cada um dos documentos requeridos na configuração
        /// de processo associada à inscrição, informando a situação da entrega como “Aguardando entrega”;
        /// o arquivo anexado, sua descrição, a data da entrega, a forma de entrega, a versão do documento, a observação,
        /// a data de devolução e o prazo de entrega nulos.
        /// </summary>
        /// <param name="inscricao"></param>
        private void IncluirDocumentosInscricao(Inscricao inscricao)
        {
            // Busca os documentos requeridos para a inscrição
            var specDoc = new DocumentoRequeridoFilterSpecification() { SeqConfiguracaoEtapa = inscricao.SeqConfiguracaoEtapa };
            IEnumerable<DocumentoRequerido> listaDoc = DocumentoRequeridoDomainService.SearchBySpecification(specDoc);

            inscricao.Documentos = inscricao.Documentos ?? new List<InscricaoDocumento>();

            // Percorre a lista de documentos requeridos incluindo os que não existem na inscrição
            foreach (var doc in listaDoc)
            {
                if (!inscricao.Documentos.Any(d => d.SeqDocumentoRequerido == doc.Seq))
                {
                    var novo = new InscricaoDocumento()
                    {
                        SeqInscricao = inscricao.Seq,
                        SeqDocumentoRequerido = doc.Seq,
                        SituacaoEntregaDocumento = SituacaoEntregaDocumento.AguardandoEntrega,
                        EntregaPosterior = false,
                        ExibirObservacaoParaInscrito = false
                    };
                    inscricao.Documentos.Add(novo);
                }
            }
        }

        /// <summary>
        /// Método que Valida a consistência de bolsa social
        /// Quando o tipo do processo for o 18 (Agendamento de Entrevista), ao clicar em “Próximo”
        /// * verificar se o candidato possui uma inscrição na situação de INSCRICAO_CONFIRMADA ou SITUACAO_INSCRICAO_FINALIZADA para um
        /// * processo do tipo 16 (Processo Seletivo - Graduação/Tecnólogo - Bolsa Social) ou
        /// * processo do tipo 22 (Processo Seletivo - Graduação/Tecnólogo - Bolsa Social - Reabertura de Matrícula), no mesmo ano/semestre
        /// * do processo em questão, para uma oferta cujo pai(Unidade) possui a mesma descrição do grupo de ofertas em questão.
        /// </summary>
        /// <param name="config"></param>
        /// <param name="seq"></param>
        /// <param name="seqGrupoOferta"></param>
        private void ValidarBolsaSocial(ConfiguracaoEtapa config, long seqInscrito, long seqGrupoOferta)
        {
            //Alterado pela task 47629
            /* Quando o tipo do processo for o 18(Agendamento de Entrevista), ao clicar em “Próximo” verificar se o candidato possui uma inscrição na situação de 
             * INSCRICAO_CONFIRMADA para um processo do tipo 16(Processo Seletivo - Graduação / Tecnólogo - Bolsa Social) ou 
             * 22(Processo Seletivo - Graduação / Tecnólogo - Bolsa Social - Reabertura de Matrícula), no mesmo ano / semestre do processo e questão, 
             * para uma oferta cujo pai(Unidade) possui a mesma descrição do grupo de ofertas em questão. Quando o grupo selecionado for "CORAÇÃO EUCARÍSTICO / PUC MINAS VIRTUAL", 
             * verificar se o candidato possui inscrição em uma oferta cujo pai(Unidade) seja "Coração Eucarístico", ou "PUC Minas Virtual" no nome.Caso não possua, abortar a operação
             * e emitir a mensagem de erro: */
            if (config.EtapaProcesso.Processo.SeqTipoProcesso == 18)
            {
                var descGrupoOferta = GrupoOfertaDomainService.SearchProjectionByKey(seqGrupoOferta, x => x.Nome);

                var specBolsaSocial = new InscricaoBolsaSocialSpecification()
                {
                    SeqInscrito = seqInscrito,
                    SeqsTiposProcesso = new List<long> { 16, 22 },
                    TiposProcessosSituacoesToken = new List<string> { TOKENS.SITUACAO_INSCRICAO_FINALIZADA, TOKENS.SITUACAO_INSCRICAO_CONFIRMADA },
                    DescricaoGrupoOferta = descGrupoOferta,
                    AnoReferencia = config.EtapaProcesso.Processo.AnoReferencia,
                    SemestreReferencia = config.EtapaProcesso.Processo.SemestreReferencia
                };

                if (Count(specBolsaSocial) == 0)
                    throw new InscricaoBolsaSocialException(descGrupoOferta, config.EtapaProcesso.Processo.SemestreReferencia, config.EtapaProcesso.Processo.AnoReferencia);
            }
        }

        private void VerificarDebitoFinanceiro(Inscricao inscricao, long seqTipoProcesso, ISMCUnitOfWork unitOfWork)
        {
            string inscritoCpf = InscritoDomainService.SearchProjectionByKey(new SMCSeqSpecification<Inscrito>(inscricao.SeqInscrito),
                                                        x => x.Cpf);
            // Consulta o GRA.
            string mensagemValidacaoGRA = IntegracaoFinanceiroService.VerificarDebitoCpf(inscritoCpf, out bool error);
            // Verifica se existe algum problema para continuar a inscricao no GRA.
            if (!string.IsNullOrWhiteSpace(mensagemValidacaoGRA))
            {
                var processo = ProcessoDomainService.SearchProjectionByKey(new SMCSeqSpecification<Processo>(inscricao.SeqProcesso),
                                                    x => new
                                                    {
                                                        x.TipoProcesso.Situacoes.FirstOrDefault(f => f.Token == TOKENS.SITUACAO_INSCRICAO_CANCELADA).SeqSituacao,
                                                        x.Descricao,
                                                        x.Telefones,
                                                        Emails = x.EnderecosEletronicos.Where(e => e.TipoEnderecoEletronico == TipoEnderecoEletronico.Email).Select(e => e.Descricao).ToList()
                                                    });

                var seqTokenMotivo = SituacaoService.BuscarSeqMotivoSituacaoPorToken(processo.SeqSituacao, TOKENS.MOTIVO_INSCRICAO_CANCELADA_DEBITO_FINANCEIRO).Value;

                // Verifica se a inscrição já foi cancelada anteriormente pelo mesmo motivo, para evitar que fique criando registros.
                bool jaCancelouInscricao = InscricaoHistoricoSituacaoDomainService.Count(new InscricaoHistoricoSituacaoInscricaoJaExistenteSpecification()
                {
                    SeqInscrito = inscricao.SeqInscrito,
                    SeqMotivoSituacaoSGF = seqTokenMotivo,
                    SeqProcesso = inscricao.SeqProcesso
                }) > 0;

                // Se não cancelou a inscrição ainda, da commit.
                if (!jaCancelouInscricao && !error)
                {
                    CancelarInscricao(inscricao.Seq, inscricao.SeqProcesso, seqTokenMotivo);
                    unitOfWork.Commit();
                }

                // Interrompe a operação.
                if (!error)
                    throw new UsuarioComDebidoFinanceiroException(processo.Descricao, string.Join(", ", processo.Emails));
                else
                    // Se ocorrer um erro não tratado no serviço, não salva a operação e exibe o erro para o usuário.
                    throw new SMCApplicationException(mensagemValidacaoGRA);
            }
        }

        #endregion Inclusão

        #region Exclusão

        public void ExcluirInscricao(long seqInscricao)
        {
            using (var unityOfWork = SMCUnitOfWork.Begin())
            {
                try
                {
                    var specInscricao = new SMCSeqSpecification<Inscricao>(seqInscricao);

                    var dadosInscricao = this.SearchProjectionByKey(specInscricao, i => new
                    {
                        Inscricao = i,
                        SeqsNotificacoesEmailDestinatario = i.EnvioNotificacoes.Select(e => e.SeqNotificacaoEmailDestinatario).ToList(),
                        TitulosBoletosGerados = i.Boletos.SelectMany(b => b.Titulos.Where(t => !t.DataCancelamento.HasValue && !t.DataPagamento.HasValue).Select(t => t)).ToList(),
                        Documentos = i.Documentos.Select(s => s.ArquivoAnexado).ToList(),
                        GuidProcesso = i.UidProcessoGed,
                        i.ArquivoComprovante
                    });

                    //Verificar se existe algum boleto associado ao candidato que já está pago.
                    //Caso exista, abortar a operação e emitir mensagem de erro
                    if (PossuiBoletoPago(seqInscricao))
                    {
                        throw new InscricaoComBoletoPagoException();
                    }

                    //Excluir todas as notificações enviadas para o candidato
                    if (dadosInscricao.SeqsNotificacoesEmailDestinatario.Any())
                        NotificacaoService.ExcluirNotificacoesPorNotificacoesEmailDestnatario(dadosInscricao.SeqsNotificacoesEmailDestinatario.ToArray());

                    //Caso os títulos dos boletos do candidato já tenham sido gerados, cancelá-los no GRA,
                    //através da procedure st_muda_situacao_titulo, informando como parâmetro o número do título,
                    //a situação “8” e a descrição “Inscrição excluída”.
                    if (dadosInscricao.TitulosBoletosGerados.Any())
                    {
                        dadosInscricao.TitulosBoletosGerados.ForEach(t =>
                        {
                            IntegracaoFinanceiroService.CancelarTitulo(new Financeiro.ServiceContract.BLT.Data.CancelarTituloData()
                            {
                                SeqTitulo = t.SeqTitulo,
                                Descricao = "Inscrição excluída",
                                UsuarioOperacao = SMCContext.User.Identity.Name
                            });
                        });
                    }

                    //Atualizar GED ao apagar inscrição
                    AtualizarDocumentosProcessoApagandoGED(seqInscricao, dadosInscricao);

                    //Excluir a inscrição do candidato
                    this.DeleteEntity(dadosInscricao.Inscricao);

                    unityOfWork.Commit();
                }
                catch (Exception)
                {
                    unityOfWork.Rollback();
                    throw;
                }
            }
        }

        private void AtualizarDocumentosProcessoApagandoGED(long seqInscricao, dynamic dadosInscricao)
        {
            foreach (var documento in dadosInscricao.Documentos)
            {
                if (documento != null && (documento.UidArquivoGed != null && documento.UidArquivoGed != Guid.Empty))
                {
                    arquivosDeletadosGED.Add(documento.UidArquivoGed.ToString());
                }
            }

            if (dadosInscricao.ArquivoComprovante != null && dadosInscricao.ArquivoComprovante.UidArquivoGed != null && dadosInscricao.ArquivoComprovante.UidArquivoGed != Guid.Empty)
            {
                arquivosDeletadosGED.Add(dadosInscricao.ArquivoComprovante.UidArquivoGed.ToString());
            }

            ExcluirArquivoGED(seqInscricao, arquivosDeletadosGED);
            if (dadosInscricao.GuidProcesso != null && dadosInscricao.GuidProcesso != Guid.Empty)
            {
                ProcessoApiDoaminService.ApagarProcesso(seqInscricao);
            }
        }

        #endregion Exclusão

        #region Consulta de inscrições

        /// <summary>
        /// Busca a justificado pelo seq da inscricao
        /// </summary>
        /// <param name="seqInscricao"></param>
        /// <returns></returns>
        public JustificativaSituacaoInscricaoVO BuscarJustificativaSituacao(long seqInscricao)
        {
            var spec = new SituacaoInscricaoProcessoFilterSpecification() { SeqInscricao = seqInscricao };

            var justificativa =
              this.SearchProjectionByKey(seqInscricao, x => new JustificativaSituacaoInscricaoVO
              {
                  NomeInscrito = (x.Inscrito.NomeSocial != null) ? x.Inscrito.NomeSocial + " (" + x.Inscrito.Nome + ")" : x.Inscrito.Nome,
                  SeqMotivo = x.HistoricosSituacao.FirstOrDefault(h => h.Atual).SeqMotivoSituacaoSGF,
                  Justificativa = x.HistoricosSituacao.FirstOrDefault(h => h.Atual).Justificativa,

              });

            var motivo = SituacaoService.BuscarDescricaoMotivos(new long[] { justificativa.SeqMotivo.Value }).FirstOrDefault();

            if (motivo != null && !string.IsNullOrEmpty(motivo.Descricao))
            {
                justificativa.Motivo = motivo.Descricao;
            }

            return justificativa;
        }

        public bool BuscarInscricoesQuePossuiTaxaAssociadaAoBoleto(InscricaoFilterSpecification filtro)
        {
            IncludesInscricao includes = IncludesInscricao.Boletos |
                                         IncludesInscricao.Boletos_Taxas;

            return this.SearchBySpecification(filtro, includes).Any();
        }


        /// <summary>
        /// Busca as inscrições em processos para um determinado inscrito
        /// As inscrições encontram-se agrupadas por processo
        /// </summary>
        /// <param name="filtro">Filtro para pesquisa</param>
        /// <param name="total">Finalizadas de registros encontrados</param>
        /// <returns>Lista de inscrições</returns>
        public List<InscricoesProcessoVO> BuscarInscricoes(InscricaoFilterSpecification filtro, out int total)
        {
            // Filtra as inscrições
            var numeroPagina = filtro.PageNumber;
            var tamanhoPagina = filtro.MaxResults;
            filtro.PageNumber = 1;
            filtro.MaxResults = Int32.MaxValue;
            IncludesInscricao includes = IncludesInscricao.Ofertas |
                                         IncludesInscricao.Ofertas_Oferta |
                                         IncludesInscricao.GrupoOferta |
                                         IncludesInscricao.ConfiguracaoEtapa |
                                         IncludesInscricao.ConfiguracaoEtapa_EtapaProcesso |
                                         IncludesInscricao.HistoricosPagina_ConfiguracaoEtapaPagina |
                                         IncludesInscricao.Processo |
                                         IncludesInscricao.Processo_PermissoesInscricaoForaPrazo_Inscritos |
                                         IncludesInscricao.Processo_TipoProcesso |
                                         IncludesInscricao.Processo_GruposOferta |
                                         IncludesInscricao.Processo_GruposOferta_Ofertas |
                                         IncludesInscricao.Processo_ConfiguracoesModeloDocumento |
                                         IncludesInscricao.Processo_ConfiguracoesModeloDocumento_TipoDocumento;

            List<Inscricao> inscricoes = this.SearchBySpecification(filtro, includes).ToList();

            // Agrupa as inscrições por processo incluindo ordenação
            List<InscricoesProcessoVO> processosInscricao = inscricoes.Select(c => new InscricoesProcessoVO
            {
                SeqProcesso = c.SeqProcesso,
                Inscricoes = new List<InscricaoVO>(),
                DescricaoProcesso = c.Processo.Descricao,
                UidProcesso = c.Processo.UidProcesso,
                QuantidadeOfertas = c.Processo.GruposOferta.Count,
                TokenResource = c.Processo.TipoProcesso.TokenResource,

            })
            .Distinct(new InscricoesProcessoVOComparer())
            .ToList();

            total = processosInscricao.Count;
            processosInscricao = processosInscricao.Skip((numeroPagina - 1) * tamanhoPagina).Take(tamanhoPagina).ToList();

            // Cada processo encontrado, associa os as inscrições
            foreach (var processo in processosInscricao)
            {
                foreach (var inscr in inscricoes.Where(x => x.SeqProcesso == processo.SeqProcesso))
                {
                    bool existeDocumentoObrigatorioIndeferidoOuPendente = false;
                    bool existeGrupoDocumentoIndeferidoOuPendente = false;

                    if (inscr.ConfiguracaoEtapa.PermiteNovaEntregaDocumentacao)
                    {
                        var documentosEntregues = this.InscricaoDocumentoDomainService.BuscarSumarioDocumentosEntregue(new InscricaoDocumentoFilterSpecification() { SeqInscricao = inscr.Seq });

                        existeDocumentoObrigatorioIndeferidoOuPendente = documentosEntregues.DocumentosObrigatorios.Any(d => d.InscricaoDocumentos.Any(i => (i.SituacaoEntregaDocumento == SituacaoEntregaDocumento.Indeferido || i.SituacaoEntregaDocumento == SituacaoEntregaDocumento.Pendente) && i.PermiteUploadArquivo));

                        existeGrupoDocumentoIndeferidoOuPendente = documentosEntregues.GruposDocumentos.Any(g => g.DocumentosRequeridosGrupo.Any(d => d.InscricaoDocumentos.Any(i => (i.SituacaoEntregaDocumento == SituacaoEntregaDocumento.Indeferido || i.SituacaoEntregaDocumento == SituacaoEntregaDocumento.Pendente) && i.PermiteUploadArquivo)));
                    }
                    
                    var inscricaoVO = new InscricaoVO
                    {
                        SeqInscrito = inscr.SeqInscrito,
                        SeqInscricao = inscr.Seq,
                        SeqProcesso = inscr.Processo.Seq,
                        SeqConfiguracaoEtapa = inscr.SeqConfiguracaoEtapa,
                        SeqGrupoOferta = inscr.SeqGrupoOferta,
                        DataInscricao = inscr.DataInscricao,
                        IdiomaInscricao = inscr.Idioma,
                        DocumentacaoEntregue = inscr.DocumentacaoEntregue,
                        // Somente exibe o grupo de ofertas caso o processo possua mais de um.
                        DescricaoGrupoOferta = (processo.QuantidadeOfertas > 1) ? inscr.GrupoOferta.Nome : "",
                        ConfiguracaoEtapaVigente = inscr.ConfiguracaoEtapa.Vigente,
                        ProcessoCancelado = inscr.Processo.Cancelado,
                        UidProcesso = inscr.Processo.UidProcesso,
                        PermissaoInscricaoForaPrazo = inscr.Processo.PermissoesInscricaoForaPrazo.Any(f => f.Inscritos.Any(o => o.SeqInscrito == filtro.SeqInscrito)
                                                                                                        && f.DataFim >= DateTime.Now && f.DataInicio <= DateTime.Now),
                        SituacaoEtapa = inscr.ConfiguracaoEtapa.EtapaProcesso.SituacaoEtapa,
                        PaginaAtualInscricao = inscr.HistoricosPagina.OrderByDescending(f => f.DataAcesso).FirstOrDefault()?.ConfiguracaoEtapaPagina.Token,
                        PermiteNovaEntregaDocumentacao = inscr.ConfiguracaoEtapa.PermiteNovaEntregaDocumentacao,
                        ExisteDocumentoObrigatorioIndeferidoOuPendente = existeDocumentoObrigatorioIndeferidoOuPendente,
                        ExisteGrupoDocumentoIndeferidoOuPendente = existeGrupoDocumentoIndeferidoOuPendente,
                        DataPrazoNovaEntregaDocumentacao = inscr.DataPrazoNovaEntregaDocumentacao,
                        DataEncerramentoProcesso = inscr.Processo.DataEncerramento,
                        TokenResource = inscr.Processo.TipoProcesso.TokenResource,
                        SeqTipoDocumento = BuscarSeqTipoDocumento(inscr),
                        ProcessoEncerrado = inscr.Processo.DataEncerramento.HasValue && inscr.Processo.DataEncerramento <= DateTime.Now,
                        SituacaoDocumentacao = inscr.SituacaoDocumentacao,
                        GestaoEventos = inscr.Processo.TipoProcesso.GestaoEventos
                    };

                    // Adição da descrição da oferta. Limita a exibição das ofertas nas 5 primeiras. As ofertas adicionais serão concatenadas em um único texto.
                    var ofertaHabilitaCheckin = CriarListaOfertas(inscr);
                    inscricaoVO.DescricaoOfertas = ofertaHabilitaCheckin.Item1;

                    // Buscar a situação atual da inscrição para preencher os campos
                    InscricaoHistoricoSituacaoFilterSpecification specHistorico = new InscricaoHistoricoSituacaoFilterSpecification
                    {
                        SeqInscricao = inscr.Seq,
                        Atual = true
                    };
                    var situacaoAtual = InscricaoHistoricoSituacaoDomainService.SearchByKey(specHistorico, IncludesInscricaoHistoricoSituacao.TipoProcessoSituacao);
                    if (situacaoAtual == null)
                        throw new InscricaoInvalidaException();
                    inscricaoVO.DescricaoSituacaoAtual = situacaoAtual.TipoProcessoSituacao.Descricao;
                    inscricaoVO.TokenSituacaoAtual = situacaoAtual.TipoProcessoSituacao.Token;

                    // Busca o label da oferta
                    ProcessoIdiomaFilterSpecification specIdioma = new ProcessoIdiomaFilterSpecification(inscr.SeqProcesso, inscr.Idioma);
                    inscricaoVO.DescricaoLabelOferta = ProcessoIdiomaDomainService.SearchProjectionByKey(specIdioma, p => p.LabelOferta);
                    inscricaoVO.HabilitaCheckin = inscr.Ofertas.Any(a => a.Oferta.HabilitaCheckin);

                    // Verifica se o grupo oferta possui ofertas ativas, não canceladas e vigentes
                    OfertaFilterSpecification specOferta = new OfertaFilterSpecification()
                    {
                        SeqGrupoOferta = inscricaoVO.SeqGrupoOferta,
                        Ativo = true
                    };
                    specOferta.SetOrderBy(x => x.Nome);
                    SMCAndSpecification<Oferta> spec = new SMCAndSpecification<Oferta>(specOferta, new SMCAndSpecification<Oferta>(!(new OfertaCanceladaSpecification()), new OfertaVigenteSpecification()));
                    inscricaoVO.GrupoPossuiOfertaVigente = (OfertaDomainService.Count(spec) > 0);



                    processo.Inscricoes.Add(inscricaoVO);

                    processo.SituacaoProcesso = BuscarSituacaoAtualInscricao(inscr.Seq);
                }
            }

            EspecificarBotaoContinuar(processosInscricao);

            return processosInscricao;
        }

        private long BuscarSeqTipoDocumento(Inscricao inscr)
        {
            var configuracoesModeloDocumento = inscr.Processo.ConfiguracoesModeloDocumento;
            if (configuracoesModeloDocumento == null)
                return 0L;

            return inscr.Processo.ConfiguracoesModeloDocumento.Select(c => c.TipoDocumento.Seq).FirstOrDefault();
        }

        /// <summary>
        /// Cria uma lista de descrição das ofertas selecionadas de uma inscrição. Exibe as 5 primeiras ofertas selecionadas e concatena as restantes em um único texto.
        /// </summary>
        public (List<string>, bool) CriarListaOfertas(Inscricao inscricao)
        {
            var ofertas = new List<string>();
            var maxOfertas = (inscricao.Ofertas.Count < 5) ? inscricao.Ofertas.Count : 5;
            bool habilitaCheckin = false;
            for (int i = 0; i < maxOfertas; i++)
            {
                var oferta = OfertaDomainService.BuscarHierarquiaOfertaCompleta(inscricao.Ofertas[i].SeqOferta, false);
                habilitaCheckin = oferta.HabilitaCheckin;
                ofertas.Add(oferta.DescricaoCompleta);
            }
            if (inscricao.Ofertas.Count > 5)
            {
                var qtOfertas = inscricao.Ofertas.Count - 5;
                if (qtOfertas == 1)
                {
                    ofertas.Add($"+{qtOfertas} {Resources.MessagesResource.Descricao_Oferta}");
                }
                else
                {
                    ofertas.Add($"+{qtOfertas} {Resources.MessagesResource.Descricao_Ofertas}");
                }
            }
            return (ofertas, habilitaCheckin);
        }

        /// <summary>
        /// Verifica se existe um bolet opago para uma inscrição
        /// </summary>
        public bool PossuiBoletoPago(long seqInscricao)
        {
            var specInscricao = new SMCSeqSpecification<Inscricao>(seqInscricao);

            var possuiBoletoPago = this.SearchProjectionByKey(specInscricao, i => i.Boletos.Any(b => b.Titulos.Any(t => t.DataCancelamento == null && t.DataPagamento.HasValue)));

            return possuiBoletoPago;
        }

        /// <summary>
        /// Verifica se existe um boleto para a inscrição
        /// </summary>
        public bool PossuiBoleto(long seqInscricao)
        {
            var possuiBoletoPago = this.SearchProjectionByKey(seqInscricao, i => i.Boletos.Any());

            return possuiBoletoPago;
        }

        public bool PossuiOfertaVigente(long seqInscricao)
        {
            var specInscricao = new SMCSeqSpecification<Inscricao>(seqInscricao);

            return this.SearchProjectionByKey(specInscricao, i => i.Ofertas.Any(o => o.Oferta.Ativo && !o.Oferta.DataCancelamento.HasValue && (
                (o.Oferta.DataInicio.HasValue && DateTime.Now >= o.Oferta.DataInicio.Value && o.Oferta.DataFim.HasValue && DateTime.Now <= o.Oferta.DataFim.Value) ||
                (o.Oferta.DataInicio.HasValue && DateTime.Now >= o.Oferta.DataInicio.Value && !o.Oferta.DataFim.HasValue))));
        }

        /// <summary>
        /// Busca a situação das inscrições para um processo com a situação atual na etapa de inscrição
        /// Caso a inscrição não tenha situação no histórico dentro da etapa inscrição, ela não será listada
        /// </summary>
        public List<SituacaoInscricaoProcessoVO> BuscarSituacaoInscricaoProcesso(SituacaoInscricaoProcessoFilterSpecification filtro, out int total)
        {
            if (filtro.SeqGrupoOferta.HasValue && filtro.SeqGrupoOferta.Value == default(long)) filtro.SeqGrupoOferta = null;
            if (filtro.SeqOferta.HasValue && filtro.SeqOferta.Value == default(long)) filtro.SeqOferta = null;
            //Este ajuste foi feito, pois quando temos um valor do lookup de formulário concatenado por ';' ele ajusta o campo com o valor
            //correto que tem no banco para fazer uma pesquisa com o valor do campo correto.
            if (filtro.Dados.SMCAny())
            {
                var novosDados = new List<KeyValuePair<long, string>>();
                foreach (var item in filtro.Dados)
                {
                    var spec = new InscricaoDadoFormularioCampoFilterSpecification() { SeqElemento = item.Key, Valor = item.Value };
                    var campoFormulario = InscricaoDadoFormularioCampoDomainService.SearchBySpecification(spec).First();
                    novosDados.Add(new KeyValuePair<long, string>(item.Key, campoFormulario.Valor));
                }
                filtro.Dados = novosDados;
            }
            var numeroPagina = filtro.PageNumber;
            var tamanhoPagina = filtro.MaxResults;
            filtro.PageNumber = 1;
            filtro.MaxResults = Int32.MaxValue;

            #region resolução de ordenação

            //Nome do inscrito
            var clausula = filtro.OrderByClauses.FirstOrDefault(x => x.FieldName == "NomeInscrito");
            if (clausula != null) clausula.FieldName = "Inscrito.Nome";
            //Grupo de oferta
            clausula = filtro.OrderByClauses.FirstOrDefault(x => x.FieldName == "DescricaoGrupoOferta");
            if (clausula != null) clausula.FieldName = "GrupoOferta.Nome";

            clausula = filtro.OrderByClauses.FirstOrDefault(x => x.FieldName == "Nota");
            if (clausula != null) clausula.FieldName = "NotaGeral";

            clausula = filtro.OrderByClauses.FirstOrDefault(x => x.FieldName == "Classificacao");
            if (clausula != null) clausula.FieldName = "NumeroClassificacao";

            //FIX: Pensar em como resolver situação e oferta

            #endregion resolução de ordenação

            List<SituacaoInscricaoProcessoVO> inscricoesProcesso =
                this.SearchProjectionBySpecification(filtro, x => new SituacaoInscricaoProcessoVO
                {
                    Seq = x.Seq,
                    SeqProcesso = x.SeqProcesso,
                    SeqInscrito = x.SeqInscrito,
                    NomeInscrito = (x.Inscrito.NomeSocial != null) ? x.Inscrito.NomeSocial + " (" + x.Inscrito.Nome + ")" : x.Inscrito.Nome,
                    DescricaoGrupoOferta = x.GrupoOferta.Nome,
                    SeqGrupoOferta = x.SeqGrupoOferta,
                    DescricaoSituacaoAtual = x.HistoricosSituacao.FirstOrDefault(h => h.Atual).TipoProcessoSituacao.Descricao,
                    SeqMotivo = x.HistoricosSituacao.FirstOrDefault(h => h.Atual).SeqMotivoSituacaoSGF,
                    JustificativaSituacaoAtual = x.HistoricosSituacao.FirstOrDefault(h => h.Atual).Justificativa,
                    SeqSituacao = x.HistoricosSituacao.FirstOrDefault(h => h.Atual).SeqTipoProcessoSituacao,
                    DataInscricao = x.DataInscricao,

                    OpcoesOferta = x.Ofertas.Select(y => new OpcaoOfertaVO()
                    {
                        NumeroOpcao = y.NumeroOpcao,
                        Descricao = y.Oferta.Nome,
                        DescricaoOferta = y.Oferta.DescricaoCompleta, //y.Oferta.Nome,
                        HierarquiaCompleta = y.Oferta.HierarquiaCompleta,
                        DataInicioAtividade = y.Oferta.DataInicioAtividade,
                        DataFimAtividade = y.Oferta.DataFimAtividade,
                        CargaHorariaAtividade = y.Oferta.CargaHorariaAtividade,
                        ExibirPeriodoAtividadeOferta = y.Oferta.Processo.ExibirPeriodoAtividadeOferta
                    }).OrderBy(o => o.NumeroOpcao).ToList(),

                    DocumentacaoEntregue =
                    x.ConfiguracaoEtapa.DocumentosRequeridos.Any(d => d.Obrigatorio)
                     || x.ConfiguracaoEtapa.GruposDocumentoRequerido.Any(g => g.MinimoObrigatorio > 0) ?
                        x.DocumentacaoEntregue : new Nullable<bool>(),
                    TaxaInscricaoPaga = x.Ofertas.Any(o => o.NumeroOpcao == 1 && o.Oferta.ExigePagamentoTaxa) ? x.TituloPago : new Nullable<bool>(),
                    SituacaoDocumentacao = x.SituacaoDocumentacao
                    
                }
                , out total).ToList();

            // Filtra por um galho da hierarquia. A specification é falha e traz os dados por "Contains". Isso pode trazer resultados falsos.
            // Ex: Pesquisar por 1 e trazer 12, 16 etc. A specification irá reduzir a quantidade de dados buscados para o algoritmo abaixo filtrar os corretos.
            if (filtro.SeqItemHierarquiaOferta.HasValue)
            {
                var tmpList = new List<SituacaoInscricaoProcessoVO>();
                foreach (var item in inscricoesProcesso)
                {
                    if (item.OpcoesOferta.Any(f => new Oferta() { HierarquiaCompleta = f.HierarquiaCompleta }.VerificarHierarquia(filtro.SeqItemHierarquiaOferta.Value)))
                    {
                        tmpList.Add(item);
                    }

                }
                inscricoesProcesso = tmpList;
            }

            var motivos = SituacaoService.BuscarDescricaoMotivos(inscricoesProcesso.Where(f => f.SeqMotivo.HasValue).Select(f => f.SeqMotivo.Value).ToArray());

            foreach (var inscricao in inscricoesProcesso)
            {
                var descricaoMotivo = motivos.Where(f => f.Seq == inscricao.SeqMotivo).FirstOrDefault();
                if (descricaoMotivo != null)
                {
                    inscricao.MotivoSituacaoAtual = descricaoMotivo.Descricao;
                }

                foreach (var oferta in inscricao.OpcoesOferta)
                {
                    var of = new Oferta()
                    {
                        Nome = oferta.Descricao,
                        DescricaoCompleta = oferta.DescricaoOferta,
                        DataInicioAtividade = oferta.DataInicioAtividade,
                        DataFimAtividade = oferta.DataFimAtividade,
                        CargaHorariaAtividade = oferta.CargaHorariaAtividade,
                        Processo = new Processo()
                        {
                            ExibirPeriodoAtividadeOferta = oferta.ExibirPeriodoAtividadeOferta
                        }
                    };

                    OfertaDomainService.AdicionarDescricaoCompleta(of, of.Processo.ExibirPeriodoAtividadeOferta, false);

                    if (!string.IsNullOrEmpty(of.DescricaoCompleta))
                        oferta.DescricaoOferta = of.DescricaoCompleta;
                }
            }

            total = inscricoesProcesso.Count;
            return inscricoesProcesso.Skip((numeroPagina - 1) * tamanhoPagina).Take(tamanhoPagina).ToList();
            //return inscricoesProcesso;
        }

        /// <summary>
        /// Busca a situação das inscrições para um processo com a situação atual na etapa de inscrição
        /// Caso a isncrição não tenha situação no histórico dentro da etapa inscrição, ela não será listada
        /// </summary>
        public List<SituacaoInscricaoInscritoProcessoVO> BuscarSituacaoInscricaoInscritoProcesso(SituacaoInscricaoProcessoFilterSpecification filtro, out int total)
        {
            if (filtro.SeqGrupoOferta.HasValue && filtro.SeqGrupoOferta.Value == default(long)) filtro.SeqGrupoOferta = null;
            if (filtro.SeqOferta.HasValue && filtro.SeqOferta.Value == default(long)) filtro.SeqOferta = null;
            //Este ajuste foi feito, pois quando temos um valor do lookup de formulário concatenado por ';' ele ajusta o campo com o valor
            //correto que tem no banco para fazer uma pesquisa com o valor do campo correto.
            if (filtro.Dados.SMCAny())
            {
                var novosDados = new List<KeyValuePair<long, string>>();
                foreach (var item in filtro.Dados)
                {
                    var spec = new InscricaoDadoFormularioCampoFilterSpecification() { SeqElemento = item.Key, Valor = item.Value };
                    var campoFormulario = InscricaoDadoFormularioCampoDomainService.SearchBySpecification(spec).First();
                    novosDados.Add(new KeyValuePair<long, string>(item.Key, campoFormulario.Valor));
                }
                filtro.Dados = novosDados;
            }

            #region resolução de ordenação

            //Nome do inscrito
            var clausula = filtro.OrderByClauses.Where(x => x.FieldName == "NomeInscrito").FirstOrDefault();
            if (clausula != null) clausula.FieldName = "Inscrito.Nome";
            //Grupo de oferta
            clausula = filtro.OrderByClauses.Where(x => x.FieldName == "DescricaoGrupoOferta").FirstOrDefault();
            if (clausula != null) clausula.FieldName = "GrupoOferta.Nome";

            clausula = filtro.OrderByClauses.Where(x => x.FieldName == "Nota").FirstOrDefault();
            if (clausula != null) clausula.FieldName = "NotaGeral";

            clausula = filtro.OrderByClauses.Where(x => x.FieldName == "Classificacao").FirstOrDefault();
            if (clausula != null) clausula.FieldName = "NumeroClassificacao";

            //FIX: Pensar em como resolver situação e oferta

            #endregion resolução de ordenação

            List<SituacaoInscricaoInscritoProcessoVO> inscricoesProcesso =
                this.SearchProjectionBySpecification(filtro, x => new SituacaoInscricaoInscritoProcessoVO
                {
                    Seq = x.Seq,
                    SeqProcesso = x.SeqProcesso,
                    SeqInscrito = x.SeqInscrito,
                    NomeInscrito = (x.Inscrito.NomeSocial != null) ? x.Inscrito.NomeSocial + " (" + x.Inscrito.Nome + ")" : x.Inscrito.Nome,
                    DescricaoGrupoOferta = x.GrupoOferta.Nome,
                    DescricaoSituacaoAtual = x.HistoricosSituacao.FirstOrDefault(h => h.Atual).TipoProcessoSituacao.Descricao,
                    SeqMotivo = x.HistoricosSituacao.FirstOrDefault(h => h.Atual).SeqMotivoSituacaoSGF,
                    JustificativaSituacaoAtual = x.HistoricosSituacao.FirstOrDefault(h => h.Atual).Justificativa,
                    SeqSituacao = x.HistoricosSituacao.FirstOrDefault(h => h.Atual).SeqTipoProcessoSituacao,
                    DataInscricao = x.DataInscricao,
                    DocumentacaoEntregue = x.DocumentacaoEntregue,
                    TaxaInscricaoPaga = x.TituloPago,
                    ValorTitulo = x.Boletos.SelectMany(sm => sm.Titulos.Where(w => w.DataCancelamento == null).Select(s => s.ValorTitulo)).FirstOrDefault(),
                    EmailInscrito = x.Inscrito.Email,
                    Telefones = x.Inscrito.Telefones.ToList(),
                    Cpf = x.Inscrito.Cpf,
                    DataNascimento = x.Inscrito.DataNascimento,
                    SituacaoDocumentacao = x.SituacaoDocumentacao
                }
                , out total).ToList();

            var motivos = SituacaoService.BuscarDescricaoMotivos(inscricoesProcesso.Where(f => f.SeqMotivo.HasValue).Select(f => f.SeqMotivo.Value).ToArray());

            var documentos = RawQuery<DocumentosPendentesIndeferidosVO>(string.Format(_query_aqruivos_pendentes_indeferidos, filtro.SeqProcesso)).ToList();

            foreach (var inscr in inscricoesProcesso)
            {
                var spec = new InscricaoOfertaFilterSpecification { SeqInscricao = inscr.Seq };
                spec.SetOrderBy(x => x.NumeroOpcao);
                var ofertas = InscricaoOfertaDomainService
                    .SearchProjectionBySpecification(spec, x => new OpcaoOfertaVO
                    {
                        DescricaoOferta = x.Oferta.DescricaoCompleta,
                        NumeroOpcao = x.NumeroOpcao,
                        ExibirPeriodoAtividadeOferta = x.Oferta.Processo.ExibirPeriodoAtividadeOferta,
                        DataInicioAtividade = x.Oferta.DataInicioAtividade,
                        DataFimAtividade = x.Oferta.DataFimAtividade,
                        CargaHorariaAtividade = x.Oferta.CargaHorariaAtividade,
                        Descricao = x.Oferta.Nome
                    }
                );

                foreach (var oferta in ofertas)
                {
                    var of = new Oferta()
                    {
                        Nome = oferta.Descricao,
                        DescricaoCompleta = oferta.DescricaoOferta,
                        DataInicioAtividade = oferta.DataInicioAtividade,
                        DataFimAtividade = oferta.DataFimAtividade,
                        CargaHorariaAtividade = oferta.CargaHorariaAtividade,
                        Processo = new Processo()
                        {
                            ExibirPeriodoAtividadeOferta = oferta.ExibirPeriodoAtividadeOferta
                        }
                    };

                    OfertaDomainService.AdicionarDescricaoCompleta(of, of.Processo.ExibirPeriodoAtividadeOferta);

                    if (!string.IsNullOrEmpty(of.DescricaoCompleta))
                        oferta.DescricaoOferta = of.DescricaoCompleta;
                }

                inscr.OpcoesOferta = ofertas.ToList();

                var descricaoMotivo = motivos.Where(f => f.Seq == inscr.SeqMotivo).FirstOrDefault();
                if (descricaoMotivo != null)
                {
                    inscr.MotivoSituacaoAtual = descricaoMotivo.Descricao;
                }

                inscr.DocumentosPendentes = documentos.FirstOrDefault(f => f.SeqInscricao == inscr.Seq).DocumentosPendentes ?? string.Empty;
                inscr.DocumentosIndeferidos = documentos.FirstOrDefault(f => f.SeqInscricao == inscr.Seq).DocumentosIndeferidos ?? string.Empty;
            }
            return inscricoesProcesso;
        }

        /// <summary>
        /// Busca os dados da inscrição resumidos par aa exibição
        /// </summary>
        /// <param name="seqInscricao"></param>
        public DadosInscricaoVO BuscarInscricaoResumida(long seqInscricao, bool exibirDescricaoOfertaPorNome = true)
        {
            var inscricao = this.SearchProjectionByKey(new SMCSeqSpecification<Inscricao>(seqInscricao),
                x => new DadosInscricaoVO()
                {
                    SeqInscricao = x.Seq,
                    SeqInscrito = x.SeqInscrito,
                    DescricaoEtapaAtual = x.ConfiguracaoEtapa.Nome,
                    DescricaoGrupoOferta = x.GrupoOferta.Nome,
                    SeqProcesso = x.SeqProcesso,
                    SeqArquivoComprovante = x.SeqArquivoComprovante,
                    Finalizada = x.HistoricosSituacao.Any(i => i.TipoProcessoSituacao.Token == TOKENS.SITUACAO_INSCRICAO_FINALIZADA),
                    NomeInscrito = (x.Inscrito.NomeSocial != null) ? x.Inscrito.NomeSocial + " (" + x.Inscrito.Nome + ")" : x.Inscrito.Nome,
                    NumeroOpcoesDesejadas = x.NumeroOpcoesDesejadas,
                    Observacao = x.Observacao,
                    SituacaoInscrito = x.HistoricosSituacao.FirstOrDefault(h => h.Atual).TipoProcessoSituacao.Descricao,
                    TokenSituacaoInscrito = x.HistoricosSituacao.FirstOrDefault(h => h.Atual).TipoProcessoSituacao.Token,
                    CandidatoComBoletoPago = x.Boletos.Any(b => b.Titulos.Any(t => t.DataPagamento != null)),
                    OfertaVigente = x.Ofertas.Any(o => o.Oferta.DataInicio.HasValue && DateTime.Now >= o.Oferta.DataInicio.Value && o.Oferta.DataFim.HasValue && DateTime.Now <= o.Oferta.DataFim.Value),
                    DescricaoOfertas = x.Ofertas.Select(f => new OpcaoOfertaVO
                    {
                        DescricaoOferta = f.Oferta.Nome,
                        DescricaoOfertaCompleta = f.Oferta.DescricaoCompleta,
                        NumeroOpcao = f.NumeroOpcao,
                        Justificativa = f.JustificativaInscricao,
                        DataInicioAtividade = f.Oferta.DataInicioAtividade,
                        DataFimAtividade = f.Oferta.DataFimAtividade,
                        CargaHorariaAtividade = f.Oferta.CargaHorariaAtividade,
                        ExibirPeriodoAtividadeOferta = f.Oferta.Processo.ExibirPeriodoAtividadeOferta,

                        DescricaoOfertaOriginal = f.OfertaOriginal.Nome,
                        DescricaoOfertaCompletaOriginal = f.OfertaOriginal.DescricaoCompleta,
                        DataInicioAtividadeOfertaOriginal = f.OfertaOriginal.DataInicioAtividade,
                        DataFimAtividadeOfertaOriginal = f.OfertaOriginal.DataFimAtividade,
                        CargaHorariaAtividadeOfertaOriginal = f.OfertaOriginal.CargaHorariaAtividade,
                        ExibirPeriodoAtividadeOfertaOfertaOriginal = f.OfertaOriginal.Processo.ExibirPeriodoAtividadeOferta

                    }).ToList(),

                    SituacaoDocumentacao = x.SituacaoDocumentacao,
                    DescricaoTermoEntregaDocumentacao = x.ConfiguracaoEtapa.DescricaoTermoEntregaDocumentacao,
                    DataPrazoNovaEntregaDocumentacao = x.DataPrazoNovaEntregaDocumentacao,
                    RecebeuBolsa = x.RecebeuBolsa,
                    OrientacaoAceiteConversaoArquivosPDF = x.Processo.TipoProcesso.OrientacaoAceiteConversaoPDF,
                    TermoAceiteConversaoArquivosPDF = x.Processo.TipoProcesso.TermoAceiteConversaoPDF,
                    GestaoEventos = x.Processo.TipoProcesso.GestaoEventos,
                    UidInscricaoOferta = x.Processo.GruposOferta.FirstOrDefault().Ofertas.FirstOrDefault().InscricoesOferta.FirstOrDefault().UidInscricaoOferta,
                    UidProcesso = x.Processo.UidProcesso
                });

            foreach (var oferta in inscricao.DescricaoOfertas)
            {
                Oferta of = new Oferta()
                {
                    Nome = oferta.DescricaoOferta,
                    DescricaoCompleta = oferta.DescricaoOfertaCompleta,
                    DataInicioAtividade = oferta.DataInicioAtividade,
                    DataFimAtividade = oferta.DataFimAtividade,
                    CargaHorariaAtividade = oferta.CargaHorariaAtividade,
                    Processo = new Processo()
                    {
                        ExibirPeriodoAtividadeOferta = oferta.ExibirPeriodoAtividadeOferta
                    }
                };

                OfertaDomainService.AdicionarDescricaoCompleta(of, of.Processo.ExibirPeriodoAtividadeOferta, exibirDescricaoOfertaPorNome);

                if (!string.IsNullOrEmpty(of.DescricaoCompleta))
                    oferta.DescricaoOferta = of.DescricaoCompleta;

                //Oferta Original
                if (!string.IsNullOrEmpty(oferta.DescricaoOfertaOriginal))
                {
                    of = new Oferta()
                    {
                        Nome = oferta.DescricaoOfertaOriginal,
                        DescricaoCompleta = oferta.DescricaoOfertaCompletaOriginal,
                        DataInicioAtividade = oferta.DataInicioAtividadeOfertaOriginal,
                        DataFimAtividade = oferta.DataFimAtividadeOfertaOriginal,
                        CargaHorariaAtividade = oferta.CargaHorariaAtividadeOfertaOriginal,
                        Processo = new Processo()
                        {
                            ExibirPeriodoAtividadeOferta = oferta.ExibirPeriodoAtividadeOfertaOfertaOriginal
                        }
                    };

                    OfertaDomainService.AdicionarDescricaoCompleta(of, of.Processo.ExibirPeriodoAtividadeOferta, exibirDescricaoOfertaPorNome);

                    oferta.DescricaoOfertaOriginal = of.DescricaoCompleta;
                }

            }

            return inscricao;
        }

        public string BuscarUrlCss(long seqInscricao)
        {
            var urlCss = this.SearchProjectionByKey(seqInscricao, p => new
            {
                p.Processo.UnidadeResponsavelTipoProcessoIdVisual.CssAplicacao
            });
            return urlCss.CssAplicacao;
        }

        #endregion Consulta de inscrições

        #region Finalizar Inscrição

        /// <summary>
        /// Finaliza uma inscrição
        /// VALIDAÇÕES PARA FINALIZAR UMA INSCRIÇÃO:
        /// 1) Regras para avançar na inscrição
        /// 2) Verificar se o período de inscrição de cada oferta selecionada terminou ou se ela foi desativada
        /// ou cancelada
        ///
        /// AÇÕES REALIZADAS NA FINALIZAÇÃO DE UMA INSCRIÇÃO:
        /// 1) Gera os documentos da inscrição que são requeridos e não foram entregues via upload
        /// 2) Altera a situação da inscrição para TOKENS.SITUACAO_INSCRICAO_FINALIZADA
        /// 3) Salva o arquivo com o comprovante
        /// 4) Cria o primeiro título do boleto da inscrição
        /// </summary>
        /// <param name="seqInscricao">Sequencial da inscrição a ser finalizada</param>
        /// <param name="arquivoComprovante">Conteudo do arquivo de comprovante</param>
        /// <param name="aceiteConversaoPDF">Aceite do termo conversão do PDF</param>
        public void FinalizarInscricao(long seqInscricao, bool aceiteConversaoPDF, bool consentimentoLGPD, byte?[] arquivoComprovante)
        {
            // Verifica se a inscriçao já está finalizada ou confirmada
            // Esta regra está sendo chamada dentro do método VerificarRegrasAvancarInscricao.. não precisa ser chamada aqui também.
            //this.VerificarInscricaoJaFinalizadaConfirmada(seqInscricao);

            // Busca as informações da inscrição
            /*IncludesInscricao includes = IncludesInscricao.Documentos |
                                         IncludesInscricao.Documentos_ArquivoAnexado |
                                         IncludesInscricao.HistoricosSituacao |
                                         IncludesInscricao.Processo |
                                         IncludesInscricao.ConfiguracaoEtapa |
                                         IncludesInscricao.ConfiguracaoEtapa_EtapaProcesso |
                                         IncludesInscricao.Ofertas |
                                         IncludesInscricao.Ofertas_Oferta |
                                         IncludesInscricao.ArquivoComprovante |
                                         IncludesInscricao.Boletos_Titulos;
            Inscricao dadosInscricao = this.SearchByKey(new SMCSeqSpecification<Inscricao>(seqInscricao), includes);*/




            var dadosInscricao = this.SearchProjectionByKey(seqInscricao, x => new
            {
                x.SeqInscrito,
                x.SeqConfiguracaoEtapa,
                x.SeqGrupoOferta,
                x.SeqProcesso,
                x.Idioma,
                x.SeqPermissaoInscricaoForaPrazoInscrito,
                x.SeqArquivoComprovante,
                x.ConfiguracaoEtapa.SeqEtapaProcesso,
                x.Processo.SeqTipoProcesso,
                x.Processo.TipoProcesso.TokenResource,
                ControlaVagaInscricaoProcesso = x.Processo.ControlaVagaInscricao,
                x.ConfiguracaoEtapa.EtapaProcesso.Processo.TipoProcesso.IntegraGPC,
                Ofertas = x.Ofertas.Select(o => new
                {
                    o.SeqOferta,
                    o.Oferta.Ativo,
                    Cancelada = o.Oferta.DataCancelamento.HasValue && o.Oferta.DataCancelamento.Value <= DateTime.Now,
                    Vigente = o.Oferta.DataInicio <= DateTime.Now && DateTime.Now <= o.Oferta.DataFim,
                    o.Oferta.NumeroVagas,
                    o.Oferta.DescricaoCompleta,
                    o.Oferta.ExigePagamentoTaxa
                }).ToList(),
                HistoricosSituacao = x.HistoricosSituacao,
                TituloPagina = x.ConfiguracaoEtapa.Paginas.FirstOrDefault(f => f.Token == TOKENS.PAGINA_SELECAO_OFERTA).Idiomas.FirstOrDefault(i => i.Idioma == x.Idioma).Titulo,
                ConfiguracaoEtapaVigente = x.ConfiguracaoEtapa.DataInicio <= DateTime.Now && DateTime.Now <= x.ConfiguracaoEtapa.DataFim,
                DescricaoProcesso = x.Processo.Descricao,
                BoletoInscricao = x.Boletos.Select(b => new
                {
                    b.TipoBoleto,
                    b.Seq,
                    GerarNovoTitulo = !b.Titulos.Any() || b.Titulos.All(t => t.DataCancelamento.HasValue && t.DataCancelamento.Value <= DateTime.Now),
                    Taxas = b.Taxas.Select(s => new { s.NumeroItens, s.SeqTaxa })

                }).FirstOrDefault(b => b.TipoBoleto == TipoBoleto.Inscricao),
                SituacaoDocumentacao = x.SituacaoDocumentacao,
                TituloPago = x.TituloPago
            });
            if (dadosInscricao == null)
                throw new InscricaoInvalidaException();

            foreach (var oferta in dadosInscricao.Ofertas)
            {
                if (dadosInscricao.BoletoInscricao != null)
                {
                    foreach (var taxa in dadosInscricao.BoletoInscricao.Taxas)
                    {
                        var ofertaPeriodo = OfertaPeriodoTaxaDomainService.SearchBySpecification(
                                        new OfertaPeriodoTaxaVigenteSpecification() &
                                        new OfertaPeriodoTaxaFilterSpecification { SeqOferta = oferta.SeqOferta, SeqTaxa = taxa.SeqTaxa },
                                        x => x.Taxa.TipoTaxa).FirstOrDefault();

                        if (ofertaPeriodo != null)
                        {
                            if (taxa.NumeroItens > ofertaPeriodo.NumeroMaximo)
                            {
                                // Excecao de maior maximo
                                throw new InscricaoComTaxasNaoPermitidaException(ofertaPeriodo.Taxa.TipoTaxa.Descricao, dadosInscricao.TituloPagina);
                                //throw new NumeroMaximoTaxasExcedidoException(ofertaPeriodo.Taxa.TipoTaxa.Descricao, ofertaPeriodo.NumeroMaximo.HasValue ? ofertaPeriodo.NumeroMaximo.Value : 0);
                            }
                        }
                    }
                }

            }


            // Verifica regra para avançar na inscrição
            this.VerificarRegrasAvancarInscricao(seqInscricao, dadosInscricao.SeqConfiguracaoEtapa, dadosInscricao.SeqGrupoOferta, dadosInscricao.SeqInscrito);

            // Verifica regra de ofertas ativas
            var ofertaInativa = dadosInscricao.Ofertas.FirstOrDefault(o => o.Cancelada || !o.Ativo || !o.Vigente);
            if (ofertaInativa != null)
            {
                if (!ofertaInativa.Vigente && !InscritoDomainService.VerificaPermissaoInscricaoForaPrazo(dadosInscricao.SeqProcesso))
                {
                    string descricaoCompleta = OfertaDomainService.BuscarHierarquiaOfertaCompleta(ofertaInativa.SeqOferta).DescricaoCompleta;
                    switch (dadosInscricao.TokenResource)
                    {
                        case TOKENS.TOKEN_RESOURCE_AGENDAMENTO:
                            throw new InscricaoComOfertaInvalidaException("O", "agendamento", descricaoCompleta);
                        case TOKENS.TOKEN_RESOURCE_ENTREGA_DOCUMENTACAO:
                            throw new InscricaoComOfertaInvalidaException("A", "entrega de documentação", descricaoCompleta);
                        default:
                            throw new InscricaoComOfertaInvalidaException("A", "inscrição", descricaoCompleta);
                    }
                }
            }

            //Se o tipo de processo em questão estiver configurado para integrar com o GPC
            //e existir o campo PROJETO no formulário, verificar se existe alguma outra
            //inscrição para o processo em questão, com a situação atual da inscrição
            //igual a INSCRICAO_FINALIZADA ou INSCRICAO_CONFIRMADA, e com o mesmo projeto selecionado.
            //Em caso afirmativo, abortar a operação e emitir a mensagem de erro:
            //"Já existe uma inscrição para o projeto " < nome do projeto > ".            
            if (dadosInscricao.IntegraGPC)
            {
                var descricaoProjetoGPC = RawQuery<string>(string.Format(_query_formulario_gpc_campo_projeo, seqInscricao)).FirstOrDefault();
                //Passar a descrição completa do projeto com o Seq para a função validar o seq e a descrição
                if (!string.IsNullOrEmpty(descricaoProjetoGPC))
                {
                    ValidarFormularioSeminario(dadosInscricao.SeqProcesso, seqInscricao, descricaoProjetoGPC);
                }
            }

            // Verifica se o processo está configurado para controlar vagas
            if (dadosInscricao.ControlaVagaInscricaoProcesso)
            {
                foreach (var inscricaoOferta in dadosInscricao.Ofertas)
                {
                    var totalVagas = inscricaoOferta.NumeroVagas;
                    var vagasOcupadas = InscricaoOfertaDomainService.Count(new InscricaoOfertaInscricoesValidasSpecification(inscricaoOferta.SeqOferta));

                    if (vagasOcupadas >= totalVagas)
                    {
                        var hierarquiaCompletaOferta = inscricaoOferta.DescricaoCompleta;
                        var titualoPagina = dadosInscricao.TituloPagina;
                        var labelOferta = ProcessoIdiomaDomainService.BuscarDescricaoOfertaProcesso(dadosInscricao.SeqConfiguracaoEtapa, dadosInscricao.Idioma);
                        throw new InscricaoOfertaSemVagasException(hierarquiaCompletaOferta, titualoPagina, labelOferta);
                    }
                }
            }

            using (var unitOfWork = SMCUnitOfWork.Begin())
            {
                try
                {
                    var seqPermissaoInscricaoForaPrazoInscrito = RegistraInscriçãoForaPrazo(dadosInscricao.SeqPermissaoInscricaoForaPrazoInscrito, dadosInscricao.SeqInscrito, dadosInscricao.SeqProcesso, dadosInscricao.ConfiguracaoEtapaVigente);

                    //Alteração para atender a task: 64768.
                    //UC relacionado: Requirement 4511: UC_INS_002_02 - Inscrição
                    //Descrição: Incluir a NV06 no botão Comprovante: "O botão Comprovante deverá ser exibido somente se " +
                    //"a página de Comprovante de Inscrição  estiver configurada no fluxo de páginas da configuração de etapa associada à inscrição em questão."
                    long? seqArquivoComprovante = null;

                    if (arquivoComprovante != null)
                    {
                        var arquivo = arquivoComprovante.TransformToArray<byte>();

                        // Cria o arquivo do comprovante
                        var arquivoAnexado = new ArquivoAnexado();
                        arquivoAnexado.Seq = dadosInscricao.SeqArquivoComprovante.GetValueOrDefault();
                        arquivoAnexado.Nome = string.Format("ComprovanteInscricao{0}.pdf", seqInscricao);
                        arquivoAnexado.Tipo = "application/pdf";
                        arquivoAnexado.Conteudo = arquivo;
                        arquivoAnexado.Tamanho = arquivo.Length;
                        arquivoAnexado.State = SMCUploadFileState.Changed;

                        ArquivoAnexadoDomainService.SaveEntity(arquivoAnexado);
                        seqArquivoComprovante = arquivoAnexado.Seq;

                        //Arquivo no GED
                        ParametrosArquivoVO modelo = new ParametrosArquivoVO();
                        List<long> documentos = new List<long>() { seqArquivoComprovante.Value };
                        modelo.SeqInscricao = seqInscricao;
                        modelo.SeqsDocumentos = documentos;
                        modelo.TipoSistema = TipoSistema.Inscricao;
                        modelo.OrigemComprovanteInscricao = true;
                        ArquivoApiDoaminService.CriarArquivo(modelo);
                    }

                    // Inclui para a inscrição o historico de situação finalizada
                    // Atualiza os históricos
                    foreach (var situacao in dadosInscricao.HistoricosSituacao.Where(s => s.SeqEtapaProcesso == dadosInscricao.SeqEtapaProcesso && s.AtualEtapa))
                    {
                        situacao.AtualEtapa = false;
                        InscricaoHistoricoSituacaoDomainService.UpdateFields(situacao, x => x.AtualEtapa);
                    }
                    foreach (var situacao in dadosInscricao.HistoricosSituacao.Where(s => s.Atual))
                    {
                        situacao.Atual = false;
                        InscricaoHistoricoSituacaoDomainService.UpdateFields(situacao, x => x.Atual);
                    }

                    // Atualiza a data da inscrição para a data atual

                    var tokenSituacao = TOKENS.SITUACAO_INSCRICAO_FINALIZADA;
                    // Encontra a situação com token TOKENS.SITUACAO_INSCRICAO_FINALIZADA
                    InscricaoHistoricoSituacao novaSituacao = CriarNovaSituacao(seqInscricao, dadosInscricao.SeqTipoProcesso, dadosInscricao.SeqEtapaProcesso, tokenSituacao);
                    InscricaoHistoricoSituacaoDomainService.SaveEntity(novaSituacao);

                    // Cria o título para a inscrição
                    if (dadosInscricao.BoletoInscricao != null)
                    {
                        if (dadosInscricao.BoletoInscricao.GerarNovoTitulo)
                            this.InscricaoBoletoTituloDomainService.GerarNovoTitulo(dadosInscricao.BoletoInscricao.Seq);
                        //var titulo = InscricaoBoletoTituloDomainService.BuscarTitulo(dadosInscricao.BoletoInscricao.Seq, seqTitulo);
                        //InscricaoBoletoTituloDomainService.SaveEntity(titulo);
                    }

                    // Envia notificação para o inscrito
                    InscricaoEnvioNotificacaoDomainService.EnviarNotificacaoFinalizarInscricao(seqInscricao, dadosInscricao.SeqProcesso, dadosInscricao.SeqInscrito, dadosInscricao.SeqConfiguracaoEtapa, dadosInscricao.DescricaoProcesso, dadosInscricao.Idioma, tokenSituacao);

                    // Se a inscrição não possuir documentos obrigatórios, ou se existir, mas a situação da documentação for "Entregue" ou "Entregue com pendência"
                    // E não existir cobrança de taxa, ou existir mas a taxa já estiver paga,
                    // alterar a situação da inscrição para INSCRICAO_CONFIRMADA *
                    var possuiDocumentoRequerido = PossuiDocumentoRequerido(dadosInscricao.SeqConfiguracaoEtapa);
                    if (
                        (!possuiDocumentoRequerido
                        || dadosInscricao.SituacaoDocumentacao == SituacaoDocumentacao.Entregue
                        || dadosInscricao.SituacaoDocumentacao == SituacaoDocumentacao.EntregueComPendencia)
                        &&
                        (!dadosInscricao.Ofertas.FirstOrDefault().ExigePagamentoTaxa
                        || dadosInscricao.TituloPago)
                        )
                        tokenSituacao = TOKENS.SITUACAO_INSCRICAO_CONFIRMADA;

                    if (dadosInscricao.SeqProcesso == 256)
                        SMCLogger.Information($"Inscrição: {seqInscricao}; Situacao final {tokenSituacao}");

                    // Salva os campos atualizados da inscrição
                    var inscricaoUpdate = new Inscricao
                    {
                        Seq = seqInscricao,
                        SeqPermissaoInscricaoForaPrazoInscrito = dadosInscricao.SeqPermissaoInscricaoForaPrazoInscrito,
                        SeqArquivoComprovante = seqArquivoComprovante,
                        DataInscricao = DateTime.Now,
                        AceiteConversaoPDF = aceiteConversaoPDF
                    };

                    if (seqPermissaoInscricaoForaPrazoInscrito != null)
                        inscricaoUpdate.SeqPermissaoInscricaoForaPrazoInscrito = seqPermissaoInscricaoForaPrazoInscrito;

                    UpdateFields(inscricaoUpdate, x => x.SeqPermissaoInscricaoForaPrazoInscrito, x => x.SeqArquivoComprovante, x => x.DataInscricao, x => x.AceiteConversaoPDF);

                    // Se o token da situação for confirmada, muda a situação para inscrição confirmada
                    if (tokenSituacao == TOKENS.SITUACAO_INSCRICAO_CONFIRMADA)
                    {
                        var tipoProcessoSituacao = BuscarTipoProcessoSituacao(dadosInscricao.SeqTipoProcesso, TOKENS.SITUACAO_INSCRICAO_CONFIRMADA);
                        InscricaoHistoricoSituacaoDomainService.AlterarSituacaoInscricoes(new AlteracaoSituacaoVO()
                        {
                            SeqTipoProcessoSituacaoDestino = tipoProcessoSituacao.Seq,
                            SeqInscricoes = new List<long> { seqInscricao }
                        });
                    }


                    AtualizarDadosLGPD(dadosInscricao.SeqInscrito, consentimentoLGPD);

                    unitOfWork.Commit();
                }
                catch (Exception e)
                {
                    unitOfWork.Rollback();
                    throw e;
                }
            }
        }

        public void AlterarObservacaoInscricao(long seq, string observacao)
        {
            var inscricao = SearchByKey(new SMCSeqSpecification<Inscricao>(seq));
            inscricao.Observacao = observacao;
            SaveEntity(inscricao);
        }

        private InscricaoHistoricoSituacao CriarNovaSituacao(long seqInscricao, long seqTipoProcesso, long seqEtapaProcesso, string token)
        {
            TipoProcessoSituacao tipoProcessoSituacao = BuscarTipoProcessoSituacao(seqTipoProcesso, token);

            var novaSituacao = new InscricaoHistoricoSituacao()
            {
                SeqInscricao = seqInscricao,
                SeqTipoProcessoSituacao = tipoProcessoSituacao.Seq,
                SeqEtapaProcesso = seqEtapaProcesso,
                DataSituacao = DateTime.Now,
                AtualEtapa = true,
                Atual = true
            };

            return novaSituacao;
        }

        private TipoProcessoSituacao BuscarTipoProcessoSituacao(long seqTipoProcesso, string token)
        {
            TipoProcessoSituacaoFilterSpecification spec = new TipoProcessoSituacaoFilterSpecification()
            {
                SeqTipoProcesso = seqTipoProcesso,
                Token = token
            };
            TipoProcessoSituacao tipoProcessoSituacao = TipoProcessoSituacaoDomainService.SearchByKey(spec);
            if (tipoProcessoSituacao == null)
                throw new SituacaoTokenNaoIdentificadaException(token);
            return tipoProcessoSituacao;
        }

        private long? RegistraInscriçãoForaPrazo(long? seqPermissaoInscricaoForaPrazoInscrito, long seqInscrito, long seqProcesso, bool configuracaoEtapaVigente)
        {
            if (!seqPermissaoInscricaoForaPrazoInscrito.HasValue)
            {
                // Verifica se o usuario está fazendo uma inscrição fora do prazo
                var inscricaoForaPrazo = PermissaoInscricaoForaPrazoInscritoDomainService.SearchBySpecification(
                                            new PermissaoInscricaoForaPrazoInscritoSpecification
                                            {
                                                SeqInscrito = seqInscrito,
                                                SeqProcesso = seqProcesso,
                                                DataAtual = DateTime.Now
                                            }).FirstOrDefault();

                if (!configuracaoEtapaVigente && inscricaoForaPrazo != null)
                    return inscricaoForaPrazo.Seq;
            }
            return null;
        }

        #endregion Finalizar Inscrição

        #region Integracao

        public List<PessoaIntegracaoVO> BuscarDadosInscricoes(List<long> seqOfertas)
        {
            // Busca por todos os inscritos que possuem alguma situação CONVOCADA que ainda não foram exportados.
            var spec = new InscritoConvocadoSGASpecification()
            {
                SeqOfertas = seqOfertas
            };
            var data = SearchProjectionBySpecification(spec,
                                x => new PessoaIntegracaoVO
                                {
                                    SeqInscricao = x.Seq,
                                    TokenNivelEnsino = x.Formularios.Select(
                                                            f => f.DadosCampos.FirstOrDefault(g => g.Token == TOKENS.CAMPO_NIVEL_ENSINO).Valor).FirstOrDefault(),

                                    SeqUsuarioSAS = x.Inscrito.SeqUsuarioSas,
                                    SeqInscrito = x.SeqInscrito,
                                    Nome = x.Inscrito.Nome,
                                    NomeSocial = x.Inscrito.NomeSocial,
                                    NomePai = x.Inscrito.NomePai,
                                    NomeMae = x.Inscrito.NomeMae,
                                    Email = x.Inscrito.Email,
                                    Sexo = x.Inscrito.Sexo,
                                    EstadoCivil = x.Inscrito.EstadoCivil,
                                    UfNaturalidade = x.Inscrito.UfNaturalidade,
                                    CodigoCidadeNaturalidade = x.Inscrito.CodigoCidadeNaturalidade,
                                    DescricaoNaturalidadeEstrangeira = x.Inscrito.DescricaoNaturalidadeEstrangeira,
                                    NumeroIdentidade = x.Inscrito.NumeroIdentidade,
                                    OrgaoEmissorIdentidade = x.Inscrito.OrgaoEmissorIdentidade,
                                    UfIdentidade = x.Inscrito.UfIdentidade,
                                    Cpf = x.Inscrito.Cpf,
                                    DataNascimento = x.Inscrito.DataNascimento,
                                    TipoNacionalidade = x.Inscrito.Nacionalidade,
                                    CodigoPaisNacionalidade = x.Inscrito.CodigoPaisNacionalidade,
                                    NumeroPassaporte = x.Inscrito.NumeroPassaporte,
                                    DataValidadePassaporte = x.Inscrito.DataValidadePassaporte,
                                    CodigoPaisEmissaoPassaporte = x.Inscrito.CodigoPaisEmissaoPassaporte,

                                    RacaCor = x.Formularios.Select(
                                                            f => f.DadosCampos.FirstOrDefault(g => g.Token == TOKENS.CAMPO_RACA_COR).Valor).FirstOrDefault(),

                                    // Titulo Eleitor
                                    NumeroTituloEleitor = x.Formularios.Select(
                                                            f => f.DadosCampos.FirstOrDefault(g => g.Token == TOKENS.CAMPO_TITULO_ELEITOR_NUMERO).Valor).FirstOrDefault(),
                                    NumeroZonaTituloEleitor = x.Formularios.Select(
                                                            f => f.DadosCampos.FirstOrDefault(g => g.Token == TOKENS.CAMPO_TITULO_ELEITOR_ZONA).Valor).FirstOrDefault(),
                                    NumeroSecaoTituloEleitor = x.Formularios.Select(
                                                            f => f.DadosCampos.FirstOrDefault(g => g.Token == TOKENS.CAMPO_TITULO_ELEITOR_SECAO).Valor).FirstOrDefault(),
                                    UfTituloEleitor = x.Formularios.Select(
                                                            f => f.DadosCampos.FirstOrDefault(g => g.Token == TOKENS.CAMPO_TITULO_ELEITOR_ESTADO_EMISSOR).Valor).FirstOrDefault(),

                                    // PisPasep
                                    TipoPisPasep = x.Formularios.Select(
                                                            f => f.DadosCampos.FirstOrDefault(g => g.Token == TOKENS.CAMPO_PIS_PASEP_TIPO).Valor).FirstOrDefault(),
                                    NumeroPisPasep = x.Formularios.Select(
                                                            f => f.DadosCampos.FirstOrDefault(g => g.Token == TOKENS.CAMPO_PIS_PASEP_NUMERO).Valor).FirstOrDefault(),
                                    DataPisPasep = x.Formularios.Select(
                                                            f => f.DadosCampos.FirstOrDefault(g => g.Token == TOKENS.CAMPO_PIS_PASEP_DATA).Valor).FirstOrDefault(),

                                    // Documento Militar
                                    NumeroDocumentoMilitar = x.Formularios.Select(
                                                            f => f.DadosCampos.FirstOrDefault(g => g.Token == TOKENS.CAMPO_DOCUMENTO_MILITAR_NUMERO).Valor).FirstOrDefault(),
                                    CsmDocumentoMilitar = x.Formularios.Select(
                                                            f => f.DadosCampos.FirstOrDefault(g => g.Token == TOKENS.CAMPO_DOCUMENTO_MILITAR_CSM).Valor).FirstOrDefault(),
                                    TipoDocumentoMilitar = x.Formularios.Select(
                                                            f => f.DadosCampos.FirstOrDefault(g => g.Token == TOKENS.CAMPO_DOCUMENTO_MILITAR_TIPO).Valor).FirstOrDefault(),
                                    UfDocumentoMilitar = x.Formularios.Select(
                                                            f => f.DadosCampos.FirstOrDefault(g => g.Token == TOKENS.CAMPO_DOCUMENTO_MILITAR_ESTADO).Valor).FirstOrDefault(),

                                    NecessidadeEspecial = x.Formularios.Select(
                                                            f => f.DadosCampos.FirstOrDefault(g => g.Token == TOKENS.CAMPO_PORTADOR_NECESSIDADES_ESPECIAIS).Valor).FirstOrDefault(),
                                    TipoNecessidadeEspecial = x.Formularios.Select(
                                                            f => f.DadosCampos.FirstOrDefault(g => g.Token == TOKENS.CAMPO_TIPO_NECESSIDADE_ESPECIAL).Valor).FirstOrDefault(),

                                    // Endereços
                                    Enderecos = x.Inscrito.Enderecos.Select(f => new EnderecoPessoaIntegracaoVO()
                                    {
                                        TipoEndereco = f.TipoEndereco,
                                        CodigoPais = f.CodigoPais,
                                        Cep = f.Cep,
                                        Logradouro = f.Logradouro,
                                        Numero = f.Numero,
                                        Complemento = f.Complemento,
                                        Bairro = f.Bairro,
                                        CodigoCidade = f.CodigoCidade,
                                        NomeCidade = f.NomeCidade,
                                        SiglaUf = f.Uf,
                                        Correspondencia = f.Correspondencia
                                    }).ToList(),

                                    Telefones = x.Inscrito.Telefones.Select(f => new TelefonePessoaIntegracaoVO()
                                    {
                                        TipoTelefone = f.TipoTelefone,
                                        CodigoPais = f.CodigoPais,
                                        CodigoArea = f.CodigoArea,
                                        Numero = f.Numero
                                    }).ToList(),

                                    EnderecosEletronicos = x.Inscrito.EnderecosEletronicos.Select(f => new EnderecoEletronicoPessoaIntegracaoVO()
                                    {
                                        TipoEnderecoEletronico = f.TipoEnderecoEletronico,
                                        Descricao = f.Descricao
                                    }).ToList(),

                                    // Busca apenas as ofertas que estão com a situação CONVOCADO
                                    Ofertas = x.Ofertas.Where(f => f.HistoricosSituacao.Any(y => y.Atual && y.TipoProcessoSituacao.Token == TOKENS.SITUACAO_CONVOCADO)
                                                                && (!f.Exportado.HasValue || !f.Exportado.Value))
                                                        .Select(f => new OfertaIntegracaoVO()
                                                        {
                                                            SeqOferta = f.SeqOferta,
                                                            SeqInscricaoOferta = f.Seq
                                                        }).ToList(),

                                    Documentos = x.Documentos.Select(f => new DocumentosIntegracaoVO
                                    {
                                        SeqArquivoAnexado = f.SeqArquivoAnexado,
                                        SeqTipoDocumento = f.DocumentoRequerido.SeqTipoDocumento,
                                        DataEntrega = f.DataEntrega,
                                        DataPrazoEntrega = f.DataPrazoEntrega,
                                        FormaEntregaDocumento = f.FormaEntregaDocumento,
                                        VersaoDocumento = f.VersaoDocumento,
                                        Observacao = f.Observacao,
                                        SituacaoEntregaDocumento = f.SituacaoEntregaDocumento,
                                        DescricaoTipoDocumento = f.DocumentoRequerido.TipoDocumento.Descricao
                                    }).ToList()
                                }).ToList();

            foreach (var item in data)
            {
                // Adiciona o email da pessoa se não estiver cadastrado na lista de endereços eletronicos
                item.EnderecosEletronicos.Add(new EnderecoEletronicoPessoaIntegracaoVO
                {
                    TipoEnderecoEletronico = TipoEnderecoEletronico.Email,
                    Descricao = item.Email
                });

                // Busca os emails ativos para o inscrito no SAS.
                var emailsSas = UsuarioService.BuscarEmailsUsuario(item.SeqUsuarioSAS.Value, true);
                foreach (var emailSas in emailsSas)
                {
                    item.EnderecosEletronicos.Add(new EnderecoEletronicoPessoaIntegracaoVO
                    {
                        TipoEnderecoEletronico = TipoEnderecoEletronico.Email,
                        Descricao = emailSas.Email
                    });
                }

                // Remove todos os endereços eletronicos duplicados.
                item.EnderecosEletronicos = item.EnderecosEletronicos.SMCDistinct(f => new { Descricao = f.Descricao?.ToLower().Trim(), f.TipoEnderecoEletronico }).ToList();

                // Ajusta os campos objetivos do SGF para pegar apenas o Seq
                item.TokenNivelEnsino = item.TokenNivelEnsino?.Split('|').FirstOrDefault();

                item.RacaCor = item.RacaCor?.Split('|').FirstOrDefault();

                item.UfTituloEleitor = item.UfTituloEleitor?.Split('|').FirstOrDefault();

                item.UfDocumentoMilitar = item.UfDocumentoMilitar?.Split('|').FirstOrDefault();
                item.TipoDocumentoMilitar = item.TipoDocumentoMilitar?.Split('|').FirstOrDefault();

                item.TipoPisPasep = item.TipoPisPasep?.Split('|').FirstOrDefault();

                item.NecessidadeEspecial = item.NecessidadeEspecial?.Split('|').FirstOrDefault();
                item.TipoNecessidadeEspecial = item.TipoNecessidadeEspecial?.Split('|').FirstOrDefault();
            }

            return data;
        }

        #endregion Integracao

        #region Liberação da Alteração de Inscrição

        public void LiberarAlteracaoInscricao(long seqInscricao)
        {
            using (var unityOfWork = SMCUnitOfWork.Begin())
            {
                try
                {
                    var specInscricao = new SMCSeqSpecification<Inscricao>(seqInscricao);

                    var dadosLiberacaoInscricao = this.SearchProjectionByKey(specInscricao, i => new DadosLiberacaoInscricaoVO()
                    {
                        SeqInscricao = i.Seq,
                        TokenSituacaoInscricao = i.HistoricosSituacao.FirstOrDefault(h => h.Atual).TipoProcessoSituacao.Token,
                        InscricaoForaPrazoDataInicio = i.ConfiguracaoEtapa.EtapaProcesso.Processo.PermissoesInscricaoForaPrazo.Where(p => p.SeqProcesso == i.SeqProcesso && p.Inscritos.FirstOrDefault(w => w.SeqInscrito == i.SeqInscrito) != null).FirstOrDefault().DataInicio,
                        InscricaoForaPrazoDataFim = i.ConfiguracaoEtapa.EtapaProcesso.Processo.PermissoesInscricaoForaPrazo.Where(p => p.SeqProcesso == i.SeqProcesso && p.Inscritos.FirstOrDefault(w => w.SeqInscrito == i.SeqInscrito) != null).FirstOrDefault().DataFim,
                        ArquivoComprovante = i.ArquivoComprovante,
                        EnviarNotificacao = i.Processo.ConfiguracoesNotificacao.FirstOrDefault(c => c.TipoNotificacao.Token == TOKENS.NOTIFICACAO_LIBERACAO_ALTERACAO_INSCRICAO).EnvioAutomatico,
                        SeqConfiguracaoEtapaPagina = i.ConfiguracaoEtapa.Paginas.FirstOrDefault(p => p.Token == TOKENS.PAGINA_CONFIRMACAO_INSCRICAO).Seq
                    });

                    //1. Excluir o comprovante gerado para a inscrição do candidato.
                    ExcluirArquivoComprovanteInscricao(dadosLiberacaoInscricao.SeqInscricao, dadosLiberacaoInscricao.ArquivoComprovante);

                    var inscricao = this.SearchByKey(new SMCSeqSpecification<Inscricao>(seqInscricao),
                        IncludesInscricao.Processo |
                        IncludesInscricao.ConfiguracaoEtapa |
                        IncludesInscricao.HistoricosSituacao |
                        IncludesInscricao.HistoricosPagina);

                    var tokenSituacao = string.Empty;

                    //2. Caso a situação da inscrição seja INSCRICAO_CONFIRMADA,
                    //voltar a inscrição para a situação “INSCRICAO_FINALIZADA” e em seguida voltar
                    //para “INSCRICAO_INICIADA”.
                    //Caso a situação da inscrição já seja “INSCRICAO_FINALIZADA”, volta-la para “INSCRICAO_INICIADA”.
                    if (dadosLiberacaoInscricao.TokenSituacaoInscricao == TOKENS.SITUACAO_INSCRICAO_CONFIRMADA)
                    {
                        // Inclui para a inscrição o historico de situação finalizada
                        foreach (var situacao in inscricao.HistoricosSituacao
                                                          .Where(s => s.SeqEtapaProcesso == inscricao.ConfiguracaoEtapa.SeqEtapaProcesso && s.AtualEtapa))
                        {
                            situacao.AtualEtapa = false;
                        }
                        foreach (var situacao in inscricao.HistoricosSituacao.Where(s => s.Atual))
                        {
                            situacao.Atual = false;
                        }

                        tokenSituacao = TOKENS.SITUACAO_INSCRICAO_FINALIZADA;
                        var inscricaoHistoricoSituacao = CriarNovaSituacao(inscricao.Seq, inscricao.Processo.SeqTipoProcesso, inscricao.ConfiguracaoEtapa.SeqEtapaProcesso, tokenSituacao);
                        inscricaoHistoricoSituacao.AtualEtapa = false;
                        inscricaoHistoricoSituacao.Atual = false;
                        inscricao.HistoricosSituacao.Add(inscricaoHistoricoSituacao);

                        // Inclui para a inscrição o historico de situação iniciada
                        tokenSituacao = TOKENS.SITUACAO_INSCRICAO_INICIADA;
                        inscricaoHistoricoSituacao = CriarNovaSituacao(inscricao.Seq, inscricao.Processo.SeqTipoProcesso, inscricao.ConfiguracaoEtapa.SeqEtapaProcesso, tokenSituacao);
                        inscricao.HistoricosSituacao.Add(inscricaoHistoricoSituacao);
                    }
                    else if (dadosLiberacaoInscricao.TokenSituacaoInscricao == TOKENS.SITUACAO_INSCRICAO_FINALIZADA)
                    {
                        // Inclui para a inscrição o historico de situação iniciada
                        foreach (var situacao in inscricao.HistoricosSituacao
                                                          .Where(s => s.SeqEtapaProcesso == inscricao.ConfiguracaoEtapa.SeqEtapaProcesso && s.AtualEtapa))
                        {
                            situacao.AtualEtapa = false;
                        }
                        foreach (var situacao in inscricao.HistoricosSituacao.Where(s => s.Atual))
                        {
                            situacao.Atual = false;
                        }

                        tokenSituacao = TOKENS.SITUACAO_INSCRICAO_INICIADA;
                        var inscricaoHistoricoSituacao = CriarNovaSituacao(inscricao.Seq, inscricao.Processo.SeqTipoProcesso, inscricao.ConfiguracaoEtapa.SeqEtapaProcesso, tokenSituacao);
                        inscricao.HistoricosSituacao.Add(inscricaoHistoricoSituacao);
                    }

                    /*6. Se houver registro de histórico de situação da inscrição-oferta com a situação atual
                     * igual a CANDIDATO_CONFIRMADO, passar o indicador de atual e de atual etapa para "Não"
                     * no histórico de situação da inscrição oferta.*/
                    ValidarHistoricoSituacaoInscricaoOferta(seqInscricao);

                    //Salva a inscrição
                    this.SaveEntity(inscricao);

                    //3.Incluir no histórico de páginas da inscrição do candidato, um registro para a
                    //página CONFIRMACAO_INSCRICAO.
                    var inscricaoHistoricoPagina = new InscricaoHistoricoPagina()
                    {
                        Seq = 0,
                        DataAcesso = DateTime.Now,
                        SeqConfiguracaoEtapaPagina = dadosLiberacaoInscricao.SeqConfiguracaoEtapaPagina.Value,
                        SeqInscricao = dadosLiberacaoInscricao.SeqInscricao,
                        IpAcesso = SMCContext.ClientAddress.Ip
                    };
                    InscricaoHistoricoPaginaDomainService.InsertEntity(inscricaoHistoricoPagina);

                    //4.Enviar para o inscrito o e-mail de liberação da alteração dos dados da inscrição,
                    //caso este esteja configurado no processo. Token: LIBERACAO_ALTERACAO_INSCRICAO.
                    if (dadosLiberacaoInscricao.EnviarNotificacao.HasValue && dadosLiberacaoInscricao.EnviarNotificacao.Value)
                        InscricaoEnvioNotificacaoDomainService.EnviarNotificacaoLiberarAlteracaoInscricao(inscricao);

                    if (dadosLiberacaoInscricao.ArquivoComprovante != null)
                    {
                        //Arquivo no GED
                        if (dadosLiberacaoInscricao.ArquivoComprovante.UidArquivoGed.HasValue)
                        {
                            arquivosDeletadosGED.Add(dadosLiberacaoInscricao.ArquivoComprovante.UidArquivoGed.ToString());
                            ExcluirArquivoGED(dadosLiberacaoInscricao.SeqInscricao, arquivosDeletadosGED);
                        }
                    }

                    unityOfWork.Commit();
                }
                catch (Exception)
                {
                    unityOfWork.Rollback();
                    throw;
                }
            };
        }

        /// <summary>
        /// Se houver registro de histórico de situação da inscrição-oferta com a situação atual
        /// * igual a CANDIDATO_CONFIRMADO, passar o indicador de atual e de atual etapa para "Não"
        /// * no histórico de situação da inscrição oferta.
        /// </summary>
        /// <param name="seqInscricao"></param>
        private void ValidarHistoricoSituacaoInscricaoOferta(long seqInscricao)
        {
            var historicos = this.SearchProjectionByKey(seqInscricao, x => x.Ofertas.SelectMany(o => o.HistoricosSituacao)
                                                                                    .Where(h => h.Atual && h.TipoProcessoSituacao.Token == TOKENS.SITUACAO_CANDIDATO_CONFIRMADO)).ToList();
            if (historicos != null)
            {
                foreach (var historico in historicos)
                {
                    historico.Atual = false;
                    historico.AtualEtapa = false;

                    InscricaoOfertaHistoricoSituacaoDomainService.SaveEntity(historico);
                }
            }
        }

        /// <summary>
        /// Validar RN_INS_141 Liberação da alteração de inscrição
        /// Regras 1 e 2
        /// </summary>
        /// <param name="seqInscricao"></param>
        public void ValidarLiberacaoAlteracaoInscricao(long seqInscricao)
        {
            var specInscricao = new SMCSeqSpecification<Inscricao>(seqInscricao);

            var dadosLiberacaoInscricao = this.SearchProjectionByKey(specInscricao, i => new
            {
                TokenSituacaoInscricao = i.HistoricosSituacao.FirstOrDefault(h => h.Atual).TipoProcessoSituacao.Token,
                TokenSituacoesOfertas = i.Ofertas.Select(o => o.HistoricosSituacao.FirstOrDefault(h => h.Atual).TipoProcessoSituacao.Token).ToList(),
                SituacaoEtapa = i.ConfiguracaoEtapa.EtapaProcesso.SituacaoEtapa,
                DataInicioEtapa = i.ConfiguracaoEtapa.EtapaProcesso.DataInicioEtapa,
                DataFimEtapa = i.ConfiguracaoEtapa.EtapaProcesso.DataFimEtapa,
                PermissoesForaDoPrazo = i.ConfiguracaoEtapa.EtapaProcesso.Processo.PermissoesInscricaoForaPrazo.Where(p => p.SeqProcesso == i.SeqProcesso && p.Inscritos.Any(w => w.SeqInscrito == i.SeqInscrito)).Select(ifp => new
                {
                    ifp.DataInicio,
                    ifp.DataFim
                }).ToList(),
                //InscricaoForaPrazoDataInicio = i.ConfiguracaoEtapa.EtapaProcesso.Processo.PermissoesInscricaoForaPrazo.Where(p => p.SeqProcesso == i.SeqProcesso && p.Inscritos.FirstOrDefault(w => w.SeqInscrito == i.SeqInscrito) != null).FirstOrDefault().DataInicio,
                //InscricaoForaPrazoDataFim = i.ConfiguracaoEtapa.EtapaProcesso.Processo.PermissoesInscricaoForaPrazo.Where(p => p.SeqProcesso == i.SeqProcesso && p.Inscritos.FirstOrDefault(w => w.SeqInscrito == i.SeqInscrito) != null).FirstOrDefault().DataFim,
            });

            //RN_INS_141 Liberação da alteração de inscrição
            //1 - Se a situação da inscrição for diferente de INSCRICAO_FINALIZADA e INSCRICAO_CONFIRMADA,
            //    ou for INSCRICAO_CONFIRMADA e existir inscrição-oferta cuja situação seja diferente de
            //    CANDIDATO_CONFIRMADO, abortar a operação e emitir a mensagem de erro:
            //    “Para liberar a alteração dos dados é necessário que a inscrição esteja na situação
            //    “Inscrição Finalizada” ou “Inscrição Confirmada” e não haja lançamento de resultado.
            //  Se a inscrição está como INSCRICAO_CONFIRMADA e não tem registro na inscrição-oferta não é pra dar erro.
            if ((dadosLiberacaoInscricao.TokenSituacaoInscricao != TOKENS.SITUACAO_INSCRICAO_FINALIZADA &&
                dadosLiberacaoInscricao.TokenSituacaoInscricao != TOKENS.SITUACAO_INSCRICAO_CONFIRMADA) ||
               (dadosLiberacaoInscricao.TokenSituacaoInscricao == TOKENS.SITUACAO_INSCRICAO_CONFIRMADA &&

                dadosLiberacaoInscricao.TokenSituacoesOfertas.Where(g => g != null).Any() &&
                dadosLiberacaoInscricao.TokenSituacoesOfertas.Any(s => s != TOKENS.SITUACAO_CANDIDATO_CONFIRMADO)))
            {
                throw new InscricaoForaSituacaoPermitidaAlteracaoExclusaoException();
            }

            /*2. Se a etapa de inscrição do processo não estiver liberada e vigente e
             *   não existir uma permissão de inscrição fora do prazo vigente para o candidato em questão,
             *   abortar a operação e emitir a mensagem de erro:*/

            if (dadosLiberacaoInscricao.SituacaoEtapa != SituacaoEtapa.Liberada)
                throw new InscricaoComEtapaInativaException();

            // Verifica se está fora do prazo a etapa
            var vigenciaEtapaInativa = DataInativa(dadosLiberacaoInscricao.DataInicioEtapa, dadosLiberacaoInscricao.DataFimEtapa);

            // Armazena se tem alguma permissão de inscrição fora do prazo ativa
            bool algumForaPrazoAtiva = false;
            foreach (var item in dadosLiberacaoInscricao.PermissoesForaDoPrazo)
            {
                if (!DataInativa(item.DataInicio, item.DataFim))
                {
                    algumForaPrazoAtiva = true;
                    break;
                }
            }

            if (vigenciaEtapaInativa && !algumForaPrazoAtiva)
                throw new InscricaoComEtapaInativaException();
        }

        private bool DataInativa(DateTime? dataInicioEtapa, DateTime? dataFimEtapa)
        {
            return  //Se a data atual estiver antes do período de vigência da etapa
                (DateTime.Now <= dataInicioEtapa.Value && dataFimEtapa.HasValue && DateTime.Now <= dataFimEtapa.Value) ||
                (DateTime.Now <= dataInicioEtapa.Value && !dataFimEtapa.HasValue) ||
                // ou se a data atual estiver após o período de vigência
                (DateTime.Now >= dataInicioEtapa.Value && dataFimEtapa.HasValue && DateTime.Now >= dataFimEtapa.Value);
        }

        /// <summary>
        /// Excluir o comprovante gerado para a inscrição do candidato.
        /// </summary>
        /// <param name="seqInscricao">Sequencial da inscrição</param>
        /// <param name="arquivoComprovante">Arquivo comprovante</param>
        private void ExcluirArquivoComprovanteInscricao(long seqInscricao, ArquivoAnexado arquivoComprovante)
        {
            var inscricao = SearchByKey(new SMCSeqSpecification<Inscricao>(seqInscricao));
            if (inscricao != null && arquivoComprovante != null)
            {
                inscricao.SeqArquivoComprovante = null;
                SaveEntity(inscricao);

                this.ArquivoAnexadoDomainService.DeleteEntity(arquivoComprovante);
            }
        }

        #endregion Liberação da Alteração de Inscrição

        #region Nova Entrega de Documentos

        public NovaEntregaDocumentacaoCabecalhoVO BuscarCabecalhoNovaEntregaDocumentacao(long seqInscricao)
        {
            var specInscricao = new SMCSeqSpecification<Inscricao>(seqInscricao);

            var inscricao = this.SearchProjectionByKey(specInscricao, i => new
            {
                NomeInscrito = i.Inscrito.Nome,
                NomeSocialInscrito = i.Inscrito.NomeSocial,
                Processo = i.Processo.Descricao,
                GrupoOferta = i.GrupoOferta.Nome,
                QuantidadeGruposOfertaProcesso = i.Processo.GruposOferta.Count,
                Ofertas = i.Ofertas.Select(o => new { o.SeqOferta, o.NumeroOpcao }).ToList()
            });

            var novaEntregaDocumentacaoCabecalho = new NovaEntregaDocumentacaoCabecalhoVO()
            {
                Usuario = !string.IsNullOrEmpty(inscricao.NomeSocialInscrito) ? inscricao.NomeSocialInscrito : inscricao.NomeInscrito,
                Processo = inscricao.Processo,
                GrupoOferta = inscricao.QuantidadeGruposOfertaProcesso > 1 ? inscricao.GrupoOferta : string.Empty,
            };

            novaEntregaDocumentacaoCabecalho.Oferta = new List<string>();

            foreach (var oferta in inscricao.Ofertas.OrderBy(o => o.NumeroOpcao))
            {
                novaEntregaDocumentacaoCabecalho.Oferta.Add(OfertaDomainService.BuscarHierarquiaOfertaCompleta(oferta.SeqOferta, false).DescricaoCompleta);
            }

            return novaEntregaDocumentacaoCabecalho;
        }

        public NovaEntregaDocumentacaoVO BuscarDocumentosNovaEntregaDocumentacao(long seqInscricao)
        {
            NovaEntregaDocumentacaoVO retorno = new NovaEntregaDocumentacaoVO()
            {
                SeqInscricao = seqInscricao,
                Documentos = new List<NovaEntregaDocumentacaoDocumentoVO>()
            };

            InscricaoDocumentoFilterSpecification filtroDocumentos = new InscricaoDocumentoFilterSpecification() { SeqInscricao = seqInscricao };
            SumarioDocumentosEntreguesVO documentosSumarioEntrega = InscricaoDocumentoDomainService.BuscarSumarioDocumentosEntregue(filtroDocumentos);

            List<InscricaoDocumentoVO> documentosInscricaoIndiferenteOuPedente = ListarDocumentosInscricaoPendentesOuIndeferido(documentosSumarioEntrega);

            retorno.Documentos.AddRange(MontarListaNovaEntregaDocumentacao(documentosInscricaoIndiferenteOuPedente));

            return retorno;
        }

        /// <summary>
        /// Listar documentos obrigatórios requeridos pendentes ou indeferidos da inscrição
        /// </summary>
        /// <param name="sumarioDocumentosEntreguesVO">Dados do sumario de documentos</param>
        /// <returns>Lista de documentos obrigatorios</returns>
        private List<InscricaoDocumentoVO> ListarDocumentosInscricaoPendentesOuIndeferido(SumarioDocumentosEntreguesVO sumarioDocumentosEntreguesVO)
        {
            List<InscricaoDocumentoVO> retorno = new List<InscricaoDocumentoVO>();
            retorno.AddRange(sumarioDocumentosEntreguesVO.DocumentosObrigatorios
                                                   .Where(d => d.PermiteUploadArquivo)
                                                   .SelectMany(sm => sm.InscricaoDocumentos)
                                                   .Where(id => id.SituacaoEntregaDocumento == SituacaoEntregaDocumento.Indeferido ||
                                                                        id.SituacaoEntregaDocumento == SituacaoEntregaDocumento.Pendente).ToList());

            retorno.AddRange(sumarioDocumentosEntreguesVO.GruposDocumentos
                                                            .SelectMany(g => g.DocumentosRequeridosGrupo)
                                                            .Where(d => d.PermiteUploadArquivo)
                                                            .SelectMany(sm => sm.InscricaoDocumentos)
                                                            .Where(id => id.SituacaoEntregaDocumento == SituacaoEntregaDocumento.Indeferido ||
                                                            id.SituacaoEntregaDocumento == SituacaoEntregaDocumento.Pendente).ToList());
            return retorno;
        }

        /// <summary>
        /// Montar lista de uma nova documentação 
        /// </summary>
        /// <param name="listaDocumentosRequeridosObrigatorios">Lista de documentos requeridos obrigatórios</param>
        /// <returns>Lista formatada dos documentos para nova entrega ordenados</returns>
        private List<NovaEntregaDocumentacaoDocumentoVO> MontarListaNovaEntregaDocumentacao(List<InscricaoDocumentoVO> listaDocumentosInscricao)
        {
            List<NovaEntregaDocumentacaoDocumentoVO> retorno = new List<NovaEntregaDocumentacaoDocumentoVO>();

            retorno = listaDocumentosInscricao.Select(s => new NovaEntregaDocumentacaoDocumentoVO()
            {
                Seq = s.Seq,
                SeqDocumentoRequerido = s.SeqDocumentoRequerido,
                DescricaoTipoDocumento = s.DescricaoTipoDocumento,
                SeqArquivoAnexadoAnterior = s.SeqArquivoAnexado,
                Observacao = s.ExibirObservacaoParaInscrito && !string.IsNullOrEmpty(s.Observacao) ?
                                                                             s.Observacao : string.Empty,
                SituacaoEntregaDocumento = s.SituacaoEntregaDocumento
            }).ToList();

            return retorno.OrderBy(o => o.DescricaoTipoDocumento).ToList();

        }

        #endregion Nova Entrega de Documentos

        #region Buscar dados acompanhamento inscrito


        public List<AcompanhamentoInscritoListarVO> BuscarInscrito(InscricaoFilterSpecification spec, out int total)
        {
            spec.SetOrderByDescending(d => d.Processo.AnoReferencia)
                .SetOrderByDescending(d => d.Processo.SemestreReferencia)
                .SetOrderBy(o => o.Inscrito.Nome)
                .SetOrderBy(o => o.Processo.Descricao);
            var lista = this.SearchProjectionBySpecification(spec, a => new AcompanhamentoInscritoListarVO()
            {
                SeqInscrito = a.SeqInscrito,
                NomeInscrito = a.Inscrito.Nome,
                Cpf = a.Inscrito.Cpf,
                NumeroPassaporte = a.Inscrito.NumeroPassaporte,
                SeqProcesso = a.Processo.Seq,
                TipoProcesso = a.Processo.TipoProcesso.Descricao,
                DescricaoProcesso = a.Processo.Descricao,
                SeqInscricao = a.Seq,
                SituacaoInscricao = a.HistoricosSituacao.FirstOrDefault(f => f.Atual).TipoProcessoSituacao.Descricao,
                Semestre = a.Processo.SemestreReferencia,
                Ano = a.Processo.AnoReferencia,
                SeqSituacao = a.HistoricosSituacao.FirstOrDefault(h => h.Atual).SeqTipoProcessoSituacao,
                OpcoesOferta = a.Ofertas.Select(y => new OpcaoOfertaVO()
                {
                    SeqOferta = y.SeqOferta,
                    NumeroOpcao = y.NumeroOpcao,
                    DescricaoOferta = y.Oferta.DescricaoCompleta,
                    HierarquiaCompleta = y.Oferta.HierarquiaCompleta,
                    InscricaoOferta = y.Oferta.InscricoesOferta.Select(z => z.SeqInscricao).ToList(),
                    SituacaoOferta = y.HistoricosSituacao.FirstOrDefault(f => f.Atual).TipoProcessoSituacao.Descricao,
                    SeqProcesso = a.SeqProcesso,
                    SeqInscricao = a.Seq,
                    ExibirOpcoes = (a.HistoricosSituacao.FirstOrDefault(f => f.Atual).TipoProcessoSituacao.SeqSituacao != 2 && a.HistoricosSituacao.FirstOrDefault(f => f.Atual).TipoProcessoSituacao.SeqSituacao != 6),
                    SeqInscricaoOferta = y.Seq,
                    SeqInscrito = a.SeqInscrito,
                    NomeInscrito = a.Inscrito.Nome,

                    ExibirPeriodoAtividadeOferta = y.Oferta.Processo.ExibirPeriodoAtividadeOferta,
                    DataInicioAtividade = y.Oferta.DataInicioAtividade,
                    DataFimAtividade = y.Oferta.DataFimAtividade,
                    CargaHorariaAtividade = y.Oferta.CargaHorariaAtividade,
                    Descricao = y.Oferta.Nome
                }).OrderBy(o => o.NumeroOpcao).ThenBy(t => t.DescricaoOferta).ToList(),
            }, out total).ToList();

            foreach (var item in lista)
            {
                foreach (var oferta in item.OpcoesOferta)
                {

                    var of = new Oferta()
                    {
                        Nome = oferta.Descricao,
                        DescricaoCompleta = oferta.DescricaoOferta,
                        DataInicioAtividade = oferta.DataInicioAtividade,
                        DataFimAtividade = oferta.DataFimAtividade,
                        CargaHorariaAtividade = oferta.CargaHorariaAtividade,
                        Processo = new Processo()
                        {
                            ExibirPeriodoAtividadeOferta = oferta.ExibirPeriodoAtividadeOferta
                        }
                    };

                    OfertaDomainService.AdicionarDescricaoCompleta(of, of.Processo.ExibirPeriodoAtividadeOferta);

                    if (!string.IsNullOrEmpty(of.DescricaoCompleta))
                        oferta.DescricaoOferta = of.DescricaoCompleta;
                }
            }

            lista.SMCForEach(f =>
            {

                if (!string.IsNullOrEmpty(f.Cpf))
                {
                    f.Cpf = SMCMask.ApplyMaskCPF(f.Cpf.Trim());
                }
            });
            lista.SMCForEach(f => f.SemestreAno = string.Format("{0}/{1}", f.Semestre, f.Ano));
            lista.SMCForEach(f => f.OpcoesOferta.ForEach(x => x.NumeroOpcaoFormatado = string.Format("{0}ª", x.NumeroOpcao)));
            lista.SMCForEach(f =>
            {
                if (string.IsNullOrEmpty(f.Cpf) && !string.IsNullOrEmpty(f.NumeroPassaporte))
                {
                    f.CpfouPassaporte = f.NumeroPassaporte;
                }
                else
                {
                    f.CpfouPassaporte = f.Cpf;
                }
            });

            return lista.ToList();
        }

        #endregion

        /// <summary>
        /// Buscar dados Formulario Seminario
        /// </summary>
        /// <param name="seqAcao">Sequencial da Ação</param>
        /// <param name="seqProcesso">Sequencial Processo</param>
        /// <param name="seqIncricao">Sequencial da Inscrição</param>
        public DadosFormularioSeminarioVO DadosFormularioSeminarioSGF(long seqAcao, long seqProcesso, long seqIncricao)
        {
            DadosFormularioSeminarioVO retorno = new DadosFormularioSeminarioVO();
            var nomeInscrito = BuscarInscricaoResumida(seqIncricao).NomeInscrito;
            var areaConhecimento = RawQuery<DadosFormularioSeminarioVO>(string.Format(_query_area_conhecimento, seqProcesso, seqAcao)).FirstOrDefault().AreaConhecimento;
            var orientador = RawQuery<DadosFormularioSeminarioVO>(string.Format(_query_orientador, seqProcesso, seqAcao)).FirstOrDefault();
            var coordenador = RawQuery<DadosFormularioSeminarioVO>(string.Format(_query_coordenador, seqProcesso, seqAcao)).FirstOrDefault();
            var alunos = RawQuery<string>(string.Format(_query_alunos, seqProcesso, seqAcao)).ToList();

            if (alunos == null)
            {
                retorno.Alunos = new List<string>();
            }

            retorno.AreaConhecimento = areaConhecimento;
            // Para os projetos FAPEMIG, retornar os dados do orientador.
            // Para os projetos PIBIC / PIBIT, FIP / Projeto de Pesquisa, IC Voluntário,
            // retornar os dados do coordenador.
            // A query já filtra caso pelos projetos desta forma caso não exista coordenador este projeto é do tipo FAPEMIG
            if (coordenador != null)
            {
                var dados = PessoaService.BuscarEmailPadrao(coordenador.CodigoPessoa);
                retorno.EmailOrientador = dados.Descricao;
                retorno.NomeOrientador = coordenador.NomeOrientador;
            }
            else
            {
                retorno.EmailOrientador = orientador.EmailOrientador;
                retorno.NomeOrientador = orientador.NomeOrientador;
            }
            retorno.Alunos = alunos;
            retorno.Alunos = retorno.Alunos.OrderBy(o => o).ToList();

            return retorno;
        }

        /// <summary>
        /// Valida se por ventura o formulario é consistente para proseguir
        /// Incluir no botão “Próximo” a regra “RN_INS_187 - Consistência seminários de iniciação científica”:
        /// Se o tipo de processo em questão estiver configurado para integrar com o GPC 
        /// e existir o campo PROJETO no formulário, verificar se existe alguma outra 
        /// inscrição para o processo em questão, com a situação atual da inscrição 
        /// igual a INSCRICAO_FINALIZADA ou INSCRICAO_CONFIRMADA, e com o mesmo projeto 
        /// selecionado.Em caso afirmativo, abortar a operação e emitir a mensagem de erro:
        ///"Já existe uma inscrição para o projeto 'nome do projeto'".
        /// </summary>
        /// <param name="seqProcesso">Sequencial do proceesso no GPI</param>
        /// <param name="seqInscricao">Sequencial da Inscrição</param>
        /// <param name="descricaoProjeto">Descrição do Projeto GPC</param>
        public void ValidarFormularioSeminario(long seqProcesso, long seqInscricao, string descricaoProjeto)
        {
            var splitDescricaoProjeto = descricaoProjeto.Split('|');
            long seqProjeto = Convert.ToInt64(splitDescricaoProjeto[0]);
            var inscricoes = RawQuery<string>(string.Format(_query_validar_projeto_seminario, seqProjeto, seqProcesso, seqInscricao)).ToList();

            if (inscricoes.Count > 0)
            {
                throw new InscricaoSeminarioInvalidaException(splitDescricaoProjeto[1]);
            }
        }

        /// <summary>
        /// Valida consitencia Inscrito Seminario
        /// </summary>
        /// <param name="seqConfiguracaoEtapa">Sequencial configuração etapa</param>
        /// <param name="seqInscrito">Sequencial inscrito</param>
        public void ValidarConsistenciaInscritoSeminario(long seqConfiguracaoEtapa, long seqInscrito)
        {
            var integraGPC = ConfiguracaoEtapaDomainService.SearchProjectionByKey(seqConfiguracaoEtapa,
                                                                p => p.EtapaProcesso.Processo.TipoProcesso.IntegraGPC);
            //RN_INS_188 - Consistência do inscrito nos seminários de iniciação científica:
            //Se o tipo e processo em questão estiver configurado para integrar com o GPC e o 
            //inscrito possuir contrato de professor na PUC, abortar a operação e emitir a mensagem de erro:
            //"Somente alunos da Iniciação científica podem fazer inscrição."
            //conferir se TipoEmpregado = "P" (Professor) e se o CodigoEstabelecimento = "002" (PUC Minas)
            if (integraGPC)
            {
                var inscrito = InscritoDomainService.SearchProjectionByKey(seqInscrito, x => new { x.Cpf });

                var dadosContrato = PessoaService.BuscarContratosFuncionarioPorCPF(inscrito.Cpf);

                foreach (var contrato in dadosContrato)
                {
                    if (contrato.CodigoEstabelecimento == "002" && contrato.TipoEmpregado == "P")
                    {
                        throw new InscricaoInicacaoCientificaException();
                    }
                }
            }
        }

        public ObservacaoInscricaoVO BuscarObservacaoInscricao(long seqInscricao)
        {
            var retorno = this.SearchProjectionByKey(new SMCSeqSpecification<Inscricao>(seqInscricao), x => new ObservacaoInscricaoVO
            {
                Seq = x.Seq,
                Nome = x.Inscrito.Nome,
                SeqProcesso = x.SeqProcesso,
                Observacao = x.Observacao
            });

            return retorno;


        }

        /// <summary>
        /// Buscar situação atual da inscrição
        /// </summary>
        /// <param name="seqInscricao">Sequencial da inscricao</param>
        /// <returns>Token de segurança</returns>
        public string BuscarSituacaoAtualInscricao(long seqInscricao)
        {
            string situacaoAtual = this.SearchProjectionByKey(seqInscricao, p => p.HistoricosSituacao.FirstOrDefault(f => f.Atual).TipoProcessoSituacao.Token);

            return situacaoAtual;
        }

        /// <summary>
        /// Especificar qual será o botão continuar inscrição
        /// </summary>
        /// <param name="lista">Lista com informações para gerar o botão</param>
        private void EspecificarBotaoContinuar(List<InscricoesProcessoVO> lista)
        {
            foreach (var processo in lista)
            {
                foreach (var inscricao in processo.Inscricoes)
                {
                    var resultValidarBotao = VerificarPermissaoIniciarContinuarInscricaoBotoes(inscricao.SeqInscrito,
                                                                                                inscricao.SeqConfiguracaoEtapa,
                                                                                                inscricao.SeqGrupoOferta,
                                                                                                inscricao.SeqInscricao);
                    inscricao.HabilitarBotaoContinuar = resultValidarBotao.habilitarBotao;
                    inscricao.MensagemBotaoContinuar = resultValidarBotao.mensagem;
                    inscricao.BotaoContiuar = $"Continuarinscricao{processo.TokenResource.ToLower()}";
                    inscricao.BotaoContiuarTootip = $"Continuarinscricao{processo.TokenResource.ToLower()}Tooltip";
                }
            }
        }

        /// <summary>
        /// Excluir os arquivos no GED
        /// </summary>
        /// <param name="seqIncricao">Sequencial da inscrição</param>
        /// <param name="documentos">Lista dos documentos</param>
        private void ExcluirArquivoGED(long seqIncricao, List<string> documentos)
        {
            ParametrosArquivoVO modelo = new ParametrosArquivoVO();
            modelo.SeqInscricao = seqIncricao;
            modelo.SeqsGuidArquivo = documentos;

            ArquivoApiDoaminService.ExcluirArquivo(modelo);
        }
        public string BuscarNomeInscritosSeqInscricao(long seqInscricao)
        {
            string retorno = string.Empty;

            var inscricao = this.SearchProjectionByKey(new SMCSeqSpecification<Inscricao>(seqInscricao), x => new { Nome = x.Inscrito.Nome });

            if (inscricao != null)
            {
                retorno = inscricao.Nome;
            }

            return retorno;
        }

        public string BuscarDescricaoProcessoInscricao(long seqInscricao)
        {
            string retorno = string.Empty;

            var inscricao = this.SearchProjectionByKey(new SMCSeqSpecification<Inscricao>(seqInscricao), x => new { DescricaoProcesso = x.Processo.Descricao });

            if (inscricao != null)
            {
                retorno = inscricao.DescricaoProcesso;
            }

            return retorno;
        }

        public byte[] EmitirDocumentacao(long seqInscricao, long seqTipoDocumento)
        {
            long seqProcesso = ProcessoDomainService.BuscarSeqProcessoPorSeqInscricao(seqInscricao);
            long? seqUsuarioLogado = User.SMCGetSequencialUsuario();
            long? seqInscrito = InscritoDomainService.BuscarSeqInscrito(seqUsuarioLogado.GetValueOrDefault());
            
            bool permissaoEmitirDocumentacao = ProcessoDomainService.ValidarPermissaoEmitirDocumentacao(seqProcesso, seqInscricao, seqInscrito.Value, seqTipoDocumento);
            if (!permissaoEmitirDocumentacao)
            {
                throw new PermissaoEmitirDocumentacaoException();
            }
            var inscricao = RawQuery<DadosDocumentacaoInscricaoVO>(string.Format(_query_emitir_documentacao_incricao, seqInscricao, seqTipoDocumento, seqInscricao)).FirstOrDefault();
            if (inscricao != null)
            {
                inscricao.DataAtual = RemoverHorario(DateTime.Now.SMCDataHoraPorExtenso());
                inscricao.DataInicioEvento = RemoverHorario(inscricao.DataInicioEvento);
                inscricao.DataFimEvento = RemoverHorario(inscricao.DataFimEvento);
                var situacoesTemplateProcesso = TemplateProcessoService.BuscarSituacoesPorTemplateProcesso(inscricao.SeqTemplateProcessoSGF);
                inscricao.ProcessoPossuiDeferimento = situacoesTemplateProcesso.Contains(TOKENS.SITUACAO_INSCRICAO_DEFERIDA);
            }

            CalcularCargaHorariaTotal(inscricao);
            ValidarInscricaoDeferida(inscricao);

            var conteudoArquivoModelo = ConfiguracaoModeloDocumentoDomainService.SearchProjectionByKey(inscricao.SeqConfiguracaoModeloDocumento, p => new
            {
                p.ArquivoModelo.Conteudo,
                p.ArquivoModelo.Nome,
                p.ArquivoModelo.Tipo
            });

            var listaCampos = SautinsoftHelper.FindFieldsMerge(conteudoArquivoModelo.Conteudo);
            var camposMerge = CriarObjetoDinamico(inscricao, listaCampos);

            string jsonCamposMerge = JsonConvert.SerializeObject(camposMerge);
            var arquivo = SautinsoftHelper.MailMergeToPdf(conteudoArquivoModelo.Conteudo, conteudoArquivoModelo.Nome, "dotx", jsonCamposMerge);
            return arquivo;
        }

        /// <summary>
        /// Cria um objeto dinâmico (ExpandoObject) a partir de um objeto de origem,
        /// contendo apenas as propriedades especificadas por uma string.
        /// </summary>
        /// <param name="objetoFonte">O objeto que contém todos os dados.</param>
        /// <param name="listaDePropriedades">Uma lista string com nomes de propriedades.</param>
        /// <returns>Um ExpandoObject (tratado como dynamic) com as propriedades e valores solicitados.</returns>
        private dynamic CriarObjetoDinamico(object objetoFonte, List<string> listaDePropriedades)
        {
            // Cria o objeto dinâmico que será preenchido e retornado.
            IDictionary<string, object> objetoDinamico = new ExpandoObject();

            // Itera sobre cada nome de propriedade que queremos extrair.
            foreach (var nomePropriedade in listaDePropriedades)
            {
                // Usa Reflection para encontrar a propriedade no objeto de origem.
                // As 'BindingFlags' são essenciais aqui:
                // - IgnoreCase: Permite encontrar "NOME_INSCRITO" mesmo que a busca seja por "nome_inscrito".
                // - Public | Instance: Especifica que queremos buscar propriedades públicas na instância do objeto.
                PropertyInfo informacaoDaPropriedade = objetoFonte.GetType()
                    .GetProperty(nomePropriedade, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                // Verifica se a propriedade foi realmente encontrada no objeto de origem.
                if (informacaoDaPropriedade != null)
                {
                    // Se encontrou, obtém o valor da propriedade a partir da instância do objetoFonte.
                    object valor = informacaoDaPropriedade.GetValue(objetoFonte, null);

                    // Adiciona a propriedade e seu valor ao nosso objeto dinâmico.
                    // Usamos o nome original da propriedade (com a capitalização correta) como chave.
                    objetoDinamico[informacaoDaPropriedade.Name] = valor;
                }
            }

            return objetoDinamico;
        }

        private string RemoverHorario(string dataComHora)
        {
            // Primeiro, removemos espaços em branco no início ou fim para evitar erros
            dataComHora = dataComHora.Trim();

            int indiceDoUltimoEspaco = dataComHora.LastIndexOf(' ');

            // Se encontrarmos um espaço, pegamos a substring. Senão, retornamos a string original.
            if (indiceDoUltimoEspaco > -1)
            {
                return dataComHora.Substring(0, indiceDoUltimoEspaco);
            }

            return dataComHora; // Retorna a string como está se não houver espaços
        }

        private void CalcularCargaHorariaTotal(DadosDocumentacaoInscricaoVO inscricao)
        {
            var ofertasIncricao = InscricaoOfertaDomainService.BuscarOfertasInscrito(new InscricaoOfertaFilterSpecification() { SeqInscricao = inscricao.SeqInscricao });
            int cargaHorariaTotal = 0;
            if (inscricao.DocumentoRequerCheckin)
            {
                foreach (var oferta in ofertasIncricao)
                {
                    if (oferta.DataCheckin.HasValue)
                    {
                        cargaHorariaTotal += oferta.Oferta.CargaHorariaAtividade ?? 0;
                    }
                }
            }
            else
            {
                foreach (var oferta in ofertasIncricao)
                {
                        cargaHorariaTotal += oferta.Oferta.CargaHorariaAtividade ?? 0;
                }
            }

            inscricao.CargaHorariaTotal = $"{cargaHorariaTotal} hora(s)";
        }

        private void ValidarInscricaoDeferida(DadosDocumentacaoInscricaoVO inscricao)
        {
            if (inscricao.ProcessoPossuiDeferimento)
            {
                var dadosInscricao = this.SearchProjectionByKey(inscricao.SeqInscricao, p => new
                {
                    p.SeqInscrito,
                    p.HistoricosSituacao.FirstOrDefault(h => h.Atual).TipoProcessoSituacao.Token,
                    p.HistoricosSituacao.FirstOrDefault(h => h.Atual).TipoProcessoSituacao.Descricao,
                    DescricaoTipoProcesso = p.Processo.TipoProcesso.Descricao
                });

                if(dadosInscricao.Token != TOKENS.SITUACAO_INSCRICAO_DEFERIDA)
                {
                    throw new InscricaoNaoDeferidaException(dadosInscricao.Descricao, dadosInscricao.DescricaoTipoProcesso, inscricao.NomeInscrito);
                }
            }
        }

        #region Ingressos

        /// <summary>
        /// Buscar ingressos
        /// </summary>
        /// <param name="seqInscricao">Sequencial da Inscricao</param>
        /// <returns>Retorna os ingresso da Inscricao</returns>
        public IngressoVO BuscarIngressos(long seqInscricao)
        {
            IngressoVO retorno = this.SearchProjectionByKey(seqInscricao, x => new IngressoVO
            {
                SeqInscricao = x.Seq,
                SeqProcesso = x.SeqProcesso,
                DescricaoProcesso = x.Processo.Descricao,
                ExibirPeriodoAtividadeOferta = x.Processo.ExibirPeriodoAtividadeOferta,
                TokenResource = x.Processo.TipoProcesso.TokenResource,
                UidProcesso = x.Processo.UidProcesso,
                SeqArquivoComprovante = x.SeqArquivoComprovante,
                NomeInscrito = x.Inscrito.Nome,
                QuantidadeHorasAberturaCheckin = x.Processo.HoraAberturaCheckin,
                Ofertas = x.Ofertas.Select(s => new IngressoOfertaVO
                {
                    SeqInscricaoOferta = s.Seq,
                    UidInscricaoOferta = s.UidInscricaoOferta,
                    HabilitaCheckin = s.Oferta.HabilitaCheckin,
                    DescricaoOferta = s.Oferta.DescricaoCompleta,
                    SeqOferta = s.SeqOferta,
                    DataInicioAtividade = s.Oferta.DataInicioAtividade,
                    DataFimAtividade = s.Oferta.DataFimAtividade,
                }).ToList(),
                TokenTipoProcesso = x.Processo.TipoProcesso.Token
            });

            DefineCssIngresso(retorno);

            retorno.TituloInscricoes = $"Titulo_Inscricoes_{retorno.TokenResource.ToLower().SMCToPascalCase()}";
            // Buscar a situação atual da inscrição para preencher os campos
            InscricaoHistoricoSituacaoFilterSpecification specHistorico = new InscricaoHistoricoSituacaoFilterSpecification
            {
                SeqInscricao = seqInscricao,
                Atual = true
            };
            var situacaoAtual = InscricaoHistoricoSituacaoDomainService.SearchByKey(specHistorico, IncludesInscricaoHistoricoSituacao.TipoProcessoSituacao);
            retorno.TokenSituacaoAtual = situacaoAtual.TipoProcessoSituacao.Token;

            PrepararDescricaoTagIngresso(retorno);
            return retorno;
        }

        /// <summary>
        /// Controla o css que vai ser usado no ingresso
        /// </summary>
        /// <param name="ingresso"></param>
        private void DefineCssIngresso(IngressoVO ingresso)
        {
            var cssUrl = ProcessoDomainService.BuscarCssProcesso(ingresso.UidProcesso);

            // Substitui o CSS de inscrição pelo CSS do ingresso, se necessário
            if (cssUrl.Contains(CSS_PROCESSO.CSS_PROCESSO_INSCRICAO))
            {
                cssUrl = cssUrl.Replace(CSS_PROCESSO.CSS_PROCESSO_INSCRICAO, CSS_PROCESSO.CSS_PDF_INGRESSO);
            }

            // Garante que o CSS do ingresso esteja presente na URL
            if (!cssUrl.Contains(CSS_PROCESSO.CSS_PDF_INGRESSO))
            {
                cssUrl += cssUrl[cssUrl.Length - 1] == '/' ? CSS_PROCESSO.CSS_PDF_INGRESSO : "/" + CSS_PROCESSO.CSS_PDF_INGRESSO;

            }

            ingresso.CssUrl = cssUrl;

            // Ajusta o caminho para uso no sistema de arquivos
            var cssFisico = cssUrl.Replace("/", "\\");
            ingresso.CssFisico = $"E:\\Aplicativos\\Recursos\\Inscricoes\\4.0\\GPI.Inscricao\\{cssFisico}";

            //if (ingresso.TokenTipoProcesso == "FEIRA_DE_CARREIRAS")
            //{
            //    ingresso.CssUrl = "PUCCarreiras2024/Css/smc-pdf-ingressos-puccarreiras2024.min.css";
            //    ingresso.CssFisico += "\\PUCCarreiras2024\\Css\\smc-pdf-ingressos-puccarreiras2024.min.css";
            //}
            //else
            //{
            //    ingresso.CssUrl = "PUCAberta2025/Css/smc-pdf-ingressos-pucaberta2025.min.css";
            //    ingresso.CssFisico += "\\PUCAberta2025\\Css\\smc-pdf-ingressos-pucaberta2025.min.css";
            //}
        }

        private void PrepararDescricaoTagIngresso(IngressoVO ingresso)
        {
            foreach (var item in ingresso.Ofertas)
            {
                if (item.HabilitaCheckin)
                {
                    var descOfertaSplit = item.DescricaoOferta.Split(new string[] { "=>" }, StringSplitOptions.RemoveEmptyEntries);
                    if (descOfertaSplit.Length == 1)
                    {
                        item.DescicaoParte1 = descOfertaSplit[0];
                    }
                    else if (descOfertaSplit.Length == 2)
                    {
                        item.DescicaoParte1 = descOfertaSplit[0];
                        item.DescicaoParte2 = descOfertaSplit[1];
                    }
                    else if (descOfertaSplit.Length == 3)
                    {
                        item.DescicaoParte1 = descOfertaSplit[0];
                        item.DescicaoParte2 = descOfertaSplit[1];
                        item.DescicaoParte3 = descOfertaSplit[2];
                    }
                    else
                    {
                        for (int i = 0; i < descOfertaSplit.Length; i++)
                        {
                            if (i == 0)
                            {
                                item.DescicaoParte1 = descOfertaSplit[i];
                            }
                            else if (i == 1)
                            {
                                item.DescicaoParte2 = descOfertaSplit[i];
                            }
                            else
                            {
                                item.DescicaoParte3 += descOfertaSplit[i] + " => ";
                            }
                            item.DescicaoParte3.Remove(item.DescicaoParte3.Length - 4);
                        }
                    }

                    var diasIncioAtivida = item.DataInicioAtividade.Value.Date - DateTime.Now.Date;

                    if (diasIncioAtivida.Days > 0)
                    {
                        var pluralDias = diasIncioAtivida.Days == 1 ? "" : "s";
                        var pluralFaltar = diasIncioAtivida.Days == 1 ? "" : "m";
                        item.TagAtividade = $"Falta{pluralFaltar} {diasIncioAtivida.Days} dia{pluralDias}";
                        item.CSSTagAtividade = "smc-gpi-tag-atividade-falta";
                    }
                    else if (diasIncioAtivida.Days == 0)
                    {
                        item.TagAtividade = "Hoje";
                        item.CSSTagAtividade = "smc-gpi-tag-atividade-andamento";
                    }
                    else
                    {
                        item.TagAtividade = "Atividade encerrada";
                        item.CSSTagAtividade = "smc-gpi-tag-atividade-encerrada";
                    }

                    item.QrCodeOferta = SMCQrCode.ConvertToWeb(SMCQrCode.CreateQrCode(item.UidInscricaoOferta.ToString()), ImageFormat.Jpeg);

                }
            }
        }

        #endregion
    }
}
