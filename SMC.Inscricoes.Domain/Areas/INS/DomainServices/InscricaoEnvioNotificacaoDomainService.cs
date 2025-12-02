using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework;
using SMC.Framework.Domain;
using SMC.Framework.Specification;
using SMC.Inscricoes.Common;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Inscricoes.Common.Areas.INS.Exceptions;
using SMC.Inscricoes.Common.Areas.RES;
using SMC.Inscricoes.Extensions;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using SMC.Inscricoes.Domain.Areas.RES.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Notificacoes.ServiceContract.Areas.NTF.Interfaces;
using SMC.Inscricoes.Domain.Areas.RES.Models;
using SMC.Localidades.Common.Areas.LOC.Enums;
using SMC.Notificacoes.ServiceContract.Areas.NTF.Data;
using SMC.Notificacoes.Common.Areas.NTF.Enums;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using SMC.Inscricoes.Domain.Models;
using System.Runtime.InteropServices;
using SMC.Inscricoes.Domain.Areas.INS.ValueObjects;

namespace SMC.Inscricoes.Domain.Areas.INS.DomainServices
{
    public class InscricaoEnvioNotificacaoDomainService : InscricaoContextDomain<InscricaoEnvioNotificacao>
    {
        #region DomainService
        private InscricaoDomainService InscricaoDomainService => Create<InscricaoDomainService>();
        private ConfiguracaoEtapaDomainService ConfiguracaoEtapaDomainService
        {
            get { return this.Create<ConfiguracaoEtapaDomainService>(); }
        }

        private InscritoDomainService InscritoDomainService
        {
            get { return this.Create<InscritoDomainService>(); }
        }

        private UnidadeResponsavelDomainService UnidadeResponsavelDomainService
        {
            get { return this.Create<UnidadeResponsavelDomainService>(); }
        }

        private OfertaDomainService OfertaDomainService
        {
            get { return this.Create<OfertaDomainService>(); }
        }

        private InscricaoBoletoTituloDomainService InscricaoBoletoTituloDomainService
        {
            get { return this.Create<InscricaoBoletoTituloDomainService>(); }
        }

        private ProcessoConfiguracaoNotificacaoIdiomaDomainService ProcessoConfiguracaoNotificacaoIdiomaDomainService
        {
            get { return this.Create<ProcessoConfiguracaoNotificacaoIdiomaDomainService>(); }
        }
        
        private ProcessoDomainService ProcessoDomainService
        {
            get { return this.Create<ProcessoDomainService>(); }
        }

        
        #endregion

        #region Services

        private INotificacaoService NotificacaoService
        {
            get { return this.Create<INotificacaoService>(); }
        }

        #endregion

        #region Notificação NOTIFICACAO_FINALIZAR_INSCRICAO

