using System;
using System.Collections.Generic;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace GoodBooks.Pages
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class EventsPage : GoodBooks.Common.LayoutAwarePage
    {
        private double longtitute;
        private double latitude;
        private double accuracy;

        public EventsPage()
        {
            this.InitializeComponent();
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

        private async void GetGeoLocation_Click(object sender, RoutedEventArgs e)
        {
            var geo = new Geolocator();

            try
            {
                Geoposition pos = await geo.GetGeopositionAsync();

                this.latitude = pos.Coordinate.Latitude;
                this.longtitute = pos.Coordinate.Longitude;
                this.accuracy = pos.Coordinate.Accuracy;
                this.LatituteTextBox.Text = "Latitude: " + this.latitude.ToString();
                this.LongtitudeTextBox.Text = "Longitude: " + this.longtitute.ToString();
                this.AccuracyTextBox.Text = "Accuracy: " + this.accuracy.ToString();
            }

            catch (Exception)
            {
                //Windows notifier is throwing access exceptions...
                //Better leave with no responce, but crash the app.
            }
        }

        private void CheckForEvents_Click(object sender, RoutedEventArgs e)
        {
            //Since goodreads does not update their event api controller, there is no sence for making request. If they fix this, it will be very easier to add this functionality as well. 
            // For more info about it visit: http://www.goodreads.com/api
            if (this.latitude != 0 && this.longtitute != 0 && this.accuracy != 0)
            {
                var link = "http://www.goodreads.com/event";
                this.EventsTextBlock.Text = "No events found!";
                this.ResultTextBlock.Text = "Unfortunately there aren't any events in this area. For more acurate information please visit the source website below:";
                this.HyperlinkButton.Content = link;
                this.HyperlinkButton.NavigateUri = new System.Uri(link);
            }
        }
    }
}
