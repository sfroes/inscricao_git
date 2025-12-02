using SMC.Formularios.Common.Areas.FRM.Includes;
using SMC.Formularios.ServiceContract.Areas.FRM.Data;
using SMC.Formularios.ServiceContract.Areas.FRM.Interfaces;
using SMC.Formularios.ServiceContract.FRM.Data;
using SMC.Framework.Domain;
using SMC.Framework.Extensions;
using SMC.Framework.Model;
using SMC.Framework.Specification;
using SMC.Inscricoes.Common.Areas.RES.Exceptions;
using SMC.Inscricoes.Domain.Areas.INS.DomainServices;
using SMC.Inscricoes.Domain.Areas.RES.Models;
using SMC.Inscricoes.Domain.Areas.RES.ValueObjects;
using System.Collections.Generic;
using System.Linq;

namespace SMC.Inscricoes.Domain.Areas.RES.DomainServices
{
    public class UnidadeResponsavelTipoFormularioDomainService : InscricaoContextDomain<UnidadeResponsavelTipoFormulario>
    {
        #region DomainServices
        private IFormularioService FormularioService
        {
            get { return this.Create<IFormularioService>(); }
        }

        private ConfiguracaoEtapaPaginaIdiomaDomainService ConfiguracaoEtapaPaginaIdiomaDomainService
        {
            get { return this.Create<ConfiguracaoEtapaPaginaIdiomaDomainService>(); }
        }
        #endregion

        public List<UnidadeResponsavelTipoFormularioVO> BuscarUnidadeResponsavelTiposFormularios(long seqUnidadeResponsavel)
        {
            var spec = new SMCPropertySpecification<UnidadeResponsavelTipoFormulario>("SeqUnidadeResponsavel", seqUnidadeResponsavel);

            var itens = this.SearchProjectionBySpecification(spec,
                 x => new UnidadeResponsavelTipoFormularioVO
                 {
                     Seq = x.Seq,
                     SeqUnidadeResponsavel = seqUnidadeResponsavel,
                     SeqTipoFormularioSGF = x.SeqTipoFormularioSGF,
                     Ativo = x.Ativo
                 }).ToList();

            long[] seqSfg = itens.Select(f => f.SeqTipoFormularioSGF).ToArray();

            var filtroTipoForm = new TipoFormularioKeyValueFiltroData() { SeqsTipoFormulario = seqSfg};
            var descricaoFormularios = FormularioService.BuscarTipoFormularioKeyValue(filtroTipoForm);

            foreach (var item in itens)
            {
                item.DescricaoTipoFormulario = descricaoFormularios.Where(f => f.Seq == item.SeqTipoFormularioSGF).Select(f => f.Descricao).FirstOrDefault();
            }

            return itens.OrderBy(o => o.DescricaoTipoFormulario).ToList();
        }

        public long Salvar(UnidadeResponsavelTipoFormulario unidadeResponsavelTipoFormulario)
        {
            if (unidadeResponsavelTipoFormulario.Seq != default(long))
            {
                var spec = new SMCSeqSpecification<UnidadeResponsavelTipoFormulario>(unidadeResponsavelTipoFormulario.Seq);
                var tipoFormularioUnidadeResponsavel = this.SearchByKey(spec);

                if (tipoFormularioUnidadeResponsavel.SeqTipoFormularioSGF != unidadeResponsavelTipoFormulario.SeqTipoFormularioSGF
                    && !tipoFormularioUnidadeResponsavel.Ativo)
                {
                    string unidadeResponsavel;
                    if (VerificaTipoFormularioEmUso(unidadeResponsavelTipoFormulario.Seq, out unidadeResponsavel))
                    {
                        throw new UnidadeResponsavelTipoFormularioAlterException(unidadeResponsavel);
                    }
                }
            }

            this.SaveEntity(unidadeResponsavelTipoFormulario);
            return unidadeResponsavelTipoFormulario.Seq;
        }

        public void Excluir(long seqTipoFormularioUnidadeResponsavel)
        {
            string unidadeResponsavel;
            if (VerificaTipoFormularioEmUso(seqTipoFormularioUnidadeResponsavel, out unidadeResponsavel))
            {
                throw new UnidadeResponsavelTipoFormularioExcludeException(unidadeResponsavel);
            }            

            this.DeleteEntity<UnidadeResponsavelTipoFormulario>(seqTipoFormularioUnidadeResponsavel);
        }

        private bool VerificaTipoFormularioEmUso(long seqConfiguracaoTipoFormulario, out string unidadeResponsavel)
        {
            unidadeResponsavel = null;

            var spec = new SMCSeqSpecification<UnidadeResponsavelTipoFormulario>(seqConfiguracaoTipoFormulario);
            var entity = this.SearchProjectionBySpecification(spec,
                                x => new
                                {
                                    Nome = x.UnidadeResponsavel.Nome,
                                    SeqTipoFormulario = x.SeqTipoFormularioSGF,
                                    SeqSGF = x.UnidadeResponsavel.Processos.SelectMany(
                                                p => p.EtapasProcesso.SelectMany(
                                                    e => e.Configuracoes.SelectMany(
                                                        c => c.Paginas.SelectMany(
                                                            a => a.Idiomas.Where(w => w.SeqFormularioSGF != null).Select(
                                                                i => i.SeqFormularioSGF)))))
                                }).First();

            //var formularios = FormularioService.BuscarFormulariosPorCodigos(entity.SeqSGF.Select(f => f.Value).ToArray());
            var formularios = FormularioService.BuscarFormularios(new FormularioFiltroData() { Seqs = entity.SeqSGF.Select(f => f.Value).ToList<long>() },IncludesFormulario.Nenhum);



            if (formularios.Any(f => f.SeqTipoFormulario == entity.SeqTipoFormulario))
            {
                unidadeResponsavel = entity.Nome;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Busca a lista de tipos de formulário associados a unidade responsável
        /// </summary>
        public List<SMCDatasourceItem> BuscarTiposFormularioKeyValue(long seqUnidadeResponsavel)
        {
            var spec = new SMCPropertySpecification<UnidadeResponsavelTipoFormulario>("SeqUnidadeResponsavel", seqUnidadeResponsavel);
            var seqTiposSGF=this.SearchProjectionBySpecification(spec,
                x=> x.SeqTipoFormularioSGF);
            if (seqTiposSGF.Count() > 0)
            {
                return this.FormularioService.BuscarTipoFormularioKeyValue(
                    new TipoFormularioKeyValueFiltroData
                    {
                        SeqsTipoFormulario = seqTiposSGF
                    });
            }
            else
            {
                return new List<SMCDatasourceItem>();
            }
        }
    }
}
