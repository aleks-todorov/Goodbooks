using GoodBooks.Models;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

// The Search Contract item template is documented at http://go.microsoft.com/fwlink/?LinkId=234240

namespace GoodBooks.Pages
{
    /// <summary>
    /// This page displays search results when a global search is directed to this application.
    /// </summary>
    public sealed partial class SearchResultsPage : GoodBooks.Common.LayoutAwarePage
    {
        ViewModels.SearchResults currentViewModel = null;

        public SearchResultsPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            this.currentViewModel = new ViewModels.SearchResults();
            this.DataContext = this.currentViewModel;
        }

        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            var queryText = navigationParameter as String;
            this.currentViewModel.QueryText = queryText;
        }

        protected override void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var queryText = e.Parameter as String;
            this.currentViewModel.QueryText = queryText;
        }

        private void ResultsGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var itemClicked = e.ClickedItem as SearchResultBookModel;
            var title = itemClicked.Title;
            var book = new BookPartialModel();
            book.Title = title;
            this.Frame.Navigate(typeof(Book), book);
        }

    }
}
