using System.Collections.Generic;

namespace MovieCore.Entities
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<Movie> Movies { get; set; } = new List<Movie>();
    }
}