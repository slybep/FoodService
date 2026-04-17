namespace AuthorizationAPI.Abstractions
{
    public interface IDocument
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
