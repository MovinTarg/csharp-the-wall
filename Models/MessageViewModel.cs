using System.ComponentModel.DataAnnotations;

namespace the_wall.Models
{
    public class MessageViewModel : BaseEntity
    {
        [MinLength (5)]
        [Display(Name = "Post a Message!")]
        public string messageText { get; set; }
        [MinLength (5)]
        [Display(Name = "Post a comment!")]
        public string commentText { get; set; }
    }
}