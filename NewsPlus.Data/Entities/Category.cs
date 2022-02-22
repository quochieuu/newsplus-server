namespace NewsPlus.Data.Entities
{
    public class Category : ModelBase
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public string? Description { get; set; }
        public int SortOrder { get; set; }
        public Guid? ParentId { get; set; }
        public virtual IEnumerable<News> News { get; set; }
    }
}
