using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Models
{
    public class Movie
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string ThumbnailName { get; set; }
        public string Director { get; set; }
        public string Starring { get; set; }

        public string Production { get; set; }
        public string Distribution { get; set; }

        public DateTime ReleaseDate { get; set; }
        public int Runtime { get; set; }
        public string Country { get; set; }
        public string Language { get; set; }

        public string Description { get; set; }

        public string UserId { get; set; }

        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
