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

namespace ballen9_PROG1621_Lab_3
{
    public sealed partial class MainPage : Page
    {
        // Global properties
        List<VideoGame> games = new List<VideoGame>();

        BinarySearchTree tree;

        public MainPage()
        {
            this.InitializeComponent();

            // Set the default window size and the minimum window size for the program
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(700, 500));

            ApplicationView.PreferredLaunchViewSize = new Size(700, 500);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            // Add a method to the event of the application closing
            Application.Current.Suspending += OnSuspending;

            // Load in the saved list of games
            ReadInList();

            // Sort the list of games and create a binary tree out of the sorted list
            if (games.Count > 0) games.Sort();

            tree = new BinarySearchTree(games);

            // Display the Binar Search Tree in a string
            txtGameList.Text = tree.AllTheGames();

            // Set the values and displays of the ComboBox based on the Platform Enum
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

        #region Read and Save List
        /// <summary>
        /// This method is called when the program starts and will load in an existing
        /// list of games and add them to a Binary Search Tree for use in the application
        /// </summary>
        private async void ReadInList()
        {
            games = await FileReader.ReadGameList();
            tree = new BinarySearchTree(games);
            txtGameList.Text = tree.AllTheGames();
        }

        /// <summary>
        /// This method is called when the application is closed and will save the updated list of games to the local file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            await FileReader.SaveGameList(games);
        }
        #endregion

        #region Button Methods
        /// <summary>
        /// This method will search the BST for any game that matches the exact information given.
        /// The user MUST specify at least two parameters for the search
        /// Display the result of the search in a message dialogue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            if (dtpReleaseDate.Date != null && dtpReleaseDate.Date <= DateTimeOffset.Now.AddMonths(1))
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

        /// <summary>
        /// This method activates after the add button is pressed.
        /// It will check to see if all the relevant information is valid and filled out.
        /// If the relevant information is not filled out then the user will receive a message dialogue,
        /// informing them to completely fill out the information.
        /// If the information is filled out it will attempt to add a new game to the BST as long as it does not exist
        /// already in the BST. After which the list of games will be updated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

                bool exists = false;

                foreach (VideoGame game in games)
                {
                    if (newGame.Equals(game))
                    {
                        exists = true;
                        MessageDialog msg = new MessageDialog($"This Game:\n{newGame.Title}\nalready exists in your list.");
                        msg.ShowAsync();
                    }
                }
                if (!exists)
                {
                    games.Add(newGame);
                    games.Sort();
                    tree = new BinarySearchTree(games);
                    txtGameList.Text = tree.AllTheGames();
                    ClearFields();
                }
            }
            else
            {
                MessageDialog msg = new MessageDialog("Please fill out all information for the game");
                msg.ShowAsync();
            }
        }

        /// <summary>
        /// Clear the input fields and reset them to their default value
        /// when the button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
        }
        #endregion

        #region Custom Methods
        /// <summary>
        /// Reset the input fields on the application to their default values
        /// </summary>
        private void ClearFields()
        {
            txtTitle.Text = string.Empty;
            txtGenre.Text = string.Empty;
            txtDeveloper.Text = string.Empty;
            cmbPlatform.SelectedIndex = -1;
            dtpReleaseDate.Date = null;
            txtTitle.Focus(FocusState.Programmatic);
        }
        #endregion
    }
}
