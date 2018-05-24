using System;

namespace the_wall.Models
{
    public class Comment : BaseEntity
    {
        public int message_id { get; set; }
        public int user_id { get; set; }
        public string commentText { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }
}