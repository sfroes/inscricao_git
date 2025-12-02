using SMC.Framework.Fake;
using SMC.Formularios.UI.Mvc.Models;

namespace SMC.GPI.Inscricao.Fake
{
    public class DadoFormularioViewModelStrategy : SMCFakeStrategyBase
    {
        public override int Priority
        {
            get { return 99; }
        }

        protected override bool CheckMethod(System.Reflection.MethodInfo methodInfo)
        {
            return methodInfo.ReturnType == typeof(DadoFormularioViewModel);
        }

        protected override object CreateMethod(System.Reflection.MethodInfo methodInfo)
        {
            return null;
        }
    }
}