        /// <summary>
        /// Envia a notificação de Finalizar Inscrição
        /// </summary>
        /// <param name="inscricao">Inscrição sendo finalizada</param>
        public void EnviarNotificacaoFinalizarInscricao(long seqInscricao,
                long seqProcesso,
                long seqInscrito,
                long seqConfiguracaoEtapa,
                string descricaoProcesso,
                SMCLanguage idioma,
                string tokenSituacao)
        {
            // Busca a configuração de notificação para finalizar a inscrição no idioma da mesma
            var specNotificacao = new ProcessoConfiguracaoNotificacaoIdiomaFilterSpecification()
            {
                SeqProcesso = seqProcesso,
                Idioma = idioma,
                TokenTipoNotificacao = (tokenSituacao == TOKENS.SITUACAO_INSCRICAO_FINALIZADA) ?
                                            TOKENS.NOTIFICACAO_FINALIZAR_INSCRICAO :
                                            TOKENS.NOTIFICACAO_CONFIRMAR_INSCRICAO
            };
            var configNotificacaoIdioma = ProcessoConfiguracaoNotificacaoIdiomaDomainService.SearchByKey(specNotificacao, IncludesProcessoConfiguracaoNotificacaoIdioma.ProcessoConfiguracaoNotificacao);

            // Se não tem notificação para enviar, ou a configuração não está configurada com envio automático, retorna
            if (configNotificacaoIdioma == null || !configNotificacaoIdioma.ProcessoConfiguracaoNotificacao.EnvioAutomatico)
                return;

            // Busca as informações para mesclagem de dados
            Dictionary<string, string> dados = new Dictionary<string, string>();

            // Busca os dados do inscrito
            var specInscrito = new SMCSeqSpecification<Inscrito>(seqInscrito);
            var inscrito = InscritoDomainService.SearchByKey(specInscrito);

            // Busca as informações da configuração de etapa
            var specConfig = new SMCSeqSpecification<ConfiguracaoEtapa>(seqConfiguracaoEtapa);
            var includesConfig = IncludesConfiguracaoEtapa.EtapaProcesso |
                                 IncludesConfiguracaoEtapa.EtapaProcesso_Processo;
            var configEtapa = ConfiguracaoEtapaDomainService.SearchByKey(specConfig, includesConfig);

            // Busca as informações da unidade responsável pelo processo
            var specUnidade = new SMCSeqSpecification<UnidadeResponsavel>(configEtapa.EtapaProcesso.Processo.SeqUnidadeResponsavel);
            var includesUnidade = IncludesUnidadeResponsavel.Enderecos |
                                  IncludesUnidadeResponsavel.EnderecosEletronicos |
                                  IncludesUnidadeResponsavel.Telefones;
            var unidade = UnidadeResponsavelDomainService.SearchByKey(specUnidade, includesUnidade);

            // Busca as dados do titulo
            var specTitulo = new InscricaoBoletoTituloFilterSpecification()
            {
                SeqInscricao = seqInscricao,
                TipoBoleto = TipoBoleto.Inscricao
            };
            var titulo = InscricaoBoletoTituloDomainService.SearchByKey(specTitulo);

            // Busca as informações para merge
            dados.Add(TAGS_NOTIFICACAO.NOME_INSCRITO, inscrito.Nome.Trim());
            dados.Add(TAGS_NOTIFICACAO.NOME_SOCIAL_INSCRITO, !string.IsNullOrEmpty(inscrito.NomeSocial) ? inscrito.NomeSocial.Trim() : inscrito.Nome.Trim());
            dados.Add(TAGS_NOTIFICACAO.NUMERO_INSCRICAO, seqInscricao.ToString());
            dados.Add(TAGS_NOTIFICACAO.DSC_PROCESSO, descricaoProcesso);

            // Busca as Ofertas
            var ofertas = InscricaoDomainService.SearchProjectionByKey(seqInscricao, x => x.Ofertas.Select(o => new
            {
                o.NumeroOpcao,
                o.SeqOferta
            })).ToList();

            // Oferta
            if (ofertas != null && ofertas.Count > 0)
            {
                string dscOfertas = string.Empty;
                bool virgula = false;
                foreach (var oferta in ofertas.OrderBy(o => o.NumeroOpcao))
                {
                    if (virgula)
                        dscOfertas += ", ";
                    dscOfertas += OfertaDomainService.BuscarHierarquiaOfertaCompleta(oferta.SeqOferta, false).DescricaoCompleta;
                    virgula = true;
                }
                dados.Add(TAGS_NOTIFICACAO.DESCRICAO_OFERTAS, dscOfertas);
            }

            // Unidade Responsavel
            dados.Add(TAGS_NOTIFICACAO.NOME_UNIDADE_RESPONSAVEL, unidade.Nome.Trim());
            Endereco endComercial = unidade.Enderecos.Where(e => e.TipoEndereco == TipoEndereco.Comercial).FirstOrDefault();
            if (endComercial != null)
                dados.Add(TAGS_NOTIFICACAO.ENDERECO_UNIDADE_RESPONSAVEL, endComercial.FormatarParaImpressao());

            string telefones = "";
            foreach (var tel in unidade.Telefones)
            {
                telefones += tel.FormatarParaImpressao() + ", ";
            }
            telefones = telefones.Substring(0, telefones.Length - 2);
            //Telefone telComercial = unidade.Telefones.Where(t => t.TipoTelefone == TipoTelefone.Comercial).FirstOrDefault();
            if (!String.IsNullOrEmpty(telefones))
                dados.Add(TAGS_NOTIFICACAO.TELEFONE_UNIDADE_RESPONSAVEL, telefones);

            EnderecoEletronico email = unidade.EnderecosEletronicos.Where(e => e.TipoEnderecoEletronico == TipoEnderecoEletronico.Email).FirstOrDefault();
            if (email != null)
                dados.Add(TAGS_NOTIFICACAO.EMAIL_UNIDADE_RESPONSAVEL, email.Descricao.Trim());

            // Documentação
            if (configEtapa.DataLimiteEntregaDocumentacao.HasValue)
                dados.Add(TAGS_NOTIFICACAO.DATA_ENTREGA_DOCUMENTACAO, string.Format("{0:dd/MM/yyyy HH:mm}", configEtapa.DataLimiteEntregaDocumentacao));
            if (!string.IsNullOrEmpty(configEtapa.DescricaoEntregaDocumentacao))
                dados.Add(TAGS_NOTIFICACAO.DESCRICAO_DOCUMENTACAO, configEtapa.DescricaoEntregaDocumentacao.Trim());

            // Titulo
            if (titulo != null)
                dados.Add(TAGS_NOTIFICACAO.DATA_VENCIMENTO_TITULO, string.Format("{0:dd/MM/yyyy}", titulo.DataVencimento));

            // Envia a notificação para o email do inscrito
            var destinatario = new NotificacaoEmailDestinatarioData() { EmailDestinatario = inscrito.Email };

            var seqLayouMensagemEmail = ProcessoDomainService.BuscarSeqLayouMensagemEmailPorProcesso(seqProcesso);

            // Chama a rotina para salvar a notificação
            this.SalvarInscricaoEnvioNotificacao(seqInscricao, configNotificacaoIdioma, dados, destinatario, seqLayouMensagemEmail);
        }

