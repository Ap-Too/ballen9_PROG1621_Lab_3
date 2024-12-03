using Lab3_ClassLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Lab3_ClassLibrary;
using Windows.UI.ViewManagement;
using Windows.ApplicationModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ballen9_PROG1621_Lab_3
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        List<VideoGame> games = new List<VideoGame>();

        public MainPage()
        {
            this.InitializeComponent();

            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(700, 500));

            ApplicationView.PreferredLaunchViewSize = new Size(700, 500);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            Application.Current.Suspending += OnSuspending;

            // Load in the saved list of games
            ReadInList(games);

            // Sort the list of games and create a binary tree out of the sorted list
            if (games.Count > 0) games.Sort();

            BinarySearchTree tree = new BinarySearchTree(games);

            txtGameList.Text = tree.AllTheGames();
        }

        private async void ReadInList(List<VideoGame> games)
        {
            games = await FileReader.ReadGameList();
        }

        // Add in a method for the user to add in a new object to the list.
        // Vaildate the information given by the user
        // Add the new object if there are no duplicates in the list
        // Use the display method to show the updated list after the new object is created


        // Create a control to search for a specific object.
        // The user MUST specify at least two parameters for the search
        // Validate both values before searching the Binary Tree
        // The search is looking for EXACT MATCHES
        // Display the result of the search in a message dialogue


        // Any changes to the object collection should be saved to the local text file when the program closes
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            await FileReader.SaveGameList(games);
        }
    }
}
