namespace NewsPlus.Data.ViewModel
{
    public class CreateCategoryViewModel
    {
        public string Name { get; set; }
        public Guid? ParentId { get; set; }
        public int Status { get; set; }
    }
}
