using GoodBooks.Models;
using GoodBooks.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace GoodBooks.Pages
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class Notes : GoodBooks.Common.LayoutAwarePage
    {
        private string text;
        public Notes()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Required;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter != null)
            {
                text = e.Parameter.ToString();
            }

            else
            {
                text = null;
            }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            if (pageState != null)
            {
                if (pageState.ContainsKey("title"))
                {
                    this.TitleTextBox.Text = (string)pageState["title"];
                }

                if (pageState.ContainsKey("content"))
                {
                    this.ContentTextBox.Text = (string)pageState["content"];
                }
                var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                if (localSettings.Values.ContainsKey("notesBackup"))
                {
                    var json = localSettings.Values["notesBackup"].ToString();
                    var result = JsonConvert.DeserializeObject<IList<MyNotes>>(localSettings.Values["notesBackup"].ToString());
                    var dataContext = this.pageRoot.DataContext as NotesViewModel;
                    dataContext.LoadMyNotes(result);
                }
            }
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
            pageState["title"] = this.TitleTextBox.Text;
            pageState["content"] = this.ContentTextBox.Text;
            var dataContext = this.pageRoot.DataContext as NotesViewModel;
            var notesOriginal = dataContext.MyNotes;
            var json = JsonConvert.SerializeObject(notesOriginal);
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["notesBackup"] = json;
            if (json.Length < 90000)
            {
                var roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
                roamingSettings.Values["notesBackup"] = json;
            }
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as MyNotes;
            this.TitleTextBox.Text = item.Title;
            this.ContentTextBox.Text = item.Content;
        }

        private void PageRoot_Loaded(object sender, RoutedEventArgs e)
        {
            if (text != null)
            {
                this.TitleTextBox.Text = "[" + text + "]";
            }

            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            var roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;

            if (localSettings.Values.ContainsKey("notesBackup"))
            {
                var json = localSettings.Values["notesBackup"].ToString();
                var result = JsonConvert.DeserializeObject<IList<MyNotes>>(json);
                var dataContext = this.pageRoot.DataContext as NotesViewModel;
                dataContext.LoadMyNotes(result);
            }

            else if (localSettings.Values.ContainsKey("notesBackup"))
            {
                var json = roamingSettings.Values["notesBackup"].ToString();
                var result = JsonConvert.DeserializeObject<IList<MyNotes>>(json);
                var dataContext = this.pageRoot.DataContext as NotesViewModel;
                dataContext.LoadMyNotes(result);
            }
        }

        private async void SaveAsTextButton_Click(object sender, RoutedEventArgs e)
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            var json = localSettings.Values["notesBackup"].ToString();
            var notes = JsonConvert.DeserializeObject<IList<MyNotes>>(json);

            var sb = new StringBuilder();
            sb.AppendLine("Notes:");

            foreach (var item in notes)
            {
                sb.AppendLine("Title: " + item.Title);
                sb.AppendLine("Content: " + item.Content);
                sb.AppendLine("DateCreated: " + item.DateCreated);
                sb.AppendLine();
                sb.AppendLine();
            }

            var savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.Downloads;

            var plainTextFileTypes = new List<string>(new string[] { ".txt" });

            savePicker.FileTypeChoices.Add(
               new KeyValuePair<string, IList<string>>("Plain Text", plainTextFileTypes)
               );

            var saveFile = await savePicker.PickSaveFileAsync();

            if (saveFile != null)
            {
                await Windows.Storage.FileIO.WriteTextAsync(saveFile, sb.ToString());
                await new Windows.UI.Popups.MessageDialog("File Saved!").ShowAsync();
            }
        }

        private async void SaveAsTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            var text = localSettings.Values["notesBackup"].ToString();

            var savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.Downloads;

            var templateTextFileTypes = new List<string>(new string[] { ".json" });

            savePicker.FileTypeChoices.Add(
               new KeyValuePair<string, IList<string>>("Template Text", templateTextFileTypes)
               );

            var saveFile = await savePicker.PickSaveFileAsync();

            if (saveFile != null)
            {
                await Windows.Storage.FileIO.WriteTextAsync(saveFile, text);
                await new Windows.UI.Popups.MessageDialog("File Saved!").ShowAsync();
            }
        }

        private async void LoadTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();
            openPicker.SuggestedStartLocation = PickerLocationId.Downloads;

            openPicker.FileTypeFilter.Add(".json");

            var file = await openPicker.PickSingleFileAsync();
            var json = string.Empty;
            if (file != null)
            {
                try
                {
                    var text = await Windows.Storage.FileIO.ReadTextAsync(file);
                    json = text;
                    var result = JsonConvert.DeserializeObject<IList<MyNotes>>(json);
                    var dataContext = this.pageRoot.DataContext as NotesViewModel;
                    dataContext.LoadMyNotes(result);
                }
                catch (Exception)
                {
                    new Windows.UI.Popups.MessageDialog("File could not be loaded! Please check if it is the correct template file and try again.").ShowAsync();
                }
            }
        }

        private void PrintNotes_Click(object sender, RoutedEventArgs e)
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            var json = localSettings.Values["notesBackup"].ToString();
            var notes = JsonConvert.DeserializeObject<IList<MyNotes>>(json);

            var sb = new StringBuilder();
            sb.AppendLine("Notes:");
            sb.AppendLine();

            foreach (var item in notes)
            {
                sb.AppendLine("Title: " + item.Title);
                sb.AppendLine("Content: " + item.Content);
                sb.AppendLine("DateCreated: " + item.DateCreated);
                sb.AppendLine();
            }

            var text = sb.ToString();

            this.Frame.Navigate(typeof(MainPage), text);
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Home));
        }
    }
}
