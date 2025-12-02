using SMC.Framework.Caching;
using SMC.Framework.Ioc;
using SMC.Framework.UI.Mvc;
using SMC.Inscricoes.ServiceContract.Areas.INS.Interfaces;
using System;
using System.Web.Mvc;

namespace SMC.Inscricoes.UI.Mvc.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class GoogleTagManagerFilterAttribute : ActionFilterAttribute
    {
        public static string TempDataKey = "__IdsTagManager";

        private string RecuperarTagsInscricao(long seqInscricao)
        {
            // Verifica se está no cache
            var keyCache = TempDataKey + "_inscricao_" + seqInscricao;
            string tags = SMCCacheManager.Get(keyCache)?.ToString();
            if (string.IsNullOrEmpty(tags))
            {
                using (var manager = new SMCContainerManager())
                {
                    using (var inscricaoService = manager.Create<IInscricaoService>())
                    {
                        var seqProcesso = inscricaoService.BuscarInscricaoResumida(seqInscricao).SeqProcesso;
                        using (var processoService = manager.Create<IProcessoService>())
                        {
                            var seqTipoProcesso = processoService.BuscarProcesso(seqProcesso).SeqTipoProcesso;
                            using (var tipoProcessoService = manager.Create<ITipoProcessoService>())
                            {
                                tags = tipoProcessoService.BuscarTipoProcesso(seqTipoProcesso).IdsTagManager;
                                SMCCacheManager.Add(keyCache, tags ?? "-1", new TimeSpan(12, 0, 0));
                            }
                        }
                    }
                }
            }
            return tags == "-1" ? null : tags;
        }

        private string RecuperarTagsConfiguracaoEtapa(long seqConfiguracaoEtapa)
        {
            // Verifica se está no cache
            var keyCache = TempDataKey + "_config_" + seqConfiguracaoEtapa;
            string tags = SMCCacheManager.Get(keyCache)?.ToString();
            if (string.IsNullOrEmpty(tags))
            {
                using (var manager = new SMCContainerManager())
                {
                    using (var configuracaoEtapaService = manager.Create<IConfiguracaoEtapaService>())
                    {
                        var seqProcesso = configuracaoEtapaService.BuscarConfiguracaoEtapa(seqConfiguracaoEtapa).SeqProcesso;
                        using (var processoService = manager.Create<IProcessoService>())
                        {
                            var seqTipoProcesso = processoService.BuscarProcesso(seqProcesso).SeqTipoProcesso;
                            using (var tipoProcessoService = manager.Create<ITipoProcessoService>())
                            {
                                tags = tipoProcessoService.BuscarTipoProcesso(seqTipoProcesso).IdsTagManager;
                                SMCCacheManager.Add(keyCache, tags ?? "-1", new TimeSpan(12, 0, 0));
                            }
                        }
                    }
                }
            }
            return tags == "-1" ? null : tags;
        }

        private string RecuperarTagsUidProcesso(Guid uidProcesso)
        {
            // Verifica se está no cache
            var keyCache = TempDataKey + "_uid_" + uidProcesso;
            string tags = SMCCacheManager.Get(keyCache)?.ToString();
            if (string.IsNullOrEmpty(tags))
            {
                using (var manager = new SMCContainerManager())
                {
                    using (var processoService = manager.Create<IProcessoService>())
                    {
                        var seqTipoProcesso = processoService.BuscarProcessoHome(uidProcesso, Framework.SMCLanguage.Portuguese, null).SeqTipoProcesso;
                        using (var tipoProcessoService = manager.Create<ITipoProcessoService>())
                        {
                            tags = tipoProcessoService.BuscarTipoProcesso(seqTipoProcesso).IdsTagManager;
                            SMCCacheManager.Add(keyCache, tags ?? "-1", new TimeSpan(12, 0, 0));
                        }
                    }
                }
            }
            return tags == "-1" ? null : tags;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Verifica se existe o sequencial do processo nos parâmetros da request
            var uidProcesso = RecuperarUidProcessoRequest(filterContext);
            if (uidProcesso.HasValue)
            {
                var tags = RecuperarTagsUidProcesso(uidProcesso.Value);
                if (!string.IsNullOrEmpty(tags))
                    filterContext.Controller.TempData[TempDataKey] = tags;
            }
            else
            {
                var seqConfiguracaoEtapa = RecuperarSeqConfiguracaoEtapaRequest(filterContext);
                if (seqConfiguracaoEtapa.HasValue)
                {
                    var tags = RecuperarTagsConfiguracaoEtapa(seqConfiguracaoEtapa.Value);
                    if (!string.IsNullOrEmpty(tags))
                        filterContext.Controller.TempData[TempDataKey] = tags;
                }
                else
                {
                    var seqOrigem = RecuperarOrigemEtapaRequest(filterContext);
                    if (seqOrigem.HasValue)
                    {
                        var tags = RecuperarTagsInscricao(seqOrigem.Value);
                        if (!string.IsNullOrEmpty(tags))
                            filterContext.Controller.TempData[TempDataKey] = tags;
                    }
                }
            }
        }

        private Guid? RecuperarUidProcessoRequest(ActionExecutingContext filterContext)
        {
            foreach (var key in filterContext.RequestContext.HttpContext.Request.Params.AllKeys)
            {
                if (key.ToLower() == "uidprocesso")
                {
                    Guid guid = Guid.Empty;
                    if (Guid.TryParse(filterContext.RequestContext.HttpContext.Request.Params[key], out guid))
                        return guid;
                    return null;
                }
            }

            return null;
        }

        private long? RecuperarSeqConfiguracaoEtapaRequest(ActionExecutingContext filterContext)
        {
            foreach (var key in filterContext.RequestContext.HttpContext.Request.Params.AllKeys)
            {
                if (key.ToLower() == "seqconfiguracaoetapa")
                {
                    long ret = 0;
                    if (long.TryParse(filterContext.RequestContext.HttpContext.Request.Params[key], out ret))
                        return ret;
                    else
                    {
                        try
                        {
                            return new SMCEncryptedLong(filterContext.RequestContext.HttpContext.Request.Params[key]).Value;
                        }
                        catch (Exception)
                        {
                            return null;
                        }
                    }
                }
            }

            return null;
        }

        private long? RecuperarOrigemEtapaRequest(ActionExecutingContext filterContext)
        {
            foreach (var key in filterContext.RequestContext.HttpContext.Request.Params.AllKeys)
            {
                if (key.ToLower() == "origem")
                {
                    long ret = 0;
                    if (long.TryParse(filterContext.RequestContext.HttpContext.Request.Params[key], out ret))
                        return ret;
                    else
                    {
                        try
                        {
                            return new SMCEncryptedLong(filterContext.RequestContext.HttpContext.Request.Params[key]).Value;
                        }
                        catch (Exception)
                        {
                            return null;
                        }
                    }
                }
            }

            return null;
        }
    }
}