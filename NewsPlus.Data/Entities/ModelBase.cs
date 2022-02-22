using NewsPlus.Data.Entities.Common;

namespace NewsPlus.Data.Entities
{
    public class ModelBase : IDateTracking
    {
        public Guid Id { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
        public int? Status { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? DeletedBy { get; set; }
        public virtual SysAppUser? SysAppUser { get; set; }
    }
}
