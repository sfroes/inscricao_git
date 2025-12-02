using SMC.Framework;
using SMC.Framework.Mapper;
using SMC.Framework.Model;
using SMC.Inscricoes.Common.Areas.INS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    /// <summary>
    /// Value Objec para representar uma inscrição em um processo com sua situação
    /// usado para listagem de situção de inscrição no processo
    /// </summary>
    public class InscricaoDocumentoGruposVO : InscricaoDocumentoVO
    {
        public InscricaoDocumentoGruposVO()
        {

        }

        public InscricaoDocumentoGruposVO(InscricaoDocumentoVO documento)
        {
            this.Seq = documento.Seq;
            this.SeqArquivoAnexado = documento.SeqArquivoAnexado;
            this.SeqDocumentoRequerido = documento.SeqDocumentoRequerido;            
            this.Observacao = documento.Observacao;
            this.SeqInscricao = documento.SeqInscricao;
            this.SeqTipoDocumento = documento.SeqTipoDocumento;
            this.SituacaoEntregaDocumento = documento.SituacaoEntregaDocumento;
            this.TipoDocumentoPermiteVariosArquivos = documento.TipoDocumentoPermiteVariosArquivos;
            this.UploadObrigatorio = documento.UploadObrigatorio;
            this.VersaoDocumento = documento.VersaoDocumento;
            this.VersaoDocumentoExigido = documento.VersaoDocumentoExigido;
            this.ArquivoAnexado = documento.ArquivoAnexado;
            this.DataEntrega = documento.DataEntrega;
            this.DescricaoArquivoAnexado = documento.DescricaoArquivoAnexado;
            this.DescricaoTipoDocumento = documento.DescricaoTipoDocumento;
            this.FormaEntregaDocumento = documento.FormaEntregaDocumento;
            this.ExibeTermoResponsabilidadeEntrega = documento.ExibeTermoResponsabilidadeEntrega;
            this.EntregaPosterior = documento.EntregaPosterior;
            this.DataLimiteEntrega = documento.DataLimiteEntrega;
    }

        public string DescricaoGrupoDocumentos { get; set; }

        public long SeqGrupoDocumentoRequerido { get; set; }

        public List<SMCDatasourceItem> DocumentosRequeridosGrupo { get; set; }
    }
}

