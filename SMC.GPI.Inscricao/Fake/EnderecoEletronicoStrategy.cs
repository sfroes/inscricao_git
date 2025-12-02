using SMC.Framework.Fake;
using SMC.GPI.Inscricao.Models;
using System.Collections.Generic;
using System.Reflection;

namespace SMC.GPI.Inscricao.Fake
{
    public class EnderecoEletronicoStrategy : SMCFakeStrategyBase
    {
        /// <summary>
        /// Define a prioridade da estratégia de fake
        /// </summary>
        public override int Priority
        {
            get { return 99; }
        }

        protected override bool CheckProperty(PropertyInfo propertyInfo)
        {
            return propertyInfo.PropertyType == typeof(List<EnderecoEletronicoViewModel>);
        }

        protected override object CreateProperty(PropertyInfo propertyInfo)
        {
            return GeraListaEnderecoEletronico();
        }

        private List<EnderecoEletronicoViewModel> GeraListaEnderecoEletronico()
        {
            List<EnderecoEletronicoViewModel> lista = new List<EnderecoEletronicoViewModel>();

            for (int i = 0; i < 2; i++)
            {
                lista.Add(
                    new EnderecoEletronicoViewModel()
                    {
                        //TipoEnderecoEletronico = SMCFakeHelper.Random<TipoEnderecoEletronico>(1,4),
                        Descricao = SMCFakeHelper.RandomListElement<string>(FakeConfig.ENDERECOS_ELETRONICOS)
                    }
                );
            }

            return lista;
        }
    }
}