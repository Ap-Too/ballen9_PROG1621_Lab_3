using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Appointments.AppointmentsProvider;
using Windows.Storage;
using Windows.UI.Popups;

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

        public static async Task ReadGameList()
        {
            List<VideoGame> temp;

            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;

            try
            {
                StorageFile storageFile = await storageFolder.GetFileAsync("GameList.txt");
                
                // Use ReadAllLine() to get a string array split on new lines

                // Read each string in the array
                // split it by a delimiter
                // Input seperate strings into the proper area of the new Video Game object
                // Add each new video game to the temp list

                // assign the temp list of games to the static list in the class
            }
            catch (Exception ex)
            {
                MessageDialog msg = new MessageDialog(ex.Message);
                msg.ShowAsync();
            }
        }
    }
}