        public void EnviarNotificacaoLiberarAlteracaoInscricao(Inscricao inscricao)
        {
            // Busca a configuração de notificação para finalizar a inscrição no idioma da mesma
            var specNotificacao = new ProcessoConfiguracaoNotificacaoIdiomaFilterSpecification()
            {
                SeqProcesso = inscricao.SeqProcesso,
                Idioma = inscricao.Idioma,
                TokenTipoNotificacao = TOKENS.NOTIFICACAO_LIBERACAO_ALTERACAO_INSCRICAO
            };
            var configNotificacaoIdioma = ProcessoConfiguracaoNotificacaoIdiomaDomainService.SearchByKey(specNotificacao, IncludesProcessoConfiguracaoNotificacaoIdioma.ProcessoConfiguracaoNotificacao);

            // Se não tem notificação para enviar, ou a configuração não está configurada com envio automático, retorna
            if (configNotificacaoIdioma == null || !configNotificacaoIdioma.ProcessoConfiguracaoNotificacao.EnvioAutomatico)
                return;

            // Busca as informações para mesclagem de dados
            Dictionary<string, string> dados = new Dictionary<string, string>();

            // Busca os dados do inscrito
            var specInscrito = new SMCSeqSpecification<Inscrito>(inscricao.SeqInscrito);
            var inscrito = InscritoDomainService.SearchByKey(specInscrito);

            // Busca as informações da configuração de etapa
            var specConfig = new SMCSeqSpecification<ConfiguracaoEtapa>(inscricao.SeqConfiguracaoEtapa);
            var includesConfig = IncludesConfiguracaoEtapa.EtapaProcesso |
                                 IncludesConfiguracaoEtapa.EtapaProcesso_Processo;
            var configEtapa = ConfiguracaoEtapaDomainService.SearchByKey(specConfig, includesConfig);

            // Busca as informações da unidade responsável pelo processo
            var specUnidade = new SMCSeqSpecification<UnidadeResponsavel>(configEtapa.EtapaProcesso.Processo.SeqUnidadeResponsavel);
            var includesUnidade = IncludesUnidadeResponsavel.Enderecos |
                                  IncludesUnidadeResponsavel.EnderecosEletronicos |
                                  IncludesUnidadeResponsavel.Telefones;
            var unidade = UnidadeResponsavelDomainService.SearchByKey(specUnidade, includesUnidade);

            // Busca as informações para merge
            dados.Add(TAGS_NOTIFICACAO.NOME_INSCRITO, inscrito.Nome.Trim());
            dados.Add(TAGS_NOTIFICACAO.NOME_SOCIAL_INSCRITO, !string.IsNullOrEmpty(inscrito.NomeSocial) ? inscrito.NomeSocial.Trim() : inscrito.Nome.Trim());
            dados.Add(TAGS_NOTIFICACAO.NUMERO_INSCRICAO, inscricao.Seq.ToString());
            dados.Add(TAGS_NOTIFICACAO.DSC_PROCESSO, inscricao.Processo.Descricao);
            dados.Add(TAGS_NOTIFICACAO.DATA_FIM_ETAPA, configEtapa.DataFim.ToShortDateString());

            // Unidade Responsavel
            dados.Add(TAGS_NOTIFICACAO.NOME_UNIDADE_RESPONSAVEL, unidade.Nome.Trim());
            Endereco endComercial = unidade.Enderecos.Where(e => e.TipoEndereco == TipoEndereco.Comercial).FirstOrDefault();
            if (endComercial != null)
                dados.Add(TAGS_NOTIFICACAO.ENDERECO_UNIDADE_RESPONSAVEL, endComercial.FormatarParaImpressao());

            string telefones = "";
            foreach (var tel in unidade.Telefones)
            {
                telefones += tel.FormatarParaImpressao() + ", ";
            }
            telefones = telefones.Substring(0, telefones.Length - 2);
            if (!String.IsNullOrEmpty(telefones))
                dados.Add(TAGS_NOTIFICACAO.TELEFONE_UNIDADE_RESPONSAVEL, telefones);

            EnderecoEletronico email = unidade.EnderecosEletronicos.Where(e => e.TipoEnderecoEletronico == TipoEnderecoEletronico.Email).FirstOrDefault();
            if (email != null)
                dados.Add(TAGS_NOTIFICACAO.EMAIL_UNIDADE_RESPONSAVEL, email.Descricao.Trim());

            // Envia a notificação para o email do inscrito
            var destinatario = new NotificacaoEmailDestinatarioData() { EmailDestinatario = inscrito.Email };

            var seqLayouMensagemEmail = ProcessoDomainService.BuscarSeqLayouMensagemEmailPorProcesso(inscricao.SeqProcesso);
            // Chama a rotina para salvar a notificação
            this.SalvarInscricaoEnvioNotificacao(inscricao.Seq, configNotificacaoIdioma, dados, destinatario, seqLayouMensagemEmail);
        }

