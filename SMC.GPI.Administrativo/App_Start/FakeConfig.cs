using SMC.Framework.Fake;
using SMC.Framework.Model;
using SMC.Framework.UI.Mvc.Html;
using SMC.GPI.Administrativo.Areas.INS.Models;
using SMC.GPI.Administrativo.Areas.INS.Services;
using SMC.GPI.Administrativo.Areas.RES.Services;
using SMC.Localidades.UI.Mvc.Models;
using System.Collections.Generic;

namespace SMC.GPI.Administrativo
{
    public static class FakeConfig
    {
        public static void RegisterStrategies(SMCFakeStrategyConfiguration strategies)
        {
            #region Tipo de Processo

            strategies.AddForProperty<string>()
                    .Where(prop => prop.DeclaringType.Name.Equals("TipoProcessoListaViewModel") && prop.Name == "Descricao")
                    .RandomValues(DESCRICAO_TIPO_PROCESSO);

            strategies.AddForProperty<string>()
                    .Where(prop => prop.DeclaringType.Name.Equals("TipoProcessoViewModel") && prop.Name == "Descricao")
                    .RandomValues(DESCRICAO_TIPO_PROCESSO);

            strategies.AddForProperty<string>()
                    .Where(prop => prop.DeclaringType.Name.Equals("TipoProcessoSituacaoViewModel") && prop.Name == "Descricao")
                    .RandomValues(DESCRICAO_SITUACAO_TIPO_PROCESSO);

            strategies.AddForProperty<string>()
                    .Where(prop => prop.DeclaringType.Name.Equals("TipoProcessoSituacaoViewModel") && prop.Name == "DescricaoInformada")
                    .RandomValues(DESCRICAO_SITUACAO_TIPO_PROCESSO);

            strategies.AddForSelect(typeof(TipoProcessoControllerService), "BuscarTiposProcessosSelect")
                    .RandomValues(DESCRICAO_TIPO_PROCESSO);

            strategies.AddForSelect(typeof(TipoProcessoControllerService), "BuscarTemplatesTiposProcessoSelect")
                    .RandomValues(DESCRICAO_TEMPLATE_PROCESSO);

            strategies.AddForSelect(typeof(TipoProcessoControllerService), "BuscarTiposTemplateProcessoSelect")
                    .RandomValues(DESCRICAO_TIPO_TEMPLATE_PROCESSO);

            strategies.AddForProperty<long>()
                    .Where(prop => prop.DeclaringType.Name.Equals("TipoProcessoViewModel") && prop.Name == "SeqTipoTemplateProcessoSGF")
                    .RandomValues(SMCFakeHelper.Random<long>(1, 2));

            strategies.AddForProperty<List<TipoProcessoTemplateProcessoViewModel>>()
                    .Where(prop => prop.DeclaringType.Name.Equals("TipoProcessoViewModel") && prop.Name == "TemplatesProcesso")
                    .RandomValues(new List<TipoProcessoTemplateProcessoViewModel>()
                    {
                        new TipoProcessoTemplateProcessoViewModel()
                        {
                            Ativo= true,
                            SeqTipoProcessoTemplate = SMCFakeHelper.Random<long>(1,4),
                            SeqTemplateProcessoSGF = SMCFakeHelper.Random<long>(1,4)
                        },
                        new TipoProcessoTemplateProcessoViewModel()
                        {
                            Ativo= false,
                            SeqTipoProcessoTemplate = SMCFakeHelper.Random<long>(1,4),
                            SeqTemplateProcessoSGF = SMCFakeHelper.Random<long>(1,4)
                        },
                        new TipoProcessoTemplateProcessoViewModel()
                        {
                            Ativo= false,
                            SeqTipoProcessoTemplate = SMCFakeHelper.Random<long>(1,4),
                            SeqTemplateProcessoSGF = SMCFakeHelper.Random<long>(1,4)
                        },
                    });

            #endregion Tipo de Processo

            #region AcompanhamentoProcesso

            strategies.AddForProperty<string>()
                 .Where(prop => prop.DeclaringType.Name.Equals("ConsultaConsolidadaProcessoListaViewModel") && prop.Name == "DescricaoProcesso")
                 .RandomValues(DESCRICAO_PROCESSO);

            strategies.AddForProperty<string>()
               .Where(prop => prop.DeclaringType.Name.Equals("ConsultaConsolidadaProcessoCabecalhoViewModel") && prop.Name == "TipoProcesso")
               .RandomValues(TIPO_PROCESSO);

            strategies.AddForProperty<string>()
               .Where(prop => prop.DeclaringType.Name.Equals("ConsultaConsolidadaProcessoCabecalhoViewModel") && prop.Name == "DescricaoProcesso")
               .RandomValues(DESCRICAO_PROCESSO);

            strategies.AddForProperty<string>()
               .Where(prop => prop.DeclaringType.Name.Equals("ConsultaConsolidadaGrupoOfertaListaViewModel") && prop.Name == "DescricaoGrupoOferta")
               .RandomValues(GRUPO_OFERTA);

            strategies.AddForProperty<string>()
               .Where(prop => prop.DeclaringType.Name.Equals("ConsultaConsolidadaOfertaListaViewModel") && prop.Name == "DescricaoOferta")
               .RandomValues(OFERTA);

            //strategies.AddForSelect(typeof(IClienteControllerService), "BuscarClientesSelect")
            //           .RandomValues(CLIENTES);

            //strategies.AddForSelect(typeof(UnidadeResponsavelControllerService), "BuscarUnidadesResponsaveisSelect")
            //           .RandomValues(NOME_SIGLA_UNIDADE_RESPONSAVEL);

            //strategies.AddForSelect(typeof(IAcompanhamentoProcessoControllerService), "BuscarProcessosSelect")
            //           .RandomValues(DESCRICAO_PROCESSO);

            //strategies.AddForSelect(typeof(IAcompanhamentoProcessoControllerService), "BuscarGruposOfertasSelect")
            //         .RandomValues(GRUPO_OFERTA);

            //strategies.AddForSelect(typeof(IAcompanhamentoProcessoControllerService), "BuscarSituacoesProcessoSelect")
            //         .RandomValues(SITUACAO_PROCESSO);

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("ProcessoCabecalhoViewModel") && prop.Name == "TipoProcesso")
                .RandomValues(TIPO_PROCESSO);

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("ProcessoCabecalhoViewModel") && prop.Name == "Cliente")
                .RandomValues(SMCFakeHelper.NomeCompleto());

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("ProcessoCabecalhoViewModel") && prop.Name == "DescricaoProcesso")
                .RandomValues(DESCRICAO_PROCESSO);

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("ConsultaInscricaoProcessoListaViewModel") && prop.Name == "Situacao")
                .RandomValues(SITUACAO_PROCESSO);

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("ConsultaInscricaoProcessoListaViewModel") && prop.Name == "GrupoOferta")
                .RandomValues(GRUPO_OFERTA);

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("ConsultaInscricaoProcessoListaViewModel") && prop.Name == "TaxaInscricao")
                .RandomValues(SITUACAO_TAXA_INSCRICAO);

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("ConsultaInscricaoProcessoListaViewModel") && prop.Name == "Documentacao")
                .RandomValues(SITUACAO_DOCUMENTACAO);

            strategies.AddForProperty<int>()
                .Where(prop => prop.DeclaringType.Name.Equals("ConsultaInscricaoProcessoListaViewModel") && prop.Name == "Nota")
                .RandomValues(SMCFakeHelper.Random<int>(1, 30));

            strategies.AddForProperty<int>()
                .Where(prop => prop.DeclaringType.Name.Equals("ConsultaInscricaoProcessoListaViewModel") && prop.Name == "Classificacao")
                .RandomValues(SMCFakeHelper.Random<int>(1, 100));

            strategies.AddForProperty<short>()
                .Where(prop => prop.DeclaringType.Name.Equals("OfertaInscricaoViewModel") && prop.Name == "NumeroOpcao")
                .RandomValues(SMCFakeHelper.Random<short>(1, 2));

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("OfertaInscricaoViewModel") && prop.Name == "DescricaoOferta")
                .RandomValues(OFERTAS);

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("AnaliseInscricaoLoteListaViewModel") && prop.Name == "Situacao")
                .RandomValues(SITUACAO_PROCESSO);

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("AnaliseInscricaoLoteListaViewModel") && prop.Name == "GrupoOferta")
                .RandomValues(GRUPO_OFERTA);
            /*
            strategies.AddForProperty<List<string>>()
                .Where(prop => prop.DeclaringType.Name.Equals("AnaliseInscricaoLoteListaViewModel") && prop.Name == "DescricaoOfertas")
                .RandomValues(OFERTAS);*/

            strategies.AddForProperty<int>()
                .Where(prop => prop.DeclaringType.Name.Equals("AnaliseInscricaoLoteListaViewModel") && prop.Name == "Nota")
                .RandomValues(SMCFakeHelper.Random<int>(1, 30));

            strategies.AddForProperty<int>()
                .Where(prop => prop.DeclaringType.Name.Equals("AnaliseInscricaoLoteListaViewModel") && prop.Name == "Classificacao")
                .RandomValues(SMCFakeHelper.Random<int>(1, 100));

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("AnaliseInscricaoLoteCabecalhoViewModel") && prop.Name == "TipoProcesso")
                .RandomValues(TIPO_PROCESSO);

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("AnaliseInscricaoLoteCabecalhoViewModel") && prop.Name == "Cliente")
                .RandomValues(SMCFakeHelper.NomeCompleto());

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("AnaliseInscricaoLoteCabecalhoViewModel") && prop.Name == "DescricaoProcesso")
                .RandomValues(DESCRICAO_PROCESSO);

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("AnaliseInscricaoLoteCabecalhoViewModel") && prop.Name == "DescricaoProcesso")
                .RandomValues(DESCRICAO_PROCESSO);

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("AlteracaoSituacaoViewModel") && prop.Name == "Situacao")
                .RandomValues(SITUACOESiNSCRICOES);

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("AlteracaoSituacaoViewModel") && prop.Name == "Justificativa")
                .RandomValues(JUSTIFICATIVAS_ALTERACAO_SITUACAO);

            strategies.AddForProperty<int>()
                .Where(prop => prop.DeclaringType.Name.Equals("DetalheLancamentoNotaClassificacaoViewModel") && prop.Name == "Nota")
                .RandomValues(SMCFakeHelper.Random<int>(1, 30));

            strategies.AddForProperty<int>()
                .Where(prop => prop.DeclaringType.Name.Equals("DetalheLancamentoNotaClassificacaoViewModel") && prop.Name == "Classificacao")
                .RandomValues(SMCFakeHelper.Random<int>(1, 100));

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("SituacaoInscricaoViewModel") && prop.Name == "Justificativa")
                .RandomValues(JUSTIFICATIVAS_ALTERACAO_SITUACAO);

            #endregion AcompanhamentoProcesso

            #region Dynamics

            strategies.AddForProperty<string>()
                     .Where(prop => prop.DeclaringType.Name.Equals("TipoItemHierarquiaOfertaDynamicModel") && prop.Name == "Descricao")
                     .RandomValues(TIPOSHIERAQUIAS);

            strategies.AddForProperty<string>()
                     .Where(prop => prop.DeclaringType.Name.Equals("CodigoAutorizacaoDynamicModel") && prop.Name == "Descricao")
                     .RandomValues(DESCRICAO_CODIGO_AUTORIZACAO);

            strategies.AddForProperty<string>()
                     .Where(prop => prop.DeclaringType.Name.Equals("CodigoAutorizacaoDynamicModel") && prop.Name == "Codigo")
                     .RandomValues(CODIGO_AUTORIZACAO);

            strategies.AddForProperty<string>()
                      .Where(prop => prop.DeclaringType.Name.Contains("CodigoAutorizacaoDynamicModel") && prop.Name == "NomeCliente")
                      .RandomValues(CLIENTES);

            strategies.AddForProperty<string>()
                      .Where(prop => prop.DeclaringType.Name.Contains("CodigoAutorizacaoDynamicModel") && prop.Name == "NomeUnidadeResponsavel")
                      .RandomValues(NOME_UNIDADE_RESPONSAVEL);

            strategies.AddForProperty<string>()
                      .Where(prop => prop.DeclaringType.Name.Contains("TipoDocumentoDynamicModel") && prop.Name == "DescricaoTipoDocumento")
                      .RandomValues(DESCRICAO_TIPO_DOCUMENTO);

            #endregion Dynamics

            #region Unidade Responsável

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("UnidadeResponsavelCabecalhoViewModel") && prop.Name == "Nome")
                .RandomValues(NOME_UNIDADE_RESPONSAVEL);

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("UnidadeResponsavelCabecalhoViewModel") && prop.Name == "Sigla")
                .RandomValues(SIGLA_UNIDADE_RESPONSAVEL);

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("UnidadeResponsavelListaViewModel") && prop.Name == "Nome")
                .RandomValues(NOME_UNIDADE_RESPONSAVEL);

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("UnidadeResponsavelListaViewModel") && prop.Name == "Sigla")
                .RandomValues(SIGLA_UNIDADE_RESPONSAVEL);

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("UnidadeResponsavelViewModel") && prop.Name == "Nome")
                .RandomValues(NOME_UNIDADE_RESPONSAVEL);

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("UnidadeResponsavelViewModel") && prop.Name == "Sigla")
                .RandomValues(SIGLA_UNIDADE_RESPONSAVEL);

            strategies.AddForProperty<List<SMCDatasourceItem>>()
                .Where(prop => prop.DeclaringType.Name.Equals("UnidadeResponsavelViewModel") && prop.Name == "TiposTelefone")
                .RandomValues(new List<SMCDatasourceItem>()
                {
                    new SMCDatasourceItem(){Seq = 1, Descricao = "Comercial"},
                    new SMCDatasourceItem(){Seq = 2, Descricao = "Fax"},
                });           

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("UnidadeResponsavelTipoFormularioListaViewModel") && prop.Name == "DescricaoTipoFormulario")
                .RandomValues(DESCRICAO_TIPO_FORMULARIO);

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("EnderecoEletronicoViewModel") && prop.Name == "Descricao")
                .RandomValues(SMCFakeHelper.Email());

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("InformacoesEnderecoViewModel") && prop.Name == "Numero")
                .RandomValues(new string[] { "123", "567", "432" });

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("TelefoneViewModel") && prop.Name == "CodigoPais")
                .RandomValues(new string[] { "55" });

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("TelefoneViewModel") && prop.Name == "CodigoArea")
                .RandomValues(new string[] { "31", "21", "11", });

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("TelefoneViewModel") && prop.Name == "Numero")
                .RandomValues(new string[] { "5648-1458", "5458-9645", "6541-4652", });

            strategies.AddForSelect(typeof(UnidadeResponsavelControllerService), "BuscarTiposHierarquiaOfertaSelect")
                .RandomValues(DESCRICAO_TIPO_HIERARQUIA_OFERTA);

            strategies.AddForSelect(typeof(UnidadeResponsavelControllerService), "BuscarTiposProcessosSelect")
                .RandomValues(DESCRICAO_TIPO_PROCESSO);

            strategies.AddForSelect(typeof(UnidadeResponsavelControllerService), "BuscarTiposFormulariosSelect")
                .RandomValues(DESCRICAO_TIPO_FORMULARIO);

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("UnidadeResponsavelTipoProcessoListaViewModel") && prop.Name == "Descricao")
                .RandomValues(DESCRICAO_TIPO_PROCESSO);

            #endregion Unidade Responsável

            #region Cliente

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("ClienteViewModel") && prop.Name == "Nome")
                .RandomValues(CLIENTES);

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("ClienteViewModel") && prop.Name == "Sigla")
                .RandomValues(CLIENTES_SIGLA);

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("ClienteListaViewModel") && prop.Name == "Nome")
                .RandomValues(CLIENTES);

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("ClienteListaViewModel") && prop.Name == "Sigla")
                .RandomValues(CLIENTES_SIGLA);

            #endregion Cliente

            #region Registro de Documentação Entregue

            strategies.AddForProperty<string>()
                 .Where(prop => prop.DeclaringType.Name.Equals("RegistroDocumentacaoViewModel") && prop.Name == "DescricaoProcesso")
                 .RandomValues(DESCRICAO_TIPO_PROCESSO);

            strategies.AddForProperty<string>()
                 .Where(prop => prop.DeclaringType.Name.Equals("RegistroDocumentacaoViewModel") && prop.Name == "TipoProcesso")
                 .RandomValues(TIPO_PROCESSO);

            strategies.AddForProperty<string>()
               .Where(prop => prop.DeclaringType.Name.Equals("RegistroDocumentacaoViewModel") && prop.Name == "Nome")
               .RandomValues(CLIENTES);

            strategies.AddForProperty<string>()
             .Where(prop => prop.DeclaringType.Name.Equals("RegistroDocumentacaoViewModel") && prop.Name == "Etapa")
             .RandomValues(ETAPA);

            strategies.AddForProperty<string>()
             .Where(prop => prop.DeclaringType.Name.Equals("RegistroDocumentacaoViewModel") && prop.Name == "Cliente")
             .RandomValues(CLIENTES);

            strategies.AddForProperty<string>()
            .Where(prop => prop.DeclaringType.Name.Equals("RegistroDocumentacaoViewModel") && prop.Name == "GrupoOferta")
            .RandomValues(GRUPO_OFERTA);

            strategies.AddForProperty<string>()
               .Where(prop => prop.DeclaringType.Name.Equals("RegistroDocumentacaoViewModel") && prop.Name == "Oferta")
               .RandomValues(OFERTA);

            strategies.AddForProperty<string>()
               .Where(prop => prop.DeclaringType.Name.Equals("DocumentosEntreguesViewModel") && prop.Name == "NomeDocumento")
               .RandomValues(TIPO_DOCUMENTO);

            strategies.AddForProperty<string>()
           .Where(prop => prop.DeclaringType.Name.Equals("DocumentosEntreguesViewModel") && prop.Name == "NomeInscrito")
           .RandomValues(NOME_INSCRITO);

            #endregion Registro de Documentação Entregue

            #region Tipo Hirarquia Oferta

            strategies.AddForProperty<string>()
                 .Where(prop => prop.DeclaringType.Name.Equals("TipoHierarquiaOfertaListaViewModel") && prop.Name == "Descricao")
                 .RandomValues(DESCRICAO_TIPO_HIERARQUIA_OFERTA);

            strategies.AddForProperty<string>()
                 .Where(prop => prop.DeclaringType.Name.Equals("TipoHierarquiaOfertaViewModel") && prop.Name == "Descricao")
                 .RandomValues(DESCRICAO_TIPO_HIERARQUIA_OFERTA);

            strategies.AddForSelect(typeof(TipoHierarquiaOfertaControllerService), "BuscarTiposHierarquiaOfertaSelect")
                 .RandomValues(TIPOS_HIERAQUIA_OFERTA);

            strategies.AddForProperty<string>()
                 .Where(prop => prop.DeclaringType.Name.Equals("AssociarTipoHierarquiaOfertaViewModel") && prop.Name == "Descricao")
                 .RandomValues(TIPOS_HIERAQUIA_OFERTA);

            #endregion Tipo Hirarquia Oferta

            #region Processo

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("ProcessoListaViewModel") && prop.Name == "Descricao")
                .RandomValues(DESCRICAO_PROCESSO);

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("ProcessoListaViewModel") && prop.Name == "TipoProcesso")
                .RandomValues(DESCRICAO_TIPO_PROCESSO);

            strategies.AddForProperty<int>()
                .Where(prop => prop.DeclaringType.Name.Equals("ProcessoListaViewModel") && prop.Name == "AnoReferencia")
                .RandomValues(SMCFakeHelper.Random<int>(2015, 2016));

            strategies.AddForProperty<int>()
                .Where(prop => prop.DeclaringType.Name.Equals("ProcessoListaViewModel") && prop.Name == "SemestreReferencia")
                .RandomValues(SMCFakeHelper.Random<int>(1, 2));

            strategies.AddForSelect(typeof(ProcessoControllerService), "BuscarTiposProcessoSelect")
                .RandomValues(DESCRICAO_TIPO_PROCESSO);

            strategies.AddForSelect(typeof(ProcessoControllerService), "BuscarTiposProcessoPorUnidadeResponsavelSelect")
                .RandomValues(DESCRICAO_TIPO_PROCESSO);

            strategies.AddForSelect(typeof(ProcessoControllerService), "BuscarUnidadesResponsaveisSelect")
                .RandomValues(NOME_UNIDADE_RESPONSAVEL);

            strategies.AddForSelect(typeof(ProcessoControllerService), "BuscarTiposHierarquiaOfertaSelect")
                .RandomValues(DESCRICAO_TIPO_HIERARQUIA_OFERTA);

            strategies.AddForSelect(typeof(ProcessoControllerService), "BuscarTemplatesProcessoSelect")
                .RandomValues(DESCRICAO_TEMPLATE_PROCESSO);

            strategies.AddForSelect(typeof(ProcessoControllerService), "BuscarClientesSelect")
                .RandomValues(CLIENTES);

            strategies.AddForSelect(typeof(ProcessoControllerService), "BuscarGruposOfertaSelect")
         .RandomValues(GRUPO_OFERTA);

            strategies.AddForSelect(typeof(ProcessoControllerService), "BuscarTaxasOfertaSelect")
