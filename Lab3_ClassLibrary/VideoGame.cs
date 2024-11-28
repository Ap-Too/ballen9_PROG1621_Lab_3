using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3_ClassLibrary
{
    public class VideoGame : IComparable<VideoGame>, IEquatable<VideoGame>
    {
        // Fields
        private string title;
        private string genre;
        private string developer;
        private DateTime releaseDate;
        private string platform;

        // Properties
        public string Title { get { return title; } set { title = value; } }
        public string Genre { get { return genre; } set { genre = value; } }
        public string Developer { get { return developer; } set { developer = value; } }
        public DateTime ReleaseDate { get { return releaseDate; } set { releaseDate = value; } }
        public string Platform { get { return platform; } set { platform = value; } }

        // Constructor
        public VideoGame(string title, string genre, string developer, DateTime releaseDate, string platform)
        {
            Title = title;
            Genre = genre;
            Developer = developer;
            ReleaseDate = releaseDate;
            Platform = platform;
        }


        public override string ToString() => 
            $"{Title}\nGenre: {Genre}\nDeveloper: {Developer}\nRelease Date: {ReleaseDate.ToShortDateString()}\nPlatform: {Platform}";
        
        public int CompareTo(VideoGame other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(VideoGame other)
        {
            throw new NotImplementedException();
        }
    }
}
