using System.Collections.Generic;

namespace MovieCore.Entities
{
    public class Actor
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int BirthYear { get; set; }
        public ICollection<MovieActor> MovieActors { get; set; } = new List<MovieActor>();
    }
}