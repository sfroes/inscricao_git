using SMC.Inscricoes.Common.Enums;
using SMC.Inscricoes.Pdf.Invoke;
using SMC.Inscricoes.Rest.Models;

namespace SMC.Inscricoes.Rest.Invoke
{
    public static class SauntinsoftResquest
    {
        public static T Send<T>(object value, MetodoHttp metodoHttp, string rota) where T : ResultBase
        {
            var sautinSoftInvoke = new SautinsoftInvokeHttp();
            return sautinSoftInvoke.Send<T>(value, metodoHttp, rota);
        }
    }
}
