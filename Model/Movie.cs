using System;

namespace Movie_Hub.Model
{
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public int MovieRatings { get; set; }
        public DateTime ReleaseDate { get; set; }
        public TimeSpan Duration { get; set; }

        public DateTime DateAdded { get; set; }
    }
}
