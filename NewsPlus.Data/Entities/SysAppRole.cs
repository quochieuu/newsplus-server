namespace NewsPlus.Data.Entities
{
    public class SysAppRole : ModelBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual IEnumerable<SysAppUser> SysAppUser { get; set; }
    }
}
