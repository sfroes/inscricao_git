using SMC.Framework.Service;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using System.Collections.Generic;
using System.Linq;
using SMC.Framework.Model;
using SMC.Inscricoes.ServiceContract.Areas.INS.Data;
using SMC.Inscricoes.Domain.Areas.INS.DomainServices;
using SMC.Inscricoes.Domain.Areas.INS.Specifications;
using SMC.Framework.Extensions;
using SMC.Inscricoes.Domain.Areas.INS.ValueObjects;
using SMC.Framework.Specification;
using SMC.Inscricoes.Domain.Areas.INS.Models;

namespace SMC.Inscricoes.Service.Areas.INS.Services
{
    public class InscricaoForaPrazoService : SMCServiceBase, IInscricaoForaPrazoService
    {
        #region DomainServices
        private PermissaoInscricaoForaPrazoDomainService PermissaoInscricaoForaPrazoDomainService
        {
            get { return this.Create<PermissaoInscricaoForaPrazoDomainService>(); }
        }
        #endregion

        public SMCPagerData<InscricaoForaPrazoListaData> BuscarInscricoesForaPrazo(InscricaoForaPrazoFiltroData filtroData)
        {
            int total;
            var lista = PermissaoInscricaoForaPrazoDomainService.SearchProjectionBySpecification(filtroData.Transform<PermissaoInscricaoForaPrazoCoincidenteSpecification>(),
                        f => new InscricaoForaPrazoListaData
                        {
                            Seq = f.Seq,                            
                            DataInicio = f.DataInicio,
                            DataFim = f.DataFim,
                            DataVencimento = f.DataVencimento
                        }, out total);

            return new SMCPagerData<InscricaoForaPrazoListaData>(lista, total);
        }

        public PermissaoInscricaoForaPrazoData BuscarInscricaoForaPrazo(long seq)
        {
            var permissao = PermissaoInscricaoForaPrazoDomainService.SearchProjectionByKey(new SMCSeqSpecification<PermissaoInscricaoForaPrazo>(seq),
                        f => new PermissaoInscricaoForaPrazoData
                        {
                            Seq = f.Seq,
                            SeqProcesso = f.SeqProcesso,
                            Inscritos = f.Inscritos.SelectMany(x => new List<LookupInscritoData>()
                                        {
                                            new LookupInscritoData()
                                            {
                                                Seq = x.Seq,
                                                SeqInscrito = x.SeqInscrito,
                                                Nome = string.IsNullOrEmpty(x.Inscrito.NomeSocial) ? x.Inscrito.Nome : x.Inscrito.NomeSocial + " (" + x.Inscrito.Nome + ")",
                                                DataNascimento = x.Inscrito.DataNascimento,
                                                CPF = x.Inscrito.Cpf,
                                                NumeroPassaporte = x.Inscrito.NumeroPassaporte
                                            }
                                        }),
                            DataInicio = f.DataInicio,
                            DataFim = f.DataFim,
                            DataVencimento = f.DataVencimento
                        });
            if (permissao != null)
                permissao.SeqInscritoEdicao = permissao.Inscritos.First().SeqInscrito;
            return permissao;
        }

        public void SalvarPermissoes(PermissaoInscricaoForaPrazoData data)
        {
            PermissaoInscricaoForaPrazoDomainService.SalvarPermissoes(data.Transform<PermissaoInscricaoForaPrazoVO>());            
        }

        public void ExcluirPermissao(long seq)
        {
            PermissaoInscricaoForaPrazoDomainService.DeletarPermissao(seq);
        }
    }
}
