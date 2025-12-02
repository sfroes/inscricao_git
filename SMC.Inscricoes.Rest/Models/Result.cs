namespace SMC.Inscricoes.Rest.Models
{
    public class Result<T> : ResultBase
    {
        public T data { get; set; }
    }
}
