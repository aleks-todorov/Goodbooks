using GoodBooks.Models;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace GoodBooks.Pages
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class Home : GoodBooks.Common.LayoutAwarePage
    {
        public Home()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Required;
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

        private void SearchAuthorButtonClicked(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Author));
        }

        private void SearchBookButtonClicked(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Book));
        }

        private void MyNotesButtonClicked(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Notes));
        }

        private void CheckForEventsButtonClicked(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(EventsPage));
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var author = e.ClickedItem as AuthorFullModel;
            this.Frame.Navigate(typeof(Author), author.Name);
        }

        private void FaqButtonClicked(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Faq));
        }
    }
}
