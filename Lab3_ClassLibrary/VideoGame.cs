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
        private Platform platform;

        // Properties
        public string Title { get { return title; } set { title = value; } }
        public string Genre { get { return genre; } set { genre = value; } }
        public string Developer { get { return developer; } set { developer = value; } }
        public DateTime ReleaseDate { get { return releaseDate; } set { releaseDate = value; } }
        public Platform Platform { get { return platform; } set { platform = value; } }

        // Constructor
        public VideoGame(string title, string genre, string developer, DateTime releaseDate, Platform platform)
        {
            Title = title;
            Genre = genre;
            Developer = developer;
            ReleaseDate = releaseDate;
            Platform = platform;
        }


        public override string ToString() => 
            $"{Title}" +
            $"\n\tDeveloper: {Developer}" +
            $"\n\tPlatform: {Platform}" +
            $"\n\tRelease Date: {ReleaseDate.ToShortDateString()}" +
            $"\n\tGenre: {Genre}";
        
        public int CompareTo(VideoGame other)
        {
            if (this.Platform.CompareTo(other.Platform) < 0) return -1;
            else if (this.Platform == other.Platform)
            {
                if (this.Title.CompareTo(other.Title) < 0) return -1;
                else if (this.Title == other.Title)
                {
                    if (this.Developer.CompareTo(other.Developer) < 0) return -1;
                    else if (this.Developer == other.Developer)
                    {
                        return this.ReleaseDate.CompareTo(other.ReleaseDate);
                    }
                    else return 1;
                }
                else return 1;
            }
            else return 1;
        }

        public bool Equals(VideoGame other)
        {
            return (this.Title == other.Title && 
                this.Developer == other.Developer && 
                this.ReleaseDate == other.ReleaseDate && 
                this.Platform == other.Platform);
        }

        public static bool operator == (VideoGame vg1, VideoGame vg2)
        {
            // Evaluate does vg1 == vg2
            if (object.ReferenceEquals(vg1, null))
                return object.ReferenceEquals(vg2, null);
            if (object.ReferenceEquals(vg2, null))
                return false;

            return vg1.Title == vg2.Title && vg1.Developer == vg2.Developer &&
                vg1.Platform == vg2.Platform && vg1.ReleaseDate == vg2.ReleaseDate;
        }

        public static bool operator != (VideoGame vg1, VideoGame vg2)
        {
            // Evaluate does vg1 != vg2
            if (object.ReferenceEquals(vg1,null))
                return !object.ReferenceEquals(vg2,null);
            if (object.ReferenceEquals(vg2, null))
                return true;

            return vg1.Title != vg2.Title || vg1.Developer != vg2.Developer ||
                vg1.Platform != vg2.Platform || vg1.ReleaseDate != vg2.ReleaseDate;
        }
    }
}
