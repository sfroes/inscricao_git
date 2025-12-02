using SMC.Framework;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace SMC.Inscricoes.Common.Areas.NOT.Enums
{
	[DataContract(Namespace = NAMESPACES.MODEL)]
    public enum Temporalidade : short
	{
		[EnumMember]
        [SMCIgnoreValue]
		[Description("")]
		Nenhum = 0,

		[EnumMember]
        [Description("Antes")]
        Antes = 1,

		[EnumMember]
        [Description("Após")]
        Apos = 2
    }
}
  