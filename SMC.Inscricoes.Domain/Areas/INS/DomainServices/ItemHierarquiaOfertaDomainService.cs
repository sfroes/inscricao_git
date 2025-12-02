using SMC.Framework.Domain;
using SMC.Framework.Specification;
using SMC.Framework.UnitOfWork;
using SMC.Inscricoes.Common.Areas.INS.Exceptions;
using SMC.Inscricoes.Domain.Areas.INS.Models;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using SMC.Inscricoes.Domain.Areas.INS.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Inscricoes.Domain.Areas.INS.DomainServices
{
    public class ItemHierarquiaOfertaDomainService : InscricaoContextDomain<ItemHierarquiaOferta>
    {
        #region DomainServices

        private TipoHierarquiaOfertaDomainService TipoHierarquiaOfertaDomainService
        {
            get
            {
                return this.Create<TipoHierarquiaOfertaDomainService>();
            }
        }

        private ProcessoDomainService ProcessoDomainService
        {
            get
            {
                return this.Create<ProcessoDomainService>();
            }
        }

        #endregion

        /// <summary>
        /// Salva um item da Hierarquia de Oferta
        /// </summary>        
        public long SalvarItemHierarquiaOferta(ItemHierarquiaOferta item)
        {            
            ValidarAlteracaoArvoreHierarquiaOfertas(item.SeqTipoHierarquiaOferta);
            this.SaveEntity(item);
            return item.Seq;
        }


        /// <summary>
        /// Exclui um item da árvore de tipo de hieraquia de oferta em cascata
        /// </summary>        
        public void ExcluirItemHierarquiaOferta(long seqItemHierarquiaOferta)
        {
            var item = this.SearchByDepth(new SMCSeqSpecification<ItemHierarquiaOferta>(seqItemHierarquiaOferta),
                10, x => x.ItensHierarquiaOfertaFihos).FirstOrDefault();
            ValidarAlteracaoArvoreHierarquiaOfertas(item.SeqTipoHierarquiaOferta);
            //Teste e rodar excluir em arvore
            using (var unitOfWork = SMCUnitOfWork.Begin())
            {
                try
                {
                    ExcluirItem(item);
                    unitOfWork.Commit();
                }
                catch (Exception)
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }
        /// <summary>
        /// Retorna a hierarquia de itens de hierarquia de oferta
        /// </summary>
        /// <param name="seqProcesso">Sequencial do processo</param>
        /// <returns></returns>
        public List<ItemHierarquiaOfertaArvoreVO> BuscarItensHierarquiaOfertaArvore(long seqProcesso) 
        {
            var seqTipoHierarquiaOferta = ProcessoDomainService.SearchProjectionByKey(new SMCSeqSpecification<Processo>(seqProcesso), p => p.SeqTipoHierarquiaOferta);

            var specItemHierarquiaOferta = new ItemHierarquiaOfertaFilterSpecification() {SeqProcesso = seqProcesso, SeqTipoHierarquiaOferta = seqTipoHierarquiaOferta };

            specItemHierarquiaOferta.SetOrderBy(x => x.Seq);

            return SearchProjectionBySpecification(specItemHierarquiaOferta, i => new ItemHierarquiaOfertaArvoreVO()
            {
                Seq = i.Seq,
                SeqPai = i.SeqPai,
                Descricao = i.TipoItemHierarquiaOferta.Descricao,
                Token = i.TipoItemHierarquiaOferta.Token
            }).ToList();
        }


        /// <summary>
        /// Verifica se é possível alterar o registro
        /// </summary>        
        private void ValidarAlteracaoArvoreHierarquiaOfertas(long seqTipoHierarquiaOferta)
        {
            var spec = new TipoHierarquiaOfertaFilterSpecification
            {
                Seq = seqTipoHierarquiaOferta
            };
            var associado = TipoHierarquiaOfertaDomainService.SearchProjectionByKey(spec, x => x.Processo.Any());
            if (associado)
            {
                throw new TipoHierarquiaOfertaAssociadoException();
            }
        }

        /// <summary>
        /// Exclui o item e seus filhos em cascata de forma recursiva
        /// </summary>        
        private void ExcluirItem(ItemHierarquiaOferta item)
        {
            if (item.ItensHierarquiaOfertaFihos != null && item.ItensHierarquiaOfertaFihos.Count > 0)
            {
                foreach (var filho in item.ItensHierarquiaOfertaFihos)
                {
                    ExcluirItem(filho);
                }
            }
            this.DeleteEntity(item);
        }




    }
}
