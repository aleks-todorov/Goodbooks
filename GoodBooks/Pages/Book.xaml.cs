using GoodBooks.Models;
using GoodBooks.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace GoodBooks.Pages
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class Book : GoodBooks.Common.LayoutAwarePage
    {
        BookPartialModel book;
        DataTransferManager dtm;
        public Book()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter != null && e.Parameter.GetType() == typeof(BookPartialModel))
            {
                book = e.Parameter as BookPartialModel;
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
            var datacontext = this.pageRoot.DataContext as BookViewModel;
            var book = datacontext.Book;
            Uri linkSource = new Uri(book.Link);
            string linkTitle = "Great book I have found in Goodbooks.";


            DataPackage data = args.Request.Data;
            data.Properties.Title = linkTitle;
            data.SetUri(linkSource);
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

        private void SimilarBookButtonClicked(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var book = button.DataContext as BookPartialModel;
            var dataContext = this.pageRoot.DataContext as BookViewModel;
            dataContext.ChangeBook(book.Title);
        }

        private void PageRoot_Loaded(object sender, RoutedEventArgs e)
        {
            var dataContext = this.pageRoot.DataContext as BookViewModel;
            if (book != null)
            {
                dataContext.ChangeBook(book.Title);
            }
        }

        private void ShowAuthorButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var book = button.DataContext as BookViewModel;
            if (book.Book.Authors.Count > 0)
            {
                this.Frame.Navigate(typeof(Author), book.Book.Authors.FirstOrDefault().Name);
            }
        }

        private void AddNoteButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var dataContext = button.DataContext as BookViewModel;
            var bookName = dataContext.Book.Title;
            this.Frame.Navigate(typeof(Notes), bookName);
        }

        private async void SaveBookToFile_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var dataContext = button.DataContext as BookViewModel;
            var book = dataContext.Book;

            if (book.Title != null && book.Description != null)
            {
                var text = GetBookInfo(book);

                var savePicker = new Windows.Storage.Pickers.FileSavePicker();
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

        private static string GetBookInfo(BookPartialModel book)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Book:");
            sb.AppendLine("Title: " + book.Title);
            sb.AppendLine("Author: " + book.Authors.FirstOrDefault().Name);
            sb.AppendLine("Number of Pages: " + book.NumberOfPages);
            sb.AppendLine("Publisher: " + book.Publisher);
            sb.AppendLine("Published Year: " + book.PublishedYear);
            sb.AppendLine("ISBN: " + book.ISBN);
            sb.AppendLine("Rating: " + book.AverageRating);
            sb.AppendLine("Description: " + book.Description);
            sb.AppendLine();
            sb.AppendLine("Similar Books:");
            sb.AppendLine();
            foreach (var item in book.SimilarBooks)
            {
                sb.AppendLine("Title: " + item.Title);
                sb.AppendLine("Author: " + item.Authors.FirstOrDefault().Name);
                sb.AppendLine();
            }

            sb.AppendLine("More information: " + book.Link);
            return sb.ToString();
        }

        private void PrintBookInfo_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var dataContext = button.DataContext as BookViewModel;
            var book = dataContext.Book;

            if (book.Title != null && book.Description != null)
            {
                var text = GetBookInfo(book);

                this.Frame.Navigate(typeof(MainPage), text);
            }
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var bookClicked = e.ClickedItem as BookPartialModel;
            var viewModel = new BookViewModel();
            this.pageRoot.DataContext = viewModel;
            viewModel.ChangeBook(bookClicked.Title);
            this.BookName.Text = string.Empty;
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Home));
        }
    }
}
