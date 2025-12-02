using SMC.Inscricoes.Common.Enums;
using SMC.Inscricoes.Rest.Consts;
using SMC.Inscricoes.Rest.Data;
using SMC.Inscricoes.Rest.Invoke;
using SMC.Inscricoes.Rest.Models;
using SMC.Inscricoes.Rest.Models.File;
using System;
using System.Collections.Generic;

namespace SMC.Inscricoes.Rest.Helper
{
    public static class SautinsoftHelper
    {
        public static List<string> FindFieldsMerge(byte[] arquivo)
        {
            var document = new DocumentFile()
            {
                Type = "dotx",
                Name = "arquivo",
                FileData = arquivo
            };

            var result = SauntinsoftResquest.Send<Result<List<string>>>(document, MetodoHttp.POST, SautinsoftEndPoint.FindFieldsMerge);

            if (result.success)
            {
                return result.data;
            }
            else
            {
                throw new Exception(result.errorMessage);
            }
        }

        public static byte[] MailMergeToPdf(byte[] file, string nameFile, string typeFile, string tags)
        {
            var documento = new MailMergeDocumentFileData()
            {
                File = new DocumentFileData()
                {
                    FileData = file,
                    Name = nameFile,
                    Type = typeFile,
                },
                Fields = tags
            };

            //string requisicaoJson = JsonConvert.SerializeObject(documento, Newtonsoft.Json.Formatting.Indented);

            var result = SauntinsoftResquest.Send<Result<byte[]>>(documento, MetodoHttp.POST, SautinsoftEndPoint.MailMergeDocument);

            if (!result.success)
            {
                throw new Exception(result.errorMessage);
            }

            return result.data;
        }
    }
}
