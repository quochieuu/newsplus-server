namespace NewsPlus.Data.ViewModel
{
    public class CreateNewsViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string UrlImage { get; set; }
        public string Content { get; set; }
        public Guid CategoryId { get; set; }
        public int Status { get; set; }
    }
}
