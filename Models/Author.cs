using System.ComponentModel.DataAnnotations;

namespace SimplyBooks.Models
{
    public class Author
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Email { get; set; }

        public bool Favorite { get; set; }

        public string Image { get; set; }
        [Required]
        public string Uid { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}
