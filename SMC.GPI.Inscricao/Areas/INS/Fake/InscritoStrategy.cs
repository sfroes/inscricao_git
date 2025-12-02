using SMC.Framework;
using SMC.Framework.Fake;
using SMC.Framework.Model;
using SMC.GPI.Inscricao.Areas.INS.Models;
using SMC.GPI.Inscricao.Models;
using SMC.Inscricoes.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace SMC.GPI.Inscricao.Areas.INS.Fake
{

	public class InscritoStrategy : SMCFakeStrategyBase
	{
		/// <summary>
		/// Define a prioridade da estratégia de fake
		/// </summary>
		public override int Priority
		{
			get { return 99; }
		}


		protected override bool CheckType(Type type)
		{
			return type == typeof(InscritoViewModel);
		}

		protected override object CreateType(Type type)
		{
			return Gerar();
		}

		private InscritoViewModel Gerar()
		{
			var pagina = new InscritoViewModel();
			 
			//pagina.
				 
			//private List<EnderecoEletronicoViewModel> GeraListaEnderecoEletronico()
			//{
			//	List<EnderecoEletronicoViewModel> lista = new List<EnderecoEletronicoViewModel>();

			//	for (int i = 0; i < FakeConfig.NUM_ITENS_MESTRE_DETALHE; i++)
			//	{
			//		lista.Add(
			//			new EnderecoEletronicoViewModel()
			//			{
			//				Tipo = SMCFakeHelper.Random<TipoEnderecoEletronico>(),
			//				Descricao = SMCFakeHelper.RandomListElement<string>(FakeConfig.ENDERECOS_ELETRONICOS)
			//			}
			//		);
			//	}

			//	return lista;
			//}

            return pagina;
		} 
	}
}   