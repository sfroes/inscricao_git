using SMC.Framework.Model;
using SMC.Framework.Service;
using SMC.Framework.Extensions;
using SMC.Framework.Mapper;
using SMC.Inscricoes.Domain.Areas.INS.DomainServices;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using System.Collections.Generic;
using System.Linq;
using SMC.Inscricoes.Domain.Areas.INS;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using System;

namespace SMC.Inscricoes.Service.Areas.INS.Services
{
    public class DocumentoRequeridoService : SMCServiceBase, IDocumentoRequeridoService
    {
        #region Domain Service

        private DocumentoRequeridoDomainService DocumentoRequeridoDomainService
        {
            get { return this.Create<DocumentoRequeridoDomainService>(); }
        }

        private GrupoDocumentoRequeridoDomainService GrupoDocumentoRequeridoDomainService
        {
            get { return this.Create<GrupoDocumentoRequeridoDomainService>(); }
        }

        private InscricaoDocumentoDomainService InscricaoDocumentoDomainService
        {
            get { return this.Create<InscricaoDocumentoDomainService>(); }
        }

        #endregion

        #region Services

        private DadosMestres.ServiceContract.Areas.GED.Interfaces.ITipoDocumentoService TipoDocumentoService
        {
            get { return this.Create<SMC.DadosMestres.ServiceContract.Areas.GED.Interfaces.ITipoDocumentoService>(); }
        }

        #endregion

        #region Métodos de inscrição

        /// <summary>
        /// Buscar a lista de tipos de documento requeridos que são de upload obrigatório
        /// </summary>
        /// <param name="seqConfiguracaoEtapa">Sequencial de configuração da etapa</param>
        /// <returns>Sequencial dos documentos requeridos que são de upload obrigatório</returns>
        public List<InscricaoDocumentoUploadData> BuscarTiposDocumentoRequeridoUploadObrigatorio(long seqConfiguracaoEtapa)
        {
            DocumentoRequeridoFilterSpecification spec = new DocumentoRequeridoFilterSpecification()
            {
                SeqConfiguracaoEtapa = seqConfiguracaoEtapa,
                UploadObrigatorio = true
            };
            var docs = DocumentoRequeridoDomainService.SearchProjectionBySpecification(spec,
                x => new InscricaoDocumentoUploadData
                {
                    SeqDocumentoRequerido = x.Seq,
                    SeqTipoDocumento = x.SeqTipoDocumento,
                    DescricaoTipoDocumento = x.TipoDocumento.Descricao
                }).ToList();

            /* Busca a descrição da view e não mais via serviço
            foreach (var item in docs) 
            {
                item.DescricaoTipoDocumento = TipoDocumentoService.BuscarTipoDocumento(item.SeqTipoDocumento).Descricao;
            }
            */
            return docs;
        }


        /// <summary>
        /// Busca a lista de documentos permitidos para um determinado grupo
        /// </summary>                
        public List<SMCDatasourceItem> BuscarTiposDocumentoGrupo(long seqGrupoDocumentos)
        {
            return this.GrupoDocumentoRequeridoDomainService.BuscarTiposDocumentoGrupo(seqGrupoDocumentos);
        }

        /// <summary>
        /// Busca a lista de documentos permitidos para um determinado grupo
        /// </summary>                
        public List<SMCDatasourceItem> BuscarDocumentosOpcionais(long seqConfiguracaoEtapa)
        {
            return this.InscricaoDocumentoDomainService.BuscarDocumentosOpcionais(seqConfiguracaoEtapa)
                ;
        }

        #endregion

        #region CRUD Documentos Requeridos


        /// <summary>
        /// Busca os tipos de documento de uma configuração de etapa para listagem
        /// </summary>        
        public SMCPagerData<DocumentoRequeridoListaData> BuscarDocumentosRequeridos(DocumentoRequeridoFiltroData filtro)
        {
            int total = 0;
            var itens = this.DocumentoRequeridoDomainService.
                SearchProjectionBySpecification(filtro.Transform<DocumentoRequeridoFilterSpecification>(),
                x => new DocumentoRequeridoListaData
                {
                    Seq = x.Seq,
                    SeqConfiguracaoEtapa = x.SeqConfiguracaoEtapa,
                    SeqEtapaProcesso = x.ConfiguracaoEtapa.SeqEtapaProcesso,
                    SeqProcesso = x.ConfiguracaoEtapa.EtapaProcesso.SeqProcesso,
                    Sexo = x.Sexo,
                    Obrigatorio = x.Obrigatorio,
                    PermiteUploadArquivo = x.PermiteUploadArquivo,
                    UploadObrigatorio = x.UploadObrigatorio,
                    DescricaoTipoDocumento = x.TipoDocumento.Descricao,
                    VersaoDocumento = x.VersaoDocumento
                }, out total);
            return new SMCPagerData<DocumentoRequeridoListaData>(itens, total);
        }


        /// <summary>
        /// Busca um documento requerido completo para edição/exibiç/ao
        /// </summary>        
        public DocumentoRequeridoData BuscarDocumentoRequerido(long seqDocumentoRequerido)
        {
            return this.DocumentoRequeridoDomainService
                .SearchByKey<DocumentoRequerido, DocumentoRequeridoData>(seqDocumentoRequerido);
        }

