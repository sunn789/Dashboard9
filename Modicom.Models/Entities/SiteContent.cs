using System.ComponentModel.DataAnnotations;

namespace Modicom.Models.Entities;

public class SiteContent
{
     public int Id { get; set; }
    [Display(Name = "Title")]
        public string? Title { get; set; }

        [Display(Name = "Image URL")]
        public string? ImageUrl { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        [Display(Name = "Priority")]
        public int Priority { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime ModifyDate { get; set; }

        [Display(Name = "Active")]
        public bool Active { get; set; }

        [Display(Name = "Related Category")]
        public int? SiteContentCategoryId { get; set; }

        [Display(Name = "Related Category")]
        public SiteContentCategory? SiteContentCategory { get; set; }
}