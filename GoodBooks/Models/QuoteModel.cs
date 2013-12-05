using Windows.Data.Xml.Dom;

namespace GoodBooks.Models
{
    public class QuoteModel
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public static QuoteModel ParseQuote(string text)
        {
            var quote = new QuoteModel();

            XmlDocument quotesXml = new XmlDocument();
            quotesXml.LoadXml(text);

            var collection = quotesXml.GetElementsByTagName("item");

            foreach (var item in collection)
            {
                foreach (var quoteProperties in item.ChildNodes)
                {
                    switch (quoteProperties.NodeName)
                    {
                        case "title": quote.Title = quoteProperties.InnerText; break;
                        case "description": quote.Description = quoteProperties.InnerText; break;
                    }
                }
                break;
            }

            return quote;
        }
    }
}
