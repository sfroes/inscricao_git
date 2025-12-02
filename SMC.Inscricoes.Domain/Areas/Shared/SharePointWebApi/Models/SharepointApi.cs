using SMC.Framework;
using SMC.Framework.Audit;
using SMC.Framework.Mapper;
using System;

namespace SMC.Inscricoes.Domain.Areas.Shared.SharePointWebApi.Models
{
    public class SharepointApi : ISMCSeq, ISMCAuditData, ISMCMappable
    {
        public DateTime DataInclusao { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string UsuarioInclusao { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime? DataAlteracao { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string UsuarioAlteracao { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public long Seq { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
