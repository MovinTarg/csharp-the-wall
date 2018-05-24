using System.ComponentModel.DataAnnotations;

namespace the_wall.Models
{
    public class CommentViewModel : BaseEntity
    {
        [Required]
        [MinLength (5)]
        [Display(Name = "Post a comment!")]
        public string commentText { get; set; }
    }
}