        #endregion

        #region Enviar notificação recebimento bolsa

        public void EnviarNotificacaoRecebimentoBolsa(long seqInscricao, long seqOferta)
        {
            var dadosInscricao = InscricaoDomainService.SearchProjectionByKey(seqInscricao, p => new
            {
                p.Seq,
                NomeInscrito = p.Inscrito.Nome,
                NomeSocialInscrito = p.Inscrito.NomeSocial,
                EmailInscrito = p.Inscrito.Email,
                DescricaoProcesso = p.Processo.Descricao,
                p.SeqProcesso,
                p.Idioma,
                p.Ofertas.FirstOrDefault(f => f.SeqOferta == seqOferta).Oferta,
                p.Processo.ExibirPeriodoAtividadeOferta,
                DescricaoCompletaOferta = p.Ofertas.FirstOrDefault(f => f.SeqOferta == seqOferta).Oferta.DescricaoCompleta,
                LimitePercentualDescontoOferta = p.Ofertas.FirstOrDefault().Oferta.LimitePercentualDesconto,
                NomeUnidadeResponsavel = p.Processo.UnidadeResponsavel.Nome,
                TelefonesUnidadeResponsavel = p.Processo.UnidadeResponsavel.Telefones,
                EmailsUnidadeResponsavel = p.Processo.UnidadeResponsavel.EnderecosEletronicos
                    .Where(w => w.TipoEnderecoEletronico == TipoEnderecoEletronico.Email)
                    .Select(s => s.Descricao),
                EnderecosUnidadeResponsavel = p.Processo.UnidadeResponsavel.Enderecos.FirstOrDefault(f => f.TipoEndereco == TipoEndereco.Comercial)
            });

            Oferta oferta = dadosInscricao.Oferta;
            OfertaDomainService.AdicionarDescricaoCompleta(oferta, dadosInscricao.ExibirPeriodoAtividadeOferta, false);


            // Busca a configuração de notificação para finalizar a inscrição no idioma da mesma
            var specNotificacao = new ProcessoConfiguracaoNotificacaoIdiomaFilterSpecification()
            {
                SeqProcesso = dadosInscricao.SeqProcesso,
                Idioma = dadosInscricao.Idioma,
                TokenTipoNotificacao = TOKENS.RECEBIMENTO_BOLSA
            };
            var configNotificacaoIdioma = ProcessoConfiguracaoNotificacaoIdiomaDomainService.SearchByKey(specNotificacao, IncludesProcessoConfiguracaoNotificacaoIdioma.ProcessoConfiguracaoNotificacao);

            // Se não tem notificação para enviar, ou a configuração não está configurada com envio automático, retorna
            if (configNotificacaoIdioma == null || !configNotificacaoIdioma.ProcessoConfiguracaoNotificacao.EnvioAutomatico)
                return;

            // Busca as informações para mesclagem de dados
            var dados = new Dictionary<string, string>
            {
                {
                    TAGS_NOTIFICACAO.NOME_SOCIAL_INSCRITO,
                    string.IsNullOrEmpty(dadosInscricao.NomeSocialInscrito) ?
                        dadosInscricao.NomeInscrito :
                        dadosInscricao.NomeSocialInscrito
                },
                { TAGS_NOTIFICACAO.DSC_PROCESSO, dadosInscricao.DescricaoProcesso },
                { TAGS_NOTIFICACAO.DESCRICAO_OFERTAS, oferta.DescricaoCompleta },
                {
                    TAGS_NOTIFICACAO.VALOR_PERCENTUAL_DESCONTO,
                    (dadosInscricao.LimitePercentualDescontoOferta.GetValueOrDefault() / 100).ToString("P")
                },
                { TAGS_NOTIFICACAO.NOME_UNIDADE_RESPONSAVEL, dadosInscricao.NomeUnidadeResponsavel },
                {
                    TAGS_NOTIFICACAO.TELEFONE_UNIDADE_RESPONSAVEL,
                    string.Join(", ", dadosInscricao.TelefonesUnidadeResponsavel.Select(s => s.FormatarParaImpressao()))
                },
                { TAGS_NOTIFICACAO.EMAIL_UNIDADE_RESPONSAVEL, string.Join(", ", dadosInscricao.EmailsUnidadeResponsavel) },
                { TAGS_NOTIFICACAO.ENDERECO_UNIDADE_RESPONSAVEL, dadosInscricao.EnderecosUnidadeResponsavel?.FormatarParaImpressao() ?? "" }
            };

            // Envia a notificação para o email do inscrito
            var destinatario = new NotificacaoEmailDestinatarioData() { EmailDestinatario = dadosInscricao.EmailInscrito };

            var seqLayouMensagemEmail = ProcessoDomainService.BuscarSeqLayouMensagemEmailPorProcesso(dadosInscricao.SeqProcesso);

            // Chama a rotina para salvar a notificação
            SalvarInscricaoEnvioNotificacao(dadosInscricao.Seq, configNotificacaoIdioma, dados, destinatario, seqLayouMensagemEmail);
        }

