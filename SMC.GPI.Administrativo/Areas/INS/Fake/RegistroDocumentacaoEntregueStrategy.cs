using SMC.DadosMestres.Common.Areas.PES.Enums;
using SMC.Framework.Fake;
using SMC.Framework.Model;
using SMC.GPI.Administrativo.Areas.INS.Models;
using SMC.GPI.Administrativo.Models;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Localidades.UI.Mvc.Models;
using System;
using System.Collections.Generic;

namespace SMC.GPI.Administrativo.Areas.INS.Fake
{
    public class RegistroDocumentacaoEntregueStrategy : SMCFakeStrategyBase
    {
        /// <summary>
        /// Define a prioridade da estratégia de fake
        /// </summary>
        public override int Priority
        {
            get { return 99; }
        }


        protected override bool CheckType(Type type)
        {
            return type == typeof(RegistroDocumentacaoViewModel);
        }

        protected override object CreateType(Type type)
        {
            return Gerar();
        }

        private RegistroDocumentacaoViewModel Gerar()
        {
            var pagina = new RegistroDocumentacaoViewModel()
            {
                SeqInscricao = SMCFakeHelper.Random<long>(1, 999999999),
                DescricaoGrupoOferta = SMCFakeHelper.RandomListElement(FakeConfig.GRUPO_OFERTA),
                DescricaoOfertas = new List<OfertaInscricaoViewModel>()
                {
                    new OfertaInscricaoViewModel()
                    {
                        NumeroOpcao = 1,
                        DescricaoOferta = SMCFakeHelper.RandomListElement(FakeConfig.OFERTAS)
                    },
                    new OfertaInscricaoViewModel()
                    {
                        NumeroOpcao = 2,
                        DescricaoOferta = SMCFakeHelper.RandomListElement(FakeConfig.OFERTAS)
                    }
                },
                NomeInscrito = SMCFakeHelper.RandomListElement(FakeConfig.NOME_INSCRITO),
                DescricaoEtapaAtual = SMCFakeHelper.RandomListElement(FakeConfig.ETAPA)
            };

            var DocumentosEntregues = new List<DocumentoEntregueViewModel>()
            {
                //           new DocumentoEntregueViewModel()
                //           {

                //               SeqInscricao = SMCFakeHelper.Random<long>(1, 999999999),
                //               Seq = SMCFakeHelper.Random<long>(1, 999999999),
                ////DataEntrega = DateTime.Now,
                //SituacaoEntregaDocumento = SituacaoEntregaDocumento.AguardandoEntrega,
                //               FormaEntregaDocumento = FormaEntregaDocumento.Presencial,
                //               DescricaoTipoDocumento = SMCFakeHelper.RandomListElement(FakeConfig.TIPO_DOCUMENTO),
                //               Observacao = "",
                //               VersaoDocumento = VersaoDocumento.CopiaSimples,
                //               //VersaoDocumentoExigido = VersaoDocumento.CopiaSimples,
                //               TipoDocumentoPermiteVariosArquivos = true
                //           },
                //           new DocumentoEntregueViewModel()
                //           {
                //               SeqInscricao = SMCFakeHelper.Random<long>(1, 999999999),
                //               Seq = SMCFakeHelper.Random<long>(1, 999999999),
                ////DataEntrega = DateTime.Now,
                //SituacaoEntregaDocumento = SituacaoEntregaDocumento.AguardandoValidacao,
                //               FormaEntregaDocumento = FormaEntregaDocumento.Correios,
                //               DescricaoTipoDocumento = SMCFakeHelper.RandomListElement(FakeConfig.TIPO_DOCUMENTO),
                //               Observacao = "",
                //               VersaoDocumento = VersaoDocumento.CopiaSimples,
                //               //VersaoDocumentoExigido = VersaoDocumento.CopiaSimples,
                //LinkArquivo = "DocumentoInscrito.pdf",
                //               DescricaoArquivoAnexado = "Curriculo do José da Silva",
                //               TipoDocumentoPermiteVariosArquivos = true
                //           },
                //           new DocumentoEntregueViewModel()
                //           {
                //               SeqInscricao = SMCFakeHelper.Random<long>(1, 999999999),
                //               Seq = SMCFakeHelper.Random<long>(1, 999999999),
                ////DataEntrega = DateTime.Now,
                //SituacaoEntregaDocumento = SituacaoEntregaDocumento.Deferido,
                //               FormaEntregaDocumento = FormaEntregaDocumento.Correios,
                //               DescricaoTipoDocumento = SMCFakeHelper.RandomListElement(FakeConfig.TIPO_DOCUMENTO),
                //               Observacao = "",
                //               VersaoDocumento = VersaoDocumento.Original,
                //               //VersaoDocumentoExigido = VersaoDocumento.Original,
                //               TipoDocumentoPermiteVariosArquivos = true
                //           }
            };

            //       pagina.DocumentosEntregues = new SumarioDocumentosEntreguesViewModel();
            //       pagina.DocumentosEntregues.DocumentosObrigatorios = DocumentosEntregues;

            return pagina;
        }
    }
}