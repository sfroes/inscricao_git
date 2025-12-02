namespace SMC.Inscricoes.Common
{
    public class TOKENS
    {
        #region Tokens situação

        public const string SITUACAO_INSCRICAO_INICIADA = "INSCRICAO_INICIADA";
        public const string SITUACAO_INSCRICAO_FINALIZADA = "INSCRICAO_FINALIZADA";
        public const string SITUACAO_INSCRICAO_CONFIRMADA = "INSCRICAO_CONFIRMADA";
        public const string SITUACAO_INSCRICAO_INDEFERIDA = "INSCRICAO_INDEFERIDA";
        public const string SITUACAO_INSCRICAO_DEFERIDA = "INSCRICAO_DEFERIDA";
        public const string SITUACAO_INSCRICAO_CANCELADA = "INSCRICAO_CANCELADA";
        public const string SITUACAO_CANDIDATO_CONFIRMADO = "CANDIDATO_CONFIRMADO";
        public const string SITUACAO_CANDIDATO_DESISTENTE = "CANDIDATO_DESISTENTE";
        public const string SITUACAO_CANDIDATO_REPROVADO = "CANDIDATO_REPROVADO";
        public const string SITUACAO_CANDIDATO_SELECIONADO = "CANDIDATO_SELECIONADO";
        public const string SITUACAO_CANDIDATO_EXCEDENTE = "CANDIDATO_EXCEDENTE";
        public const string SITUACAO_EXCEDENTE_SELECIONADO = "EXCEDENTE_SELECIONADO";
        public const string SITUACAO_CONVOCADO = "CONVOCADO";
        public const string SITUACAO_CONVOCADO_DESISTENTE = "CONVOCADO_DESISTENTE";
        public const string SITUACAO_CONVOCADO_CONFIRMADO = "CONVOCADO_CONFIRMADO";

        #endregion

        #region Tokens motivos
        public const string MOTIVO_INSCRICAO_CANCELADA_TESTE = "INSCRICAO_CANCELADA_TESTE";
        public const string MOTIVO_INSCRICAO_CANCELADA_DEBITO_FINANCEIRO = "INSCRICAO_CANCELADA_DEBITO_FINANCEIRO";
        public const string MOTIVO_INSCRICAO_CANCELADA_VINCULO_ACADEMICO = "INSCRICAO_CANCELADA_VINCULO_ACADEMICO";
        public const string MOTIVO_CONVOCADO_DESISTENTE_NAO_MATRICULADO = "CONVOCADO_DESISTENTE_NAO_MATRICULADO";
        #endregion

        #region Tokens etapas

        public const string ETAPA_INSCRICAO = "INSCRICAO";
        public const string ETAPA_SELECAO = "SELECAO";
        public const string ETAPA_CONVOCACAO = "CONVOCACAO";

        #endregion

        #region Tokens pagina

        public const string PAGINA_INSTRUCOES_INICAIS = "INSTRUCOES_INICIAIS";
        public const string PAGINA_CONFIRMACAO_DADOS_INSCRITO = "CONFIRMACAO_DADOS_INSCRITO";
        public const string PAGINA_SELECAO_OFERTA = "SELECAO_OFERTA";
        public const string PAGINA_CODIGO_AUTORIZACAO = "CODIGO_AUTORIZACAO";
        public const string PAGINA_FORMULARIO_INSCRICAO = "FORMULARIO_INSCRICAO";
        public const string PAGINA_UPLOAD_DOCUMENTOS = "UPLOAD_DOCUMENTOS";
        public const string PAGINA_CONFIRMACAO_INSCRICAO = "CONFIRMACAO_INSCRICAO";
        public const string PAGINA_INSTRUCOES_FINAIS = "INSTRUCOES_FINAIS";
        public const string PAGINA_COMPROVANTE_INSCRICAO = "COMPROVANTE_INSCRICAO";

        #endregion

        #region Tokens de seção

        public const string SECAO_ARQUIVOS = "ARQUIVOS";
        public const string SECAO_CONTROLE_SECRETARIA = "CONTROLE_SECRETARIA";
        public const string SECAO_FOTO = "FOTO";
        public const string SECAO_INSTRUCOES = "INSTRUCOES";
        public const string SECAO_INSTRUCOES_TAXA = "INSTRUCOES_TAXA";
        public const string SECAO_PROTOCOLO_ENTREGA = "PROTOCOLO_ENTREGA";
        public const string SECAO_RODAPE = "RODAPE";

        #endregion

        #region Tokens para notificação

        public const string NOTIFICACAO_FINALIZAR_INSCRICAO = "INSCRICAO_FINALIZADA";

        public const string NOTIFICACAO_CONFIRMAR_INSCRICAO = "INSCRICAO_CONFIRMADA";

        public const string NOTIFICACAO_LIBERACAO_ALTERACAO_INSCRICAO = "LIBERACAO_ALTERACAO_INSCRICAO";

        public const string NOTIFICACAO_NOVA_ENTREGA_DOCUMENTACAO = "NOVA_ENTREGA_DOCUMENTACAO";
        
        public const string RECEBIMENTO_BOLSA = "RECEBIMENTO_BOLSA";

        public const string FORMULARIO_AVALIACAO = "FORMULARIO_AVALIACAO";

        #endregion

        #region Tokens para campos SGF

        public const string CAMPO_TITULO_ELEITOR_NUMERO = "TITULO_ELEITOR_NUMERO";
        public const string CAMPO_TITULO_ELEITOR_ZONA = "TITULO_ELEITOR_ZONA";
        public const string CAMPO_TITULO_ELEITOR_SECAO = "TITULO_ELEITOR_SECAO";
        public const string CAMPO_TITULO_ELEITOR_ESTADO_EMISSOR = "TITULO_ELEITOR_ESTADO_EMISSOR";

        public const string CAMPO_PIS_PASEP_TIPO = "PIS_PASEP_TIPO";
        public const string CAMPO_PIS_PASEP_NUMERO = "PIS_PASEP_NUMERO";
        public const string CAMPO_PIS_PASEP_DATA = "PIS_PASEP_DATA";

        public const string CAMPO_DOCUMENTO_MILITAR_NUMERO = "DOCUMENTO_MILITAR_NUMERO";
        public const string CAMPO_DOCUMENTO_MILITAR_CSM = "DOCUMENTO_MILITAR_CSM";
        public const string CAMPO_DOCUMENTO_MILITAR_TIPO = "DOCUMENTO_MILITAR_TIPO";
        public const string CAMPO_DOCUMENTO_MILITAR_ESTADO = "DOCUMENTO_MILITAR_ESTADO";

        public const string CAMPO_PORTADOR_NECESSIDADES_ESPECIAIS = "PORTADOR_NECESSIDADES_ESPECIAIS";
        public const string CAMPO_TIPO_NECESSIDADE_ESPECIAL = "TIPO_NECESSIDADE_ESPECIAL";

        public const string CAMPO_NIVEL_ENSINO = "NIVEL_ENSINO";
        public const string CAMPO_RACA_COR = "RACA_COR";

        public const string CAMPO_AVALISTA_FIADOR_CPF = "AVALISTA_FIADOR_CPF";
        public const string CAMPO_AVALISTA_FIADOR_NOME = "AVALISTA_FIADOR_NOME";

        public const string CAMPO_PROJETO = "PROJETO";

        #endregion

        #region Tokens resource
        public const string TOKEN_RESOURCE_INCRICAO = "INSCRICAO";
        public const string TOKEN_RESOURCE_AGENDAMENTO = "AGENDAMENTO";
        public const string TOKEN_RESOURCE_ENTREGA_DOCUMENTACAO = "ENTREGA_DE_DOCUMENTACAO";
        #endregion

        #region Tokens Tipo Documento GED
        public const string TOKEN_TIPO_DOCUMENTO_CPF = "CPF_FRENTE_E_VERSO";
        public const string TOKEN_TIPO_DOCUMENTO_IDENTIDADE = "CARTEIRA_DE_IDENTIDADE_FRENTE_E_VERSO";
        public const string TOKEN_TIPO_DOCUMENTO_PASSAPORTE = "PASSAPORTE";
        public const string TOKEN_TIPO_DOCUMENTO_COMPROVANTE_INSCRICAO = "COMPROVANTE_DE_INSCRICAO";
        public const string TOKEN_TIPO_DOCUMENTO_CERTIFICADO_PARTICIPACAO = "CERTIFICADO_PARTICIPACAO";
        public const string TOKEN_TIPO_DOCUMENTO_DECLARACAO_PARTICIPACAO = "DECLARACAO_PARTICIPACAO";
        #endregion
    }
}
