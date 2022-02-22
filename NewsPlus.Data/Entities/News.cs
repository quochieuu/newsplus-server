namespace NewsPlus.Data.Entities
{
    public class News : ModelBase
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public string UrlImage { get; set; }
        public string Content { get; set; }
        public int ViewCount { get; set; }
        public int? Locked { get; set; }
        public Guid CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
