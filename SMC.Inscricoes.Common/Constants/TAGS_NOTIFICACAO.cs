using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Common
{
    public class TAGS_NOTIFICACAO
    {

        #region Inscrito

        public const string NOME_INSCRITO = "{{NOM_INSCRITO}}";
        public const string NOME_SOCIAL_INSCRITO = "{{NOM_SOCIAL_INSCRITO}}";

        #endregion

        #region Inscrição

        public const string NUMERO_INSCRICAO = "{{NUM_INSCRICAO}}";
        public const string MOTIVO_INDEFERIMENTO = "{{DSC_MOTIVO_INDEFERIMENTO}}";
        public const string DESCRICAO_OFERTAS = "{{DSC_OFERTA}}";
        public const string DSC_PROCESSO = "{{DSC_PROCESSO}}";
        public const string DATA_FIM_ETAPA = "{{DAT_FIM_ETAPA}}";

        #endregion

        #region Processo

        public const string NUMERO_EDITAL = "{{NUM_EDITAL}}";
        public const string DATA_FIM_OFERTA = "{{DAT_FIM_OFERTA}}";

        #endregion

        #region Unidade Responsável

        public const string NOME_UNIDADE_RESPONSAVEL = "{{NOM_UNIDADE_RESPONSAVEL}}";
        public const string ENDERECO_UNIDADE_RESPONSAVEL = "{{END_UNIDADE_RESPONSAVEL}}";
        public const string TELEFONE_UNIDADE_RESPONSAVEL = "{{TEL_UNIDADE_RESPONSAVEL}}";
        public const string EMAIL_UNIDADE_RESPONSAVEL = "{{END_ELETRONICO_UNIDADE_RESPONSAVEL}}";

        #endregion

        #region Entrega Documentacao

        public const string DATA_ENTREGA_DOCUMENTACAO = "{{DAT_ENTREGA_DOCUMENTACAO}}";
        public const string DESCRICAO_DOCUMENTACAO = "{{DSC_DOCUMENTACAO}}";
        public const string DAT_PRAZO_NOVA_ENTREGA_DOCUMENTACAO = "{{DAT_PRAZO_NOVA_ENTREGA_DOCUMENTACAO}}";
        public const string DSC_LISTA_DOCUMENTACAO_INDEFERIDA = "{{DSC_LISTA_DOCUMENTACAO_INDEFERIDA}}";
        public const string DSC_LISTA_DOCUMENTACAO_PENDENTE = "{{DSC_LISTA_DOCUMENTACAO_PENDENTE}}";

        #endregion

        #region Titulo

        public const string DATA_VENCIMENTO_TITULO = "{{DAT_VENCIMENTO_BOLETO}}";

        #endregion

        #region Oferta

        public const string VALOR_PERCENTUAL_DESCONTO = "{{VAL_PERCENTUAL_DESCONTO}}";

        #endregion
    }
}