        #endregion

        #region Salvar

        /// <summary>
        /// Envia a notificação e registra o seu envio.
        /// </summary>
        /// <param name="seqInscricao">Sequencial da inscrição</param>
        /// <param name="config">Configuração da notificação</param>
        /// <param name="dados">Dados para merge</param>
        /// <param name="destinatario">Destinatário para envio da notificação</param>
        public void SalvarInscricaoEnvioNotificacao(long seqInscricao, ProcessoConfiguracaoNotificacaoIdioma config,
            Dictionary<string, string> dados, NotificacaoEmailDestinatarioData destinatario, long seqLayouMensagemEmail)
        {
            // Solicita o envio da notificação
            var lista = new List<NotificacaoEmailDestinatarioData>();
            lista.Add(destinatario);
            NotificacaoEmailData data = new NotificacaoEmailData()
            {
                SeqConfiguracaoNotificacao = config.SeqConfiguracaoTipoNotificacao,
                DadosMerge = dados,
                DataPrevistaEnvio = DateTime.Now,
                PrioridadeEnvio = PrioridadeEnvio.QuandoPossivel,
                Destinatarios = lista,
                SeqLayouMensagemEmail = seqLayouMensagemEmail
            };
            long seqNotificacaoEnviada = NotificacaoService.SalvarNotificacao(data);

            // Busca o sequencial da notificação-email-destinatário enviada
            var envioDestinatario = NotificacaoService.BuscaNotificacaoEmailDestinatario(seqNotificacaoEnviada);
            if (envioDestinatario.Count != 1)
                throw new EnvioNotificacaoException();

            // Salva a referencia da notificação enviada para a inscrição
            InscricaoEnvioNotificacao envio = new InscricaoEnvioNotificacao()
            {
                SeqInscricao = seqInscricao,
                SeqProcessoConfiguracaoNotificacaoIdioma = config.Seq,
                SeqNotificacaoEmailDestinatario = envioDestinatario.First().Seq
            };
            this.InsertEntity(envio);
        }

        #endregion 
    }
}
