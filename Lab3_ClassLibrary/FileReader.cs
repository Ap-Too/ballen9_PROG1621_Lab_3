using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

namespace Lab3_ClassLibrary
{
    public class FileReader
    {
        public static List<VideoGame> Games {  get; set; } = new List<VideoGame>();

        public static async Task SaveGameList(List<VideoGame> games)
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;

            StorageFile storageFile =
                await storageFolder.CreateFileAsync("GameList.txt", CreationCollisionOption.ReplaceExisting);

            string output = "";

            foreach (VideoGame game in games)
            {
                output += $"{game.Title}|{game.Genre}|{game.Developer}|{game.ReleaseDate.ToString()}|{game.Platform}\n";
            }
            await FileIO.WriteTextAsync(storageFile, output);
        }

        public static async Task<List<VideoGame>> ReadGameList()
        {
            List<VideoGame> temp = new List<VideoGame>();

            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;

            try
            {
                StorageFile storageFile = await storageFolder.GetFileAsync("GameList.txt");
                
                var lines = await FileIO.ReadLinesAsync(storageFile);

                foreach (var line in lines)
                {
                    var parts = line.Split('|');
                    if (parts.Length != 5) continue;

                    string title = parts[0];
                    string genre = parts[1];
                    string devel = parts[2];
                    DateTime release = DateTime.Parse(parts[3]);
                    Platform plat = Enum.Parse<Platform>(parts[4]);

                    temp.Add(new VideoGame(title, genre, devel, release, plat));
                }

                return temp;
            }
            catch (Exception ex)
            {
                return new List<VideoGame>();
            }
        }
    }
}
