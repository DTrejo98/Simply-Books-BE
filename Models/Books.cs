using System;
using System.ComponentModel.DataAnnotations;

namespace SimplyBooks.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public bool Sale { get; set; }
        public string Uid { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }
}


