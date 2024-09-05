namespace FinalProject.Models
{
    public class Comment
    {
        public int Id { get; set; } // Primary key
        public int MovieId { get; set; } // Foreign key to the Movie entity
        public string UserId { get; set; } // Foreign key to the User (Identity)
        public string UserName { get; set; } // Store the username for display purposes
        public string Text { get; set; } // The content of the comment
    }
}