        /// <summary>
        /// Salva um documento requerido realizando as validações
        /// </summary>
        public long SalvarDocumentoRequerido(DocumentoRequeridoData documentoRequerido)
        {
            return this.DocumentoRequeridoDomainService.SalvarDocumentoRequerido(
                documentoRequerido.Transform<DocumentoRequerido>());
        }


        /// <summary>
        /// Exclui um documento requerido realizando as validações        
        public void ExcluirDocumentoRequerido(long seqDocumentoRequerido)
        {
            this.DocumentoRequeridoDomainService.ExcluirDocumentoRequerido(
                seqDocumentoRequerido);
        }

        #endregion

        #region CRUD Grupo de documentos

        /// <summary>
        /// Retorna a lista de grupos de documentos de uma configuração de etapa para exibição
        /// </summary>
        public List<GrupoDocumentoRequeridoListaData> BuscarGruposDocumentosRequiridos(GrupoDocumentoRequeridoFiltroData filtros)
        {
            return this.GrupoDocumentoRequeridoDomainService.SearchProjectionBySpecification(
                filtros.Transform<GrupoDocumentoRequeridoFilterSpecification>(),
                x => new GrupoDocumentoRequeridoListaData
                {
                    Seq = x.Seq,
                    SeqConfiguracaoEtapa = x.SeqConfiguracaoEtapa,
                    MinimoObrigatorio = x.MinimoObrigatorio,
                    Descricao = x.Descricao,
                    UploadObrigatorio = x.UploadObrigatorio,
                    Itens = x.Itens.OrderBy(o => o.DocumentoRequerido.TipoDocumento.Descricao).Select(i => i.DocumentoRequerido.TipoDocumento.Descricao).ToList()
                }).ToList();
        }

        /// <summary>
        /// Busca uma lista de documentos requeridos para preencher selects
        /// </summary>       
        public IEnumerable<SMCDatasourceItem> BuscarDocumentosRequeridosKeyValue(DocumentoRequeridoFiltroData filtro)
        {
            return this.DocumentoRequeridoDomainService.SearchProjectionBySpecification(
                filtro.Transform<DocumentoRequeridoFilterSpecification>(),
                x => new SMCDatasourceItem
                {
                    Seq = x.Seq,
                    Descricao = x.TipoDocumento.Descricao
                });
        }

        /// <summary>
        /// Retorna um grupo de documento requerido para exibição/edição
        /// </summary>
        public GrupoDocumentoRequeridoData BuscarGrupoDocumentoRequerido(long seqGrupoDocumentoRequerido)
        {
            return this.GrupoDocumentoRequeridoDomainService.
                SearchByKey<GrupoDocumentoRequerido, GrupoDocumentoRequeridoData>(seqGrupoDocumentoRequerido,
                IncludesGrupoDocumentoRequerido.Itens);
        }

        /// <summary>
        /// Salva um grupo de documento requerido no banco
        /// </summary>        
        public long SalvarGrupoDocumentoRequerido(GrupoDocumentoRequeridoData grupo)
        {
            return this.GrupoDocumentoRequeridoDomainService.
                SalvarGrupoDocumentoRequerido(grupo.Transform<GrupoDocumentoRequerido>());
        }

        /// <summary>
        /// Exclui um grupo de documento requerido
        /// </summary>        
        public void ExcluirGrupoDocumentoRequerido(long seqGrupoDocumentoRequerido)
        {
            this.GrupoDocumentoRequeridoDomainService.ExcluirGrupoDocumentoRequerido(seqGrupoDocumentoRequerido);
        }

        #endregion

        public bool VerificaInscricaoComDocumentoCadastrado(long seqDocumentoRequerido)
        {
            return DocumentoRequeridoDomainService.VerificaInscricaoComDocumentoCadastrado(seqDocumentoRequerido);
        }

        public DateTime? BuscarDataLimiteEntregaDocumentoRequerido(long seqDocumentoRequerido)
        {
            var documentoRequerido = this.DocumentoRequeridoDomainService.SearchProjectionByKey(new SMCSeqSpecification<DocumentoRequerido>(seqDocumentoRequerido), d => new
            {
                d.Seq,
                d.SeqConfiguracaoEtapa,
                d.SeqTipoDocumento,
                d.DataLimiteEntrega
            });

            if (documentoRequerido.DataLimiteEntrega.HasValue)
                return documentoRequerido.DataLimiteEntrega.Value;
            else
            {
                var specGrupoDocumentoRequerido = new GrupoDocumentoRequeridoFilterSpecification() 
                { 
                    SeqConfiguracaoEtapa = documentoRequerido.SeqConfiguracaoEtapa, 
                    SeqDocumentoRequerido = documentoRequerido.Seq,
                    SeqTipoDocumento = documentoRequerido.SeqTipoDocumento
                };

                var gruposDocumentoRequerido = this.GrupoDocumentoRequeridoDomainService.SearchProjectionBySpecification(specGrupoDocumentoRequerido, g => new 
                {
                    g.Seq,
                    g.DataLimiteEntrega
                }).ToList();

                if (gruposDocumentoRequerido.Any())
                    return gruposDocumentoRequerido.FirstOrDefault().DataLimiteEntrega;


            }

            return null;
            
        }
    }
}
