using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using SMC.Framework.Mapper;
using SMC.Inscricoes.Common.Areas.INS;
using SMC.Inscricoes.Common.Areas.INS.Enums;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using SMC.Seguranca.ServiceContract.Areas.USU.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SMC.Inscricoes.Domain.Areas.INS.ValueObjects
{
    public class DadosDocumentacaoInscricaoVO : ISMCMappable
    {
        #region TagsValidas
        public string NomeInscrito { get; set; }
        public string Processo { get; set; }
        public string DataInicioAtividades { get; set; }
        public string DataInicioEvento { get; set; }
        public string DataFimEvento { get; set; }
        public string AnoReferencia { get; set; }
        public string SemestreReferencia { get; set; }
        public string Edital { get; set; }
        public string AreaConcentracao { get; set; }
        public string LinhaPesquisa { get; set; }
        public string Orientador { get; set; }
        public string Departamento { get; set; }
        public string Vaga { get; set; }
        public string Curso { get; set; }
        public string Disciplina { get; set; }
        public string Unidade { get; set; }
        public string Data { get; set; }
        public string DataAtual { get; set; }
        public string Atividade { get; set; }
        public string ListaOfertas { get; set; }
        public string CargaHoraria { get; set; }
        public string CargaHorariaTotal { get; set; }
        public string GrupoOferta { get; set; }
        public string Classificacao { get; set; }
        public string UnidadeResponsavel { get; set; }
        public string EnderecoUnidadeResponsavel { get; set; }
        public string TelefoneUnidadeResponsavel { get; set; }
        public string EmailUnidadeResponsavel { get; set; }
        #endregion

        #region DadosInscricao
        public string DocumentoTokenTipoDocumento {  get; set; }
        public bool DocumentoAssinaturaEletronica { get; set; }
        public string DocumentoTokenConfiguracaoGAD { get; set; }
        public bool DocumentoRequerCheckin { get; set; }
        public bool DocumentoExibeHome { get; set; }
        public long SeqConfiguracaoModeloDocumento { get; set; }
        public bool ProcessoPossuiDeferimento { get; set; }
        public long SeqInscricao { get; set; }
        public long SeqTemplateProcessoSGF { get; set;}
        #endregion
    }
}
