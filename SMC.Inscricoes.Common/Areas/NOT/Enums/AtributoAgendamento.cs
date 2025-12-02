using SMC.Framework;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.Common.Areas.NOT.Enums
{
	[DataContract(Namespace = NAMESPACES.MODEL)]
    public enum AtributoAgendamento : short
	{
		[EnumMember]
        [SMCIgnoreValue]
		[Description("")]
		Nenhum = 0,

		[EnumMember]
        [Description("Data da Inscrição no Processo Seletivo")]
		DataInscricao = 1,

		[EnumMember]
        [Description("Data de Término das Inscrições do Processo Seletivo")]
		DataFimEtapaInscricao = 2,

		[EnumMember]
        [Description("Data de Vencimento do Boleto")]
		DataVencimentoBoleto = 3,

        [EnumMember]
        [Description("Data Limite da Entrega da Documentação")]
        DataLimiteEntregaDocumentacao = 4,

		[EnumMember]
		[Description("Prazo para a nova entrega da documentação")]
		PrazoParaNovaEntregaDaDocumentacao = 5,

		[EnumMember]
		[Description("Data de início do formulário do evento")]
		DataInicioFormularioEvento = 6,

		[EnumMember]
		[Description("Data de fim do formulário do evento")]
		DataFimFormularioEvento = 7

	}
}
  