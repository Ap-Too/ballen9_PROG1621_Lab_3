using Lab3_ClassLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ballen9_PROG1621_Lab_3
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        List<VideoGame> games = new List<VideoGame>();

        BinarySearchTree tree;

        public MainPage()
        {
            this.InitializeComponent();

            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(700, 500));

            ApplicationView.PreferredLaunchViewSize = new Size(700, 500);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            Application.Current.Suspending += OnSuspending;

            // Load in the saved list of games
            ReadInList();

            // Sort the list of games and create a binary tree out of the sorted list
            if (games.Count > 0) games.Sort();

            tree = new BinarySearchTree(games);

            txtGameList.Text = tree.AllTheGames();

            cmbPlatform.ItemsSource = Enum.GetValues(typeof(Platform))
                .Cast<Platform>()
                .Select(e => new
                {
                    Value = e,
                    Description = e.GetType()
                        .GetField(e.ToString())
                        .GetCustomAttributes(typeof(DescriptionAttribute), false)
                        .FirstOrDefault() is DescriptionAttribute descriptionAttribute
                        ? descriptionAttribute.Description : e.ToString()
                })
                .ToList();

            cmbPlatform.DisplayMemberPath = "Description";
            cmbPlatform.SelectedValuePath = "Value";
            cmbPlatform.SelectedIndex = -1;
        }

        private async void ReadInList()
        {
            games = await FileReader.ReadGameList();
            tree = new BinarySearchTree(games);
            txtGameList.Text = tree.AllTheGames();
        }

        // Any changes to the object collection should be saved to the local text file when the program closes
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            await FileReader.SaveGameList(games);
        }

        // Create a control to search for a specific object.
        // The user MUST specify at least two parameters for the search
        // Validate both values before searching the Binary Tree
        // The search is looking for EXACT MATCHES
        // Display the result of the search in a message dialogue
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            VideoGame temp;
            // Check if at least two boxes are filled out correctly
            int fieldCount = 0;

            if (!string.IsNullOrWhiteSpace(txtTitle.Text))
                fieldCount++;
            if (!string.IsNullOrWhiteSpace(txtGenre.Text))
                fieldCount++;
            if (!string.IsNullOrWhiteSpace(txtDeveloper.Text))
                fieldCount++;
            if (dtpReleaseDate.Date != null)
                fieldCount++;
            if (cmbPlatform.SelectedIndex != -1)
                fieldCount++;

            // Search for the exact match if the field count is >= 2
            if (fieldCount >= 2)
            {
                if (dtpReleaseDate.Date == null && cmbPlatform.SelectedIndex == -1)
                    temp = tree.Search(txtTitle.Text.Trim(), txtDeveloper.Text.Trim(), txtGenre.Text.Trim());
                else if (dtpReleaseDate.Date == null && cmbPlatform.SelectedIndex > -1)
                    temp = tree.Search(txtTitle.Text.Trim(), txtDeveloper.Text.Trim(), txtGenre.Text.Trim(), (Platform)cmbPlatform.SelectedIndex);
                else if (dtpReleaseDate.Date != null && cmbPlatform.SelectedIndex == -1)
                    temp = tree.Search(txtTitle.Text.Trim(), txtDeveloper.Text.Trim(), txtGenre.Text.Trim(), null, dtpReleaseDate.Date.Value.DateTime);
                else
                    temp = tree.Search(txtTitle.Text.Trim(), txtDeveloper.Text.Trim(), txtGenre.Text.Trim(), (Platform)cmbPlatform.SelectedValue, dtpReleaseDate.Date.Value.DateTime);
                
                if (temp != null)
                {
                    MessageDialog msg = new MessageDialog($"There is a match in your list.\n\n{temp.ToString()}");
                    msg.ShowAsync();
                }
                else
                {
                    MessageDialog msg = new MessageDialog($"There was no match for your search");
                    msg.ShowAsync();
                }
            }
            else
            {
                MessageDialog msg = new MessageDialog("Please fill out at least 2 fields to search for.");
                msg.ShowAsync();
            }
        }

        // Add in a method for the user to add in a new object to the list.
        // Vaildate the information given by the user
        // Add the new object if there are no duplicates in the list
        // Use the display method to show the updated list after the new object is created
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            // Check if all boxes are filled out correctly
            bool filled = true;

            if (string.IsNullOrWhiteSpace(txtTitle.Text))
                filled = false;
            if (string.IsNullOrWhiteSpace(txtGenre.Text))
                filled = false;
            if (string.IsNullOrWhiteSpace(txtDeveloper.Text))
                filled = false;
            if (dtpReleaseDate.Date == null && dtpReleaseDate.Date.Value.Month <= new DateTimeOffset().Month + 1)
                filled = false;
            if (cmbPlatform.SelectedIndex == -1)
                filled = false;

            // If all are filled create a new VideoGame object otherwise ask the user to fill out all the info 
            if (filled)
            {
                VideoGame newGame = new VideoGame(txtTitle.Text.Trim(), txtGenre.Text.Trim(), txtDeveloper.Text.Trim(), dtpReleaseDate.Date.Value.DateTime, (Platform)cmbPlatform.SelectedValue);


                foreach (VideoGame game in games)
                {
                    if (game ==  newGame)
                    {
                        MessageDialog msg = new MessageDialog($"This Game:\n{newGame.Title}\nalready exists in your list.");
                        msg.ShowAsync();
                    }
                    else
                    {
                        games.Add(newGame);
                        games.Sort();
                        tree = new BinarySearchTree(games);
                        txtGameList.Text = tree.AllTheGames();
                        ClearFields();
                    }
                }
            }
            else
            {
                MessageDialog msg = new MessageDialog("Please fill out all information for the game");
                msg.ShowAsync();
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            txtTitle.Text = string.Empty;
            txtGenre.Text = string.Empty;
            txtDeveloper.Text = string.Empty;
            cmbPlatform.SelectedIndex = -1;
            dtpReleaseDate.Date = null;
        }
    }
}
