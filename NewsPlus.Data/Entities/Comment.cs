namespace NewsPlus.Data.Entities
{
    public class Comment : ModelBase
    {
        public Guid NewsId { get; set; }
        public string Content { get; set; }
        public int? Locked { get; set; }
    }
}