.RandomValues(TAXAS);

            strategies.AddForSelect(typeof(ProcessoControllerService), "BuscarCodigosAutorizacaoSelect")
         .RandomValues(CODIGO_AUTORIZACAO_SIGLA);

            strategies.AddForSelect(typeof(ProcessoControllerService), "BuscarTiposItemHierarquiaOfertaSelect")
                .RandomValues(TIPOS_HIERAQUIA_OFERTA);

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("CabecalhoProcessoViewModel") && prop.Name == "TipoProcesso")
                .RandomValues(DESCRICAO_TIPO_PROCESSO);

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("CabecalhoProcessoViewModel") && prop.Name == "Processo")
                .RandomValues(DESCRICAO_PROCESSO);

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("CabecalhoProcessoEtapaConfiguracaoViewModel") && prop.Name == "TipoProcesso")
                .RandomValues(DESCRICAO_TIPO_PROCESSO);

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("CabecalhoProcessoEtapaConfiguracaoViewModel") && prop.Name == "Processo")
                .RandomValues(DESCRICAO_PROCESSO);

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("CabecalhoProcessoEtapaConfiguracaoViewModel") && prop.Name == "Etapa")
                .RandomValues(ETAPA);

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("CabecalhoProcessoEtapaConfiguracaoViewModel") && prop.Name == "ConfiguracaoEtapa")
                .RandomValues(CONFIGURACAO_ETAPA);

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("AssociarItemHierarquiaOfertaViewModel") && prop.Name == "DescricaoItemHierarquiaOfertaSuperior")
                .RandomValues(OFERTAS);

            strategies.AddForProperty<string>()
               .Where(prop => prop.DeclaringType.Name.Equals("AssociarItemHierarquiaOfertaViewModel") && prop.Name == "Descricao")
               .RandomValues(OFERTAS);

            strategies.AddForProperty<long>()
                .Where(prop => prop.DeclaringType.Name.Equals("AssociarItemHierarquiaOfertaViewModel") && prop.Name == "SeqTipoItemHierarquiaOfertaSelecionado")
                .RandomValues(SMCFakeHelper.Random<long>(1, 5));

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("AssociarItemHierarquiaOfertaViewModel") && prop.Name == "Processo")
                .RandomValues(DESCRICAO_PROCESSO);

            strategies.AddForProperty<long>()
                .Where(prop => prop.DeclaringType.Name.Equals("GrupoOfertaViewModel") && prop.Name == "SeqGrupoOferta")
                .RandomValues(SMCFakeHelper.Random<long>(1, 2));

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("GrupoOfertaListaViewModel") && prop.Name == "Descricao")
                .RandomValues(GRUPO_OFERTA);

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("GrupoOfertaViewModel") && prop.Name == "Descricao")
                .RandomValues(GRUPO_OFERTA);

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("ItemGrupoOfertaViewModel") && prop.Name == "Descricao")
                .RandomValues(SMCFakeHelper.NomeCompleto());

            #endregion Processo

            #region Associacao de Etapa

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("AssociacaoEtapaListaViewModel") && prop.Name == "Etapa")
                .RandomValues(ETAPA);

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("AssociacaoEtapaListaViewModel") && prop.Name == "Situacao")
                .RandomValues(SITUACAO_ETAPA);

            strategies.AddForSelect(typeof(ProcessoControllerService), "BuscarEtapasSelect")
                 .RandomValues(ETAPA);

            strategies.AddForSelect(typeof(ProcessoControllerService), "BuscarSituacoesEtapaSelect")
                .RandomValues(SITUACAO_ETAPA);

            strategies.AddForProperty<long>()
                .Where(prop => prop.DeclaringType.Name.Equals("AssociacaoEtapaViewModel") && prop.Name == "SeqEtapaSelecionado")
                .RandomValues(SMCFakeHelper.Random<long>(1, 2));

            strategies.AddForProperty<long>()
                .Where(prop => prop.DeclaringType.Name.Equals("AssociacaoEtapaViewModel") && prop.Name == "SeqSituacaoSelecionado")
                .RandomValues(SMCFakeHelper.Random<long>(1, 2));

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("ConfiguracaoEtapaListaViewModel") && prop.Name == "Descricao")
                .RandomValues(CONFIGURACAO_ETAPA);

            strategies.AddForProperty<long>()
                .Where(prop => prop.DeclaringType.Name.Equals("GrupoOfertaDetalheViewModel") && prop.Name == "SeqGrupoOfertaSelecionado")
                .RandomValues(SMCFakeHelper.Random<long>(1, 3));

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("ConfiguracaoEtapaViewModel") && prop.Name == "Descricao")
                .RandomValues(CONFIGURACAO_ETAPA);

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("ConfiguracaoEtapaViewModel") && prop.Name == "DescricaoDocumentacao")
                .RandomValues(DESCRICAO_ENTREGA_DOCUMENTACAO);

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("CabecalhoProcessoEtapaViewModel") && prop.Name == "TipoProcesso")
                .RandomValues(TIPO_PROCESSO);

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("CabecalhoProcessoEtapaViewModel") && prop.Name == "Etapa")
                .RandomValues(ETAPA);

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("CabecalhoProcessoEtapaViewModel") && prop.Name == "Processo")
                .RandomValues(DESCRICAO_PROCESSO);

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("CabecalhoProcessoEtapaViewModel") && prop.Name == "ConfiguracaoEtapa")
                .RandomValues(CONFIGURACAO_ETAPA);

            strategies.AddForProperty<long>()
                .Where(prop => prop.DeclaringType.Name.Equals("CabecalhoProcessoEtapaViewModel") && prop.Name == "SeqProcesso")
                .RandomValues(SMCFakeHelper.Random<long>(1, 99999));

            strategies.AddForProperty<long>()
    .Where(prop => prop.DeclaringType.Name.Equals("CabecalhoProcessoEtapaConfiguracaoViewModel") && prop.Name == "SeqProcesso")
    .RandomValues(SMCFakeHelper.Random<long>(1, 99999));

            strategies.AddForSelect(typeof(ProcessoControllerService), "BuscarTiposFormularioSelect")
                .RandomValues(DESCRICAO_TIPO_FORMULARIO);

            strategies.AddForSelect(typeof(ProcessoControllerService), "BuscarFormulariosSelect")
                .RandomValues(FORMULARIOS);

            strategies.AddForSelect(typeof(ProcessoControllerService), "BuscarVisoesSelect")
                .RandomValues(VISOES);

            strategies.AddForSelect(typeof(ProcessoControllerService), "BuscarVersoesDocumentosSelect")
                .RandomValues(VERSOES_DOCUMENTOS);

            strategies.AddForSelect(typeof(ProcessoControllerService), "BuscarTiposDocumentosSelect")
                .RandomValues(TIPOS_DOCUMENTO);

            strategies.AddForProperty<long>()
                .Where(prop => prop.DeclaringType.Name.Equals("DocumentacaoRequeridaViewModel") && prop.Name == "SeqTipoDocumentoSelecionado")
                .RandomValues(SMCFakeHelper.Random<long>(1, 5));

            strategies.AddForProperty<long>()
                .Where(prop => prop.DeclaringType.Name.Equals("DocumentacaoRequeridaViewModel") && prop.Name == "SeqVersaoDocumentoSelecionado")
                .RandomValues(SMCFakeHelper.Random<long>(1, 2));

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("DocumentacaoRequeridaListaViewModel") && prop.Name == "TipoDocumento")
                .RandomValues(TIPOS_DOCUMENTO);

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("DocumentacaoRequeridaListaViewModel") && prop.Name == "Versao")
                .RandomValues(VERSOES_DOCUMENTOS);

            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("GrupoDocumentacaoRequeridaListaViewModel") && prop.Name == "DescricaoGrupo")
                .RandomValues(DESCRICAO_GRUPO_DOCUMENTACAO);

            strategies.AddForProperty<int>()
                .Where(prop => prop.DeclaringType.Name.Equals("GrupoDocumentacaoRequeridaListaViewModel") && prop.Name == "MinimoDocumentosObrigatorios")
                .RandomValues(SMCFakeHelper.Random<int>(1, 2));

            strategies.AddForProperty<List<string>>()
                .Where(prop => prop.DeclaringType.Name.Equals("GrupoDocumentacaoRequeridaListaViewModel") && prop.Name == "ItensGrupo")
                .RandomValues(new List<string>() { SMCFakeHelper.RandomListElement<string>(TIPO_DOCUMENTO),
                                                   SMCFakeHelper.RandomListElement<string>(TIPO_DOCUMENTO),
                                                   SMCFakeHelper.RandomListElement<string>(TIPO_DOCUMENTO) });
            strategies.AddForProperty<string>()
                .Where(prop => prop.DeclaringType.Name.Equals("GrupoDocumentacaoRequeridaViewModel") && prop.Name == "DescricaoGrupo")
                .RandomValues(DESCRICAO_GRUPO_DOCUMENTACAO);

            strategies.AddForProperty<int>()
                .Where(prop => prop.DeclaringType.Name.Equals("GrupoDocumentacaoRequeridaViewModel") && prop.Name == "MinimoDocumentosObrigatorios")
                .RandomValues(SMCFakeHelper.Random<int>(1, 2));

            strategies.AddForProperty<List<GrupoDocumentosRequeridosDetalheViewModel>>()
                .Where(prop => prop.DeclaringType.Name.Equals("GrupoDocumentacaoRequeridaViewModel") && prop.Name == "ItensGrupoDetalhe")
                .RandomValues(new List<GrupoDocumentosRequeridosDetalheViewModel>()
                {
                    new GrupoDocumentosRequeridosDetalheViewModel(){SeqDocumentoRequerido = SMCFakeHelper.Random<long>(1,3)},
                    new GrupoDocumentosRequeridosDetalheViewModel(){SeqDocumentoRequerido = SMCFakeHelper.Random<long>(1,3)},
                    new GrupoDocumentosRequeridosDetalheViewModel(){SeqDocumentoRequerido = SMCFakeHelper.Random<long>(1,3)},
                    new GrupoDocumentosRequeridosDetalheViewModel(){SeqDocumentoRequerido = SMCFakeHelper.Random<long>(1,3)}
                });

            strategies.AddForSelect(typeof(ProcessoControllerService), "BuscarItensGrupoDocumentacaoRequeridaSelect")
                .RandomValues(DESCRICAO_TIPO_DOCUMENTO);

            #endregion Associacao de Etapa
        }

        #region Lista de Constantes

        public static string[] NOME_UNIDADE_RESPONSAVEL =  {"Centro de Registros Acadêmicos",
                                                            "Colégio Santa Maria",
                                                            "Departamento de Relações Internacionais",
                                                            "Diretoria de Educação Continuada"};

        public static string[] SIGLA_UNIDADE_RESPONSAVEL = { "CRA", "CSM", "IEC" };

        public static string[] NOME_SIGLA_UNIDADE_RESPONSAVEL = { "Centro de Registros Acadêmicos - CRA",
                                                                  "Colégio Santa Maria - CSM"};

        public static int[] CODIGO_AREA_TELEFONE_CONTATO_UNIDADE_RESPONSAVEL = { 31, 21, 61, 11 };

        public static int[] CODIGO_PAIS_TELEFONE_CONTATO_UNIDADE_RESPONSAVEL = { 55, 78, 85, 35, 67 };

        public static string[] DESCRICAO_TIPO_FORMULARIO =  {"Tipo de Formulário de Inscrição - CRA",
                                                             "Tipo de Formulário de Inscrição - Colégio Santa Maria",
                                                             "Tipo de Formulário de Inscrição - Minionu",
                                                             "Tipo de Formulário de Inscrição - Programa de Pós-Graduação em Direito"};

        public static string[] DESCRICAO_TIPO_HIERARQUIA_OFERTA =  {"Stricto Sensu - Área de Concentração",
                                                                         "Stricto Sensu - Linha de Pesquisa",
                                                                         "Stricto Sensu - Orientador",
                                                                         "Stricto Sensu - Disciplina Isolada"};

        public static string[] DESCRICAO_TIPO_PROCESSO =  {"Vestibular - Graduação",
                                                           "Enem - Graduação",
                                                           "Processo Seletivo - Disciplina Isolada - Graduação",
                                                           "Portador de Diploma - Graduação"};

        public static string[] DESCRICAO_TIPO_DOCUMENTO =  {"CPF",
                                                           "Carteira de Identidade",
                                                           "Diploma da Graduação",
                                                           "Histórico Escolar da Graduação"};

        public static string[] IDIOMAS = { "Português",
                                             "Inglês",
                                             "Francês",
                                             "Alemão",
                                             "Espanhol" };

        public static string[] SITUACOESiNSCRICOES = { "Inscrição Iniciada",
                                                         "Aguardando confirmação da inscrição",
                                                         "Inscrição indeferida",
                                                         "Candidato desistente",
                                                         "Candidato reprovado na etapa",
                                                         "Candidato aprovado" };

        public static string[] TIPONOTIFICACOES = { "Cobrança da documentação",
                                                      "Cobrança da taxa",
                                                      "Confirmação de baixa da documentação",
                                                      "Confirmação de baixa do pagamento",
                                                      "Confirmação de inscrição",
                                                      "Deferimento da inscrição" };

        public static string[] TIPOSHIERAQUIAS = {  "Curso",
                                                    "Unidade",
                                                    "localidade",
                                                    "Turno",
                                                    "Turma",
                                                    "Área de Concentração" };

        public static string[] TIPOS_HIERAQUIA_OFERTA = {  "Programa",
                                                            "Curso",
                                                            "localidade",
                                                            "Área de Concentração" ,
                                                            "Linha de Pesquisa",
                                                            "Orientador",
                                                            "Disciplina Isolada"
                                                        };

        public static string[] DESCRICAO_SITUACAO_TIPO_PROCESSO = {"Inscrição iniciada",
                                                                   "Aguardando confirmação da inscrição",
                                                                   "Inscrição confirmada",
                                                                   "Inscrição indeferida",
                                                                   "Inscrição cancelada",
                                                                   "Candidato em avaliação",
                                                                   "Candidato desistente"};

        public static string[] CLIENTES = {"Hospital Madre Tereza",
                                            "Vale do Rio Doce",
                                            "Tribunal de Justiça do Estado de Minas Gerais",
                                            "Prefeitura de Belo Horizonte"};

        public static string[] CLIENTES_SIGLA = { "HMT", "VALE", "TJMG", "PBH" };

        public static string[] DESCRICAO_CODIGO_AUTORIZACAO = {  "Código de Inscrições da Vale 1º/2015",
                                                     "Código de Inscrições do TJMG",
                                                     "Código de Inscrições da FIAT",
                                                     "Código de Inscrições Trilhas 2015"};

        public static string[] CODIGO_AUTORIZACAO = {  "VALE12015",
                                                     "TJMG2015",
                                                     "FIAT2015",
                                                     "TRILHA2015"};

        public static string[] CODIGO_AUTORIZACAO_SIGLA = { "Código de Autorização da FIAT - FIAT2008 - FIAT Automóveis S.A.",
                                                                "Código de Autorização do Trilhas - TRILHAS2015",
                                                                "Código de Autorização da Vale - VALE2009 - Vale S.A." };

        public static string[] DESCRICAO_PROCESSO = {  "Processo de Seleção para Matrícula em Disciplinas Isoladas dos Cursos de Mestrado e Doutorado do Programa de Pós Graduação em Direito –1º/2015",
                                                     "Inscrições para 2º semestre de 2014 - IEC Praça da Liberdade",
                                                     "Processo Seletivo IEC - 1/2015",
                                                     "Admissão de novos alunos - Colégio Santa Maria/2015"};

        public static string[] GRUPO_OFERTA = {  "Master",
                                                    "MBA",
                                                    "PDP",
                                                    "Curta Duração",
                                                    "Aperfeiçoamento" };

        public static string[] TIPO_PROCESSO = { "Processo Seletivo –Lato Sensu" };

        public static string[] OFERTA = {  "MBA Executivo em Estratégia e Negócios",
                                            "MBA Executivo em Finanças",
                                            "PDP em mídias sociais",
                                            "MBA Executivo em Marketing"  };

        public static string[] SITUACAO_PROCESSO = {  "Inscrição Confirmada",
                                                       "Inscrição Cancelada",
                                                       "Inscrição Deferida",
                                                       "Inscrição Indeferida",
                                                       "Candidato Desistente",
                                                       "Candidato Reprovado",
                                                       "Candidato Aprovado",
                                                       "Candidato Selecionado",
                                                       "Candidato Excedente",
                                                       "Convocado Desistente",
                                                       "Convocado Confirmado"
                                                   };

        public static string[] SITUACAO_TAXA_INSCRICAO = { "Baixada" };

        public static string[] SITUACAO_DOCUMENTACAO = { "Entregue" };

        public static string[] OFERTAS = { "MBA Executivo em Estratégia e Negócios", "MBA Executivo em Finanças" };

        public static string[] DESCRICAO_DOCUMENTO = { "Currículo - Samila Souza", "Certificado do curso de aperfeiçoamento em Inovação em Negócios" };

        public static string[] ARQUIVO_DOCUMENTO = { "Currículo - Samila Souza.pdf", "Certificado.pdf", "-" };

        public static string[] TIPO_DOCUMENTO = { "Currículo", "Comprovação de Currículo", "RG", "CPF",
                                                    "Certificado de reservista", "Diploma de graduação",
                                                    "Histórico Escolar da graduação", "Formulário de inscrição"};

        public static string[] VALOR = { "R$ 30,00", "R$ 20,00", "R$ 15,00", "R$ 45,00" };

        public static string[] JUSTIFICATIVAS_ALTERACAO_SITUACAO = { "Desistência de continuar o processo.", "Processo de inscrição cancelado." };

        public static string[] ETAPA = { "Inscrição", "Seleção", "Convocação" };

        public static string[] SITUACAO_ETAPA = { "Liberada", "Aguardando Liberação", "Em Manutenção" };

        public static string[] NOME_INSCRITO = { "Joice Batista", "Leandro Almeida", "Joao Ricardo Costa" };

        public static string[] DESCRICAO_TEMPLATE_PROCESSO = {
                                                                "Template do Processo de Inscrição do Lato Sensu",
                                                                "Template do Processo de Inscrição do Stricto Sensu",
                                                                "Template do Processo de Inscrição IEC / FIAT",
                                                                "Template do Processo de Inscrição do Minionu"
                                                             };

        public static string[] DESCRICAO_TIPO_TEMPLATE_PROCESSO = { "Inscrição/Seleção/Convocação", "Matrícula" };

        public static string[] MOTIVO_CANCELAMENTO_OFERTA = { "Oferta cancelada" };

        public static string[] CONFIGURACAO_ETAPA = { "Processo Seletivo para os Cursos de Mestrado e Doutorado do Programa de Pós-graduação em Geografia 1º/2015 – Inscrição – Mestrado",
                                                      "Processo Seletivo para os Cursos de Mestrado e Doutorado do Programa de Pós-graduação em Geografia 1º/2015 – Inscrição – Doutorado "
                                                    };

        public static string[] DESCRICAO_ENTREGA_DOCUMENTACAO = { "Entrega da documentação obrigatória.", "Data da entrega deve ser realizada dentro do mês corrente." };

        public static string[] VISOES = { "Visão 1", "Visão 2", "Visão 3", "Visão 4" };

        public static string[] FORMULARIOS = { "Formulário 1", "Formulário 2", "Formulário 3", "Formulário 4" };

        public static string[] TIPOS_DOCUMENTO = {"Formulário de inscrição",
                                                   "Fotografia 3x4",
                                                   "RG",
                                                   "CPF",
                                                   "Certificado de reservista",
                                                   "Dispensa do serviço militar",
                                                   "Diploma de graduação",
                                                   "Histórico escolar da graduação",
                                                   "Diploma do mestrado",
                                                   "Histórico escolar do mestrado",
                                                   "Curriculum vitae",
                                                   "Comprovação do curriculum",
                                                   "Pré-projeto da dissertação",
                                                   "Comprovante de pagamento da taxa",
                                                   "Conprovante de endereço",
                                                   "Carta de apresentação"
                                                  };

        public static string[] VERSOES_DOCUMENTOS = { "Cópia simples", "Original" };

        public static string[] DESCRICAO_GRUPO_DOCUMENTACAO = { "Documento de Identificação", "Certificado de Graduação" };

        public static string[] TAXAS = { "Taxa de Inscrição", "Taxa de Administração" };

        #endregion Lista de Constantes
    }
}