using SMC.Framework.Fake;
using SMC.Framework.Model;
using SMC.GPI.Administrativo.Areas.INS.Models;
using System;
using System.Collections.Generic;

namespace SMC.GPI.Administrativo.Areas.INS.Fake
{
	public class ProcessoPorGrupoOfertaStrategy : SMCFakeStrategyBase
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
			return !type.IsAbstract && type == typeof(SMCPagerData<ConsultaConsolidadaGrupoOfertaListaViewModel>);
        }

        protected override object CreateType(Type type)
        {
            return CreatePager();
        }

        private object CreatePager()
        {
			SMCPagerData<ConsultaConsolidadaGrupoOfertaListaViewModel> pager = new SMCPagerData<ConsultaConsolidadaGrupoOfertaListaViewModel>()
            {
				Itens = new List<ConsultaConsolidadaGrupoOfertaListaViewModel>()
                {				
                    new ConsultaConsolidadaGrupoOfertaListaViewModel()
                    {
						Descricao = SMCFakeHelper.RandomListElement(FakeConfig.GRUPO_OFERTA),
						Seq = 1, SeqProcesso = 1,
						DocumentacoesEntregues = 231,
						Finalizadas = 889,
						Confirmadas = 56,
						Pagas = 5,
						Deferidos = 3,
						NaoConfirmadas = 233,
						Indeferidos = 4,
                        Canceladas = 2,
						PosicoesConsolidadasOfertas = new List<ConsultaConsolidadaOfertaListaViewModel>()
						{
							new ConsultaConsolidadaOfertaListaViewModel()
							{
								Seq = 1,								
								DocumentacoesEntregues = 231,
								Finalizadas = 889,
								Confirmadas = 56,
								Pagas = 5,
								Deferidos = 3,
								NaoConfirmadas = 233,
								Indeferidos = 4,
                                Canceladas = 1,
								Descricao = SMCFakeHelper.RandomListElement(FakeConfig.OFERTA)
							}, 
							new ConsultaConsolidadaOfertaListaViewModel()
							{
								Seq = 2,
								DocumentacoesEntregues = 231,
								Finalizadas = 889,
								Confirmadas = 56,
								Pagas = 5,
								Deferidos = 3,
								NaoConfirmadas = 233,
								Indeferidos = 4,
                                Canceladas = 1,
								Descricao = SMCFakeHelper.RandomListElement(FakeConfig.OFERTA)
							} 
						} 
                    }, 

					new ConsultaConsolidadaGrupoOfertaListaViewModel()
                    {
						Descricao = SMCFakeHelper.RandomListElement(FakeConfig.GRUPO_OFERTA),
						Seq = 1, SeqProcesso = 1,
						DocumentacoesEntregues = 231,
						Finalizadas = 889,
						Confirmadas = 56,
						Pagas = 5,
						Deferidos = 3,
						NaoConfirmadas = 233,
						Indeferidos = 4,
                        Canceladas = 2,
						PosicoesConsolidadasOfertas = new List<ConsultaConsolidadaOfertaListaViewModel>()
						{
							new ConsultaConsolidadaOfertaListaViewModel()
							{
								Seq = 2,
								DocumentacoesEntregues = 231,
								Finalizadas = 889,
								Confirmadas = 56,
								Pagas = 5,
								Deferidos = 3,
								NaoConfirmadas = 233,
								Indeferidos = 4,
                                Canceladas = 1,
								Descricao = SMCFakeHelper.RandomListElement(FakeConfig.OFERTA)
							}  
						} 
                    } 
                } 
            }; 

            return pager;
        }
    }
}