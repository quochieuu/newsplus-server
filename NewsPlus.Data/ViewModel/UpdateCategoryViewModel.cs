﻿namespace NewsPlus.Data.ViewModel
{
    public class UpdateCategoryViewModel
    {
        public string Name { get; set; }
        public string? Slug { get; set; }
        public Guid? ParentId { get; set; }
        public int Status { get; set; }
    }
}
