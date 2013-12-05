using GoodBooks.Models;
using GoodBooks.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.ApplicationModel.DataTransfer;
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
    public sealed partial class Author : GoodBooks.Common.LayoutAwarePage
    {
        private string name;
        DataTransferManager dtm;

        public Author()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter != null)
            {
                this.name = e.Parameter.ToString();
            }
            base.OnNavigatedTo(e);
            dtm = DataTransferManager.GetForCurrentView();
            dtm.DataRequested += dtm_DataRequested;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            dtm.DataRequested -= dtm_DataRequested;
        }

        void dtm_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            var datacontext = this.pageRoot.DataContext as AuthorViewModel;
            var author = datacontext.Author;
            Uri linkSource = new Uri(author.Link);
            string linkTitle = "Great author I have found in Goodbooks.";


            DataPackage data = args.Request.Data;
            data.Properties.Title = linkTitle;
            data.SetUri(linkSource);
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
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        private void pageRoot_Loaded(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.name))
            {
                var dataContext = this.pageRoot.DataContext as AuthorViewModel;
                dataContext.GetAuthorByName(this.name);
            }
        }

        private void MakeNoteButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var author = button.DataContext as AuthorViewModel;
            this.Frame.Navigate(typeof(Notes), author.Author.Name);
        }

        private async void SaveAuthorToFile_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var author = button.DataContext as AuthorViewModel;
            if (author.Author.Name != null && author.Author.About != null)
            {
                var text = GetAuthorInfo(author);

                var savePicker = new Windows.Storage.Pickers.FileSavePicker();
                savePicker.SuggestedStartLocation = PickerLocationId.Downloads;
                var plainTextFileTypes = new List<string>(new string[] { ".txt" });

                savePicker.FileTypeChoices.Add(
                   new KeyValuePair<string, IList<string>>("Plain Text", plainTextFileTypes));

                var saveFile = await savePicker.PickSaveFileAsync();

                if (saveFile != null)
                {
                    await Windows.Storage.FileIO.WriteTextAsync(saveFile, text);
                    await new Windows.UI.Popups.MessageDialog("File Saved!").ShowAsync();
                }
            }
        }

        private string GetAuthorInfo(AuthorViewModel author)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Author:");
            sb.AppendLine("Name: " + author.Author.Name);
            sb.AppendLine("Born at: " + author.Author.BornAt);
            sb.AppendLine("Died at: " + author.Author.DiedAt);
            sb.AppendLine("Works count: " + author.Author.WorksCount);
            sb.AppendLine("Hometown: " + author.Author.Hometown);
            sb.AppendLine("Fans Count: " + author.Author.FansCount);
            sb.AppendLine("Gender: " + author.Author.Gender);
            sb.AppendLine("About: " + author.Author.About);
            sb.AppendLine();
            sb.AppendLine("Books:");
            sb.AppendLine();
            foreach (var book in author.Author.Books)
            {
                sb.AppendLine("Title: " + book.Title);
                sb.AppendLine("Description: " + book.Description);
                sb.AppendLine("Publication Year: " + book.PublishedYear);
                sb.AppendLine("Publisher: " + book.Publisher);
                sb.AppendLine("Number of pages: " + book.NumberOfPages);
                sb.AppendLine();
            }

            sb.AppendLine("More info: " + author.Author.Link);

            return sb.ToString();
        }

        private void PrintInformation_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var dataContext = button.DataContext as AuthorViewModel;
            var author = dataContext.Author;

            if (author.Name != null && author.About != null)
            {
                var text = GetAuthorInfo(dataContext);

                this.Frame.Navigate(typeof(MainPage), text);
            }
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var itemClicked = e.ClickedItem as BookPartialModel;
            this.Frame.Navigate(typeof(Book), itemClicked);
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Home));
        }
    }
}
