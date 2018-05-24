using System;

namespace the_wall.Models
{
    public class Message : BaseEntity
    {
        public int user_id { get; set; }
        public string messageText { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }
}