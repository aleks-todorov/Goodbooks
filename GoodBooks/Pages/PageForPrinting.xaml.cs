using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace GoodBooks.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PageForPrinting : Page
    {
        public PageForPrinting()
        {
            this.InitializeComponent();
        }

        public PageForPrinting(string text)
            : this()
        {
            var paragraph = new Paragraph();
            paragraph.Inlines.Add(new Run { Text = text });
            this.textContent.Blocks.Add(paragraph);
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}
